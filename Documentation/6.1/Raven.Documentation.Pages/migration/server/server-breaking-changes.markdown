# Migration: Server Breaking Changes
---

{NOTE: }
The features listed in this page were available in former RavenDB versions.  
In RavenDB `6.1.x`, they are either unavailable or their behavior is inconsistent 
with their behavior in previous versions.  

* In this page:  
   * [Tracking operations using the Changes API now requires Node Tag](../../migration/server/server-breaking-changes#tracking-operations-using-the-changes-api-now-requires-node-tag)  
   * [Custom identity parts-separator is operative](../../migration/server/server-breaking-changes#custom-identity-parts-separator-is-operative)  
   * [MsSql connection string requires an Encrypt property](../../migration/server/server-breaking-changes#mssql-connection-string-requires-an-encrypt-property)  
   * [Corax handling of complex fields in a static index is configurable](../../migration/server/server-breaking-changes#corax-handling-of-complex-fields-in-a-static-index-is-configurable)  

{NOTE/}

---

{PANEL: Tracking operations using the Changes API now requires Node Tag}

[Tracking operations using the changes API](../../client-api/changes/how-to-subscribe-to-operation-changes) 
now requires that you pass the changes API both a database name **and** a node tag for the specific node that 
runs the operation/s you want to track, to assure that the API consistently tracks this selected node.  

The changes API can be opened using two `Changes` overloads. The first passes the API only the database name, 
and is capable of tracking all entities besides operations. Attempting to track operations after opening the 
API this way will fail with the following exception:  
`"Changes API must be provided a node tag in order to track node-specific operations."`

To track operations, open the API using this overload:  
{CODE:csharp changes-definition@migration\BreakingChanges.cs /}  

Then, you can use `ForOperationId` or `ForAllOperations` to track a certain operation or all 
the operations executed by the selected node. Here's a simple usage example:  
{CODE:csharp changes_ForOperationId@migration\BreakingChanges.cs /}  
 
{PANEL/}

{PANEL: Custom identity parts-separator is operative}

An **identity parts separator** can now be defined using the server-wide client configuration, 
to replace the default separator with a user-defined one.  

The configuration option was available in Studio's **Client Configuration** for a while, but 
up until now wasn't operative. Please be aware that changing this setting **will** change your 
IDs now.  

![Identity parts separator](images/breaking-changes_identity-parts-separator.png "Identity parts separator")

After making this change, creating a new document with the identity prefix `|`, e.g. `user|`, 
will apply your new separator.  

![New separator](images/breaking-changes_new-separator.png "New separator")

{PANEL/}

{PANEL: MsSql connection string requires an Encrypt property}

RavenDB is now required by the `Microsoft.Data.SqlClient` package it comprises for 
MsSql ETL communication, to include an `Encrypt` property in its MsSql connection strings.  
The property needs to be included in the connection string to determine whether to encrypt 
the connection or not. 

Recent RavenDB versions up to 6.0 included a `Encrypt=Optional` property in their MsSql 
connection strings, which effectively left the connection unencrypted. RavenDB 6.1 no 
longer includes this property in its connection string, and users are required to explicitly 
choose whether to encrypt the connection or not.  

To encrypt the connection, set this property to `Encrypt=true` and provide the server with 
a valid certificate. The connection string can be provided to RavenDB via Studio or using the API.  

![SQL ETL task](images/breaking-changes_SQL-ETL-task.png "SQL ETL task")

{PANEL/}

{PANEL: Corax handling of complex fields in a static index is configurable}

RavenDB's [Corax](../../indexes/search-engine/corax) search engine's behavior while 
handling [complex fields](../../indexes/search-engine/corax#handling-of-complex-json-objects) 
in **static indexes** is now configurable using the `Indexing.Corax.Static.ComplexFieldIndexingBehavior` 
configuration option.  
(The [handling of auto indexes](../../indexes/search-engine/corax#if-corax-encounters-a-complex-property-while-indexing) 
remains unhanged.)  

By default, `ComplexFieldIndexingBehavior` is set to **`Throw`** to instruct the search 
engine to throw a `NotSupportedInCoraxException` exception when it encounters a complex 
field in a static index.  
The exception is accompanied by this message:  
**"The value of '`{fieldName}`' field is a complex object. Indexing it as a text 
isn't supported. You should consider querying on individual fields of that object.**

Alternatively, you can set `ComplexFieldIndexingBehavior` to **`Skip`** to disable the 
indexing of complex fields without throwing an exception or raising a notification.  

* The configuration option will apply only to **new** static indexes, created after the release 
  of RavenDB 6.1. It will not affect older indexes.  
* `ComplexFieldIndexingBehavior` can be set for a particular index as well as for all indexes.  
* Though complex fields cannot be indexed, they **can** still be stored and projected.  
* To search by the contents of a static index's complex field, you can convert it 
  to a string (using `ToString` on the field value in the index definition).  
  It is recommended, though, to index individual properties of the complex field.  

{PANEL/}
