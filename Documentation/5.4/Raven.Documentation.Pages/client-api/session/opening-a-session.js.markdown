# Open a Session

---

{NOTE: }

* A session object is obtained from the [document store](../../client-api/what-is-a-document-store).  

* Various session options can be configured using the `SessionOptions` object.  
  If no database is specified in the options then the [default database](../../client-api/setting-up-default-database) is assumed.  

* Most methods on the session object are asynchronous and return a `Promise`.    
  Either use `async & await` or `.then() & callback functions`.  
  Refer to the specific documentation for each method usage.

* In this page:  
  * [Syntax](../../client-api/session/opening-a-session#syntax)  
  * [Session options](../../client-api/session/opening-a-session#session-options)  
  * [Open session example](../../client-api/session/opening-a-session#open-session-example)
{NOTE/}

---

{PANEL:Syntax}

* Use `openSession()` to open a session from the document store.  
* The following overloads are available:

{CODE:nodejs open_session_1@client-api\session\openSession.js /}

| Parameter    | Type             | Description                                                                                                                    |
|--------------|------------------|--------------------------------------------------------------------------------------------------------------------------------|
| **database** | string           | The session will operate on this database,<br>overriding the default database set in the document store.                       |
| **options**  | `SessionOptions` | An object with session configuration options. See details [below](../../client-api/session/opening-a-session#session-options). |

| Return Value        | Description                    |
|---------------------|--------------------------------|
| `IDocumentSession`  | Instance of a session object   |

{PANEL/}

{PANEL:Session options}

* The `SessionOptions` object contains various options to configure the Session's behavior.

| Option              | Type              | Description                                                                                                                                                                                                                               | Default Value                                          |
|---------------------|-------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------|
| **database**        | string            | The session will operate on this database,<br>overriding the Default Database.                                                                                                                                                            | `null` - the session operates on the Default Database |
| **noTracking**      | boolean           | `true` - The session tracks changes made to all entities it loaded, stored, or queried for.<br>`false` - Tracking will be turned off.<br>Learn more in [Disable tracking](../../client-api/session/configuration/how-to-disable-tracking) | `false`                                                |
| **noCaching**       | boolean           | `true` - Server responses will Not be cached.<br>`false` - The session caches the server responses.<br>Learn more in [Disable caching](../../client-api/session/configuration/how-to-disable-caching)                                     | `false`                                                |
| **requestExecutor** | `RequestExecutor` | ( _Advanced option_ ) <br>The request executor the session should use.                                                                                                                                                                    | `null` - the default request executor is used          |
| **transactionMode** | `TransactionMode` | Specify the session's transaction mode<br>`SingleNode` / `ClusterWide`<br>Learn more in [Cluster-wide vs. Single-node](../../client-api/session/cluster-transaction/overview#cluster-wide-transaction-vs.-single-node-transaction)        | `SingleNode`                                           |

* Experts Only:

| Option                                                         | Type     | Description                                                                                                                                                                                                                                             | Default Value |
|----------------------------------------------------------------|----------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------|
| **disableAtomicDocumentWrites-<br>InClusterWideTransaction**   | boolean  | ( _Experts only_ ) <br>`true` - Disable Atomic-Guards in cluster-wide sessions.<br>`false` - Automatic atomic writes in cluster-wide sessions are enabled.<br>Learn more in [Atomic-Guards](../../client-api/session/cluster-transaction/atomic-guards) | `false`       |

{PANEL/}

{PANEL: Open session example}

* The following example opens a **cluster-wide Session**:

{CODE-TABS}
{CODE-TAB:nodejs:Using-await open_session_2@client-api\session\openSession.js /}
{CODE-TAB:nodejs:Using-then open_session_3@client-api\session\openSession.js /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Client API

- [What is a Session and How Does it Work](../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Storing Entities](../../client-api/session/storing-entities)
- [Loading Entities](../../client-api/session/loading-entities)
- [Saving Changes](../../client-api/session/saving-changes)
- [What is a Document Store](../../client-api/what-is-a-document-store)

### Querying

- [Query Overview](../../client-api/session/querying/how-to-query)


