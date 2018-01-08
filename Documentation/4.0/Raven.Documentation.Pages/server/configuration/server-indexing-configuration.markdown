## Server Configuration : Indexing Options

<br>

#### RunInMemory
###### Set if indexes should run purely in memory
###### Default Value : null

 When running in memory, the index information is not written to disk and if the server is restarted all indexing data will be lost. This is mostly useful for testing, or faster non-persistant indexing
 
 If not set or set to null - indexing will run in memory if core settings *RunInMemory* is set to true.
 
 Values:
 
 * null - use the value set in core configuration *RunInMemory*
 * true - run indexing in memory
 * false - store information on the disk
 
<br><br>

#### Disable
###### Disable indexing
###### Default Value : false

<br><br>

#### TempPath
###### Temporary path for indexing files
###### Default Value : null

If not set, or set to null - use system's temp directory

<br><br>

#### MaxTimeForDocumentTransactionToRemainOpenInSec
###### Set how many seconds indexing will keep document transaction open when indexing
###### Default Value :15

When triggered, transaction will be closed and a new one will be opened

<br><br>

#### TimeBeforeDeletionOfSupersededAutoIndexInSec
###### Set how many seconds to keep a superseded auto index
###### Default Value : 15

<br><br>


#### TimeToWaitBeforeMarkingAutoIndexAsIdleInMin
###### Set how many minutes to wait before marking auto index as idle
###### Default Value : 30

<br><br>

#### DisableQueryOptimizerGeneratedIndexes
###### Disable query optimizer generated indexes
###### Default Value : false

{WARNING Use with cautious /}

<br><br>

#### TimeToWaitBeforeDeletingAutoIndexMarkedAsIdleInHrs
###### Set how many hours the database should wait before deleting an auto index with the idle flag
###### Default Value : 72

<br><br>

#### MinNumberOfMapAttemptsAfterWhichBatchWillBeCanceledIfRunningLowOnMemory
###### Set minimum number of map attempts after which batch will be canceled if running low on memory
###### Default Value : 512

{WARNING Use with cautious /}

<br><br>


#### NumberOfConcurrentStoppedBatchesIfRunningLowOnMemory
###### Number of concurrent stopped batches if running low on memory
###### Default Value : 3

<br><br>

#### MapTimeoutInSec
###### Number of seconds after which mapping will end even if there is more to map
###### Default Value : -1

Value of *-1* for map as much as possible in single batch

<br><br>

#### MapTimeoutAfterEtagReachedInMin
###### Number of minutes after which mapping will end even if there is more to map
###### Default Value : 15

This will only be applied if we pass the last etag in collection that we saw when batch was started
