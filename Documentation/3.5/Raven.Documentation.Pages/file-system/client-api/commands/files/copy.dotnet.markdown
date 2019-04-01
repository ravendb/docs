#Commands: CopyAsync

**CopyAsync** is used to copy the file.

## Syntax

{CODE copy_1@FileSystem\ClientApi\Commands\Files.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **sourceName** | string | The full path of the file that you want to copy from |
| **targetName** | string | The name of the new file you want to copy to |
| **etag** | Etag | The current file etag used for concurrency checks (`null` skips check) |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task** | A task that represents the asynchronous copy operation |

## Example

{CODE copy_2@FileSystem\ClientApi\Commands\Files.cs /}

Note that with file copy operation you can change the name of the file too:

{CODE copy_3@FileSystem\ClientApi\Commands\Files.cs /}

## Related articles

- [RetryCopyingAsync](../storage/retry-copying)
