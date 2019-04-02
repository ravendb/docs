# Session: What is a Session and How Does it Work

---

{NOTE: }

* To perform any operation we want on our database, we begin by obtaining a new `session` object from the `DocumentStore`.  
  * Read more about [the document store](../../client-api/what-is-a-document-store) and [how to create one](../../client-api/creating-document-store).  

* If the **document store** is the connection manager for a cluster of servers, the **document session** (or just 'session') is what we use to make an actual HTTP call to the server.  

* The `session` object is **disposable**.  
  * Open every session in a `using` statement to ensure proper disposal.  

* **Optimized** to minimize the total number of calls that go over the network.  

* Designed to be as straightforward as possible for the basic CRUD operations. Further options are accessible through the `session.advanced` property.  

* In this page:  
  * [Example - Typical Session Usage](../../client-api/session/what-is-a-session-and-how-does-it-work#example---typical-session-usage)  
  * [Unit of Work Pattern](../../client-api/session/what-is-a-session-and-how-does-it-work#unit-of-work)  
  * [Batching](../../client-api/session/what-is-a-session-and-how-does-it-work#batching)  
  * [Select N+1 Problem](../../client-api/session/what-is-a-session-and-how-does-it-work#select-n+1-problem)  

{NOTE/} 

{PANEL:Example - Typical Session Usage}

The Client API, and the session in particular, is designed to be as straightforward as possible. The example code below shows how we can use a session to store a document in the database, 
and later use another session to load that document back into our client application.  

{CODE session_usage_1@ClientApi\Session\WhatIsSession.cs /}

* `//first session`: we open a session, create a new entity of class 'company', and `Store()` it. Calling `SaveChanges()` sends the entity to the RavenDB server where it is stored as a new document 
in the database. (Since no other is specified, this will be the [default database](http://localhost:54391/docs/article-page/4.1/csharp/client-api/setting-up-default-database)).  

* `//second session`: we open a different session and load the document we saved earlier. Now we can do something with it, like make edits.
{PANEL/}

{PANEL:Unit of Work}

The Client API implements the Unit of Work pattern. This has several implications:  

* In the context of a single session, a single document (identified by its ID) always resolves to the same instance.  

{CODE session_usage_3@ClientApi\Session\WhatIsSession.cs /}

* The session manages change tracking for all the entities that it has either loaded or stored.  

{CODE session_usage_2@ClientApi\Session\WhatIsSession.cs /}

{INFO:How to Disable Entity Tacking}

Entity tracking can be disabled using the `SessionOptions.NoTracking` property when a session is being [opened](../../client-api/session/opening-a-session#example-ii---disabling-entities-tracking).

{INFO/}
{PANEL/}

{PANEL:Batching}

Remote calls to the server are among the most expensive operations an application can make. The RavenDB Client API optimizes this by batching all write calls to the server into a single call. This is the default behavior of a session; you don't have to enable it. This also ensures that writes to the database are always executed as a single transaction, no matter how many operations you are actually executing.
{PANEL/}

{PANEL:Select N+1 Problem}

The [Select N+1 problem](http://blogs.microsoft.co.il/gilf/2010/08/18/select-n1-problem-how-to-decrease-your-orm-performance/) is very common with all ORMs and ORM-like APIs, including the RavenDB Client API. It results in an excessive number of remote calls to the server, which makes a query very expensive.

The select n+1 problem should never arise if RavenDB is being utilized correctly. The number of remote calls per session should be as close to 1 as possible. If maximum requests limit is reached, it is a sure sign of either select n+1 or some other misuse of the RavenDB session.

Should it arise, RavenDB offers a number of ways to mitigate this problem:  

* [Perform Queries Lazily](https://ravendb.net/docs/article-page/4.1/csharp/client-api/session/querying/how-to-perform-queries-lazily)  
* [Stream Query Results](https://ravendb.net/docs/article-page/4.1/csharp/client-api/session/querying/how-to-stream-query-results)  

{INFO: Configuring Maximum Requests per Session} 
By default the maximum number of requests allowed per session is 30. Exceeding this limit causes an exception to be thrown.
This limit be changed at the `DocumentConventions::MaxNumberOfRequestsPerSession` property.
{INFO/}
{PANEL/}

## Related Articles  

### Client API  

#### Session
- [Opening a Session](../../client-api/session/opening-a-session)
- [Storing Entities](../../client-api/session/storing-entities)
- [Loading Entities](../../client-api/session/loading-entities)
- [Saving Changes](../../client-api/session/saving-changes)

#### Querying
- [Basics](../../indexes/querying/basics)

#### Document Store
- [What is a Document Store](../../client-api/what-is-a-document-store)
