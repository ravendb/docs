#Commands: StartCompact

**StartCompact** initializes the compaction of the indicated file system. This operation makes the file system offline for the time of compaction.

## Syntax

{CODE start_compact_1@FileSystem\ClientApi\Commands\Admin.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **filesystemName** | string | The name of a file system to compact |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;long&gt;** | A task that represents the asynchronous restore operation. The task result is the operation identifier. |


## Example

{CODE start_compact_2@FileSystem\ClientApi\Commands\Admin.cs /}

If you need to wait until the operation finishes, you will have to initialize `DocumentStore` associated with `<system>` database and wait for the operation completion:

{CODE start_compact_3@FileSystem\ClientApi\Commands\Admin.cs /}
