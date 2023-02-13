# Pause Indexing Operation
---

{NOTE: }

* Use `StopIndexingOperation` to __pause indexing__ for ALL indexes in the database.  

* To pause only a specific index use the [pause index operation](../../../../client-api/operations/maintenance/indexes/stop-index).

* In this page:
  * [Overview](../../../../client-api/operations/maintenance/indexes/stop-indexing#overview)
  * [Pause indexing example](../../../../client-api/operations/maintenance/indexes/stop-indexing#pause-indexing-example)
  * [Syntax](../../../../client-api/operations/maintenance/indexes/stop-indexing#syntax)

{NOTE/}

---

{PANEL: Overview}

{NOTE: }

__On which node indexing is paused__:

* When pausing indexing from the __client__:  
  Indexing will be paused on the [preferred node](../../../../client-api/configuration/load-balance/overview#the-preferred-node) only, and Not on all the database-group nodes.  

* When pausing indexing from the __Studio__ (from the [database list view](../../../../studio/database/databases-list-view#more-actions)):  
  Indexing will be paused on the local node the browser is opened on, even if it is Not the preferred node.

{NOTE/}

{NOTE: }

__When indexing is paused on a node__:
 
* No indexing will take place on the node where indexing has paused.  
  New data will be indexed on other database-group nodes where indexing is not paused.

* You can query any index,  
  but results may be stale when querying the node where indexing has paused.
 
* New indexes can be created in the database,  
  however, they will also be in a 'pause' state on the node where you paused indexing,  
  until indexing is resumed on the node.

* When [resetting](../../../../client-api/operations/maintenance/indexes/reset-index) indexes, or editing index definitions, then re-indexing on the node  
  where indexing has paused will only be triggered when indexing is resumed on the node.

{NOTE/}

{NOTE: }

__How to resume indexing__:

* To resume indexing for all indexes from the client:  
  See [resume indexing](../../../../client-api/operations/maintenance/indexes/start-indexing).

* To resume indexing for all indexes from the Studio:  
  Go to the [database list view](../../../../studio/database/databases-list-view#more-actions).  

* Pausing indexing is __Not a persistent operation__.  
  This means that all paused indexes will resume upon either of the following:
    * The server is restarted.
    * The database is re-loaded (by disabling and then enabling the database state).  
      Toggling the database state can be done from [Studio](../../../../studio/database/databases-list-view#database-actions),  
      or from the client by sending the [ToggleDatabasesStateOperation](../../../../client-api/operations/server-wide/toggle-databases-state).

{NOTE/}

{PANEL/}

{PANEL: Pause indexing example}

{CODE:nodejs pause_indexing@ClientApi\Operations\Maintenance\Indexes\pauseIndexing.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Operations\Maintenance\Indexes\pauseIndexing.js /}

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Enable Index](../../../../client-api/operations/maintenance/indexes/enable-index)
- [How to Pause Index Until Restart](../../../../client-api/operations/maintenance/indexes/stop-index)
- [How to Resume Indexing](../../../../client-api/operations/maintenance/indexes/start-indexing)

### Studio

- [Enable/Disable database from Studio](../../../../studio/database/databases-list-view#database-actions)
- [Pause indexing from Studio](../../../../studio/database/databases-list-view#more-actions)
