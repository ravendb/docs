# CDC Sink Example: Denormalization with Embedded Tables
---

{NOTE: }

* This example shows how to merge a normalized SQL schema (orders + order_lines)
  into denormalized RavenDB documents with embedded arrays.

* In this page:
  * [Source Schema](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-denormalization#source-schema)
  * [REPLICA IDENTITY Setup](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-denormalization#replica-identity-setup)
  * [Task Configuration](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-denormalization#task-configuration)
  * [Resulting Documents](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-denormalization#resulting-documents)
  * [What Happens on Change Events](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-denormalization#what-happens-on-change-events)

{NOTE/}

---

{PANEL: Source Schema}

    CREATE TABLE orders (
        order_id   SERIAL PRIMARY KEY,
        customer   TEXT NOT NULL,
        status     TEXT NOT NULL DEFAULT 'pending',
        created_at TIMESTAMPTZ DEFAULT now()
    );

    CREATE TABLE order_lines (
        line_id    SERIAL PRIMARY KEY,
        order_id   INT NOT NULL REFERENCES orders(order_id),
        product    TEXT NOT NULL,
        qty        INT NOT NULL,
        unit_price NUMERIC(10,2) NOT NULL
    );

{PANEL/}

---

{PANEL: REPLICA IDENTITY Setup}

`order_lines` has a surrogate PK (`line_id`). The join column `order_id` is not
part of the primary key. Without `REPLICA IDENTITY FULL`, DELETE events for
`order_lines` rows would not include `order_id`, and CDC Sink could not find
the parent document.

    ALTER TABLE order_lines REPLICA IDENTITY FULL;

See [REPLICA IDENTITY](../../../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity).

{PANEL/}

---

{PANEL: Task Configuration}

    var config = new CdcSinkConfiguration
    {
        Name = "OrdersSync",
        ConnectionStringName = "MyPostgresConnection",
        Tables = new List<CdcSinkTableConfig>
        {
            new CdcSinkTableConfig
            {
                Name = "Orders",
                SourceTableName = "orders",
                PrimaryKeyColumns = new List<string> { "order_id" },
                ColumnsMapping = new Dictionary<string, string>
                {
                    { "order_id",   "OrderId" },
                    { "customer",   "Customer" },
                    { "status",     "Status" },
                    { "created_at", "CreatedAt" }
                },
                EmbeddedTables = new List<CdcSinkEmbeddedTableConfig>
                {
                    new CdcSinkEmbeddedTableConfig
                    {
                        SourceTableName = "order_lines",
                        PropertyName = "Lines",
                        Type = CdcSinkRelationType.Array,
                        JoinColumns = new List<string> { "order_id" },
                        PrimaryKeyColumns = new List<string> { "line_id" },
                        ColumnsMapping = new Dictionary<string, string>
                        {
                            { "line_id",    "LineId" },
                            { "product",    "Product" },
                            { "qty",        "Qty" },
                            { "unit_price", "UnitPrice" }
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

SQL rows:

    orders:      order_id=1, customer='Acme Corp', status='pending'
    order_lines: line_id=1, order_id=1, product='Widget A', qty=3, unit_price=9.99
    order_lines: line_id=2, order_id=1, product='Widget B', qty=1, unit_price=24.99

RavenDB document `orders/1`:

    {
        "OrderId": 1,
        "Customer": "Acme Corp",
        "Status": "pending",
        "CreatedAt": "2024-06-01T09:00:00+00:00",
        "Lines": [
            {
                "LineId": 1,
                "Product": "Widget A",
                "Qty": 3,
                "UnitPrice": 9.99
            },
            {
                "LineId": 2,
                "Product": "Widget B",
                "Qty": 1,
                "UnitPrice": 24.99
            }
        ],
        "@metadata": { "@collection": "Orders" }
    }

{PANEL/}

---

{PANEL: What Happens on Change Events}

**INSERT into `order_lines`:**
A new item is appended to the `Lines` array.

**UPDATE to `order_lines`:**
CDC Sink finds the item by `line_id` within the `Lines` array and updates its properties.

**DELETE from `order_lines`:**
CDC Sink finds the item by `line_id` and removes it from the array.
(Requires `REPLICA IDENTITY FULL` as configured above.)

**UPDATE to `orders`:**
Only the root document properties (`Status`, etc.) are updated. The `Lines` array
is not affected.

**DELETE from `orders`:**
The entire `orders/1` document is deleted, including all embedded lines.

{PANEL/}

---

## Related Articles

### CDC Sink Examples

- [Simple Migration](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-simple-migration)
- [Complex Nesting](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-complex-nesting)

### CDC Sink

- [Embedded Tables](../../../../../server/ongoing-tasks/cdc-sink/embedded-tables)
- [REPLICA IDENTITY](../../../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity)
- [Delete Strategies](../../../../../server/ongoing-tasks/cdc-sink/delete-strategies)
