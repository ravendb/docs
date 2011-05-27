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

We know whenever we load an order from the database we need to know the user name and address. So we decided to denormalize the `Order.Customer` field, and store those details in the order object. Obviously, the password and other irrelevant details will not be denormalized:

	public class DenormalizedCustomer
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
	}

As you can see there isn’t a direct reference between the `Order` and the `Customer`. Instead, `Order` holds a `DenormalizedCustomer`, which holds the interesting bits from `Customer` that we need to process requests on `Order`.

Now, what happens when the user's address is changed? we will have to perform an aggregate operation to update all orders this customer has made. And what if this is a constantly returning customer? This operation can become very demanding.

Using the RavenDB Includes feature you can do this much more efficiently, by instructing RavenDB to load the associated document on the first request. We can do so using:

{CODE includes1@Consumer/Includes.cs /}

You can even use Includes with queries:

{CODE includes2@Consumer/Includes.cs /}

What actually happens under the hood is that RavenDB actually has two channels in which it can return information for a load request. The first is the results channel, which is what is returned from the Load method call. The second is the Includes channel, which contains all the included documents. Those documents are not returned from the `Load` method call, but they are added to the session unit of work, and subsequent requests to load them can be served directly from the session cache, without any additional queries to the server.

## Live Projections

Using Includes is very useful, but sometimes we want to do better than that, or just more. The Live Projection feature is unique to RavenDB, and it can be thought of as the third step of the Map/Reduce operation: after done with mapping all data, and it has been reduced (if asked to), the RavenDB server can transform the results into a completely different data structure and return it back instead of the original results.

Using the Live Projections feature, you get more control over what to load into the result entity, and since it returns a projection of the actual entity you also get the chance to filter out properties you do not need.

Lets use an example to show how it can be used. Assuming we have many `User` entities, and many of them are actually an alias for another user. If we wanted to display all users with their aliases, we would probably need to do something like this:

{CODE liveprojections1@Consumer\LiveProjections.cs /}

Since we use Includes, the server will only be accessed once - indeed, but the entire object graph will be sent by the server for each referenced document (user entity for the alias). And its an awful lot of code to write, too.

Using Live Projections, we can do this much more easily and on the server side:

{CODE liveprojections2@Consumer\LiveProjections.cs /}

The function declared in `TransformResults` will be executed on the results on the query, and that gives it the chance to modify, extend or filter them. In this case, it lets us look at data from another document and use it to project a new return type.

Since each Live Projection will return a projection, you can use the `.As<>` clause to convert it back to a type known by your application:

{CODE liveprojections3@Consumer\LiveProjections.cs /}

The main benefit of using Live Projections is by having to write much less code, which will run on the server and can save a lot of network bandwidth by returning only the data we are interested in.

{NOTE An important difference to note is that while Includes is useful for explicit loading by id or querying, Live Projections can be used for querying only. /}

## Summary

There are no strict rules as to when to use which approach, but the general idea is to give it a good thought, and consider the various implication each has.

As an example, in an e-commerce application product names and prices are actually better be denormalized into an order line object, since you want to make sure the customer sees the same price and product title in the order history. But the customer name and addresses should never be denormalized into the order entity.

For most cases where denormalization is not an option, Includes are probably the answer. Whenever a serious processing is required after the Map/Reduce work is done, or when you need a different entity structure returned than those defined by your index - take a look at Live Projections.