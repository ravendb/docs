# Index Administration

---

{NOTE: }

* Indexes can be easily managed from the [Studio](../studio/database/indexes/indexes-list-view#indexes-list-view) or via [maintenance operations](../client-api/operations/what-are-operations#what-are-operations) in the Client API.

* The following actions can be applied:
    * [Pause & resume index](../indexes/index-administration#pause-&-resume-index)
    * [Pause & resume indexing](../indexes/index-administration#pause-&-resume-indexing)
    * [Disable & enable index](../indexes/index-administration#disable-&-enable-index)
    * [Disable & enable indexing](../indexes/index-administration#disable-&-enable-indexing)
    * [Reset index](../indexes/index-administration#reset-index)
    * [Delete index](../indexes/index-administration#delete-index)
    * [Set index lock mode](../indexes/index-administration#set-index-lock-mode)
    * [Set index priority](../indexes/index-administration#set-index-priority)
    * [Customize indexing configuration](../indexes/index-administration#customize-indexing-configuration)

{NOTE/}

---

{PANEL: Pause & resume index}

* An index can be paused (and resumed).  
* See [pause index](../client-api/operations/maintenance/indexes/stop-index) & [resume index](../client-api/operations/maintenance/indexes/start-index) for detailed information.  
* Operation scope: Single node.

{PANEL/}

{PANEL: Pause & resume indexing}

* You can pause (and resume) indexing of ALL indexes.  
* See [pause indexing](../client-api/operations/maintenance/indexes/stop-indexing) & [resume indexing](../client-api/operations/maintenance/indexes/start-indexing) for detailed information.
* Operation scope: Single node.

{PANEL/}

{PANEL: Disable & enable index}

* An index can be disabled (and enabled), this is a persistent operation.  
* See [disable index](../client-api/operations/maintenance/indexes/disable-index) & [enable index](../client-api/operations/maintenance/indexes/enable-index) for detailed information.
* Operation scope: Single node, or all database-group nodes.

{PANEL/}

{PANEL: Disable & enable indexing}

* Indexing can be disabled (and enabled) for ALL indexes, this is a persistent operation.  
* This is done from the [database list view](../studio/database/databases-list-view#more-actions) in the Studio.  
* Operation scope: All database-group nodes.  

{PANEL/}

{PANEL: Reset index}

* Resetting an index will force re-indexing of all documents that match the index definition.  
  An index usually needs to be reset once it reached its error quota and is in an _Error_ state.
* See [reset index](../client-api/operations/maintenance/indexes/reet-index) for detailed information.
* Operation scope: Single node.

{PANEL/}

{PANEL: Delete index}

* An index can be deleted from the database.   
* See [delete index](../client-api/operations/maintenance/indexes/delete-index) for detailed information.
* Operation scope: All database-group nodes.

{PANEL/}

{PANEL: Set index lock mode}

* The lock mode controls whether modifications to the index definition are applied (static indexes only). 
* See [set index lock](../client-api/operations/maintenance/indexes/set-index-lock) for detailed information.
* Operation scope: All database-group nodes.

{PANEL/}  

{PANEL: Set index priority}

* Each index has a dedicated thread that handles all the work for the index.  
  Setting the index priority will affect the thread priority at the operating system level.
* See [set index priority](../client-api/operations/maintenance/indexes/set-index-priority) for detailed information.
* Operation scope: All database-group nodes.

{PANEL/}

{PANEL: Customize indexing configuration}

* There are many [indexing configuration](../server/configuration/indexing-configuration) options available.  

* A configuration key with a __"per-index" scope__ can be customized for a specific index,  
  overriding the server-wide and the database configuration values.

* The "per-index" configuration key can be set from:
  * The [configuration tab](../studio/database/indexes/create-map-index#configuration) in the Edit Index view in the Studio.  
  * The [index class constructor](../indexes/creating-and-deploying#creating-an-index-with-custom-configuration) when defining an index.  
  * The [index definition](../client-api/operations/maintenance/indexes/put-indexes#put-indexes-operation-with-indexdefinition) when sending a [putIndexesOperation](../client-api/operations/maintenance/indexes/put-indexes).

__Expert configuration options:__

* [Server.IndexingAffinityMask](../server/configuration/server-configuration#server.indexingaffinitymask) - Control the affinity mask of indexing threads
* [Server.NumberOfUnusedCoresByIndexes](../server/configuration/server-configuration#server.numberofunusedcoresbyindexes) - Set the number of cores that _won't_ be used by indexes

{PANEL/}

## Related Articles

### Indexes

- [Indexes: Overview](../studio/database/indexes/indexes-overview#indexes-overview)
- [What are Indexes](../indexes/what-are-indexes)

### Troubleshooting

- [Debugging Index Errors](../indexes/troubleshooting/debugging-index-errors)
