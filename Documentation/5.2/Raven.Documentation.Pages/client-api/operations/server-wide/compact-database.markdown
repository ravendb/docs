# Operations: Server: How to Compact a Database

* Use the `CompactDatabaseOperation` to compact a database.  
  {INFO: The operation compacts on one node.}
     The command must be sent to each node separately by using `ForNode("node tag")`.  
     [See example I](../../../client-api/operations/server-wide/compact-database#example-i---compact-specific-indexes) 
     or [example II](../../../client-api/operations/server-wide/compact-database#example-ii---compact-all-indexes) below.
  {INFO/}
* Compaction removes empty gaps that still occupy space after deletes.
* You can choose what should be compacted: documents and/or listed indexes.  
  [See example I](../../../client-api/operations/server-wide/compact-database#example-i---compact-specific-indexes) 
  and [example II](../../../client-api/operations/server-wide/compact-database#example-ii---compact-all-indexes) below.
* Compaction can also be done in the Studio database statistics [Storage Report](../../../studio/database/settings/documents-compression#database-storage-report).
   * In Studio, compaction will be done only on the node that is being viewed.  
     See the info bar at the bottom of the Studio interface to see which node you're viewing.

{WARNING: The database will be offline during compaction.}
The compacting operation is executed **asynchronously**, 
and during this operation, **the database will be offline**.  
{WARNING/}

{INFO: Compaction triggers compression on all collections configured for compression.}
If [documents compression](../../../server/storage/documents-compression) is set on any collection, 
all documents in that collection will be compressed upon compaction.  
Without using CompactDatabaseOperation, only documents that are created or modified become compressed.
{INFO/}

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

{CODE compact_3@ClientApi\Operations\Server\Compact.cs /}


## Example II - Compact all indexes

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

- [How to **create** database?](../../../client-api/operations/server-wide/create-database) 
- [How to get database **statistics**?](../../../client-api/operations/maintenance/get-statistics)
- [How to start **restore** operation?](../../../client-api/operations/server-wide/restore-backup)

