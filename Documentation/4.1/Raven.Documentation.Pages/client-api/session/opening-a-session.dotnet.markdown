# Session: Opening a Session

---

{NOTE: }

* Always open a new session object in a `using` statement so that the resources allocated to it can be released when you're done with it. Alternatively, call `.Dispose()` on it. (The session implements [IDisposable](https://docs.microsoft.com/en-us/dotnet/api/system.idisposable?view=netframework-4.7.2))  

* There are two kinds of session: **sync** and **async**. Both can be opened from the `DocumentStore`, with `.OpenSession()` and `.OpenAsyncSession()` respectively.  

* `OpenSession/OpenAsyncSession` have three overloaded versions that receive different parameters. These parameters alter the **session options**.

* In this page:  
  * [Syntax](../../client-api/session/opening-a-session#syntax)  
  * [SessionOptions](../../client-api/session/opening-a-session#sessionoptions)  
{NOTE/}

---

{PANEL:Syntax}

There are three overloaded versions of `OpenSession() / OpenAsyncSession()`:

{CODE-TABS}
{CODE-TAB:csharp:Sync open_session_1@ClientApi\Session\OpeningSession.cs /}
{CODE-TAB:csharp:Async open_session_1_1@ClientApi\Session\OpeningSession.cs /}
{CODE-TABS/}

The first two overloaded methods simply wrap the third overloaded method and pass it a `new SessionOptions()` object.

{CODE-TABS}
{CODE-TAB:csharp:Sync open_session_2@ClientApi\Session\OpeningSession.cs /}
{CODE-TAB:csharp:Async open_session_2_1@ClientApi\Session\OpeningSession.cs /}
{CODE-TABS/}
{PANEL/}

{PANEL:SessionOptions}

The `SessionOptions` object contains the following properties which configure the session's behavior:  

| Option | Type | Description | Default Value |
| --- | --- | --- | --- |
| **Database** | string | The name of the database the session should operate on | `null` - the session operates on the [default database](../../client-api/setting-up-default-database) |
| **NoTracking** | bool | Whether the session should keep track of changes to `stored` or `loaded` entities. [How to Disable Tracking](../../client-api/session/configuration/how-to-disable-tracking) | `false` |
| **NoCaching** | bool | Whether the session should cache responses. [How to Disable Caching](../../client-api/session/configuration/how-to-disable-caching) | `false` |
| **RequestExecutor** | `RequestExecutor` | _(Advanced)_ Which request executor the session should use | `null` - the default request executor is used |
| **TransactionMode** | `TransactionMode` | The two transaction modes are `SingleNode` and `ClusterWide`. [Cluster-Wide Transactions](../../server/clustering/cluster-transactions) | `SingleNode` |

{PANEL/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Storing Entities](../../client-api/session/storing-entities)
- [Loading Entities](../../client-api/session/loading-entities)
- [Saving Changes](../../client-api/session/saving-changes)

### Querying

- [Basics](../../indexes/querying/basics)

### Document Store

- [What is a Document Store](../../client-api/what-is-a-document-store)
