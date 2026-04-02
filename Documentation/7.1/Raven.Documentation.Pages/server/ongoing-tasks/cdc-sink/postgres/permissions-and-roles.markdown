# CDC Sink for PostgreSQL: Permissions and Roles
---

{NOTE: }

* This page documents the PostgreSQL permissions required by the CDC Sink user,
  and how to grant them.

* In this page:
  * [Minimum Permissions](../../../../server/ongoing-tasks/cdc-sink/postgres/permissions-and-roles#minimum-permissions)
  * [Permissions for Automatic Setup](../../../../server/ongoing-tasks/cdc-sink/postgres/permissions-and-roles#permissions-for-automatic-setup)
  * [Creating a Dedicated CDC User](../../../../server/ongoing-tasks/cdc-sink/postgres/permissions-and-roles#creating-a-dedicated-cdc-user)

{NOTE/}

---

{PANEL: Minimum Permissions}

At a minimum, the CDC Sink user needs:

* **`REPLICATION`** attribute — allows the user to initiate logical replication
* **`SELECT`** on each table being replicated — required for the initial load phase

Example:

    -- Grant replication privilege
    ALTER USER cdc_user REPLICATION;

    -- Grant SELECT on each table
    GRANT SELECT ON TABLE orders TO cdc_user;
    GRANT SELECT ON TABLE order_lines TO cdc_user;
    GRANT SELECT ON TABLE customers TO cdc_user;

With only these permissions, a database administrator must manually create the
replication slot and publication before starting the CDC Sink task.
See [Initial Setup](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup).

{PANEL/}

---

{PANEL: Permissions for Automatic Setup}

If you want CDC Sink to create and manage the replication slot and publication
automatically, the user needs additional permissions:

**Create/drop replication slots:**

PostgreSQL 14+:

    GRANT pg_replication_slot_admin TO cdc_user;

PostgreSQL 10–13: Replication slot management requires `SUPERUSER`.

**Create/drop publications:**

The user must either own the tables being published, or have `SUPERUSER`.

Alternatively, you can grant `CREATE` on the database:

    GRANT CREATE ON DATABASE mydb TO cdc_user;

{WARNING: }
Granting `SUPERUSER` gives the user unrestricted access to the database server.
For production environments, prefer granting only the specific privileges listed
above rather than `SUPERUSER`.
{WARNING/}

{NOTE: }
For added security in production, consider having your database administrator create
the replication slot and publication manually with the minimal permissions shown above,
rather than granting CDC Sink the ability to manage them automatically. The CDC Sink
user then only needs `REPLICATION` privilege and `SELECT` on the relevant tables.
See [Initial Setup](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup).
{NOTE/}

{PANEL/}

---

{PANEL: Creating a Dedicated CDC User}

It is recommended to use a dedicated database user for CDC Sink rather than an
application or admin user.

Example setup:

    -- Create the user
    CREATE USER cdc_user WITH PASSWORD 'secure_password' REPLICATION;

    -- Grant SELECT on tables to replicate
    GRANT SELECT ON TABLE orders TO cdc_user;
    GRANT SELECT ON TABLE order_lines TO cdc_user;

    -- If using PostgreSQL 14+ and want automatic slot management:
    GRANT pg_replication_slot_admin TO cdc_user;

    -- If user needs to create publications (requires table ownership or superuser):
    -- Option A: Grant ownership of tables
    ALTER TABLE orders OWNER TO cdc_user;
    -- Option B: Create publications as a superuser before starting the task

{PANEL/}

---

## Related Articles

### CDC Sink for PostgreSQL

- [Prerequisites Checklist](../../../../server/ongoing-tasks/cdc-sink/postgres/prerequisites-checklist)
- [Initial Setup](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup)
- [Cleanup and Maintenance](../../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance)
