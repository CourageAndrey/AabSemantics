## AabSemantics: Project Overview and AI Agent Guide

This document provides a high-level description of the AabSemantics repository and precise guidance for AI agents contributing changes. Keep this document concise, actionable, and up to date.

### What this repository contains

- **Solution**: `Code/AabSemantics.sln`
  - Central solution aggregating core library, modules, extensions, clients, samples, and tests.

- **Core**: `Code/Core/AabSemantics`
  - The main semantics engine: concepts, contexts, modules, statements, questions, answers, text, serialization, and utilities.
  - Subfolders of note:
    - `Interfaces/`: Public contracts; treat as API surface.
    - `Modules/`: Built-in modules and composition points.
    - `Serialization/`: DTOs and persistence wire formats; backwards compatibility matters.
    - `Text/`, `Localization/`: Text generation, localization, and structured text.

- **Modules**: `Code/Modules/*`
  - Optional domain modules (e.g., Set, Processes, Mathematics, NaturalSciense) that extend the core engine.
  - Some module-specific sample and integration projects exist under `Inventor.Semantics.*` and `Samples.*`.

- **Extensions**: `Code/Extensions/*`
  - EF integration (`AabSemantics.Extensions.EF`) and WPF integration helpers (`AabSemantics.Extensions.WPF`).

- **Clients**: `Code/Clients/*`
  - `AabSemantics.SimpleRestClient`: Minimal ASP.NET-based REST service exposing semantics operations.
  - `AabSemantics.SimpleWpfClient`: WPF showcase UI.

- **Samples**: `Code/Samples/*`
  - Small console or UI samples demonstrating statements, questions, modules, customizations, productions, and EF usage.

- **Tests**: `Code/Tests/*`
  - Unit and integration tests across core and modules. Use them to verify behavioral compatibility.

### Build and run

- Open `Code/AabSemantics.sln` in Visual Studio 2019/2022 with the .NET development workload.
- Restore NuGet packages (automatic on build). Packages are managed via `packages.config` in several projects; keep using that unless migrating the entire solution.
- Build the solution (Any CPU is generally supported; prefer Debug while iterating).
- Run samples from `Code/Samples/*` to validate expected behavior after changes.

### Technology notes

- This repository uses C# and .NET (some projects use classic `packages.config`).
- Entity Framework 6.x is used in EF-related projects (`Code/packages/EntityFramework.*`). Avoid uncoordinated major upgrades.
- WPF client targets .NET Framework; do not assume .NET (Core) WPF unless explicitly migrated.

### Architectural overview

- The core (`AabSemantics`) defines the semantic network, concepts, statements, questions, answers, and text generation/localization.
- Modules extend the core with domain-specific statements, questions, and processing.
- Extensions integrate with EF and WPF for persistence and UI.
- Clients expose the engine via REST or present a WPF UX.

### Backwards compatibility priorities

When modifying code, maintain compatibility in:

- Public contracts in `Core/AabSemantics/Interfaces/*` and other public types that are consumed by modules/clients.
- Serialization contracts in `Core/AabSemantics/Serialization/*` and any wire formats used by REST clients.
- Resource keys and localization structure in `Localization/*`.

Breaking changes must be isolated behind adapters or versioned DTOs. Prefer additive changes over mutating existing contracts.

### Testing expectations

- Run unit tests under `Code/Tests/*` after any non-trivial change.
- Add tests when changing behavior, fixing bugs, or adding features.
- Keep tests deterministic; avoid time- or randomness-dependent assertions.

### Coding standards (C#)

- Favor clarity and explicitness over cleverness. Prefer descriptive names over abbreviations.
- Keep functions small with clear responsibilities; use early returns instead of deep nesting.
- Minimize `try/catch`; handle only anticipated exceptions meaningfully.
- Document only non-obvious rationale, invariants, or edge cases. Avoid redundant comments.
- Match existing formatting; do not reformat unrelated code in the same edit.

### Contributing workflow

- Use feature branches. Keep edits scoped and reviewable.
- Write clear commit messages summarizing the change and impact (present tense, imperative mood).
- For multi-file changes, structure commits logically (e.g., API addition, then implementation, then tests).

### Areas that require extra care

- `Core/AabSemantics/Interfaces`: Treat as stable API; prefer extension points over edits.
- `Core/AabSemantics/Serialization`: Do not change existing DTO shapes without versioning and migration.
- `Extensions/EF`: Ensure model mappings remain consistent; migrations should be explicit if added.
- `Clients/*`: Keep endpoints stable; changing REST contracts requires versioning.
- `Localization/*`: Preserve resource keys; adding is fine, renaming requires a cross-repo audit.

### Guidance for AI agents (Cursor / automated assistants)

Follow these rules strictly when making changes:

- Make the smallest viable edit that achieves the goal. Do not refactor unrelated code.
- Preserve existing indentation style and width; do not mix tabs and spaces.
- Maintain file encoding and line endings; do not introduce BOM changes.
- If adding new files, place them in the most specific directory (e.g., a new module under `Code/Modules/<ModuleName>`; a shared contract under `Core/AabSemantics/Interfaces`).
- Prefer additive, backwards-compatible changes. Avoid breaking public APIs and serialization.
- Update or add tests for any behavioral change; run tests locally.
- Keep comments concise and only for non-obvious context.
- Do not upgrade third-party packages without an explicit instruction.
- Avoid introducing long-lived feature flags unless specified; keep configuration simple.
- For REST work, document new endpoints and payloads inline and in the client project README or controllers' XML docs.

When uncertain:

- Search for usage across `Core`, `Modules`, `Extensions`, `Clients`, `Samples`, and `Tests` to assess impact.
- Prefer introducing new interfaces or overloads rather than mutating existing ones.

### How to add a new module (quick checklist)

1. Create a project under `Code/Modules/<YourModuleName>` following existing module csproj patterns.
2. Define statements, questions, and answers types extending core abstractions.
3. Register module with the semantic network composition where needed.
4. Add unit tests under `Code/Tests/<YourModuleName>.Tests`.
5. Add a minimal sample under `Code/Samples/` if appropriate.

### How to extend the REST client (quick checklist)

1. Add a new controller under `Code/Clients/AabSemantics.SimpleRestClient/Controllers`.
2. Reuse core/module services; avoid duplicating business logic.
3. Ensure request/response models are versioned or additive.
4. Add sample requests to `README.md` or controller XML docs.

### License and provenance

- See `LICENSE` at the repository root for licensing.

### Document maintenance

- Keep this guide accurate with any project-wide or process changes.
- If major architecture evolves, update the overview and the “Areas that require extra care” section.


