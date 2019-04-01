#Commands: SetDestinationsAsync

**SetDestinationsAsync** is used to setup the servers where files should be synchronized.

### Syntax

{CODE set_destinations_1@FileSystem\ClientApi\Commands\Synchronization.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **destinations** | SynchronizationDestination[] | The array of `SynchronizationDestination` objects representing destination file systems |

<hr/>

| Return Value | |
| ------------- | ------------- |
| **Task** | A task that represents the asynchronous set operation |

###Example

{CODE set_destinations_2@FileSystem\ClientApi\Commands\Synchronization.cs /}

## Related articles

- [Synchronization destinations](../../../../synchronization/how-it-works#destinations)
