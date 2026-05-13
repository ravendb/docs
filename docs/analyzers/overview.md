# RavenDB Roslyn Analyzers

RavenDB ships a set of Roslyn diagnostic analyzers embedded in the `RavenDB.Client` NuGet package. They run at compile time inside any project that references the client and catch common misuse patterns before tests, staging, or production.

## Distribution

Both analyzer assemblies (`Raven.Analyzers.dll` and `Raven.Analyzers.CodeFixes.dll`) are embedded in `RavenDB.Client.nupkg` under `analyzers/dotnet/cs/`. No separate install is required. The analyzers activate automatically on every build once the client package is referenced.

## Severity

All rules ship at **`DiagnosticSeverity.Info`** by default. This means they appear as informational hints in the IDE and in build output but **do not break existing builds on upgrade**.

Teams can promote individual rules to `Warning` or `Error` via `.editorconfig` once existing occurrences have been addressed:

```ini
[*.cs]
dotnet_diagnostic.RVN012.severity = warning
dotnet_diagnostic.RVN013.severity = error
```

## Code Fixes

Two rules ship with automated code fixes (IDE lightbulb / `dotnet fix`):

- **RVN011** — rewrites `store.OpenSession()` to `batch.OpenSession()` inside a subscription `Run` lambda.
- **RVN012** — rewrites a block of independent eager `Load` / `Query` calls into lazy registrations followed by a single `ExecuteAllPendingLazyOperations[Async]()` call.

## Rules

| ID | Title | Category | Code Fix |
|----|-------|----------|----------|
| [RVN001](RVN001.md) | Index Map or Reduce assigned outside constructor | Index definition | No |
| [RVN002](RVN002.md) | RavenDB query operator after projection | Query | No |
| [RVN003](RVN003.md) | ProjectInto called more than once in a query chain | Query | No |
| [RVN004](RVN004.md) | AbstractIndexCreationTask subclass is missing a Map assignment | Index definition | No |
| [RVN005](RVN005.md) | Multi-map index has no AddMap call in any constructor | Index definition | No |
| [RVN006](RVN006.md) | Multi-map index uses only a single AddMap | Index definition | No |
| [RVN007](RVN007.md) | Query field not present in the index projection | Query | No |
| [RVN008](RVN008.md) | Projected field not retrievable under the applied ProjectionBehavior | Query | No |
| [RVN009](RVN009.md) | Unsupported method call inside index Map/Reduce expression | Index definition | No |
| [RVN010](RVN010.md) | Unsupported method call inside RavenDB query expression | Query | No |
| [RVN011](RVN011.md) | Use batch.OpenSession inside a subscription Run delegate | Subscriptions | Yes |
| [RVN012](RVN012.md) | Batch independent session operations using the lazy API | Session | Yes |
| [RVN013](RVN013.md) | Query result is not bounded by Take() | Query | No |
| [RVN014](RVN014.md) | Index Map fans out over a collection | Index definition | No |
