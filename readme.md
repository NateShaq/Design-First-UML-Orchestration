# NL2UML Experimental Benchmarking Suite
**Project:** Fidelity Gap Analysis: Single-Shot vs. UML-Anchored Code Generation  
**Researcher:** Jordan Howell  
**Environment:** MacOS Terminal / OpenAI Codex v0.116.0 (Research Preview)

---

## 1. Experimental Objective
To quantify the "Fidelity Gap" in high-complexity (30+ classes) enterprise system generation. This suite compares:
* **Path A (Vibe Coding):** Monolithic, single-shot generation of models, controllers, and schemas.
* **Path B (Anchored Design):** A 3-stage pipeline using PlantUML as a persistent structural and academic "anchor."

## 2. Environment Prerequisites
* **Runtime:** .NET 8.0 SDK
* **Packages:** EntityFrameworkCore, Swashbuckle (OpenAPI), System.Transactions
* **CLI Tools:** `codex` (v0.116.0), `git` (v2.x+), `zsh`
* **Sandbox Mode:** `workspace-write`

---

## 3. The "Tabula Rasa" (Blank Slate) Protocol
To ensure statistical independence and prevent **Model Learning Bias**, the following cleanup commands are executed before every test cycle:

```bash
# Purge Short-Term Session Context
rm -rf ~/.codex/sessions/*

# Purge Long-Term Workspace Memory (Prevents cross-test spillover)
rm -rf ~/.codex/memories/*


## 4. Test Matrix & Automated Workflow
[cite_start]The `run_benchmarks.sh` script iterates through **9 Tier-3 complexity prompts**[cite: 6]. For each prompt, the following lifecycle is performed:

### Phase I: Baseline (LCP-Vibe-X)
* [cite_start]**Direct Generation:** Executes a one-shot request for 30 distinct classes, a C# .NET Web API, and a SQL schema[cite: 1, 9].
* [cite_start]**Build Audit:** Performs an automated `dotnet build` dry-run to identify missing types, namespace collisions, or incomplete class definitions[cite: 5].

### Phase II: Anchored (LCP-UML-X)
* [cite_start]**UML Synthesis:** Initial architectural drafting of the system using PlantUML[cite: 2, 7].
* [cite_start]**Academic Audit:** Conducts an AI-led "Deep Architectural Review" of the `.puml` file to detect "Ghost Writes" (concurrency anomalies) and verify 3rd Normal Form (3NF) normalization[cite: 3].
* [cite_start]**Implementation:** Generates the C# and SQL codebase using the validated PlantUML as a strict structural anchor to prevent architectural drift[cite: 4, 11].
* [cite_start]**Build Audit:** Final verification of compilation, package integrity, and class recall[cite: 5].

## 5. Metrics for Evaluation
Results are logged in `experiment_metadata.log` and `build_audit.log` within each folder. Key metrics include:

| Metric | Academic Relevance | Detection Method |
| :--- | :--- | :--- |
| **Structural Recall** | [cite_start]Contextual Persistence [cite: 26, 31, 41, 46, 51] | `grep` class definitions vs. Prompt requirements |
| **Build Status** | Syntactic Integrity | `dotnet build` exit code (Target: PASS) |
| **Concurrency Logic** | [cite_start]09concurrency.pptx (Ghost Writes) [cite: 3, 4, 10] | Presence of `RowVersion` or `[Timestamp]` |
| **Transactional Scope** | [cite_start]ACID Compliance  | Presence of `IsolationLevel.Serializable` |

## 6. Reproducibility
To reproduce these experimental results, ensure that the `~/.codex/` directory is accessible and that the `codex` CLI is authenticated. Run the following command from the project root:

```bash
chmod +x run_benchmarks.sh && ./run_benchmarks.sh

## 7. Results Aggregation
After the benchmark completes, a summary of the 20 test cases (Vibe vs. Anchored) can be generated to compare efficiency and integrity metrics.

### Automated Results Summary
A secondary script is utilized to crawl all generated project folders (`LCP-Vibe-*` and `LCP-UML-*`) to output a unified `results_summary.csv`. This provides an instant, structured data set for the paper’s Results section, including:

* **Total Token Count:** Measuring the "Reasoning Effort" and computational cost for each approach.
* **Build Status:** A binary **PASS/FAIL** metric for syntactic integrity and dependency resolution.
* **Class Count:** Verification of the **30-class requirement** to measure structural recall.
* **Anomaly Detection:** Logging specific instances of **Ghost Writes** or **Transactional Failures** identified during the audit.