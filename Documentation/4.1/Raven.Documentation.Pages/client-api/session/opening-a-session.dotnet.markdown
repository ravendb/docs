# Session: Opening a Session

---

{NOTE: }

* A Session object is obtained from the [Document Store](../../client-api/what-is-a-document-store).  

* A Session can operate Synchronously or Asynchronously.  
  * `OpenSession()` - Open a Session for a **Synchrounous** mode of operation.  
  * `OpenAsyncSession()` - Open a Session for **Asychrounous** mode of operation.  

* Various Session options can be configured using the `SessionOptions` object.  

* Be sure to wrap the Session variable with a 'using' statement to ensure proper disposal.  

* If no database is specified then the Default Database (stored in the Document Store) is assumed.  

* In this page:  
  * [Syntax](../../client-api/session/opening-a-session#syntax)  
  * [Session Options](../../client-api/session/opening-a-session#session-options)  
  * [Example](../../client-api/session/opening-a-session#example)
{NOTE/}

---

{PANEL:Syntax}

`OpenSession()` / `OpenAsyncSession()` have three overloads:  

{CODE-TABS}
{CODE-TAB:csharp:Sync open_session_1@ClientApi\session\OpeningSession.cs /}
{CODE-TAB:csharp:Async open_session_1_1@ClientApi\session\OpeningSession.cs /}
{CODE-TABS/}

| Parameter | Type | Description |
|-|-|-|
| **database** | `string` | The Session will operate on this database, overriding the default database set in the document store |
| **options** | `SessionOptions` | An object with Session configuration options. See details [below](../../client-api/session/opening-a-session#session-options) |

| Return Value | Description |
|-|-|
| `IDocumentSession` / `IAsyncDocumentSession` | Instance of a Session object |

{PANEL/}

{PANEL:Session Options}

The `SessionOptions` object contains various options to configure the Session's behavior.

| Option | Type | Description | Default Value |
| --- | --- | --- | --- |
| **Database** | string | The Session will operate on this database, overriding the Default Database. | `null` - the Session operates on the [Default Database](../../client-api/setting-up-default-database) |
| **NoTracking** | boolean | Whether the Session should track changes made to all entities that it has either loaded, stored, or queried on. <br>See [Disable Tracking Example](../../client-api/session/configuration/how-to-disable-tracking) | `false` |
| **NoCaching** | boolean | Whether the Session should cache the server responses. <br>See [Disable Caching Example](../../client-api/session/configuration/how-to-disable-caching) | `false` |
| **RequestExecutor** | `RequestExecutor` | _(Advanced)_ The request executor the Session should use | `null` - the default request executor is used |
| **TransactionMode** | `TransactionMode` | Specify the Session's transaction mode (`SingleNode` / `ClusterWide`). <br>See [Cluster-Wide Transactions](../../server/clustering/cluster-transactions) | `SingleNode` |

{PANEL/}

{PANEL:Example}

Here is an example of opening a Session using a `SessionOptions` object:  

{CODE-TABS}
{CODE-TAB:csharp:Sync open_session_3@ClientApi\session\OpeningSession.cs /}
{CODE-TAB:csharp:Async open_session_3_1@ClientApi\session\OpeningSession.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Client API

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Storing Entities](../../client-api/session/storing-entities)
- [Loading Entities](../../client-api/session/loading-entities)
- [Saving Changes](../../client-api/session/saving-changes)
- [What is a Document Store](../../client-api/what-is-a-document-store)

### Indexes

- [Querying Basics](../../indexes/querying/basics)


