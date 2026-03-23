# Evaluation Metrics and Reproducibility Guide

This document explains the evaluation metrics used in this study and clarifies which metrics are supported by the current automation scripts.

The project currently includes automation for:

- **SonarQube-based code quality evaluation**
- **Design pattern evaluation scripting**

These scripts help produce reproducible evidence for several of the paper’s core evaluation dimensions.

---

# Overview of the Five Key Metrics

## 1. Structural Integrity & Completeness (Recall)

### Purpose
This metric measures whether the generated code fully reflects the structure requested in the original prompt.

### Test
Map the generated code back to the original system requirements and count how many requested classes actually exist in the produced codebase.

### Metric
- **Count of requested classes found in final code**
- Example:
  - Prompt requested 30 classes
  - Generated system contains 28 valid classes
  - Structural recall = 28 / 30 = 93.3%

### Why it matters
For large enterprise-style prompts, missing classes often indicate architecture collapse, context loss, or incomplete implementation.

### Prediction
The UML-mediated path is expected to perform better because the UML acts as a validated structural blueprint before implementation begins.

### Script support
This metric is **not directly produced by the SonarQube batch script**.

It is best evaluated through:
- class inventory extraction,
- UML-to-code mapping,
- or an additional structural audit script.

---

## 2. SOLID Principles Adherence

### Purpose
This metric evaluates whether the generated code follows sound object-oriented design principles, especially:

- **Single Responsibility Principle (SRP)**
- **Interface Segregation Principle (ISP)**

### Test
Inspect generated classes for merged responsibilities, oversized “God classes,” or interfaces that force clients to depend on methods they do not need.

### Metric
- Count or percentage of classes flagged for SOLID violations
- Presence of God Classes
- Optional LLM-as-a-Judge review or static analysis interpretation

### Why it matters
A design-first process should produce cleaner separations of responsibility and more modular abstractions.

### Prediction
Single-shot generation often compresses too much functionality into fewer classes. The UML-first approach should naturally separate concerns better.

### Script support
This metric is **partially supported by the SonarQube batch script**.

The script does not directly label “SRP violation” or “ISP violation,” but it does generate artifacts that help identify likely SOLID problems:

- `sonar-measures.json`
- `sonar-issues.json`
- `sonar-summary.txt`

Relevant indicators include:
- cognitive complexity
- cyclomatic complexity
- code smells
- large issue counts
- maintainability concerns

So this metric is **indirectly supported** by the SonarQube workflow.

---

## 3. Design Pattern Density

### Purpose
This metric evaluates whether the generated system uses recognized object-oriented design patterns where appropriate.

### Test
Identify whether requested relationships or responsibilities are implemented using standard patterns such as:

- Factory
- Strategy
- Observer

### Metric
- **Percentage of requested relationships correctly implemented using recognized patterns**

### Why it matters
This measures whether the generated code reflects intentional design thinking rather than brute-force procedural logic.

For example, different freight handling behaviors may be more appropriately implemented using a **Strategy pattern** instead of a chain of `if/else` statements.

### Prediction
The UML stage should improve this metric because explicit inheritance, composition, and separation of concerns in the UML design can pressure the downstream code generation step toward proper pattern usage.

### Script support
This metric is **supported by the design pattern evaluation script**.

The `evaluate_patterns.sh` workflow is intended to help inspect generated projects for evidence of pattern-oriented implementation.

This makes **Metric 3 one of the primary metrics supported by automation beyond SonarQube**.

---

## 4. Database Normalization (3NF)

### Purpose
This metric evaluates whether the generated persistence model and related classes reflect normalized data design principles.

### Test
Inspect the persistence layer, DTOs, entities, and related storage classes for:

- transitive dependencies,
- redundant storage,
- duplicated business data across entities,
- poor separation between conceptual entities.

### Metric
- presence/absence of transitive dependencies
- redundant field duplication
- normalized vs denormalized class structure
- qualitative 3NF readiness assessment

### Why it matters
This metric extends the design-first idea into the data layer. Clean UML should lead to cleaner data modeling, which is essential for enterprise-scale maintainability.

### Script support
This metric is **not directly produced by the SonarQube script**.

It may be evaluated through:
- manual inspection,
- entity/DTO relationship review,
- ERD comparison,
- or a future specialized script.

---

## 5. Automated Quality (SonarQube / Static Analysis)

### Purpose
This metric evaluates maintainability and automated code quality using static analysis.

### Test
Run each generated project through SonarQube.

### Primary Metrics Tracked
- **Cyclomatic Complexity**
- **Cognitive Complexity**
- **Technical Debt estimate**
- optionally code smells, bugs, vulnerabilities, duplication, and lines of code

### Why it matters
This provides a reproducible and automated signal of maintainability and structural cleanliness.

### Prediction
The UML-mediated design-first workflow should generate code with:
- lower complexity,
- cleaner separation,
- lower debt,
- fewer maintainability issues.

### Script support
This metric is **directly supported by the SonarQube batch script**.

The script produces:
- `sonar-scan.log`
- `sonar-report-task.txt`
- `sonar-quality-gate.json`
- `sonar-measures.json`
- `sonar-issues.json`
- `sonar-summary.txt`

For this paper, the most important extracted measures are:
- `complexity`
- `cognitive_complexity`
- `sqale_index`

---

# Which Metrics Are Produced by the Current Scripts?

## SonarQube Batch Script (`run_sonar_batch.sh`)

This script directly supports:

- **Metric 5 – Automated Quality**
- **Metric 2 – SOLID Principles Adherence** (indirectly)

### Direct outputs relevant to Metric 5
- Cyclomatic Complexity
- Cognitive Complexity
- Technical Debt estimate
- Code Smells
- Bugs
- Vulnerabilities
- Duplication measures
- Lines of code
- Quality gate results

### Indirect support for Metric 2
Although SonarQube does not explicitly say “this is a God Class,” the following outputs are useful signals for probable SOLID violations:

- high cognitive complexity
- high cyclomatic complexity
- elevated code smell counts
- maintainability warnings
- classes with excessive branching or multiple responsibilities

So the SonarQube workflow provides strong evidence for **Metric 5** and supporting evidence for **Metric 2**.

---

## Design Pattern Script (`evaluate_patterns.sh`)

This script primarily supports:

- **Metric 3 – Design Pattern Density**

Depending on how the script is implemented, it may also assist qualitative review of:
- inheritance usage
- composition structure
- architectural intent

Its main purpose is to identify whether generated code relationships were implemented using recognized design patterns rather than ad hoc logic.

---

# Metrics Not Fully Automated Yet

The following metrics still require either manual review or additional tooling:

- **Metric 1 – Structural Integrity & Completeness**
- **Metric 4 – Database Normalization (3NF)**

These are still important to the paper, but they are not fully produced by the current SonarQube automation.