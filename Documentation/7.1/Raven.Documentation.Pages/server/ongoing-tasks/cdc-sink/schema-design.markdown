# CDC Sink: Schema Design
---

{NOTE: }

* CDC Sink maps a relational schema to a RavenDB document model through configuration.
  This page explains the three building blocks — root tables, embedded tables, and
  linked tables — and how to combine them.

* In this page:
  * [Root Tables](../../../server/ongoing-tasks/cdc-sink/schema-design#root-tables)
  * [Embedded Tables](../../../server/ongoing-tasks/cdc-sink/schema-design#embedded-tables)
  * [Linked Tables](../../../server/ongoing-tasks/cdc-sink/schema-design#linked-tables)
  * [Primary Key and Join Column Requirements](../../../server/ongoing-tasks/cdc-sink/schema-design#primary-key-and-join-column-requirements)
  * [Multi-Level Nesting](../../../server/ongoing-tasks/cdc-sink/schema-design#multi-level-nesting)
  * [Relation Types](../../../server/ongoing-tasks/cdc-sink/schema-design#relation-types)
  * [Choosing Between Embedded and Linked](../../../server/ongoing-tasks/cdc-sink/schema-design#choosing-between-embedded-and-linked)

{NOTE/}

---

{PANEL: Root Tables}

A **root table** maps a SQL table to a RavenDB collection. Each row in the SQL table
becomes one document.

    new CdcSinkTableConfig
    {
        Name = "Orders",                      // RavenDB collection name
        SourceTableSchema = "public",         // SQL schema (optional, default: "public")
        SourceTableName = "orders",           // SQL table name
        PrimaryKeyColumns = ["id"],           // Used for document ID generation
        ColumnsMapping = new Dictionary<string, string>
        {
            { "id", "Id" },
            { "customer_name", "CustomerName" },
            { "total", "Total" }
        }
    }

**Document ID generation:** `{CollectionName}/{pk1}/{pk2}/...`
A row with `id = 42` and collection `Orders` becomes document `Orders/42`.
A composite PK `(region, id)` with values `(US, 42)` becomes `Orders/US/42`.

**Column mapping:** Only mapped columns appear in the document. Unmapped columns are
still available in patch scripts via `$row` but are not stored in the document.

{PANEL/}

---

{PANEL: Embedded Tables}

An **embedded table** creates nested data within a parent document. For example, a
SQL `order_lines` table becomes an array inside each `Orders` document.

    new CdcSinkTableConfig
    {
        Name = "Orders",
        SourceTableName = "orders",
        PrimaryKeyColumns = ["id"],
        ColumnsMapping = { { "id", "Id" }, { "customer_name", "CustomerName" } },
        EmbeddedTables =
        [
            new CdcSinkEmbeddedTableConfig
            {
                SourceTableName = "order_lines",
                PropertyName = "Lines",                   // Property in parent document
                Type = CdcSinkRelationType.Array,         // Array of items
                JoinColumns = ["order_id"],               // FK referencing parent's PK
                PrimaryKeyColumns = ["line_id"],          // Used to match items on update/delete
                ColumnsMapping =
                {
                    { "line_id", "LineId" },
                    { "product", "Product" },
                    { "quantity", "Quantity" }
                }
            }
        ]
    }

This produces documents like:

    {
        "Id": 1,
        "CustomerName": "Alice",
        "Lines": [
            { "LineId": 1, "Product": "Apples", "Quantity": 5 },
            { "LineId": 2, "Product": "Bananas", "Quantity": 3 }
        ],
        "@metadata": { "@collection": "Orders" }
    }

{PANEL/}

---

{PANEL: Linked Tables}

A **linked table** creates a document ID reference rather than embedding data.
A foreign key in the source row becomes a RavenDB document ID.

    new CdcSinkTableConfig
    {
        Name = "Orders",
        SourceTableName = "orders",
        PrimaryKeyColumns = ["id"],
        ColumnsMapping = { { "id", "Id" }, { "customer_id", "CustomerId" } },
        LinkedTables =
        [
            new CdcSinkLinkedTableConfig
            {
                SourceTableName = "customers",
                PropertyName = "Customer",                // Property in parent document
                LinkedCollectionName = "Customers",       // Target collection for ID
                Type = CdcSinkRelationType.Value,         // Single reference
                JoinColumns = ["customer_id"]             // FK used to build the ID
            }
        ]
    }

With `customer_id = 42`, the document gets `"Customer": "Customers/42"`.

{PANEL/}

---

{PANEL: Primary Key and Join Column Requirements}

### Root Tables

The `PrimaryKeyColumns` list defines which SQL columns are used to generate the document ID.
All PK columns must be present in every INSERT, UPDATE, and DELETE event.

### Embedded Tables (One Level)

An embedded table needs:

* **PrimaryKeyColumns** — Used to match items within the parent's array for UPDATE and DELETE
* **JoinColumns** — Foreign key referencing the parent's `PrimaryKeyColumns`

The `JoinColumns` must exactly match the parent's `PrimaryKeyColumns`:

| Parent PK | Required JoinColumns | Valid? |
|-----------|---------------------|--------|
| `[id]` | `[order_id]` where `order_id` = parent's `id` | ✓ |
| `[id, year]` | `[order_id, order_year]` | ✓ |
| `[id]` | `[customer_id, order_id]` | ✗ Extra column not from parent PK |
| `[id, year]` | `[order_id]` | ✗ Missing `order_year` |

### DELETE Events and REPLICA IDENTITY

For DELETE events, the source database must include the join column values so CDC Sink
can route the delete to the correct parent document.

If the join column is not part of the SQL table's primary key, the source database may
need additional configuration to include it in DELETE events.

See [REPLICA IDENTITY](../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity) for
the PostgreSQL-specific requirement and how CDC Sink handles it automatically.

{PANEL/}

---

{PANEL: Multi-Level Nesting}

Embedded tables can themselves have embedded tables, creating arbitrarily deep hierarchies.

**Example: Company → Departments → Employees**

    new CdcSinkTableConfig
    {
        Name = "Companies",
        SourceTableName = "companies",
        PrimaryKeyColumns = ["company_id"],
        ColumnsMapping = { { "company_id", "CompanyId" }, { "name", "Name" } },
        EmbeddedTables =
        [
            new CdcSinkEmbeddedTableConfig
            {
                SourceTableName = "departments",
                PropertyName = "Departments",
                Type = CdcSinkRelationType.Array,
                JoinColumns = ["company_id"],              // Root FK
                PrimaryKeyColumns = ["dept_id"],
                ColumnsMapping = { { "dept_id", "DeptId" }, { "dept_name", "DeptName" } },
                EmbeddedTables =
                [
                    new CdcSinkEmbeddedTableConfig
                    {
                        SourceTableName = "employees",
                        PropertyName = "Employees",
                        Type = CdcSinkRelationType.Array,
                        JoinColumns = ["company_id", "dept_id"],  // Root FK + parent FK
                        PrimaryKeyColumns = ["emp_id"],
                        ColumnsMapping = { { "emp_id", "EmpId" }, { "emp_name", "EmpName" } }
                    }
                ]
            }
        ]
    }

**Critical requirement for deep nesting:** All descendant tables must carry the root
table's primary key as a denormalized column. The `employees` table must have
`company_id` even though it joins directly to `departments` via `dept_id`.

This is required because CDC Sink needs to route every row to the correct root document
in a single pass, without additional lookups.

**SQL schema to support this:**

    CREATE TABLE employees (
        company_id INT NOT NULL,   -- Denormalized root FK
        dept_id    INT NOT NULL,   -- Parent FK
        emp_id     INT NOT NULL,   -- Local PK
        emp_name   VARCHAR(200),
        PRIMARY KEY (company_id, dept_id, emp_id)
    );

Including all routing columns in the primary key also avoids the need for REPLICA IDENTITY
configuration — the default DELETE events include all PK columns.

{PANEL/}

---

{PANEL: Relation Types}

The `Type` property on embedded and linked tables controls the document structure:

| Type | Use Case | Document Structure |
|------|----------|--------------------|
| `Array` | One-to-many: parent has many children | `"Lines": [{ ... }, { ... }]` |
| `Map` | One-to-many with direct key lookup | `"Lines": { "1": { ... }, "2": { ... } }` |
| `Value` | Many-to-one: parent has one child/reference | `"Customer": { ... }` or `"Customer": "Customers/42"` |

**Array** — Items are matched by `PrimaryKeyColumns` for UPDATE and DELETE.
Use when you need to iterate over all items.

**Map** — Items are stored as a JSON object keyed by the primary key value(s).
Use when you need fast direct-key access within the document.

**Value** — Stores a single embedded object or document reference.
Use for many-to-one relationships (many orders share one customer).

{PANEL/}

---

{PANEL: Choosing Between Embedded and Linked}

| Consideration | Embedded | Linked |
|--------------|----------|--------|
| Data location | Stored inside parent document | Stored in a separate document |
| Access pattern | Read parent to get all data | Load parent, then load referenced doc |
| Updates | Automatic via CDC | Automatic via CDC for each table |
| Document size | Grows with embedded items | Parent stays small |
| Use case | Parent owns child (orders own lines) | Independent entities (orders reference customers) |

**Use embedded tables** when:
* The child entity has no meaning outside the parent (order lines without an order)
* You always read the parent and child together
* You want a single-document read

**Use linked tables** when:
* The referenced entity is independently meaningful (customers exist without orders)
* The referenced entity is shared by many parents
* You want RavenDB's include loading to handle the join

{PANEL/}

---

## Related Articles

### CDC Sink

- [Embedded Tables](../../../server/ongoing-tasks/cdc-sink/embedded-tables)
- [Linked Tables](../../../server/ongoing-tasks/cdc-sink/linked-tables)
- [Column Mapping](../../../server/ongoing-tasks/cdc-sink/column-mapping)
- [Configuration Reference](../../../server/ongoing-tasks/cdc-sink/configuration-reference)

### PostgreSQL

- [REPLICA IDENTITY](../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity)
