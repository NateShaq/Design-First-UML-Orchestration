#!/usr/bin/env bash
set -euo pipefail

###############################################################################
# CONFIG
###############################################################################
ROOT_DIR="${1:-$PWD}"
SONAR_HOST_URL="${SONAR_HOST_URL:-http://localhost:9000}"
SONAR_CONTAINER_NAME="${SONAR_CONTAINER_NAME:-sonarqube}"
SONAR_DOCKER_IMAGE="${SONAR_DOCKER_IMAGE:-sonarqube:latest}"
SONAR_PORT="${SONAR_PORT:-9000}"

# Required: export SONAR_TOKEN="your_token_here"
SONAR_TOKEN="${SONAR_TOKEN:-}"

# Optional: set to 1 if you want to scan only folders starting with LCP-
ONLY_LCP_PREFIX="${ONLY_LCP_PREFIX:-1}"

###############################################################################
# HELPERS
###############################################################################
log() {
  printf "\n[%s] %s\n" "$(date '+%Y-%m-%d %H:%M:%S')" "$*"
}

fail() {
  echo "ERROR: $*" >&2
  exit 1
}

require_cmd() {
  command -v "$1" >/dev/null 2>&1 || fail "Missing required command: $1"
}

ensure_docker_desktop_running() {
  if docker info >/dev/null 2>&1; then
    return 0
  fi

  log "Docker is not running. Attempting to start Docker Desktop..."
  open -ga Docker || true

  local waited=0
  local max_wait=180
  until docker info >/dev/null 2>&1; do
    sleep 3
    waited=$((waited + 3))
    if (( waited >= max_wait )); then
      fail "Docker Desktop did not become ready within ${max_wait}s."
    fi
  done

  log "Docker Desktop is running."
}

ensure_sonarqube_container() {
  if docker ps --format '{{.Names}}' | grep -qx "${SONAR_CONTAINER_NAME}"; then
    log "SonarQube container '${SONAR_CONTAINER_NAME}' is already running."
    return 0
  fi

  if docker ps -a --format '{{.Names}}' | grep -qx "${SONAR_CONTAINER_NAME}"; then
    log "Starting existing SonarQube container '${SONAR_CONTAINER_NAME}'..."
    docker start "${SONAR_CONTAINER_NAME}" >/dev/null
    return 0
  fi

  log "Creating SonarQube container '${SONAR_CONTAINER_NAME}'..."
  docker run -d \
    --name "${SONAR_CONTAINER_NAME}" \
    -e SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true \
    -p "${SONAR_PORT}:9000" \
    "${SONAR_DOCKER_IMAGE}" >/dev/null

  log "SonarQube container created."
}

wait_for_sonarqube() {
  log "Waiting for SonarQube at ${SONAR_HOST_URL} ..."
  local waited=0
  local max_wait=420

  while true; do
    local response status
    response="$(curl -s "${SONAR_HOST_URL}/api/system/status" 2>/dev/null || true)"

    status="$(python3 -c '
import sys, json
try:
    data = json.loads(sys.argv[1])
    print(data.get("status", ""))
except Exception:
    print("")
' "$response")"

    case "${status}" in
      UP)
        log "SonarQube is UP."
        return 0
        ;;
      STARTING|DB_MIGRATION_NEEDED|DB_MIGRATION_RUNNING|RESTART_REQUIRED|"")
        ;;
      *)
        log "Current SonarQube status: ${status}"
        ;;
    esac

    sleep 5
    waited=$((waited + 5))
    if (( waited >= max_wait )); then
      fail "SonarQube did not become ready within ${max_wait}s."
    fi
  done
}

ensure_dotnet_scanner() {
  if command -v dotnet-sonarscanner >/dev/null 2>&1; then
    log "dotnet-sonarscanner already installed."
    return 0
  fi

  log "Installing dotnet-sonarscanner ..."
  dotnet tool update --global dotnet-sonarscanner >/dev/null 2>&1 || \
  dotnet tool install --global dotnet-sonarscanner >/dev/null 2>&1

  export PATH="$PATH:$HOME/.dotnet/tools"

  command -v dotnet-sonarscanner >/dev/null 2>&1 || \
    fail "dotnet-sonarscanner installation failed."
}

project_key_from_name() {
  local name="$1"
  echo "$name" | tr '[:upper:]' '[:lower:]' | sed -E 's/[^a-z0-9._:-]+/-/g; s/^-+//; s/-+$//'
}

create_project_if_missing() {
  local project_key="$1"
  local project_name="$2"

  local exists_http
  exists_http="$(curl -s -o /tmp/sonar_proj_check.json -w '%{http_code}' \
    -u "${SONAR_TOKEN}:" \
    "${SONAR_HOST_URL}/api/projects/search?projects=${project_key}")"

  if [[ "${exists_http}" != "200" ]]; then
    fail "Unable to query SonarQube project list for ${project_key}."
  fi

  local found
  found="$(python3 - <<PY
import json
with open('/tmp/sonar_proj_check.json','r') as f:
    data=json.load(f)
components=data.get("components",[])
print("yes" if any(c.get("key")==${project_key@Q} for c in components) else "no")
PY
)"

  if [[ "${found}" == "yes" ]]; then
    log "SonarQube project exists: ${project_key}"
    return 0
  fi

  log "Creating SonarQube project: ${project_key}"
  curl -s -u "${SONAR_TOKEN}:" \
    -X POST "${SONAR_HOST_URL}/api/projects/create" \
    --data-urlencode "project=${project_key}" \
    --data-urlencode "name=${project_name}" >/dev/null
}

find_build_target() {
  local dir="$1"

  local sln
  sln="$(find "$dir" -type f -name '*.sln' \
    ! -path '*/bin/*' \
    ! -path '*/obj/*' \
    | head -n 1 || true)"
  if [[ -n "${sln}" ]]; then
    echo "${sln}"
    return 0
  fi

  local csproj
  csproj="$(find "$dir/src" "$dir" -type f -name '*.csproj' \
    ! -path '*/bin/*' \
    ! -path '*/obj/*' 2>/dev/null | head -n 1 || true)"
  if [[ -n "${csproj}" ]]; then
    echo "${csproj}"
    return 0
  fi

  echo ""
}

wait_for_ce_task() {
  local report_file="$1"
  local project_dir="$2"

  [[ -f "${report_file}" ]] || fail "Missing report-task file: ${report_file}"

  local ce_task_url ce_task_id dashboard_url
  ce_task_url="$(grep '^ceTaskUrl=' "${report_file}" | sed 's/^ceTaskUrl=//')"
  ce_task_id="$(grep '^ceTaskId=' "${report_file}" | sed 's/^ceTaskId=//')"
  dashboard_url="$(grep '^dashboardUrl=' "${report_file}" | sed 's/^dashboardUrl=//')"

  [[ -n "${ce_task_url}" ]] || fail "ceTaskUrl not found in ${report_file}"
  [[ -n "${ce_task_id}" ]] || fail "ceTaskId not found in ${report_file}"

  echo "${dashboard_url}" > "${project_dir}/sonar-dashboard-url.txt"

  log "Waiting for compute engine task ${ce_task_id} ..."
  local waited=0
  local max_wait=600

  while true; do
    curl -s -u "${SONAR_TOKEN}:" "${ce_task_url}" > "${project_dir}/.ce-task.json"

    local status
    status="$(python3 - <<PY
import json
with open(${project_dir@Q} + "/.ce-task.json", "r") as f:
    data=json.load(f)
print(data.get("task", {}).get("status", ""))
PY
)"

    case "${status}" in
      SUCCESS)
        log "Background analysis task completed."
        rm -f "${project_dir}/.ce-task.json"
        return 0
        ;;
      PENDING|IN_PROGRESS)
        ;;
      FAILED|CANCELED)
        echo "Background task failed. See ${project_dir}/.ce-task.json" >&2
        return 1
        ;;
      *)
        ;;
    esac

    sleep 4
    waited=$((waited + 4))
    if (( waited >= max_wait )); then
      echo "Timed out waiting for CE task. See ${project_dir}/.ce-task.json" >&2
      return 1
    fi
  done
}

fetch_paged_issues() {
  local project_key="$1"
  local outfile="$2"
  local page=1
  local page_size=500
  local tmpdir
  tmpdir="$(mktemp -d)"

  while true; do
    local page_file="${tmpdir}/issues_${page}.json"
    curl -s -u "${SONAR_TOKEN}:" \
      "${SONAR_HOST_URL}/api/issues/search?componentKeys=${project_key}&resolved=false&p=${page}&ps=${page_size}" \
      > "${page_file}"

    local paging_total paging_pageindex paging_pagesize
    read -r paging_total paging_pageindex paging_pagesize < <(python3 - <<PY
import json
with open(${page_file@Q}, "r") as f:
    data=json.load(f)
paging=data.get("paging", {})
print(paging.get("total", 0), paging.get("pageIndex", 1), paging.get("pageSize", ${page_size}))
PY
)

    if (( page * page_size >= paging_total )); then
      break
    fi
    page=$((page + 1))
  done

  python3 - <<PY > "${outfile}"
import json, glob, os
files=sorted(glob.glob(os.path.join(${tmpdir@Q}, "issues_*.json")))
merged={"total":0, "issues":[]}
paging=None
for fp in files:
    with open(fp, "r") as f:
        data=json.load(f)
    if paging is None:
        paging=data.get("paging", {})
        merged["paging"]=paging
        for k in ("components","rules","users","languages","facets"):
            if k in data:
                merged[k]=data[k]
    merged["issues"].extend(data.get("issues", []))
    merged["total"]=len(merged["issues"])
print(json.dumps(merged, indent=2))
PY

  rm -rf "${tmpdir}"
}

write_summary() {
  local project_name="$1"
  local project_dir="$2"

  python3 - <<PY > "${project_dir}/sonar-summary.txt"
import json, os

project_name = ${project_name@Q}
project_dir = ${project_dir@Q}

def loadj(path):
    with open(path, "r") as f:
        return json.load(f)

measures = loadj(os.path.join(project_dir, "sonar-measures.json"))
qgate = loadj(os.path.join(project_dir, "sonar-quality-gate.json"))
issues = loadj(os.path.join(project_dir, "sonar-issues.json"))

measure_map = {}
for m in measures.get("component", {}).get("measures", []):
    measure_map[m.get("metric")] = m.get("value")

status = qgate.get("projectStatus", {}).get("status", "UNKNOWN")
conditions = qgate.get("projectStatus", {}).get("conditions", [])

issue_counts = {"BUG":0, "VULNERABILITY":0, "CODE_SMELL":0}
for i in issues.get("issues", []):
    t = i.get("type")
    if t in issue_counts:
        issue_counts[t] += 1

lines = [
    f"Project: {project_name}",
    f"Quality Gate: {status}",
    "",
    "Key Measures:",
    f"  Cyclomatic Complexity: {measure_map.get('complexity', 'n/a')}",
    f"  Cognitive Complexity: {measure_map.get('cognitive_complexity', 'n/a')}",
    f"  Technical Debt (sqale_index): {measure_map.get('sqale_index', 'n/a')}",
    f"  Code Smells: {measure_map.get('code_smells', 'n/a')}",
    f"  Bugs: {measure_map.get('bugs', 'n/a')}",
    f"  Vulnerabilities: {measure_map.get('vulnerabilities', 'n/a')}",
    f"  Security Hotspots: {measure_map.get('security_hotspots', 'n/a')}",
    f"  Duplicated Lines %: {measure_map.get('duplicated_lines_density', 'n/a')}",
    f"  Lines of Code: {measure_map.get('ncloc', 'n/a')}",
    "",
    "Open Issues:",
    f"  BUG: {issue_counts['BUG']}",
    f"  VULNERABILITY: {issue_counts['VULNERABILITY']}",
    f"  CODE_SMELL: {issue_counts['CODE_SMELL']}",
    "",
    "Quality Gate Conditions:",
]

for c in conditions:
    metric = c.get("metricKey", "unknown")
    cstatus = c.get("status", "")
    actual = c.get("actualValue", "")
    error = c.get("errorThreshold", "")
    lines.append(f"  {metric}: status={cstatus}, actual={actual}, threshold={error}")

print("\n".join(lines))
PY
}

scan_project() {
  local project_dir="$1"
  local project_name
  project_name="$(basename "$project_dir")"
  local project_key
  project_key="$(project_key_from_name "$project_name")"

  local build_target
  build_target="$(find_build_target "$project_dir")"

  if [[ -z "${build_target}" ]]; then
    log "Skipping ${project_name}: no .sln or .csproj found."
    return 0
  fi

  log "================================================================="
  log "Scanning project: ${project_name}"
  log "Directory: ${project_dir}"
  log "Build target: ${build_target}"
  log "Project key: ${project_key}"

  create_project_if_missing "${project_key}" "${project_name}"

  local log_file="${project_dir}/sonar-scan.log"
  local report_file="${project_dir}/sonar-report-task.txt"

  rm -f \
    "${log_file}" \
    "${report_file}" \
    "${project_dir}/sonar-quality-gate.json" \
    "${project_dir}/sonar-measures.json" \
    "${project_dir}/sonar-issues.json" \
    "${project_dir}/sonar-summary.txt" \
    "${project_dir}/sonar-dashboard-url.txt"

  (
    set -x
    cd "${project_dir}"

    export PATH="$PATH:$HOME/.dotnet/tools"

    dotnet-sonarscanner begin \
      /k:"${project_key}" \
      /n:"${project_name}" \
      /d:sonar.host.url="${SONAR_HOST_URL}" \
      /d:sonar.token="${SONAR_TOKEN}" \
      /d:sonar.scanner.metadataFilePath="${report_file}" \
      /d:sonar.exclusions="**/bin/**,**/obj/**,**/node_modules/**,**/*.min.js,**/*.Designer.cs,**/Migrations/**"

    dotnet build "${build_target}" --no-incremental

    dotnet-sonarscanner end /d:sonar.token="${SONAR_TOKEN}"
  ) > "${log_file}" 2>&1 || {
    echo "Scan failed for ${project_name}. See ${log_file}" >&2
    return 1
  }

  wait_for_ce_task "${report_file}" "${project_dir}" || return 1

  curl -s -u "${SONAR_TOKEN}:" \
    "${SONAR_HOST_URL}/api/qualitygates/project_status?projectKey=${project_key}" \
    > "${project_dir}/sonar-quality-gate.json"

  curl -s -u "${SONAR_TOKEN}:" \
    "${SONAR_HOST_URL}/api/measures/component?component=${project_key}&metricKeys=complexity,cognitive_complexity,sqale_index,code_smells,bugs,vulnerabilities,security_hotspots,duplicated_lines_density,ncloc,coverage,alert_status" \
    > "${project_dir}/sonar-measures.json"

  fetch_paged_issues "${project_key}" "${project_dir}/sonar-issues.json"

  write_summary "${project_name}" "${project_dir}"

  log "Completed: ${project_name}"
  log "Artifacts written to: ${project_dir}"
}

###############################################################################
# MAIN
###############################################################################
require_cmd curl
require_cmd docker
require_cmd dotnet
require_cmd python3

if [[ -z "${SONAR_TOKEN}" ]]; then
  cat >&2 <<'EOF'
ERROR: SONAR_TOKEN is not set.

One-time setup:
1. Start the script once so SonarQube comes up, or manually run Docker.
2. Open http://localhost:9000
3. Login with admin / admin
4. Generate a token in your SonarQube account
5. Export it:
   export SONAR_TOKEN="your_token_here"

Then run this script again.
EOF
  exit 1
fi

ROOT_DIR="$(cd "${ROOT_DIR}" && pwd)"

ensure_docker_desktop_running
ensure_sonarqube_container
wait_for_sonarqube
ensure_dotnet_scanner

log "Scanning root folder: ${ROOT_DIR}"

shopt -s nullglob
for project_dir in "${ROOT_DIR}"/*; do
  [[ -d "${project_dir}" ]] || continue

  project_name="$(basename "${project_dir}")"

  if [[ "${ONLY_LCP_PREFIX}" == "1" ]]; then
    [[ "${project_name}" == LCP-* ]] || continue
  fi

  scan_project "${project_dir}" || true
done

log "Batch scan complete."