## Server Configuration : Indexing Configuration

{PANEL:Indexing.RunInMemory}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Indexing.RunInMemory | Set if indexes should run purely in memory | `null` | Server-wide or per database |

When running in memory, the index information is not written to disk and if the server is restarted all indexing data will be lost. This is mostly useful for testing, or faster non-persistant indexing
 
If not set or set to null - indexing will run in memory if core settings *RunInMemory* is set to true.

 Values:
 
 * null - use the value set in core configuration *RunInMemory*
 * true - run indexing in memory
 * false - store information on the disk

{PANEL/}

{PANEL:Indexing.Disable}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Disable | Disable indexing | `false` | Server-wide or per database |

{PANEL/}

{PANEL:Indexing.TempPath}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Indexing.TempPath | Temporary path for indexing files | `null` | Server-wide or per database |

If not set, or set to null - use system's temp directory

{PANEL/}

{PANEL:Indexing.MaxTimeForDocumentTransactionToRemainOpenInSec}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Indexing.MaxTimeForDocumentTransactionToRemainOpenInSec | Set how many seconds indexing will keep document transaction open when indexing | `15` | Server-wide or per database |

When triggered, transaction will be closed and a new one will be opened

{PANEL/}

{PANEL:Indexing.TimeBeforeDeletionOfSupersededAutoIndexInSec}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Indexing.TimeBeforeDeletionOfSupersededAutoIndexInSec | Set how many seconds to keep a superseded auto index | `15` | Server-wide or per database |

{PANEL/}

{PANEL:Indexing.TimeToWaitBeforeMarkingAutoIndexAsIdleInMin}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Indexing.TimeToWaitBeforeMarkingAutoIndexAsIdleInMin | Set how many minutes to wait before marking auto index as idle | `30` | Server-wide or per database |

{PANEL/}

{PANEL:Indexing.DisableQueryOptimizerGeneratedIndexes}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Indexing.DisableQueryOptimizerGeneratedIndexes | Disable query optimizer generated indexes (Auto Indexes) | `false` | Server-wide or per database |

{WARNING Use with caution. /}

{PANEL/}

{PANEL:Indexing.TimeToWaitBeforeDeletingAutoIndexMarkedAsIdleInHrs}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Indexing.TimeToWaitBeforeDeletingAutoIndexMarkedAsIdleInHrs | Set how many hours the database should wait before deleting an auto index with the idle flag | `72` | Server-wide or per database |

{PANEL/}

{PANEL:Indexing.MinNumberOfMapAttemptsAfterWhichBatchWillBeCanceledIfRunningLowOnMemory}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Indexing.MinNumberOfMapAttemptsAfterWhichBatchWillBeCanceledIfRunningLowOnMemory | Set minimum number of map attempts after which batch will be canceled if running low on memory | `512` | Server-wide or per database |

{WARNING Use with caution. /}

{PANEL/}

{PANEL:Indexing.NumberOfConcurrentStoppedBatchesIfRunningLowOnMemory}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Indexing.NumberOfConcurrentStoppedBatchesIfRunningLowOnMemory | Number of concurrent stopped batches if running low on memory | `3` | Server-wide or per database |

{WARNING Use with caution. /}

{PANEL/}

{PANEL:Indexing.MapTimeoutInSec}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Indexing.MapTimeoutInSec | Number of seconds after which mapping will end even if there is more to map | `-1` | Server-wide or per database |

Value of *-1* for map as much as possible in single batch

{PANEL/}

{PANEL:Indexing.MapTimeoutAfterEtagReachedInMin}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Indexing.MapTimeoutAfterEtagReachedInMin | Number of minutes after which mapping will end even if there is more to map | `15` | Server-wide or per database |

This will only be applied if we pass the last etag in collection that we saw when batch was started

{PANEL/}
