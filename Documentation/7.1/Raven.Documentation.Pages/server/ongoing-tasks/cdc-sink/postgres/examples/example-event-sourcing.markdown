# CDC Sink Example: Event Sourcing with Aggregation
---

{NOTE: }

* This example shows how to use CDC Sink patches to maintain a computed aggregate
  on a RavenDB document as individual event rows arrive from PostgreSQL.

* In this page:
  * [Source Schema](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-event-sourcing#source-schema)
  * [Goal](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-event-sourcing#goal)
  * [Task Configuration](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-event-sourcing#task-configuration)
  * [Resulting Documents](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-event-sourcing#resulting-documents)
  * [Handling Deletes](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-event-sourcing#handling-deletes)

{NOTE/}

---

{PANEL: Source Schema}

An accounts table and a transactions table:

    CREATE TABLE accounts (
        account_id SERIAL PRIMARY KEY,
        owner      TEXT NOT NULL,
        currency   TEXT NOT NULL DEFAULT 'USD'
    );

    CREATE TABLE transactions (
        txn_id     SERIAL PRIMARY KEY,
        account_id INT NOT NULL REFERENCES accounts(account_id),
        amount     NUMERIC(12,2) NOT NULL,
        type       TEXT NOT NULL,   -- 'credit' or 'debit'
        created_at TIMESTAMPTZ DEFAULT now()
    );

{PANEL/}

---

{PANEL: Goal}

Store each account as a RavenDB document with a `Balance` field that reflects the
running total of all transactions. Transaction rows are embedded as an array for
history, and `Balance` is maintained using patch logic.

{PANEL/}

---

{PANEL: Task Configuration}

    var config = new CdcSinkConfiguration
    {
        Name = "AccountsSync",
        ConnectionStringName = "MyPostgresConnection",
        Tables = new List<CdcSinkTableConfig>
        {
            new CdcSinkTableConfig
            {
                Name = "Accounts",
                SourceTableName = "accounts",
                PrimaryKeyColumns = new List<string> { "account_id" },
                ColumnsMapping = new Dictionary<string, string>
                {
                    { "account_id", "AccountId" },
                    { "owner",      "Owner" },
                    { "currency",   "Currency" }
                },
                EmbeddedTables = new List<CdcSinkEmbeddedTableConfig>
                {
                    new CdcSinkEmbeddedTableConfig
                    {
                        SourceTableName = "transactions",
                        PropertyName = "Transactions",
                        Type = CdcSinkRelationType.Array,
                        JoinColumns = new List<string> { "account_id" },
                        PrimaryKeyColumns = new List<string> { "txn_id" },
                        ColumnsMapping = new Dictionary<string, string>
                        {
                            { "txn_id",     "TxnId" },
                            { "amount",     "Amount" },
                            { "type",       "Type" },
                            { "created_at", "CreatedAt" }
                        },
                        // Patch runs on the parent document for INSERT/UPDATE
                        Patch = @"
                            const oldAmount = $old?.Amount || 0;
                            const newAmount = $row.amount || 0;
                            const sign = $row.type === 'credit' ? 1 : -1;
                            const oldSign = $old?.Type === 'credit' ? 1 : -1;
                            this.Balance = (this.Balance || 0)
                                - (oldSign * oldAmount)
                                + (sign * newAmount);
                        ",
                        OnDelete = new CdcSinkOnDeleteConfig
                        {
                            Patch = @"
                                const deletedAmount = $old?.Amount || 0;
                                const sign = $old?.Type === 'credit' ? 1 : -1;
                                this.Balance = (this.Balance || 0) - (sign * deletedAmount);
                            "
                        }
                    }
                }
            }
        }
    };

    await store.Maintenance.SendAsync(new AddCdcSinkOperation(config));

{PANEL/}

---

{PANEL: Resulting Documents}

After three transactions (credit 100, debit 30, credit 50):

    {
        "AccountId": 1,
        "Owner": "Alice",
        "Currency": "USD",
        "Balance": 120.00,
        "Transactions": [
            { "TxnId": 1, "Amount": 100.00, "Type": "credit", "CreatedAt": "..." },
            { "TxnId": 2, "Amount": 30.00,  "Type": "debit",  "CreatedAt": "..." },
            { "TxnId": 3, "Amount": 50.00,  "Type": "credit", "CreatedAt": "..." }
        ],
        "@metadata": { "@collection": "Accounts" }
    }

{PANEL/}

---

{PANEL: Handling Deletes}

The `OnDelete.Patch` reverses the contribution of the deleted transaction to
`Balance`. This uses `$old` (the embedded item's last known state) rather than
`$row`, because for a DELETE event the embedded item's mapped values are in `$old`.

Without the `OnDelete.Patch`, deleting a transaction row from SQL would remove
it from the `Transactions` array but leave `Balance` stale. The delete patch
keeps `Balance` consistent.

{NOTE: }
The patch uses delta logic (`$old` → `$row`) for idempotency. If a change is
re-applied after a failover, `$old` still reflects the state before the original
update, so the delta produces the same result.
See [Failover and Consistency](../../../../../server/ongoing-tasks/cdc-sink/failover-and-consistency).
{NOTE/}

{PANEL/}

---

## Related Articles

### CDC Sink Examples

- [Simple Migration](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-simple-migration)
- [Denormalization](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-denormalization)
- [Complex Nesting](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-complex-nesting)

### CDC Sink

- [Patching](../../../../../server/ongoing-tasks/cdc-sink/patching)
- [Delete Strategies](../../../../../server/ongoing-tasks/cdc-sink/delete-strategies)
- [Failover and Consistency](../../../../../server/ongoing-tasks/cdc-sink/failover-and-consistency)
