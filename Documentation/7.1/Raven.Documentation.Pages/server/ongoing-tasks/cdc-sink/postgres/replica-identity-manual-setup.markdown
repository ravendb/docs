# CDC Sink for PostgreSQL: Manual REPLICA IDENTITY Setup
---

{NOTE: }

* When CDC Sink does not have permission to alter tables, a database administrator
  must configure `REPLICA IDENTITY` manually before the task starts.

* In this page:
  * [When Manual Setup Is Required](../../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity-manual-setup#when-manual-setup-is-required)
  * [Setting REPLICA IDENTITY FULL](../../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity-manual-setup#setting-replica-identity-full)
  * [Using an Index Instead of FULL](../../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity-manual-setup#using-an-index-instead-of-full)
  * [Verifying the Configuration](../../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity-manual-setup#verifying-the-configuration)

{NOTE/}

---

{PANEL: When Manual Setup Is Required}

CDC Sink attempts to set `REPLICA IDENTITY FULL` automatically on any embedded
table whose join columns are not part of the primary key. This requires:

* The CDC Sink user owns the table, **or**
* The CDC Sink user has `SUPERUSER`

If neither condition is met, CDC Sink will start but embedded table deletes may
not work correctly. A database administrator must configure `REPLICA IDENTITY`
manually.

{PANEL/}

---

{PANEL: Setting REPLICA IDENTITY FULL}

The simplest approach is to set `REPLICA IDENTITY FULL` on all embedded tables
that CDC Sink will replicate:

    ALTER TABLE order_lines REPLICA IDENTITY FULL;
    ALTER TABLE line_attributes REPLICA IDENTITY FULL;

`REPLICA IDENTITY FULL` includes all column values in DELETE and UPDATE events.
This is the most compatible option but increases WAL volume for tables with many
or large columns.

{PANEL/}

---

{PANEL: Using an Index Instead of FULL}

If WAL size is a concern, you can use a specific unique index that covers both
the join columns and PK columns of the embedded table.

**Step 1: Create a unique index covering the required columns**

    -- For order_lines: join column is order_id, PK is line_id
    CREATE UNIQUE INDEX order_lines_replica_idx
        ON order_lines (order_id, line_id);

**Step 2: Set REPLICA IDENTITY to use this index**

    ALTER TABLE order_lines
        REPLICA IDENTITY USING INDEX order_lines_replica_idx;

This instructs PostgreSQL to include only those indexed columns in DELETE and
UPDATE events, rather than all columns.

{NOTE: }
The index must be `UNIQUE` and `NOT DEFERRABLE`. It cannot include expressions
or partial predicates. All columns in the index must be `NOT NULL`.
{NOTE/}

{PANEL/}

---

{PANEL: Verifying the Configuration}

Confirm that the desired `REPLICA IDENTITY` mode is set:

    SELECT c.relname, c.relreplident,
           CASE c.relreplident
               WHEN 'd' THEN 'DEFAULT'
               WHEN 'f' THEN 'FULL'
               WHEN 'i' THEN 'INDEX'
               WHEN 'n' THEN 'NOTHING'
           END AS mode
    FROM pg_class c
    JOIN pg_namespace n ON n.oid = c.relnamespace
    WHERE c.relkind = 'r'
      AND n.nspname = 'public'
    ORDER BY c.relname;

Tables configured with `FULL` or `INDEX` are ready for CDC Sink embedded table
delete processing.

{PANEL/}

---

## Related Articles

### CDC Sink for PostgreSQL

- [REPLICA IDENTITY](../../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity)
- [Initial Setup](../../../../server/ongoing-tasks/cdc-sink/postgres/initial-setup)
- [Embedded Tables](../../../../server/ongoing-tasks/cdc-sink/embedded-tables)
