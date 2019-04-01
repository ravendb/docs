#Commands: StartAsync

The **StartAsync** method is used to manually force the synchronization to the destinations. It has two overloads that allow you either to synchronize all the files which require that or to synchronize just one specified file.

{PANEL:Synchronization of all destinations}

{CODE start_1@FileSystem\ClientApi\Commands\Synchronization.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **forceSyncingAll** | bool | Determines whether finished synchronization should schedule a next pending one (there can be only [a limited number of concurrent synchronizations](../../../synchronization/configurations#ravensynchronizationconfig) to a destination file system). The reports of such synchronizations will *not* be included in `DestinationSyncResult` object. |


<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;DestinationSyncResult[]&gt;** | A task that represents the asynchronous synchronization operation. The task result is an array of `DestinationSyncResult` object that contains reports about performed operations. |


### Example

{CODE start_4@FileSystem\ClientApi\Commands\Synchronization.cs /}

{PANEL/}

{PANEL:Synchronization of a particular file to a single file system}

{CODE start_2@FileSystem\ClientApi\Commands\Synchronization.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **filename** | string | The full file name |
| **destination** | SynchronizationDestination | The destination file system |


<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;SynchronizationReport&gt;** | A task that represents the asynchronous file synchronization operation. The task result is a [`SynchronizationReport`](../../../../glossary/synchronization-report).  |


### Example

{CODE start_5@FileSystem\ClientApi\Commands\Synchronization.cs /}

{PANEL/}

## Related articles

- [How file synchronization works?](../../../synchronization/how-it-works)

