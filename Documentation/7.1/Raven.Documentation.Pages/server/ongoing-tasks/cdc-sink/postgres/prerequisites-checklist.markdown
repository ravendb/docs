# CDC Sink for PostgreSQL: Prerequisites Checklist
---

{NOTE: }

* Before creating a CDC Sink task for PostgreSQL, verify that each of these requirements
  is in place.

* In this page:
  * [Source Database Requirements](../../../../server/ongoing-tasks/cdc-sink/postgres/prerequisites-checklist#source-database-requirements)
  * [User Permissions](../../../../server/ongoing-tasks/cdc-sink/postgres/prerequisites-checklist#user-permissions)
  * [Table Requirements](../../../../server/ongoing-tasks/cdc-sink/postgres/prerequisites-checklist#table-requirements)
  * [Network Access](../../../../server/ongoing-tasks/cdc-sink/postgres/prerequisites-checklist#network-access)

{NOTE/}

---

{PANEL: Source Database Requirements}

* **PostgreSQL version**: 10 or later (logical replication introduced in version 10)

* **WAL level**: `wal_level` must be set to `logical`

  Verify with:

      SHOW wal_level;

  If the result is not `logical`, see [WAL Configuration](../../../../server/ongoing-tasks/cdc-sink/postgres/wal-configuration).

* **`max_replication_slots`**: Must be at least 1 (one slot per CDC Sink task)

  Verify with:

      SHOW max_replication_slots;

* **`max_wal_senders`**: Must be at least 1

  Verify with:

      SHOW max_wal_senders;

{PANEL/}

---

{PANEL: User Permissions}

The database user in the connection string must have sufficient permissions.

**Minimum required permissions:**

    -- Replication privilege (required for logical replication)
    ALTER USER cdc_user REPLICATION;

    -- SELECT on each table CDC Sink will read
    GRANT SELECT ON TABLE orders TO cdc_user;
    GRANT SELECT ON TABLE order_lines TO cdc_user;

**Optional (allows CDC Sink to configure replication automatically):**

    -- Create/drop replication slots
    -- This requires SUPERUSER or membership in pg_replication_slot_admin (PG 14+)

    -- Create/drop publications
    -- This requires ownership of the tables being published, or SUPERUSER

If the CDC Sink user does not have permission to create publications and replication
slots, a database administrator must set them up manually.
See [Initial Setup](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup).

Full details: [Permissions and Roles](../../../../server/ongoing-tasks/cdc-sink/postgres/permissions-and-roles).

{PANEL/}

---

{PANEL: Table Requirements}

* **Primary key**: Each root table and each embedded table must have a primary key
  (or a unique index used as a replica identity).

* **REPLICA IDENTITY**: For embedded tables where the join columns are not part of
  the primary key, the table must have `REPLICA IDENTITY FULL` or a replica identity
  index that includes the join columns. Without this, DELETE events will not include
  the old row values, and CDC Sink cannot identify which embedded item to remove.

  See [REPLICA IDENTITY](../../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity).

* **Published columns**: All columns referenced in `PrimaryKeyColumns`, `JoinColumns`,
  and `ColumnsMapping` must exist in the SQL table.

{PANEL/}

---

{PANEL: Network Access}

* The RavenDB server must be able to open a TCP connection to the PostgreSQL host
  on the configured port (default: 5432).

* The connection must remain open for the duration of the replication stream.
  Ensure that firewalls, proxies, and load balancers do not terminate idle or
  long-lived connections.

{PANEL/}

---

## Related Articles

### CDC Sink for PostgreSQL

- [WAL Configuration](../../../../server/ongoing-tasks/cdc-sink/postgres/wal-configuration)
- [Permissions and Roles](../../../../server/ongoing-tasks/cdc-sink/postgres/permissions-and-roles)
- [Initial Setup](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup)
- [REPLICA IDENTITY](../../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity)
