# How to handle document relationships

One of the design principles that RavenDB adheres to is the idea that documents are independent, meaning all data required to process a document is stored within the document itself. However, this doesn't mean there should not be relations between objects.

There are valid scenarios where we need to define relationships between objects. By doing so, we expose ourselves to one major problem: whenever we load the containing entity, we are going to need to load data from the referenced entities too (unless we are not interested in them). While the alternative of storing the whole entity in every object graph it is referenced in seems cheaper at first, this proves to be quite costly in terms of database resources and network traffic.

RavenDB offers three elegant approaches to solve this problem. Each scenario will need to use one or more of them. When applied correctly, they can drastically improve performance, reduce network bandwidth and speedup development.

<hr />

## Denormalization

The easiest solution is to denormalize the data within the containing entity, forcing it to contain the actual value of the referenced entity in addition (or instead) of the foreign key.

Take this JSON document for example:

{CODE-BLOCK:json}
// Order document with id: orders/1-A
{ 
    "Customer": 
    {
        "Name": "Itamar",
        "Id": "customers/1-A"
    },
    Items: 
    [
        { 
            "Product": 
            { 
                "Id": "products/1-A",
                "Name": "Milk",
                "Cost": 2.3
             },
            "Quantity": 3
        }
    ]
}
{CODE-BLOCK/}

As you can see, the `Order` document now contains denormalized data from both the `Customer` and the `Product` documents, which are saved elsewhere in full. Note we won’t have copied all the customer properties into the order; instead we just clone the ones that we care about when displaying or processing an order. This approach is called *denormalized reference*.

The denormalization approach avoids many cross document lookups and results in only the necessary data being transmitted over the network, but it makes other scenarios more difficult. For example, consider the following entity structure as our start point:

{CODE order@ClientApi/HowTo/HandleDocumentRelationships.cs /}

{CODE customer@ClientApi/HowTo/HandleDocumentRelationships.cs /}

If we know that whenever we load an `Order` from the database we will need to know the customer's name and address, we could decide to create a denormalized `Order.Customer` field, and store those details in the directly in the `Order` object. Obviously, the password and other irrelevant details will not be denormalized:

{CODE denormalized_customer@ClientApi/HowTo/HandleDocumentRelationships.cs /}

There wouldn’t be a direct reference between the `Order` and the `Customer`. Instead, `Order` holds a `DenormalizedCustomer`, which contains the interesting bits from `Customer` that we need whenever we process `Order` objects.

But, what happens when the user's address is changed? We will have to perform an aggregate operation to update all orders this customer has made. And what if the customer has a lot of orders or changes their address frequently? Keeping these details in sync could become very demanding on the server. What if another process that works with orders needs a different set of customer properties? The `DenormalizedCustomer` will need to be expanded, possibly to the point that the majority of the customer record is cloned.

{INFO:Tip}
**Denormalization** is a viable solution for rarely changing data, or for data that must remain the same despite the underlying referenced data changing over time.
{INFO/}

<hr />

## Includes

**Includes** feature addresses the limitations of denormalization. Instead of one object containing copies of the properties from another object, it is only necessary to hold a reference to the second object. Then server can be instructed to pre-load the referenced document at the same time that the root object is retrieved. We can do this using:

{CODE-TABS}
{CODE-TAB:csharp:Session includes_1_0@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TAB:csharp:Command includes_1_1@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TABS/}

Above we are asking RavenDB to retrieve the `Order` `orders/1-A` and at the same time "include" the `Customer` referenced by the `Order.CustomerId` property. The second call to `Load()` is resolved completely client side (i.e. without a second request to the RavenDB server) because the relevant `Customer` object has already been retrieved (this is the full `Customer` object not a denormalized version). 

There is also a possibility to load multiple documents:

{CODE-TABS}
{CODE-TAB:csharp:Session includes_2_0@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TAB:csharp:Command includes_2_1@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TABS/}

You can also use Includes with queries:

{CODE-TABS}
{CODE-TAB:csharp:Query includes_3_0@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TAB:csharp:DocumentQuery includes_3_1@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TAB:csharp:Index includes_3_3@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TABS/}

Under the hood, this works because RavenDB has two channels through which it can return information in response to a load request. The first is the Results channel, through which the root object retrieved by the `Load()` method call is returned. The second is the Includes channel, through which any included documents are sent back to the client. Client side, those included documents are not returned from the `Load()` method call, but they are added to the session unit of work, and subsequent requests to load them are served directly from the session cache, without requiring any additional queries to the server.

### One to many includes

Include can be used with a many to one relationship. In the above classes, an `Order` has a property `SupplierIds` which contains an array of references to `Supplier` documents. The following code will cause the suppliers to be pre-loaded:

{CODE-TABS}
{CODE-TAB:csharp:Session includes_4_0@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TAB:csharp:Command includes_4_1@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TABS/}

Again, the calls to `Load()` within the `foreach` loop will not require a call to the server as the `Supplier` objects will already be loaded into the session cache.

Multi-loads are also possible:

{CODE-TABS}
{CODE-TAB:csharp:Session includes_5_0@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TAB:csharp:Command includes_5_1@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TABS/}

### Secondary level includes

An Include does not need to work only on the value of a top level property within a document. It can be used to load a value from a secondary level. In the classes above, the `Order` contains a `Referral` property which is of the type:

{CODE referral@ClientApi/HowTo/HandleDocumentRelationships.cs /}

This class contains an identifier for a `Customer`. The following code will include the document referenced by that secondary level identifier:

{CODE-TABS}
{CODE-TAB:csharp:Session includes_6_0@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TAB:csharp:Command includes_6_1@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TABS/}

Alternative way is to provide string based path:

{CODE-TABS}
{CODE-TAB:csharp:Session includes_6_2@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TAB:csharp:Command includes_6_1@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TABS/}

This secondary level include will also work with collections. The `Order.LineItems` property holds a collection of `LineItem` objects which each contain a reference to a `Product`:

{CODE line_item@ClientApi/HowTo/HandleDocumentRelationships.cs /}

The `Product` documents can be included using this syntax:

{CODE-TABS}
{CODE-TAB:csharp:Session includes_7_0@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TAB:csharp:Command includes_7_1@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TABS/}

when you want to load multiple documents.

The `Select()` within the Include tells RavenDB which property of secondary level objects to use as a reference.

{WARNING:Conventions}

When using string-based includes like:

{CODE-TABS}
{CODE-TAB:csharp:Session includes_6_0@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TAB:csharp:Command includes_6_1@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TABS/}

you must remember to follow certain rules that must apply to the provided string path:

1.	**Dots** are used to separate properties e.g. `"Referral.CustomerId"` in example above means that our `Order` contains property `Referral` and that property contains another property called `CustomerId`.
2.	**Indexer operator** is used to indicate that property is a collection type. So if our `Order` have a list of LineItems and each `LineItem` contains `ProductId` property then we can create string path as follows: `"LineItems[].ProductId"`.
3.	**Prefixes** can be used to indicate prefix of the identifier of the document that is going to be included. It can be useful when working with custom or semantic identifiers. For example, if you have a customer stored under `customers/login@domain.com` then you can include it using `"Referral.CustomerEmail(customers/)"` (`customers/` is the prefix here).

Learning string path rules may be useful when you will want to query database using HTTP API.

{CODE-BLOCK:json}
	curl -X GET "http://localhost:8080/databases/Northwind/docs?id=orders/1-A&include=Lines[].Product"
{CODE-BLOCK/}

{WARNING/}

### Dictionary includes

Dictionary keys and values can also be used when doing includes. Consider following scenario:

{CODE person_1@ClientApi/HowTo/HandleDocumentRelationships.cs /}

{CODE includes_10_0@ClientApi/HowTo/HandleDocumentRelationships.cs /}

Now we want to include all documents that are under dictionary values:

{CODE-TABS}
{CODE-TAB:csharp:Session includes_10_1@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TAB:csharp:Command includes_10_2@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TABS/}

You can also include values from dictionary keys:

{CODE-TABS}
{CODE-TAB:csharp:Session includes_10_3@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TAB:csharp:Command includes_10_4@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TABS/}

#### Complex types

If values in dictionary are more complex e.g.

{CODE person_2@ClientApi/HowTo/HandleDocumentRelationships.cs /}

{CODE includes_11_0@ClientApi/HowTo/HandleDocumentRelationships.cs /}

We can do includes on specific properties also:

{CODE-TABS}
{CODE-TAB:csharp:Session includes_11_1@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TAB:csharp:Command includes_11_2@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TABS/}

<hr />

## Combining Approaches

It is possible to combine the above techniques. Using the `DenormalizedCustomer` from above and creating an order that uses it:

{CODE order_3@ClientApi/HowTo/HandleDocumentRelationships.cs /}

We have the advantages of a denormalization, a quick and simple load of an `Order` and the fairly static `Customer` details that are required for most processing. But we also have the ability to easily and efficiently load the full `Customer` object when necessary using:

{CODE-TABS}
{CODE-TAB:csharp:Session includes_9_0@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TAB:csharp:Command includes_9_1@ClientApi/HowTo/HandleDocumentRelationships.cs /}
{CODE-TABS/}

This combining of denormalization and Includes could also be used with a list of denormalized objects.

It is possible to use Include on a query being a projection. Includes are evaluated after the projection has been evaluated. This opens up the possibility of implementing Tertiary Includes (i.e. retrieving documents that are referenced by documents that are referenced by the root document). 

Whilst RavenDB can support Tertiary Includes, before resorting to them you should re-evaluate your document model. Needing to use Tertiary Includes can be an indication that you are designing your documents along "Relational" lines.

<hr />

## Summary

There are no strict rules as to when to use which approach, but the general idea is to give it a lot of thought, and consider the implications each approach has.

As an example, in an e-commerce application it might be better to denormalize product names and prices into an order line object, since you want to make sure the customer sees the same price and product title in the order history. But the customer name and addresses should probably be references, rather than denormalized into the order entity.

For most cases where denormalization is not an option, Includes are probably the answer.

## Related articles

- [Indexing : Basics](../../indexes/indexing-basics)
- [Querying : Basics](../../indexes/querying/basics)
