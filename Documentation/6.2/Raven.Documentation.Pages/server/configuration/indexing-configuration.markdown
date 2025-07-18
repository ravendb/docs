# Configuration: Indexing
---

{NOTE: }

* The below **indexing configuration keys** can be modified via any of the following methods:
    * As explained in the [Config overview](../../server/configuration/configuration-options) article
    * Set a custom configuration per index from the [Client API](../../indexes/creating-and-deploying#customizing-configuration)
    * Set a custom configuration per index from the [Studio](../../studio/database/indexes/create-map-index#configuration)

{NOTE/}

{NOTE: }

* In this article:  
  * Server-wide scope:  
    [Indexing.CleanupIntervalInMin](../../server/configuration/indexing-configuration#indexing.cleanupintervalinmin)  
    [Indexing.GlobalScratchSpaceLimitInMb](../../server/configuration/indexing-configuration#indexing.globalscratchspacelimitinmb)  
    [Indexing.MaxNumberOfConcurrentlyRunningIndexes](../../server/configuration/indexing-configuration#indexing.maxnumberofconcurrentlyrunningindexes)  
    [Indexing.MaxTimeToWaitAfterFlushAndSyncWhenExceedingScratchSpaceLimitInSec](../../server/configuration/indexing-configuration#indexing.maxtimetowaitafterflushandsyncwhenexceedingscratchspacelimitinsec)  
    [Indexing.NuGetAllowPreReleasePackages](../../server/configuration/indexing-configuration#indexing.nugetallowprereleasepackages)  
    [Indexing.NuGetPackageSourceUrl](../../server/configuration/indexing-configuration#indexing.nugetpackagesourceurl)  
    [Indexing.NuGetPackagesPath](../../server/configuration/indexing-configuration#indexing.nugetpackagespath)  
    [Indexing.QueryClauseCache.ExpirationScanFrequencyInSec](../../server/configuration/indexing-configuration#indexing.queryclausecache.expirationscanfrequencyinsec)  
    [Indexing.QueryClauseCache.RepeatedQueriesCount](../../server/configuration/indexing-configuration#indexing.queryclausecache.repeatedqueriescount)  
    [Indexing.QueryClauseCache.SizeInMb](../../server/configuration/indexing-configuration#indexing.queryclausecache.sizeinmb)  
  * Server-wide, or database scope:  
    [Indexing.Auto.ArchivedDataProcessingBehavior](../../server/configuration/indexing-configuration#indexing.auto.archiveddataprocessingbehavior)  
    [Indexing.Auto.DeploymentMode](../../server/configuration/indexing-configuration#indexing.auto.deploymentmode)  
    [Indexing.Auto.SearchEngineType](../../server/configuration/indexing-configuration#indexing.auto.searchenginetype)  
    [Indexing.Disable](../../server/configuration/indexing-configuration#indexing.disable)  
    [Indexing.DisableQueryOptimizerGeneratedIndexes](../../server/configuration/indexing-configuration#indexing.disablequeryoptimizergeneratedindexes)  
    [Indexing.ErrorIndexStartupBehavior](../../server/configuration/indexing-configuration#indexing.errorindexstartupbehavior)  
    [Indexing.History.NumberOfRevisions](../../server/configuration/indexing-configuration#indexing.history.numberofrevisions)  
    [Indexing.IndexStartupBehavior](../../server/configuration/indexing-configuration#indexing.indexstartupbehavior)  
    [Indexing.ResetMode](../../server/configuration/indexing-configuration#indexing.resetmode)  
    [Indexing.RunInMemory](../../server/configuration/indexing-configuration#indexing.runinmemory)  
    [Indexing.SkipDatabaseIdValidationOnIndexOpening](../../server/configuration/indexing-configuration#indexing.skipdatabaseidvalidationonindexopening)  
    [Indexing.Static.ArchivedDataProcessingBehavior](../../server/configuration/indexing-configuration#indexing.static.archiveddataprocessingbehavior)  
    [Indexing.Static.DeploymentMode](../../server/configuration/indexing-configuration#indexing.static.deploymentmode)  
    [Indexing.Static.RequireAdminToDeployJavaScriptIndexes](../../server/configuration/indexing-configuration#indexing.static.requireadmintodeployjavascriptindexes)  
    [Indexing.TempPath](../../server/configuration/indexing-configuration#indexing.temppath)  
    [Indexing.TimeBeforeDeletionOfSupersededAutoIndexInSec](../../server/configuration/indexing-configuration#indexing.timebeforedeletionofsupersededautoindexinsec)  
    [Indexing.TimeToWaitBeforeDeletingAutoIndexMarkedAsIdleInHrs](../../server/configuration/indexing-configuration#indexing.timetowaitbeforedeletingautoindexmarkedasidleinhrs)  
    [Indexing.TimeToWaitBeforeMarkingAutoIndexAsIdleInMin](../../server/configuration/indexing-configuration#indexing.timetowaitbeforemarkingautoindexasidleinmin)  
  * Server-wide, or database, or per index:  
    [Indexing.AllowStringCompilation](../../server/configuration/indexing-configuration#indexing.allowstringcompilation)  
    [Indexing.Analyzers.Default](../../server/configuration/indexing-configuration#indexing.analyzers.default)  
    [Indexing.Analyzers.Exact.Default](../../server/configuration/indexing-configuration#indexing.analyzers.exact.default)  
    [Indexing.Analyzers.Search.Default](../../server/configuration/indexing-configuration#indexing.analyzers.search.default)  
    [Indexing.Corax.DocumentsLimitForCompressionDictionaryCreation](../../server/configuration/indexing-configuration#indexing.corax.documentslimitforcompressiondictionarycreation)  
    [Indexing.Corax.IncludeDocumentScore](../../server/configuration/indexing-configuration#indexing.corax.includedocumentscore)  
    [Indexing.Corax.IncludeSpatialDistance](../../server/configuration/indexing-configuration#indexing.corax.includespatialdistance)  
    [Indexing.Corax.MaxAllocationsAtDictionaryTrainingInMb](../../server/configuration/indexing-configuration#indexing.corax.maxallocationsatdictionarytraininginmb)  
    [Indexing.Corax.MaxMemoizationSizeInMb](../../server/configuration/indexing-configuration#indexing.corax.maxmemoizationsizeinmb)  
    [Indexing.Corax.Static.ComplexFieldIndexingBehavior](../../server/configuration/indexing-configuration#indexing.corax.static.complexfieldindexingbehavior)  
    [Indexing.Corax.UnmanagedAllocationsBatchSizeLimitInMb](../../server/configuration/indexing-configuration#indexing.corax.unmanagedallocationsbatchsizelimitinmb)  
    [Indexing.Encrypted.TransactionSizeLimitInMb](../../server/configuration/indexing-configuration#indexing.encrypted.transactionsizelimitinmb)  
    [Indexing.IndexEmptyEntries](../../server/configuration/indexing-configuration#indexing.indexemptyentries)  
    [Indexing.IndexMissingFieldsAsNull](../../server/configuration/indexing-configuration#indexing.indexmissingfieldsasnull)  
    [Indexing.Lucene.Analyzers.NGram.MaxGram](../../server/configuration/indexing-configuration#indexing.lucene.analyzers.ngram.maxgram)  
    [Indexing.Lucene.Analyzers.NGram.MinGram](../../server/configuration/indexing-configuration#indexing.lucene.analyzers.ngram.mingram)  
    [Indexing.Lucene.IndexInputType](../../server/configuration/indexing-configuration#indexing.lucene.indexinputtype)  
    [Indexing.Lucene.LargeSegmentSizeToMergeInMb](../../server/configuration/indexing-configuration#indexing.lucene.largesegmentsizetomergeinmb)  
    [Indexing.Lucene.MaximumSizePerSegmentInMb](../../server/configuration/indexing-configuration#indexing.lucene.maximumsizepersegmentinmb)  
    [Indexing.Lucene.MaxTimeForMergesToKeepRunningInSec](../../server/configuration/indexing-configuration#indexing.lucene.maxtimeformergestokeeprunninginsec)  
    [Indexing.Lucene.MergeFactor](../../server/configuration/indexing-configuration#indexing.lucene.mergefactor)  
    [Indexing.Lucene.NumberOfLargeSegmentsToMergeInSingleBatch](../../server/configuration/indexing-configuration#indexing.lucene.numberoflargesegmentstomergeinsinglebatch)  
    [Indexing.Lucene.ReaderTermsIndexDivisor](../../server/configuration/indexing-configuration#indexing.lucene.readertermsindexdivisor)  
    [Indexing.Lucene.UseCompoundFileInMerging](../../server/configuration/indexing-configuration#indexing.lucene.usecompoundfileinmerging)  
    [Indexing.ManagedAllocationsBatchSizeLimitInMb](../../server/configuration/indexing-configuration#indexing.managedallocationsbatchsizelimitinmb)  
    [Indexing.MapBatchSize](../../server/configuration/indexing-configuration#indexing.mapbatchsize)  
    [Indexing.MapTimeoutAfterEtagReachedInMin](../../server/configuration/indexing-configuration#indexing.maptimeoutafteretagreachedinmin)  
    [Indexing.MapTimeoutInSec](../../server/configuration/indexing-configuration#indexing.maptimeoutinsec)  
    [Indexing.MaxStepsForScript](../../server/configuration/indexing-configuration#indexing.maxstepsforscript)  
    [Indexing.MaxTimeForDocumentTransactionToRemainOpenInSec](../../server/configuration/indexing-configuration#indexing.maxtimefordocumenttransactiontoremainopeninsec)  
    [Indexing.MaxTimeToWaitAfterFlushAndSyncWhenReplacingSideBySideIndexInSec](../../server/configuration/indexing-configuration#indexing.maxtimetowaitafterflushandsyncwhenreplacingsidebysideindexinsec)  
    [Indexing.Metrics.Enabled](../../server/configuration/indexing-configuration#indexing.metrics.enabled)   
    [Indexing.MinNumberOfMapAttemptsAfterWhichBatchWillBeCanceledIfRunningLowOnMemory](../../server/configuration/indexing-configuration#indexing.minnumberofmapattemptsafterwhichbatchwillbecanceledifrunninglowonmemory)   
    [Indexing.MinimumTotalSizeOfJournalsToRunFlushAndSyncWhenReplacingSideBySideIndexInMb](../../server/configuration/indexing-configuration#indexing.minimumtotalsizeofjournalstorunflushandsyncwhenreplacingsidebysideindexinmb)
    [Indexing.NumberOfConcurrentStoppedBatchesIfRunningLowOnMemory](../../server/configuration/indexing-configuration#indexing.numberofconcurrentstoppedbatchesifrunninglowonmemory)   
    [Indexing.NumberOfLargeSegmentsToMergeInSingleBatch](../../server/configuration/indexing-configuration#indexing.numberoflargesegmentstomergeinsinglebatch)  
    [Indexing.OrderByScoreAutomaticallyWhenBoostingIsInvolved](../../server/configuration/indexing-configuration#indexing.orderbyscoreautomaticallywhenboostingisinvolved)  
    [Indexing.OrderByTicksAutomaticallyWhenDatesAreInvolved](../../server/configuration/indexing-configuration#indexing.orderbyticksautomaticallywhendatesareinvolved)  
    [Indexing.QueryClauseCache.Disabled](../../server/configuration/indexing-configuration#indexing.queryclausecache.disabled)  
    [Indexing.QueryClauseCache.RepeatedQueriesTimeFrameInSec](../../server/configuration/indexing-configuration#indexing.queryclausecache.repeatedqueriestimeframeinsec)  
    [Indexing.ScratchSpaceLimitInMb](../../server/configuration/indexing-configuration#indexing.scratchspacelimitinmb)  
    [Indexing.Static.SearchEngineType](../../server/configuration/indexing-configuration#indexing.static.searchenginetype)  
    [Indexing.Throttling.TimeIntervalInMs](../../server/configuration/indexing-configuration#indexing.throttling.timeintervalinms)  
    [Indexing.TimeSinceLastQueryAfterWhichDeepCleanupCanBeExecutedInMin](../../server/configuration/indexing-configuration#indexing.timesincelastqueryafterwhichdeepcleanupcanbeexecutedinmin)  
    [Indexing.TransactionSizeLimitInMb](../../server/configuration/indexing-configuration#indexing.transactionsizelimitinmb)  

{NOTE/}

---

{PANEL: Indexing.CleanupIntervalInMin}

Time (in minutes) between auto-index cleanup.

- **Type**: `int`
- **Default**: `10`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Indexing.GlobalScratchSpaceLimitInMb}

* Maximum amount of scratch space in megabytes that we allow to use for all index storages per server.

* After exceeding this limit the indexes will complete their current indexing batches and force flush and sync storage environments.

---

- **Type**: `int`
- **Default**: `null` (no limit)
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Indexing.MaxNumberOfConcurrentlyRunningIndexes}

Set how many indexes can run concurrently on the server to prevent overwhelming system resources and slow indexing.

- **Type**: `int`
- **Default**: `null` (No limit)
- **MinValue**: 1
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Indexing.MaxTimeToWaitAfterFlushAndSyncWhenExceedingScratchSpaceLimitInSec}

Max time to wait in seconds when forcing the storage environment flush and sync after exceeding the scratch space limit.

- **Type**: `int`
- **Default**: `30`
- **Scope**: Server-wide only
- **Alias:** `Indexing.MaxTimeToWaitAfterFlushAndSyncWhenExceedingScratchSpaceLimit`

{PANEL/}

{PANEL: Indexing.NuGetAllowPreReleasePackages}

Allow installation of NuGet prerelease packages.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only
- **Alias:** `Indexing.NuGetAllowPreleasePackages`

{PANEL/}

{PANEL: Indexing.NuGetPackageSourceUrl}

Default NuGet source URL.

- **Type**: `string`
- **Default**: `https://api.nuget.org/v3/index.json`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Indexing.NuGetPackagesPath}

Location of NuGet packages cache.

- **Type**: `string`
- **Default**: `Packages/NuGet`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Indexing.QueryClauseCache.ExpirationScanFrequencyInSec}

EXPERT ONLY:  
The frequency by which to scan the query clause cache for expired values.

- **Type**: `int`
- **Default**: `180`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Indexing.QueryClauseCache.RepeatedQueriesCount}

EXPERT ONLY:  
The number of recent queries that we will keep to identify repeated queries, relevant for caching.

- **Type**: `int`
- **Default**: `512`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Indexing.QueryClauseCache.SizeInMb}

EXPERT ONLY:

* Maximum size that the query clause cache will utilize for caching partial query clauses,  
  defaulting to 10% of the system memory on 64-bit machines.

* The default value, which is determined based on your platform details, is set by the constructor of class `IndexingConfiguration`.

---

- **Type**: `int`
- **Default**: `DefaultValueSetInConstructor`
- **Scope**: Server-wide only

{PANEL/}

{PANEL: Indexing.Auto.ArchivedDataProcessingBehavior}

The default processing behavior for archived documents in auto-indexes.

- **Type**: `enum ArchivedDataProcessingBehavior`:
    * `ExcludeArchived`: only non-archived documents are processed by the index.
    * `IncludeArchived`: both archived and non-archived documents are processed by the index.
    * `ArchivedOnly`: only archived documents are processed by the index.
- **Default**: `ExcludeArchived`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL: Indexing.Auto.DeploymentMode}

Set the default deployment mode for auto indexes.

- **Type**: `enum IndexDeploymentMode` (`Parallel`, `Rolling`)
- **Default**: `Parallel`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL: Indexing.Auto.SearchEngineType}

Set the search engine to be used with auto-indexes.

- **Type**: `enum SearchEngineType` (`Lucene`, `Corax`)
- **Default**: `Lucene`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL: Indexing.Disable}

Set whether to disable all indexes in the database.  
All indexes in the database will be disabled when set to `true`.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL: Indexing.DisableQueryOptimizerGeneratedIndexes}

EXPERT ONLY:  
Disable query optimizer generated indexes (auto-indexes). Dynamic queries will not be supported.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL: Indexing.ErrorIndexStartupBehavior}

Set how faulty indexes should behave on database startup when they are loaded.  
By default they are not started.

- **Type**: `enum ErrorIndexStartupBehaviorType` (`Default`, `Start`, `ResetAndStart`)
- **Default**: `Default`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL:Indexing.History.NumberOfRevisions}

Number of index history revisions to keep per index.

- **Type**: `int`
- **Default**: `10`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL: Indexing.IndexStartupBehavior}

* Set how indexes should behave on database startup when they are loaded.  
  By default they are started immediately.

* Setting this param can prevent slow index startup behavior in scenarios where many indexes open and start processing concurrently, which may cause IO usage to max out system resources.

---

- **Type**: `enum IndexStartupBehaviorType` (`Default`, `Immediate`, `Pause`, `Delay`)
- **Default**: `Default`
- **Scope**: Server-wide, or per database

Optional values:

- `Default`: Each index starts as soon as it is loaded.
- `Immediate`: Same as Default.
- `Pause`: Loads all indexes, but they are paused until manually started.
- `Delay`: Delays starting index processes until all indexes are loaded.

{PANEL/}

{PANEL: Indexing.ResetMode}

The default mode of the index reset operation.

- **Type**: `enum IndexResetMode` (`InPlace`, `SideBySide`)
- **Default**: `InPlace`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL: Indexing.RunInMemory}

* Set if indexes should run purely in memory.

* When running in memory:
    * No index data is written to the disk, and if the server is restarted, all index data will be lost.
    * Note that the index definition itself is kept on disk and remains unaffected by server restarts.
    * This is mostly useful for testing or faster, non-persistent indexing.

* If _Indexing.RunInMemory_ is not set explicitly,  
  then this configuration key will take the value of the core configuration key [RunInMemory](../../server/configuration/core-configuration#runinmemory).

---

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide, or per database

Optional values:

* `true` - indexing is run only in memory
* `false` - the index data is stored on disk

{PANEL/}

{PANEL: Indexing.SkipDatabaseIdValidationOnIndexOpening}

EXPERT ONLY:  
Allow to open an index without checking if current Database ID matched the one for which index was created.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide, per database

{PANEL/}

{PANEL: Indexing.Static.ArchivedDataProcessingBehavior}

* Set the default processing behavior for archived documents in static indexes.
* This setting applies only to static indexes that use _Documents_ as their data source.  
  It does not apply to indexes based on _Time Series_ or _Counters_, which default to `IncludeArchived`.

---

- **Type**: `enum ArchivedDataProcessingBehavior`:
    * `ExcludeArchived`: only non-archived documents are processed by the index.
    * `IncludeArchived`: both archived and non-archived documents are processed by the index.
    * `ArchivedOnly`: only archived documents are processed by the index.
- **Default**: `ExcludeArchived`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL: Indexing.Static.DeploymentMode}

Set the default deployment mode for static indexes.

- **Type**: `enum IndexDeploymentMode` (`Parallel`, `Rolling`)
- **Default**: `Parallel`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL: Indexing.Static.RequireAdminToDeployJavaScriptIndexes}

Require database `Admin` [clearance](../../server/security/authorization/security-clearance-and-permissions) to deploy [JavaScript indexes](../../indexes/javascript-indexes).

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL: Indexing.TempPath}

* Use this setting to specify a different path for the indexes' temporary files.

* By default, temporary files are created under the `Temp` directory inside the index data directory.  
  Learn more about RavenDB directory structure [here](../../server/storage/directory-structure).

---

- **Type**: `string`
- **Default**: `null`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL: Indexing.TimeBeforeDeletionOfSupersededAutoIndexInSec}

Set the number of seconds to keep a superseded auto index.

- **Type**: `int`
- **Default**: `15`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL: Indexing.TimeToWaitBeforeDeletingAutoIndexMarkedAsIdleInHrs}

Set the number of hours the database should wait before deleting an auto-index that is marked as idle.

- **Type**: `int`
- **Default**: `72`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL: Indexing.TimeToWaitBeforeMarkingAutoIndexAsIdleInMin}

Set the number of minutes to wait before marking an auto index as idle.

- **Type**: `int`
- **Default**: `30`
- **Scope**: Server-wide, or per database

{PANEL/}

{PANEL: Indexing.AllowStringCompilation}

* When defining a [JavaScript index](../../indexes/javascript-indexes),  
  this option determines whether the JavaScript engine is allowed to compile code from strings at runtime,
  using constructs such as `eval(...)` or `new Function(arg1, arg2, ..., functionBody)`.

* A `JavaScriptException` is thrown if this option is disabled and such a construct is used.

---

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL:Indexing.Analyzers.Default}

[Default analyzer](../../indexes/using-analyzers#ravendb) that will be used for fields.

- **Type**: `string`
- **Default**: `LowerCaseKeywordAnalyzer`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL:Indexing.Analyzers.Exact.Default}

[Default analyzer](../../indexes/using-analyzers#ravendb) that will be used for exact fields.

- **Type**: `string`
- **Default**: `KeywordAnalyzer`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.Analyzers.Search.Default}

[Default analyzer](../../indexes/using-analyzers#ravendb) that will be used for search fields.

- **Type**: `string`
- **Default**: `RavenStandardAnalyzer`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.Corax.DocumentsLimitForCompressionDictionaryCreation}

Corax index compression max documents used for dictionary creation.

- **Type**: `int`
- **Default**: `100_000`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.Corax.IncludeDocumentScore}

Include score value in the metadata when sorting by score.  
Disabling this option could enhance query performance.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.Corax.IncludeSpatialDistance}

Include spatial information in the metadata when sorting by distance.  
Disabling this option could enhance query performance.

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.Corax.MaxAllocationsAtDictionaryTrainingInMb}

EXPERT ONLY:

* The maximum amount of megabytes that we'll allocate for training indexing dictionaries.

* The default value, which is determined based on your platform details, is set by the constructor of class `IndexingConfiguration`.

---

- **Type**: `int`
- **Default**: `DefaultValueSetInConstructor`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.Corax.MaxMemoizationSizeInMb}

The maximum amount of memory in megabytes that Corax can use for memoization during query processing.

- **Type**: `int`
- **Default**: `512`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.Corax.Static.ComplexFieldIndexingBehavior}

* Set Corax's [default behavior](../../indexes/search-engine/corax#if-corax-encounters-a-complex-property-while-indexing)
  when a static index is requested to index a complex JSON object.

    * `CoraxComplexFieldIndexingBehavior.Throw` -  
      Corax will throw a `NotSupportedInCoraxException` exception.
    * `CoraxComplexFieldIndexingBehavior.Skip` -  
      Corax will skip indexing the complex field without throwing an exception.

---

- **Type**: `enum CoraxComplexFieldIndexingBehavior` (`Throw`, `Skip`)
- **Default**: `Throw`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.Corax.UnmanagedAllocationsBatchSizeLimitInMb}

* The maximum amount of unmanaged memory (in MB) that Corax can allocate during a single indexing batch.  
  When this limit is reached, the batch completes and indexing continues in a new batch.

* The default value is set by the constructor of the `IndexingConfiguration` class and depends on the environment:
    * If the machine is running in a 32-bit environment,  
      or if RavenDB is explicitly configured to use a 32-bit pager on a 64-bit system, the default is `128 MB`.
    * In all other cases (i.e., standard 64-bit environments), the default is `2048 MB`.

---

- **Type**: `int`
- **Default**: `DefaultValueSetInConstructor`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL:Indexing.Encrypted.TransactionSizeLimitInMb}

* Transaction size limit in megabytes for _encrypted_ databases, after which an index will stop and complete the current batch.

* The default value, which is determined based on your platform details, is set by the constructor of class `IndexingConfiguration`.

---

- **Type**: `int`
- **Default**: `DefaultValueSetInConstructor`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.IndexEmptyEntries}

* Set how the indexing process should handle documents that are missing fields.

* When set to `true`, the indexing process will index documents even if they lack the fields that are supposed to be indexed.

---

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.IndexMissingFieldsAsNull}

* Set how the indexing process should handle fields that are missing.

* When set to `true`, missing fields will be indexed with a `null` value.

---

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.Lucene.Analyzers.NGram.MaxGram}

* This configuration applies only to the Lucene indexing engine.

* Largest n-gram to generate when NGram analyzer is used.

---

- **Type**: `int`
- **Default**: `6`
- **Scope**: Server-wide, or per database, or per index
- **Alias:** `Indexing.Analyzers.NGram.MaxGram`

{PANEL/}

{PANEL: Indexing.Lucene.Analyzers.NGram.MinGram}

* This configuration applies only to the Lucene indexing engine.

* Smallest n-gram to generate when NGram analyzer is used.

---

- **Type**: `int`
- **Default**: `2`
- **Scope**: Server-wide, or per database, or per index
- **Alias:** `Indexing.Analyzers.NGram.MinGram`

{PANEL/}

{PANEL: Indexing.Lucene.IndexInputType}

Lucene index input

- **Type**: `enum LuceneIndexInputType` (`Standard`, `Buffered`)
- **Default**: `Buffered`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.Lucene.LargeSegmentSizeToMergeInMb}

EXPERT ONLY:

* This configuration applies only to the Lucene indexing engine.

* The definition of a large segment in MB.  
  We won't merge more than [Indexing.NumberOfLargeSegmentsToMergeInSingleBatch](../../server/configuration/indexing-configuration#indexing.numberoflargesegmentstomergeinsinglebatch) in a single batch.

* The default value, which is determined based on your platform details, is set by the constructor of class `IndexingConfiguration`.

---

- **Type**: `int`
- **Default**: `DefaultValueSetInConstructor`
- **Scope**: Server-wide, or per database, or per index
- **Alias:** `Indexing.LargeSegmentSizeToMergeInMb`

{PANEL/}

{PANEL: Indexing.Lucene.MaximumSizePerSegmentInMb}

EXPERT ONLY:

* This configuration applies only to the Lucene indexing engine.

* The maximum size in MB that we'll consider for segments merging.

* The default value, which is determined based on your platform details, is set by the constructor of class `IndexingConfiguration`.

---

- **Type**: `int`
- **Default**: `DefaultValueSetInConstructor`
- **Scope**: Server-wide, or per database, or per index
- **Alias:** `Indexing.MaximumSizePerSegmentInMb`

{PANEL/}

{PANEL: Indexing.Lucene.MaxTimeForMergesToKeepRunningInSec}

EXPERT ONLY:

* This configuration applies only to the Lucene indexing engine.

* How long will we let merges to run before we close the transaction.

---

- **Type**: `int`
- **Default**: `15`
- **Scope**: Server-wide, or per database, or per index
- **Alias:** `Indexing.MaxTimeForMergesToKeepRunningInSec`

{PANEL/}

{PANEL: Indexing.Lucene.MergeFactor}

EXPERT ONLY:

* This configuration applies only to the Lucene indexing engine.

* Set how often index segments are merged into larger ones.  
  The merge process will start when the number of segments in an index reaches this number.

* With smaller values, less RAM is used while indexing, and searches on unoptimized indexes are faster, but indexing speed is slower.

---

- **Type**: `int`
- **Default**: `10`
- **Scope**: Server-wide, or per database, or per index
- **Alias:** `Indexing.MergeFactor`

{PANEL/}

{PANEL: Indexing.Lucene.NumberOfLargeSegmentsToMergeInSingleBatch}

EXPERT ONLY:

* This configuration applies only to the Lucene indexing engine.

* Number of large segments defined by [Indexing.LargeSegmentSizeToMergeInMb](../../server/configuration/indexing-configuration#indexing.largesegmentsizetomergeinmb) to merge in a single batch.

---

- **Type**: `int`
- **Default**: `2`
- **Scope**: Server-wide, or per database, or per index
- **Alias:** `Indexing.NumberOfLargeSegmentsToMergeInSingleBatch`

{PANEL/}

{PANEL: Indexing.Lucene.ReaderTermsIndexDivisor}

EXPERT ONLY:  
Control how many terms we'll keep in the cache for each field.  
Higher values reduce the memory usage at the expense of increased search time for each term.

- **Type**: `int`
- **Default**: `1`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.Lucene.UseCompoundFileInMerging}

EXPERT ONLY:

* This configuration applies only to the Lucene indexing engine.

* Use compound file in merging.

---

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide, or per database, or per index
- **Alias:** `Indexing.UseCompoundFileInMerging`

{PANEL/}

{PANEL: Indexing.ManagedAllocationsBatchSizeLimitInMb}

Managed allocations limit in an indexing batch after which the batch will complete and an index will continue by starting a new one.

- **Type**: `int`
- **Default**: `2048`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.MapBatchSize}

Maximum number of documents to be processed by the index per indexing batch.

- **Type**: `int?`
- **Default**: `null` (no limit)
- **MinValue**: `128`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.MapTimeoutAfterEtagReachedInMin}

* Number of minutes after which mapping will end even if there is more to map.

* This will only be applied if we pass the last etag we saw in the collection when the batch was started.

---

- **Type**: `int`
- **Default**: `15`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL:Indexing.MapTimeoutInSec}

Number of seconds after which mapping will end even if there is more to map.  
Using the default value of `-1` will map everything possible in a single batch.

- **Type**: `int`
- **Default**: `-1`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.MaxStepsForScript}

The maximum number of steps in the script execution of a JavaScript index.

- **Type**: `int`
- **Default**: `10_000`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.MaxTimeForDocumentTransactionToRemainOpenInSec}

Set how many seconds indexing will keep document transaction open when indexing.  
When triggered, transaction will be closed and a new one will be opened.

- **Type**: `int`
- **Default**: `15`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.MaxTimeToWaitAfterFlushAndSyncWhenReplacingSideBySideIndexInSec}

Max time to wait when forcing the storage environment flush and sync when replacing side-by-side index.

- **Type**: `int`
- **Default**: `30`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.Metrics.Enabled}

Set whether indexing performance metrics will be gathered.

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.MinNumberOfMapAttemptsAfterWhichBatchWillBeCanceledIfRunningLowOnMemory}

EXPERT ONLY:  
Set minimum number of map attempts after which the batch will be canceled if running low on memory.

- **Type**: `int`
- **Default**: `512`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.MinimumTotalSizeOfJournalsToRunFlushAndSyncWhenReplacingSideBySideIndexInMb}

Minimum total size of journals to run flush and sync when replacing side by side index in megabytes.

- **Type**: `int`
- **Default**: `512`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.NumberOfConcurrentStoppedBatchesIfRunningLowOnMemory}

EXPERT ONLY:  
Number of concurrent stopped batches if running low on memory.

- **Type**: `int`
- **Default**: `2`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.OrderByScoreAutomaticallyWhenBoostingIsInvolved}

Set whether query results will be automatically ordered by score when a boost factor is involved in the query.  
(A boost factor may be [assigned inside an index definition](../../indexes/boosting) or can be [applied at query time](../../client-api/session/querying/text-search/boost-search-results)).

- **Type**: `bool`
- **Default**: `true`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.OrderByTicksAutomaticallyWhenDatesAreInvolved}

Sort by ticks when field contains dates.  
When sorting in descending order, null dates are returned at the end with this option enabled.

- **Type**: `bool`
- **Default**: `true`
  {NOTE: }
  **Note** that the default value for this configuration key has changed in version 6.0 from `false` to `true` 
  {NOTE/}
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.QueryClauseCache.Disabled}

EXPERT ONLY:

* Disable the query clause cache for a server, database, or a single index.

* The default value is set by the constructor of class `IndexingConfiguration`.  
  It will be `true` if your core configuration key [Features.Availability](../../server/configuration/core-configuration#features.availability) is Not set to 'Experimental'.

---

- **Type**: `bool`
- **Default**: `DefaultValueSetInConstructor`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.QueryClauseCache.RepeatedQueriesTimeFrameInSec}

EXPERT ONLY:  
Queries that repeat within this time frame will be considered worth caching.

- **Type**: `int`
- **Default**: `300`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.ScratchSpaceLimitInMb}

* Amount of scratch space in megabytes that we allow to use for the index storage.

* After exceeding this limit the current indexing batch will complete and the index will force flush and sync storage environment.

---

- **Type**: `int`
- **Default**: `null` (no limit)
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.Static.SearchEngineType}

Set the search engine to be used with static indexes.

- **Type**: `enum SearchEngineType` (`Lucene`, `Corax`)
- **Default**: `Lucene`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.Throttling.TimeIntervalInMs}

How long the index should delay processing after new work is detected in milliseconds.

- **Type**: `int`
- **Default**: `null`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.TimeSinceLastQueryAfterWhichDeepCleanupCanBeExecutedInMin}

Set how many minutes to wait before deep cleaning an idle index.  
Deep cleanup reduces the cost of idle indexes.  
It might slow the first query after the deep cleanup, thereafter queries return to normal performance.

- **Type**: `int`
- **Default**: `10`
- **Scope**: Server-wide, or per database, or per index

{PANEL/}

{PANEL: Indexing.TransactionSizeLimitInMb}

Transaction size limit in megabytes after which an index will stop and complete the current batch.

- **Type**: `int`
- **Default**: `null` (no limit)
- **Scope**: Server-wide, or per database, or per index

{PANEL/}
