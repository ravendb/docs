# Ongoing Tasks: ETL Basics
---

{NOTE: }

* **ETL (Extract, Transform & Load)** is a three-stage RavenDB process that transfers data from a RavenDB database to an external target. 
  The data can be filtered and transformed along the way.  

* The external target can be:  
  * Another RavenDB database instance (outside of the [Database Group](../../../studio/database/settings/manage-database-group)) -or-  
  * A relational database  

* In this page:  
  * [Why use ETL](../../../server/ongoing-tasks/etl/basics#why-use-etl)  
  * [Defining ETL Tasks](../../../server/ongoing-tasks/etl/basics#defining-etl-tasks)  
  * [ETL Stages:](../../../server/ongoing-tasks/etl/basics#etl-stages)  
      * [Extract](../../../server/ongoing-tasks/etl/basics#extract)  
      * [Transform](../../../server/ongoing-tasks/etl/basics#transform)  
      * [Load](../../../server/ongoing-tasks/etl/basics#load)  
{NOTE/}

---

{PANEL: Why use ETL}

* **Share relevant data**  
  Data that needs to be shared can be sent in a well-defined format matching your requirements so that only relevant data is sent.  

* **Protect your data - Share partial data**  
  Limit access to sensitive data, details that should remain private can be filtered out as you can sare partial data.  

* **Reduce system calls**  
  Distribute data to related services in your system architecture so that they have their _own copy_ of the data and can access it without making a cross-service call.  
  i.e. A product catalog can be shared among multiple stores where each can modify the products or add new ones.  

* **Transform the data**  
  * Modify content sent as needed with a javascript script.  
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
  Thus, if one of the destination nodes is down, RavenDB will run the ETL process against another node in the Database Group topology.  
  See more in [Connection Strings](../../../todo-update-me-later)

* The tasks can be defined from code or from the [Studio](../../../studio/database/tasks/ongoing-tasks/ravendb-etl-task)  
{PANEL/}

{PANEL: ETL Stages}

ETL's three stages are:  

* [Extract](../../../server/ongoing-tasks/etl/basics#extract) - Extract the documents from the database  
* [Transform](../../../server/ongoing-tasks/etl/basics#transform) - Transform & filter the documents data according to the supplied script (optional)  
* [Load](../../../server/ongoing-tasks/etl/basics#load) - Load (write) the transformed data into the target destination  

### Extract

ETL process starts with retrieving the documents from the database.  
You can choose which documents will be processed by next two stages (Transform and Load).  
The possible options are:  

* Documents from a single collection  
* Documents from multiple collections  
* All documents (RavenDB ETL only)  

### Transform

This stage transforms and filters the extracted documents according to a provided script.  
Any transformation can be done so that only relevant data is shared.  
The script is written in JavaScript and its input is a document.  

In addition to ECMAScript 5.1 API, RavenDB introduces the following functions and members:  

| ------ | ------ | ------ |
| `this` | object | The current document (with metadata) |
| `id(document)` | function | Returns the document ID |
| `load(id)` | function | Load another document.<br/>This will increase the maximum number of allowed steps in a script.<br/>**Note**: Changes made to the other _loaded_ document will _not_ trigger the ETL process.|

Specific ETL functions:  

| ------ |:------:| ------ |
| `loadTo<Target>(obj)` | function | Load an object to a specified `<Target>`.<br/>The target must be either a collection name (RavenDB ETL) or a table name (SQL ETL).<br/>**An object will be sent to the destination only if** `loadTo` method was called.|
| `loadAttachment(name)` | function | Load an attachment (SQL ETL only) |

{INFO: Batch processing}

Documents are extracted and transformed by the ETL process in a batch manner.  
The number of documents processed depends on the following configuration limits:  

* [`ETL.ExtractAndTransformTimeoutInSec`](../../../server/configuration/etl-configuration#etl.extractandtransformtimeoutinsec) (default: 300 sec)  
  Timeframe for the extraction and transformation stages (in seconds), after which the loading stage will start.  

* [`ETL.MaxNumberOfExtractedDocuments`](../../../server/configuration/etl-configuration#etl.maxnumberofextracteddocuments) (default: null)  
  Max number of extracted documents in an ETL batch.  

{INFO/}

### Load

* Loading the results to the target destination is the last stage.

* Note: In contrast to Replication, ETL is a push-only process that _writes_ data to the destination
  whenever documents from the relevant collections were changed. Existing entries on the target will always be _overwritten_.  

* Updates are implemented by executing consecutive DELETEs and INSERTs.  
  When a document is modified, the delete command is sent before the new data is inserted,  
  and both are processed under the same transaction on the destination side.  
  This applies to both ETL types.  

* There are two exceptions to this behavior:  
  * In RavenDB ETL - when documents are loaded to **the same** collection there is no need to send DELETE because the document on the other side has the same identifier and it will just update it.  
  * in SQL ETL - you can configure to use inserts only, which is a viable option for append-only systems.  

{PANEL/}

{NOTE: }

Type specific ETL scripts details and examples can be found in the following articles:  

* [RavenDB ETL](../../../server/ongoing-tasks/etl/raven)  
* [SQL ETL](../../../server/ongoing-tasks/etl/sql)  
{NOTE/}

## Related Articles

- [RavenDB ETL Task](../../../server/ongoing-tasks/etl/raven)
- [SQL ETL Task](../../../server/ongoing-tasks/etl/sql)
- [Define RavenDB ETL Task in Studio](../../../todo-update-me-later)
- [Define SQL ETL Task in Studio](../../../todo-update-me-later)

