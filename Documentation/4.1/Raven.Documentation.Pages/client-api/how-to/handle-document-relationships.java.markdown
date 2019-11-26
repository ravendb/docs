# Client API: How to Handle Document Relationships

One of the design principles that RavenDB adheres to is the idea that documents are independent, meaning all data required to process a document is stored within the document itself. However, this doesn't mean there should not be relations between objects.

There are valid scenarios where we need to define relationships between objects. By doing so, we expose ourselves to one major problem: whenever we load the containing entity, we are going to need to load data from the referenced entities as well (unless we are not interested in them). While the alternative of storing the whole entity in every object graph it is referenced in seems cheaper at first, this proves to be quite costly in terms of database resources and network traffic.

RavenDB offers three elegant approaches to solve this problem. Each scenario will need to use one or more of them. When applied correctly, they can drastically improve performance, reduce network bandwidth, and speed up development.

## Denormalization

The easiest solution is to denormalize the data within the containing entity, forcing it to contain the actual value of the referenced entity in addition to (or instead of) the foreign key.

Take this JSON document for example:

{CODE-BLOCK:json}
// Order document with ID: orders/1-A
{
    "Customer": {
        "Name": "Itamar",
        "Id": "customers/1-A"
    },
    Items: [
        {
            "Product": {
                "Id": "products/1-A",
                "Name": "Milk",
                "Cost": 2.3
            },
            "Quantity": 3
        }
    ]
}
{CODE-BLOCK/}

As you can see, the `Order` document now contains denormalized data from both the `Customer` and the `Product` documents which are saved elsewhere in full. Note we won’t have copied all the customer fields into the order; instead we just clone the ones that we care about when displaying or processing an order. This approach is called *denormalized reference*.

The denormalization approach avoids many cross document lookups and results in only the necessary data being transmitted over the network, but it makes other scenarios more difficult. For example, consider the following entity structure as our start point:

{CODE:java order@ClientApi/HowTo/HandleDocumentRelationships.java /}

{CODE:java customer@ClientApi/HowTo/HandleDocumentRelationships.java /}

If we know that whenever we load an `Order` from the database we will need to know the customer's name and address, we could decide to create a denormalized `Order.Customer` field and store those details directly in the `Order` object. Obviously, the password and other irrelevant details will not be denormalized:

{CODE:java denormalized_customer@ClientApi/HowTo/HandleDocumentRelationships.java /}

There wouldn’t be a direct reference between the `Order` and the `Customer`. Instead, `Order` holds a `DenormalizedCustomer`, which contains the interesting bits from `Customer` that we need whenever we process `Order` objects.

But what happens when the user's address is changed? We will have to perform an aggregate operation to update all orders this customer has made. What if the customer has a lot of orders or changes their address frequently? Keeping these details in sync could become very demanding on the server. What if another process that works with orders needs a different set of customer fields? The `DenormalizedCustomer` will need to be expanded, possibly to the point that the majority of the customer record is cloned.

{INFO:Tip}
**Denormalization** is a viable solution for rarely changing data or for data that must remain the same despite the underlying referenced data changing over time.
{INFO/}

## Includes

The **Includes** feature addresses the limitations of denormalization. Instead of one object containing copies of the fields from another object, it is only necessary to hold a reference to the second object. Then the server can be instructed to pre-load the referenced document at the same time that the root object is retrieved. We do this using:

{CODE:java includes_1_0@ClientApi/HowTo/HandleDocumentRelationships.java /}

Above we are asking RavenDB to retrieve the `Order` `orders/1-A`, and at the same time "include" the `Customer` referenced by the `Order.CustomerId` field. The second call to `load()` is resolved completely client side (i.e. without a second request to the RavenDB server) because the relevant `Customer` object has already been retrieved (this is the full `Customer` object not a denormalized version). 


There is also a possibility to load multiple documents:

{CODE:java includes_2_0@ClientApi/HowTo/HandleDocumentRelationships.java /}

You can also use Includes with queries:

{CODE-TABS}
{CODE-TAB:java:DocumentQuery includes_3_0@ClientApi/HowTo/HandleDocumentRelationships.java /}
{CODE-TAB:java:DocumentQuery(Builder) includes_3_0_builder@ClientApi/HowTo/HandleDocumentRelationships.java /}
{CODE-TAB-BLOCK:sql:RQL(Embedded)}
from Orders
where TotalPrice > 100
include CustomerId
{CODE-TAB-BLOCK/}
{CODE-TAB-BLOCK:sql:RQL(Builder)}
from Orders as o
where TotalPrice > 100
include CustomerId,counters(o,'OrderUpdateCount')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

This works because RavenDB has two channels through which it can return information in response to a load request. The first is the Results channel, through which the root object retrieved by the `load()` method call is returned. The second is the Includes channel, through which any included documents are sent back to the client. Client side, those included documents are not returned from the `load()` method call, but they are added to the session unit of work, and subsequent requests to load them are served directly from the session cache, without requiring any additional queries to the server.

{NOTE Embedded and builder variants of Include clause are essentially syntax sugar and are equivalent at the server side. /}

### One to Many Includes

Include can be used with a many to one relationship. In the above classes, an `Order` has a field `SupplierIds` which contains an array of references to `Supplier` documents. The following code will cause the suppliers to be pre-loaded:

{CODE:java includes_4_0@ClientApi/HowTo/HandleDocumentRelationships.java /}

Alternatively, it is possible to use the fluent builder syntax.

{CODE:java includes_4_0_builder@ClientApi/HowTo/HandleDocumentRelationships.java /}

The calls to `load()` within the `foreach` loop will not require a call to the server as the `Supplier` objects will already be loaded into the session cache.

Multi-loads are also possible:

{CODE:java includes_5_0@ClientApi/HowTo/HandleDocumentRelationships.java /}

### Secondary Level Includes

An Include does not need to work only on the value of a top level field within a document. It can be used to load a value from a secondary level. In the classes above, the `Order` contains a `Referral` field which is of the type:

{CODE:java referral@ClientApi/HowTo/HandleDocumentRelationships.java /}

This class contains an identifier for a `Customer`. The following code will include the document referenced by that secondary level identifier:

{CODE:java includes_6_0@ClientApi/HowTo/HandleDocumentRelationships.java /}

It is possible to execute the same code with the fluent builder syntax:

{CODE:java includes_6_0_builder@ClientApi/HowTo/HandleDocumentRelationships.java /}

This secondary level include will also work with collections. The `Order.LineItems` field holds a collection of `LineItem` objects which each contain a reference to a `Product`:

{CODE:java line_item@ClientApi/HowTo/HandleDocumentRelationships.java /}

The `Product` documents can be included using the following syntax:

{CODE:java includes_7_0@ClientApi/HowTo/HandleDocumentRelationships.java /}

The fluent builder syntax works here too.

{CODE:java includes_7_0_builder@ClientApi/HowTo/HandleDocumentRelationships.java /}

when you want to load multiple documents.

The `[]` within the `include` tells RavenDB which field of secondary level objects to use as a reference.

{WARNING: }
### `String`&nbsp;Path Conventions

When using string-based includes like:

{CODE:java includes_6_0@ClientApi/HowTo/HandleDocumentRelationships.java /}

you must remember to follow certain rules that must apply to the provided string path:

1.	**Dots** are used to separate fields e.g. `"Referral.CustomerId"` in the example above means that our `Order` contains field `Referral` and that field contains another field called `CustomerId`.
2.	**Indexer operator** is used to indicate that field is a collection type. So if our `Order` has a list of LineItems and each `LineItem` contains a `ProductId` field, then we can create string path as follows: `"LineItems[].ProductId"`.
3.	**Prefixes** can be used to indicate the prefix of the identifier of the document that is going to be included. It can be useful when working with custom or semantic identifiers. For example, if you have a customer stored under `customers/login@domain.com` then you can include it using `"Referral.CustomerEmail(customers/)"` (`customers/` is the prefix here).

Learning string path rules may be useful when you will want to query database using HTTP API.

{CODE-BLOCK:json}
	curl -X GET "http://localhost:8080/databases/Northwind/docs?id=orders/1-A&include=lines[].product"
{CODE-BLOCK/}

{WARNING/}

### Dictionary Includes

Dictionary keys and values can also be used when doing includes. Consider following scenario:

{CODE:java person_1@ClientApi/HowTo/HandleDocumentRelationships.java /}

{CODE:java includes_10_0@ClientApi/HowTo/HandleDocumentRelationships.java /}

Now we want to include all documents that are under dictionary values:

{CODE:java includes_10_1@ClientApi/HowTo/HandleDocumentRelationships.java /}

The code above can be also rewritten with fluent builder syntax:

{CODE:java includes_10_1_builder@ClientApi/HowTo/HandleDocumentRelationships.java /}

You can also include values from dictionary keys:

{CODE:java includes_10_3@ClientApi/HowTo/HandleDocumentRelationships.java /}

Here, as well, this can be written with fluent builder syntax:

{CODE:java includes_10_3_builder@ClientApi/HowTo/HandleDocumentRelationships.java /}

#### Complex Types

If values in dictionary are more complex e.g.

{CODE:java person_2@ClientApi/HowTo/HandleDocumentRelationships.java /}

{CODE:java includes_11_0@ClientApi/HowTo/HandleDocumentRelationships.java /}

We can also do includes on specific fields:

{CODE:java includes_11_1@ClientApi/HowTo/HandleDocumentRelationships.java /}

## Combining Approaches

It is possible to combine the above techniques. Using the `DenormalizedCustomer` from above and creating an order that uses it:

{CODE:java order_3@ClientApi/HowTo/HandleDocumentRelationships.java /}

We have the advantages of a denormalization, a quick and simple load of an `Order`, and the fairly static `Customer` details that are required for most processing. But we also have the ability to easily and efficiently load the full `Customer` object when necessary using:

{CODE:java includes_9_0@ClientApi/HowTo/HandleDocumentRelationships.java /}

This combining of denormalization and Includes could also be used with a list of denormalized objects.

It is possible to use Include on a query being a projection. Includes are evaluated after the projection has been evaluated. This opens up the possibility of implementing Tertiary Includes (i.e. retrieving documents that are referenced by documents that are referenced by the root document). 

RavenDB can support Tertiary Includes, but before resorting to them you should re-evaluate your document model. Needing Tertiary Includes can be an indication that you are designing your documents along "Relational" lines.

## Summary

There are no strict rules as to when to use which approach, but the general idea is to give it a lot of thought and consider the implications each approach has.

As an example, in an e-commerce application it might be better to denormalize product names and prices into an order line object since you want to make sure the customer sees the same price and product title in the order history. But the customer name and addresses should probably be references rather than denormalized into the order entity.

For most cases where denormalization is not an option, Includes are probably the answer.

## Related Articles

### Indexes

- [Indexing Basics](../../indexes/indexing-basics)
- [Indexing Related Documents](../../indexes/indexing-related-documents)

### Querying

- [Basics](../../indexes/querying/basics)
