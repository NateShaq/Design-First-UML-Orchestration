#!/bin/zsh

CSV_FILE="pattern_density_report.csv"

# Write the CSV Header
echo "Project_ID,Architecture_Path,Pattern_Density,Implementation_Grade,Abstraction_Ratio,Primary_Patterns" > "$CSV_FILE"

echo "Starting Codex Data Extraction..."
echo "------------------------------------------"

# Iterate only through LCP- folders to avoid errors in 'IdentityPublicServicesScaffoldErr'
for PROJECT_DIR in LCP-*/; do
    PROJECT="${PROJECT_DIR%/}"
    LOG_FILE="./$PROJECT/pattern_evaluation.log"

    if [ -f "$LOG_FILE" ]; then
        echo "[Processing] $PROJECT"

        # Logic to determine the architecture path for the CSV
        [[ "$PROJECT" == *"UML"* ]] && PATH_TYPE="UML-Anchored" || PATH_TYPE="Vibe-SingleShot"

        # Silent cleanup of previous session artifacts
        rm -rf ~/.codex/sessions/* 2>/dev/null
        rm -rf ~/.codex/memories/* 2>/dev/null

        # Use escaped quotes around the log file path to handle spaces in directory names
        RESULT_ROW=$(codex exec --sandbox read-only "
            Identify and read the file at path: \"$LOG_FILE\".
            Extract these specific metrics from the text:
            1. Pattern Density (as a decimal or fraction).
            2. Implementation Grade (A, B, C, D, or F).
            3. Abstraction Ratio (Number of Interfaces/Abstract classes vs Total Classes).
            4. Primary Patterns (The top 2-3 patterns found).

            Return ONLY a single line of CSV data in this exact order:
            $PROJECT,$PATH_TYPE,Density,Grade,Ratio,Patterns
        ")

        # Append the successfully parsed row to the CSV
        echo "$RESULT_ROW" >> "$CSV_FILE"
        echo "[Success] Data added for $PROJECT"
    else
        echo "[Skip] No evaluation log found in $PROJECT"
    fi
done

echo "------------------------------------------"
echo "Aggregation Complete: $CSV_FILE"