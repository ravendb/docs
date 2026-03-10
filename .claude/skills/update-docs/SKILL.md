---
name: update-docs
description: >
  Analyze a GitHub PR from ravendb/ravendb and update RavenDB documentation accordingly.
  Use when you have a product PR URL and need to update docs to match code changes.
argument-hint: <github-pr-url-or-number>
disable-model-invocation: true
allowed-tools: Read, Write, Edit, Bash(gh *), Bash(git *), Bash(mkdir *), Bash(rm -rf /tmp/ravendb-source), Glob, Grep
---

# Update RavenDB Documentation from Product PR

You are updating the RavenDB documentation (`ravendb/docs`) based on code changes in a GitHub PR from the product repository (`ravendb/ravendb`).

## Phase 1 — Parse Input

Extract the PR number from `$ARGUMENTS`. Accept these formats:
- Full URL: `https://github.com/ravendb/ravendb/pull/22442`
- Shorthand: `ravendb/ravendb#22442`
- Number only: `22442` (assumes `ravendb/ravendb`)

**Important**: The working directory is the `ravendb/docs` repo. Always use `--repo ravendb/ravendb` with all `gh` commands.

## Phase 2 — Fetch PR Metadata

Run these commands to gather context without flooding the context window:

```bash
# Get structured PR metadata
gh pr view <NUMBER> --repo ravendb/ravendb --json title,body,labels,state,baseRefName,files,additions,deletions,commits

# Get list of changed files (no diff content yet)
gh pr diff <NUMBER> --repo ravendb/ravendb --name-only
```

If `gh` fails with an authentication error, stop and tell the user to run `gh auth login`.

Read the PR title and description carefully — they often explain what changed and why.

## Phase 2.5 — Clone PR Source Code

Clone the full product repository so you have complete source code access for context.
Use a blobless clone — it downloads file contents lazily (only when you read them), making the initial clone fast.

```bash
# Remove any previous clone
rm -rf /tmp/ravendb-source

# Blobless clone (fast — file contents downloaded on demand)
git clone --filter=blob:none https://github.com/ravendb/ravendb.git /tmp/ravendb-source

# Checkout the PR branch
cd /tmp/ravendb-source && gh pr checkout <NUMBER>
```

After cloning, you have **full access** to the entire product codebase at `/tmp/ravendb-source/`.
Use `Read`, `Glob`, and `Grep` on this path to:
- Read complete source files (not just diffs)
- Browse related/unchanged code for context (base classes, interfaces, callers)
- Search the entire codebase for patterns, usages, and references
- Read test files for real usage examples and expected behavior

**Important**: The docs repo remains your working directory (`D:\workspaces\docs`). The cloned product repo is read-only reference material at `/tmp/ravendb-source/`.

## Phase 3 — Classify Changes

### Code-to-Docs Mapping

Use this table to map product code paths to documentation areas:

| Product Code Path | Documentation Area |
|---|---|
| `src/Raven.Client/Documents/Session/` | `docs/client-api/session/` |
| `src/Raven.Client/Documents/Session/Querying/` | `docs/client-api/session/querying/` |
| `src/Raven.Client/Documents/Operations/` | `docs/client-api/operations/` |
| `src/Raven.Client/Documents/Queries/` | `docs/client-api/session/querying/` and `docs/querying/` |
| `src/Raven.Client/Documents/Linq/` | `docs/client-api/session/querying/` |
| `src/Raven.Client/Documents/Subscriptions/` | `docs/client-api/data-subscriptions/` |
| `src/Raven.Client/Documents/BulkInsert/` | `docs/client-api/bulk-insert/` |
| `src/Raven.Client/Documents/Changes/` | `docs/client-api/changes/` |
| `src/Raven.Client/Documents/Smuggler/` | `docs/client-api/smuggler/` |
| `src/Raven.Client/Documents/Conventions/` | `docs/client-api/configuration/conventions` |
| `src/Raven.Client/Documents/Identity/` | `docs/client-api/document-identifiers/` |
| `src/Raven.Client/Documents/Commands/` | `docs/client-api/commands/` |
| `src/Raven.Client/Http/` | `docs/client-api/configuration/` |
| `src/Raven.Server/Documents/Indexes/` | `docs/indexes/` |
| `src/Raven.Server/Documents/Queries/` | `docs/querying/` and `docs/indexes/querying/` |
| `src/Raven.Server/Documents/ETL/` | `docs/server/ongoing-tasks/etl/` |
| `src/Raven.Server/Documents/Replication/` | `docs/server/clustering/replication/` |
| `src/Raven.Server/Documents/TimeSeries/` | `docs/document-extensions/timeseries/` |
| `src/Raven.Server/Documents/Revisions/` | `docs/document-extensions/revisions/` |
| `src/Raven.Server/Documents/Counters/` | `docs/document-extensions/counters/` |
| `src/Raven.Server/Documents/Attachments/` | `docs/document-extensions/attachments/` |
| `src/Raven.Server/Documents/CompareExchange/` | `docs/compare-exchange/` |
| `src/Raven.Server/Documents/Sharding/` | `docs/sharding/` |
| `src/Raven.Server/Documents/DataArchival/` | `docs/data-archival/` |
| `src/Raven.Server/Documents/QueueSink/` | `docs/server/ongoing-tasks/queue-sink/` |
| `src/Raven.Server/ServerWide/` | `docs/server/` |
| `src/Raven.Server/Config/` | `docs/server/configuration/` |
| `src/Raven.Server/Web/` | `docs/client-api/rest-api/` |
| `src/Raven.Server/Commercial/` | `docs/licensing/` |
| `src/Raven.Server/Integrations/` | `docs/integrations/` |
| `src/Raven.Server/Documents/Schemas/` | `docs/documents/schema-validation/` |
| `src/Raven.Studio/` | `docs/studio/` |
| `src/Corax/` | `docs/indexes/search-engine/` |
| `src/Voron/` | `docs/server/storage/` |
| `src/Raven.Embedded/` | `docs/server/embedded` |
| `src/Raven.Server/Utils/AiIntegration/` | `docs/ai-integration/` |
| `src/Raven.Client/Documents/Session/Tokens/` | `docs/client-api/session/querying/` |
| `src/Raven.Server/Documents/Queries/AST/` | `docs/querying/` |
| `test/` | Use as code examples / usage reference (do not create docs for tests) |

### Change Type Classification

Based on the changed files, classify the PR as one or more of:

- **New public API**: New class/method in `Raven.Client` — likely needs a new doc page or section addition
- **Modified API signature**: Changed parameters, return types — update existing API documentation and code samples
- **New configuration key**: Changes in `Config/` — add to relevant page in `docs/server/configuration/`
- **New RQL function/syntax**: Changes in `Queries/` — update `docs/querying/` and relevant query pages
- **Bug fix**: Test + small server changes — usually no doc update unless it corrects documented behavior
- **Behavior change**: Existing feature works differently — update existing docs to reflect new behavior
- **Studio UI change**: TypeScript/React in `Raven.Studio` — update `docs/studio/` pages
- **New feature (cross-cutting)**: Changes across client + server — may need an entirely new documentation section
- **Internal refactoring / test-only**: Likely no documentation needed — report this and skip

## Phase 4 — Present Analysis (REQUIRED)

**Before making any changes**, present the following to the user:

1. **PR Summary**: Title and brief description of what the PR does
2. **Change Classification**: Type of change (from the list above)
3. **Affected Documentation**: List of doc files/areas that need updating (mapped from code paths)
4. **Proposed Updates**: What specifically should be added, modified, or created in each file
5. **Gaps**: Note anything that cannot be fully addressed (e.g., "This PR adds a C# API — samples for Java, Node.js, Python, and PHP will need to be added separately")

**Ask the user to confirm or adjust the plan before proceeding to Phase 5.**

## Phase 5 — Explore Source Code

After user confirmation, use the cloned repo at `/tmp/ravendb-source/` to deeply understand the changes.

### Read the changed files in full
For each file identified in Phase 2, read the complete source file (not just the diff) to understand the full API surface:
```
Read /tmp/ravendb-source/src/Raven.Client/Documents/Session/SomeNewFeature.cs
```

### Browse related code for context
Use `Glob` and `Grep` on `/tmp/ravendb-source/` to find:
- Base classes, interfaces, and abstract types that changed code extends
- Other callers or usages of the modified API
- Configuration key registrations and default values
- Related test files showing usage patterns

### Get a high-level diff overview
Use the base branch from the PR metadata (`baseRefName` from Phase 2) — it may be `v6.2`, `v7.1`, etc., not necessarily `main`:
```bash
cd /tmp/ravendb-source && git diff origin/<BASE_BRANCH>...HEAD --stat
```

### Prioritize reading
1. **Public API surface** — `Raven.Client` types, method signatures, new classes
2. **Configuration definitions** — new config keys, default values, descriptions
3. **Query/RQL changes** — new functions, syntax additions
4. **Test files** — use as real-world usage examples for documentation code samples

## Phase 6 — Read Existing Documentation and Study Style

Read the documentation files identified in Phase 3, **plus 2–3 sibling pages in the same directory** to use as style references. The goal is to match the look and feel of existing documentation exactly.

### Read target pages
- Current structure, headings, and content
- Frontmatter fields (especially `see_also` cross-references)
- Whether the page uses multi-language patterns
- What components are imported

### Study sibling pages as style templates
Read nearby pages in the same documentation section (same parent folder) to learn:
- **Heading hierarchy** — what heading levels are used and in what order (e.g., do they start with a summary paragraph? An admonition? A code example?)
- **Tone and voice** — formal vs conversational, second person ("you") vs third person, imperative vs descriptive
- **Section patterns** — do pages in this section follow a consistent structure? (e.g., Overview → Syntax → Example → Related)
- **Code sample style** — inline code vs `<CodeBlock>`, length of examples, how parameters are documented, whether examples include comments
- **Component usage** — which components (Admonition, Tabs, CodeBlock, etc.) are used and how
- **Level of detail** — how much explanation accompanies code samples, how deeply parameters/options are described
- **Cross-reference patterns** — how `see_also` is structured, inline links to related pages

When creating or editing pages, **replicate the patterns found in sibling pages**. New content should be indistinguishable in style from existing content in the same section.

### Find related pages
Use `Glob` and `Grep` to find additional pages that mention the affected feature across the docs.

## Phase 7 — Make Documentation Updates

**Style rule**: Match the look and feel of sibling pages studied in Phase 6. Use the same heading hierarchy, section order, tone, code sample style, and component patterns. New or updated content should read as if the same author wrote the entire section.

Apply changes following these conventions:

### File Format
- All content files use `.mdx` extension
- YAML frontmatter is required: `title` and `sidebar_position` at minimum
- Optional frontmatter: `sidebar_label`, `supported_languages`, `see_also`

### Multi-Language API Pages
For pages that show API usage in multiple languages:

```
docs/client-api/some-feature/
├── some-feature.mdx              # Main page (imports partials)
└── content/
    ├── _some-feature-csharp.mdx
    ├── _some-feature-java.mdx
    └── _some-feature-nodejs.mdx
```

Main page pattern:
```mdx
import LanguageSwitcher from "@site/src/components/LanguageSwitcher";
import LanguageContent from "@site/src/components/LanguageContent";
import SomeFeatureCsharp from './content/_some-feature-csharp.mdx';

<LanguageSwitcher supportedLanguages={frontMatter.supported_languages} />
<LanguageContent language="csharp">
  <SomeFeatureCsharp />
</LanguageContent>
```

Supported languages: `csharp`, `java`, `nodejs`, `python`, `php`

### Components Available in MDX
- `<Admonition type="note|warning|tip|info" title="">` — alert boxes
- `<Tabs>` + `<TabItem value="..." label="...">` — tabbed content
- `<CodeBlock language="csharp">` — syntax-highlighted code

### Structural Rules
- New folders need a `_category_.json` with `label` and `position`
- Sidebars use `autogenerated` from `dirName` — new files in existing directories are auto-discovered
- **Only edit files in `docs/`** — never touch `versioned_docs/`
- Add `see_also` cross-references in frontmatter when connecting to related documentation

## Phase 8 — Post-Update Summary

After making changes:

### Clean up the cloned repo
```bash
rm -rf /tmp/ravendb-source
```

### Provide a summary
1. **Files modified**: List of all files created or edited with brief description of changes
2. **Files created**: Any new pages with their sidebar position and parent section
3. **Remaining TODOs**: Missing language samples, screenshots needed, related pages to update
4. **Preview command**: Suggest running `npm run start:current` to preview the documentation changes locally

## Edge Cases

- **PR is not docs-relevant** (pure refactoring, test-only, internal optimization): Report "No documentation updates needed for this PR" with a brief explanation of why, and stop.
- **Very large PR** (50+ files changed): Focus on public API surface changes. Skip internal implementation details. Summarize what was skipped.
- **PR targets older version** (labels like `v6.0`, `v6.2`): Warn the user that this docs repo only edits `docs/` which is the current version (7.2). The user must decide whether the change applies to current docs.
- **New C# API without other languages**: Create the C# partial. Flag Java, Node.js, Python, and PHP as TODOs that need separate implementation.
- **Feature not yet documented**: Note that no existing documentation was found. Propose creating a new page with appropriate structure and sidebar position.
