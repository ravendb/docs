# CDC Sink for PostgreSQL: Monitoring PostgreSQL
---

{NOTE: }

* This page covers PostgreSQL-side monitoring for CDC Sink — replication slot health,
  lag, and WAL usage.

* For RavenDB-side monitoring, see [Monitoring](../../../../server/ongoing-tasks/cdc-sink/monitoring).

* In this page:
  * [Replication Slot Health](../../../../server/ongoing-tasks/cdc-sink/postgres/monitoring-postgres#replication-slot-health)
  * [Replication Lag](../../../../server/ongoing-tasks/cdc-sink/postgres/monitoring-postgres#replication-lag)
  * [WAL Disk Usage](../../../../server/ongoing-tasks/cdc-sink/postgres/monitoring-postgres#wal-disk-usage)

{NOTE/}

---

{PANEL: Replication Slot Health}

Check that CDC Sink replication slots are active and being consumed:

    SELECT slot_name, active, confirmed_flush_lsn
    FROM pg_replication_slots
    WHERE slot_name LIKE 'rvn_cdc_s_%';

| Column | Meaning |
|--------|---------|
| `active` | `true` if CDC Sink is connected and consuming; `false` if the connection is down |
| `confirmed_flush_lsn` | The LSN up to which changes have been confirmed as consumed |

A slot with `active = false` means CDC Sink is not currently connected. This is
expected during failover or when the task is paused. If the slot remains inactive
for an extended period, investigate the task state in RavenDB Studio.

{PANEL/}

---

{PANEL: Replication Lag}

Replication lag measures how far behind the slot is relative to the current WAL position:

    SELECT slot_name,
           pg_current_wal_lsn() - confirmed_flush_lsn AS lag_bytes
    FROM pg_replication_slots
    WHERE slot_name LIKE 'rvn_cdc_s_%';

A consistently growing `lag_bytes` means CDC Sink is not keeping up with the rate
of changes in the source database. Consider:

* Increasing `CdcSink.MaxBatchSize` to process more changes per batch
* Reducing load on the source database
* Checking the per-table processing statistics in the Management Studio for slow scripts —
  complex patch scripts are a common cause of processing slowdowns

{PANEL/}

---

{PANEL: WAL Disk Usage}

PostgreSQL retains WAL segments until all replication slots have consumed them.
An inactive or slow slot can cause WAL to accumulate on disk.

Check approximate WAL retained per slot:

    SELECT slot_name, active,
           pg_wal_lsn_diff(pg_current_wal_lsn(), restart_lsn) AS retained_wal_bytes
    FROM pg_replication_slots
    WHERE slot_name LIKE 'rvn_cdc_s_%';

High `retained_wal_bytes` on an inactive slot indicates the slot is not being
consumed and is holding WAL. If the slot corresponds to a deleted or abandoned
CDC Sink task, drop it.
See [Cleanup and Maintenance](../../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance).

{PANEL/}

---

## Related Articles

### CDC Sink for PostgreSQL

- [Cleanup and Maintenance](../../../../server/ongoing-tasks/cdc-sink/postgres/cleanup-and-maintenance)
- [Troubleshooting](../../../../server/ongoing-tasks/cdc-sink/troubleshooting)

### CDC Sink

- [Monitoring](../../../../server/ongoing-tasks/cdc-sink/monitoring)
- [Failover and Consistency](../../../../server/ongoing-tasks/cdc-sink/failover-and-consistency)
