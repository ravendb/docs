#Commands : GetFinishedAsync

**GetFinishedAsync** allows to page through [`SynchronizationReports`](../../../../../glossary/synchronization-report) of already accomplished file synchronizations.

## Syntax

{CODE get_finished_1@FileSystem\ClientApi\Commands\Synchronization.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | The number of reports to skip |
| **pageSize** | int | The maximum number of reports that will be returned |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;ItemsPage&lt;SynchronizationReport&gt;&gt;** | A task that represents the asynchronous operation. The task result is `ItemsPage` object that contains number of total results and the list of `SynchronizationReport` objects for the requested page. |

## Example

{CODE get_finished_2@FileSystem\ClientApi\Commands\Synchronization.cs /}