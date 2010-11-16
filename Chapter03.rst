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
=====================================

If you have the id of an existing document (for example the previous saved blog entry), it can be loaded in the following manner::

	Blog existingBlog = session.Load<Blog>("blogs/1");

This results in the HTTP communication shows in listing 3.7::
	
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

	
