# How to Handle Document Relationships

One of the design principles that RavenDB adheres to is the idea that documents are independent,
meaning all data required to process a document is stored within the document itself. 
However, this doesn't mean there should not be relations between objects.

There are valid scenarios where we need to define relationships between objects.
By doing so, we expose ourselves to one major problem: whenever we load the containing entity,
we are going to need to load data from the referenced entities as well (unless we are not interested in them).
While the alternative of storing the whole entity in every object graph it is referenced in seems cheaper at first,
this proves to be quite costly in terms of database resources and network traffic.

RavenDB offers three elegant approaches to solve this problem. Each scenario will need to use one or more of them.
When applied correctly, they can drastically improve performance, reduce network bandwidth, and speed up development.

---

{NOTE: }

* In this page:  
  * [Denormalization](../../client-api/how-to/handle-document-relationships#denormalization)  
  * [Includes](../../client-api/how-to/handle-document-relationships#includes)  
     * [One to many includes](../../client-api/how-to/handle-document-relationships#one-to-many-includes)  
     * [Secondary level includes](../../client-api/how-to/handle-document-relationships#secondary-level-includes)  
     * [Dictionary includes](../../client-api/how-to/handle-document-relationships#dictionary-includes)  
     * [Dictionary includes: complex types](../../client-api/how-to/handle-document-relationships#dictionary-includes-complex-types)  
  * [Combining approaches](../../client-api/how-to/handle-document-relationships#combining-approaches)  
  * [Summary](../../client-api/how-to/handle-document-relationships#summary)  
{NOTE/}

---

{PANEL: Denormalization}

The easiest solution is to denormalize the data within the containing entity,
forcing it to contain the actual value of the referenced entity in addition to (or instead of) the foreign key.

Take this JSON document for example:

{CODE-BLOCK:json}
// Order document with ID: orders/1-A
{
    "customer": {
        "id": "customers/1-A",
        "name": "Itamar"
    },
    "items": [
        {
            "product": {
                "id": "products/1-A",
                "name": "Milk",
                "cost": 2.3
            },
            "quantity": 3
        }
    ]
}
{CODE-BLOCK/}

As you can see, the `Order` document now contains denormalized data from both the `Customer` and the `Product` documents which are saved elsewhere in full.
Note we won't have copied all the customer properties into the order;
instead we just clone the ones that we care about when displaying or processing an order.
This approach is called _denormalized reference_.

The denormalization approach avoids many cross document lookups and results in only the necessary data being transmitted over the network,
but it makes other scenarios more difficult. For example, consider the following entity structure as our start point:

{CODE:nodejs order@client-api/howTo/handleDocumentRelationships.js /}

{CODE:nodejs customer@client-api/howTo/handleDocumentRelationships.js /}

If we know that whenever we load an `Order` from the database we will need to know the customer's name and address,
we could decide to create a denormalized `Order.customer` field and store those details directly in the `Order` object.
Obviously, the password and other irrelevant details will not be denormalized:

{CODE:nodejs denormalized_customer@client-api/howTo/handleDocumentRelationships.js /}

There wouldn't be a direct reference between the `Order` and the `Customer`.
Instead, `Order` holds a `DenormalizedCustomer`, which contains the interesting bits from `Customer` that we need whenever we process `Order` objects.

But what happens when the user's address is changed? We will have to perform an aggregate operation to update all orders this customer has made.
What if the customer has a lot of orders or changes their address frequently? Keeping these details in sync could become very demanding on the server.
What if another process that works with orders needs a different set of customer properties?
The `DenormalizedCustomer` will need to be expanded, possibly to the point that the majority of the customer record is cloned.

{INFO:Tip}
**Denormalization** is a viable solution for rarely changing data or for data that must remain the same despite the underlying referenced data changing over time.
{INFO/}

{PANEL/}

{PANEL: Includes}

The **Includes** feature addresses the limitations of denormalization.  
Instead of one object containing copies of the properties from another object, 
it is only necessary to hold a reference to the second object, which can be:  

* a Document (as described below)  
* a [Document Revision](../../document-extensions/revisions/client-api/session/including)  
* a [Counter](../../document-extensions/counters/counters-and-other-features#including-counters)  
* a [Time series](../../document-extensions/timeseries/client-api/session/include/overview)  
* a [Compare exchange value](../../client-api/operations/compare-exchange/include-compare-exchange)  

The server can then be instructed to pre-load the referenced object at the same time that the root object is retrieved, using:

{CODE:nodejs includes_1_0@client-api/howTo/handleDocumentRelationships.js /}

Above we are asking RavenDB to retrieve the `Order` `orders/1-A`, and at the same time "include" the `Customer` referenced by the `customerId` property.
The second call to `load()` is resolved completely client side (i.e. without a second request to the RavenDB server)
because the relevant `Customer` object has already been retrieved (this is the full `Customer` object not a denormalized version). 

There is also a possibility to load multiple documents:

{CODE:nodejs includes_2_0@client-api/howTo/handleDocumentRelationships.js /}

You can also use Includes with queries:

{CODE-TABS}
{CODE-TAB:nodejs:Query(Embedded) includes_3_0@client-api/howTo/handleDocumentRelationships.js /}
{CODE-TAB:nodejs:Query(Builder) includes_3_0_builder@client-api/howTo/handleDocumentRelationships.js /}
{CODE-TAB-BLOCK:sql:RQL(Embedded)}
from "orders"
where totalPrice > 100
include customerId
{CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:sql:RQL(Builder)}
from "orders" as o
where totalPrice > 100
include customerId, counters(o,'OrderUpdateCount')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This works because RavenDB has two channels through which it can return information in response to a load request.
The first is the Results channel, through which the root object retrieved by the `load()` method call is returned.
The second is the Includes channel, through which any included documents are sent back to the client.
Client side, those included documents are not returned from the `load()` method call, but they are added to the session unit of work,
and subsequent requests to load them are served directly from the session cache, without requiring any additional queries to the server.

{NOTE Embedded and builder variants of `include` clause are essentially syntax sugar and are equivalent at the server side. /}

{INFO: }
Streaming query results does not support the includes feature.  
Learn more in [How to Stream Query Results](../../client-api/session/querying/how-to-stream-query-results#stream-related-documents).  
{INFO/}
 
---

### One to many includes

Include can be used with a many to one relationship.
In the above classes, an `Order` has a property `supplierIds` which contains an array of references to `Supplier` documents.
The following code will cause the suppliers to be pre-loaded:

{CODE:nodejs includes_4_0@client-api/howTo/handleDocumentRelationships.js /}

Alternatively, it is possible to use the fluent builder syntax.

{CODE:nodejs includes_4_0_builder@client-api/howTo/handleDocumentRelationships.js /}

The calls to `load()` within the `for` loop will not require a call to the server as the `Supplier` objects will already be loaded into the session cache.

Multi-loads are also possible:

{CODE:nodejs includes_5_0@client-api/howTo/handleDocumentRelationships.js /}

---

### Secondary level includes

An Include does not need to work only on the value of a top level property within a document.
It can be used to load a value from a secondary level.
In the classes above, the `Order` contains a `referral` property which is of the type:

{CODE:nodejs referral@client-api/howTo/handleDocumentRelationships.js /}

This class contains an identifier for a `Customer`.  
The following code will include the document referenced by that secondary level identifier:

{CODE:nodejs includes_6_0@client-api/howTo/handleDocumentRelationships.js /}

It is possible to execute the same code with the fluent builder syntax:

{CODE:nodejs includes_6_0_builder@client-api/howTo/handleDocumentRelationships.js /}

This secondary level include will also work with collections.  
The `lineItems` property holds a collection of `LineItem` objects which each contain a reference to a `Product`:

{CODE:nodejs line_item@client-api/howTo/handleDocumentRelationships.js /}

The `Product` documents can be included using the following syntax:

{CODE:nodejs includes_7_0@client-api/howTo/handleDocumentRelationships.js /}

The fluent builder syntax works here too.

{CODE:nodejs includes_7_0_builder@client-api/howTo/handleDocumentRelationships.js /}

{WARNING: }

---

### String path conventions

When using string-based includes like:

{CODE:nodejs includes_6_0@client-api/howTo/handleDocumentRelationships.js /}

you must remember to follow certain rules that must apply to the provided string path:

1.	**Dots** are used to separate properties
    e.g. `"referral.customerId"` in the example above means that our `Order` contains property `referral` and that property contains another property called `customerId`.

2. **Indexer operator** is used to indicate that property is a collection type.
    So if our `Order` has a list of LineItems and each `lineItem` contains a `productId` property, then we can create string path as follows: `"lineItems[].productId"`.

3. **Prefixes** can be used to indicate the prefix of the identifier of the document that is going to be included.
    It can be useful when working with custom or semantic identifiers.
    For example, if you have a customer stored under `customers/login@domain.com` then you can include it using `"referral.customerEmail(customers/)"` (`customers/` is the prefix here).

Learning string path rules may be useful when you will want to query database using HTTP API.

{CODE-BLOCK:json}
	curl -X GET "http://localhost:8080/databases/Northwind/docs?id=orders/1-A&include=Lines[].Product"
{CODE-BLOCK/}

{WARNING/}

---

### Dictionary includes

Dictionary keys and values can also be used when doing includes. Consider following scenario:

{CODE:nodejs person_1@client-api/howTo/handleDocumentRelationships.js /}

{CODE:nodejs includes_10_0@client-api/howTo/handleDocumentRelationships.js /}

Now we want to include all documents that are under dictionary values:

{CODE:nodejs includes_10_1@client-api/howTo/handleDocumentRelationships.js /}

The code above can be also rewritten with fluent builder syntax:

{CODE:nodejs includes_10_1_builder@client-api/howTo/handleDocumentRelationships.js /}

You can also include values from dictionary keys:

{CODE:nodejs includes_10_3@client-api/howTo/handleDocumentRelationships.js /}

Here, as well, this can be written with fluent builder syntax:

{CODE:nodejs includes_10_3_builder@client-api/howTo/handleDocumentRelationships.js /}

---

### Dictionary includes: complex types

If values in dictionary are more complex, e.g.

{CODE:nodejs person_2@client-api/howTo/handleDocumentRelationships.js /}

{CODE:nodejs includes_11_0@client-api/howTo/handleDocumentRelationships.js /}

We can also do includes on specific properties:

{CODE:nodejs includes_11_1@client-api/howTo/handleDocumentRelationships.js /}

{PANEL/}

{PANEL: Combining approaches}

It is possible to combine the above techniques.  
Using the `DenormalizedCustomer` from above and creating an order that uses it:

{CODE:nodejs order_2@client-api/howTo/handleDocumentRelationships.js /}

We have the advantages of a denormalization, a quick and simple load of an `Order`,
and the fairly static `Customer` details that are required for most processing.
But we also have the ability to easily and efficiently load the full `Customer` object when necessary using:

{CODE:nodejs includes_9_0@client-api/howTo/handleDocumentRelationships.js /}

This combining of denormalization and Includes could also be used with a list of denormalized objects.

It is possible to use Include on a query being a projection.
Includes are evaluated after the projection has been evaluated.
This opens up the possibility of implementing Tertiary Includes (i.e. retrieving documents that are referenced by documents that are referenced by the root document). 

RavenDB can support Tertiary Includes, but before resorting to them you should re-evaluate your document model.
Needing Tertiary Includes can be an indication that you are designing your documents along "Relational" lines.

{PANEL/}

{PANEL: Summary}

There are no strict rules as to when to use which approach, but the general idea is to give it a lot of thought and consider the implications each approach has.

As an example, in an e-commerce application it might be better to denormalize product names and prices into an order line object
since you want to make sure the customer sees the same price and product title in the order history.
But the customer name and addresses should probably be references rather than denormalized into the order entity.

For most cases where denormalization is not an option, Includes are probably the answer.

{PANEL/}

## Related Articles

### Client API

- [Include Compare Exchange Values](../../client-api/operations/compare-exchange/include-compare-exchange)

### Indexes

- [Indexing Basics](../../indexes/indexing-basics)
- [Indexing Related Documents](../../indexes/indexing-related-documents)

### Querying

- [Query Overview](../../client-api/session/querying/how-to-query)
- [Querying an Index](../../indexes/querying/query-index)

### Document Extensions

- [Include Time Series](../../document-extensions/timeseries/client-api/session/include/overview)
- [Including Counters](../../document-extensions/counters/counters-and-other-features#including-counters)
