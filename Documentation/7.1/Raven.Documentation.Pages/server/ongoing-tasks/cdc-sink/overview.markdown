# CDC Sink: Overview
---

{NOTE: }

* **CDC Sink** is a RavenDB ongoing task that reads **Change Data Capture (CDC)**
  streams from a relational database and writes the resulting documents into RavenDB.

* CDC Sink is the reverse of ETL: instead of pushing data _from_ RavenDB _to_ SQL,
  CDC Sink _pulls_ data _from_ SQL _into_ RavenDB.
  The relational database is the source of truth; RavenDB receives a continuously-updated
  document model derived from it.

* CDC Sink maps normalized relational tables into rich, nested RavenDB documents —
  automatically and in real time.

* Supported source databases:
  * **PostgreSQL** — via logical replication
  * Additional source databases planned for future versions

* In this page:
  * [Why Use CDC Sink](../../../server/ongoing-tasks/cdc-sink/overview#why-use-cdc-sink)
  * [How It Works](../../../server/ongoing-tasks/cdc-sink/overview#how-it-works)
  * [Task Lifecycle](../../../server/ongoing-tasks/cdc-sink/overview#task-lifecycle)
  * [Licensing](../../../server/ongoing-tasks/cdc-sink/overview#licensing)

{NOTE/}

---

{PANEL: Why Use CDC Sink}

CDC Sink solves the problem of moving data from a relational database into RavenDB
without requiring changes to the source system.

* **Migrate from SQL to RavenDB**
  Transform normalized SQL tables (orders, order_lines, customers) into rich RavenDB
  documents where an Order contains embedded LineItems and a reference to the Customer —
  automatically and continuously, without changing your SQL application.

* **Build a read-optimized view**
  Your transactional system uses a relational database, but your API layer needs
  denormalized documents. CDC Sink creates and maintains those documents without
  touching your existing application.

* **Gradual migration**
  Keep your SQL application running while RavenDB documents are built in the background.
  Applications can start reading from RavenDB while writes still go to the relational database.

* **Event-driven side effects**
  Using JavaScript patches, every INSERT, UPDATE, or DELETE in the source database can
  trigger custom logic in RavenDB — computing derived fields, maintaining running totals,
  or writing custom transformations.

{PANEL/}

---

{PANEL: How It Works}

A CDC Sink task continuously reads changes from the source relational database and
applies them to RavenDB documents.

### Initial Load

When a CDC Sink task starts for the first time, it performs a full scan of every
configured table using keyset pagination. This populates RavenDB with the current
state of the data before streaming begins.

Initial load progress is persisted per-table. If the task is restarted, it resumes
from where it left off rather than re-scanning.

### Change Streaming

After the initial load, CDC Sink switches to streaming changes in real time.
Changes are grouped into transactions, preserving the exact order of operations
from the source database. Partial transactions are never written to RavenDB —
all changes within a source database transaction are applied together.

### Document Model

The relational model is mapped to RavenDB documents through configuration:

* **Root tables** map to RavenDB collections (one document per row)
* **Embedded tables** become nested arrays or objects within parent documents
* **Linked tables** become document ID references

See [Schema Design](../../../server/ongoing-tasks/cdc-sink/schema-design) for details.

{PANEL/}

---

{PANEL: Task Lifecycle}

1. **Create** — Define the task in Studio or via the Client API
   Specify the connection string, table mappings, and transformation options

2. **Verify** — CDC Sink verifies the source database is properly configured
   Checks permissions, replication prerequisites, and table configuration

3. **Initial Load** — Full table scan populates RavenDB with current data
   Progress is tracked per-table and persists across restarts

4. **Stream** — Real-time change streaming begins
   All INSERTs, UPDATEs, and DELETEs are applied to RavenDB documents as they occur

5. **Monitor** — View statistics, errors, and progress in Studio

6. **Retire** — Delete the task in RavenDB when no longer needed
   PostgreSQL artifacts (replication slot, publication) must be cleaned up by
   the database administrator separately

{PANEL/}

---

{PANEL: Licensing}

{INFO: }
CDC Sink is available on an **Enterprise** license.
{INFO/}

Learn more about licensing [here](../../../start/licensing/licensing-overview).

{PANEL/}

---

## Related Articles

### CDC Sink

- [How It Works](../../../server/ongoing-tasks/cdc-sink/how-it-works)
- [Use Cases](../../../server/ongoing-tasks/cdc-sink/use-cases)
- [Schema Design](../../../server/ongoing-tasks/cdc-sink/schema-design)
- [Configuration Reference](../../../server/ongoing-tasks/cdc-sink/configuration-reference)

### PostgreSQL

- [Prerequisites Checklist](../../../server/ongoing-tasks/cdc-sink/postgres/prerequisites-checklist)
- [Initial Setup](../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup)

### Server

- [ETL Basics](../../../server/ongoing-tasks/etl/basics)
- [Queue Sink Overview](../../../server/ongoing-tasks/queue-sink/overview)
