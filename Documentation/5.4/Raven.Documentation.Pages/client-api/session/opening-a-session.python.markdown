# Open a Session

---

{NOTE: }

* A Session object is obtained from the [Document Store](../../client-api/what-is-a-document-store).  

* Open a session using `open_session()`.  

* Various Session options can be configured using the `SessionOptions` object.  
  If no database is specified in the options then the [Default Database](../../client-api/setting-up-default-database) (stored in the Document Store) is assumed.  

* Be sure to wrap the session variable using a 'with' statement to ensure proper disposal.

* In this page:  
  * [Syntax](../../client-api/session/opening-a-session#syntax)  
  * [Session options](../../client-api/session/opening-a-session#session-options)  
  * [Open session example](../../client-api/session/opening-a-session#open-session-example)
{NOTE/}

---

{PANEL:Syntax}

Use `open_session()` to open a session from the Document Store.  

{CODE:python open_session_1@ClientApi\session\OpenSession.py /}

| Parameter    | Type             | Description                                                                                                                    |
|--------------|------------------|--------------------------------------------------------------------------------------------------------------------------------|
| **database** | `str`              | The session will operate on this database,<br>overriding the default database set in the document store.                       |
| **options**  | `SessionOptions` | An object with Session configuration options. See details [below](../../client-api/session/opening-a-session#session-options). |

| Return Value                                 | Description                   |
|----------------------------------------------|-------------------------------|
| `IDocumentSession` | Instance of a Session object  |

{PANEL/}

{PANEL:Session options}

* The `SessionOptions` object contains various options to configure the Session's behavior.

| Option                                                  | Type               | Description                                                                                                                                                                                                                               | Default Value                                         |
|---------------------------------------------------------|------------------- |-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------|
| **database**                                            | `str`                | The Session will operate on this database,<br>overriding the Default Database.                                                                                                                                                            | `None` - the Session operates on the Default Database |
| **no_tracking**                                         | `bool`               | `True` - The Session tracks changes made to all entities it loaded, stored, or queried for.<br>`False` - Tracking will be turned off.<br>Learn more in [Disable tracking](../../client-api/session/configuration/how-to-disable-tracking) | `False`                                               |
| **no_caching**                                          | `bool`               | `True` - Server responses will not be cached.<br>`False` - The Session caches the server responses.<br>Learn more in [Disable caching](../../client-api/session/configuration/how-to-disable-caching)                                     | `False`                                               |
| **request_executor**                                    | `RequestExecutor`  | ( _Advanced option_ ) <br>The request executor the Session should use.                                                                                                                                                                    | `None` - the default request executor is used         |
| **transaction_mode**                                    | `TransactionMode`  | Specify the Session's transaction mode<br>`SINGLE_NODE` / `CLUSTER_WIDE`<br>Learn more in [Cluster-wide vs. Single-node](../../client-api/session/cluster-transaction/overview#cluster-wide-transaction-vs.-single-node-transaction)      | `SINGLE_NODE`                                         |

* Experts Only:

| Option                                                             | Type                | Description                                                                                                                                                                                                                                             | Default Value |
|--------------------------------------------------------------------|---------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------|
| **disable_atomic_document_writes_in_cluster_wide_transaction**     | `bool`                | **Experts only**<br>`True` - Disable Atomic-Guards in cluster-wide sessions.<br>`False` - Automatic atomic writes in cluster-wide sessions are enabled.<br>Learn more in [Atomic-Guards](../../client-api/session/cluster-transaction/atomic-guards) | `False`       |

{PANEL/}

{PANEL:Open session example}

* The following example opens a **cluster-wide Session**:

{CODE:python open_session_2@ClientApi\session\OpenSession.py /}

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
