# Disable Index Operation

The **DisableIndexOperation** is used to turn the indexing off for a given index. Querying a `disabled` index is allowed, but it may return stale results.

{NOTE Unlike [StopIndex](../../../../client-api/operations/maintenance/indexes/stop-index) or [StopIndexing](../../../../client-api/operations/maintenance/indexes/stop-indexing) disable index is a persistent operation, so the index remains disabled even after a server restart. /}


## Syntax

{CODE:java disable_1@ClientApi\Operations\Maintenance\Indexes\DisableIndex.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | String | name of an index to disable indexing |

## Example

{CODE:java disable_2@ClientApi\Operations\Maintenance\Indexes\DisableIndex.java /}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)
- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Enable Index](../../../../client-api/operations/maintenance/indexes/enable-index)
- [How to Pause Index Until Restart](../../../../client-api/operations/maintenance/indexes/stop-index)
