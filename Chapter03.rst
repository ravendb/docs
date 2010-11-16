Chapter 3 - Basic Operations
****************************************

This is a chapter of firsts, in which you will learn about the basic operations of RavenDB:

* Creating documents
* Loading documents
* Modifying documents
* Querying documents
* Deleting documents 
* Using System.Transactions

This chapter assumes usage of the RavenDB .NET Client API, and will provide examples of the underlying HTTP calls behind made for each call.

Creating a document session
=====================================

In order to communicate with a RavenDB instance, we must first create a document store and initialize it like so::

    store = new DocumentStore()
    {
        Url = "http://localhost:8080"
    };
    store.Initialize();
	
This will create a store that connects to a RavenDB server running on port 8080 on the local machine. 

.. note::
 
 It is possible to embed RavenDB in an application and run it in-process by utilising an EmbeddableDocumentStore, more information about this can be found in the documentation
 
Once a store has been created, the next step is to create a session against that store that will allow us to perform basic CRUD operations within a "unit of work" against that store. It is important to note that when invoking any operations against this store, that no changes will be made to the underlying document database until SaveChanges has been invoked in the following pattern:

	using (IDocumentSession session = store.OpenSession())
	{
		// Operations against session

		// Flush those changes
		session.SaveChanges();
	}


In this context, the session can be thought of as managing the transaction scope, and SaveChanges can be thought of as committing that transaction. Certainly, any operations made as part of this session will be committed atomically (that is to say, either they all succeed, or they all fail).
	
It will be assumed in the following examples that a valid store has been created, and that the calls are being made within the context of a valid session, and that SaveChanges is being called safely at the end of that session lifetime.
	
Saving a new document
=====================================

Assuming the following class structure::

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
  
If we create a new object in our application like in the following example::

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
	
Persisting this entire object graph involves using Store like so::

	session.Store(blog);
	session.SaveChanges();

This will produce the following HTTP communication between the client and the server::

	// PASTE HERE THE HTTP CALL SHOWING WHAT HAPPENS WHEN YOU CALL SAVE CHANGES
	
Two things of note at this point:

* We left the "Id" property of Blog blank, and it is this property that will be used as the "primary key" for this document
* The entire object graph is serialized and persisted as a single document

.. note::
	If there is no "Id" property on a document, RavenDB will allocate an Id, but it will not be retrievable. In other words, having an Id is entirely optional, but as it is generally more useful to have this information available, most of your documents should have an Id property.

Loading & Editing an existing document
=====================================

If you have the id of an existing document (for example the previous saved blog entry), it can be loaded in the following manner::

	Blog existingBlog = session.Load<Blog>("blogs/1");

Changes can then be made to that object in the usual manner::

	existingBlog.Title = "Some new title";
	
Flushing those changes to the document store is achieved in the usual way::

	session.SaveChanges();
	
For the above example, the above example will result in the following HTTP message::
	
	// PASTE HERE THE HTTP MESSAGE BEING SENT
	
.. note::
	The entire document is sent to the server with the Id set to the existing document value, this means that the existing document will be replaced in the document store with the new one. Whilst patching operations are possible with RavenDB, the client API by default will always just replace the entire document in its entirety.
	
Deleting existing documents
=====================================

Once a valid reference to a document has been retrieved, the document can be deleted with a call to Delete in the following manner::

session.Delete(blog);

	
