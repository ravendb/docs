#Ongoing Tasks: ETL

RavenDB has the notion of extract, transform, load (ETL) process that transfers data from RavenDB to an external target. The target can be another
RavenDB instance (outside the database group in a cluster environment) or a relational database.

ETL is the three stage process:

- extract documents from a database,
- transform (and filter) them according to the script (optional),
- load transformed results into a destination.

## Why use ETL?

There are several reasons why you'll want to use this feature. In terms of deployment, it gives you an easy way to transfer part of a database's data 
without the need to send full details.

A typical example would be a product catalog that is shared among multiple tenants where each tenant can modify the products or add new ones.

Another example is the reporting database. RavenDB is a wonderful database for OLTP scenarios but for reporting you might use an existing solution working on
a relational database. In order to push RavenDB data there we need to transform it to match the relational model.


## ETL Tasks

As mentioned, RavenDB supports two types of ETL destinations:

- another RavenDB database
- SQL database

The ETL process can be defined in the Studio using the following tasks:

- RavenDB ETL
- SQL ETL

## Connection String

The URL address of a destination is defined using a named connection string. The connection string is defined separately. The ETL refers to it only. 
It makes the deployment easier between environments. Also, because RavenDB ETL works between clusters, you may need to define multiple URLs and it's simpler to put
them all in a single location.

{PANEL:Stages}

### Extract

ETL process starts from retrieving documents from a database. You can choose which documents will be processed by next two stages (transform and load). The possible options are:

- documents from a single collection
- documents from multiple collections
- all documents (only for RavenDB ETL)

### Transform

The essence of ETL process is being able to send only data that is relevant. This stage transforms and filters the extracted documents according to a provided script.
The script is written in JavaScript and its input is a document. In addition to ECMAScript 5.1 API, RavenDB introduces the following functions and members:

| ------ |:------:| ------ |
| `this` | object | Current document (with metadata) |
| `id(document)` | function | Returns the ID of a document|
| `load(id)` | function | Allows document loading, increases the maximum number of allowed steps in a script. |

The functions specific for ETL:

| ------ |:------:| ------ |
| `loadTo<Target>(obj)` | function | Indicates to load an object to a specified `<Target>`. The target must be either the name of a collection (RavenDB ETL) or a table (SQL ETL). |
| `loadAttachment(name)` | function | Loads an attachment (SQL ETL only) |

You can do any transformation and send only data you are interested in sharing. Here is an example of RavenDB ETL script processing documents from `Employees` collection:

{CODE-BLOCK:javascript}

var managerName = null;

if (this.ReportsTo !== null)
{
    var manager = load(this.ReportsTo);
    managerName = manager.FirstName + " " + manager.LastName;
}

loadToEmployees({
    Name: this.FirstName + " " + this.LastName,
    Title: this.Title ,
    BornOn: new Date(this.Birthday).getFullYear(),
    Manager: managerName
});
{CODE-BLOCK/}

{WARNING: Filtering}

An object will be sent to the destination **only** if `loadTo` method was called.

{WARNING /}

Destination type specific details about ETL scripts can be found in dedicated articles about [RavenDB ETL]() and [SQL ETL]().

{INFO:Batch processing}

The documents are extracted and processed in batch manner. The behavior is that the ETL process gets and transform as many documents as possible preserving the following configuration limits:

- [`ETL.ExtractAndTransformTimeoutInSec`](../../../server/configuration/etl-configuration#etl.extractandtransformtimeoutinsec) (default: 300 sec): the number of seconds after which the extraction and transformation will end and the loading stage will start
- [`ETL.MaxNumberOfExtractedDocuments`](../../../server/configuration/etl-configuration#etl.maxnumberofextracteddocuments) (default: null): max number of extracted documents in ETL batch

{INFO/}

### Load

The last stage is loading the results to a destination. The important note is that the ETL, in contrast to the replication, is a push-only process that writes data to the destination
whenever documents from relevant collections get changed. It means it always overwrites existing entries on the target. The updates are implemented by executing consecutive DELETEs and INSERTs.
When a document is modified the delete command is sent before the new data is inserted (everything is processed under the same transaction on the destination side). It applies to both types of ETLs.

There are two exceptions from this behavior:

- in RavenDB ETL when documents are loaded to **the same** collection there is no need to sent DELETE because the document on the other side have the same identifier and it will just update it,
- in SQL ETL you can configure it to use inserts only, which is a viable option for append-only systems

{PANEL/}

