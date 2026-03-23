#!/bin/zsh

echo "Starting Dynamic Design Pattern Evaluation Suite..."
echo "------------------------------------------"

# Loop through every directory in the current location
for PROJECT in */; do
    # Remove trailing slash for logging
    PROJECT=${PROJECT%/}

    echo "[Evaluating] Target: $PROJECT"
    
    # Reset Session for Tabula Rasa evaluation
    rm -rf ~/.codex/sessions/*
    rm -rf ~/.codex/memories/*

    # Execute the AI Static Analysis Agent
    codex exec --sandbox workspace-write "
        Act as a Senior Software Architect. 
        Navigate the source code in ./$PROJECT.
        
        1. Identify Software Design Patterns (Strategy, Factory, Repository, Observer, etc.).
        2. Identify Abstract/Interface types vs. Concrete implementations.
        3. Grade the implementation (A-F) based on Decoupling and SOLID principles.
        4. Document findings in a markdown table.
        5. Calculate 'Pattern Density' (Patterns Found / Total Classes).
        
        Output results to: ./$PROJECT/pattern_evaluation.log
    " 2>&1 | tee -a ./$PROJECT/evaluation_audit.log

    echo "[Success] Evaluation logged for $PROJECT"
done

echo "------------------------------------------"
echo "All evaluations complete."