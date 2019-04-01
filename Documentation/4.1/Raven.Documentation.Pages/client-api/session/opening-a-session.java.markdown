# Session: Opening a Session

To open session use the `openSession` method from `DocumentStore`.

## Syntax

There are three overloads of `openSession` methods

{CODE:java open_session_1@ClientApi\Session\OpeningSession.java /}

The first method is an equivalent of doing

{CODE:java open_session_2@ClientApi\Session\OpeningSession.java /}

The second method is an equivalent of doing

{CODE:java open_session_3@ClientApi\Session\OpeningSession.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | `OpenSessionOptions` | Options **containing** information such as **name of database** and **RequestExecutor**. |

| Return Value | |
| ------------- | ----- |
| IDocumentSession | Instance of a session object. |

## Options

{CODE:java session_options@ClientApi\Session\OpeningSession.java /}

| Options | | |
| ------------- | ------------- | ----- |
| **database** | String | Name of database that session should operate on. If `null` then [default database set in DocumentStore](../../client-api/setting-up-default-database) is used. |
| **noTracking** | boolean | Indicates if session should **not** keep track of the changes. Default: `false`. More [here](../../client-api/session/configuration/how-to-disable-tracking). |
| **noCaching** | boolean | Indicates if session should **not** cache responses. Default: `false`. More [here](../../client-api/session/configuration/how-to-disable-caching). |
| **requestExecutor** | `RequestExecutor` | _(Advanced)_ Request executor to use. If `null` default one will be used. |
| **transactionMode** | `TransactionMode` | Sets the mode for the session. By default it is set to `SINGLE_NODE`, but session can also operate 'CLUSTER_WIDE'. You can read more about Cluster-Wide Transactions [here](../../server/clustering/cluster-transactions). |


## Example I

{CODE:java open_session_4@ClientApi\Session\OpeningSession.java /}

## Example II - Disabling Entities Tracking

{CODE:java open_session_tracking_1@ClientApi\Session\OpeningSession.java /}

## Remarks

{DANGER:Important}
**Always remember to release session allocated resources after usage by invoking the `close` method or wrapping the session object in the `try` statement.**
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
