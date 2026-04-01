# CDC Sink for PostgreSQL: Cleanup and Maintenance
---

{NOTE: }

* When a CDC Sink task is deleted from RavenDB, the associated PostgreSQL objects
  — the replication slot and publication — are **not** automatically removed.

* These must be cleaned up manually by a database administrator.

* In this page:
  * [Why Manual Cleanup Is Required](../../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance#why-manual-cleanup-is-required)
  * [Finding Orphaned Slots and Publications](../../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance#finding-orphaned-slots-and-publications)
  * [Dropping a Replication Slot](../../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance#dropping-a-replication-slot)
  * [Dropping a Publication](../../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance#dropping-a-publication)
  * [Too Many Replication Slots](../../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance#too-many-replication-slots)
  * [Configuration Changes That Rename Slots](../../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance#configuration-changes-that-rename-slots)

{NOTE/}

---

{PANEL: Why Manual Cleanup Is Required}

An active replication slot prevents PostgreSQL from discarding WAL segments that
have not yet been consumed. If a slot is not being consumed (because the CDC Sink
task was deleted), PostgreSQL will accumulate WAL on disk indefinitely.

This can lead to:

* Disk space exhaustion on the PostgreSQL server
* Degraded performance as old WAL segments pile up

A database administrator must drop unused replication slots.

{PANEL/}

---

{PANEL: Finding Orphaned Slots and Publications}

List all CDC Sink replication slots:

    SELECT slot_name, active, confirmed_flush_lsn
    FROM pg_replication_slots
    WHERE slot_name LIKE 'rvn_cdc_s_%';

An `active = false` slot is not being consumed. Compare the list to your active CDC
Sink tasks in RavenDB — any slot whose corresponding task no longer exists is orphaned.

List all CDC Sink publications:

    SELECT p.pubname, c.relname AS table_name
    FROM pg_publication p
    JOIN pg_publication_rel pr ON pr.prpubid = p.oid
    JOIN pg_class c ON c.oid = pr.prrelid
    WHERE p.pubname LIKE 'rvn_cdc_p_%'
    ORDER BY p.pubname, c.relname;

{PANEL/}

---

{PANEL: Dropping a Replication Slot}

    SELECT pg_drop_replication_slot('rvn_cdc_s_<hash>');

{WARNING: }
You cannot drop an active replication slot (one with `active = true`). The CDC Sink
task must be stopped or deleted in RavenDB before the slot can be dropped.
{WARNING/}

{PANEL/}

---

{PANEL: Dropping a Publication}

    DROP PUBLICATION IF EXISTS "rvn_cdc_p_<hash>";

Publications are not consumed like slots — they do not accumulate data or hold WAL
segments. However, they should still be dropped to keep the database clean.

{PANEL/}

---

{PANEL: Too Many Replication Slots}

PostgreSQL limits the total number of replication slots to `max_replication_slots`.
If you exceed this limit, no new CDC Sink tasks can start (they will fail with a
connection error).

Check how many slots are in use:

    SELECT count(*) FROM pg_replication_slots;
    SHOW max_replication_slots;

To resolve this:

1. Identify inactive slots:

       SELECT slot_name, active
       FROM pg_replication_slots
       WHERE active = false;

2. Drop slots for tasks that no longer exist:

       SELECT pg_drop_replication_slot('rvn_cdc_s_<hash>');

3. If needed, increase `max_replication_slots` in `postgresql.conf` and restart
   PostgreSQL.

{PANEL/}

---

{PANEL: Configuration Changes That Rename Slots}

The replication slot name is derived from the task name, database name, and table
names. If you update a CDC Sink task in a way that changes any of these — such as
adding a table, removing a table, or renaming the task — the expected slot and
publication names change.

What happens:

* CDC Sink will look for a slot/publication with the new name
* If it has permissions, it will create them
* The old slot and publication are **not deleted** — they become orphaned

After updating a task configuration that changes table membership:

1. Let the task restart and create the new slot/publication
2. Identify the old slot (it will be inactive):

       SELECT slot_name, active
       FROM pg_replication_slots
       WHERE slot_name LIKE 'rvn_cdc_s_%'
         AND active = false;

3. Drop the old slot and publication

{PANEL/}

---

## Related Articles

### CDC Sink for PostgreSQL

- [Initial Setup](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup)
- [Monitoring PostgreSQL](../../../../server/ongoing-tasks/cdc-sink/postgres/monitoring-postgres)
- [WAL Configuration](../../../../server/ongoing-tasks/cdc-sink/postgres/wal-configuration)

### CDC Sink

- [API Reference](../../../../server/ongoing-tasks/cdc-sink/api-reference)
