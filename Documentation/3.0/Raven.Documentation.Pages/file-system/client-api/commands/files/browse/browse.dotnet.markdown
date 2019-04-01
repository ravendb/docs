#Commands: BrowseAsync

The **BrowseAsync** method is used to scan a file system for existing files.

## Syntax

{CODE browse_1@FileSystem\ClientApi\Commands\Browse.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | The number of files that will be skipped |
| **pageSize** | int | The maximum number of file headers that will be retrieved (default: 1024) |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;FileHeader[]&gt;** | A task that represents the asynchronous browse operation. The task result is the array of [`FileHeaders`](../../../../../glossary/file-header). |

## Example

{CODE browse_2@FileSystem\ClientApi\Commands\Browse.cs /}
