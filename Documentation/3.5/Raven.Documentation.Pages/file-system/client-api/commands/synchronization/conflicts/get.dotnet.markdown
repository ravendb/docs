#Commands: GetConflictsAsync

**GetConflictsAsync** retrieves the existing conflict items.

{INFO: Conflicts location}
Conflicts always exist in a destination file system.
{INFO/}

## Syntax

{CODE get_conflicts_1@FileSystem\ClientApi\Commands\Synchronization.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | The number of items to skip |
| **pageSize** | int | The maximum number of items to get |


<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;ItemsPage&lt;ConflictItem&gt;&gt;** | A task that represents the asynchronous get operation. The task result is the `ItemsPage` object that contains the number of total results and the list of [`ConflictItem`](../../../../../glossary/conflict-item) objects that represent the synchronization conflict. |


## Example

{CODE get_conflicts_2@FileSystem\ClientApi\Commands\Synchronization.cs /}

## Related articles

- [Synchronization conflicts](../../../../synchronization/conflicts)
