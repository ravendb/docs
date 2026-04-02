# CDC Sink: Attachment Handling
---

{NOTE: }

* Binary SQL columns can be stored as RavenDB **attachments** instead of document
  properties using `AttachmentNameMapping`.

* This applies to both root tables and embedded tables.

* In this page:
  * [Root Table Attachments](../../../server/ongoing-tasks/cdc-sink/attachment-handling#root-table-attachments)
  * [Embedded Table Attachments](../../../server/ongoing-tasks/cdc-sink/attachment-handling#embedded-table-attachments)
  * [Attachment Naming](../../../server/ongoing-tasks/cdc-sink/attachment-handling#attachment-naming)
  * [Attachment Lifecycle](../../../server/ongoing-tasks/cdc-sink/attachment-handling#attachment-lifecycle)

{NOTE/}

---

{PANEL: Root Table Attachments}

Use `AttachmentNameMapping` to map a binary SQL column to a RavenDB attachment:

    new CdcSinkTableConfig
    {
        Name = "Files",
        SourceTableName = "files",
        PrimaryKeyColumns = ["id"],
        ColumnsMapping = new Dictionary<string, string>
        {
            { "id",       "Id" },
            { "filename", "Filename" },
            { "mime_type","MimeType" }
        },
        AttachmentNameMapping = new Dictionary<string, string>
        {
            { "content", "file" }   // SQL column "content" → attachment named "file"
        }
    }

The binary `content` column is stored as an attachment named `"file"` on the document.
The attachment is stored with content type `application/octet-stream`.

{PANEL/}

---

{PANEL: Embedded Table Attachments}

Binary columns on embedded tables are stored as attachments on the **parent** document.
The attachment name is automatically prefixed to ensure uniqueness:

    new CdcSinkEmbeddedTableConfig
    {
        SourceTableName = "photos",
        PropertyName = "Photos",
        PrimaryKeyColumns = ["photo_num"],
        JoinColumns = ["product_id"],
        ColumnsMapping = new Dictionary<string, string>
        {
            { "photo_num", "PhotoNum" },
            { "caption",   "Caption" }
        },
        AttachmentNameMapping = new Dictionary<string, string>
        {
            { "thumbnail", "thumb" }
        }
    }

A photo with `photo_num = 1` creates an attachment named `"Photos/1/thumb"` on the
parent document. The prefix `"Photos/1/"` is generated from the `PropertyName` and
the primary key value.

{PANEL/}

---

{PANEL: Attachment Naming}

**Root table attachments:**

The attachment name is exactly the value you specify in `AttachmentNameMapping`.

    AttachmentNameMapping = { ["content"] = "file" }
    → Attachment name: "file"

**Embedded table attachments:**

The attachment name is prefixed with `{PropertyName}/{pkValue}/`:

    PropertyName = "Photos"
    PrimaryKeyColumns = ["photo_num"]  → photo_num = 1
    AttachmentNameMapping = { { "thumbnail", "thumb" } }
    → Attachment name: "Photos/1/thumb"

For composite primary keys, all key values are joined:

    PrimaryKeyColumns = ["date", "seq"]  → date='2024-01', seq=3
    → Attachment name: "Photos/2024-01/3/thumb"

{PANEL/}

---

{PANEL: Attachment Lifecycle}

* **INSERT** — Attachment is created on the document
* **UPDATE** — Attachment is replaced with the new binary data
* **DELETE (embedded item)** — All attachments for that item are automatically removed
  from the parent document
* **DELETE (root document)** — Document and all its attachments are deleted

{PANEL/}

---

## Related Articles

### CDC Sink

- [Column Mapping](../../../server/ongoing-tasks/cdc-sink/column-mapping)
- [Embedded Tables](../../../server/ongoing-tasks/cdc-sink/embedded-tables)
- [Configuration Reference](../../../server/ongoing-tasks/cdc-sink/configuration-reference)

### Document Extensions

- [Attachments: What are Attachments](../../../document-extensions/attachments/what-are-attachments)
