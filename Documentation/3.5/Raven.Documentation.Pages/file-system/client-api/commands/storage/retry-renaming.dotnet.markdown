#Commands: RetryRenamingAsync

**RetryRenamingAsync** runs a background task that will resume unaccomplished file renames. Read [Background tasks](../../../server/background-tasks) article for details.

## Syntax

{CODE retry_renaming_1@FileSystem\ClientApi\Commands\Storage.cs /}


| Return Value | |
| ------------- | ------------- |
| **Task** |  A task that represents the asynchronous operation |

## Example

{CODE retry_renaming_2@FileSystem\ClientApi\Commands\Storage.cs /}
