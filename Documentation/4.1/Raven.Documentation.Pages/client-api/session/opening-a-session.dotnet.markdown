# Session: Opening a Session

---

{NOTE: }

* Always open a new session object in a `using` statement so that the resources allocated to it can be released when you're done with it. Alternatively, call `.Dispose()` on it.  

* There are two kinds of sessions: **sync** and **async**. Both can be opened from the `DocumentStore`, with `.OpenSession()` and `.OpenAsyncSession()` respectively.  
  * Each of these methods have three overloads that receive different parameters. These parameters alter the `SessionOptions` object.

* In this page:  
  * [Syntax](../../client-api/session/opening-a-session#syntax) - the three overloaded methods for opening sessions.  
  * [SessionOptions](../../client-api/session/opening-a-session#sessionoptions) - an object containing various configurations for the session.  
{NOTE/}

---

{PANEL:Syntax}

There are three overloads of `OpenSession() / OpenAsyncSession()`

{CODE-TABS}
{CODE-TAB:csharp:Sync open_session_1@ClientApi\Session\OpeningSession.cs /}
{CODE-TAB:csharp:Async open_session_1_1@ClientApi\Session\OpeningSession.cs /}
{CODE-TABS/}

The first method is an equivalent of doing

{CODE-TABS}
{CODE-TAB:csharp:Sync open_session_2@ClientApi\Session\OpeningSession.cs /}
{CODE-TAB:csharp:Async open_session_2_1@ClientApi\Session\OpeningSession.cs /}
{CODE-TABS/}

The second method is an equivalent of doing

{CODE-TABS}
{CODE-TAB:csharp:Sync open_session_3@ClientApi\Session\OpeningSession.cs /}
{CODE-TAB:csharp:Async open_session_3_1@ClientApi\Session\OpeningSession.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL:SessionOptions}

{CODE session_options@ClientApi\Session\OpeningSession.cs /}

| Options | | |
| ------------- | ------------- | ----- |
| **Database** | string | Name of database that session should operate on. If `null` then [default database set in DocumentStore](../../client-api/setting-up-default-database) is used. |
| **NoTracking** | bool | Indicates if session should **not** keep track of the changes. Default: `false`. More [here](../../client-api/session/configuration/how-to-disable-tracking). |
| **NoCaching** | bool | Indicates if session should **not** cache responses. Default: `false`. More [here](../../client-api/session/configuration/how-to-disable-caching). |
| **RequestExecutor** | `RequestExecutor` | _(Advanced)_ Request executor to use. If `null` default one will be used. |
| **TransactionMode** | `TransactionMode` | Sets the mode for the session. By default it is set to `SingleNode`, but session can also operate 'ClusterWide'. You can read more about Cluster-Wide Transactions [here](../../server/clustering/cluster-transactions). |

## Example I

{CODE-TABS}
{CODE-TAB:csharp:Sync open_session_4@ClientApi\Session\OpeningSession.cs /}
{CODE-TAB:csharp:Async open_session_5@ClientApi\Session\OpeningSession.cs /}
{CODE-TABS/}

## Example II - Disabling Entities Tracking

{CODE-TABS}
{CODE-TAB:csharp:Sync open_session_tracking_1@ClientApi\Session\OpeningSession.cs /}
{CODE-TAB:csharp:Async open_session_tracking_2@ClientApi\Session\OpeningSession.cs /}
{CODE-TABS/}

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
