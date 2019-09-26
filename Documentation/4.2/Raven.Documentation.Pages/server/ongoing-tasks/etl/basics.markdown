# Ongoing Tasks: ETL Basics
---

{NOTE: }

* **ETL (Extract, Transform & Load)** is a three-stage RavenDB process that transfers data from a RavenDB database to an external target. 
  The data can be filtered and transformed along the way.  

* The external target can be:  
  * Another RavenDB database instance (outside of the [Database Group](../../../studio/database/settings/manage-database-group))
  * A relational database  

* In this page:  
  * [Why use ETL](../../../server/ongoing-tasks/etl/basics#why-use-etl)  
  * [Defining ETL Tasks](../../../server/ongoing-tasks/etl/basics#defining-etl-tasks)  
  * [ETL Stages:](../../../server/ongoing-tasks/etl/basics#etl-stages)  
      * [Extract](../../../server/ongoing-tasks/etl/basics#extract)  
      * [Transform](../../../server/ongoing-tasks/etl/basics#transform)  
      * [Load](../../../server/ongoing-tasks/etl/basics#load)  
  * [Troubleshooting](../../../server/ongoing-tasks/etl/basics#troubleshooting)  
{NOTE/}

---

{PANEL: Why use ETL}

* **Share relevant data**  
  Data that needs to be shared can be sent in a well-defined format matching your requirements so that only relevant data is sent.  

* **Protect your data - Share partial data**  
  Limit access to sensitive data. Details that should remain private can be filtered out as you can share partial data.  

* **Reduce system calls**  
  Distribute data to related services in your system architecture so that they have their _own copy_ of the data and can access it without making a cross-service call.  
  i.e. A product catalog can be shared among multiple stores where each can modify the products or add new ones.  

* **Transform the data**  
  * Modify content sent as needed with JavaScript code.  
  * Multiple documents can be sent from a single source document.  
  * Data can be transformed to match a _rational model_ used in the target destination.  

* **Aggregate your data**  
  Data sent from multiple locations can be aggregated in a central server.  

  For example:

  *  Send data to an already existing reporting solution.  
  *  Point of sales systems can send sales data to a central place for calculations.  
{PANEL/}

{PANEL: Defining ETL Tasks}

* The following two ETL tasks can be defined:  
  * [RavenDB ETL](../../../server/ongoing-tasks/etl/raven) - send data to another _RavenDB database_  
  * [SQL ETL](../../../server/ongoing-tasks/etl/sql) - send data to a _SQL database_  

* The destination URL address is set by using a pre-defined named _connection string_.  
  This makes deployment between environments easier.  
  For RavenDB ETL, multiple URLs can be configured in the connection string as the target database can reside on multiple nodes within the Database Group in the destination cluster. 
  If one of the destination nodes is down, RavenDB will run the ETL process against another node in the Database Group topology.  
  See more in [Connection Strings](../../../todo-update-me-later)

* The tasks can be defined from code or from the [Studio](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)  
{PANEL/}

{PANEL: ETL Stages}

ETL's three stages are:  

* [Extract](../../../server/ongoing-tasks/etl/basics#extract) - Extract the documents from the database  
* [Transform](../../../server/ongoing-tasks/etl/basics#transform) - Transform & filter the documents data according to the supplied script (optional)  
* [Load](../../../server/ongoing-tasks/etl/basics#load) - Load (write) the transformed data into the target destination  

### Extract

The ETL process starts with retrieving the documents from the database.  
You can choose which documents will be processed by next two stages (Transform and Load).  

The possible options are:  

* Documents from a single collection  
* Documents from multiple collections  
* All documents (RavenDB ETL only)  

### Transform

This stage transforms and filters the extracted documents according to a provided script.  
Any transformation can be done so that only relevant data is shared.  
The script is written in JavaScript and its input is a document.  

In addition to the ECMAScript 5.1 API, RavenDB introduces the following functions and members:  

| ------ | ------ | ------ |
| `this` | object | The current document (with metadata) |
| `id(document)` | function | Returns the document ID |
| `load(id)` | function | Load another document.<br/>This will increase the maximum number of allowed steps in a script.<br/>**Note**: Changes made to the other _loaded_ document will _not_ trigger the ETL process.|

Specific ETL functions:  

| ------ |:------:| ------ |
| `loadTo<Target>(obj)` | function | Load an object to a specified `<Target>`.<br/>The target must be either a collection name (RavenDB ETL) or a table name (SQL ETL).<br/>**An object will be sent to the destination only if the `loadTo` method was called**.|
| Attachments: |||
| `loadAttachment(name)` | function | Load an attachment of the current document |
| `hasAttachment(name)` | function | Check if an attachment with a given name exists for the current document |
| `getAttachments()` | function | Get a collection of attachments details for the current document. Each item has the following properties `Name`, `Hash`, `ContentType`, `Size` |
| `<doc>.addAttachment([name,] attachmentRef)` | function | Add an attachment to a transformed document that will be sent to a target (`<doc>`). Specific for Raven ETL only, see [here](../../../server/ongoing-tasks/etl/raven#attachments) for details |

{INFO: Batch processing}

Documents are extracted and transformed by the ETL process in a batch manner.  
The number of documents processed depends on the following configuration limits:  

* [`ETL.ExtractAndTransformTimeoutInSec`](../../../server/configuration/etl-configuration#etl.extractandtransformtimeoutinsec) (default: 30 sec)  
  Time-frame for the extraction and transformation stages (in seconds), after which the loading stage will start.  

* [`ETL.MaxNumberOfExtractedDocuments`](../../../server/configuration/etl-configuration#etl.maxnumberofextracteddocuments) (default: 8192)  
  Maximum number of extracted documents in an ETL batch.  

* [`ETL.MaxNumberOfExtractedItems`](../../../server/configuration/etl-configuration#etl.maxnumberofextracteditems) (default: 8192)  
  Maximum number of extracted items (documents, counters) in an ETL batch.

* [`ETL.MaxBatchSizeInMb`](../../../server/configuration/etl-configuration#etl.maxbatchsizeinmb) (default: 64 MB)  
  Maximum size of an ETL batch in MB.

{INFO/}

### Load

 Loading the results to the target destination is the last stage.

 Updates are implemented by executing consecutive DELETEs and INSERTs.
 When a document is modified, the delete command is sent before the new data is inserted and both are processed under the same transaction on the destination side.
 This applies to both ETL types.  

 There are two exceptions to this behavior:  

 * In RavenDB ETL, when documents are loaded to **the same** collection there is no need to send DELETE because the document on the other side has the same identifier and will just update it.  
 * in SQL ETL you can configure to use inserts only, which is a viable option for append-only systems.  

{NOTE:Note}
In contrast to Replication, ETL is a push-only process that _writes_ data to the destination whenever documents from the relevant collections were changed. Existing entries on the target will always be _overwritten_.  
{NOTE/}

{NOTE:Loading data from encrypted database}

If a database is encrypted then you must not send data in ETL process using a non encrypted channel by default. It means that a connection to a target must be secured:

- In Raven ETL, a URL of a destination server has to use HTTPS (a server certificate of the source server needs to be registered as a client certificate on the destination server)
- in SQL ETL, a connection string to an SQL database must specify encrypted connection (specific per SQL engine provided)

This validation can be turned off by selecting the _Allow ETL on a non-encrypted communication channel_ option in the Studio (or setting `AllowEtlOnNonEncryptedChannel` if a task is defined using the client API).
Please note that your data encrypted at rest _won't_ be protected in transit then.

{NOTE/}

{PANEL/}

{PANEL: Troubleshooting}

ETL errors and warnings are [logged to the files](../../../server/troubleshooting/logging) and displayed in the notification center panel. You will be notified if any
of the following events happen:

- connection error to the target
- JS script is invalid
- transformation error
- load error
- slow SQL was detected


### Fallback Mode

If the ETL cannot proceed the load stage (e.g. it can't connect to the destination) then it enters the fallback mode.
The fallback mode means suspending the process and retrying it periodically. The fallback time starts from 5 seconds and
it's doubled on every consecutive error according to the time passed since the last error, but it never crosses
[`ETL.MaxFallbackTimeInSec`](../../../server/configuration/etl-configuration#etl.maxfallbacktimeinsec) configuration (default: 900 sec)  

 Once the process is in the fallback mode, then the _Reconnect_ state is shown in the Studio.

{PANEL/}

{NOTE: }

Details and examples for type specific ETL scripts can be found in the following articles:  

* [RavenDB ETL](../../../server/ongoing-tasks/etl/raven)  
* [SQL ETL](../../../server/ongoing-tasks/etl/sql)  
{NOTE/}

## Related Articles

### ETL

- [RavenDB ETL Task](../../../server/ongoing-tasks/etl/raven)
- [SQL ETL Task](../../../server/ongoing-tasks/etl/sql)

### Studio

- [Define RavenDB ETL Task in Studio](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)
- [Define SQL ETL Task in Studio](../../../todo-update-me-later)

