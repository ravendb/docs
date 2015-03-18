#Commands : StartAsync

**StartAsync** method is used to manually force the synchronization to the destinations. It has two overloads that allow to 
synchronize all files that require that or a specified, single file.

{PANEL:Synchronization of all destinations}

{CODE start_1@FileSystem\ClientApi\Commands\Synchronization.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **forceSyncingAll** | bool | Determines whether finished synchronization should schedule a next pending one (there can be only [a limited number of concurrent synchronizations](../../../synchronization/configurations#ravensynchronizationconfig) to a destination file system). The reports of such synchronizations will *not* be included in `DestinationSyncResult` object. |


<hr />

| Return Value | |
| ------------- | ------------- |
| **Task&lt;DestinationSyncResult[]&gt;** | A task that represents the asynchronous synchronization operation. The task result is `ItemsPage` object that contains number of total results and the list of [`ConflictItem`](../../../../../glossary/conflict-item) objects that represent the synchronization conflict. |


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
| **Task&lt;SynchronizationReport&gt;** | A task that represents the asynchronous file synchronization operation. The task result is [`SynchronizationReport`](../../../../../glossary/synchronization-report).  |


### Example

{CODE start_5@FileSystem\ClientApi\Commands\Synchronization.cs /}

{PANEL/}

