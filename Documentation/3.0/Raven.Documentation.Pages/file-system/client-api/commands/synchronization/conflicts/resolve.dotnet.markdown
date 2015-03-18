#Commands : ResolveConflictAsync

**ResolveConflictAsync** resolves the conflict according to the specified conflict resolution strategy.

## Syntax

{CODE resolve_conflict_1@FileSystem\ClientApi\Commands\Synchronization.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **filename** | string | The file path |
| **strategy** | ConflictResolutionStrategy | The strategy - CurrentVersion or RemoteVersion |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task** | A task that represents the asynchronous resolve operation |

## Example I

If you want to keep the destination file version you need to resolve the conflict by using `CurrentVersion` strategy. 
That will force the history of remote file to be incorporated into the local one. So the next attempt to synchronize this file will result in
no operation because it will look as the file was already synchronized.

{CODE resolve_conflict_2@FileSystem\ClientApi\Commands\Synchronization.cs /}

## Example II

The second option is to resolve conflict in favor of source file version. You need to set `RemoveVersion` strategy then.
In contrast to the usage of `CurrentVersion` strategy, the conflict will not disappear right after applying `RemoveVersion` resolution because the destination
file system never requests any data from the source file system.

This operation will just add an appropriate metadata record to the conflicted file (`Raven-Synchronization-Conflict-Resolution`) to allow the source
file system to synchronize its version in next synchronization run (periodic or triggered manually).

{CODE resolve_conflict_3@FileSystem\ClientApi\Commands\Synchronization.cs /}