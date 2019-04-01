#Commands: GetDirectoriesAsync

The **GetDirectoriesAsync** method is designated to retrieve the paths of subdirectories of a specified directory. 

## Syntax

{CODE get_directories_1@FileSystem\ClientApi\Commands\Browse.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **from** | string | The directory path (default: `null` means the root directory)|
| **start** | int | The number of results that should be skipped (for paging purposes) |
| **pageSize** | int | The max number of results to get |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;string[]&gt;** | A task that represents the asynchronous operation. The task result is the array of subdirectory paths. |

## Example

{CODE get_directories_2@FileSystem\ClientApi\Commands\Browse.cs /}
