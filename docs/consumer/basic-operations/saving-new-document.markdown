# Saving a new document

Before we can start saving information to RavenDB, we must define *what* we will save. For the purpose of these chapters, we will use the following class structure:

{CODE blogpost_classes@Common.cs /}
  
To save a new blog post in our database server, we first create a new instance of the `BlogPost` class, as shown below:

{CODE saving_document_1@Intro\BasicOperations.cs /}

{INFO Neither the class itself nor instantiating it required anything from RavenDB, either in the form of attributes or in the form of special factories. The RavenDB Client API works with POCO (Plain Old CLR Objects) objects. /}

Persisting this entire object graph involves calling `Store` and then `SaveChanges`, on a session object we obtained from our document store:

{CODE saving_document_2@Intro\BasicOperations.cs /}

The `SaveChanges` call will produce the HTTP communication shown below. Note that the `Store` method operates purely in memory, and only the call to `SaveChanges` communicates with the server (json was prettified for clarity):

    POST /bulk_docs HTTP/1.1
    Accept-Encoding: deflate,gzip
    Content-Type: application/json; charset=utf-8
    Host: 127.0.0.1:8080
    Content-Length: 378
    Expect: 100-continue

    [
      {
        "Key": "blogs/1",
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
          "Raven-Entity-Name": "Blogs",
          "Raven-Clr-Type": "Blog"
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
        "Key": "blogs/1",
        "Metadata": {
          "Raven-Entity-Name": "Blogs",
          "Raven-Clr-Type": "Blog",
          "@id": "blogs/1"
        }
      }
    ]

	
Two things of note at this point:

* We left the "Id" property of BlogPost blank, and it is this property that will be used as the "primary key" for this document. RavenDB generated an id for us, "blogs/1", based on the default convention.

* The entire object graph is serialized and persisted as a *single document*, not as a set of distinct objects.

{NOTE If there is no `Id` property on a document, RavenDB will allocate an Id, but it will be retrievable only by calling `session.Advanced.GetDocumentId`. In other words, having an Id is entirely optional, but as it is generally more useful to have this information available, most of your documents should have an Id property. /}
