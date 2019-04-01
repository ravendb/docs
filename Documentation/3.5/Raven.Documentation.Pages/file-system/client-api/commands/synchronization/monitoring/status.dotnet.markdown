#Commands: GetSynchronizationStatusForAsync

**GetSynchronizationStatusForAsync** returns a report that contains the information about the synchronization of a specified file.

{INFO: The actual server to ask}
This method is intended to ask the destination file system about the performed synchronization report. It means that the actual request should be sent there (not to the source server which pushed the data there).
{INFO/}

## Syntax

{CODE get_sync_status_1@FileSystem\ClientApi\Commands\Synchronization.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **filename** | string | The full file name |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;SynchronizationReport&gt;** | A task that represents the asynchronous get operation. The task result is an [`SynchronizationReport`](../../../../../glossary/synchronization-report). |

## Example

{CODE get_sync_status_2@FileSystem\ClientApi\Commands\Synchronization.cs /}
