#!/usr/bin/env bash
set -euo pipefail

SONAR_CONTAINER_NAME="sonarqube"
SONAR_IMAGE="sonarqube:latest"
SONAR_URL="http://localhost:9000"

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

ensure_docker_running() {
  if docker info >/dev/null 2>&1; then
    log "Docker is already running."
    return
  fi

  log "Docker is not running. Attempting to start Docker Desktop..."
  open -ga Docker || true

  local waited=0
  local max_wait=180
  until docker info >/dev/null 2>&1; do
    sleep 3
    waited=$((waited + 3))
    if (( waited >= max_wait )); then
      fail "Docker Desktop did not become ready within ${max_wait} seconds."
    fi
  done

  log "Docker is now running."
}

ensure_sonarqube_container() {
  if docker ps --format '{{.Names}}' | grep -qx "${SONAR_CONTAINER_NAME}"; then
    log "SonarQube container '${SONAR_CONTAINER_NAME}' is already running."
    return
  fi

  if docker ps -a --format '{{.Names}}' | grep -qx "${SONAR_CONTAINER_NAME}"; then
    log "Starting existing SonarQube container '${SONAR_CONTAINER_NAME}'..."
    docker start "${SONAR_CONTAINER_NAME}" >/dev/null
    return
  fi

  log "Creating SonarQube container '${SONAR_CONTAINER_NAME}'..."
  docker run -d \
    --name "${SONAR_CONTAINER_NAME}" \
    -e SONAR_ES_BOOTSTRAP_CHECKS_DISABLE=true \
    -p 9000:9000 \
    "${SONAR_IMAGE}" >/dev/null

  log "SonarQube container created."
}

wait_for_sonarqube() {
  log "Waiting for SonarQube to become ready at ${SONAR_URL} ..."
  local waited=0
  local max_wait=420

  while true; do
    if curl -s "${SONAR_URL}/api/system/status" >/tmp/sonar_status.json 2>/dev/null; then
      status="$(python3 - <<'PY'
import json
try:
    with open("/tmp/sonar_status.json", "r") as f:
        data = json.load(f)
    print(data.get("status", ""))
except Exception:
    print("")
PY
)"
      if [[ "${status}" == "UP" ]]; then
        log "SonarQube is UP."
        return
      fi
      [[ -n "${status}" ]] && log "Current SonarQube status: ${status}"
    fi

    sleep 5
    waited=$((waited + 5))
    if (( waited >= max_wait )); then
      fail "SonarQube did not become ready within ${max_wait} seconds."
    fi
  done
}

main() {
  require_cmd docker
  require_cmd curl
  require_cmd python3

  ensure_docker_running
  ensure_sonarqube_container
  wait_for_sonarqube

  cat <<EOF

SonarQube is ready at:
  ${SONAR_URL}

Next steps:
1. Open ${SONAR_URL}
2. Log in with:
     username: admin
     password: admin
3. Change the default password if prompted
4. Go to your account security page
5. Generate a USER TOKEN
6. Copy the token

Then export it in this terminal:

  export SONAR_TOKEN='paste_your_local_token_here'

You can verify it is set with:

  echo "\$SONAR_TOKEN"

After that, run your batch scan script.

Helpful Docker commands:
  docker ps
  docker logs -f ${SONAR_CONTAINER_NAME}
  docker stop ${SONAR_CONTAINER_NAME}
  docker start ${SONAR_CONTAINER_NAME}

EOF
}

main "$@"