# Resume Index Operation

---

{NOTE: }

* After an index has been paused with [pause index operation](../../../../client-api/operations/maintenance/indexes/stop-index),  
  use `StartIndexOperation` to **resume the index**.  

* When resuming the index from the **client**:  
  The index is resumed on the preferred node only, and Not on all the database-group nodes.

* When resuming the index from the **Studio** (from the [indexes list view](../../../../studio/database/indexes/indexes-list-view#indexes-list-view---actions)):  
  The index is resumed on the local node the browser is opened on, even if it is Not the preferred node.

* In this page:
    * [Resume index example](../../../../client-api/operations/maintenance/indexes/start-index#resume-index-example)
    * [Syntax](../../../../client-api/operations/maintenance/indexes/start-index#syntax)

{NOTE/}

---

{PANEL: Resume index example}

{CODE:python resume_index@ClientApi\Operations\Maintenance\Indexes\ResumeIndex.py /}

{PANEL/}

{PANEL: Syntax}

{CODE:python syntax@ClientApi\Operations\Maintenance\Indexes\ResumeIndex.py /}

| Parameters | Type | Description |
| - | - |-|
| **indexName** | string | Name of an index to resume |

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
