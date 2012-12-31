# Saving a new document

Before we can start saving information to RavenDB, we must define *what* we will save. For the purpose of these sections, we will use the following class structure:

{CODE blogpost_classes@Common.cs /}
  
To save a new blog post in our database server, we first create a new instance of the `BlogPost` class, as shown below:

{CODE saving_document_1@Intro\BasicOperations.cs /}

Neither the class itself nor instantiating it requires anything from RavenDB, either in the form of attributes or in the form of special factories. The RavenDB Client API works with simple POCO (Plain Old CLR Objects) objects.

Persisting this entire object graph involves calling `Store` and then `SaveChanges`, on a session object we obtained from our document store:

{CODE saving_document_2@Intro\BasicOperations.cs /}

The `SaveChanges` call will produce the actual HTTP communication, which is shown below. Note that the `Store` method operates purely in memory, and only the call to `SaveChanges` communicates with the server (json was prettified for clarity):

{CODE-START:json /}
    POST /bulk_docs HTTP/1.1
    Accept-Encoding: deflate,gzip
    Content-Type: application/json; charset=utf-8
    Host: 127.0.0.1:8080
    Content-Length: 378
    Expect: 100-continue

    [
      {
        "Key": "BlogPosts/1",
        "Etag": null,
        "Method": "PUT",
        "Document": {
          "Title": "Hello RavenDB",
          "Category": "RavenDB",
          "Content": "This is a blog about RavenDB",
          "Comments": [
            {
              "Title": "Unrealistic",
              "Content": "This example is unrealistic"
            },
            {
              "Title": "Nice",
              "Content": "This example is nice"
            }
          ]
        },
        "Metadata": {
          "Raven-Entity-Name": "BlogPosts",
          "Raven-Clr-Type": "BlogPost"
        }
      }
    ]


    HTTP/1.1 200 OK
    Content-Type: application/json; charset=utf-8
    Server: Microsoft-HTTPAPI/2.0
    Date: Tue, 16 Nov 2010 20:37:00 GMT
    Content-Length: 205

    [
      {
        "Etag": "00000000-0000-0100-0000-000000000002",
        "Method": "PUT",
        "Key": "BlogPosts/1",
        "Metadata": {
          "Raven-Entity-Name": "BlogPosts",
          "Raven-Clr-Type": "BlogPost",
          "@id": "BlogPosts/1"
        }
      }
    ]
{CODE-END/}
	
Note how the entire object graph is serialized and persisted as a *single document*, not as a set of distinct objects.

## Document IDs

In the example above we had a string Id property for `BlogPost`, and left it blank. It is this property that will be used as the "primary key" for this document. Note how RavenDB generated an ID for us, "BlogPosts/1", based on the default convention which we will discuss in a second.

If there is no `Id` property on a document, RavenDB will still generate a unique ID, but it will be retrievable only by calling `session.Advanced.GetDocumentId(object)`. In other words, having an `Id` property is entirely optional, so you can explicitly define such a property only when you need this information to be more accessible.

### Document IDs generation strategies

RavenDB supports 3 ways of figuring out a unique id, or a document key, for a newly saved document.

By default, the HiLo algorithm is used. Whenever you create a new `DocumentStore` object and it connects to a RavenDB server, HiLo keys are exchanged and your connection is assigned a range of values it can use when a new entity is being stored. A new set of values is negotiated automatically if the range was consumed.

When you store an object with no `Id` property, or with one that wasn't manually set, RavenDB will assign it with a document ID that is combined of the collection name (which by default is the entity name in its plural form) and the next available ID from the given range.

{NOTE It is important to understand that a `Collection` is merely a convention, not something that is enforced by RavenDB. There is absolutely nothing that would prevent you from saving a `Post` with the document id of "users/1", and that would overwrite any existing document with the id "users/1", regardless of which collection it belongs to. /}

Numeric or Guid Id properties are supported and will work seamlessly. In this case, RavenDB will automatically make the translation between the inner string ID to the numeric or Guid value shown in the entity and back.

Using this approach, IDs are available immediately after calling `Store` on the object - the RavenDB session will set a unique ID in memory without asking one from the server.

RavenDB also supports the notion of Identity, for example if you need IDs to be consecutive. By creating a string `Id` property in your entity, and setting it to a value ending with a slash (/), you can tell RavenDB to use that as a key prefix for your entity. That prefix followed by the next available integer ID for it will be your entity's ID _after_ you call `SaveChanges()`.

You can also assign an ID manually by explicitly setting the string `Id` property of your object, but then if a document already exists in your RavenDB server under the same key it will be overwritten without any warning.

### Custom ID generation strategies

You can also setup a custom id generation strategy by supplying a `DocumentKeyGenerator` to the document store conventions, like so:

  store.Conventions.DocumentKeyGenerator = entity => store.Conventions.GetTypeTagName(entity.GetType()) + "/";
  
This will instruct RavenDB to use identity id generation strategy for all the entities that this document store manages.