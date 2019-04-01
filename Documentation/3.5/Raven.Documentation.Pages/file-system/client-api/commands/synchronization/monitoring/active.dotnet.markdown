#Commands: GetActiveAsync

The **GetActiveAsync** method returns the information about the active outgoing synchronizations.

## Syntax

{CODE get_active_1@FileSystem\ClientApi\Commands\Synchronization.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | The number of items to skip |
| **pageSize** | int | The maximum number of items to get |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;ItemsPage&lt;SynchronizationDetails&gt;&gt;** | A task that represents the asynchronous operation. The task result is an `ItemsPage` object that contains the number of total results and the list of the [`SynchronizationDetails`](../../../../../glossary/synchronization-details) objects, which contains info about synchronizations being in progress. |


## Example

{CODE get_active_2@FileSystem\ClientApi\Commands\Synchronization.cs /}
