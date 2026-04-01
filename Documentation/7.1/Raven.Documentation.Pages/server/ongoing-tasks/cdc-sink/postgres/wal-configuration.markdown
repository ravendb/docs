# CDC Sink for PostgreSQL: WAL Configuration
---

{NOTE: }

* CDC Sink uses PostgreSQL **logical replication**, which requires the Write-Ahead Log
  (WAL) to be configured at the `logical` level.

* This page explains how to verify the current setting and change it if needed.

* In this page:
  * [Check Current WAL Level](../../../../server/ongoing-tasks/cdc-sink/postgres/wal-configuration#check-current-wal-level)
  * [Enable Logical Replication](../../../../server/ongoing-tasks/cdc-sink/postgres/wal-configuration#enable-logical-replication)
  * [Other Required Settings](../../../../server/ongoing-tasks/cdc-sink/postgres/wal-configuration#other-required-settings)

{NOTE/}

---

{PANEL: Check Current WAL Level}

Connect to your PostgreSQL instance and run:

    SHOW wal_level;

If the result is `logical`, no changes are needed.

If the result is `replica` or `minimal`, logical replication is not enabled and
must be configured before CDC Sink can run.

{PANEL/}

---

{PANEL: Enable Logical Replication}

Edit `postgresql.conf` and set the following:

    wal_level = logical

This change requires a **PostgreSQL restart**.

After restarting, verify the change took effect:

    SHOW wal_level;
    -- Should return: logical

{WARNING: }
Changing `wal_level` requires a full server restart, not just a configuration reload.
Plan for a brief maintenance window.
{WARNING/}

{PANEL/}

---

{PANEL: Other Required Settings}

CDC Sink uses one replication slot per task. Ensure the following settings are
sufficient for the number of CDC Sink tasks you plan to run:

    max_replication_slots = 10   -- at least 1 per CDC Sink task
    max_wal_senders = 10         -- at least 1 per active replication connection

The defaults in a standard PostgreSQL installation are typically sufficient for
a small number of tasks, but you may need to increase them if you have many
concurrent CDC Sink tasks or other replication consumers.

Check current values:

    SHOW max_replication_slots;
    SHOW max_wal_senders;

These settings also require a **server restart** if changed.

{PANEL/}

---

## Related Articles

### CDC Sink for PostgreSQL

- [Prerequisites Checklist](../../../../server/ongoing-tasks/cdc-sink/postgres/prerequisites-checklist)
- [Permissions and Roles](../../../../server/ongoing-tasks/cdc-sink/postgres/permissions-and-roles)
- [Cleanup and Maintenance](../../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance)
