# Resume Indexing Operation
---

{NOTE: }

* After indexing has been paused with [pause indexing operation](../../../../client-api/operations/maintenance/indexes/stop-indexing),  
  use `StartIndexingOperation` to **resume indexing** for ALL indexes in the database.  
  (Calling 'StartIndexOperation' on a single index will have no effect).

* When resuming indexing from the **client**:  
  Indexing is resumed on the [preferred node](../../../../client-api/configuration/load-balance/overview#the-preferred-node) only, and Not on all the database-group nodes.  

* When resuming indexing from the **Studio** (from the [database list view](../../../../studio/database/databases-list-view#more-actions)):  
  Indexing is resumed on the local node the browser is opened on, even if it is Not the preferred node.  

* In this page:
  * [Resume indexing example](../../../../client-api/operations/maintenance/indexes/start-indexing#resume-indexing-example)
  * [Syntax](../../../../client-api/operations/maintenance/indexes/start-indexing#syntax)

{NOTE/}

---

{PANEL: Resume indexing example}

{CODE:nodejs resume_indexing@client-api\operations\maintenance\indexes\resumeIndexing.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@client-api\operations\maintenance\indexes\resumeIndexing.js /}

{PANEL/}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Disable Index](../../../../client-api/operations/maintenance/indexes/disable-index)
- [How to Pause Index Until Restart](../../../../client-api/operations/maintenance/indexes/stop-index)

### Studio

- [Resume index from Studio](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions)
- [Resume indexing from Studio](../../../../studio/database/databases-list-view#more-actions)
