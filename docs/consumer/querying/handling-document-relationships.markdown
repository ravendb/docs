# Handling document relationships

One of the design principals that RavenDB adheres to is the idea that documents are independent, meaning all data required to process a document is stored within the document itself. However, this doesn't mean there should not be relations between objects.

There are valid scenarios where we need to define relationships between objects. By doing so, we expose ourself to one major problem: whenever we load the containing entity, we are going to need to load data from the referenced entities too, unless we are not interested in them. While the alternative of storing the whole entity in every object graph it is referenced in seems cheaper at first, this proves to be quite costly in terms of database work and network traffic.

RavenDB offers three elegant approaches to solve this problem. Each scenario will need to use one of them or more, and when applied correctly, they can drastically improve performance, reduce network bandwidth and speedup development.

The theory behind this topic and other related subjects are discussed in length in the Theory section.

## Denormalization

The easiest solution is to denormalize the data in the containing entity, forcing it to contain the actual value of the referenced entity in addition (or instead) of the foreign key.

Take this JSON document for example:

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

As you can see, the `Order` document now contains denormalized data from both the `Customer` and the `Product` documents, which are saved elsewhere in full. Note how We haven't copied all the properties, and just saved the ones that we care about for this `Order`. This approach is called _denormalized reference_. The properties that we copy are the ones that we will use to display or process the root entity.

## Includes

Denormalizing data like shown above indeed avoids many lookups and results in transmitting only the necessary data over the network, but in many scenarios it will not prove very useful. For example, consider the following entity structure:

{CODE order_classes@Common.cs /}

We know whenever we load an order from the database we need to know the user name and his address. So we decided to denormalize the `Order.Customer` field, and store those details in the order object. Obviously, the password and other irrelevant details will not be denormalized:

	public class DenormalizedCustomer
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
	}

So there isn’t a direct reference between the `Order` and the `Customer`. Instead, `Order` holds a `DenormalizedCustomer`, which holds the interesting bits from `Customer` that we need to process requests on `Order`.

Now, what happens when the user's address is changed? we will have to perform an aggregate operation to update all orders this customer has made. And what if this is a constantly returning customer? This operation can become very demanding.

Using the RavenDB Includes feature you can do this much more efficiently, by instructing RavenDB to load the associated document on the first request. We can do so using:

{CODE includes1@Consumer/Includes.cs /}

You can even use Includes with queries:

{CODE includes2@Consumer/Includes.cs /}

What actually happens under the hood is that RavenDB actually has two channels in which it can return information for a load request. The first is the results channel, which is what is returned from the Load method call. The second is the Includes channel, which contains all the included documents. Those documents are not returned from the `Load` method call, but they are added to the session unit of work, and subsequent requests to load them can be served directly from the session cache, without any additional queries to the server.

## Live Projections

Using Includes is very useful, but sometimes we want to do better than that, or just more. Using the Live Projections feature, you get more control over what to load into the result entity, and since it returns a projection of the actual entity you also get the chance to filter out properties you do not need.

## An important difference is that while Includes is used for explicit loading by id, Live Projections are used for querying and cannot be used with explicit loading.

Live Projections allow to query a Map/Reduce index built for one type, and return an entity of a completely different type, having the actual transformation occur on the server.

// Querying; The As<> method

## Summary

There are no strict rules when to use which approach, but the general idea is to give it a good thought, and consider the various implication each has.

For example, in an e-commerce application product names and prices are actually better be denormalized into an order line object, since you want to make sure the customer sees the same price and product title in the order history. But the customer name and addresses should never be denormalized into the order entity.