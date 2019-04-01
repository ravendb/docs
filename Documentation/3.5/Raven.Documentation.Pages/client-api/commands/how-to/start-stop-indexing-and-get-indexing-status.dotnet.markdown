# Commands: How to start or stop indexing and get indexing status?

Following commands have been created to enable user to toggle indexing and retrieve indexing status:   
- [StartIndexing](../../../client-api/commands/how-to/start-stop-indexing-and-get-indexing-status#startindexing)   
- [StopIndexing](../../../client-api/commands/how-to/start-stop-indexing-and-get-indexing-status#stopindexing)   
- [GetIndexingStatus](../../../client-api/commands/how-to/start-stop-indexing-and-get-indexing-status#getindexingstatus)

{PANEL:StartIndexing}

This methods starts indexing, if it was previously stopped.

### Syntax

{CODE start_indexing_1@ClientApi\Commands\HowTo\StartStopIndexingAndGetIndexingStatus.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **maxNumberOfParallelIndexTasks** | int? | if set then maximum number of parallel indexing tasks will be set to this value |

### Example

{CODE start_indexing_2@ClientApi\Commands\HowTo\StartStopIndexingAndGetIndexingStatus.cs /}

{PANEL/}

{PANEL:StopIndexing}

This methods stops indexing, if it was running.

### Syntax

{CODE stop_indexing_1@ClientApi\Commands\HowTo\StartStopIndexingAndGetIndexingStatus.cs /}

### Example

{CODE stop_indexing_2@ClientApi\Commands\HowTo\StartStopIndexingAndGetIndexingStatus.cs /}

{PANEL/}

{PANEL:GetIndexingStatus}

This methods retrieves current status of the indexing.

### Syntax

{CODE get_indexing_status_1@ClientApi\Commands\HowTo\StartStopIndexingAndGetIndexingStatus.cs /}

| Return Value | |
| ------------- | ----- |
| string | `"Indexing"` if the indexing is running, `"Paused"` otherwise. |

### Example

{CODE get_indexing_status_2@ClientApi\Commands\HowTo\StartStopIndexingAndGetIndexingStatus.cs /}

{PANEL/}

## Related articles

- [How to **create** or **delete database**?](../../../client-api/commands/how-to/create-delete-database)   
