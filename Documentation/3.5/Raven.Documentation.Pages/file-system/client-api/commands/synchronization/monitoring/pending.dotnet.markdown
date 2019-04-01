#Commands: GetPendingAsync

The **GetPendingAsync** method returns the information about the files that wait for a synchronization slot to a destination file system.

## Syntax

{CODE get_pending_1@FileSystem\ClientApi\Commands\Synchronization.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | The number of items to skip |
| **pageSize** | int | The maximum number of items to get |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;ItemsPage&lt;SynchronizationDetails&gt;&gt;** | A task that represents the asynchronous operation. The task result is an `ItemsPage` object that contains the number of total results and the list of the [`SynchronizationDetails`](../../../../../glossary/synchronization-details) objects, which contains info about pending file synchronizations. |


## Example

{CODE get_pending_2@FileSystem\ClientApi\Commands\Synchronization.cs /}
