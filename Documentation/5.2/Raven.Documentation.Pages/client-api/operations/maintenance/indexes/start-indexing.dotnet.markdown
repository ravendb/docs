# Resume Indexing Operation
---

{NOTE: }

* After indexing has been paused with [pause indexing operation](../../../../client-api/operations/maintenance/indexes/stop-indexing),  
  use `StartIndexingOperation` to __resume indexing__ for ALL indexes in the database.  
  (Calling 'StartIndexOperation' on a single index will have no effect).

* When resuming indexing from the __client__:  
  Indexing is resumed on the [preferred node](../../../../client-api/configuration/load-balance/overview#the-preferred-node) only, and Not on all the database-group nodes.  

* When resuming indexing from the __Studio__ (from the [database list view](../../../../studio/database/databases-list-view#more-actions)):  
  Indexing is resumed on the local node the browser is opened on, even if it is Not the preferred node.  

* In this page:
  * [Resume indexing example](../../../../client-api/operations/maintenance/indexes/start-indexing#resume-indexing-example)
  * [Syntax](../../../../client-api/operations/maintenance/indexes/start-indexing#syntax)

{NOTE/}

---

{PANEL: Resume indexing example}

{CODE-TABS}
{CODE-TAB:csharp:Sync resume_indexing@ClientApi\Operations\Maintenance\Indexes\ResumeIndexing.cs /}
{CODE-TAB:csharp:Async resume_indexing_async@ClientApi\Operations\Maintenance\Indexes\ResumeIndexing.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax@ClientApi\Operations\Maintenance\Indexes\ResumeIndexing.cs /}

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)

### Operations

- [How to Disable Index](../../../../client-api/operations/maintenance/indexes/disable-index)
- [How to Pause Index Until Restart](../../../../client-api/operations/maintenance/indexes/stop-index)

### Studio

- [Resume index from Studio](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions)
- [Resume indexing from Studio](../../../../studio/database/databases-list-view#more-actions)
