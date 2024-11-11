# Pause Index Operation

---

{NOTE: }

* Use `StopIndexOperation` to **pause a single index** in the database.

* To pause indexing for ALL indexes in the database use [StopIndexingOperation](../../../../client-api/operations/maintenance/indexes/stop-indexing).

* In this page:
    * [Overview](../../../../client-api/operations/maintenance/indexes/stop-index#overview)
    * [Pause index example](../../../../client-api/operations/maintenance/indexes/stop-index#pause-index-example)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/stop-index#syntax)

{NOTE/}

---

{PANEL: Overview}

#### Which node is the index paused for?

* When pausing the index from the **client**:  
  The index will be paused for the [preferred node](../../../../client-api/configuration/load-balance/overview#the-preferred-node) only, 
  Not for all database-group nodes.

* When pausing the index from the **Studio** [indexes list](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions) view:  
  The index will be paused for the local node the browser is opened on, even if it is Not the preferred node.

---

#### What happens when an index is paused for a node?

* A paused index performs no indexing for the node it is paused for.  
  New data **is** indexed by the index on database-group nodes that the index is not paused for.

* A paused index **can** be queried, but results may be stale when querying the node that the index is paused for.

---

#### Resuming the index:

* Learn how to resume an index by a client here: [Resume index](../../../../client-api/operations/maintenance/indexes/start-index)  

* Learn to resume an index from **Studio** here: [Indexes list view](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions)  

* Pausing the index is **Not a persistent operation**.  
  This means the paused index will resume upon either of the following:
    * The server is restarted.
    * The database is re-loaded (by disabling and then enabling it).  
      Toggling the database state can be done using the **Studio** [database list](../../../../studio/database/databases-list-view#database-actions) view,  
      or using [ToggleDatabasesStateOperation](../../../../client-api/operations/server-wide/toggle-databases-state) by the client.

* [Resetting](../../../../client-api/operations/maintenance/indexes/reset-index) a paused index will resume the normal operation of the index  
  on the local node where the reset action was performed.

* Modifying the index definition will resume the normal operation of the index  
  on all the nodes for which it is paused.

{PANEL/}

{PANEL: Pause index example}

{CODE:php pause_index@ClientApi\Operations\Maintenance\Indexes\PauseIndex.php /}

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax@ClientApi\Operations\Maintenance\Indexes\PauseIndex.php /}

| Parameters | Type | Description |
| - | - | - |
| **$indexName** | `?string` | Name of an index to pause |

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Disable Index](../../../../client-api/operations/maintenance/indexes/disable-index)
- [How to Resume Index](../../../../client-api/operations/maintenance/indexes/start-index)
- [Pause indexing on database](../../../../client-api/operations/maintenance/indexes/stop-indexing).

### Studio

- [Pause index from Studio](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions)
- [Pause indexing from Studio](../../../../studio/database/databases-list-view#more-actions)
