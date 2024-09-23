# Open a Session

---

{NOTE: }

* A Session object is obtained from the [Document Store](../../client-api/what-is-a-document-store).  

* Open a session using `openSession()`.  

* Various Session options can be configured using the `sessionOptions` object.  
  If no database is specified in the options then the [Default Database](../../client-api/setting-up-default-database) (stored in the Document Store) is assumed.

* Make sure you release allocated session resources using the `close` method or wrapping 
  the session object in a `try` statement ([see below](../../client-api/session/opening-a-session#example-3---releasing-session-resources)).  

* In this page:  
  * [Syntax](../../client-api/session/opening-a-session#syntax)  
  * [Session options](../../client-api/session/opening-a-session#session-options)  
  * [Open session examples](../../client-api/session/opening-a-session#open-session-examples)
{NOTE/}

---

{PANEL:Syntax}

* Use `openSession()` to open a session from the Document Store.  
* The following overloads are available:  

{CODE:php open_session_1@ClientApi\session\OpenSession.php /}

| Parameter    | Type             | Description                                                                                                                    |
|--------------|------------------|--------------------------------------------------------------------------------------------------------------------------------|
| **database** | `string`         | The Session will operate on this database,<br>overriding the default database set in the document store.                       |
| **options**  | `sessionOptions` | An object with Session configuration options. See details [below](../../client-api/session/opening-a-session#session-options). |

| Return Value               | Description                |
|----------------------------|----------------------------|
| `DocumentSessionInterface` | Document session interface |

{PANEL/}

{PANEL:Session options}

* The `sessionOptions` object contains various options to configure the Session's behavior.

| Option                | Type              | Description                                                                                                                                                                                                                               | Default Value                                         |
|-----------------------|-------------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------|
| **Database**          | `string`          | The Session will operate on this database,<br>overriding the Default Database.                                                                                                                                                            | `null` - the Session operates on the Default Database |
| **NoTracking**        | `bool`            | `true` - The Session tracks changes made to all entities it loaded, stored, or queried for.<br>`false` - Tracking will be turned off.<br>Learn more in [Disable tracking](../../client-api/session/configuration/how-to-disable-tracking) | `false`                                               |
| **NoCaching**         | `bool`            | `true` - Server responses will Not be cached.<br>`false` - The Session caches the server responses.<br>Learn more in [Disable caching](../../client-api/session/configuration/how-to-disable-caching)                                     | `false`                                               |
| **RequestExecutor**   | `RequestExecutor` | ( _Advanced option_ ) <br>The request executor the Session should use.                                                                                                                                                                    | `null` - the default request executor is used         |
| **TransactionMode**   | `TransactionMode` | Specify the Session's transaction mode<br>`SingleNode` / `ClusterWide`<br>Learn more in [Cluster-wide vs. Single-node](../../client-api/session/cluster-transaction/overview#cluster-wide-transaction-vs.-single-node-transaction)        | `SingleNode`                                          |

* Experts Only:

| Option                                                       | Type                | Description                                                                                                                                                                                                                                             | Default Value |
|--------------------------------------------------------------|---------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------|
| **DisableAtomicDocumentWrites-<br>InClusterWideTransaction** | `bool?`               | ( _Experts only_ ) <br>`true` - Disable Atomic-Guards in cluster-wide sessions.<br>`false` - Automatic atomic writes in cluster-wide sessions are enabled.<br>Learn more in [Atomic-Guards](../../client-api/session/cluster-transaction/atomic-guards) | `false`       |

{PANEL/}

{PANEL:Open session examples}

#### Example 1
{CODE:php open_session_2@ClientApi\session\OpenSession.php /}

#### Example 2
{CODE:php open_session_3@ClientApi\session\OpenSession.php /}

#### Example 3 - Releasing session resources
{NOTE: }
Always remember to release allocated session resources after usage by 
invoking the `close` method or wrapping the session object in a `try` statement.
{NOTE/}
{CODE:php open_session_4@ClientApi\session\OpenSession.php /}

#### Example 4 - Disable entities tracking
{CODE:php open_session_no_tracking@ClientApi\session\OpenSession.php /}

#### Example 5 - Disable session caching
{CODE:php open_session_no_caching@ClientApi\session\OpenSession.php /}

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


