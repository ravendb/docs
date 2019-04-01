#Commands: CleanUpAsync

**CleanUpAsync** forces to run a background task that will clean up files marked as deleted. Read [Background tasks](../../../server/background-tasks) article for details.

## Syntax

{CODE clean_up_1@FileSystem\ClientApi\Commands\Storage.cs /}


| Return Value | |
| ------------- | ------------- |
| **Task** |  A task that represents the asynchronous operation |

## Example

{CODE clean_up_2@FileSystem\ClientApi\Commands\Storage.cs /}
