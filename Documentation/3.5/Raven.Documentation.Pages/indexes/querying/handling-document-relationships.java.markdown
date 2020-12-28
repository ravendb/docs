# Handling document relationships

One of the design principles that RavenDB adheres to is the idea that documents are independent, meaning all data required to process a document is stored within the document itself. However, this doesn't mean there should not be relations between objects.

There are valid scenarios where we need to define relationships between objects. By doing so, we expose ourselves to one major problem: whenever we load the containing entity, we are going to need to load data from the referenced entities too (unless we are not interested in them). While the alternative of storing the whole entity in every object graph it is referenced in seems cheaper at first, this proves to be quite costly in terms of database resources and network traffic.

RavenDB offers three elegant approaches to solve this problem. Each scenario will need to use one or more of them. When applied correctly, they can drastically improve performance, reduce network bandwidth and speedup development.

<hr />

## Denormalization

The easiest solution is to denormalize the data within the containing entity, forcing it to contain the actual value of the referenced entity in addition (or instead) of the foreign key.

Take this JSON document for example:

{CODE-BLOCK:json}
// Order document with id: orders/1234
{ 
    "Customer": {
        "Name": "Itamar",
        "Id": "customers/2345"
    },
    Items: [
        { 
        "Product": { 
            "Id": "products/1234",
            "Name": "Milk",
            "Cost": 2.3
            },
        "Quantity": 3
        }
    ]
}
{CODE-BLOCK/}

As you can see, the `Order` document now contains denormalized data from both the `Customer` and the `Product` documents, which are saved elsewhere in full. Note we won't have copied all the customer properties into the order; instead we just clone the ones that we care about when displaying or processing an order. This approach is called *denormalized reference*.

The denormalization approach avoids many cross document lookups and results in only the necessary data being transmitted over the network, but it makes other scenarios more difficult. For example, consider the following entity structure as our start point:

{CODE:java order@Indexes/Querying/HandlingDocumentRelationships.java /}

{CODE:java customer@Indexes/Querying/HandlingDocumentRelationships.java /}

If we know that whenever we load an `Order` from the database we will need to know the customer's name and address, we could decide to create a denormalized `Order.Customer` field, and store those details in the directly in the `Order` object. Obviously, the password and other irrelevant details will not be denormalized:

{CODE:java denormalized_customer@Indexes/Querying/HandlingDocumentRelationships.java /}

There wouldn't be a direct reference between the `Order` and the `Customer`. Instead, `Order` holds a `DenormalizedCustomer`, which contains the interesting bits from `Customer` that we need whenever we process `Order` objects.

But, what happens when the user's address is changed? We will have to perform an aggregate operation to update all orders this customer has made. And what if the customer has a lot of orders or changes their address frequently? Keeping these details in sync could become very demanding on the server. What if another process that works with orders needs a different set of customer properties? The `DenormalizedCustomer` will need to be expanded, possibly to the point that the majority of the customer record is cloned.

{INFO:Tip}
**Denormalization** is a viable solution for rarely changing data, or for data that must remain the same despite the underlying referenced data changing over time.
{INFO/}

<hr />

## Includes

**Includes** feature addresses the limitations of denormalization. Instead of one object containing copies of the properties from another object, it is only necessary to hold a reference to the second object. Then server can be instructed to pre-load the referenced document at the same time that the root object is retrieved. We can do this using:

{CODE-TABS}
{CODE-TAB:java:Session includes_1_0@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_1_1@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

Above we are asking RavenDB to retrieve the `Order` "orders/1234" and at the same time "include" the `Customer` referenced by the `Order.CustomerId` property. The second call to `load()` is resolved completely client side (i.e. without a second request to the RavenDB server) because the relevant `Customer` object has already been retrieved (this is the full `Customer` object not a denormalized version). 

There is also a possibility to load multiple documents:

{CODE-TABS}
{CODE-TAB:java:Session includes_2_0@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_2_1@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

You can also use Includes with queries:

{CODE-TABS}
{CODE-TAB:java:Query includes_3_0@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:DocumentQuery includes_3_1@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_3_2@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Index includes_3_3@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

Under the hood, this works because RavenDB has two channels through which it can return information in response to a load request. The first is the Results channel, through which the root object retrieved by the `load()` method call is returned. The second is the Includes channel, through which any included documents are sent back to the client. Client side, those included documents are not returned from the `load()` method call, but they are added to the session unit of work, and subsequent requests to load them are served directly from the session cache, without requiring any additional queries to the server.

### One to many includes

Include can be used with a many to one relationship. In the above classes, an `Order` has a property `SupplierIds` which contains an array of references to `Supplier` documents. The following code will cause the suppliers to be pre-loaded:

{CODE-TABS}
{CODE-TAB:java:Session includes_4_0@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_4_1@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

Again, the calls to `load()` within the `for` loop will not require a call to the server as the `Supplier` objects will already be loaded into the session cache.

Multi-loads are also possible:

{CODE-TABS}
{CODE-TAB:java:Session includes_5_0@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_5_1@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

### Secondary level includes

An Include does not need to work only on the value of a top level property within a document. It can be used to load a value from a secondary level. In the classes above, the `Order` contains a `Referral` property which is of the type:

{CODE:java referral@Indexes\Querying\HandlingDocumentRelationships.java /}

This class contains an identifier for a `Customer`. The following code will include the document referenced by that secondary level identifier:

{CODE-TABS}
{CODE-TAB:java:Session includes_6_0@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_6_1@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

Alternative way is to provide string based path:

{CODE-TABS}
{CODE-TAB:java:Session includes_6_2@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_6_1@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

This secondary level include will also work with collections. The `Order.LineItems` property holds a collection of `LineItem` objects which each contain a reference to a `Product`:

{CODE:java line_item@Indexes\Querying\HandlingDocumentRelationships.java /}

The `Product` documents can be included using this syntax:

{CODE-TABS}
{CODE-TAB:java:Session includes_7_0@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_7_1@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

when you want to load multiple documents.

The `select()` within the Include tells RavenDB which property of secondary level objects to use as a reference.

{WARNING:select()}
`select()` method is only available for `List<?>` fields. It isn't available for arrays. 
{WARNING/}

{WARNING:Conventions}

When using string-based includes like:

{CODE-TABS}
{CODE-TAB:java:Session includes_6_0@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_6_1@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

you must remember to follow certain rules that must apply to the provided string path:

1.	**Dots** are used to separate properties e.g. `"Referral.CustomerId"` in example above means that our `Order` contains property `Referral` and that property contains another property called `CustomerId`.
2.	**Commas** are used to indicate that property is a collection type e.g. List. So if our `Order` will have a list of LineItems and each `LineItem` will contain `ProductId` property then we can create string path as follows: `"LineItems.,ProductId"`.
3.	**Prefixes** are used to indicate id prefix for **non-string** identifiers. e.g. if our `CustomerId` property in `"Referral.CustomerId"` path is an integer then we should add `customers/` prefix so the final path would look like `"Referral.CustomerId(customers/)"` and for our collection example it would be `"LineItems.,ProductId(products/)"` if the `ProductId` would be a non-string type.

{NOTE **Prefix** is not required for string identifiers, because they contain it by default. /}

Learning string path rules may be useful when you will want to query database using HTTP API.

{CODE-BLOCK:json}
	curl -X GET "http://localhost:8080/databases/Northwind/queries/?include=LineItems.,ProductId(products/)&id=orders/1"
{CODE-BLOCK/}

{WARNING/}

### ValueType identifiers

The above `Include` samples assume that the Id property being used to resolve a reference is a string and it contains the full identifier for the referenced document (e.g. the `CustomerId` property will contain a value such as `"customers/5678"`). Include can also work with Value Type identifiers. Given these entities:

{CODE:java order_2@Indexes\Querying\HandlingDocumentRelationships.java /}

{CODE:java customer_2@Indexes\Querying\HandlingDocumentRelationships.java /}

{CODE:java referral_2@Indexes\Querying\HandlingDocumentRelationships.java /}

The samples above can be re-written as follows:

{CODE-TABS}
{CODE-TAB:java:Session includes_8_0@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_8_1@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:java:Query includes_8_2@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:DocumentQuery includes_8_3@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_8_4@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Index includes_8_5@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:java:Session includes_8_6@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_8_7@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:java:Session includes_8_8@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_8_9@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:java:Session includes_8_10@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_8_11@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

The class parameter to `include` specifies which document collection the reference is pointing to. RavenDB will combine the name of the collection with the value of the reference property to find the full identifier of the referenced document. For example, from the first example, if the value of the `Order.CustomerId` property is `56`, client will include the document with an Id of `customer2s/56` from the database. The `session.load(Customer2.class, Number id)` method will be passed the value `56` and will look for then load the document `customer2s/56` from the session cache.

<hr />


### Dictionary includes

Dictionary keys and values can also be used when doing includes. Consider following scenario:

{CODE:java person_1@Indexes\Querying\HandlingDocumentRelationships.java /}

{CODE:java includes_10_0@Indexes\Querying\HandlingDocumentRelationships.java /}

Now we want to include all documents that are under dictionary values:

{CODE-TABS}
{CODE-TAB:java:Session includes_10_1@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_10_2@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

You can also include values from dictionary keys:

{CODE-TABS}
{CODE-TAB:java:Session includes_10_3@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_10_4@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

#### Complex types

If values in dictionary are more complex e.g.

{CODE:java person_2@Indexes\Querying\HandlingDocumentRelationships.java /}

{CODE:java includes_11_0@Indexes\Querying\HandlingDocumentRelationships.java /}

We can do includes on specific properties also:

{CODE-TABS}
{CODE-TAB:java:Session includes_11_1@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_11_2@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

<hr />

## Combining Approaches

It is possible to combine the above techniques. Using the `DenormalizedCustomer` from above and creating an order that uses it:

{CODE:java order_3@Indexes\Querying\HandlingDocumentRelationships.java /}

We have the advantages of a denormalization, a quick and simple load of an `Order` and the fairly static `Customer` details that are required for most processing. But we also have the ability to easily and efficiently load the full `Customer` object when necessary using:

{CODE-TABS}
{CODE-TAB:java:Session includes_9_0@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TAB:java:Commands includes_9_1@Indexes\Querying\HandlingDocumentRelationships.java /}
{CODE-TABS/}

This combining of denormalization and Includes could also be used with a list of denormalized objects.

It is possible to use Include on a query against a Live Projection. Includes are evaluated after the `TransformResults` has been evaluated. This opens up the possibility of implementing Tertiary Includes (i.e. retrieving documents that are referenced by documents that are referenced by the root document). 

Whilst RavenDB can support Tertiary Includes, before resorting to them you should re-evaluate your document model. Needing to use Tertiary Includes can be an indication that you are designing your documents along "Relational" lines.

<hr />

## Summary

There are no strict rules as to when to use which approach, but the general idea is to give it a lot of thought, and consider the implications each approach has.

As an example, in an e-commerce application it might be better to denormalize product names and prices into an order line object, since you want to make sure the customer sees the same price and product title in the order history. But the customer name and addresses should probably be references, rather than denormalized into the order entity.

For most cases where denormalization is not an option, Includes are probably the answer.

## Related articles

- [Indexing : Basics](../../indexes/indexing-basics)
- [Querying : Basics](../../indexes/querying/basics)
