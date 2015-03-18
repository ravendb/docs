#Commands : GetSynchronizationStatusForAsync

**GetSynchronizationStatusForAsync** returns a report that contains information about the synchronization of a specified file.

## Syntax

{CODE get_sync_status_1@FileSystem\ClientApi\Commands\Synchronization.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **filename** | string | The full file name |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;SynchronizationReport&gt;** | A task that represents the asynchronous get operation. The task result is [`SynchronizationReport`](../../../../../glossary/synchronization-report). |

## Example

{CODE get_sync_status_2@FileSystem\ClientApi\Commands\Synchronization.cs /}