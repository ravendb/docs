#Commands: StartsWithAsync

**StartsWithAsync** can be used to retrieve multiple file headers for the specified prefix name.

## Syntax

{CODE starts_with_1@FileSystem\ClientApi\Commands\Browse.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **prefix** | string | The prefix that the returned files need to match |
| **matches** | string | Pipe ('&#124;') separated values for which file name (after 'prefix') should be matched ('?' any single character; '*' any characters) |
| **start** | int | The number of files that should be skipped |
| **pageSize** | int | The maximum number of the file headers that will be returned |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;FileHeader[]&gt;** | A task that represents the asynchronous operation. The task result is the array of [`FileHeaders`](../../../../../glossary/file-header). |

## Example I 

The below code will return 128 results at most for files which names start with `/images`.

{CODE starts_with_2@FileSystem\ClientApi\Commands\Browse.cs /}

## Example II

In contrast to the previous example, here only the images which names end with `.jpg` will be returned.

{CODE starts_with_3@FileSystem\ClientApi\Commands\Browse.cs /}
