# CDC Sink: Property Retention
---

{NOTE: }

* When CDC Sink applies an UPDATE to an existing RavenDB document, it **merges**
  the new values onto the existing document rather than replacing it entirely.

* Properties that are not part of the column mapping are preserved across updates.
  This allows RavenDB-side data to coexist safely with CDC-managed properties.

* In this page:
  * [How Merging Works](../../../server/ongoing-tasks/cdc-sink/property-retention#how-merging-works)
  * [What Is and Isn't Preserved](../../../server/ongoing-tasks/cdc-sink/property-retention#what-is-and-isnt-preserved)
  * [Editing Documents Directly in RavenDB](../../../server/ongoing-tasks/cdc-sink/property-retention#editing-documents-directly-in-ravendb)
  * [Implications for Patches](../../../server/ongoing-tasks/cdc-sink/property-retention#implications-for-patches)

{NOTE/}

---

{PANEL: How Merging Works}

When a CDC UPDATE arrives for a document that already exists:

1. The existing document is loaded
2. Mapped column values from the CDC event overwrite the corresponding properties
3. All other properties on the document are left unchanged
4. The merged document is written back

**Example:**

Initial SQL row → initial document:

    {
        "Id": 1,
        "Name": "Alice",
        "Email": "alice@example.com",
        "InternalNotes": "VIP customer",   // Added directly in RavenDB
        "@metadata": { "@collection": "Customers" }
    }

SQL UPDATE: `UPDATE customers SET email = 'alice.new@example.com' WHERE id = 1`

Document after CDC UPDATE:

    {
        "Id": 1,
        "Name": "Alice",
        "Email": "alice.new@example.com",  // ← Updated from SQL
        "InternalNotes": "VIP customer",   // ← Preserved (not in ColumnsMapping)
        "@metadata": { "@collection": "Customers" }
    }

{PANEL/}

---

{PANEL: What Is and Isn't Preserved}

**Preserved across CDC updates:**

* Properties not listed in `ColumnsMapping`
* Properties set in RavenDB directly (annotations, computed values, flags)
* Document metadata (unless the patch explicitly modifies it)

**Overwritten on CDC update:**

* Any property mapped via `ColumnsMapping` — always updated to match the current SQL value

If you manually edit a property that is part of `ColumnsMapping`, the next CDC UPDATE
for that row will overwrite your edit with the SQL value.

{PANEL/}

---

{PANEL: Editing Documents Directly in RavenDB}

You can safely add properties to CDC-managed documents:

    // Safe: these properties are not in ColumnsMapping
    {
        "Id": 1,
        "Name": "Alice",          // ← Managed by CDC
        "Email": "alice@ex.com",  // ← Managed by CDC
        "InternalNotes": "...",   // ← Safe to add and edit
        "ReviewedAt": "...",      // ← Safe to add and edit
        "Tags": ["vip"]           // ← Safe to add and edit
    }

Properties managed by CDC (those in `ColumnsMapping`) will be overwritten on
the next UPDATE from the source database. Do not rely on manual edits to mapped
properties surviving future CDC updates.

**CDC Sink does not detect or protect manual edits to mapped properties.**
If you need to preserve a value that comes from SQL, consider adding a separate
RavenDB-only property for your annotation and leaving the SQL-mapped property as-is.

{PANEL/}

---

{PANEL: Implications for Patches}

Patches run after column mapping and can set additional properties that are not
from `ColumnsMapping`. These patch-computed properties follow the same merge rules:

* If a patch sets `this.ComputedField = ...`, that value persists across future events
  where the patch doesn't explicitly change it
* If a patch sets a property that is also in `ColumnsMapping`, the column mapping
  value takes precedence (mapping is applied before patching)

For aggregates maintained via patches (e.g., `RunningTotal`), the patch itself
is responsible for keeping the value correct across INSERT, UPDATE, and DELETE events.
See [Patching](../../../server/ongoing-tasks/cdc-sink/patching).

{PANEL/}

---

## Related Articles

### CDC Sink

- [Column Mapping](../../../server/ongoing-tasks/cdc-sink/column-mapping)
- [Patching](../../../server/ongoing-tasks/cdc-sink/patching)
- [How It Works](../../../server/ongoing-tasks/cdc-sink/how-it-works)
