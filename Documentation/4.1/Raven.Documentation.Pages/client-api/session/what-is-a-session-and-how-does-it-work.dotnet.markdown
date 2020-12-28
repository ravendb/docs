# Session: What is a Session and How Does it Work  

---

{NOTE: }  

* The **Session**, which is obtained from the [Document Store](../../client-api/what-is-a-document-store), is a 
  [Unit of Work](https://martinfowler.com/eaaCatalog/unitOfWork.html) that represents  
  a single business transaction on a particular database.  

* Basic **document CRUD** actions and **document Queries** are available through the `Session`.  
  More advanced options are available using `Advanced` Session operations.  

* The Session **Tracks all Changes** made to all entities that it has either loaded, stored, or queried on,  
  and persists to the server only what is needed when `SaveChanges()` is called.  

* The number of server requests allowed per session is configurable (default is 30).  
  See: [Change Maximum Number of Requests per Session](../../client-api/session/configuration/how-to-change-maximum-number-of-requests-per-session)  

* In this page:
  * [Unit of Work Pattern](../../client-api/session/what-is-a-session-and-how-does-it-work#unit-of-work-pattern)
      * [Tracking Changes](../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes)
      * [Batching](../../client-api/session/what-is-a-session-and-how-does-it-work#batching)
      * [Store New Document Example](../../client-api/session/what-is-a-session-and-how-does-it-work#store-new-document-example)
      * [Load & Edit Document Example](../../client-api/session/what-is-a-session-and-how-does-it-work#load--edit-document-example)
  * [Identity Map Pattern](../../client-api/session/what-is-a-session-and-how-does-it-work#identity-map-pattern)
  * [Reducing Server Calls (Best Practices) For:](../../client-api/session/what-is-a-session-and-how-does-it-work#reducing-server-calls-(best-practices)-for:)
      * [The N+1 Problem](../../client-api/session/what-is-a-session-and-how-does-it-work#the-select-n1-problem)
      * [Large Query Results](../../client-api/session/what-is-a-session-and-how-does-it-work#large-query-results)
      * [Retrieving Results on Demand (Lazy)](../../client-api/session/what-is-a-session-and-how-does-it-work#retrieving-results-on-demand-lazy)

{NOTE/}  

---

{PANEL: Unit of Work Pattern}  

#### Tracking Changes
* Using the Session, perform needed operations on your documents.  
  e.g. store a new document, modify an existing document, query your database, etc.  
  Any such operation '*loads*' the documents to the Session for tracking.  
* The Session **tracks all changes** made to all entities that it has either loaded or stored.  
  You don't need to manually track the changes to these entities and decide what needs to be saved and what doesn't.  
  The Session will do it for you.  
* All these tracked changes are combined & persisted to the database only when calling `SaveChanges()`.  
* Entity tracking can be disabled if needed. See:  
  * [Disable Entity Tracking](../../client-api/session/configuration/how-to-disable-tracking)  
  * [Clear a Session](../../client-api/session/how-to/clear-a-session)  
<br>
#### Batching  
* Remote calls to a server over the network are among the most expensive operations an application makes.  
  The session optimizes this by batching all write operations it has tracked into the single `SaveChanges()` call.  
* The `SaveChanges()` call checks the Session state for all changes made to the tracked entities.  
  These changes are sent to the server as a single remote call that will complete transactionally.  
  In other words, either all changes are saved as a **Single Atomic Transaction** or none of them are.  
  Once `SaveChanges()` returns, it is guaranteed that the changes are persisted to the database.  
* The `SaveChanges()` is the only time when a RavenDB client sends updates to the server,  
  so you will experience a reduced number of network calls.  
<br>
#### Store New Document Example  
* The Client API, and the Session in particular, is designed to be as straightforward as possible.  
  Open the session, do some operations, and apply the changes to the RavenDB server.  
* The example below shows how to store a new document in the database using the Session.  

{CODE session_usage_1@ClientApi\Session\WhatIsSession.cs /}  

#### Load & Edit Document Example  
* The example below loads & edits an existing document and then saves the changes.  

{CODE session_usage_2@ClientApi\Session\WhatIsSession.cs /}  

{PANEL/}  

{PANEL:Identity Map Pattern}  

* The session implements the [Identity Map Pattern](https://martinfowler.com/eaaCatalog/identityMap.html).

* The first document `Load()` call goes to the server and fetches the document from the database.  
  The document is then saved as an entity in the Session's identity map.  

* All subsequent `Load()` calls to the same document will simply retrieve the entity from the Session -  
  no additional calls to the server  are made.  

{CODE session_usage_3@ClientApi\Session\WhatIsSession.cs /}  

* Note: To override this and update an entity with the latest changes from the server see: 
  [Refresh an Entity](../../client-api/session/how-to/refresh-entity)  

{PANEL/}  

{PANEL: Reducing Server Calls (Best Practices) For:}
#### The Select N+1 Problem
* The Select N+1 problem is common 
  with all ORMs and ORM-like APIs.  
  It results in an excessive number of remote calls to the server, which makes a query very expensive.  
* Make use of RavenDB's `include()` method to include related documents and avoid this issue.  
  See: [Document Relationships](../../client-api/how-to/handle-document-relationships)  
<br>
#### Large query results
* When query results are large and you don't want the overhead of keeping all results in memory, you can 
  [Stream the Query Results](../../client-api/session/querying/how-to-stream-query-results).  
  A single server call is executed and the client can handle the results one by one.  
* Note: [Paging](../../indexes/querying/paging) also avoids getting all query results in one time, but multiple server calls are 
  generated - one per page retrieved.  
<br>
#### Retrieving results on demand (Lazy)
* Query calls to the server can be delayed and executed on-demand as needed using `Lazily()`
* See [Perform Queries Lazily](../../client-api/session/querying/how-to-perform-queries-lazily)

{PANEL/}

## Related Articles  

### Client API  

- [Opening a Session](../../client-api/session/opening-a-session)
- [Storing Entities](../../client-api/session/storing-entities)
- [Loading Entities](../../client-api/session/loading-entities)
- [Saving Changes](../../client-api/session/saving-changes)
- [Querying Basics](../../indexes/querying/basics)
- [What is a Document Store](../../client-api/what-is-a-document-store)
