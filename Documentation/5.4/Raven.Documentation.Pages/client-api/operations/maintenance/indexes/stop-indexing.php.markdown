# Pause Indexing Operation
---

{NOTE: }

* Use `StopIndexingOperation` to **pause indexing** for ALL indexes in the database.  

* To pause only a specific index use the [StopIndexOperation](../../../../client-api/operations/maintenance/indexes/stop-index).  
 
* In this page:
  * [Overview](../../../../client-api/operations/maintenance/indexes/stop-indexing#overview)
  * [Pause indexing example](../../../../client-api/operations/maintenance/indexes/stop-indexing#pause-indexing-example)
  * [Syntax](../../../../client-api/operations/maintenance/indexes/stop-indexing#syntax)

{NOTE/}

---

{PANEL: Overview}

#### Which node is indexing paused for?

* When pausing indexing from the **client**:  
  Indexing will be paused on the [preferred node](../../../../client-api/configuration/load-balance/overview#the-preferred-node) only, and Not on all the database-group nodes.  

* When pausing indexing from the **Studio** [database list](../../../../studio/database/databases-list-view#more-actions) view:  
  Indexing will be paused on the local node the browser is opened on, even if it is Not the preferred node.

---

#### What happens when indexing is paused for a node?

* No indexing takes place on a node that indexing is paused for.  
  New data **is** indexed on database-group nodes that indexing is not paused for.

* All indexes, including paused ones, can be queried, 
  but results may be stale when querying nodes that indexing has been paused for.
 
* New indexes **can** be created for the database.  
  However, the new indexes will also be paused on any node that indexing is paused for,  
  until indexing is resumed for that node.  

* When [resetting](../../../../client-api/operations/maintenance/indexes/reset-index) indexes 
  or editing index definitions, re-indexing on a node that indexing has been paused for will 
  only be triggered when indexing is resumed on that node.

---

#### Resuming indexing:

* Learn to resume indexing for all indexes by a client, here: [resume indexing](../../../../client-api/operations/maintenance/indexes/start-indexing)  

* Learn to resume indexing for all indexes via **Studio**, here: [database list view](../../../../studio/database/databases-list-view#more-actions)  

* Pausing indexing is **Not a persistent operation**.  
  This means that all paused indexes will resume upon either of the following:
    * The server is restarted.
    * The database is re-loaded (by disabling and then enabling it).  
      Toggling the database state can be done using the **Studio** [database list](../../../../studio/database/databases-list-view#database-actions) view,  
      or using [ToggleDatabasesStateOperation](../../../../client-api/operations/server-wide/toggle-databases-state) by the client.

{PANEL/}

{PANEL: Pause indexing example}

{CODE:php pause_indexing@ClientApi\Operations\Maintenance\Indexes\PauseIndexing.php /}

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax@ClientApi\Operations\Maintenance\Indexes\PauseIndexing.php /}

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
