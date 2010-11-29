Chapter 3 - Basic Operations
****************************************

In this chapter: 

* Creating and modifying documents
* Loading documents
* Querying documents 
* Using System.Transactions

So far we have spoken in abstracts, about No SQL in general and RavenDB in particular, but in this chapter, we leave the high level concepts aside and concentrate
on actually using RavenDB. We will go through all the steps required to perform basic CRUD operations using RavenDB, familizaring ourselves with RavenDB APIs, concepts
and workings.
This chapter assumes usage of the RavenDB .NET Client API, and will provide examples of the underlying HTTP calls behind made for each call.

Creating a document session
=====================================

In order to communicate with a RavenDB instance, we must first create a document store and initialize it, you can see a sample of initializing a document store
in listing 3.1::

	// listing 3.1 - initializing a new document store
    store = new DocumentStore()
    {
        Url = "http://localhost:8080"
    };
    store.Initialize();
	
This will create a document store that connects to a RavenDB server running on port 8080 on the local machine. 

.. note::
 
 It is possible to run RavenDB using an embedded mode in an application and run it in-process by utilising an EmbeddableDocumentStore, more information about this 
 can be found in the documentation.
 
Once a document store has been created, the next step is to create a session against that document store that will allow us to perform basic CRUD operations within a Unit of Work. It is important to note that when invoking any operations against this store, that no changes will be made to the underlying document database until the ``SaveChanges`` method has been called, as in listing 3.2::

	// listing 3.2 - saving changes using the session API
	
	using (IDocumentSession session = store.OpenSession())
	{
		// Operations against session

		// Flush those changes
		session.SaveChanges();
	}


In this context, the session can be thought of as managing all changes internally, and SaveChanges can be thought of as committing all those changes to the RavenDB server. Any operations submitted in a ``SaveChanges`` call will be committed atomically (that is to say, either they all succeed, or they all fail).

It will be assumed in the following examples that a valid store has been created, and that the calls are being made within the context of a valid session, and that ``SaveChanges`` is being called safely at the end of that session lifetime.

.. note::
  
  If you don't call ``SaveChanges``, all the changes made in that session will be discarded!
	
Saving a new document
=====================================

Before we can start saving information to RavenDB, we must define *what* we will save. You can see the sample class structure in 
listing 3.3::

	// listing 3.3 - Simple class structure
	
    public class Blog
    {
		public string Id { get; set ; }
        public string Title { get; set; }
		public string Category { get; set; }
		public string Content { get; set; }
		public BlogComment[] Comments { get; set; }
    }
   
   public class BlogComment
   {
       public string Title { get; set; }
	   public string Content { get; set;}
   }
  
We can now create a new instance of the ``Blog`` class, as shown in listing 3.4::

	// listing 3.4 - creating a new instance of the Blog class
	Blog blog = new Blog()
	{
		Title = "Hello RavenDB",
		Category = "RavenDB",
		Content = "This is a blog about RavenDB",
		Comments = new BlogComment[]{
			new BlogComment() { Title = "Unrealistic", Content= "This example is unrealistic"},
			new BlogComment() { Title = "Nice", Content= "This example is nice"}

		  }
	};

.. note::

	Neither the class itself or instansiating it required anything from RavenDB, either in the form of attributes or in the form of special
	factories. The RavenDB Client API works with POCO (Plain Old CLR Objects) objects.

Persisting this entire object graph involves using ``Store`` and then ``SaveChanges``, as seen in listing 3.5::

	// listing 3.5 - saving the new instance to RavenDB
	session.Store(blog);
	session.SaveChanges();

The ``SaveChanges`` call will product the HTTP communication shown in listing 3.6. Note that the ``Store`` method operates purely in memory, and only
the call to ``SaveChanges`` communicates with the server::

	POST /bulk_docs HTTP/1.1
	Accept-Encoding: deflate,gzip
	Content-Type: application/json; charset=utf-8
	Host: 127.0.0.1:8080
	Content-Length: 378
	Expect: 100-continue

	[{"Key":"blogs/1","Etag":null,"Method":"PUT","Document":{"Title":"Hello RavenDB","Category":"RavenDB","Content":"This is a blog about RavenDB","Comments":[{"Title":"Unrealistic","Content":"This example is unrealistic"},{"Title":"Nice","Content":"This example is nice"}]},"Metadata":{"Raven-Entity-Name":"Blogs","Raven-Clr-Type":"Blog"}}]
	
	
	HTTP/1.1 200 OK
	Content-Type: application/json; charset=utf-8
	Server: Microsoft-HTTPAPI/2.0
	Date: Tue, 16 Nov 2010 20:37:00 GMT
	Content-Length: 205

	[{"Etag": "00000000-0000-0100-0000-000000000002","Method":"PUT","Key":"blogs/1","Metadata":{"Raven-Entity-Name":"Blogs","Raven-Clr-Type":"Blog","@id":"blogs/1"}}]

	
Two things of note at this point:

* We left the "Id" property of Blog blank, and it is this property that will be used as the "primary key" for this document
* The entire object graph is serialized and persisted as a *single document*, not as a set of distinct objects.

.. note::
	If there is no "Id" property on a document, RavenDB will allocate an Id, but it will be retrievable only by calling ``session.Advanced.GetDocumentId``. In other words, having an Id is entirely optional, but as it is generally more useful to have this information available, most of your documents should have an Id property.

Loading & Editing an existing document
========================================

If you have the id of an existing document (for example the previous saved blog entry), it can be loaded in the following manner::

	Blog existingBlog = session.Load<Blog>("blogs/1");

This results in the HTTP communication shown in listing 3.7::
	
	GET /docs/blogs/1 HTTP/1.1
	Accept-Encoding: deflate,gzip
	Content-Type: application/json; charset=utf-8
	Host: 127.0.0.1:8080

	HTTP/1.1 200 OK
	Content-Type: application/json; charset=utf-8
	Last-Modified: Tue, 16 Nov 2010 20:37:01 GMT
	ETag: 00000000-0000-0100-0000-000000000002
	Server: Microsoft-HTTPAPI/2.0
	Raven-Entity-Name: Blogs
	Raven-Clr-Type: Blog
	Date: Tue, 16 Nov 2010 20:39:41 GMT
	Content-Length: 214

	{"Title":"Hello RavenDB","Category":"RavenDB","Content":"This is a blog about RavenDB","Comments":[{"Title":"Unrealistic","Content":"This example is unrealistic"},{"Title":"Nice","Content":"This example is nice"}]}

Changes can then be made to that object in the usual manner::

	existingBlog.Title = "Some new title";
	
Flushing those changes to the document store is achieved in the usual way::

	session.SaveChanges();
	
You don't have to call an ``Update`` method, or track any changes yourself. RavenDB will do all of that for you.
For the above example, the above example will result in the following HTTP message::
	
	POST /bulk_docs HTTP/1.1
	Accept-Encoding: deflate,gzip
	Content-Type: application/json; charset=utf-8
	Host: 127.0.0.1:8080
	Content-Length: 501
	Expect: 100-continue

	[{"Key":"blogs/1","Etag":null,"Method":"PUT","Document":{"Title":"Some new title","Category":"RavenDB","Content":"This is a blog about RavenDB","Comments":[{"Title":"Unrealistic","Content":"This example is unrealistic"},{"Title":"Nice","Content":"This example is nice"}]},"Metadata":{"Content-Encoding":"gzip","Raven-Entity-Name":"Blogs","Raven-Clr-Type":"Blog","Content-Type":"application/json; charset=utf-8","@etag":"00000000-0000-0100-0000-000000000002"}}]
	
	HTTP/1.1 200 OK
	Content-Type: application/json; charset=utf-8
	Server: Microsoft-HTTPAPI/2.0
	Date: Tue, 16 Nov 2010 20:39:41 GMT
	Content-Length: 280

	[{"Etag": "00000000-0000-0100-0000-000000000003","Method":"PUT","Key":"blogs/1","Metadata":{"Content-Encoding":"gzip","Raven-Entity-Name":"Blogs","Raven-Clr-Type":"Blog","Content-Type":"application/json; charset=utf-8","@id":"blogs/1"}}]
	
.. note::
	The entire document is sent to the server with the Id set to the existing document value, this means that the existing document will be replaced in the document store with the new one. Whilst patching operations are possible with RavenDB, the client API by default will always just replace the entire document in its entirety.
	
Deleting existing documents
=====================================

Once a valid reference to a document has been retrieved, the document can be deleted with a call to Delete in the following manner::

	session.Delete(blog);
	session.SaveChanges();

Once again, this results in an HTTP communcation as shown in Listing listing 3.8::

	POST /bulk_docs HTTP/1.1
	Accept-Encoding: deflate,gzip
	Content-Type: application/json; charset=utf-8
	Host: 127.0.0.1:8081
	Content-Length: 49
	Expect: 100-continue

	[{"Key":"blogs/1","Etag":null,"Method":"DELETE"}]

	
Transaction support in RavenDB
=====================================

All the previous examples have assumed that a single unit of work can be achieved with a single IDocumentSession and a single call to ``SaveChanges`` - for the most part this is definitely true - sometimes however we do need multiple calls to ``SaveChanges`` for one reason or another, but we want those calls to be contained within a single atomic operation.

RavenDB supports System.Transaction for multiple operations against a RavenDB server, or even against multiple RavenDB servers.

The client code for this is as simple as::

	using (var transaction = new TransactionScope())
	{
		Blog existingBlog = session.Load<Blog>("blogs/1");

		existingBlog.Title = "Some new title";

		session.SaveChanges();


		session.Delete(existingBlog);
		session.SaveChanges();

		transaction.Complete();
	}
	
If at any point any of this code fails, none of the changes will be enacted against the RavenDB document store.

The implementation details of this are not important, although it is possible to see that RavenDB does indeed send a transaction Id along with all of the the HTTP requests under this transaction scope as shown in listing 3.9::

	POST /bulk_docs HTTP/1.1
	Raven-Transaction-Information: 975ee0bf-cac9-4b8e-ba29-377de722f037, 00:01:00
	Accept-Encoding: deflate,gzip
	Content-Type: application/json; charset=utf-8
	Host: 127.0.0.1:8081
	Content-Length: 300
	Expect: 100-continue

	[{"Key":"blogs/1","Etag":null,"Method":"PUT","Document":{"Title":"Some new title","Category":null,"Content":null,"Comments":null},"Metadata":{"Raven-Entity-Name":"Blogs","Raven-Clr-Type":"ConsoleApplication5.Blog, ConsoleApplication5","@id":"blogs/1","@etag":"00000000-0000-0200-0000-000000000002"}}]

A call to commit involves a separate call to another HTTP endpoint with that transaction id::

	POST /transaction/commit?tx=975ee0bf-cac9-4b8e-ba29-377de722f037 HTTP/1.1
	Accept-Encoding: deflate,gzip
	Content-Type: application/json; charset=utf-8
	Host: 127.0.0.1:8081
	Content-Length: 0

.. note::
	While RavenDB supports System.Transactions, it is not recommended that this be used as an ordinary part of application workflow as it is part for the partition tolerance aspect of our beloved "CAP theorum".

Basic query support in RavenDB
=====================================

Once data has been stored in RavenDB, the next useful operation is the ability to query based on some aspect of the documents that have been stored.

For example, we might wish to ask for all the blog entries that belong to a certain category like so::

	var results = from blog in session.Query<Blog>()
				  where blog.Category == "RavenDB"
				  select blog;
				  
That Just Works(tm) and gives us all the blogs with a category of RavenDB.

The HTTP communication for this operation is shown in listing 3.10::

	GET /indexes/dynamic/Blogs?query=Category:RavenDB&start=0&pageSize=128 HTTP/1.1
	Accept-Encoding: deflate,gzip
	Content-Type: application/json; charset=utf-8
	Host: 127.0.0.1:8081

The important part of this query is that we are querying the "Blogs" collection, for the property "Category" with the value of "RavenDB".

We will also notice that a page size of 128 was passed along, although none was specified, which leads us onto the next topic of "Safe by default".

Safe by default
=====================================

RavenDB, by default, will not allow operations that might compromise the stability of either the server or the client. The two examples that present themselves in the above examples are

* If a page size value is not specified, the length of the results will be limited to 128 results
* The number of remote calls to the server per session is limited to 30

The first one is obvious - unbounded result sets are dangerous, and have been the cause of many failures in ORM based systems - unless a result-size has been specified, RavenDB will automatically attempt to limit the size of the returned result set.

The second example is less immediate, and should never be reached if RavenDB is being utilised correctly - remote calls are expensive, and the number of remote calls per "session" should be as close to "1" as possible. If the limit is reached, it is a sure sign of either a Select N+1 problem or other mis-use of the RavenDB session.

Summary
=====================================

In this chapter we learned how to utilise the session as a basic "Unit of Work" in RavenDB, and saw a basic example of querying in action as well as examples of how these remote calls look in raw HTTP calls across the wire.

We also saw how RavenDB attempts to be "safe" by default in limiting the capacity of common mistakes to cause damage in your application.

In the next chapter, we will look more closely at the query API, and how to utilise this within our applications to good effect.