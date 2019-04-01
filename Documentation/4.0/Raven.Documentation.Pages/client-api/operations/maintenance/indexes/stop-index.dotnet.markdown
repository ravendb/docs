# Operations: How to Stop an Index

The **StopIndexOperation** is used to stop indexing for an index. 

{NOTE Indexing will be resumed automatically after server restart. /}

### Syntax

{CODE stop_1@ClientApi\Operations\Indexes\StopIndex.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | string | name of an index to stop indexing |

### Example

{CODE stop_2@ClientApi\Operations\Indexes\StopIndex.cs /}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)

### Operations

- [How to Enable Index](../../../../client-api/operations/maintenance/indexes/enable-index)
- [How to Stop Index Until Restart](../../../../client-api/operations/maintenance/indexes/stop-index)
- [How to Resume Index](../../../../client-api/operations/maintenance/indexes/start-index)
