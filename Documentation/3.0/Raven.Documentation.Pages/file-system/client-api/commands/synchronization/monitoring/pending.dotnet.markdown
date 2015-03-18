#Commands : GetPendingAsync

**GetPendingAsync** method returns information about files that wait for a synchronization slot to a destination file system.

## Syntax

{CODE get_pending_1@FileSystem\ClientApi\Commands\Synchronization.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | The number of items to skip |
| **pageSize** | int | The maximum number of items to get |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;ItemsPage&lt;SynchronizationDetails&gt;&gt;** | A task that represents the asynchronous operation. The task result is `ItemsPage` object that contains number of total results and the list of [`SynchronizationDetails`](../../../../../glossary/synchronization-details) objects that contains info about pending file synchronizations. |


## Example

{CODE get_pending_2@FileSystem\ClientApi\Commands\Synchronization.cs /}