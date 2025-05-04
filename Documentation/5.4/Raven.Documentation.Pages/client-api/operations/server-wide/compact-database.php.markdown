# Compact Database Operation

 ---

{NOTE: }

* Use the `CompactDatabaseOperation` compaction operation to **removes empty gaps on disk** 
  that still occupy space after deletes.  
  You can choose whether to compact _documents_ and/or _selected indexes_.  

* **During compaction the database will be offline**.  
  The operation is a executed asynchronously as a background operation and can be waited for 
  using `waitForCompletion()`.  

* The operation will **compact the database on one node**.  
  To compact all database-group nodes, the command must be sent to each node separately.  

* **Target node**:  
  By default, the operation will be executed on the server node that is defined by the 
  [client configuration](../../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node).  

* **Target database**:  
  The database to compact is specified in `CompactSettings` (see examples below).  
  An exception is thrown if the specified database doesn't exist on the server node.  

* In this page:  
  * [Examples](../../../client-api/operations/server-wide/compact-database#examples):  
      * [Compact documents](../../../client-api/operations/server-wide/compact-database#examples)  
      * [Compact specific indexes](../../../client-api/operations/server-wide/compact-database#compact-specific-indexes)  
      * [Compact all indexes](../../../client-api/operations/server-wide/compact-database#compact-all-indexes)  
      * [Compact on other nodes](../../../client-api/operations/server-wide/compact-database#compact-on-other-nodes)  
  * [Compaction triggers compression](../../../client-api/operations/server-wide/compact-database#compaction-triggers-compression)  
  * [Compact from Studio](../../../client-api/operations/server-wide/compact-database#compact-from-studio)  
  * [Syntax](../../../client-api/operations/server-wide/compact-database#syntax)  

{NOTE/}

---

{PANEL: Examples}

#### Compact documents:

The following example will compact only **documents** for the specified database.  

{CODE:php compact_0@ClientApi\Operations\Server\Compact.php /}

---

#### Compact specific indexes:

The following example will compact only specific indexes.

{CODE:php compact_1@ClientApi\Operations\Server\Compact.php /}

---

#### Compact all indexes:

The following example will compact all indexes and documents.  

{CODE:php compact_2@ClientApi\Operations\Server\Compact.php /}

---

#### Compact on other nodes:

* By default, an operation executes on the server node that is defined by the [client configuration](../../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node).  
* The following example will compact the database on all [member](../../../server/clustering/rachis/cluster-topology#nodes-states-and-types) nodes from its database-group topology.  
  `forNode` is used to execute the operation on a specific node.   

{CODE:php compact_3@ClientApi\Operations\Server\Compact.php /}
 
{PANEL/}

{PANEL: Compaction triggers compression}

* When document [compression](../../../server/storage/documents-compression) is turned on, compression is applied to the documents when:
  * **New** documents that are created and saved.  
  * **Existing** documents that are modified and saved.  

* You can use the [compaction](../../../client-api/operations/server-wide/compact-database) operation 
  to **compress existing documents without having to modify and save** them.  
  Executing compaction triggers compression on ALL existing documents for the collections that are configured for compression.

* Learn more about Compression -vs- Compaction [here](../../../server/storage/documents-compression#compression--vs--compaction).

{PANEL/}

{PANEL: Compact from Studio}

* Compaction can be triggered from the [Storage Report](../../../studio/database/stats/storage-report) view in the Studio.  
  The operation will compact the database only on the node being viewed (node info is in the Studio footer).
 
* To compact the database on another node,  
  simply trigger compaction from the Storage Report view in a browser tab opened for that other node.

{PANEL/}

{PANEL: Syntax}

{CODE:csharp syntax@ClientApi\Operations\Server\Compact.cs /}

| Parameters | Type | Description |
| - | - | - |
| **$compactSettings** | `?CompactSettings` | Settings for the compact operation |

| `$compactSettings` class parameters | Type | Description |
| - | - | - |
| **$databaseName** | `?string` | Name of database to compact. Mandatory param. |
| **$documents** | `bool` | Indicates if documents should be compacted. Optional param. |
| **$indexes** | `?StringArray` | List of index names to compact. Optional param. |
| **$skipOptimizeIndexes** | `bool` | `true` - Skip Lucene's index optimization while compacting<br>`false` - Lucene's index optimization will take place while compacting |
| | | **Note**: Either **$documents** or **$indexes** (or both) must be specified |

{PANEL/}

## Related Articles

**Database**

- [How to create database?](../../../client-api/operations/server-wide/create-database) 
- [How to get database statistics](../../../client-api/operations/maintenance/get-stats)
- [How to restore a database from backup](../../../client-api/operations/server-wide/restore-backup)
- [Switch operation to different node](../../../client-api/operations/how-to/switch-operations-to-a-different-node)

**Compression**

- [Documents Compression](../../../server/storage/documents-compression)

**Studio**

- [Storage Report](../../../studio/database/stats/storage-report)
