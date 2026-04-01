# CDC Sink Example: Simple Table Migration
---

{NOTE: }

* This example shows the minimal setup to replicate a single SQL table into a
  RavenDB collection.

* In this page:
  * [Source Schema](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-simple-migration#source-schema)
  * [Task Configuration](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-simple-migration#task-configuration)
  * [Resulting Documents](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-simple-migration#resulting-documents)

{NOTE/}

---

{PANEL: Source Schema}

A simple customers table:

    CREATE TABLE customers (
        id         SERIAL PRIMARY KEY,
        name       TEXT NOT NULL,
        email      TEXT NOT NULL,
        created_at TIMESTAMPTZ DEFAULT now()
    );

{PANEL/}

---

{PANEL: Task Configuration}

    var config = new CdcSinkConfiguration
    {
        Name = "CustomersSync",
        ConnectionStringName = "MyPostgresConnection",
        Tables = new List<CdcSinkTableConfig>
        {
            new CdcSinkTableConfig
            {
                Name = "Customers",
                SourceTableName = "customers",
                PrimaryKeyColumns = new List<string> { "id" },
                ColumnsMapping = new Dictionary<string, string>
                {
                    { "id",         "Id" },
                    { "name",       "Name" },
                    { "email",      "Email" },
                    { "created_at", "CreatedAt" }
                }
            }
        }
    };

    await store.Maintenance.SendAsync(new AddCdcSinkOperation(config));

{PANEL/}

---

{PANEL: Resulting Documents}

SQL row:

    id=1, name='Alice', email='alice@example.com', created_at='2024-01-15 10:30:00+00'

RavenDB document in collection `Customers` with ID `customers/1`:

    {
        "Id": 1,
        "Name": "Alice",
        "Email": "alice@example.com",
        "CreatedAt": "2024-01-15T10:30:00+00:00",
        "@metadata": {
            "@collection": "Customers"
        }
    }

Document IDs are generated as `{collection}/{pk}` — for example, `customers/1`
for a row with `id = 1`.

{PANEL/}

---

## Related Articles

### CDC Sink Examples

- [Denormalization](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-denormalization)
- [Event Sourcing](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-event-sourcing)
- [Complex Nesting](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-complex-nesting)

### CDC Sink

- [Schema Design](../../../../../server/ongoing-tasks/cdc-sink/schema-design)
- [Column Mapping](../../../../../server/ongoing-tasks/cdc-sink/column-mapping)
