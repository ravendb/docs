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
  * [Slot and Publication Naming](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup#slot-and-publication-naming)
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

CDC Sink derives names deterministically from the task configuration.
See [Slot and Publication Naming](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup#slot-and-publication-naming)
below for the naming scheme.

You can find the names CDC Sink expects by creating the task (it will fail to start)
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

{PANEL: Slot and Publication Naming}

CDC Sink uses deterministic names derived from the task configuration:

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
slot name. This allows multiple CDC Sink tasks to run on the same PostgreSQL
instance without naming conflicts.
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
