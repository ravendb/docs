# RavenDB Documentation — Claude Context

## Project Overview
Official RavenDB documentation site. Built with **Docusaurus 3.9** + React 19 + TypeScript 5.6.
Serves 18+ product versions simultaneously across four content areas: main docs (current: 7.2), cloud, guides, templates.

---

## Key Commands
```bash
npm run start              # Dev server (all versions — slow on first run)
npm run start:current      # Dev server (current version only — use this for dev)
npm run build              # Production build
npm run build:current      # Production build (current version only)
npm run lint               # ESLint (src/ only)
npm run lint:fix           # Auto-fix lint issues
npm run prettier           # Check formatting
npm run prettier-fix       # Auto-format
npm run typecheck          # TypeScript validation
npm run generate-icon-types # Regenerate icon types after adding SVGs to static/icons/
```

---

## Directory Structure
```
docs/                        # Current (7.2) doc content — EDIT HERE
cloud/                       # RavenDB Cloud portal docs
guides/                      # Community guides (frontmatter-driven, tag-indexed)
templates/                   # Doc authoring templates
versioned_docs/version-X.Y/  # Historical version snapshots — DO NOT EDIT
versioned_sidebars/          # Auto-managed sidebar configs for old versions
src/
  components/                # 58 shared React/TSX components
  plugins/                   # Custom Docusaurus plugins (Tailwind, guides indexer)
  typescript/                # Generated types (iconName.ts) + custom frontmatter types
  css/custom.css             # Global styles
  pages/                     # Custom standalone pages
  theme/                     # Docusaurus theme overrides
scripts/                     # Build/deploy automation
static/icons/                # SVG icon assets (source for icon type generation)
sidebars.ts                  # Main docs sidebar config
sidebarsCloud.js             # Cloud docs sidebar
sidebarsGuides.js            # Guides sidebar
sidebarsTemplates.js         # Templates sidebar
docusaurus.config.ts         # Main Docusaurus config (versions, search, plugins)
versions.json                # Active version list
```

---

## Versioning Rules
- **Only edit `docs/`** — it is the current (7.2) version.
- `versioned_docs/` are read-only snapshots managed by Docusaurus.
- `versions.json` lists all active versions.
- CI uses `onlyIncludeVersions` env var to build specific versions (e.g., 6.2, 7.1, current).

---

## Content Authoring

### File Format
All content uses `.mdx` (Markdown + JSX).

### Frontmatter Schema
```yaml
title: "Page Title"
sidebar_label: "Shorter Sidebar Label"   # Optional
sidebar_position: 1                       # Order within parent directory
supported_languages: ["csharp", "java", "nodejs", "python", "php"]  # Optional
see_also:
  - title: "Related Page Title"
    link: "section/page-slug"
    source: "docs"                        # docs | cloud | guides | external
    path: "Client API > Session"          # Breadcrumb context string
```

### Multi-Language Code Pattern
API documentation pages support 5 languages: `csharp`, `java`, `nodejs`, `python`, `php`.

Structure:
```
docs/client-api/some-feature/
├── some-feature.mdx                          # Main page (imports partials)
└── content/
    ├── _some-feature-csharp.mdx
    ├── _some-feature-java.mdx
    └── _some-feature-nodejs.mdx
```

Usage in main `.mdx`:
```mdx
import LanguageSwitcher from "@site/src/components/LanguageSwitcher";
import LanguageContent from "@site/src/components/LanguageContent";
import SomeFeatureCsharp from './content/_some-feature-csharp.mdx';

<LanguageSwitcher supportedLanguages={frontMatter.supported_languages} />
<LanguageContent language="csharp">
  <SomeFeatureCsharp />
</LanguageContent>
```

### Category Ordering
Each folder contains `_category_.json`:
```json
{ "label": "Section Name", "position": 2 }
```

### Docusaurus Theme Components (available without import in MDX)
- `<Admonition type="note|warning|tip|info" title="">` — alert boxes
- `<Tabs>` + `<TabItem value="..." label="...">` — tabbed content
- `<CodeBlock language="csharp">` — syntax-highlighted code

---

## Key Custom Components (`src/components/`)
| Component | Purpose |
|---|---|
| `LanguageSwitcher.tsx` | Tab bar for language selection (reads `supported_languages`) |
| `LanguageContent.tsx` | Conditionally renders content per selected language |
| `LanguageStore.tsx` | React context holding the active language state |
| `SeeAlso/` | Auto-versioned cross-reference link block |
| `Icon.tsx` | SVG icon renderer — sizes: `xs` `sm` `md` `lg` `xl` |
| `LazyImage.tsx` | Lazy-loaded images with lightbox support |
| `SidebarVersionDropdown.tsx` | Version picker UI in sidebar |
| `DocsTopbar.tsx` | Top navigation bar |
| `CustomBreadcrumbs.tsx` | Breadcrumb navigation |

---

## Linting Scope
- ESLint and Prettier apply **only to `src/`**.
- `.mdx` doc content is excluded from lint checks.
- TypeScript strict mode applies to all `.ts`/`.tsx` in `src/`.

---

## Frontmatter Naming Convention
**CRITICAL RULE**: All custom frontmatter fields MUST use `snake_case`. Never use camelCase for custom frontmatter.
- ✅ Correct: `published_at`, `proficiency_level`, `external_url`, `img_alt`, `repository_url`, `license_url`
- ❌ Wrong: `publishedAt`, `proficiencyLevel`, `externalUrl`, `imgAlt`, `repositoryUrl`, `licenseUrl`
- Standard Docusaurus fields (e.g., `title`, `description`, `slug`) remain as-is.
- Tag values in guides and samples MUST use `kebab-case` (e.g., `vector-search`, `azure-storage-queues-etl`).

---

## Guides
- Tags defined in `guides/tags.yml` (~40 predefined tags — do not invent new ones without adding there first).
- Guide-specific frontmatter: `tags`, `description`, `icon`, `image`, `published_at` (ISO date), `external_url`, `proficiency_level`, `author`.
- Indexed and sorted by `src/plugins/recent-guides-plugin.ts`.

### Guides Frontmatter Example
```yaml
---
title: "Guide Title"
published_at: 2026-04-02
author: "Author Name"
tags: [ai, vector-search, getting-started]
description: "Short description shown in cards."
image: "/img/guides-example.webp"
proficiency_level: "Beginner"
---
```

For external guides (linking to blog posts):
```yaml
---
title: "External Article Title"
published_at: 2026-04-02
tags: [integration]
description: "Short description."
external_url: "https://ravendb.net/articles/example"
image: "https://ravendb.net/path/to/image.jpg"
---
```

---

## Samples
- Production-ready code samples demonstrating RavenDB features and architecture patterns.
- Located in `samples/` directory.
- Three-category tag system: Challenges & Solutions, Features, Tech Stack.
- Tags defined in `samples/tags/` YAML files — do not invent new ones without adding there first.
- Indexed by `src/plugins/recent-samples-plugin.ts`.
- Hub page at `/samples` with filtering by tags.

### Sample Tag Categories
1. **Challenges & Solutions** (`samples/tags/challenges-solutions.yml`) - Business problems the sample solves
   - Examples: `semantic-search`, `integration-patterns`, `cloud-tax`, `gen-ai-data-enrichment`

2. **Features** (`samples/tags/feature.yml`) - RavenDB features demonstrated
   - Examples: `vector-search`, `document-refresh`, `include`, `azure-storage-queues-etl`

3. **Tech Stack** (`samples/tags/tech-stack.yml`) - Technologies used
   - Examples: `csharp`, `aspire`, `azure-functions`, `nodejs`, `nextjs`

### Sample Frontmatter Example
```yaml
---
title: "Sample Application Name"
description: "Brief description for sample cards."
challenges_solutions_tags: [semantic-search, integration-patterns]
feature_tags: [vector-search, document-refresh, include]
tech_stack_tags: [csharp, aspire, azure-functions]
image: "/img/samples/my-sample/cover.webp"
img_alt: "Screenshot of the application"
category: "Ecommerce"
license: "MIT License"
license_url: "https://opensource.org/licenses/MIT"
repository_url: "https://github.com/ravendb/sample-repo"
demo_url: "https://demo.example.com"
languages: ["C#"]
gallery:
  - src: "/img/samples/my-sample/screenshot-1.webp"
    alt: "Main interface"
  - src: "/img/samples/my-sample/screenshot-2.webp"
    alt: "Admin dashboard"
related_resources:
  - type: documentation
    documentation_type: docs
    subtitle: "Vector Search Overview"
    article_key: "ai-integration/vector-search/overview"
  - type: guide
    subtitle: "Related Guide Title"
    article_key: "guide-slug"
---
```

**Required fields**: `title`, `description`, `challenges_solutions_tags`, `feature_tags`, `tech_stack_tags`

**Optional fields**: `image`, `img_alt`, `category`, `license`, `license_url`, `repository_url`, `demo_url`, `languages`, `gallery`, `related_resources`

**SEO**: `repository_url` and `languages` feed `SoftwareSourceCode` JSON-LD schema for better search visibility.

---

## Icon System
- SVG icons live in `static/icons/`.
- TypeScript types auto-generated into `src/typescript/iconName.ts`.
- After adding a new SVG, run `npm run generate-icon-types`.

---

## Content Map — What Lives Where

> Language-specific content partials (`content/_*.mdx`) are excluded from all listings below — they are implementation detail. Only the main navigable articles are listed.

---

### `docs/start/` — Installation & setup (~48 files)

- **Root:** `getting-started`, `about-examples`, `playground-server`, `test-driver`
- **`installation/`:** `system-requirements`, `system-configuration-recommendations`, `manual`, `running-as-service`, `upgrading-to-new-version`, `deployment-considerations`
- **`installation/gnu-linux/`:** `deb` (DEB package install)
- **`installation/setup-wizard/`:** `overview`, `use-setup-package`, `choose-setup-method`, `choose-security-option`, `provide-license-key`, `configure-node-addresses`, `additional-settings`, `review-setup`, `finish-setup`
- **`installation/setup-examples/`:** `aws-windows-vm`, `aws-linux-vm`
- **`installation/setup-examples/kubernetes/`:** `aws-eks`, `azure-aks`, `google-gke`
- **`containers/`:** `general-guide`, `image-usage`, `deployment-guides`
- **`containers/dockerfile/`:** `overview`, `guide`, `extending`
- **`containers/requirements/`:** `compute`, `storage`, `networking`, `security`, `licensing`
- **`guides/aws-lambda/`:** `overview`, `deployment`, `existing-project`, `secrets-manager`, `troubleshooting`
- **`guides/azure-functions/`:** `overview`, `deployment`, `existing-project`, `troubleshooting`
- **`guides/cloudflare-workers/`:** `overview`, `existing-project`, `troubleshooting`

---

### `docs/client-api/` — Client SDK (~189 files)

**Root:** `what-is-a-document-store`, `creating-document-store`, `setting-up-default-database`, `setting-up-authentication-and-authorization`, `net-client-versions`, `what-is-a-public-api`

- **`bulk-insert/`:** `how-to-work-with-bulk-insert-operation`
- **`changes/`:** `what-is-changes-api`, how-to-subscribe to document / index / operation / counter / time-series changes (5 files)
- **`cluster/`:** `how-client-integrates-with-replication-and-cluster`, `speed-test`, `health-check`, `document-conflicts-in-client-side`
- **`commands/`:** `overview`; **`batches/`:** `how-to-send-multiple-commands-using-a-batch`; **`documents/`:** `put`, `get`, `delete`
- **`configuration/`:** `conventions`, `serialization`, `deserialization`; **`identifier-generation/`:** `global`, `type-specific`; **`load-balance/`:** `overview`, `read-balance-behavior`, `load-balance-behavior`
- **`data-subscriptions/`:** `what-are-data-subscriptions`, `concurrent-subscriptions`; **`creation/`:** `how-to-create-data-subscription`, `examples`, `api-overview`; **`consumption/`:** `how-to-consume-data-subscription`, `examples`, `api-overview`; **`advanced-topics/`:** `subscription-with-revisioning`, `maintenance-operations`
- **`document-identifiers/`:** `working-with-document-identifiers`, `hilo-algorithm`
- **`faq/`:** `what-is-a-collection`, `transaction-support`, `backward-compatibility`
- **`how-to/`:** `setup-aggressive-caching`, `store-dates`, `subscribe-to-store-events`, `using-timeonly-and-dateonly`, `integrate-with-excel`, `handle-document-relationships`
- **`operations/`:** `what-are-operations`; **`common/`:** `delete-by-query`; **`counters/`:** `get-counters`, `counter-batch`; **`how-to/`:** `switch-operations-to-a-different-database`, `switch-operations-to-a-different-node`; **`patching/`:** `single-document`, `set-based`, `json-patch-syntax`
- **`operations/maintenance/`:** `get-stats`, `clean-change-vector`
  - **`backup/`:** `backup-overview`, `restore`, `encrypted-backup`, `faq`
  - **`configuration/`:** `database-settings-operation`, `get-client-configuration`, `put-client-configuration`
  - **`connection-strings/`:** `add-connection-string`, `get-connection-string`, `remove-connection-string`
  - **`etl/`:** `add-etl`, `update-etl`, `reset-etl`
  - **`identities/`:** `get-identities`, `seed-identity`, `increment-next-identity`
  - **`indexes/`:** `get-index`, `get-indexes`, `get-index-names`, `put-indexes`, `delete-index`, `enable-index`, `disable-index`, `start-index`, `stop-index`, `start-indexing`, `stop-indexing`, `set-index-lock`, `set-index-priority`, `index-has-changed`, `get-index-errors`, `delete-index-errors`, `get-terms`, `reset-index`
  - **`ongoing-tasks/`:** `ongoing-task-operations`
  - **`sorters/`:** `put-sorter`
- **`operations/server-wide/`:** `create-database`, `delete-database`, `add-database-node`, `promote-database-node`, `reorder-database-members`, `compact-database`, `restore-backup`, `get-build-number`, `get-database-names`, `toggle-databases-state`, `toggle-dynamic-database-distribution`, `modify-conflict-solver`
  - **`certificates/`:** `create-client-certificate`, `get-certificate`, `get-certificates`, `delete-certificate`, `put-client-certificate`
  - **`configuration/`:** `get-serverwide-client-configuration`, `put-serverwide-client-configuration`
  - **`logs/`:** `get-logs-configuration`, `set-logs-configuration`
  - **`sorters/`:** `put-sorter-server-wide`
- **`rest-api/`:** `rest-api-intro`; **`document-commands/`:** `put-documents`, `get-documents-by-id`, `get-all-documents`, `get-documents-by-prefix`, `delete-document`, `batch-commands`; **`queries/`:** `query-the-database`, `delete-by-query`, `patch-by-query`
- **`security/`:** `deserialization-security`
- **`session/`:** `what-is-a-session-and-how-does-it-work`, `opening-a-session`, `loading-entities`, `storing-entities`, `saving-changes`, `updating-entities`, `deleting-entities`
  - **`cluster-transaction/`:** `overview`
  - **`configuration/`:** `how-to-change-maximum-number-of-requests-per-session`, `how-to-customize-collection-assignment-for-entities`, `how-to-customize-id-generation-for-entities`, `how-to-customize-identity-property-lookup-for-entities`, `how-to-disable-tracking`, `how-to-disable-caching`, `how-to-enable-optimistic-concurrency`
  - **`how-to/`:** `check-if-document-exists`, `check-if-attachment-exists`, `check-if-entity-has-changed`, `check-if-there-are-any-changes-on-a-session`, `clear-a-session`, `evict-entity-from-a-session`, `defer-operations`, `get-entity-id`, `get-entity-change-vector`, `get-entity-last-modified`, `get-entity-counters`, `get-and-modify-entity-metadata`, `get-current-session-node`, `get-tracked-entities`, `refresh-entity`, `perform-operations-lazily`, `ignore-entity-changes`, `subscribe-to-events`
  - **`querying/`:** `how-to-query`, `how-to-count-query-results`, `how-to-filter-by-field`, `how-to-filter-by-non-existing-field`, `how-to-get-query-statistics`, `how-to-project-query-results`, `how-to-perform-a-faceted-search`, `how-to-perform-group-by-query`, `how-to-perform-queries-lazily`, `how-to-stream-query-results`, `how-to-use-intersect`, `how-to-use-morelikethis`, `how-to-work-with-suggestions`, `how-to-make-a-spatial-query`, `sort-query-results`, `how-to-customize-query`, `vector-search`
  - **`querying/text-search/`:** `full-text-search`, `boost-search-results`, `exact-match-query`, `starts-with-query`, `ends-with-query`, `fuzzy-search`, `proximity-search`, `highlight-query-results`, `using-regex`
  - **`querying/document-query/`:** `what-is-document-query`, `query-vs-document-query`, `how-to-use-lucene`, `how-to-use-not-operator`
  - **`querying/debugging/`:** `query-timings`, `include-explanations`
- **`smuggler/`:** `what-is-smuggler`

---

### `docs/server/` — Server-side (~109 files)

**Root:** `embedded` (Running an Embedded Instance), `tcp-compression`

- **`administration/`:** `cli`, `statistics`; **`monitoring/`:** `mib-generation`, `open-telemetry`, `prometheus`, `telegraf`; **`snmp/`:** `snmp-overview`, `setup-zabbix`
- **`clustering/`:** `overview`, `cluster-api`, `cluster-best-practice-and-configuration`, `cluster-transactions`
  - **`distribution/`:** `cluster-observer`, `distributed-database`, `highly-available-tasks`
  - **`rachis/`:** `what-is-rachis`, `cluster-topology`, `consensus-operations`
  - **`replication/`:** `replication-overview`, `replication-conflicts`, `change-vector`, `advanced-replication`, `replication-and-embedded-instance`
- **`configuration/`:** `configuration-options` (main overview) + per-topic config files for: `ai-integration`, `backup`, `cluster`, `command-line-arguments`, `core`, `database`, `embedded`, `etl`, `http`, `indexing`, `license`, `logs`, `memory`, `monitoring`, `patching`, `performance-hints`, `query`, `queue-sink`, `replication`, `security`, `server`, `storage`, `studio`, `subscription`, `tombstone`, `traffic-watch`, `transaction-merger`, `updates` (28 files total)
- **`extensions/`:** `expiration` (Document Expiration), `refresh` (Document Refresh)
- **`kb/`:** `document-identifier-generation`, `javascript-engine`, `linux-setting-limits`, `linux-setting-memlock`, `numbers-in-ravendb`
- **`ongoing-tasks/`:** `backup-overview`, `external-replication`, `hub-sink-replication`
  - **`etl/`:** `basics`, `raven`, `sql`, `elasticsearch`, `olap`, `snowflake`, `test-scripts`
  - **`etl/queue-etl/`:** `overview`, `kafka`, `rabbit-mq`, `azure-queue`, `amazon-sqs`
  - **`queue-sink/`:** `overview`, `kafka-queue-sink`, `rabbit-mq-queue-sink`
- **`security/`:** `overview`, `common-errors-and-faq`, `fiddler-usage-with-secured-database`
  - **`audit-log/`:** `audit-log-overview`
  - **`authentication/`:** `certificate-configuration`, `certificate-management`, `certificate-renewal-and-rotation`, `client-certificate-usage`, `lets-encrypt-certificates`, `solve-cluster-certificate-renewal-issue`
  - **`authorization/`:** `security-clearance-and-permissions`
  - **`encryption/`:** `encryption-at-rest`, `database-encryption`, `server-store-encryption`, `secret-key-management`
- **`storage/`:** `storage-engine` (Voron), `directory-structure`, `documents-compression`, `customizing-raven-data-files-locations`
- **`troubleshooting/`:** `collect-info`, `debug-routes`, `logging`, `voron-recovery-tool`

---

### `docs/studio/` — Studio web UI (~90 files)

**Root:** `overview`, `ai-assistant`, `configuration`

- **`cluster/`:** `cluster-view`, `setting-a-cluster`; **`cluster-dashboard/`:** `cluster-dashboard-overview`, `cluster-dashboard-customize`, `cluster-dashboard-widgets`
- **`database/`:** `databases-list-view`
  - **`create-new-database/`:** `general-flow`, `encrypted`, `from-backup`, `from-legacy-files`
  - **`document-extensions/`:** `counters`, `time-series`; **`revisions/`:** `revisions-overview`, `all-revisions`, `revisions-bin`
  - **`documents/`:** `documents-and-collections`, `document-view`, `create-new-document`, `conflicts-view`, `identities-view`, `patch-view`
  - **`indexes/`:** `indexes-overview`, `indexes-list-view`, `create-map-index`, `create-map-reduce-index`, `create-multi-map-index`, `index-cleanup`, `index-history`, `indexing-performance`, `map-reduce-visualizer`
  - **`queries/`:** `query-view`, `spatial-queries-map-view`
  - **`settings/`:** `database-settings`, `database-record`, `manage-database-group`, `conflict-resolution`, `document-revisions`, `document-expiration`, `document-refresh`, `document-archival`, `documents-compression`, `time-series-settings`, `custom-analyzers`, `custom-sorters`, `client-configuration-per-database`, `studio-configuration`, `integrations`
  - **`stats/`:** `storage-report`; **`stats/ongoing-tasks-stats/`:** `overview`, `external-replication-stats`, `olap-etl-stats`, `ravendb-etl-stats`, `sql-etl-stats`, `subscription-stats`
  - **`tasks/`:** `backup-task`, `create-sample-data`, `export-database`
  - **`tasks/import-data/`:** `import-data-file`, `import-from-csv`, `import-from-other`, `import-from-ravendb`, `import-from-sql`
  - **`tasks/ongoing-tasks/`:** `general-info`, `external-replication-task`, `ravendb-etl-task`, `sql-etl-task`, `olap-etl-task`, `elasticsearch-etl-task`, `kafka-etl-task`, `rabbitmq-etl-task`, `amazon-sqs-etl`, `azure-queue-storage-etl`, `kafka-queue-sink`, `rabbitmq-queue-sink`, `subscription-task`
  - **`tasks/ongoing-tasks/hub-sink-replication/`:** `overview`, `replication-hub-task`, `replication-sink-task`
- **`server/`:** `manage-server`, `server-settings`, `server-wide-backup`, `client-configuration`
  - **`certificates/`:** `server-management-certificates-view`, `read-only-access-level`
  - **`debug/`:** `admin-js-console`, `admin-logs`; **`debug/advanced/`:** `cluster-observer`

---

### `docs/indexes/` — Indexing (~47 files)

**Root (30 files):** `what-are-indexes`, `indexing-basics`, `map-indexes`, `map-reduce-indexes`, `multi-map-indexes`, `javascript-indexes`, `creating-and-deploying`, `rolling-index-deployment`, `index-administration`, `index-throttling`, `stale-indexes`, `storing-data-in-index`, `using-analyzers`, `using-dynamic-fields`, `using-term-vectors`, `sorting-and-collation`, `boosting`, `indexing-related-documents`, `indexing-hierarchical-data`, `indexing-nested-data`, `indexing-polymorphic-data`, `indexing-spatial-data`, `indexing-attachments`, `indexing-counters`, `indexing-time-series`, `indexing-metadata`, `indexing-linq-extensions`, `number-type-conversion`, `additional-assemblies`, `extending-indexes`

- **`querying/` (16 files):** `query-index`, `filtering`, `sorting`, `projections`, `paging`, `searching` (full-text), `highlighting`, `faceted-search`, `suggestions`, `spatial`, `distinct`, `intersection`, `morelikethis`, `exploration-queries`, `include-explanations`, `vector-search`
- **`search-engine/`:** `corax`
- **`troubleshooting/`:** `debugging-index-errors`

---

### `docs/querying/` — RQL querying

**Root:** `overview` — covers RQL syntax, dynamic queries, projections, spatial queries

---

### `docs/document-extensions/` — Document extensions (~69 files)

**Root:** `overview`

- **`attachments/`:** `overview`, `get-attachments`, `delete-attachment`, `copy-move-rename`, `indexing-attachments`, `attachments-and-other-features`, `configure-remote-attachments`
  - **`store-attachments/`:** `store-attachments-local`, `store-attachments-remote`, `bulk-insert`
- **`counters/`:** `overview`, `create-or-modify`, `retrieve-counter-values`, `delete`, `including-counters`, `indexing`, `counters-in-clusters`, `counters-and-other-features`
- **`revisions/`:** `overview`, `revisions-and-other-features`, `revisions-bin-cleaner`, `troubleshooting`
  - **`client-api/`:** `overview`; **`client-api/operations/`:** `configure-revisions`, `conflict-revisions-configuration`, `get-revisions`, `delete-revisions`; **`client-api/session/`:** `loading`, `counting`, `including`
  - **`revert-documents-to-revisions/`:** `revert-documents-to-specific-revisions`, `revert-documents-to-specific-time`
- **`timeseries/`:** `overview`, `design`, `rollup-and-retention`, `indexing`, `time-series-and-other-features`
  - **`client-api/`:** `overview`, `named-time-series-values`, `javascript-support`
  - **`client-api/bulk-insert/`:** `append-in-bulk`
  - **`client-api/operations/`:** `append-and-delete`, `get`, `patch`
  - **`client-api/session/`:** `append`, `delete`, `patch`, `querying`; **`session/get/`:** `get-entries`, `get-names`; **`session/include/`:** `overview`, `with-session-load`, `with-session-query`, `with-raw-queries`
  - **`querying/`:** `overview-and-syntax`, `choosing-query-range`, `filtering`, `aggregation-and-projections`, `gap-filling`, `statistics`, `stream-timeseries`, `using-indexes`
- **`timeseries/incremental-time-series/`:** `overview`; **`client-api/`:** `javascript-support`; **`client-api/operations/`:** `get`; **`client-api/session/`:** `overview`, `increment`, `get`, `delete`

---

### `docs/compare-exchange/` — Cluster-wide atomic operations (~15 files)

**Root:** `overview`, `start`, `create-cmpxchg-items`, `get-cmpxchg-item`, `get-cmpxchg-items`, `update-cmpxchg-item`, `delete-cmpxchg-items`, `include-cmpxchg-items`, `atomic-guards`, `cmpxchg-expiration`, `cmpxchg-in-dynamic-queries`, `indexing-cmpxchg-values`, `configuration`

- **`api-studio-quick-links/`:** `client-api-references`, `studio-references`

---

### `docs/ai-integration/` — AI & vector search (~36 files)

**Root:** `start`, `ai-tasks-list-view`

- **`connection-strings/`:** `overview`, `open-ai`, `azure-open-ai`, `google-ai`, `vertex-ai`, `hugging-face`, `mistral-ai`, `ollama`, `embedded` (bge-micro-v2)
- **`vector-search/`:** `overview`, `start`, `data-types-for-vector-search`, `vector-search-using-dynamic-query`, `vector-search-using-static-index`, `indexing-attachments-for-vector-search`, `what-affects-vector-search-results`
- **`generating-embeddings/`:** `overview`, `start`, `embeddings-generation-task`, `embedding-collections`
- **`gen-ai-integration/`:** `overview`, `start`, `gen-ai-security-concerns`; **`create-gen-ai-task/`:** `api`, `studio`; **`modify-gen-ai-task/`:** `api`, `studio`; **`process-attachments/`:** `api`, `studio`
- **`ai-agents/`:** `overview`, `start`, `ai-agents_configuration`, `ai-agents_security-concerns`; **`creating-ai-agents/`:** `api`, `studio`

---

### `docs/sharding/` — Horizontal sharding (~17 files)

**Root:** `overview`, `migration`, `indexing`, `querying`, `subscriptions`, `etl`, `external-replication`, `document-extensions`, `import-and-export`, `resharding`, `unsupported`

- **`administration/`:** `api-admin`, `studio-admin`, `anchoring-documents`, `sharding-by-prefix`
- **`backup-and-restore/`:** `backup`, `restore`

---

### `docs/data-archival/` — Data archival (~5 files)

`overview`, `enable-data-archiving`, `schedule-document-archiving`, `unarchiving-documents`, `archived-documents-and-other-features`

---

### `docs/migration/` — Version migration (~7 files)

- **`client-api/`:** `client-migration`, `previous-versions-client-breaking-changes`
- **`server/`:** `data-migration`, `legacy-versions-data-migration`, `server-breaking-changes`, `previous-versions-server-breaking-changes`, `docker`

---

### `docs/integrations/` — Third-party integrations (~6 files)

**Root:** `terraform`

- **`akka.net-persistence/`:** `integrating-with-akka-persistence`, `events-and-snapshots`, `queries`
- **`postgresql-protocol/`:** `overview`, `power-bi`

---

### `docs/documents/` — Document schema validation (~6 files)

- **`schema-validation/`:** `overview`, `configuration`; **`write-validation/`:** `api`, `studio`; **`auditing-document-compliance/`:** `api`, `studio`

---

### `docs/glossary/` — Definitions & API reference types (~21 files)

`etag`, `tombstone`, `replication-factor`, `raft-algorithm`, `raft-command`, `ravendb-cluster`, `cluster-node`, `node-tag`, `database-group`, `database-id`, `index-query`, `query-result`, `stream-result`, `stream-query-statistics`, `blittable-json-reader-object`, `put-command-data`, `delete-command-data`, `patch-command-data`, `copy-attachment-command-data`, `move-attachment-command-data`, `counters-batch-command-data`

---

### `docs/licensing/` — License management (~8 files)

`overview`, `activate-license`, `renew-license`, `replace-license`, `force-update`, `configuration`, `license-under-docker`, `faq`

---

### `docs/users-issues/` — Troubleshooting specific issues (~6 files)

`azure-router-timeout`, `tcp-offloading`, `recovering-from-voron-errors`, `understanding-eventual-consistency`, `emergency-access`, `aspire-aca-plugin`

---

### `cloud/` — RavenDB Cloud service documentation (~24 files)
Account management, instance configuration, security (TLS, MFA, certificates), pricing/billing, scaling, backup/restore, migration, AWS/Azure Marketplace setup, and the cloud portal UI (home, products, billing, backups, support tabs).

### `guides/` — Practical how-to guides (~64 files, flat structure)
Community guides covering: connecting specific frameworks (ASP.NET Core, Next.js, SvelteKit, FastAPI), AI/ML integration, DevOps (Docker, Kubernetes/EKS, Helm, Ansible), observability (Datadog, Grafana/OpenTelemetry), data pipelines (Elasticsearch, Azure Queue, OLAP ETL), testing (unit test drivers for .NET/Java/Python), and troubleshooting specific problems.

**Frontmatter**: `title`, `published_at`, `author`, `tags`, `description`, `icon`, `image`, `proficiency_level`, `external_url` (for external guides).

**Tags**: Defined in `guides/tags.yml` (~40 predefined tags — do not invent new ones without adding there first). All tag values use kebab-case.

**Indexed by**: `src/plugins/recent-guides-plugin.ts`

### `samples/` — Production-ready code samples (~1+ files)
Production-ready code samples, architecture patterns, and starter kits demonstrating RavenDB features and integration scenarios. Hub page at `/samples` with tag-based filtering.

**Frontmatter**: `title`, `description`, `challenges_solutions_tags`, `feature_tags`, `tech_stack_tags`, `image`, `img_alt`, `category`, `license`, `license_url`, `repository_url`, `languages`, `gallery`.

**Tags**: Three categories defined in `samples/tags/`:
- `challenges-solutions.yml` - Business problems solved (e.g., `semantic-search`, `integration-patterns`)
- `feature.yml` - RavenDB features demonstrated (e.g., `vector-search`, `document-refresh`)
- `tech-stack.yml` - Technologies used (e.g., `csharp`, `aspire`, `azure-functions`)

All tag values use kebab-case.

**Indexed by**: `src/plugins/recent-samples-plugin.ts`

### `templates/` — Authoring reference templates (~9 files)
Style guide and live examples for documentation building blocks: ContentFrame/Panel layouts, icon gallery, themed images, tag reference, see-also cross-links, featured/new guide blocks, sample authoring templates.

---

## Tech Stack Summary
- Docusaurus 3.9.2 · React 19 · TypeScript 5.6
- Tailwind CSS 4.1 (via custom PostCSS plugin)
- Algolia search · Google Tag Manager · Sitemap auto-generation
- Prism React Renderer for code syntax highlighting
- ESLint 9 · Prettier 3.6
