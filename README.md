## AabSemantics: Project Overview and AI Agent Guide

This document provides a high-level description of the AabSemantics repository and precise guidance for AI agents contributing changes. Keep this document concise, actionable, and up to date.

### What this repository contains

- **Solution**: `Code/AabSemantics.sln`
  - Central solution aggregating core library, modules, extensions, clients, samples, and tests.

- **Core**: `Code/Core/AabSemantics`
  - The main semantics engine: concepts, contexts, modules, statements, questions, answers, text, serialization, and utilities.
  - Published to NuGet as the `AabSemantics` package; it is the only project intended for publication. Its csproj owns the published `Version` (currently `2.3.0`), `PackageTags` and `PackageReleaseNotes` — bump them there, not in `Directory.Build.props`.
  - It also packs the repository root `README.md` (`PackageReadmeFile`), so edits to this document ship as the package description on nuget.org.
  - Subfolders of note:
    - `Interfaces/`: Public contracts; treat as API surface.
    - `Modules/`: Built-in `Boolean` and `Classification` modules and composition points.
    - `Serialization/`: `Xml` and `Json` DTOs and persistence wire formats; backwards compatibility matters.
    - `Text/`, `Localization/`: Text generation, localization, and structured text.
    - `Mutations/`, `Questions/`, `Statements/`: Inference, question processing, and consistency checking.

- **Core**: `Code/Core/Inventor.Algorithms`
  - Standalone graph and coding algorithms (Dijkstra, Ford-Fulkerson, Huffman) used independently of the semantics engine.

- **Modules**: `Code/Modules/*`
  - Optional domain modules that extend the core engine: `AabSemantics.Modules.Set`, `AabSemantics.Modules.Processes`, `AabSemantics.Modules.Mathematics`.
  - Each module has a matching test project under `Code/Tests/AabSemantics.Modules.<Name>.Tests`.

- **Extensions**: `Code/Extensions/*`
  - EF integration (`AabSemantics.Extensions.EF`) and WPF integration helpers (`AabSemantics.Extensions.WPF`).

- **Clients**: `Code/Clients/*`
  - `AabSemantics.SimpleRestClient`: Small controller-based ASP.NET Core Web API exposing semantics operations.
  - `AabSemantics.SimpleWpfClient`: WPF showcase UI.
  - Both clients reference `Code/Tests/AabSemantics.IntegrationTests` to seed their demo semantic network, so that test project is part of their build graph and must keep compiling for the clients to build.

- **Samples**: `Code/Samples/*`
  - Small console samples named `AabSemantics.Sample01..09`, demonstrating statements, questions, modules, customizations, productions, and EF usage.

- **Tests**: `Code/Tests/*`
  - Unit and integration tests across core and modules. Use them to verify behavioral compatibility.
  - `AabSemantics.TestCore` holds shared test infrastructure and fixture data reused by every other test project except `Inventor.Algorithms.Test`, which depends only on `Inventor.Algorithms`. It also carries a few tests of its own, so it runs as a test assembly too.
  - `AabSemantics.IntegrationTests` reuses fixtures from the module test projects, so it references them directly.

### Build and run

- Requires the **.NET 8 SDK**. Visual Studio 2022 (with the .NET desktop development workload) or the `dotnet` CLI both work; Visual Studio 2019 cannot build the `net8.0` projects.
- Build: `dotnet build Code/AabSemantics.sln`. NuGet restore happens automatically.
- Test: `dotnet test Code/AabSemantics.sln`.
- Run samples from `Code/Samples/*` to validate expected behavior after changes.
- Building the WPF client and WPF extension requires Windows; the rest of the solution is cross-platform.

### Technology notes

- All projects use the SDK-style project format with `PackageReference`. There is no `packages.config` and no `Code/packages` folder.
- Target frameworks by layer:
  - `netstandard2.0`: core (`AabSemantics`, `Inventor.Algorithms`) and all modules — keep them portable.
  - `net8.0`: EF extension, REST client, samples, and tests.
  - `net8.0-windows`: WPF client and WPF extension.
- Entity Framework 6.5.2 is referenced by `AabSemantics.Extensions.EF` and `Sample09`; `Sample09` also uses `System.Data.SqlClient`. Avoid uncoordinated major upgrades.
- `GenerateDocumentationFile` is on for `AabSemantics` and `AabSemantics.Extensions.EF`, so their XML docs ship alongside the assemblies. Give new public members there proper doc comments.
- The WPF client targets `net8.0-windows`, **not** .NET Framework.
- Shared metadata (`Authors`, `Company`, `Product`, `Copyright`, `Version`, license, repository URL) lives in the root `Directory.Build.props`. Do not repeat it in individual projects; override a property only when a project genuinely differs (as the core library does for `PackageId`, `Version` and `Description`). The root `Version` (`1.0.0`) is only a fallback for the non-published projects.
- `Code/Tests/Directory.Build.props` adds `IsPackable=false` and the NUnit / test SDK package references to every test project. It explicitly imports the root file, because MSBuild only applies the nearest `Directory.Build.props`. This is the only place `IsPackable` is set anywhere in the repository.
- The solution uses **Central Package Management**: every package version is declared once as a `PackageVersion` in the root `Directory.Packages.props`. A `PackageReference` in a project carries only `Include` (plus metadata such as `PrivateAssets`) and must **not** specify `Version` — doing so is an error under CPM.
- To add a package: add a `PackageVersion` to `Directory.Packages.props`, then reference it without a version from the project that needs it.
- Transitive pinning (`CentralPackageTransitivePinningEnabled`) is not set, so it stays at its default of off and transitive dependencies still resolve to whatever their parent package requires. Leave it that way unless a concrete need arises.

### Architectural overview

- The core (`AabSemantics`) defines the semantic network, concepts, statements, questions, answers, and text generation/localization.
- Modules extend the core with domain-specific statements, questions, and processing.
- Extensions integrate with EF and WPF for persistence and UI.
- Clients expose the engine via REST or present a WPF UX.

### Asynchrony and cancellation

The storage- and processing-facing contracts (`IRepository<T>`, `IQuestion`, `ISemanticNetwork`, `IStatement`, the context interfaces) follow one pattern; match it when adding to them:

- The interface member is asynchronous (`…Async`, returning `Task`/`Task<T>`) and takes a trailing `CancellationToken cancellationToken = default`, so a semantic network can be backed by a database as readily as by memory.
- A synchronous convenience wrapper lives in a sibling static extension class (for example `RepositoryExtensions` next to `IRepository<T>`). It observes the token up front, short-circuits to the synchronous path for the in-memory implementation, and otherwise blocks via `AabSemantics.Utils.TaskHelper`.
- Cancellation surfaces as `OperationCanceledException`; document it with an `<exception>` tag on the member.

### Backwards compatibility priorities

When modifying code, maintain compatibility in:

- Public contracts in `Code/Core/AabSemantics/Interfaces/*` and other public types that are consumed by modules/clients.
- Serialization contracts in `Code/Core/AabSemantics/Serialization/*` and any wire formats used by REST clients.
- Resource keys and localization structure in `Code/Core/AabSemantics/Localization`, the built-in modules' own `Localization` folders under `Code/Core/AabSemantics/Modules/*`, and `Code/Modules/*/Localization`.

Breaking changes must be isolated behind adapters or versioned DTOs. Prefer additive changes over mutating existing contracts.

### Testing expectations

- Tests use **NUnit 4**. Run them with `dotnet test Code/AabSemantics.sln` after any non-trivial change; all seven test assemblies must be green, and the full suite is fast (a few seconds).
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

- `Code/Core/AabSemantics/Interfaces`: Treat as stable API; prefer extension points over edits.
- `Code/Core/AabSemantics/Serialization`: Do not change existing DTO shapes without versioning and migration.
- `Code/Extensions/AabSemantics.Extensions.EF`: Ensure model mappings remain consistent; migrations should be explicit if added.
- `Code/Clients/*`: Keep endpoints stable; changing REST contracts requires versioning.
- The `Localization` folders under `Code/Core/AabSemantics` and `Code/Modules/*`: Preserve resource keys; adding is fine, renaming requires a cross-repo audit.

### Guidance for AI agents (Cursor / automated assistants)

Follow these rules strictly when making changes:

- Make the smallest viable edit that achieves the goal. Do not refactor unrelated code.
- Preserve existing indentation style and width; do not mix tabs and spaces.
- Maintain file encoding and line endings; do not introduce BOM changes.
- If adding new files, place them in the most specific directory (e.g., a new module under `Code/Modules/<ModuleName>`; a shared contract under `Code/Core/AabSemantics/Interfaces`).
- Prefer additive, backwards-compatible changes. Avoid breaking public APIs and serialization.
- Update or add tests for any behavioral change; run tests locally.
- Keep comments concise and only for non-obvious context.
- Do not upgrade third-party packages without an explicit instruction.
- Avoid introducing long-lived feature flags unless specified; keep configuration simple.
- For REST work, note that Swagger/Swashbuckle is registered and the Swagger UI is served only in the Development environment. XML documentation comments are **not** wired into it: the REST client project sets neither `GenerateDocumentationFile` nor `IncludeXmlComments`, so anything written in controller XML docs will not appear in the UI until both are added.

When uncertain:

- Search for usage across `Core`, `Modules`, `Extensions`, `Clients`, `Samples`, and `Tests` to assess impact.
- Prefer introducing new interfaces or overloads rather than mutating existing ones.

### How to add a new module (quick checklist)

1. Create a project under `Code/Modules/AabSemantics.Modules.<YourModuleName>` following existing module csproj patterns (`netstandard2.0`).
2. Define statements, questions, and answers types extending core abstractions.
3. Register module with the semantic network composition where needed.
4. Add unit tests under `Code/Tests/AabSemantics.Modules.<YourModuleName>.Tests`, reusing `AabSemantics.TestCore`.
5. Add a minimal sample under `Code/Samples/` if appropriate.
6. Add both new projects to `Code/AabSemantics.sln`, under the existing `Modules` and `Tests` solution folders.
7. If the module needs a package, declare its `PackageVersion` in the root `Directory.Packages.props` first, then reference it without a version.

### How to extend the REST client (quick checklist)

1. Add a new controller under `Code/Clients/AabSemantics.SimpleRestClient/Controllers`, following the existing `[ApiController, Route("[controller]")]` pattern and taking `ILogger<T>` and `IDataService` through the constructor.
2. Reuse core/module services; avoid duplicating business logic.
3. Ensure request/response models are versioned or additive.
4. Endpoints are discovered automatically by Swagger; to make XML doc comments show up there as well, first enable `GenerateDocumentationFile` in the project and pass `IncludeXmlComments` to `AddSwaggerGen` in `Program.cs`.

### License and provenance

- MIT. See `LICENSE` at the repository root; the core package declares the same via `PackageLicenseExpression` in `Directory.Build.props`.

### Document maintenance

- Keep this guide accurate with any project-wide or process changes.
- If major architecture evolves, update the overview and the “Areas that require extra care” section.


