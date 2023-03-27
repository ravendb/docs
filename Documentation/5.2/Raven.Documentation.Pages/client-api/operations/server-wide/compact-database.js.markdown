# Compact Database Operation

 ---

{NOTE: }

* The compaction operation __removes empty gaps on disk__ that still occupy space after deletes.  
  You can choose what should be compacted: documents and/or selected indexes.  

* __During compaction the database will be offline__.  
  The operation is a executed asynchronously as a background operation and can be awaited.  

* The operation will __compact the database on one node__.  
  To compact all database-group nodes, the command must be sent to each node separately.  

* **Target node**:  
  By default, the operation will be executed on the server node that is defined by the [client configuration](../../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node).  
  The operation can be executed on a specific node by using the [forNode](../../../client-api/operations/how-to/switch-operations-to-a-different-node) method.  

* **Target database**:  
  The database to compact is specified in `CompactSettings` (see examples below). // todo.. this is interface ??  
  An exception is thrown if the specified database doesn't exist on the server node.  

* In this page:  
  * [Examples](..):  
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

{NOTE: }

#### Compact documents

* The following example will compact only documents for the specified database.  

{CODE:nodejs compact_0@ClientApi\Operations\Server\compact.js /}

{NOTE/}

{NOTE: }

#### Compact specific indexes

* The following example will compact only specific indexes.

{CODE:nodejs compact_1@ClientApi\Operations\Server\compact.js /}

{NOTE/}

{NOTE: }

#### Compact all indexes

* The following example will compact all indexes and documents.  

{CODE:nodejs compact_2@ClientApi\Operations\Server\compact.js /}

{NOTE/}

{NOTE: }

#### Compact on other nodes

* By default, an operation executes on the server node that is defined by the [client configuration](../../../client-api/configuration/load-balance/overview#client-logic-for-choosing-a-node).  
* The following example will compact the database on all [member](../../../server/clustering/rachis/cluster-topology#nodes-states-and-types) nodes from its database-group topology.  
  `forNode` is used to execute the operation on a specific node.   

{CODE:nodejs compact_3@ClientApi\Operations\Server\compact.js /}
 
{NOTE/}
{PANEL/}

{PANEL: Compaction triggers compression}

* When document [compression](../../../server/storage/documents-compression) is turned on, compression is applied to the documents when:
    * __New__ documents that are created and saved.
    * __Existing__ documents that are modified and saved.

* You can use the [compaction](../../../client-api/operations/server-wide/compact-database) operation to __compress existing documents without having to modify and save__ them.  
  Executing compaction triggers compression on ALL existing documents for the collections that are configured for compression.

* Learn more about Compression -vs- Compaction [here](../../../server/storage/documents-compression#compression--vs--compaction).

{PANEL/}

{PANEL: Compact from Studio}

* Compaction can be triggered from the [Storage Report](../../../studio/database/settings/documents-compression#database-storage-report) view in the Studio.  
  The operation will compact the database only on the node being viewed (node info is in the Studio footer).
 
* To compact the database on another node,  
  simply trigger compaction from the Storage Report view in a browser tab opened for that other node.

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Operations\Server\compact.js /}

| Parameters | Type | Description |
| - | - | - |
| **compactSettings** | object  | Settings for the compact operation.<br>See object fields below. |

| compactSettings fields | | |
| - | - | - |
| **databaseName** | string | Name of database to compact. Mandatory param. |
| **documents** | boolean | Indicates if documents should be compacted. Optional param. |
| **indexes** | string[] | List of index names to compact. Optional param. |
| | | __Note__: Either _Documents_ or _Indexes_ (or both) must be specified |

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

- [Storage Report](../../../studio/database/settings/documents-compression#database-storage-report)
