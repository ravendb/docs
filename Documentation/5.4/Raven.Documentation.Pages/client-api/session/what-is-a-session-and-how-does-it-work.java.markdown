# What is a Session and How Does it Work

---

{NOTE: }

* The **Session**, which is obtained from the [Document Store](../../client-api/what-is-a-document-store),  
  is the primary interface your application will interact with.

* In this page:
    * [Session overview](../../client-api/session/what-is-a-session-and-how-does-it-work#session-overview)
    * [Unit of work pattern](../../client-api/session/what-is-a-session-and-how-does-it-work#unit-of-work-pattern)
        * [Tracking changes](../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes)
        * [Create document example](../../client-api/session/what-is-a-session-and-how-does-it-work#create-document-example)
        * [Modify document example](../../client-api/session/what-is-a-session-and-how-does-it-work#modify-document-example)
    * [Identity map pattern](../../client-api/session/what-is-a-session-and-how-does-it-work#identity-map-pattern)
    * [Batching & Transactions](../../client-api/session/what-is-a-session-and-how-does-it-work#batching-&-transactions)
    * [Concurrency control](../../client-api/session/what-is-a-session-and-how-does-it-work#concurrency-control)<br><br>
    * [Reducing server calls (best practices) for:](../../client-api/session/what-is-a-session-and-how-does-it-work#reducing-server-calls-(best-practices)-for:)
        * [The N+1 problem](../../client-api/session/what-is-a-session-and-how-does-it-work#the-select-n1-problem)
        * [Large query results](../../client-api/session/what-is-a-session-and-how-does-it-work#large-query-results)
        * [Retrieving results on demand (Lazy)](../../client-api/session/what-is-a-session-and-how-does-it-work#retrieving-results-on-demand-lazy)

{NOTE/}

---

{PANEL: Session overview}

* **What is the session**:  

    * The session (`IDocumentSession`/`IAsyncDocumentSession`) serves as a [Unit of Work](https://en.wikipedia.org/wiki/Unit_of_work) representing a single  
      **[Business Transaction](https://martinfowler.com/eaaCatalog/unitOfWork.html)** on a specific database (not to be confused with an [ACID transaction](../../client-api/faq/transaction-support)).

    * It is a container that allows you to query for documents and load, create, or update entities  
      while keeping track of changes.

    * Basic document CRUD actions and document Queries are available through the `session`.  
      More advanced options are available using the `advanced` Session operations.

* **Batching modifications**:  
  A business transaction usually involves multiple requests such as loading of documents or execution of queries.  
  Calling [saveChanges()](../../client-api/session/saving-changes) indicates the completion of the client-side business logic .
  At this point, all modifications made within the session are batched and sent together in a **single HTTP request** to the server to be persisted as a single ACID transaction.

* **Tracking changes**:  
  Based on the [Unit of Work](https://martinfowler.com/eaaCatalog/unitOfWork.html) and the [Identity Map](https://martinfowler.com/eaaCatalog/identityMap.html) patterns,
  the session tracks all changes made to all entities that it has either loaded, stored, or queried for.  
  Only the modifications are sent to the server when _saveChanges()_ is called.

* **Client side object**:  
  The session is a pure client side object. Opening the session does Not establish any connection to a database,  
  and the session's state isn't reflected on the server side during its duration.

* **Configurability**:  
  Various aspects of the session are configurable.  
  For example, the number of server requests allowed per session is [configurable](../../client-api/session/configuration/how-to-change-maximum-number-of-requests-per-session) (default is 30).

* **The session and ORM Comparison**:  
  The RavenDB Client API is a native way to interact with a RavenDB database.  
  It is _not_ an Object–relational mapping (ORM) tool. Although if you're familiar with NHibernate of Entity Framework ORMs you'll recognize that
  the session is equivalent of NHibernate's session and Entity Framework's DataContext which implement UoW pattern as well.

{PANEL/}

{PANEL: Unit of work pattern}

#### Tracking changes

* Using the Session, perform needed operations on your documents.  
  e.g. create a new document, modify an existing document, query for documents, etc.
* Any such operation '*loads*' the document as an entity to the Session,  
  and the entity is added to the **Session's entities map**.
* The Session **tracks all changes** made to all entities stored in its internal map.  
  You don't need to manually track the changes and decide what needs to be saved and what doesn't, the Session will do it for you.  
  Prior to saving, you can review the changes made if necessary. See: [Check for session changes](../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session).
* All the tracked changes are combined & persisted in the database only when calling `saveChanges()`.
* Entity tracking can be disabled if needed. See:
    * [Disable entity tracking](../../client-api/session/configuration/how-to-disable-tracking)
    * [Clear session](../../client-api/session/how-to/clear-a-session)

---

#### Create document example
* The Client API, and the Session in particular, is designed to be as straightforward as possible.  
  Open the session, do some operations, and apply the changes to the RavenDB server.
* The following example shows how to create a new document in the database using the Session.

{CODE:java session_usage_1@ClientApi\Session\WhatIsSession.java /}

#### Modify document example
* The following example modifies the content of an existing document.

{CODE:java session_usage_2@ClientApi\Session\WhatIsSession.java /}

{PANEL/}

{PANEL: Identity map pattern}

* The session implements the [Identity Map Pattern](https://martinfowler.com/eaaCatalog/identityMap.html).
* The first `load()` call goes to the server and fetches the document from the database.  
  The document is then stored as an entity in the Session's entities map.
* All subsequent `load()` calls to the same document will simply retrieve the entity from the Session -  
  no additional calls to the server are made.

{CODE:java session_usage_3@ClientApi\Session\WhatIsSession.java /}

* Note:  
  To override this behavior and force `load()` to fetch the latest changes from the server see:
  [Refresh an entity](../../client-api/session/how-to/refresh-entity).

{PANEL/}

{PANEL: Batching & Transactions}

{NOTE: }

#### Batching

* Remote calls to a server over the network are among the most expensive operations an application makes.  
  The session optimizes this by batching all **write operations** it has tracked into the `saveChanges()` call.
* When calling _saveChanges_, the session evaluates its state to identify all pending changes requiring persistence in the database.
  These changes are then combined into a single batch that is sent to the server in a **single remote call** and executed as a single ACID transaction.  

{NOTE/}
{NOTE: }

#### Transactions

* The client API does not provide transactional semantics over the entire session.  
  The session **does not** represent a [transaction](../../client-api/faq/transaction-support) (nor a transaction scope) in terms of ACID transactions.
* RavenDB provides transactions over individual requests, so each call made within the session's usage will be processed in a separate transaction on the server side.
  This applies to both reads and writes.

##### Read transactions

* Each call retrieving data from the database will generate a separate request. Multiple requests mean separate transactions.
* The following options allow you to read _multiple_ documents in a single request:
    * Using overloads of the [load()](../../client-api/session/loading-entities#load---multiple-entities) method that specify a collection of IDs or a prefix of ID.
    * Using [include](../../client-api/session/loading-entities#load-with-includes) to retrieve additional documents in a single request.
    * A query that can return multiple documents is executed in a single request,  
      hence it is processed in a single read transaction.

##### Write transactions

* The batched operations that are sent in the `saveChanges()` complete transactionally, as this call generates a single request to the database.
  In other words, either all changes are saved as a **Single Atomic Transaction** or none of them are.  
  So once _saveChanges_ returns successfully, it is guaranteed that all changes are persisted to the database.
* _saveChanges_ is the only time when the RavenDB Client API sends updates to the server from the Session,  
  resulting in a reduced number of network calls.
* To execute an operation that both loads and updates a document within the same write transaction, use the patching feature.
  This can be done either with the usage of a [JavaScript patch](../../client-api/operations/patching/single-document) syntax or [JSON Patch](../../client-api/operations/patching/json-patch-syntax) syntax.

{NOTE/}
{NOTE: }

#### Transaction mode

* The session's transaction mode can be set to either:
    * **Single-Node** - transaction is executed on a specific node and then replicated
    * **Cluster-Wide** - transaction is registered for execution on all nodes in an atomic fashion
    * {WARNING: }
      The phrase "session's transaction mode" refers to the type of transaction that will be executed on the server-side when `saveChanges()` is called.
      As mentioned earlier, the session itself does not represent an ACID transaction.
      {WARNING/}
    * Learn more about these modes in [Cluster-wide vs. Single-node](../../client-api/session/cluster-transaction/overview#cluster-wide-transaction-vs.-single-node-transaction) transactions.

{NOTE/}

{INFO: Transactions in RavenDB}
For a detailed description of transactions in RavenDB please refer to the [Transaction support in RavenDB](../../client-api/faq/transaction-support) article.
{INFO/}

{PANEL/}

{PANEL: Concurrency control}

The typical usage model of the session is:

  * Load documents
  * Modify the documents
  * Save changes

For example, a real case scenario would be:

  * Load an entity from a database.
  * Display an Edit form on the screen.
  * Update the entity after the user completes editing.

When using the session, the interaction with the database is divided into two parts - the load part and save changes part.
Each of these parts is executed separately, via its own HTTP request.  
Consequently, data that was loaded and edited could potentially be changed by another user in the meantime.  
To address this, the session API offers the concurrency control feature.

#### Default strategy on single node

* By default, concurrency checks are turned off.
  This means that with the default configuration of the session, concurrent changes to the same document will use the Last Write Wins strategy.

* The second write of an updated document will override the previous version, causing potential data loss.
  This behavior should be considered when using the session with single node transaction mode.

#### Optimistic concurrency on single node

* The modification or editing stage can extend over a considerable time period or may occur offline.  
  To prevent conflicting writes, where a document is modified while it is being edited by another user or client,  
  the session can be configured to employ [optimistic concurrency](../../client-api/session/configuration/how-to-enable-optimistic-concurrency).

* Once optimistic concurrency is enabled, the session performs version tracking to ensure that any document modified within the session has not been altered in the database since it was loaded into the session.  
  The version is tracked using a [change vector](../../server/clustering/replication/change-vector).

* When `saveChanges()` is called, the session additionally transmits the version of the modified documents to the database, allowing it to verify if any changes have occurred in the meantime.  
  If modifications are detected, the transaction will be aborted with a `ConcurrencyException`,  
  providing the caller with an opportunity to retry or handle the error as needed.

#### Concurrency control in cluster-wide transactions

* In a cluster-wide transaction scenario, RavenDB server tracks a cluster-wide version for each modified document, updating it through the Raft protocol.
  This means that when using a session with the cluster-wide transaction mode, a `ConcurrencyException` will be triggered upon calling `saveChanges()`
  if another user has modified a document and saved it in a separate cluster-wide transaction in the meantime.

* More information about cluster transactions can be found in [Cluster Transaction - Overview](../../client-api/session/cluster-transaction/overview).

{PANEL/}

{PANEL: Reducing server calls (best practices) for:}

#### The select N+1 problem
* The Select N+1 problem is common
  with all ORMs and ORM-like APIs.  
  It results in an excessive number of remote calls to the server, which makes a query very expensive.
* Make use of RavenDB's `include()` method to include related documents and avoid this issue.  
  See: [Document relationships](../../client-api/how-to/handle-document-relationships)

#### Large query results
* When query results are large and you don't want the overhead of keeping all results in memory,  
  then you can [Stream query results](../../client-api/session/querying/how-to-stream-query-results).  
  A single server call is executed and the client can handle the results one by one.
* [Paging](../../indexes/querying/paging) also avoids getting all query results at one time,  
  however, multiple server calls are generated - one per page retrieved.

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
- [Optimistic Concurrency](../../client-api/session/configuration/how-to-enable-optimistic-concurrency)
- [Transaction Support](../../client-api/faq/transaction-support)
