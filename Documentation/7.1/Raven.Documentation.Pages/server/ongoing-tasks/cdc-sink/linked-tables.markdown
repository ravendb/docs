# CDC Sink: Linked Tables
---

{NOTE: }

* A **linked table** creates a document ID reference in the parent document rather
  than embedding data. A foreign key value becomes a RavenDB document ID pointing
  to a related collection.

* In this page:
  * [Basic Configuration](../../../server/ongoing-tasks/cdc-sink/linked-tables#basic-configuration)
  * [Composite Foreign Keys](../../../server/ongoing-tasks/cdc-sink/linked-tables#composite-foreign-keys)
  * [Array References](../../../server/ongoing-tasks/cdc-sink/linked-tables#array-references)
  * [Linked vs Embedded](../../../server/ongoing-tasks/cdc-sink/linked-tables#linked-vs-embedded)

{NOTE/}

---

{PANEL: Basic Configuration}

`CdcSinkLinkedTableConfig` is placed inside a root table's `LinkedTables` list:

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
                PropertyName = "Customer",              // Property name in document
                LinkedCollectionName = "Customers",     // Target collection for ID
                Type = CdcSinkRelationType.Value,       // Single reference
                JoinColumns = ["customer_id"]           // FK used to build the document ID
            }
        ]
    }

With `customer_id = 42`, the Orders document gets:

    {
        "Id": 1,
        "CustomerId": 42,
        "Customer": "Customers/42"
    }

The `Customer` property is a RavenDB document ID. Load the referenced document using
[includes](../../../client-api/session/loading-entities#load-with-includes) to avoid
a second network call.

{PANEL/}

---

{PANEL: Composite Foreign Keys}

When the target table has a composite primary key, the linked reference includes
all parts of that key:

    new CdcSinkLinkedTableConfig
    {
        SourceTableName = "customers",
        PropertyName = "Customer",
        LinkedCollectionName = "Customers",
        Type = CdcSinkRelationType.Value,
        JoinColumns = ["customer_region", "customer_id"]  // Must match Customers PK order
    }

With `customer_region = 'US'` and `customer_id = 42`, the document gets:

    "Customer": "Customers/US/42"

{PANEL/}

---

{PANEL: Array References}

Use `Type = CdcSinkRelationType.Array` for one-to-many references, where a parent
row has multiple foreign keys pointing to the same collection:

    new CdcSinkLinkedTableConfig
    {
        SourceTableName = "tags",
        PropertyName = "Tags",
        LinkedCollectionName = "Tags",
        Type = CdcSinkRelationType.Array,
        JoinColumns = ["tag_id"]
    }

This creates an array of document references:

    {
        "Tags": ["Tags/primary", "Tags/urgent", "Tags/follow-up"]
    }

{PANEL/}

---

{PANEL: Linked vs Embedded}

| Consideration | Embedded | Linked |
|--------------|----------|--------|
| Data stored | Full nested object/array inside document | Document ID reference only |
| Load pattern | Single document load | Load parent + include references |
| Document size | Grows with embedded items | Parent document stays small |
| Updates to referenced data | Reflected via CDC | Reflected via CDC on the referenced collection |
| Independence | Child has no meaning without parent | Referenced entity exists independently |
| Typical use | Orders own LineItems | Orders reference Customers |

**Choose embedded** when the child data belongs to the parent and is always read
together with it.

**Choose linked** when the referenced entity is independently meaningful and shared
across many parents, and you want to avoid duplicating data.

{PANEL/}

---

## Related Articles

### CDC Sink

- [Schema Design](../../../server/ongoing-tasks/cdc-sink/schema-design)
- [Embedded Tables](../../../server/ongoing-tasks/cdc-sink/embedded-tables)
- [Configuration Reference](../../../server/ongoing-tasks/cdc-sink/configuration-reference)

### Client API

- [Load with Includes](../../../client-api/session/loading-entities#load-with-includes)
