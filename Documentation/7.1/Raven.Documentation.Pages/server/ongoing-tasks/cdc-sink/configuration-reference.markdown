# CDC Sink: Configuration Reference
---

{NOTE: }

* This page documents all configuration classes used to define a CDC Sink task.

* In this page:
  * [CdcSinkConfiguration](../../../server/ongoing-tasks/cdc-sink/configuration-reference#cdcsinkconfiguration)
  * [CdcSinkPostgresSettings](../../../server/ongoing-tasks/cdc-sink/configuration-reference#cdcsinkpostgressettings)
  * [CdcSinkTableConfig](../../../server/ongoing-tasks/cdc-sink/configuration-reference#cdcsinktableconfig)
  * [CdcSinkEmbeddedTableConfig](../../../server/ongoing-tasks/cdc-sink/configuration-reference#cdcsinkembeddedtableconfig)
  * [CdcSinkLinkedTableConfig](../../../server/ongoing-tasks/cdc-sink/configuration-reference#cdcsinklinkedtableconfig)
  * [CdcSinkOnDeleteConfig](../../../server/ongoing-tasks/cdc-sink/configuration-reference#cdcsinkondeleteconfig)
  * [CdcSinkRelationType](../../../server/ongoing-tasks/cdc-sink/configuration-reference#cdcsinkrelationtype)

{NOTE/}

---

{PANEL: CdcSinkConfiguration}

The top-level configuration object for a CDC Sink task.

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Name` | `string` | ✓ | Unique task name |
| `ConnectionStringName` | `string` | ✓ | Name of the SQL connection string |
| `Tables` | `List<CdcSinkTableConfig>` | ✓ | Root table configurations (at least one required) |
| `Postgres` | `CdcSinkPostgresSettings` | | PostgreSQL-specific settings (slot and publication names) |
| `SkipInitialLoad` | `bool` | | When `true`, skip the initial full-table scan and start streaming CDC changes immediately. Only applies on first startup — has no effect once the initial load has completed. Default: `false` |
| `Disabled` | `bool` | | Pause the task without deleting it. Default: `false` |
| `MentorNode` | `string` | | Preferred cluster node for execution |
| `PinToMentorNode` | `bool` | | Pin the task to the mentor node. Default: `false` |
| `TaskId` | `long` | | Set by the server on creation |

{PANEL/}

---

{PANEL: CdcSinkPostgresSettings}

PostgreSQL-specific settings. Assigned to `CdcSinkConfiguration.Postgres`.
Leave `null` for non-PostgreSQL connections.

| Property | Type | Description |
|----------|------|-------------|
| `SlotName` | `string` | Name of the PostgreSQL logical replication slot. If omitted on creation, a deterministic hash-based name is used. Immutable once set. Max 63 characters, alphanumeric and underscores only. |
| `PublicationName` | `string` | Name of the PostgreSQL publication. Same auto-fill and immutability rules as `SlotName`. |

Setting these explicitly is useful when:
- A database administrator pre-creates the slot and publication with human-readable names
- Migrating from a previous CDC Sink task and reusing an existing slot
- Running multiple environments (dev/staging/prod) with predictable names

See [Initial Setup](../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup) for details.

{NOTE: }
For embedded tables where the join columns are not part of the primary key, the
PostgreSQL table must have `REPLICA IDENTITY` configured so that DELETE events include
the join column values. See [REPLICA IDENTITY](../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity).
{NOTE/}

{PANEL/}

---

{PANEL: CdcSinkTableConfig}

Configures a root table — one SQL table mapped to one RavenDB collection.

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Name` | `string` | ✓ | RavenDB collection name (e.g., `"Orders"`) |
| `SourceTableName` | `string` | ✓ | SQL table name (e.g., `"orders"`) |
| `SourceTableSchema` | `string` | | SQL schema name. Default: `"public"` |
| `PrimaryKeyColumns` | `List<string>` | ✓ | SQL columns used for document ID generation |
| `ColumnsMapping` | `Dictionary<string, string>` | ✓ | SQL column → document property |
| `AttachmentNameMapping` | `Dictionary<string, string>` | | Binary SQL column → attachment name |
| `Patch` | `string` | | JavaScript patch for INSERT and UPDATE |
| `OnDelete` | `CdcSinkOnDeleteConfig` | | Delete behavior. Default: delete document |
| `EmbeddedTables` | `List<CdcSinkEmbeddedTableConfig>` | | Nested table configurations |
| `LinkedTables` | `List<CdcSinkLinkedTableConfig>` | | Foreign key reference configurations |
| `Disabled` | `bool` | | Skip this table. Default: `false` |

{PANEL/}

---

{PANEL: CdcSinkEmbeddedTableConfig}

Configures a table whose rows are embedded as nested objects within a parent document.

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `SourceTableName` | `string` | ✓ | SQL table name |
| `SourceTableSchema` | `string` | | SQL schema name. Default: `"public"` |
| `PropertyName` | `string` | ✓ | Property name in the parent document (e.g., `"Lines"`) |
| `Type` | `CdcSinkRelationType` | ✓ | `Array`, `Map`, or `Value` |
| `JoinColumns` | `List<string>` | ✓ | FK columns referencing parent's `PrimaryKeyColumns` |
| `PrimaryKeyColumns` | `List<string>` | ✓ | PK columns for matching items on UPDATE/DELETE |
| `ColumnsMapping` | `Dictionary<string, string>` | ✓ | SQL column → embedded property |
| `AttachmentNameMapping` | `Dictionary<string, string>` | | Binary SQL column → attachment name |
| `Patch` | `string` | | JavaScript patch on **parent** document for INSERT/UPDATE |
| `OnDelete` | `CdcSinkOnDeleteConfig` | | Delete behavior for embedded items |
| `CaseSensitiveKeys` | `bool` | | Case-sensitive PK matching. Default: `false` |
| `EmbeddedTables` | `List<CdcSinkEmbeddedTableConfig>` | | Nested embedded tables |
| `Disabled` | `bool` | | Skip this table. Default: `false` |

{PANEL/}

---

{PANEL: CdcSinkLinkedTableConfig}

Configures a foreign key reference that becomes a document ID in the parent document.

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `SourceTableName` | `string` | ✓ | SQL table name of the referenced table |
| `SourceTableSchema` | `string` | | SQL schema name. Default: `"public"` |
| `PropertyName` | `string` | ✓ | Property name in the parent document (e.g., `"Customer"`) |
| `LinkedCollectionName` | `string` | ✓ | Target RavenDB collection for ID generation (e.g., `"Customers"`) |
| `Type` | `CdcSinkRelationType` | ✓ | `Value` (single reference) or `Array` (multiple references) |
| `JoinColumns` | `List<string>` | ✓ | FK columns used to build the referenced document ID |

{PANEL/}

---

{PANEL: CdcSinkOnDeleteConfig}

Controls how DELETE events are handled for a table or embedded table.

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Patch` | `string` | null | JavaScript patch that runs when a DELETE event arrives |
| `IgnoreDeletes` | `bool` | `false` | When `true`, skip the delete — document/item is kept |

**Available patch variables for OnDelete:**

* `this` — the document (root or parent for embedded)
* `$row` — all SQL columns from the DELETE event
* `$old` — the embedded item's last known state (for embedded tables)

**Behavior matrix:**

| IgnoreDeletes | Patch | Result |
|---------------|-------|--------|
| `false` | null | Normal delete (default) |
| `false` | set | Patch runs, then delete proceeds |
| `true` | null | DELETE discarded silently |
| `true` | set | Patch runs, delete skipped |

{PANEL/}

---

{PANEL: CdcSinkRelationType}

Specifies the structure of embedded or linked data in the document.

| Value | Description |
|-------|-------------|
| `Array` | One-to-many: stored as a JSON array. Items matched by PK for UPDATE/DELETE |
| `Map` | One-to-many: stored as a JSON object keyed by PK value(s) |
| `Value` | Many-to-one: stored as a single embedded object or document reference |

{PANEL/}

---

## Related Articles

### CDC Sink

- [Schema Design](../../../server/ongoing-tasks/cdc-sink/schema-design)
- [Embedded Tables](../../../server/ongoing-tasks/cdc-sink/embedded-tables)
- [Linked Tables](../../../server/ongoing-tasks/cdc-sink/linked-tables)
- [Patching](../../../server/ongoing-tasks/cdc-sink/patching)
- [Delete Strategies](../../../server/ongoing-tasks/cdc-sink/delete-strategies)
- [API Reference](../../../server/ongoing-tasks/cdc-sink/api-reference)
