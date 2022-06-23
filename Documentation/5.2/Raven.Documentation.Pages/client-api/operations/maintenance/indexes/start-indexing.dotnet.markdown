# Operations: How to Start Indexing
---

{NOTE: }

* **StartIndexingOperation** is used to resume indexing for entire database.  

* You must [restart the database](../../../../client-api/operations/maintenance/configuration/database-settings-operation#toggledatabasesstateoperation) 
  with `ToggleDatabasesStateOperation` to implement the operation after using `StartIndexingOperation` ([see example](../../../../client-api/operations/maintenance/indexes/start-indexing#example)).

* Use [StartIndexOperation](../../../../client-api/operations/maintenance/indexes/stop-index) to start a single index.

{NOTE/}

---

### Syntax

{CODE start_1@ClientApi\Operations\Indexes\StartIndexing.cs /}

### Example

{CODE start_2@ClientApi\Operations\Indexes\StartIndexing.cs /}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)

### Operations

- [How to Disable Index](../../../../client-api/operations/maintenance/indexes/disable-index)
- [How to Stop Index Until Restart](../../../../client-api/operations/maintenance/indexes/stop-index)
