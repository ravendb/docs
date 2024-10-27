# Configuration: Indexing

{PANEL:Indexing.RunInMemory}

Set if indexes should run purely in memory.

- **Type**: `bool`
- **Default**: `null`
- **Scope**: Server-wide or per database

When running in memory, the index information is not written to disk and if the server is restarted all indexing data will be lost. This is mostly useful for testing or faster non-persistant indexing.
 
If not set or set to **null** - indexing will run in memory if core settings *RunInMemory* is set to true.

 Values:
 
 * `null` - use the value set in core configuration *RunInMemory*
 * `true` - run indexing in memory
 * `false` - store information on the disk

{PANEL/}

{PANEL:Indexing.Disable}

Disable indexing.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Indexing.TempPath}

Use this setting to specify a different path for the indexes' temporary files.  
By default, temporary files are created under the `Temp` directory inside the index data directory.  
Learn more about RavenDB directory structure [here](../../server/storage/directory-structure).

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Indexing.MaxTimeForDocumentTransactionToRemainOpenInSec}

Set how many seconds indexing will keep document transaction open when indexing.

- **Type**: `int`
- **Default**: `15`
- **Scope**: Server-wide or per database

When triggered, transaction will be closed and a new one will be opened

{PANEL/}

{PANEL:Indexing.TimeBeforeDeletionOfSupersededAutoIndexInSec}

Set how many seconds to keep a superseded auto index.

- **Type**: `int`
- **Default**: `15`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Indexing.TimeToWaitBeforeMarkingAutoIndexAsIdleInMin}

Set how many minutes to wait before marking auto index as idle.

- **Type**: `int`
- **Default**: `30`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Indexing.DisableQueryOptimizerGeneratedIndexes}

Disable query optimizer generated indexes (Auto Indexes).

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide or per database

{WARNING Use with caution. /}

{PANEL/}

{PANEL:Indexing.TimeToWaitBeforeDeletingAutoIndexMarkedAsIdleInHrs}

Set how many hours the database should wait before deleting an auto index with the idle flag.

- **Type**: `int`
- **Default**: `72`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Indexing.MinNumberOfMapAttemptsAfterWhichBatchWillBeCanceledIfRunningLowOnMemory}

Set minimum number of map attempts after which batch will be canceled if running low on memory.

- **Type**: `int`
- **Default**: `512`
- **Scope**: Server-wide or per database

{WARNING Use with caution. /}

{PANEL/}

{PANEL:Indexing.NumberOfConcurrentStoppedBatchesIfRunningLowOnMemory}

Number of concurrent stopped batches if running low on memory.

- **Type**: `int`
- **Default**: `3`
- **Scope**: Server-wide or per database

{WARNING Use with caution. /}

{PANEL/}

{PANEL:Indexing.MapTimeoutInSec}

Number of seconds after which mapping will end even if there is more to map.

- **Type**: `int`
- **Default**: `-1`
- **Scope**: Server-wide or per database

Value of *-1* for map as much as possible in single batch.

{PANEL/}

{PANEL:Indexing.MapTimeoutAfterEtagReachedInMin}

Number of minutes after which mapping will end even if there is more to map. This will only be applied if we pass the last etag in collection that we saw when batch was started.

- **Type**: `int`
- **Default**: `15`
- **Scope**: Server-wide or per database

This will only be applied if we pass the last etag in collection that we saw when batch was started.

{PANEL/}

{PANEL:Indexing.MaxStepsForScript}

The maximum number of steps in the script execution of a JavaScript index.

- **Type**: `int`
- **Default**: `10000`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Indexing.CleanupIntervalInMin}

Time (in minutes) between auto-index cleanup.

- **Type**: `int`
- **Default**: `10`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Indexing.TransactionSizeLimitInMb}

Transaction size limit in megabytes after which an index will stop and complete the current batch.

- **Type**: `int`
- **Default**: `null` (no limit)
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Indexing.ScratchSpaceLimitInMb}

Amount of scratch space in megabytes that we allow to use for the index storage. After exceeding this limit the current indexing batch will complete and the index will force flush and sync storage environment.

- **Type**: `int`
- **Default**: `null` (no limit)
- **Scope**: Server-wide only or per database

{PANEL/}


{PANEL:Indexing.GlobalScratchSpaceLimitInMb}

Maximum amount of scratch space in megabytes that we allow to use for all index storages per server. After exceeding this limit the indexes will complete their current indexing batches and force flush and sync storage environments.

- **Type**: `int`
- **Default**: `null` (no limit)
- **Scope**: Server-wide only

{PANEL/}


{PANEL:Indexing.MaxTimeToWaitAfterFlushAndSyncWhenExceedingScratchSpaceLimit}

Max time to wait in seconds when forcing the storage environment flush and sync after exceeding the scratch space limit.

- **Type**: `int`
- **Default**: `30`
- **Scope**: Server-wide only

{PANEL/}
