#Commands: ResolveConflictAsync

**ResolveConflictAsync** resolves the conflict according to the specified conflict resolution strategy.   
**ResolveConflictsAsync** resolves all the conflicts according to the specified conflict resolution strategy.

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


{CODE resolve_conflicts_1@FileSystem\ClientApi\Commands\Synchronization.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **strategy** | ConflictResolutionStrategy | The strategy - CurrentVersion or RemoteVersion |

<hr />

| Return Value | |
| ------------- | ------------- |
| **Task** | A task that represents the asynchronous resolve operation |

## Example I

If you want to keep the destination file version, you need to resolve the conflict by using a `CurrentVersion` strategy. 
That will force the history of remote file to be incorporated into the local one, so the next attempt to synchronize the file will result in no operation, as it will appear that the file was already synchronized.

{CODE resolve_conflict_2@FileSystem\ClientApi\Commands\Synchronization.cs /}

{CODE resolve_conflicts_2@FileSystem\ClientApi\Commands\Synchronization.cs /}

## Example II

The second option is to resolve the conflict in favor of the source file version. In this case, you need to set a `RemoveVersion` strategy.
In contrast to the usage of the `CurrentVersion` strategy, the conflict will not disappear right after applying the `RemoveVersion` resolution, as the destination file system never requests any data from the source file system.

This operation will simply add an appropriate metadata record to the conflicted file (`Raven-Synchronization-Conflict-Resolution`) to allow the source file system to synchronize its version in next synchronization run (periodic or triggered manually).

{CODE resolve_conflict_3@FileSystem\ClientApi\Commands\Synchronization.cs /}

{CODE resolve_conflicts_3@FileSystem\ClientApi\Commands\Synchronization.cs /}

## Related articles

- [Synchronization conflicts](../../../../synchronization/conflicts)
