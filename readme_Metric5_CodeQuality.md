# Metric 5 – Automated Code Quality Analysis with SonarQube

This document explains how to reproduce **Metric 5: Automated Quality (SonarQube / Static Analysis)** for the generated .NET codebases in this experiment.

## Purpose

The goal of this metric is to compare the automated code quality of generated project outputs using a consistent static analysis process.

This test evaluates whether one generation approach produces code that is:

- less structurally complex,
- easier for humans to read and maintain,
- and estimated to require less remediation effort.

## Metric Definition

### Automated Quality (SonarQube / Static Analysis)

**The Test:**  
Run each generated project through **SonarQube** using local static analysis.

**Primary Metrics Tracked:**

- **Cyclomatic Complexity**  
  Measures the structural complexity of the code and the number of possible execution paths. Higher values may indicate deeply nested or overly complex logic.

- **Cognitive Complexity**  
  Measures how difficult the code is for a human to understand. This emphasizes readability and maintainability beyond raw branching count.

- **Technical Debt Ratio / Debt Estimate**  
  Provides an automated estimate of how much remediation effort would be required to bring the code to a cleaner maintainability state.

## Project Context

The generated outputs in this experiment are **C# .NET web applications** with SQL-backed architecture.  
Each project is stored in its own folder, for example:

- `LCP-UML-1`
- `LCP-UML-2`
- `LCP-Vibe-1`
- `LCP-Vibe-2`

Each project may contain the actual `.csproj` file deeper in the directory tree, typically under a nested `src/` folder.

## Tooling Used

- **Docker Desktop**
- **SonarQube (local Docker container)**
- **SonarScanner for .NET**
- **macOS Terminal Bash scripting**

## Reproducibility Workflow

The scan process was split into two stages:

### Step 1 – Start Local SonarQube

A helper script is used to start the local SonarQube Docker container:

```bash
./setup_local_sonarqube.sh

After the container is ready:
	1.	Open http://localhost:9000
	2.	Log in using the local SonarQube account
	3.	Generate a local SonarQube token
	4.	Export the token into the terminal session:

    export SONAR_TOKEN='your_local_sonarqube_token'

Step 2 – Run Batch Analysis Across Projects
./run_sonar_batch.sh

This script:
	•	ensures Docker is running,
	•	confirms SonarQube is available,
	•	iterates through each experiment folder,
	•	detects the .sln or .csproj file,
	•	runs SonarScanner for .NET,
	•	waits for SonarQube background analysis to finish,
	•	exports project-level quality artifacts into the root of each project folder.

Output Artifacts

For each scanned project, the script writes the following files to the project root:
	•	sonar-scan.log
Full scanner/build log.
	•	sonar-report-task.txt
SonarQube task metadata.
	•	sonar-quality-gate.json
Quality gate result.
	•	sonar-measures.json
Exported SonarQube measures for selected metrics.
	•	sonar-issues.json
Open issues identified by SonarQube.
	•	sonar-summary.txt
Human-readable summary of key results.

Metrics Extracted

The batch script queries SonarQube for the following measures:
	•	complexity
	•	cognitive_complexity
	•	sqale_index
	•	code_smells
	•	bugs
	•	vulnerabilities
	•	security_hotspots
	•	duplicated_lines_density
	•	ncloc
	•	coverage
	•	alert_status

For the purposes of Metric 5, the primary measures of interest are:
	•	complexity → Cyclomatic Complexity
	•	cognitive_complexity → Cognitive Complexity
	•	sqale_index → Technical Debt estimate