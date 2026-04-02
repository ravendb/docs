# CDC Sink: Embedded Tables
---

{NOTE: }

* Embedded tables allow CDC Sink to nest SQL table data inside a parent document as
  arrays, maps, or single objects.

* This page covers configuration options, nesting constraints, and how embedded items
  are updated and deleted.

* In this page:
  * [Basic Configuration](../../../server/ongoing-tasks/cdc-sink/embedded-tables#basic-configuration)
  * [Join Columns and Primary Key Interaction](../../../server/ongoing-tasks/cdc-sink/embedded-tables#join-columns-and-primary-key-interaction)
  * [Matching Items on Update and Delete](../../../server/ongoing-tasks/cdc-sink/embedded-tables#matching-items-on-update-and-delete)
  * [Deep Nesting](../../../server/ongoing-tasks/cdc-sink/embedded-tables#deep-nesting)
  * [Attachments on Embedded Items](../../../server/ongoing-tasks/cdc-sink/embedded-tables#attachments-on-embedded-items)
  * [Disabling an Embedded Table](../../../server/ongoing-tasks/cdc-sink/embedded-tables#disabling-an-embedded-table)

{NOTE/}

---

{PANEL: Basic Configuration}

`CdcSinkEmbeddedTableConfig` is placed inside a root table's `EmbeddedTables` list:

    new CdcSinkEmbeddedTableConfig
    {
        SourceTableSchema = "public",         // SQL schema (optional)
        SourceTableName = "order_lines",      // SQL table name
        PropertyName = "Lines",               // Property name in parent document
        Type = CdcSinkRelationType.Array,     // Array, Map, or Value
        JoinColumns = ["order_id"],           // FK to parent's PrimaryKeyColumns
        PrimaryKeyColumns = ["line_id"],      // Used to match items on UPDATE/DELETE
        ColumnsMapping = new Dictionary<string, string>
        {
            { "line_id", "LineId" },
            { "product", "Product" },
            { "quantity", "Quantity" }
        }
    }

{PANEL/}

---

{PANEL: Join Columns and Primary Key Interaction}

### Purpose of JoinColumns

`JoinColumns` specifies the foreign key columns in the embedded table that reference
the parent's primary key. CDC Sink uses these columns to route each row to the correct
parent document.

The `JoinColumns` values must exactly match the parent's `PrimaryKeyColumns`:

    // Parent root table:
    PrimaryKeyColumns = ["order_id"]

    // Embedded table - correct:
    JoinColumns = ["order_id"]          // References parent's PK column

    // Embedded table - INCORRECT:
    JoinColumns = ["customer_id"]       // Does not reference the parent's PK

### DELETE Events

For DELETE events, the source database must include the join column values so CDC Sink
can route the delete to the correct parent document.

By default, many databases only include primary key columns in DELETE events. If the
join column is _not_ in the embedded table's primary key, additional source database
configuration is required.

**PostgreSQL:** See [REPLICA IDENTITY](../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity)
for how CDC Sink handles this automatically when it has sufficient permissions, or how
a DBA can configure it manually.

### Avoiding REPLICA IDENTITY Requirements

The cleanest solution is to include the join column in the embedded table's primary key:

    -- SQL schema:
    CREATE TABLE order_lines (
        order_id INT NOT NULL REFERENCES orders(id),
        line_id  INT NOT NULL,
        product  VARCHAR(200),
        PRIMARY KEY (order_id, line_id)   -- order_id in PK means DELETE events include it
    );

    // Configuration:
    PrimaryKeyColumns = ["order_id", "line_id"]
    JoinColumns = ["order_id"]

With `order_id` in the primary key, DELETE events include it by default and no additional
source database configuration is needed.

Alternatively, set `OnDelete.IgnoreDeletes = true` to skip delete routing entirely
if deletes on that embedded table don't need to be processed.
See [Delete Strategies](../../../server/ongoing-tasks/cdc-sink/delete-strategies).

{PANEL/}

---

{PANEL: Matching Items on Update and Delete}

When an UPDATE or DELETE arrives for an embedded row, CDC Sink must find the correct
item within the parent document's array or map.

Items are matched by their full `PrimaryKeyColumns` composite key:

* **INSERT** — New item appended to array / added to map
* **UPDATE** — Item found by PK match; mapped columns overwritten
* **DELETE** — Item found by PK match; removed from array/map (or OnDelete behavior applied)

**Composite PKs** work the same way — all PK columns must match:

    PrimaryKeyColumns = ["invoice_date", "invoice_seq"]

    // UPDATE event for (invoice_date='2024-01-15', invoice_seq=3)
    // → Finds and updates the item where both columns match

**Case sensitivity:** By default, PK matching is case-insensitive. Set
`CaseSensitiveKeys = true` on `CdcSinkEmbeddedTableConfig` if your keys are
case-sensitive.

{PANEL/}

---

{PANEL: Deep Nesting}

Embedded tables can contain their own `EmbeddedTables`, creating hierarchies
with multiple levels.

**Key constraint:** Every descendant table must carry the **root table's primary key**
as a denormalized column. This is required because CDC Sink routes each row to its
root document in a single pass.

**Example: Company → Departments → Employees**

The `employees` table must have `company_id` (the root PK) even though it only
directly joins to `departments`:

    CREATE TABLE employees (
        company_id INT NOT NULL,   -- Denormalized root PK
        dept_id    INT NOT NULL,
        emp_id     INT NOT NULL,
        PRIMARY KEY (company_id, dept_id, emp_id)
    );

    // Configuration for the employees embedded table:
    JoinColumns = ["company_id", "dept_id"]   // Root PK + parent PK
    PrimaryKeyColumns = ["emp_id"]

**Why is the root PK required?**

When a CDC Sink event arrives for an `employees` row, CDC Sink needs to:

1. Find the root document: `Companies/{company_id}`
2. Navigate to the correct `Departments` array item: `Departments.find(d => d.dept_id == dept_id)`
3. Add or update the `Employees` array item: `Employees.find(e => e.emp_id == emp_id)`

Without `company_id` in the event, CDC Sink cannot identify the root document
without an additional lookup, which is not supported.

{PANEL/}

---

{PANEL: Attachments on Embedded Items}

Binary columns from embedded tables can be stored as RavenDB attachments using
`AttachmentNameMapping`.

    new CdcSinkEmbeddedTableConfig
    {
        SourceTableName = "photos",
        PropertyName = "Photos",
        PrimaryKeyColumns = ["photo_num"],
        JoinColumns = ["product_id"],
        ColumnsMapping = { { "photo_num", "PhotoNum" }, { "caption", "Caption" } },
        AttachmentNameMapping = { { "thumbnail", "thumb" } }
    }

A photo with `photo_num = 1` creates an attachment named `"Photos/1/thumb"` on the
parent document. The attachment name is prefixed with the embedded path and PK to
ensure uniqueness within the document.

When the embedded item is deleted, its attachments are automatically removed.

{PANEL/}

---

{PANEL: Disabling an Embedded Table}

Set `Disabled = true` to pause processing for a specific embedded table without
removing it from the configuration:

    new CdcSinkEmbeddedTableConfig
    {
        SourceTableName = "audit_log",
        PropertyName = "AuditLog",
        Disabled = true,
        // ... other config
    }

Changes from the source table are ignored while `Disabled = true`. When re-enabled,
CDC Sink resumes from the current position — it does not backfill missed events.

{PANEL/}

---

## Related Articles

### CDC Sink

- [Schema Design](../../../server/ongoing-tasks/cdc-sink/schema-design)
- [Linked Tables](../../../server/ongoing-tasks/cdc-sink/linked-tables)
- [Delete Strategies](../../../server/ongoing-tasks/cdc-sink/delete-strategies)
- [Attachment Handling](../../../server/ongoing-tasks/cdc-sink/attachment-handling)
- [Configuration Reference](../../../server/ongoing-tasks/cdc-sink/configuration-reference)

### PostgreSQL

- [REPLICA IDENTITY](../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity)
- [REPLICA IDENTITY Manual Setup](../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity-manual-setup)
