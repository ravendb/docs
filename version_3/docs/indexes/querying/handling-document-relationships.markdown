# Handling document relationships

One of the design principles that RavenDB adheres to is the idea that documents are independent, meaning all data required to process a document is stored within the document itself. However, this doesn't mean there should not be relations between objects.

There are valid scenarios where we need to define relationships between objects. By doing so, we expose ourselves to one major problem: whenever we load the containing entity, we are going to need to load data from the referenced entities too (unless we are not interested in them). While the alternative of storing the whole entity in every object graph it is referenced in seems cheaper at first, this proves to be quite costly in terms of database resources and network traffic.

RavenDB offers three elegant approaches to solve this problem. Each scenario will need to use one or more of them. When applied correctly, they can drastically improve performance, reduce network bandwidth and speedup development.

The concepts behind this topic and other related subjects are discussed in length in the Theory section.

## Denormalization

The easiest solution is to denormalize the data within the containing entity, forcing it to contain the actual value of the referenced entity in addition (or instead) of the foreign key.

Take this JSON document for example:

{CODE-START:json /}
  { // Order document with id: orders/1234
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
{CODE-END/}

As you can see, the `Order` document now contains denormalized data from both the `Customer` and the `Product` documents, which are saved elsewhere in full. Note we won’t have copied all the customer properties into the order; instead we just clone the ones that we care about when displaying or processing an order. This approach is called *denormalized reference*.

The denormalization approach avoids many cross document lookups and results in only the necessary data being transmitted over the network, but it makes other scenarios more difficult. For example, consider the following entity structure as our start point:

{XCODE order_classes_basic@Entities.cs /}

If we know that whenever we load an `Order` from the database we will need to know the customer's name and address, we could decide to create a denormalized `Order.Customer` field, and store those details in the directly in the `Order` object. Obviously, the password and other irrelevant details will not be denormalized:

{XCODE order_classes_denormalizedcustomer@Entities.cs /}

There wouldn’t be a direct reference between the `Order` and the `Customer`. Instead, `Order` holds a `DenormalizedCustomer`, which contains the interesting bits from `Customer` that we need whenever we process `Order` objects.

But, what happens when the user's address is changed? We will have to perform an aggregate operation to update all orders this customer has made. And what if the customer has a lot of orders or changes their address frequently? Keeping these details in sync could become very demanding on the server. What if another process that works with orders needs a different set of customer properties? The `DenormalizedCustomer` will need to be expanded, possibly to the point that the majority of the customer record is cloned.

Denormalization is a viable solution for rarely changing data, or for data that must remain the same despite the underlying referenced data changing over time.

## Includes

The RavenDB "Includes" feature addresses the limitations of denormalization. Instead of one object containing copies of the properties from another object, it is only necessary to hold a reference to the second object. Then RavenDB can be instructed to pre-load the referenced document at the same time that the root object is retrieved. We can do this using:

{CODE includes1@Indexes\Querying\HandlingDocumentRelationships.cs /}

Above we are asking RavenDB to retrieve the `Order` "orders/1234" and at the same time "include" the `Customer` referenced by the `Order.CustomerId` property. The second call to `Load()` is resolved completely client side (i.e. without a second request to the RavenDB server) because the relevant `Customer` object has already been retrieved (this is the full `Customer` object not a denormalized version). 

There is also a possibility to load multiple documents:

{CODE includes_1@Indexes\Querying\HandlingDocumentRelationships.cs /}

You can also use Includes with queries:

{CODE includes2@Indexes\Querying\HandlingDocumentRelationships.cs /}

Under the hood, this works because RavenDB has two channels through which it can return information in response to a load request. The first is the Results channel, through which the root object retrieved by the `Load()` method call is returned. The second is the Includes channel, through which any included documents are sent back to the client. Client side, those included documents are not returned from the `Load()` method call, but they are added to the session unit of work, and subsequent requests to load them are served directly from the session cache, without requiring any additional queries to the server.

### One to many includes

Include can be used with a many to one relationship. In the above classes, an `Order` has a property `SupplierIds` which contains an array of references to `Supplier` documents. The following code will cause the suppliers to be pre-loaded:

{CODE includes3@Indexes\Querying\HandlingDocumentRelationships.cs /}

Again, the calls to `Load()` within the `foreach` loop will not require a call to the server as the `Supplier` objects will already be loaded into the session cache.

Multi-loads are also possible:

{CODE includes_3@Indexes\Querying\HandlingDocumentRelationships.cs /}

### Secondary level includes

An Include does not need to work only on the value of a top level property within a document. It can be used to load a value from a secondary level. In the classes above, the `Order` contains a `Referral` property which is of the type:

{XCODE order_classes_referral@Entities.cs /}

This class contains an identifier for a `Customer`. The following code will include the document referenced by that secondary level identifier:

{CODE includes4@Indexes\Querying\HandlingDocumentRelationships.cs /}

Alternative way is to provide string based path:

{CODE includes_4@Indexes\Querying\HandlingDocumentRelationships.cs /}

This secondary level include will also work with collections. The `Order.LineItems` property holds a collection of `LineItem` objects which each contain a reference to a `Product`:

{XCODE order_classes_lineitem@Entities.cs /}

The `Product` documents can be included using this syntax:

{CODE includes5@Indexes\Querying\HandlingDocumentRelationships.cs /}

when you want to load multiple documents.

The `Select()` within the Include tells RavenDB which property of secondary level objects to use as a reference.

### ValueType identifiers

The above `Include` samples assume that the Id property being used to resolve a reference is a string and it contains the full identifier for the referenced document (e.g. the `CustomerId` property will contain a value such as `"customers/5678"`). Include can also work with Value Type identifiers. Given these entities:

{XCODE order_classes2@Entities.cs /}

The samples above can be re-written as follows:

{CODE includes1_2@Indexes\Querying\HandlingDocumentRelationships.cs /}

{CODE includes2_2@Indexes\Querying\HandlingDocumentRelationships.cs /}

{CODE includes3_2@Indexes\Querying\HandlingDocumentRelationships.cs /}

{CODE includes4_2@Indexes\Querying\HandlingDocumentRelationships.cs /}

{CODE includes5_2@Indexes\Querying\HandlingDocumentRelationships.cs /}

The second parameter to the generic `Include<T, TInclude>` specifies which document collection the reference is pointing to. RavenDB will combine the name of the collection with the value of the reference property to find the full identifier of the referenced document. For example, from the first example, if the value of the `Order.Customer2Id` property is the integer 56, RavenDB will include the document with an Id of "customer2s/56" from the database. The `Session.Load<Customer2>()` method will be passed the value 56 and will look for then load the document "customer2s/56" from the session cache.

### Lucene Queries

Same query extensions have been applied to LuceneQuery, so to include `Customer` by `CustomerId` property can be achieved in both ways:

{CODE includes_7@Indexes\Querying\HandlingDocumentRelationships.cs /}

or

{CODE includes_8@Indexes\Querying\HandlingDocumentRelationships.cs /}

### Include rules

When using string-based includes like:

{CODE includes_4@Indexes\Querying\HandlingDocumentRelationships.cs /}

you must remember to follow certain rules that must apply to the provided string path:

1.	**Dots** are used to separate properties e.g. `"Referral.CustomerId"` in example above means that our `Order` contains property `Referral` and that property contains another property called `CustomerId`.
2.	**Commas** are used to indicate that property is a collection type e.g. List. So if our `Order` will have a list of LineItems and each `LineItem` will contain `ProductId` property then we can create string path as follows: `"LineItems.,ProductId"`.
3.	**Prefixes** are used to indicate id prefix for **non-string** identifiers. e.g. if our `CustomerId` property in `"Referral.CustomerId"` path is an integer then we should add `customers/` prefix so the final path would look like `"Referral.CustomerId(customers/)"` and for our collection example it would be `"LineItems.,ProductId(products/)"` if the `ProductId` would be a non-string type.

{NOTE **Prefix** is not required for string identifiers, because they contain it by default. /}

Learning string path rules may be useful when you will want to query database using HTTP API.

{CODE-START:json /}
   > curl -X GET "http://localhost:8080/queries/?include=LineItems.,ProductId(products/)&id=orders/1"

{CODE-END /}

## Summary

There are no strict rules as to when to use which approach, but the general idea is to give it a lot of thought, and consider the implications each approach has.

As an example, in an e-commerce application it might be better to denormalize product names and prices into an order line object, since you want to make sure the customer sees the same price and product title in the order history. But the customer name and addresses should probably be references, rather than denormalized into the order entity.

For most cases where denormalization is not an option, Includes are probably the answer.

##Related articles

TODO
