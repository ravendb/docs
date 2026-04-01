# CDC Sink: Column Mapping
---

{NOTE: }

* Column mapping controls which SQL columns appear in the RavenDB document and
  under what property names.

* In this page:
  * [Mapping Columns to Properties](../../../server/ongoing-tasks/cdc-sink/column-mapping#mapping-columns-to-properties)
  * [Unmapped Columns](../../../server/ongoing-tasks/cdc-sink/column-mapping#unmapped-columns)
  * [Attachment Mapping](../../../server/ongoing-tasks/cdc-sink/column-mapping#attachment-mapping)
  * [Schema (Source Table Schema)](../../../server/ongoing-tasks/cdc-sink/column-mapping#schema-source-table-schema)

{NOTE/}

---

{PANEL: Mapping Columns to Properties}

`ColumnsMapping` is a `Dictionary<string, string>` where each entry maps a SQL column
name to a RavenDB document property name:

    ColumnsMapping = new Dictionary<string, string>
    {
        { "id",            "Id" },
        { "customer_name", "CustomerName" },
        { "order_date",    "OrderDate" },
        { "total_amount",  "TotalAmount" }
    }

**Key:** SQL column name (case-insensitive match against the column names in CDC events)
**Value:** Property name in the RavenDB document

**At least one mapping is required.** An empty `ColumnsMapping` is a validation error.

The same rules apply to embedded table column mappings.

{PANEL/}

---

{PANEL: Unmapped Columns}

Columns not listed in `ColumnsMapping` are **not stored** in the document, but they
are available in patch scripts via `$row`.

This allows you to use data for computations without permanently storing raw SQL values:

    ColumnsMapping = new Dictionary<string, string>
    {
        { "id",   "Id" },
        { "name", "Name" }
        // base_price and tax_rate are NOT mapped — won't appear in document
    },
    Patch = "this.FinalPrice = $row.base_price * (1 + $row.tax_rate);"

In this example, `base_price` and `tax_rate` are available during the patch but
not stored as document properties. Only the computed `FinalPrice` is stored.

{PANEL/}

---

{PANEL: Attachment Mapping}

Binary SQL columns (e.g., PostgreSQL `BYTEA`) can be stored as RavenDB attachments
instead of document properties using `AttachmentNameMapping`:

    new CdcSinkTableConfig
    {
        Name = "Files",
        SourceTableName = "files",
        PrimaryKeyColumns = ["id"],
        ColumnsMapping = { { "id", "Id" }, { "filename", "Filename" } },
        AttachmentNameMapping = new Dictionary<string, string>
        {
            { "content", "file" }   // SQL column "content" → attachment named "file"
        }
    }

The binary column `content` becomes an attachment named `"file"` on the document.

**Embedded table attachments:**

Binary columns on embedded tables are also supported. The attachment name is prefixed
with the embedded property path and primary key to ensure uniqueness:

    new CdcSinkEmbeddedTableConfig
    {
        SourceTableName = "photos",
        PropertyName = "Photos",
        PrimaryKeyColumns = ["photo_num"],
        AttachmentNameMapping = { { "thumbnail", "thumb" } }
    }

A photo with `photo_num = 1` creates attachment `"Photos/1/thumb"` on the parent document.
When the embedded item is deleted, its attachments are automatically removed.

{PANEL/}

---

{PANEL: Schema (Source Table Schema)}

`SourceTableSchema` specifies the SQL schema containing the table. It defaults to
`"public"` for PostgreSQL if omitted.

    new CdcSinkTableConfig
    {
        SourceTableSchema = "sales",    // Table is in the "sales" schema
        SourceTableName = "orders",
        // ...
    }

For most PostgreSQL setups using the default `public` schema, this can be omitted.

{PANEL/}

---

## Related Articles

### CDC Sink

- [Schema Design](../../../server/ongoing-tasks/cdc-sink/schema-design)
- [Patching](../../../server/ongoing-tasks/cdc-sink/patching)
- [Attachment Handling](../../../server/ongoing-tasks/cdc-sink/attachment-handling)
- [Configuration Reference](../../../server/ongoing-tasks/cdc-sink/configuration-reference)
