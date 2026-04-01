# CDC Sink Example: Complex Nesting with Linked Tables
---

{NOTE: }

* This example shows a multi-level embedded table structure combined with linked
  table references, representing a product catalog with variants, attributes, and
  a category reference.

* In this page:
  * [Source Schema](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-complex-nesting#source-schema)
  * [REPLICA IDENTITY Setup](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-complex-nesting#replica-identity-setup)
  * [Task Configuration](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-complex-nesting#task-configuration)
  * [Resulting Documents](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-complex-nesting#resulting-documents)

{NOTE/}

---

{PANEL: Source Schema}

    CREATE TABLE categories (
        category_id SERIAL PRIMARY KEY,
        name        TEXT NOT NULL
    );

    CREATE TABLE products (
        product_id  SERIAL PRIMARY KEY,
        name        TEXT NOT NULL,
        category_id INT REFERENCES categories(category_id)
    );

    CREATE TABLE product_variants (
        variant_id  SERIAL PRIMARY KEY,
        product_id  INT NOT NULL REFERENCES products(product_id),
        sku         TEXT NOT NULL,
        price       NUMERIC(10,2)
    );

    CREATE TABLE variant_attributes (
        attr_id    SERIAL PRIMARY KEY,
        product_id INT NOT NULL,    -- denormalized root PK (required for deep nesting)
        variant_id INT NOT NULL REFERENCES product_variants(variant_id),
        attr_name  TEXT NOT NULL,
        attr_value TEXT NOT NULL
    );

{PANEL/}

---

{PANEL: REPLICA IDENTITY Setup}

Both `product_variants` and `variant_attributes` have surrogate PKs with join
columns that are not part of their primary keys:

    ALTER TABLE product_variants REPLICA IDENTITY FULL;
    ALTER TABLE variant_attributes REPLICA IDENTITY FULL;

{PANEL/}

---

{PANEL: Task Configuration}

    var config = new CdcSinkConfiguration
    {
        Name = "ProductCatalogSync",
        ConnectionStringName = "MyPostgresConnection",
        Tables = new List<CdcSinkTableConfig>
        {
            new CdcSinkTableConfig
            {
                Name = "Products",
                SourceTableName = "products",
                PrimaryKeyColumns = new List<string> { "product_id" },
                ColumnsMapping = new Dictionary<string, string>
                {
                    { "product_id", "ProductId" },
                    { "name",       "Name" }
                },
                // Linked table: category_id FK → document ID in Categories collection
                LinkedTables = new List<CdcSinkLinkedTableConfig>
                {
                    new CdcSinkLinkedTableConfig
                    {
                        SourceTableName = "categories",
                        PropertyName = "Category",
                        LinkedCollectionName = "Categories",
                        Type = CdcSinkRelationType.Value,
                        JoinColumns = new List<string> { "category_id" }
                    }
                },
                EmbeddedTables = new List<CdcSinkEmbeddedTableConfig>
                {
                    new CdcSinkEmbeddedTableConfig
                    {
                        SourceTableName = "product_variants",
                        PropertyName = "Variants",
                        Type = CdcSinkRelationType.Array,
                        JoinColumns = new List<string> { "product_id" },
                        PrimaryKeyColumns = new List<string> { "variant_id" },
                        ColumnsMapping = new Dictionary<string, string>
                        {
                            { "variant_id", "VariantId" },
                            { "sku",        "Sku" },
                            { "price",      "Price" }
                        },
                        // Deep-nested: attributes within each variant
                        EmbeddedTables = new List<CdcSinkEmbeddedTableConfig>
                        {
                            new CdcSinkEmbeddedTableConfig
                            {
                                SourceTableName = "variant_attributes",
                                PropertyName = "Attributes",
                                Type = CdcSinkRelationType.Array,
                                // JoinColumns must include the ROOT PK for deep nesting
                                JoinColumns = new List<string> { "product_id", "variant_id" },
                                PrimaryKeyColumns = new List<string> { "attr_id" },
                                ColumnsMapping = new Dictionary<string, string>
                                {
                                    { "attr_id",    "AttrId" },
                                    { "attr_name",  "Name" },
                                    { "attr_value", "Value" }
                                }
                            }
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

    {
        "ProductId": 42,
        "Name": "Hiking Boot",
        "Category": "categories/3",        // linked document ID
        "Variants": [
            {
                "VariantId": 101,
                "Sku": "HB-BLK-10",
                "Price": 89.99,
                "Attributes": [
                    { "AttrId": 1, "Name": "Color", "Value": "Black" },
                    { "AttrId": 2, "Name": "Size",  "Value": "10" }
                ]
            },
            {
                "VariantId": 102,
                "Sku": "HB-BRN-11",
                "Price": 89.99,
                "Attributes": [
                    { "AttrId": 3, "Name": "Color", "Value": "Brown" },
                    { "AttrId": 4, "Name": "Size",  "Value": "11" }
                ]
            }
        ],
        "@metadata": { "@collection": "Products" }
    }

{NOTE: }
The `Categories` collection is also synced by CDC Sink (it would be a separate
root table configuration). `"categories/3"` is a standard RavenDB document ID
that enables the use of RavenDB includes when querying products.
{NOTE/}

{PANEL/}

---

## Related Articles

### CDC Sink Examples

- [Simple Migration](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-simple-migration)
- [Denormalization](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-denormalization)
- [Event Sourcing](../../../../../server/ongoing-tasks/cdc-sink/postgres/examples/example-event-sourcing)

### CDC Sink

- [Schema Design](../../../../../server/ongoing-tasks/cdc-sink/schema-design)
- [Embedded Tables](../../../../../server/ongoing-tasks/cdc-sink/embedded-tables)
- [Linked Tables](../../../../../server/ongoing-tasks/cdc-sink/linked-tables)
- [REPLICA IDENTITY](../../../../../server/ongoing-tasks/cdc-sink/postgres/replica-identity)
