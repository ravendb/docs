# What is a Session and How Does it Work  

---

{NOTE: }  

* The **session**, which is obtained from the [Document Store](../../client-api/what-is-a-document-store),  
  is a [Unit of Work](https://martinfowler.com/eaaCatalog/unitOfWork.html) that represents a single business transaction on a particular database.  

* Basic **document CRUD** actions and **document Queries** are available through the `session`.  
  More advanced options are available using `advanced` session operations.  

* The session **tracks all changes** made to all entities that it has either loaded, stored, or queried for,  
  and persists to the server only what is needed when `saveChanges()` is called.  

* The number of server requests allowed per session is configurable (default is 30).  
  See: [Change maximum number of requests per session](../../client-api/session/configuration/how-to-change-maximum-number-of-requests-per-session).  

* In this page:
  * [Unit of work pattern](../../client-api/session/what-is-a-session-and-how-does-it-work#unit-of-work-pattern)  
      * [Tracking changes](../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes)
      * [Create document example](../../client-api/session/what-is-a-session-and-how-does-it-work#create-document-example)
      * [Modify document example](../../client-api/session/what-is-a-session-and-how-does-it-work#modify-document-example)
  * [Identity map pattern](../../client-api/session/what-is-a-session-and-how-does-it-work#identity-map-pattern)
  * [Batching & Transactions](../../client-api/session/what-is-a-session-and-how-does-it-work#batching-&-transactions)<br><br>
  * [Reducing server calls (best practices) for:](../../client-api/session/what-is-a-session-and-how-does-it-work#reducing-server-calls-(best-practices)-for:)  
      * [The N+1 problem](../../client-api/session/what-is-a-session-and-how-does-it-work#the-select-n1-problem)    
      * [Large query results](../../client-api/session/what-is-a-session-and-how-does-it-work#large-query-results)    
      * [Retrieving results on demand (Lazy)](../../client-api/session/what-is-a-session-and-how-does-it-work#retrieving-results-on-demand-lazy)

{NOTE/}  

---

{PANEL: Unit of work pattern}  

#### Tracking changes

* Using the session, perform needed operations on your documents.  
  e.g. create a new document, modify an existing document, query for documents, etc.  
* Any such operation '*loads*' the document as an entity to the Session,  
  and it is added to the __Session's entities map__.
* The session **tracks all changes** made to all entities stored in its internal map.  
  You don't need to manually track the changes to these entities and decide what needs to be saved and what doesn't.  
  The session will do it for you.  
* All these tracked changes are combined & persisted in the database only when calling `saveChanges()`.  
* Entity tracking can be disabled if needed. See:
    * [Disable entity tracking](../../client-api/session/configuration/how-to-disable-tracking)
    * [Clear session](../../client-api/session/how-to/clear-a-session)

---

#### Create document example  
* The Client API, and the session in particular, is designed to be as straightforward as possible.  
  Open the session, do some operations, and apply the changes to the RavenDB server.  
* The following example shows how to create a new document in the database using the session.  

{CODE-TABS}
{CODE-TAB:nodejs:Using-await session_usage_1@ClientApi\Session\whatIsSession.js /}
{CODE-TAB:nodejs:Using-then session_usage_1_then@ClientApi\Session\whatIsSession.js /}
{CODE-TABS/}

#### Modify document example  
* The following example modifies the content of an existing document.

{CODE-TABS}
{CODE-TAB:nodejs:Using-await session_usage_2@ClientApi\Session\whatIsSession.js /}
{CODE-TAB:nodejs:Using-then session_usage_2_then@ClientApi\Session\whatIsSession.js /}
{CODE-TABS/}

{PANEL/}  

{PANEL:Identity map pattern}  

* The session implements the [Identity Map Pattern](https://martinfowler.com/eaaCatalog/identityMap.html).
* The first `load()` call goes to the server and fetches the document from the database.  
  The document is then stored as an entity in the session's entities map.  
* All subsequent `load()` calls to the same document will simply retrieve the entity from the session -  
  no additional calls to the server are made.  

{CODE:nodejs session_usage_3@ClientApi\Session\whatIsSession.js /}  

* Note:  
  To override this behavior and force `load()` to fetch the latest changes from the server see: 
  [Refresh an entity](../../client-api/session/how-to/refresh-entity).  

{PANEL/}

{PANEL: Batching & Transactions}

#### Batching

* Remote calls to a server over the network are among the most expensive operations an application makes.  
  The session optimizes this by batching all __write operations__ it has tracked into the `saveChanges()` call.  
* When calling saveChanges, the session checks its state for all changes made that need to be saved in the database,  
  and combines them into a single batch that is sent to the server as a __single remote call__.

#### Transactions

* The batched operations that are sent in the `saveChanges()` will complete transactionally.  
  In other words, either all changes are saved as a **Single Atomic Transaction** or none of them are.  
  So once saveChanges returns successfully, it is guaranteed that all changes are persisted to the database.  
* The saveChanges is the only time when a RavenDB client sends updates to the server from the session,  
  so you will experience a reduced number of network calls.

#### Transaction mode

* The session's transaction mode can be set to either:
  * __Single-Node__ - transaction is executed on a specific node and then replicated
  * __Cluster-Wide__ - transaction is registered for execution on all nodes in an atomic fashion
* Learn more about these modes in [Cluster-wide vs. Single-node](../../client-api/session/cluster-transaction/overview#cluster-wide-transaction-vs.-single-node-transaction) transactions. 

{PANEL/}

{PANEL: Reducing server calls (best practices) for:}

#### The select N+1 problem
* The Select N+1 problem is common 
  with all ORMs and ORM-like APIs.  
  It results in an excessive number of remote calls to the server, which makes a query very expensive.  
* Make use of RavenDB's `include()` method to include related documents and avoid this issue.  
  See: [Document relationships](../../client-api/how-to/handle-document-relationships)  
<br>
#### Large query results
* When query results are large and you don't want the overhead of keeping all results in memory,  
  then you can [Stream query results](../../client-api/session/querying/how-to-stream-query-results).  
  A single server call is executed and the client can handle the results one by one.  
* [Paging](../../indexes/querying/paging) also avoids getting all query results at one time,  
  however, multiple server calls are generated - one per page retrieved.  
<br>
#### Retrieving results on demand (Lazy)
* Query calls to the server can be delayed and executed on-demand as needed using `Lazily()`
* See [Perform queries lazily](../../client-api/session/querying/how-to-perform-queries-lazily)

{PANEL/}

## Related Articles  

### Client API  

- [Open a Session](../../client-api/session/opening-a-session)
- [Storing Entities](../../client-api/session/storing-entities)
- [Loading Entities](../../client-api/session/loading-entities)
- [Saving Changes](../../client-api/session/saving-changes)
- [Query Overview](../../client-api/session/querying/how-to-query)
- [Querying an Index](../../indexes/querying/query-index)
- [What is a Document Store](../../client-api/what-is-a-document-store)
