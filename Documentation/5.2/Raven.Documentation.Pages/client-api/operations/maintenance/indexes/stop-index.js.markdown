# Pause Index Operation

---

{NOTE: }

* Use `StopIndexOperation` to __pause a single index__ in the database.

* To pause indexing for ALL indexes in the database use the [pause indexing operation](../../../../client-api/operations/maintenance/indexes/stop-indexing).

* In this page:
    * [Overview](../../../../client-api/operations/maintenance/indexes/stop-index#overview)
    * [Pause index example](../../../../client-api/operations/maintenance/indexes/stop-index#pause-index-example)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/stop-index#syntax)

{NOTE/}

---

{PANEL: Overview}

{NOTE: }

__On which node the index is paused__:

* When pausing the index from the client:  
  The index will be paused on the [preferred node](../../../../client-api/configuration/load-balance/overview#the-preferred-node) only, and Not on all the database-group nodes.

* When pausing the index from the Studio (from the [indexes list view](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions)):  
  The index will be paused on the local node the browser is opened on, even if it is Not the preferred node.

{NOTE/}

{NOTE: }

__When index is paused on a node__:

* No indexing will be done by the paused index on the node where index was paused.  
  However, new data will be indexed by the index on other database-group nodes where it was not paused.

* You can query the index,  
  but results may be stale when querying the node where the index is paused.

* You can modify the index definition of a paused index.  
  Once index is resumed, re-indexing will be triggered on the node.

{NOTE/}

{NOTE: }

__How to resume the index__:

* To resume the index from the client - see [resume index](../../../../client-api/operations/maintenance/indexes/start-index).

* To resume the index from the Studio - go to the [indexes list view](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions).

* Pausing the index is Not a persistent operation.  
  This means the paused index will resume upon either of the following:
    * The server is restarted.
    * The database is re-loaded (by disabling and then enabling the database state).  
      Toggling the database state can be done from [database list view](../../../../studio/database/databases-list-view#database-actions) in Studio,  
      or from the client by sending the [ToggleDatabasesStateOperation](../../../../client-api/operations/server-wide/toggle-databases-state).

{NOTE/}

{PANEL/}

{PANEL: Pause index example}

{CODE:nodejs pause_index@ClientApi\Operations\Maintenance\Indexes\pauseIndex.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Operations\Maintenance\Indexes\pauseIndex.js /}

| Parameters | Type | Description |
| - | - | - |
| **indexName** | string | Name of an index to pause |

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)

### Operations

- [How to Disable Index](../../../../client-api/operations/maintenance/indexes/disable-index)
- [How to Resume Index](../../../../client-api/operations/maintenance/indexes/start-index)
- [Pause indexing on database](../../../../client-api/operations/maintenance/indexes/stop-indexing).

### Studio

- [Pause index from Studio](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions)
- [Pause indexing from Studio](../../../../studio/database/databases-list-view#more-actions)
