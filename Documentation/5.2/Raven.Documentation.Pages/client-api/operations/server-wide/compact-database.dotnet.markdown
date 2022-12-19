# How to Compact a Database

 ---

{NOTE: }

* The compaction operation __removes empty gaps on disk__ that still occupy space after deletes.  
  You can choose what should be compacted: documents and/or selected indexes.  

* __During compaction the database will be offline__.  
  The operation is a executed asynchronously as a background operation and can be awaited.  

* The operation will __compact the database on one node__.  
  To compact all database-group nodes, the command must be sent to each node separately.  

* **Target node**:  
  By default, the operation will be executed on the server node that is defined by the [client configuration](../../../client-api/configuration/load-balance-and-failover).  
  The operation can be executed on a specific node by using the [ForNode](../../../client-api/operations/how-to/switch-operations-to-different-node) method.  

* **Target database**:  
  The database to compact is specified in `CompactSettings` (see examples below).  
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

{CODE-TABS}
{CODE-TAB:csharp:Sync compact_0@ClientApi\Operations\Server\Compact.cs /}
{CODE-TAB:csharp:Async compact_0_async@ClientApi\Operations\Server\Compact.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

#### Compact specific indexes

* The following example will compact only specific indexes.

{CODE-TABS}
{CODE-TAB:csharp:Sync compact_1@ClientApi\Operations\Server\Compact.cs /}
{CODE-TAB:csharp:Async compact_1_async@ClientApi\Operations\Server\Compact.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

#### Compact all indexes

* The following example will compact all indexes and documents.  

{CODE-TABS}
{CODE-TAB:csharp:Sync compact_2@ClientApi\Operations\Server\Compact.cs /}
{CODE-TAB:csharp:Async compact_2_async@ClientApi\Operations\Server\Compact.cs /}
{CODE-TABS/}

{NOTE/}

{NOTE: }

#### Compact on other nodes

* By default, an operation executes on the server node that is defined by the [client configuration](../../../client-api/configuration/load-balance-and-failover).  
* The following example will compact the database on all [member](../../../server/clustering/rachis/cluster-topology#nodes-states-and-types) nodes from its database-group topology.  
  `ForNode` is used to execute the operation on a specific node.   

{CODE-TABS}
{CODE-TAB:csharp:Sync compact_3@ClientApi\Operations\Server\Compact.cs /}
{CODE-TAB:csharp:Async compact_3_async@ClientApi\Operations\Server\Compact.cs /}
{CODE-TABS/}
 
{NOTE/}
{PANEL/}

{PANEL: Compaction triggers compression}

* The __compaction__ operation triggers documents [compression](../../../server/storage/documents-compression) on all existing documents in collections that are configured for compression.  
* Differences between the two features are summarized below:

| __Compaction__ | |
| - | - |
| Action: | Remove empty gaps on disk that still occupy space after deletes |
| Items that can be compacted: | Documents and/or indexes on the specified database |
| Triggered by: | Client API code |
| Triggered when: | Explicitly calling `CompactDatabaseOperation` |

| __Compression__ | |
| - | - |
| Action: | Reduce storage space using the Zstd compression algorithm |
| Items that can be compressed: | __-__ Documents in collections that are configured for compression<br>__-__ Revisions for all collections |
| Triggered by: | The server |
| Triggered when: | Compression feature is configured,<br> __and__ when either of the following occurs for the configured collections:<br>&nbsp;&nbsp;&nbsp;__-__ Storing new documents<br>&nbsp;&nbsp;&nbsp;__-__ Modifying & saving existing documents<br>&nbsp;&nbsp;&nbsp;__-__ Compact operation is triggered, existing documents will be compressed |

{PANEL/}

{PANEL: Compact from Studio}

* Compaction can be triggered from the [Storage Report](../../../studio/database/settings/documents-compression#database-storage-report) view in the Studio.  
  The operation will compact the database only on the node being viewed (node info is in the Studio footer).
 
* To compact the database on another node,  
  simply trigger compaction from the Storage Report view in a browser tab opened for that other node.

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Operations\Server\Compact.cs /}

| Parameters | Type | Description |
| - | - | - |
| **compactSettings** | `CompactSettings`  | settings for the compact operation |

| `CompactSettings` | | |
| - | - | - |
| **DatabaseName** | string | Name of a database to compact. Mandatory param. |
| **Documents** | bool | Indicates if documents should be compacted. Optional param. |
| **Indexes** | string[] | List of index names to compact. Optional param. |
| **SkipOptimizeIndexes** | bool | `true` - Skip Lucene's index optimization while compacting<br>`false` - Lucene's index optimization will take place while compacting |
| | | __Note__: Either _Documents_ or _Indexes_ (or both) must be specified |

{PANEL/}

## Related Articles

**Database**

- [How to create database?](../../../client-api/operations/server-wide/create-database) 
- [How to get database statistics](../../../client-api/operations/maintenance/get-stats)
- [How to restore a database from backup](../../../client-api/operations/server-wide/restore-backup)
- [Switch operation to different node](../../../client-api/operations/how-to/switch-operations-to-different-node)

**Compression**

- [Documents Compression](../../../server/storage/documents-compression)

**Studio**

- [Storage Report](../../../studio/database/settings/documents-compression#database-storage-report)
