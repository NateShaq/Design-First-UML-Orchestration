#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="${1:-$PWD}"
OUTPUT_CSV="${2:-$ROOT_DIR/lcp_pattern_metrics.csv}"

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

pattern_log_exists() {
  local f="$1/pattern_evaluation.log"
  [[ -f "$f" ]] && echo "1" || echo "0"
}

parse_pattern_log() {
  local log_file="$1"

  [[ -f "$log_file" ]] || {
    printf '|||||\n'
    return
  }

  python3 - "$log_file" <<'PY'
import re
import sys

path = sys.argv[1]

try:
    with open(path, "r", encoding="utf-8") as f:
        text = f.read()
except Exception:
    print("|||||")
    sys.exit(0)

def grab(pattern, text, flags=0):
    m = re.search(pattern, text, flags)
    return m.group(1).strip() if m else ""

total_classes = grab(r"Total classes counted:\s*([0-9]+)", text)
patterns_found_count = grab(r"Patterns found:\s*([0-9]+)", text)

patterns_found_description = grab(
    r"Patterns found:\s*[0-9]+\s*\((.*?)\)",
    text,
    flags=re.DOTALL
)

pattern_density_ratio = grab(
    r"Pattern density:\s*([0-9]+\s*/\s*[0-9]+\s*=\s*[0-9.]+)",
    text
)

pattern_density_pct = grab(
    r"Pattern density:.*?\(([0-9.]+%)\)",
    text
)

solid_grade = grab(r"SOLID/Decoupling grade:\s*([A-F][+-]?)", text)

solid_notes = ""
m = re.search(r"SOLID/Decoupling grade:\s*[A-F][+-]?\s*(.*)$", text, flags=re.DOTALL)
if m:
    tail = m.group(1).strip()
    # collapse whitespace but preserve bullet meanings
    lines = []
    for line in tail.splitlines():
        s = line.strip()
        if s:
            lines.append(s)
    solid_notes = " | ".join(lines)

print("|".join([
    total_classes,
    patterns_found_count,
    patterns_found_description,
    pattern_density_ratio,
    pattern_density_pct,
    solid_grade,
    solid_notes
]))
PY
}

main() {
  require_cmd python3

  ROOT_DIR="$(cd "$ROOT_DIR" && pwd)"

  log "Aggregating pattern metrics from: $ROOT_DIR"
  log "Writing CSV to: $OUTPUT_CSV"

  {
    echo "project_name,project_family,pattern_log_exists,total_classes_counted,patterns_found_count,patterns_found_description,pattern_density_ratio,pattern_density_pct,solid_decoupling_grade,solid_notes"
  } > "$OUTPUT_CSV"

  shopt -s nullglob
  for project_dir in "$ROOT_DIR"/LCP-*; do
    [[ -d "$project_dir" ]] || continue

    project_name="$(basename "$project_dir")"
    family="$(project_family "$project_name")"
    log_exists="$(pattern_log_exists "$project_dir")"

    IFS='|' read -r total_classes_counted patterns_found_count patterns_found_description pattern_density_ratio pattern_density_pct solid_decoupling_grade solid_notes \
      <<< "$(parse_pattern_log "$project_dir/pattern_evaluation.log")"

    {
      csv_escape "$project_name"; printf ","
      csv_escape "$family"; printf ","
      csv_escape "$log_exists"; printf ","
      csv_escape "$total_classes_counted"; printf ","
      csv_escape "$patterns_found_count"; printf ","
      csv_escape "$patterns_found_description"; printf ","
      csv_escape "$pattern_density_ratio"; printf ","
      csv_escape "$pattern_density_pct"; printf ","
      csv_escape "$solid_decoupling_grade"; printf ","
      csv_escape "$solid_notes"; printf "\n"
    } >> "$OUTPUT_CSV"

    log "Processed: $project_name"
  done

  log "Done. CSV created at: $OUTPUT_CSV"
}

main "$@"