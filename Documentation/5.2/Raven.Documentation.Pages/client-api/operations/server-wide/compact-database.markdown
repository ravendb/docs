# Operations: Server: How to Compact a Database

* The compaction operation removes empty gaps on the disk that still occupy space after deletes.

* You can choose what should be compacted: documents and/or listed indexes.  
  [See Example I - Compact specific indexes](../../../client-api/operations/server-wide/compact-database#example-i---compact-specific-indexes) 
  and [Example II- Compact all indexes](../../../client-api/operations/server-wide/compact-database#example-ii---compact-all-indexes) below.

* The operation compacts on one node.  
  If you wish to compact all nodes in the database group, the command must be sent to each node separately  
  by using `ForNode("node tag")`.  [See example I](../../../client-api/operations/server-wide/compact-database#example-i---compact-specific-indexes) 
  or [example II](../../../client-api/operations/server-wide/compact-database#example-ii---compact-all-indexes) below.

* Compaction triggers [documents compression](../../../server/storage/documents-compression) 
  on all collections that are configured for compression.  
  Without using CompactDatabaseOperation, only new or modified documents become compressed.
  
* **From Client API:**
  Use the `CompactDatabaseOperation` to select the collections and compact them.  
  To compact on more than one node, the command must be sent to each node by specifying `ForNode("node tag")`. 

* **From Studio:**
  Compaction can be triggered from the [Storage Report](../../../studio/database/settings/documents-compression#database-storage-report) view.  
  The operation will compact the database only on the node being viewed.  
  See the info bar at the bottom of the Studio interface to see which node you're viewing.

{WARNING: The database will be offline during compaction.}
The compacting operation is executed **asynchronously**, 
and during this operation, **the database will be offline**.  
{WARNING/}

* In this page:
   * [Syntax](../../../client-api/operations/server-wide/compact-database#syntax)
   * [Example I - Compact specific indexes](../../../client-api/operations/server-wide/compact-database#example-i---compact-specific-indexes)
   * [Example II - Compact all indexes](../../../client-api/operations/server-wide/compact-database#example-ii---compact-all-indexes)
   * [Example III - Compact a database that is external to the store](../../../client-api/operations/server-wide/compact-database#example-iii)

## Syntax

{CODE compact_1@ClientApi\Operations\Server\Compact.cs /}

{CODE compact_2@ClientApi\Operations\Server\Compact.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **DatabaseName** | string | Name of a database to compact |
| **Documents** | bool | Indicates if documents should be compacted |
| **Indexes** | string[] | List of index names to compact |

## Example I - Compact specific indexes

Specific indexes are listed in `CompactSettings` in this example.

{CODE compact_3@ClientApi\Operations\Server\Compact.cs /}


## Example II - Compact all indexes

The following example shows how compact all of the indexes and documents in the database. 

{CODE compact_4@ClientApi\Operations\Server\Compact.cs /}


## Example III - Compact a database that is external to the store

* CompactDatabaseOperation automatically runs **on the store's database**.  
  If we try to compact **a different database**, the process will succeed only if the database 
  resides **on the cluster's first online node**.  
  Trying to compact a non-default database on a different node will fail with an error such as -  
  **_"500 Internal Server Error : 
  System.InvalidOperationException , 
  Cannot compact database 'name' on node A, because it doesn't reside on this node."_**  
  
* To solve this, we can explicitly identify the database we want to compact by providing 
  its name to CompactDatabaseOperation as in the following example.  
  {CODE compact_5@ClientApi\Operations\Server\Compact.cs /}

## Related Articles

**Database**

- [How to create database?](../../../client-api/operations/server-wide/create-database) 
- [How to get database statistics](../../../client-api/operations/maintenance/get-statistics)
- [How to restore a database from backup](../../../client-api/operations/server-wide/restore-backup)

**Compression**

- [Documents Compression](../../../server/storage/documents-compression)

**Studio**

- [Storage Report](../../../studio/database/settings/documents-compression#database-storage-report)
