#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="${1:-$PWD}"
OUTPUT_CSV="${2:-$ROOT_DIR/lcp_sonar_metrics.csv}"

log() {
  printf "[%s] %s\n" "$(date '+%Y-%m-%d %H:%M:%S')" "$*"
}

require_cmd() {
  command -v "$1" >/dev/null 2>&1 || {
    echo "Missing required command: $1" >&2
    exit 1
  }
}

csv_escape() {
  local s="${1:-}"
  s="${s//\"/\"\"}"
  printf '"%s"' "$s"
}

json_metric_value() {
  local json_file="$1"
  local metric_key="$2"

  [[ -f "$json_file" ]] || {
    echo ""
    return
  }

  python3 - "$json_file" "$metric_key" <<'PY'
import json, sys

path = sys.argv[1]
metric_key = sys.argv[2]

try:
    with open(path, "r") as f:
        data = json.load(f)
    measures = data.get("component", {}).get("measures", [])
    for m in measures:
        if m.get("metric") == metric_key:
            print(m.get("value", ""))
            break
    else:
        print("")
except Exception:
    print("")
PY
}

json_quality_gate_status() {
  local json_file="$1"

  [[ -f "$json_file" ]] || {
    echo ""
    return
  }

  python3 - "$json_file" <<'PY'
import json, sys

try:
    with open(sys.argv[1], "r") as f:
        data = json.load(f)
    print(data.get("projectStatus", {}).get("status", ""))
except Exception:
    print("")
PY
}

json_issue_counts() {
  local json_file="$1"

  [[ -f "$json_file" ]] || {
    echo "0,0,0,0"
    return
  }

  python3 - "$json_file" <<'PY'
import json, sys

try:
    with open(sys.argv[1], "r") as f:
        data = json.load(f)

    issues = data.get("issues", [])
    total = len(issues)
    bugs = sum(1 for i in issues if i.get("type") == "BUG")
    vulns = sum(1 for i in issues if i.get("type") == "VULNERABILITY")
    smells = sum(1 for i in issues if i.get("type") == "CODE_SMELL")

    print(f"{total},{bugs},{vulns},{smells}")
except Exception:
    print("0,0,0,0")
PY
}

file_exists_flag() {
  local f="$1"
  [[ -f "$f" ]] && echo "1" || echo "0"
}

project_family() {
  local name="$1"
  if [[ "$name" == LCP-UML-* ]]; then
    echo "UML"
  elif [[ "$name" == LCP-Vibe-* ]]; then
    echo "Vibe"
  else
    echo "Other"
  fi
}

did_sonar_scan() {
  local project_dir="$1"

  if [[ -f "$project_dir/sonar-measures.json" && -f "$project_dir/sonar-issues.json" && -f "$project_dir/sonar-quality-gate.json" ]]; then
    echo "1"
  else
    echo "0"
  fi
}

did_build() {
  local project_dir="$1"
  local scan_log="$project_dir/sonar-scan.log"

  [[ -f "$scan_log" ]] || {
    echo "0"
    return
  }

  if grep -Eq "Build succeeded\.|0 Error\(s\)" "$scan_log"; then
    echo "1"
  else
    echo "0"
  fi
}

main() {
  require_cmd python3

  ROOT_DIR="$(cd "$ROOT_DIR" && pwd)"

  log "Aggregating Sonar metrics from: $ROOT_DIR"
  log "Writing CSV to: $OUTPUT_CSV"

  {
    echo "project_name,project_family,did_build,did_sonar_scan,quality_gate_status,cyclomatic_complexity,cognitive_complexity,technical_debt_sqale_index,code_smells,bugs,vulnerabilities,security_hotspots,duplicated_lines_density,ncloc,coverage,open_issue_count,bug_issue_count,vulnerability_issue_count,code_smell_issue_count,build_audit_exists,evaluation_audit_exists,pattern_evaluation_exists,experiment_metadata_exists,sonar_summary_exists"
  } > "$OUTPUT_CSV"

  shopt -s nullglob
  for project_dir in "$ROOT_DIR"/LCP-*; do
    [[ -d "$project_dir" ]] || continue

    local_name="$(basename "$project_dir")"
    local_family="$(project_family "$local_name")"

    sonar_measures="$project_dir/sonar-measures.json"
    sonar_issues="$project_dir/sonar-issues.json"
    sonar_qgate="$project_dir/sonar-quality-gate.json"

    local_did_build="$(did_build "$project_dir")"
    local_did_sonar_scan="$(did_sonar_scan "$project_dir")"

    quality_gate_status="$(json_quality_gate_status "$sonar_qgate")"
    complexity="$(json_metric_value "$sonar_measures" "complexity")"
    cognitive_complexity="$(json_metric_value "$sonar_measures" "cognitive_complexity")"
    sqale_index="$(json_metric_value "$sonar_measures" "sqale_index")"
    code_smells="$(json_metric_value "$sonar_measures" "code_smells")"
    bugs="$(json_metric_value "$sonar_measures" "bugs")"
    vulnerabilities="$(json_metric_value "$sonar_measures" "vulnerabilities")"
    security_hotspots="$(json_metric_value "$sonar_measures" "security_hotspots")"
    duplicated_lines_density="$(json_metric_value "$sonar_measures" "duplicated_lines_density")"
    ncloc="$(json_metric_value "$sonar_measures" "ncloc")"
    coverage="$(json_metric_value "$sonar_measures" "coverage")"

    IFS=',' read -r open_issue_count bug_issue_count vulnerability_issue_count code_smell_issue_count \
      <<< "$(json_issue_counts "$sonar_issues")"

    build_audit_exists="$(file_exists_flag "$project_dir/build_audit.log")"
    evaluation_audit_exists="$(file_exists_flag "$project_dir/evaluation_audit.log")"
    pattern_evaluation_exists="$(file_exists_flag "$project_dir/pattern_evaluation.log")"
    experiment_metadata_exists="$(file_exists_flag "$project_dir/experiment_metadata.log")"
    sonar_summary_exists="$(file_exists_flag "$project_dir/sonar-summary.txt")"

    {
      csv_escape "$local_name"; printf ","
      csv_escape "$local_family"; printf ","
      csv_escape "$local_did_build"; printf ","
      csv_escape "$local_did_sonar_scan"; printf ","
      csv_escape "$quality_gate_status"; printf ","
      csv_escape "$complexity"; printf ","
      csv_escape "$cognitive_complexity"; printf ","
      csv_escape "$sqale_index"; printf ","
      csv_escape "$code_smells"; printf ","
      csv_escape "$bugs"; printf ","
      csv_escape "$vulnerabilities"; printf ","
      csv_escape "$security_hotspots"; printf ","
      csv_escape "$duplicated_lines_density"; printf ","
      csv_escape "$ncloc"; printf ","
      csv_escape "$coverage"; printf ","
      csv_escape "$open_issue_count"; printf ","
      csv_escape "$bug_issue_count"; printf ","
      csv_escape "$vulnerability_issue_count"; printf ","
      csv_escape "$code_smell_issue_count"; printf ","
      csv_escape "$build_audit_exists"; printf ","
      csv_escape "$evaluation_audit_exists"; printf ","
      csv_escape "$pattern_evaluation_exists"; printf ","
      csv_escape "$experiment_metadata_exists"; printf ","
      csv_escape "$sonar_summary_exists"; printf "\n"
    } >> "$OUTPUT_CSV"

    log "Processed: $local_name"
  done

  log "Done. CSV created at: $OUTPUT_CSV"
}

main "$@"