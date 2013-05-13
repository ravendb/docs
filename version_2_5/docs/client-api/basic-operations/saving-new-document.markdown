
### Saving a new document

Before we can start saving information to RavenDB, we must define *what* we will save. For the purpose of these sections, we will use the following class structure:

	public class BlogPost
	{
	    public string Id { get; set; }
	    public string Title { get; set; }
	    public string Category { get; set; }
	    public string Content { get; set; }
	    public DateTime PublishedAt { get; set; }
	    public string[] Tags { get; set; }
	    public BlogComment[] Comments { get; set; }
	}
	 
	public class BlogComment
	{
	    public string Title { get; set; }
	    public string Content { get; set; }
	}
  
To save a new blog post in our database server, we first create a new instance of the `BlogPost` class, as shown below:

{CODE saving_document_1@ClientApi\BasicOperations\SavingNewDocument.cs /}

Neither the class itself nor instantiating it requires anything from RavenDB, either in the form of attributes or in the form of special factories. The RavenDB Client API works with simple POCO (Plain Old CLR Objects) objects.

Persisting this entire object graph involves calling `Store` and then `SaveChanges`, on a session object we obtained from our document store:

{CODE saving_document_2@ClientApi\BasicOperations\SavingNewDocument.cs /}

The `Store` method operates purely in memory and only the call of `SaveChanges` will produce the actual communication with the server (a single POST by using [request batching](../../http-api/http-api-multi#batching-requests)).

{INFO Note that the entire object graph will serialized and persisted as a *single document* on the server side, not as a set of distinct objects. /}	

#### Document IDs

In the example above we had a string Id property for `BlogPost`, and left it blank. It is this property that will be used as the "primary key" for this document. Note how RavenDB generated an ID for us, "BlogPosts/1", based on the default convention which we will discuss in a second.

If there is no `Id` property on a document, RavenDB will still generate a unique ID, but it will be retrievable only by calling `session.Advanced.GetDocumentId(object)`. In other words, having an `Id` property is entirely optional, so you can explicitly define such a property only when you need this information to be more accessible.

##### Document IDs generation strategies

RavenDB supports 3 ways of figuring out a unique id, or a document key, for a newly saved document.

By default, the HiLo algorithm is used. Whenever you create a new `DocumentStore` object and it connects to a RavenDB server, HiLo keys are exchanged and your connection is assigned a range of values it can use when a new entity is being stored. A new set of values is negotiated automatically if the range was consumed.

When you store an object with no `Id` property, or with one that wasn't manually set, RavenDB will assign it with a document ID that is combined of the collection name (which by default is the entity name in its plural form) and the next available ID from the given range.

{NOTE It is important to understand that a `Collection` is merely a convention, not something that is enforced by RavenDB. There is absolutely nothing that would prevent you from saving a `Post` with the document id of "users/1", and that would overwrite any existing document with the id "users/1", regardless of which collection it belongs to. /}

Numeric or Guid Id properties are supported and will work seamlessly. In this case, RavenDB will automatically make the translation between the inner string ID to the numeric or Guid value shown in the entity and back.

Using this approach, IDs are available immediately after calling `Store` on the object - the RavenDB session will set a unique ID in memory without asking one from the server.

RavenDB also supports the notion of Identity, for example if you need IDs to be consecutive. By creating a string `Id` property in your entity, and setting it to a value ending with a slash (/), you can tell RavenDB to use that as a key prefix for your entity. That prefix followed by the next available integer ID for it will be your entity's ID _after_ you call `SaveChanges()`.

You can also assign an ID manually by explicitly setting the string `Id` property of your object, but then if a document already exists in your RavenDB server under the same key it will be overwritten without any warning.

{WARNING If you are going to store large number of documents with custom generated IDs, you must necessarily read the topic *Dealing with custom ID for high number of documents* presented below. /}

##### Custom ID generation strategies

You can also setup a custom id generation strategy by supplying a `DocumentKeyGenerator` to the document store conventions, like so:

{CODE saving_document_3@ClientApi\BasicOperations\SavingNewDocument.cs /}

This will instruct RavenDB to use identity id generation strategy for all the entities that this document store manages.

##### Overriding default ID generation

To override default document key generation algorithms, we added `RegisterIdConvention` and `RegisterAsyncIdConvention` methods to `DocumentConvention` where you can include your own identifier generation logic.

{CODE saving_new_document_8@ClientApi\BasicOperations\SavingNewDocument.cs /}


Consider a `User` class:

{CODE saving_new_document_1@ClientApi\BasicOperations\SavingNewDocument.cs /}

To generate a custom Id with `users/` prefix combined with `Name` of the user you need to do as follows:

{CODE saving_new_document_2@ClientApi\BasicOperations\SavingNewDocument.cs /}

or if you want to register your convention for async operations then:

{CODE saving_new_document_3@ClientApi\BasicOperations\SavingNewDocument.cs /}

{NOTE Note that spectrum of identifier generation abilities is very wide, because `DatabaseCommands` object is passed into an identifier convention function and can be used for advanced calculation techniques. /}

{NOTE As of version 2.0, the `dbname` is passed to the register convention methods to allow users to make `Id` generation decision per database (e.g. default document key generator - the `MultiDatabaseHiloGenerator` - is using this parameter to prevent sharing HiLo values accross the databases) /}

{CODE saving_new_document_4@ClientApi\BasicOperations\SavingNewDocument.cs /}

Above code will store new entity in the database with `users/jdoe` as a key and below code will store user using async operation with `users/jcarter` key:

	using (var session = store.OpenAsyncSession())
	{
	    session.Store(new User
	    {
	        Name = "jcarter",
	        FirstName = "John",
	        LastName = "Carter"
	    });
	 
	    session.SaveChangesAsync();
	}

####Inheritance

Registered conventions are inheritance-aware, so all types that can be assigned from registered type will fall into that convention according to inheritance-hierarchy tree

If we will create a new class `PrivilegedUser` that will derive from our `User` class

{CODE saving_new_document_5@ClientApi\BasicOperations\SavingNewDocument.cs /}

then if we will add convention for `User`, both our types will use our custom convention

{CODE saving_new_document_6@ClientApi\BasicOperations\SavingNewDocument.cs /}

If we register two conventions, one for `User` and second for `PrivilegedUser` then they will be picked for their specific types.

{CODE saving_new_document_7@ClientApi\BasicOperations\SavingNewDocument.cs /}

####Dealing with custom ID for high number of documents

The ways shown in this section give to you a great flexibility in creating identifiers of documents. You are able to assign to a document any `ID` as you can imagine.
Everything is going to work correctly however you have to be aware that some kind of IDs might cause performance issues when number of documents with custom generated IDs is very high (millions of documents).

The very unrecommended way is to use non-sequential data as document IDs (e.g. created by hash functions). In such case you might be experiencing the declining performance of searching for an existing document and inserting new ones.
If your intention is to use custom identifiers and you expect that very high number of such documents might be saved in your database, then we suggest to make use of incremented IDs in order to ensure the good performance.
