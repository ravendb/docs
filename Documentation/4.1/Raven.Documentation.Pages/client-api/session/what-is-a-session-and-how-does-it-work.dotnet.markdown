# Session: What is a Session and How Does it Work

---

{NOTE: }

The **Session** is:  

* A [Unit of Work](https://martinfowler.com/eaaCatalog/unitOfWork.html) that represents a single business transaction on a particular database.

* Used to perform any database operation you might want. The basic CRUD operations are performed through `session`, more **advanced operations** are available through `session.advanced`.

* Obtained from the `DocumentStore`.  
  * Read more about [The Document Store](../../client-api/what-is-a-document-store) and [How To Create One](../../client-api/creating-document-store).  

* Disposable. (Implements [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable?view=netframework-4.7.2))  
  * Open every session in a `using` statement to ensure proper disposal.  

* Tracks all changes made to entities and combines them into a single write operation in the next call to `SaveChanges()`. This minimizes the total number of calls that go to the server over the network.  
  

* In this page:  
  * [Simple Example of Session Usage](../../client-api/session/what-is-a-session-and-how-does-it-work#simple-example-of-session-usage)  
  * [Unit of Work Pattern](../../client-api/session/what-is-a-session-and-how-does-it-work#unit-of-work-pattern)  
      * [Batching](../../client-api/session/what-is-a-session-and-how-does-it-work#batching)  
  * [Identity Map Pattern](../../client-api/session/what-is-a-session-and-how-does-it-work#identity-map-pattern)
  * [session.advanced]() **TODO**
  * [Select N+1 Problem](../../client-api/session/what-is-a-session-and-how-does-it-work#select-n+1-problem)

{NOTE/}  

---
{PANEL:Simple Example of Session Usage}

The Client API, and the session in particular, is designed to be as straightforward as possible. The example code below shows how we can use a session to store a document in the database.  

{CODE session_usage_1@ClientApi\Session\WhatIsSession.cs /}

Calling `SaveChanges()` sends the entity to the RavenDB server where it is stored as a new document in the database. (Since no other database was specified, this will be the [default database](http://localhost:54391/docs/article-page/4.1/csharp/client-api/setting-up-default-database)).  
{PANEL/}

{PANEL:Unit of Work Pattern}

The session implements the [Unit of Work Pattern](https://martinfowler.com/eaaCatalog/unitOfWork.html). The session manages change tracking for all the entities that it has either loaded or stored.  

{CODE session_usage_2@ClientApi\Session\WhatIsSession.cs /}

###Batching
Remote calls to a server are among the most expensive operations your application can make. The session optimizes this by batching all write operations it has accumulated into a single call to the server, sent when `SaveChanges()` is called. This is the default behavior of the session; you don't have to enable it. This also ensures that all writes to the database are always executed as a single atomic transaction, no matter how many operations you are actually executing.  

{INFO:How to Disable Entity Tracking}
Entity tracking can be disabled using the `SessionOptions.NoTracking` property when a session is being [opened](../../client-api/session/opening-a-session#example-ii---disabling-entities-tracking).  
{INFO/}
{PANEL/}

{PANEL:Identity Map Pattern}
The session implements the [Identity Map Pattern](https://martinfowler.com/eaaCatalog/identityMap.html). If we load the same document twice in the same session, it won't be saved as two different objects. A single document always resolves to the same entity instance.  
{CODE session_usage_3@ClientApi\Session\WhatIsSession.cs /}
{PANEL/}

{PANEL:`session.advanced`} 
{PANEL/}

{PANEL:Select N+1 Problem}

The [Select N+1 problem](http://blogs.microsoft.co.il/gilf/2010/08/18/select-n1-problem-how-to-decrease-your-orm-performance/) is very common with all ORMs and ORM-like APIs, including the RavenDB Client API. It results in an excessive number of remote calls to the server, which makes a query very expensive.

The select n+1 problem should never arise if RavenDB is being utilized correctly. The number of remote calls per session should be as close to 1 as possible. If the maximum requests limit is reached, it is a sure sign of either select n+1 or some other misuse of the session.

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

- [Opening a Session](../../client-api/session/opening-a-session)
- [Storing Entities](../../client-api/session/storing-entities)
- [Loading Entities](../../client-api/session/loading-entities)
- [Saving Changes](../../client-api/session/saving-changes)
- [Querying Basics](../../indexes/querying/basics)
- [What is a Document Store](../../client-api/what-is-a-document-store)
