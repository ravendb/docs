# CDC Sink: How It Works
---

{NOTE: }

* This page describes the internal operation of CDC Sink — how it connects to the
  source database, loads initial data, streams changes, and handles failover.

* Understanding these mechanics is important when designing patches, planning for
  failover, and setting up monitoring.

* In this page:
  * [Startup and Verification](../../../server/ongoing-tasks/cdc-sink/how-it-works#startup-and-verification)
  * [Initial Load](../../../server/ongoing-tasks/cdc-sink/how-it-works#initial-load)
  * [Change Streaming](../../../server/ongoing-tasks/cdc-sink/how-it-works#change-streaming)
  * [Transaction Ordering](../../../server/ongoing-tasks/cdc-sink/how-it-works#transaction-ordering)
  * [State Persistence and Failover](../../../server/ongoing-tasks/cdc-sink/how-it-works#state-persistence-and-failover)
  * [Child Before Parent](../../../server/ongoing-tasks/cdc-sink/how-it-works#child-before-parent)

{NOTE/}

---

{PANEL: Startup and Verification}

When a CDC Sink task starts, it first verifies that the source database is properly
configured. For PostgreSQL, this includes checking:

* WAL level is set to `logical`
* The connecting user has sufficient privileges
* REPLICA IDENTITY is configured correctly for embedded tables that need delete routing

If any check fails, CDC Sink reports the exact issue and the SQL an administrator
needs to run to fix it. The task does not start until all checks pass.

After verification, CDC Sink creates the necessary replication infrastructure
in the source database (publication and replication slot), then begins the initial load.

See the [PostgreSQL Prerequisites Checklist](../../../server/ongoing-tasks/cdc-sink/postgres/prerequisites-checklist)
for the full list of requirements.

{PANEL/}

---

{PANEL: Initial Load}

Before streaming live changes, CDC Sink performs a full scan of every configured table
(root, embedded, and linked) using keyset pagination ordered by primary key.

**Progress tracking:** Initial load progress is persisted per-table as a document in
RavenDB. If the task is restarted, it resumes from the last processed key rather than
re-scanning the entire table.

**Batch pipelining:** While one batch is being written to RavenDB, the next batch is
being read from the source database, keeping both systems busy.

**Ordering:** Tables are scanned in dependency order. Root tables are loaded first,
then embedded tables. This minimises the number of stub documents created (see
[Child Before Parent](../../../server/ongoing-tasks/cdc-sink/how-it-works#child-before-parent) below).

{PANEL/}

---

{PANEL: Change Streaming}

After the initial load completes, CDC Sink opens a streaming connection to the
source database and begins receiving changes in real time.

Changes arrive grouped by source database transaction, preserving the exact order
of operations. A transaction is only applied to RavenDB after it is fully committed
in the source database — partial transactions are never written.

**Document merging:** When an UPDATE arrives, CDC Sink merges the new column values
onto the existing RavenDB document. Properties that are not part of the column mapping
are preserved. This allows RavenDB-side annotations and computed fields to coexist with
CDC-managed properties.

See [Property Retention](../../../server/ongoing-tasks/cdc-sink/property-retention) for details.

{PANEL/}

---

{PANEL: Transaction Ordering}

CDC Sink preserves the full order of operations within a source database transaction.
If a single transaction performs multiple operations on the same row, all operations
are applied in order.

**Example:** A source transaction that does:

    BEGIN;
    INSERT INTO items (id, name) VALUES (1, 'Alpha');
    UPDATE items SET name = 'Beta' WHERE id = 1;
    DELETE FROM items WHERE id = 1;
    INSERT INTO items (id, name) VALUES (1, 'Gamma');
    UPDATE items SET name = 'Delta' WHERE id = 1;
    COMMIT;

CDC Sink applies all five operations in order. The final document state is `name = 'Delta'`.

Multiple documents modified in the same transaction are also applied atomically within
a single RavenDB batch.

{PANEL/}

---

{PANEL: State Persistence and Failover}

### State Storage

CDC Sink persists its progress as a **document in RavenDB**, alongside your data.
This document records:

* The last acknowledged position in the source database's change log (LSN for PostgreSQL)
* Per-table initial load progress (which tables completed, and the last key scanned)

Like any RavenDB document, this state document is subject to normal replication behavior.
Different nodes in a cluster may have different versions of this document at any point in time.

### Failover Behavior

When the cluster elects a new mentor node for the CDC Sink task, the new node reads
the **replicated** state document — which may be older than the work the previous
mentor had completed but not yet replicated.

The new mentor resumes from that replicated state. This means:

* **No data is lost** — CDC Sink resumes from a known position and the source database
  retains all changes from that position onward
* **Some changes may be re-read** — Changes between the replicated state and the
  previous mentor's actual progress will be processed again

Re-reading is normal and expected. The document merge strategy means that re-applying
the same INSERT or UPDATE is safe — column values are simply overwritten with the same values.

{WARNING: }
Patches that are not idempotent can produce incorrect results if the same change is
re-read after a failover. Design patches to handle re-processing safely.
See [Patching](../../../server/ongoing-tasks/cdc-sink/patching) for guidance.
{WARNING/}

{PANEL/}

---

{PANEL: Child Before Parent}

If an embedded row arrives before its parent row exists in RavenDB — which can happen
during initial load when tables are scanned in parallel, or due to relaxed foreign key
constraints in the source database — CDC Sink creates a **stub document** containing
only the embedded data.

When the parent row arrives later, its columns are merged onto the stub document.
The final document contains both the parent fields and all embedded items that arrived earlier.

This ensures no data is lost regardless of the order in which rows are processed.

{PANEL/}

---

## Related Articles

### CDC Sink

- [Overview](../../../server/ongoing-tasks/cdc-sink/overview)
- [Schema Design](../../../server/ongoing-tasks/cdc-sink/schema-design)
- [Patching](../../../server/ongoing-tasks/cdc-sink/patching)
- [Property Retention](../../../server/ongoing-tasks/cdc-sink/property-retention)
- [Failover and Consistency](../../../server/ongoing-tasks/cdc-sink/failover-and-consistency)

### PostgreSQL

- [Prerequisites Checklist](../../../server/ongoing-tasks/cdc-sink/postgres/prerequisites-checklist)
- [Initial Setup](../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup)
