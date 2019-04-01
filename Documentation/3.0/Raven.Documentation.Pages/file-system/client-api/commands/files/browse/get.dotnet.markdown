#Commands: GetAsync

**GetAsync** is used to get the file headers of the selected files.

## Syntax

{CODE get_1@FileSystem\ClientApi\Commands\Browse.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **filenames** | string[] | Names of the files you want to get headers for. |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;FileHeader[]&gt;** | A task that represents the asynchronous get operation. The task result is the array of [`FileHeaders`](../../../../../glossary/file-header). |

## Example

{CODE get_2@FileSystem\ClientApi\Commands\Browse.cs /}
