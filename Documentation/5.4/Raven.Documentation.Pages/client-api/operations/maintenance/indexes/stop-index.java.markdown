# Pause Index Operation

The **StopIndexOperation** is used to pause indexing of a single index. 

{NOTE Indexing will be resumed automatically after server restart. /}

### Syntax

{CODE:java stop_1@ClientApi\Operations\Maintenance\Indexes\PauseIndex.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | String | name of an index to stop indexing |

### Example

{CODE:java stop_2@ClientApi\Operations\Maintenance\Indexes\PauseIndex.java /}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Enable Index](../../../../client-api/operations/maintenance/indexes/enable-index)
- [How to Stop Index Until Restart](../../../../client-api/operations/maintenance/indexes/stop-index)
- [How to Resume Index](../../../../client-api/operations/maintenance/indexes/start-index)
