#Commands: RetryCopyingAsync

**RetryCopyingAsync** runs a background task that will resume unaccomplished file copies. Read [Background tasks](../../../server/background-tasks) article for details.

## Syntax

{CODE retry_copying_1@FileSystem\ClientApi\Commands\Storage.cs /}


| Return Value | |
| ------------- | ------------- |
| **Task** |  A task that represents the asynchronous operation |

## Example

{CODE retry_copying_2@FileSystem\ClientApi\Commands\Storage.cs /}
