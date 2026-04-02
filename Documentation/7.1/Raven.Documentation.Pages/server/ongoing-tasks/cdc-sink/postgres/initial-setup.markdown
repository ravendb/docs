# CDC Sink for PostgreSQL: Initial Setup
---

{NOTE: }

* When CDC Sink starts, it verifies and (if it has the necessary permissions) creates
  the **replication slot** and **publication** required for logical replication.

* If the CDC Sink user does not have permission to create these objects, a database
  administrator must create them manually before the task can start.

* In this page:
  * [Automatic Setup](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup#automatic-setup)
  * [Manual Setup](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup#manual-setup)
  * [Specifying Custom Slot and Publication Names](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup#specifying-custom-slot-and-publication-names)
  * [Slot and Publication Naming (Auto-generated)](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup#slot-and-publication-naming-auto-generated)
  * [Verifying Setup](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup#verifying-setup)

{NOTE/}

---

{PANEL: Automatic Setup}

If the CDC Sink user has the required permissions (see
[Permissions and Roles](../../../../server/ongoing-tasks/cdc-sink/postgres/permissions-and-roles)),
CDC Sink will:

1. Compute the replication slot and publication names from the task configuration
2. Check whether they already exist
3. Create them if they do not exist
4. Begin the initial load

No manual database administration is needed in this case.

{PANEL/}

---

{PANEL: Manual Setup}

If the CDC Sink user does not have permission to create replication slots or
publications, a database administrator must create them before the task is started.

**Step 1: Determine the slot and publication names**

The simplest approach is to specify the names explicitly in `CdcSinkPostgresSettings`
so both you and the database administrator know what names to use.
See [Specifying Custom Slot and Publication Names](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup#specifying-custom-slot-and-publication-names).

If using auto-generated names (no `Postgres` settings), CDC Sink derives names
deterministically from the task configuration.
See [Slot and Publication Naming](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup#slot-and-publication-naming-auto-generated)
for the naming scheme.

You can also find the names CDC Sink expects by creating the task (it will fail to start)
and reading the error message, which includes the expected names.

**Step 2: Create the publication**

Create a publication that includes all the tables CDC Sink will replicate:

    CREATE PUBLICATION rvn_cdc_p_<hash>
    FOR TABLE orders, order_lines, customers;

The publication must include all tables from the task's root and embedded table
configurations.

**Step 3: Create the replication slot**

    SELECT pg_create_logical_replication_slot(
        'rvn_cdc_s_<hash>',
        'pgoutput'
    );

**Step 4: Start the CDC Sink task**

Once the slot and publication exist, CDC Sink will detect them on startup and
proceed with the initial load.

{WARNING: }
If the task configuration changes (tables added, removed, or renamed), the expected
slot and publication names change. The old slot and publication become orphaned and
must be dropped manually. The new names must be created before the task can restart.
See [Cleanup and Maintenance](../../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance).
{WARNING/}

{PANEL/}

---

{PANEL: Specifying Custom Slot and Publication Names}

You can explicitly specify the replication slot and publication names by setting
`CdcSinkPostgresSettings` on the task configuration:

    var config = new CdcSinkConfiguration
    {
        Name = "OrdersSync",
        ConnectionStringName = "MyPostgresConnection",
        Postgres = new CdcSinkPostgresSettings
        {
            SlotName = "orders_sync_slot",
            PublicationName = "orders_sync_pub"
        },
        Tables = [ ... ]
    };

Custom names are useful when:

* A database administrator pre-creates the slot and publication with human-readable
  names before starting the task
* You are migrating from a previous CDC Sink task and want to reuse an existing slot
  to avoid re-reading history
* You need predictable names across environments (dev/staging/prod)

**Constraints:** Names must be valid PostgreSQL identifiers — alphanumeric characters
and underscores only, maximum 63 characters.

**Immutability:** Once set, `SlotName` and `PublicationName` cannot be changed. The
slot and publication names are fixed for the lifetime of the task. If you need to
rename them, delete the task and create a new one.

{PANEL/}

---

{PANEL: Slot and Publication Naming (Auto-generated)}

When `CdcSinkPostgresSettings` is not set (or `SlotName`/`PublicationName` are null),
CDC Sink generates deterministic names:

* **Slot name**: `rvn_cdc_s_<hash>`
* **Publication name**: `rvn_cdc_p_<hash>`

The `<hash>` is a base32-encoded SHA-256 hash of:

* The RavenDB database name
* The CDC Sink task name
* The names of all tables in the task configuration

**Example:**

For a task named `OrdersSync` on database `NorthWind` with tables `orders` and
`order_lines`, the generated names look like:

    rvn_cdc_s_jl0okb2prit591rfdfjt1g4kebi3879vgl3avve7gunadllk4re0
    rvn_cdc_p_jl0okb2prit591rfdfjt1g4kebi3879vgl3avve7gunadllk4re0

The slot and publication share the same hash, making it easy to match them to
a specific task.

{NOTE: }
Because the hash is derived from the task name, database name, and table names,
changing any of these values produces a different hash and therefore a different
slot name. This prevents naming conflicts between multiple CDC Sink tasks on the
same PostgreSQL instance, but means that renaming a task or adding/removing tables
produces a new slot — the old one becomes orphaned.
{NOTE/}

{PANEL/}

---

{PANEL: Verifying Setup}

To verify the slot and publication were created:

    -- View CDC Sink replication slots
    SELECT slot_name, plugin, slot_type, active
    FROM pg_replication_slots
    WHERE slot_name LIKE 'rvn_cdc_s_%';

    -- View CDC Sink publications
    SELECT p.pubname, c.relname AS table_name
    FROM pg_publication p
    JOIN pg_publication_rel pr ON pr.prpubid = p.oid
    JOIN pg_class c ON c.oid = pr.prrelid
    WHERE p.pubname LIKE 'rvn_cdc_p_%'
    ORDER BY p.pubname, c.relname;

{PANEL/}

---

## Related Articles

### CDC Sink for PostgreSQL

- [Prerequisites Checklist](../../../../server/ongoing-tasks/cdc-sink/postgres/prerequisites-checklist)
- [Permissions and Roles](../../../../server/ongoing-tasks/cdc-sink/postgres/permissions-and-roles)
- [REPLICA IDENTITY](../../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity)
- [Cleanup and Maintenance](../../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance)
