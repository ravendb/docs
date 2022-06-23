# Operations: How to Stop Indexing
---

{NOTE: }

* **StopIndexingOperation** is used to resume indexing for entire database.  

* You must [restart the database](../../../../client-api/operations/maintenance/configuration/database-settings-operation#toggledatabasesstateoperation) 
  with `ToggleDatabasesStateOperation` to implement the operation after using `StopIndexingOperation` ([see example](../../../../client-api/operations/maintenance/indexes/stop-indexing#example)).

* Use [StopIndexOperation](../../../../client-api/operations/maintenance/indexes/stop-index) to stop a single index.

{NOTE/}

---

### Syntax

{CODE stop_1@ClientApi\Operations\Indexes\StopIndexing.cs /}

### Example

{CODE stop_2@ClientApi\Operations\Indexes\StopIndexing.cs /}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)

### Operations

- [How to Enable Index](../../../../client-api/operations/maintenance/indexes/enable-index)
- [How to Stop Index Until Restart](../../../../client-api/operations/maintenance/indexes/stop-index)
- [How to Resume Indexing](../../../../client-api/operations/maintenance/indexes/start-indexing)
