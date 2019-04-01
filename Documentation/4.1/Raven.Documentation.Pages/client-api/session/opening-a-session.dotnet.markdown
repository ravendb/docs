# Session: Opening a Session

To open a synchronous session, use the `OpenSession` method from `DocumentStore`. If you prefer working in an asynchronous manner, use `OpenAsyncSession`.

## Syntax

There are three overloads of `OpenSession / OpenAsyncSession` methods

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

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | `OpenSessionOptions` | Options **containing** information such as **name of database** and **RequestExecutor**. |

| Return Value | |
| ------------- | ----- |
| IDocumentSession / IAsyncDocumentSession | Instance of a session object. |

## Options

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

## Remarks

{DANGER:Important}
**Always remember to release session allocated resources after usage by invoking the `Dispose` method or wrapping the session object in the `using` statement.**
{DANGER/}

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
