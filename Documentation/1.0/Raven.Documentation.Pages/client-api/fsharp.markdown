# The F# Client API

The F# client API is a thin wrapper around the standard RavenDB client API, that provides a small set of combinators and a computation builder that hides the complexity of dealing with Linq expressions from F#.

This documentation assumes some familiarity with the basics of RavenDB. 

## Creating a Document Store

the F# language, contains a type called Discriminated Unions / Algebraic Data Types. By default the JSON.net serializer that RavenDB uses cannot (de)serialize these types. 

The Raven DB F# client library comes with a JSON.net converter that allows these types to be handled. But we need to customize the document store. This can be achieved by adding the following code. 

	store.Conventions.CustomizeJsonSerializer <- (fun s -> s.Converters.Add(new UnionTypeConverter()))

alternatively the library provides an extension method on *DocumentStore* that creates a document store based on a named Connection String. 

	DocumentStore.OpenInitializedStore("RavenDb")
	
## Data Model 

Throughout the examples we will use the following Data Model

	type Product = {
		mutable Id : string
		Name : string
		Price : float
	}

	type Order = {
		mutable Id : string
		Date : DateTimeOffset
		Customer : string
		Items : Product array
	}

	type CustomerAttachementMetaData = {
		Description : string	
	}

	type Customer = {
		mutable Id : string
		Name : string
		Dob : DateTime
	}


## Inserting Data

To insert data we can simply run the following expression, 

	let customer = 
		{ Id = null; Name = "Test"; Dob = DateTimeOffset.Now.Date}

	use session = docStore.OpenSession()
	store customer |> run session

alternatively using the computation expression syntax

	raven {
		return! store customer
	} |> run session

## Queries

To query for all of the customers in our database born before 7/1/2012 we can write something like the following... 

	let customerQuery = 
		raven { 
		   return! query (where <@ fun x -> x.Dob < new DateTime(2012,1,7) @>)
		}


This defines the query, but at this point the query has not been executed, To execute the query we can then execute 

	let result = 
		use session = docStore.OpenSession()
		customerQuery |> run session

### Includes (joins)

One of the neat features of raven is that it support joins on the server side, so I can get a document and other related documents back in a single call across the wire, 

From the data model above we can see that a order has a reference to a customer, to retrieve this in a single call we can create the following query

	let orderIncludingCustomer = 
		raven { 
			return! (fun s -> 
                              let order = including <@ fun s -> s.Customer @> (fun s -> s.Load("orders/1")) s
                              let customer : Customer = s.Load(order.Customer)
                              order, customer
                        )
		}

and then run it

	use session = docStore.OpenSession()
	orderIncludingCustomer |> run session

as you can see this query will return you a tuple of order & customer.

## Composition of queries

Due to the functional nature of the API we have the ability to compose queries.

For example let's say we wanted all of the customers born on a certain date and all of the orders placed on that same date. We can define the following

	let ordersOn (date : DateTime) = 
		raven { 
			return! query (where <@ fun x -> x.Dob = date @>)
		}
	
	let customersWithDob (date : DateTime) = 
		raven { 
			return! query (where <@ fun x -> x.Dob < date @>)
		}

	let composedQuery date = 
		raven { 
			let! orders = ordersOn date
			let! customers = customersWithDob date
			return orders, customers 
		}


We can then execute this query as normal

	use session = docStore.OpenSession()
	composedQuery (new DateTime(2012, 1, 1)) |> run session	
	

 
