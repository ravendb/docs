#Commands: ResetIndexes

**ResetIndexes** forces RavenFS to rebuild Lucene indexes from scratch.

## Syntax

{CODE reset_indexes_1@FileSystem\ClientApi\Commands\Admin.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **filesystemName** | string | The name of the file system |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task** |  A task that represents the asynchronous restore operation |

## Example

{CODE reset_indexes_2@FileSystem\ClientApi\Commands\Admin.cs /}
