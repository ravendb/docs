# CDC Sink: Delete Strategies
---

{NOTE: }

* CDC Sink provides configurable behavior for DELETE events through the
  `CdcSinkOnDeleteConfig` object on both root and embedded table configurations.

* By default, a DELETE event deletes the corresponding RavenDB document or removes
  the embedded item. The `OnDelete` configuration changes this.

* In this page:
  * [Default Behavior](../../../server/ongoing-tasks/cdc-sink/delete-strategies#default-behavior)
  * [OnDelete Configuration](../../../server/ongoing-tasks/cdc-sink/delete-strategies#ondelete-configuration)
  * [Pattern: Archive](../../../server/ongoing-tasks/cdc-sink/delete-strategies#pattern-archive)
  * [Pattern: Audit Trail](../../../server/ongoing-tasks/cdc-sink/delete-strategies#pattern-audit-trail)
  * [Pattern: Silent Ignore](../../../server/ongoing-tasks/cdc-sink/delete-strategies#pattern-silent-ignore)
  * [OnDelete for Embedded Tables](../../../server/ongoing-tasks/cdc-sink/delete-strategies#ondelete-for-embedded-tables)
  * [Behavior Summary](../../../server/ongoing-tasks/cdc-sink/delete-strategies#behavior-summary)
  * [DELETE Routing and REPLICA IDENTITY](../../../server/ongoing-tasks/cdc-sink/delete-strategies#delete-routing-and-replica-identity)

{NOTE/}

---

{PANEL: Default Behavior}

When `OnDelete` is `null` (or not set):

* **Root table DELETE** — The corresponding RavenDB document is deleted
* **Embedded table DELETE** — The item is removed from the parent document's array or map

{PANEL/}

---

{PANEL: OnDelete Configuration}

`CdcSinkOnDeleteConfig` has two properties:

    public class CdcSinkOnDeleteConfig
    {
        // JavaScript patch that runs when a DELETE event arrives
        // Available variables: this, $row, $old
        public string Patch { get; set; }

        // When true, the delete is not applied — document/item is kept
        public bool IgnoreDeletes { get; set; }
    }

* If `Patch` is set, it runs **before** the delete decision is made
* If `IgnoreDeletes = true`, the deletion is skipped after the patch runs
* If `IgnoreDeletes = false` (default), the deletion proceeds after the patch runs

{PANEL/}

---

{PANEL: Pattern: Archive}

Keep the document in RavenDB but mark it as deleted. The patch runs to mark it,
and `IgnoreDeletes = true` prevents the actual deletion:

    OnDelete = new CdcSinkOnDeleteConfig
    {
        IgnoreDeletes = true,
        Patch = @"
            this.Archived = true;
            this.ArchivedAt = new Date().toISOString();
        "
    }

The document remains in RavenDB with `Archived = true`. Queries can filter on this
field to exclude archived records.

{PANEL/}

---

{PANEL: Pattern: Audit Trail}

Allow the deletion to proceed, but first capture the deleted state as a separate
audit record or as data on the document before it disappears.

Since the goal is to allow the delete to proceed, set `IgnoreDeletes = false`
(the default — omit the field entirely):

    OnDelete = new CdcSinkOnDeleteConfig
    {
        // IgnoreDeletes defaults to false — delete proceeds after patch
        Patch = @"
            // $row contains the deleted row's column values
            // $old contains the document's last known state
            // Record what was deleted as a separate document or side effect
            this.DeletedAt = new Date().toISOString();
            this.FinalState = { Name: $old?.Name, Status: $old?.Status };
        "
    }

The patch runs, then the document is deleted. Any logic you need to run before
the document disappears (such as writing an audit entry via a subscription) should
read from the document before the delete takes effect.

{PANEL/}

---

{PANEL: Pattern: Silent Ignore}

Discard DELETE events without running any patch. Use this for append-only data
where deletes should never result in document removal:

    OnDelete = new CdcSinkOnDeleteConfig
    {
        IgnoreDeletes = true
        // No patch — DELETE events are silently discarded
    }

{PANEL/}

---

{PANEL: OnDelete for Embedded Tables}

The same `OnDelete` configuration works on embedded tables. For embedded tables:

* `Patch` runs on the **parent document** (not the embedded item)
* `$old` contains the embedded item's last known state before deletion
* `IgnoreDeletes = true` prevents the item from being removed from the array/map

**Example: Keep deleted items in an audit array**

Rather than removing a deleted line item from the array, move it to a separate
`DeletedLines` property:

    new CdcSinkEmbeddedTableConfig
    {
        SourceTableName = "order_lines",
        PropertyName = "Lines",
        // ...
        OnDelete = new CdcSinkOnDeleteConfig
        {
            IgnoreDeletes = true,
            Patch = @"
                // Remove from active Lines
                this.Lines = (this.Lines || [])
                    .filter(l => l.LineId !== $old?.LineId);

                // Add to DeletedLines audit array
                this.DeletedLines = this.DeletedLines || [];
                this.DeletedLines.push({
                    LineId: $old?.LineId,
                    Product: $old?.Product,
                    Quantity: $old?.Quantity,
                    DeletedAt: new Date().toISOString()
                });
            "
        }
    }

With `IgnoreDeletes = true`, CDC Sink does not automatically remove the item —
the patch takes full control of both the `Lines` array and the `DeletedLines` audit trail.

{PANEL/}

---

{PANEL: Behavior Summary}

| IgnoreDeletes | Patch | Behavior |
|---------------|-------|----------|
| `false` | null | Normal delete (default) |
| `false` | set | Patch runs, then delete proceeds |
| `true` | null | DELETE event discarded silently |
| `true` | set | Patch runs, then delete is skipped |

{PANEL/}

---

{PANEL: DELETE Routing and REPLICA IDENTITY}

For CDC Sink to route a DELETE event to the correct document or embedded item,
the source database must include the necessary column values in the DELETE event.

For embedded tables where the join column is not in the primary key, the source
database may need additional configuration. See:

* [REPLICA IDENTITY](../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity) (PostgreSQL)

**Skipping REPLICA IDENTITY requirements:** Set `OnDelete.IgnoreDeletes = true`
to discard DELETE events for an embedded table entirely. This skips the REPLICA
IDENTITY check, since delete routing is no longer needed.

{PANEL/}

---

## Related Articles

### CDC Sink

- [Patching](../../../server/ongoing-tasks/cdc-sink/patching)
- [Embedded Tables](../../../server/ongoing-tasks/cdc-sink/embedded-tables)
- [Configuration Reference](../../../server/ongoing-tasks/cdc-sink/configuration-reference)

### PostgreSQL

- [REPLICA IDENTITY](../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity)
