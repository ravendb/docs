# What's new

{PANEL:3.0.3800 - 2015/09/21}

### Server

- improved formatted index generation with better error handling,
- fixed issue with single OAuth tokens caching,
- larger batches are now handles better for Map-Reduce indexes,
- added support for HEAD request for streams,
- fixed `ArgumentOutOfRangeException` that could occur during reading from Lucene index

### File systems

- better handling of larger files with longer names

<hr />

### Client API

- fixed issue with saving documents to proper database in ShardedBulkInsert operation,
- fixed issues with index generation,
- IndexCreation now takes into account conventions,
- added the option to specify timeout of a subscriptions pull request

<hr />

### Studio

- fixed replication topology graph
- fixed replication settings page

{/PANEL}

{PANEL:3.0.3785 - 2015/08/31}

### Server

- `[Voron]` increased scratch buffer size to 6144 MB and added a threshold after which indexing/reducing batch sizes will start decreasing,
- `[Voron]` map/reduce optimizations. We have done major work to optimize how RavenDB uses map/reduce on Voron. As a result, map/reduce performance on Voron has improved tremendously. However, this require a migration step during the first startup,
- `[Voron]` optimized recovery code heavily to support slow I/O systems on large databases,
- Changed shutdown sequence - each database / file system waits up to 3 seconds to complete existing requests before they get aborted,
- Fixed creation of future batches (prefetching mechanism),
- Changed index priority does not force index reset,
- Handled failures of index resets,
- Fixed loading of startup tasks when hosted in IIS,
- Fixed `Lucene.Net` to properly dispose files in out of disk space scenario,
- Fixed `Lucene.Net` memory allocation on queries. We have drastically reduced the amount of memory that is allocated per query, and improved the performance of queries substantially
- Better handling of buffer allocations in websockets, reduces memory fragmentation,
- Better handling of Take() / Skip() inside an index
- Many small perf optimizations, memory allocations reductions, object pooling, etc. Drastic reduction in memory allocations on common code paths,
- Allow only a single index to use the fast precomputation optimization at a time (reduce memory usage if multiple medium sized indexes are changed concurrently),
- Re-implemented memory statistics checks using native calls to avoid expensive allocations,
- Provide more detailed information when an index is corrupted,
- Adding endpoint for stopping/starting just reduce work. More [here](../../http/client-api/commands/how-to/start-stop-reducing),
- Less aggressive changes to the batch size at scale, being more cautious gives us a bit slower perf but more stable system under load,
- Fixed side-by-side index updates,
- Allowed to update side-by-side index when it is still running,
- Fixed .NET 4.6 compilation errors,
- Fixed an NRE when the index definition was removed forcibly when using dynamic queries,
- Fixed error handling during disposal causing an exception to escape thread boundary and crashing,
- Fixed FIPS licensing issue on embedded databases,
- Fixed a finalizer usage bug causing us to try to read from a closed handle,
- Prevent corrupted index warning when creating a map-reduce index and indexing is disabled,
- Preventing code from trying to use disposed internal transactions,
- Properly dispose of timer instance when shutting down a database using expiration bundle,
- Prevent an error loading ICSharpCode.NRefactory from killing RavenDB client startup

#### [Configuration](../server/configuration/configuration-options)

- Increased `Raven/Voron/MaxScratchBufferSize` from 1024 to 6144. More [here](../server/configuration/configuration-options#voron-settings),
- Added `Raven/Voron/ScratchBufferSizeNotificationThreshold`. More [here](../server/configuration/configuration-options#voron-settings),
- Added `Raven/MaxClauseCount`. More [here](../server/configuration/configuration-options#index-settings),
- Added `Raven/Indexing/DisableIndexingFreeSpaceThreshold`. More [here](../server/configuration/configuration-options#index-settings)

### File systems

- Fixed file synchronization mechanism,
- Fixed files handling with `#` character in name

### Bundles

- `[Replication]` Fixed request buffering issues	

<hr />

### Client API

- Added `AbstractScriptedIndexCreationTask`. More [here](../server/bundles/scripted-index-results#example-ii---abstractscriptedindexcreationtask),
- Added `SetTransformerLock` command. More [here](../client-api/commands/transformers/how-to/change-transformer-lock-mode),
- Added `PutIndexes` command. More [here](../client-api/commands/indexes/put#putindexes),
- Added `Include<TResult>(Expression<Func<T, object>> path)` to async session,
- Implemented `GetMetadataForAsync<T>(T instance)` in advanced options of async session of `ShardedDocumentStore`,
- `WithinRadiusOf` marked as obsolete in spatial querying because of the parameter order inconsistency. `WithinRadius` is designated to be used instead. More [here](../indexes/querying/spatial),
- Added `StartEtag` to `SubscriptionCriteria`. More [here](../client-api/data-subscriptions/how-to-create-data-subscription),
- Added opening strategies to data subscriptions. More [here](../client-api/data-subscriptions/how-to-open-data-subscription),
- Added `BeforeAcknowledgment` and `AfterAcknowledgment` events to data subscription. More [here](../client-api/data-subscriptions/events),
- Added "Query parsing" measure for `ShowTimings` query customization. More [here](../client-api/session/querying/how-to-customize-query#showtimings),
- Added `TransformerLockMode`. More [here](../client-api/commands/transformers/how-to/change-transformer-lock-mode),
- Added `Load` overload with transformer to `ILoaderWithInclude`. More [here](../client-api/session/loading-entities#example-iii-1),
- `IndexCreation.CreateIndexes` creates indexes in a single request,
- `DocumentStore.SideBySideExecuteIndexes` and `DocumentStore.SideBySideExecuteIndexesAsync` creates side by side indexes in a single request,
- Implemented bulk inserts for `ShardedDocumentStore`,
- Optimized memory allocation and better performance in [profiling](../client-api/how-to/enable-profiling),
- Fixed implementations of sync methods to avoid hangs,
- Fixed caching of `HttpClient`,
- Extended IEnumerable implementation of `DynamicList` - more available extensions in an index definition

<hr />

### Studio

- Environment based studio themes. More [here](../studio/management/studio-config),
- Added `Status -> Debug -> Currently indexing`,
- Added IO Test. More [here](../studio/management/io-test),
- Added License server information. More [here](../studio/management/license-information),
- Fixed authentication by API keys,
- Fixed inconsistency bug in Query intellisense,
- Exposed option StoreAllFields (Edit index view),
- Support for pre 3.0 versioning documents

<hr />

### Installer

- Added options to check port availability and revoke URL reservation according to provided port number when installing on IIS,
- Added support for IIS 10 detection on Windows 10

### Smuggler

- Fixed import of conflicted documents

### Tools

- Added Traffic recorder and simulator tool

{PANEL/}

{PANEL:3.0.3690 - 2015/05/22}

### Server

- `[JavaScript]` Added `IncreaseNumberOfAllowedStepsBy` method. More [here](../client-api/commands/patches/how-to-use-javascript-to-patch-your-documents#methods-objects-and-variables),
- `[JavaScript]` Debug information now contains number of steps that script took,
- `[Voron]` Less aggresive disk space allocation,
- Various performance improvements

#### [Configuration](../server/configuration/configuration-options)

- Added `Raven/AllowScriptsToAdjustNumberOfSteps`. More [here](../server/configuration/configuration-options#javascript-parser),
- Added `Raven/Voron/AllowOn32Bits`. More [here](../server/configuration/configuration-options#voron-settings),
- Added `Raven/PreventSchemaUpdate`. More [here](../server/configuration/configuration-options#data-settings).

#### Bundles

- `[SQL Replication]` Adding new replication will not force others to wait till it catches up with them

<hr />

### Studio

- Patching now displays metadata,
- Added the ability to force side-by-side index replacement,
- Added the ability to create C# class from JSON document,
- Various fixes and enhancements

<hr />

### Client API

- added `ToFacetsLazyAsync` extension method to `IQueryable`,
- conflicts can be automatically resolved by Client API during query operations if there is `IDocumentConflictListener` available

<hr />

### Installer

- installer now contains `NLog.Ignore.config` for easier logging setup

{PANEL/}

{PANEL:3.0.3660 - 2015/04/07}

### Global

- Various performance optimizations across both server and client

<hr />

### Server

- `[JavaScript]` Parser now returns more descriptive errors,
- `[JavaScript]` `PutDocument` method now returns Id of generated document,
- `[JavaScript]` Each `LoadDocument` increases maximum number of steps in script using following formula `MaxSteps = MaxSteps + (MaxSteps / 2 + (SerializedSizeOfDocumentOnDisk * AdditionalStepsPerSize))`,
- Added `debug/raw-doc` endpoint,
- Prevented high CPU and excessive GC runs under low memory conditions,
- Avoid leaking resources when failing to create a database,
- Faster JSON serialization and deserialization,
- Added backoff strategy for failing periodic exports,
- Recognize Windows users with admin rights to system database as server admins,
- Facets can now have very large number of facets

#### [Configuration](../server/configuration/configuration-options)

- Added `Raven/WorkingDir`. More [here](../server/configuration/configuration-options#data-settings),
- Added `Raven/AdditionalStepsForScriptBasedOnDocumentSize` (5 by default). More [here](../server/configuration/configuration-options#javascript-parser),
- Added `Raven/MaxServicePointIdleTime`. More [here](../server/configuration/configuration-options#http-settings),
- Added `Raven/ImplicitFetchFieldsFromDocumentMode`. More [here](../server/configuration/configuration-options#index-settings),
- Added `Raven/Replication/ForceReplicationRequestBuffering`. More [here](../server/configuration/configuration-options#replication)

#### Indexes

- [`AbstractIndexCreationTask`](../indexes/creating-and-deploying#using-abstractindexcreationtask) will add sorting to numerical fields automatically

#### Bundles

- `[Periodic Export]` Added support for remote folders for Amazon S3 and Microsoft Azure. Source [here](https://github.com/ravendb/ravendb/blob/build-3660/Raven.Abstractions/Data/PeriodicBackupSetup.cs#L45-L53),
- `[SQL Replication]` Renamed `PerformTableQuatation` to `QuoteTables` in `SqlReplicationConfig`. Source [here](https://github.com/ravendb/ravendb/blob/build-3660/Raven.Database/Bundles/SqlReplication/SqlReplicationConfig.cs#L23-L28),
- `[SQL Replication]` Added `Insert-only mode` for tables, which will prevent deletes on that table. Source [here](https://github.com/ravendb/ravendb/blob/build-3660/Raven.Database/Bundles/SqlReplication/SqlReplicationConfig.cs#L46),
- `[Replication]` Added support for index and transformer replication (including deletions). Source [here](https://github.com/ravendb/ravendb/blob/build-3660/Raven.Abstractions/Replication/ReplicationDestination.cs#L73)

<hr />

### Client API

- Indexes can be deployed side-by-side using `SideBySideExecute` from `AbstractIndexCreationTask`, `SideBySideCreateIndexes` from `IndexCreation` and directly from `DocumentStore` using `SideBySideExecuteIndex`,
- Added the ability to provide additional query to MoreLikeThis queries,
- Added `SetIndexLock` to `IDatabaseCommands`. More [here](../client-api/commands/indexes/how-to/change-index-lock-mode),
- Added `SetIndexPriority` to `IDatabaseCommands`. More [here](../client-api/commands/indexes/how-to/change-index-priority),
- Index priority can be set through `IndexPriority` property in `IndexDefinition` or `Priority` property in `AbstractIndexCreationTask`,

<hr />

### Smuggler

- Added the ability to disable versioning during smuggling using `disable-versioning-during-import` option

<hr />

### FileSystem

- Added support for @in queries (fixed the `WhereIn` method),
- Added `DeleteByQueryAsync` to `IAsyncFilesCommands`,
- Added `RegisterDeletionQuery` to `IAsyncFilesSession`,
- Added `RegisterResultsForDeletion` to `IAsyncFilesQuery`
- Deleted `progress` parameter of `UploadAsync` method in `IAsyncFilesCommands`,
- Renamed `StreamFilesAsync` to `StreamFileHeadersAsync` in `IAsyncFilesCommands`,
- Exposed `Import/Export` options in the Studio,
- Exposed synchronization settings in the Studio,
- Added concurrency checks support. Available by providing file Etags or enabling optimistic concurrency (added `DefaultUseOptimisticConcurrency` convention),
- Added `Take` and `Skip` methods to querying API,
- Fix: Registered files are tracked by session after `SaveChangesAsync` call,
- Fix: Metadata update operation creates a file revision when `Versioning Bundle` is enabled,
- Fix: Creating revisions of synchronized files when `Versioning Bundle` is enabled,
- Fix: File revisions are not synchronized to destination file systems,
- Added option `RenameOnReset` to `Versioning Bundle` configuration,
- Added ability to create `Versioning Bundle` configuration for a specific directory,
- Added `AbstractSynchronizationTrigger` trigger,
- Added querying support for numeric metadata fields,
- Renamed `SynchronizeAsync` to `StartAsync` in `IAsyncFilesSynchronizationCommands`,
- Added support for smuggling RavenFS configurations

{PANEL/}

{PANEL:3.0.3599 - 2015/02/08}

### Server

- preventing, by default, unrestricted access (`Raven/AnonymousAccess` set to `Admin`) to server when license is used. More [here](../server/configuration/license-registration),
- `[Voron]` added compaction,
- added Data Subscriptions,
- added _admin/low-memory-notification_ endpoint,
- performance improvements

#### [Configuration](../server/configuration/configuration-options)

- added `Raven/Indexing/MaxNumberOfItemsToProcessInTestIndexes`,
- added `Raven/Licensing/AllowAdminAnonymousAccessForCommercialUse`,
- added `Raven/IncrementalBackup/AlertTimeoutHours`,
- added `Raven/IncrementalBackup/RecurringAlertTimeoutDays`,
- added `Raven/NewIndexInMemoryMaxTime`,
- added `Raven/AssembliesDirectory`,
- added `Raven/Replication/IndexAndTransformerReplicationLatency`,
- added `Raven/MaxConcurrentRequestsForDatabaseDuringLoad`,
- added `Raven/Replication/MaxNumberOfItemsToReceiveInSingleBatch`,
- added `Raven/DynamicLoadBalancing`,
- added `Raven/ExposeConfigOverTheWire`

#### Indexes

- test indexes. More [here](../indexes/testing-indexes),
- side-by-side indexes. More [here](../indexes/side-by-side-indexes),
- added safe number parsing methods. More [here](../indexes/indexing-linq-extensions#parsing-numbers),
- added the ability to replicate index and transformer definitions.

#### Bundles

- `[Replication]` Added the ability to limit maximum received number of items in single replication batch using `Raven/Replication/MaxNumberOfItemsToReceiveInSingleBatch` setting,
- `[Replication]` Source server will take into account low-memory conditions on destination server and adjust batch size

<hr />

### Client API

- added `PreserveDocumentPropertiesNotFoundOnModel` convention. More [here](../client-api/configuration/conventions/request-handling#preservedocumentpropertiesnotfoundonmodel),
- **highlights** can be accessed when performing **projection** or querying **map-reduce** index. More [here](../indexes/querying/highlights#highlights--projections),
- added `IndexAndTransformerReplicationMode` convention that indicates if index and transformer definitions should be replicated when they are created using `AbstractIndexCreationTask` or `AbstractTransformerCreationTask`. More [here](../client-api/configuration/conventions/misc#indexandtransformerreplicationmode),
- added [Data Subscriptions](../client-api/data-subscriptions/what-are-data-subscriptions).

<hr />

### Studio

- more detailed _indexing performance chart_ available at `Status -> Indexing -> Indexing performance`,
- added the _persist auto index view_ available at `Status -> Debug -> Persist auto index`,
- added the _explain replication view_ available at `Status -> Debug -> Explain replication`,
- added CancellationToken support for various methods in client (e.g. in queries and commands),
- performance improvements

{PANEL/}

{PANEL:3.0.3525 - 2014/11/25}

### Server

- Voron - new [storage engine](../server/configuration/storage-engines),
- [RavenFS](../file-system/what-is-raven-fs),
- FIPS encryption support. More [here](../server/configuration/enabling-fips-compliant-encryption-algorithms),
- low memory notifications,
- simpler deployment (less files),
- using OWIN/Web API,
- added various [debug endpoints](../server/troubleshooting/debug-endpoints) and [metrics](../studio/overview/status/indexing/indexing-performance)

#### Bundles

- `[Periodic Export]` Support for full & incremental exports,
- `[Replication]` Default failover behavior for client can be defined server-side,
- `[Replication]` Default server-side conflict resolver can be defined,
- `[Replication]` Ability to generate replication topology. More [here](../studio/overview/status/replication-stats#replication-topology),
- `[SQL Replication]` Ability to define connection strings,
- `[SQL Replication]` Ability to use [custom functions](../studio/overview/settings/custom-functions),
- `[SQL Replication]` Added replication simulator,
- `[SQL Replication]` Added detailed metrics

#### Indexes

- indexing to memory - reducing number of I/O operations by flushing to disk only when certain threshold is passed,
- async index deletion,
- parallelized indexing & task execution,
- better large document handling,
- I/O bounded batching,
- attachment indexing (`LoadAttachmentForIndexing`),
- optimized new index creation,
- better control for Cartesian/fanout indexing,
- better auto-index generation,
- ...more details [here](http://ayende.com/blog/168417/what-is-new-in-ravendb-3-0-indexing-backend) and [here](http://ayende.com/blog/168418/what-is-new-in-ravendb-3-0-indexing-enhancements)

#### Transformers

- ability to nest transformers. More [here](../transformers/nesting-transformers)

<hr />

### Client API

- [Java API](http://ayende.com/blog/168354/what-is-new-in-ravendb-3-0-jvm-client-api),
- fully async (sync version is using async client underneath),
- full support for Lazy async,
- full support for Lazy timings,
- Embedded and Remote clients use the same code,
- administrative operations (both database and server-wide) were separated and can be found in `store.Commands.Admin` and `store.Commands.GlobalAdmin`,
- Bulk insert errors are handled immediately, not at the end of operation,
- Bulk insert detect document changes and skip updated if inserted documents are the same (this way documents will not have to be re-indexed or re-replicated),
- better memory management - less allocations,
- missing properties retention - if there are more properties in the document on server-side than in client-side, during update (load -> change -> save) all extra properties will be retained,
- added `WhatChanged` and `HasChanges` to session. More [here](../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- added `GetIndexMergeSuggestions`. More [here](../client-api/commands/indexes/how-to/get-index-merge-suggestions).
- patching (JavaScript) now supports [custom functions](../studio/overview/settings/custom-functions),
- optimistic concurrency can be turned on globally using `store.Conventions.DefaultUseOptimisticConcurrency`,
- `EmbeddedDocumentStore` was moved to `Raven.Database.dll`,
- ...more details [here](http://ayende.com/blog/168386/what-is-new-in-ravendb-3-0-client-side)

#### Querying

- renamed `LuceneQuery` to `DocumentQuery`,
- ability to return detailed query timings. More [here](../client-api/session/querying/how-to-customize-query#showtimings),
- ability to cancel long-running queries. More [here](../studio/overview/status/running-tasks),
- ability to explain query, by using [ExplainScores]() in `DocumentQuery`,
- `MoreLikeThisQuery` now supports includes. More [here](../client-api/session/how-to/use-morelikethis),

<hr />

### Smuggler

- Server to server smuggling. More [here](../server/administration/exporting-and-importing-data#moving-data-between-two-databases)

<hr />

### Studio

- HTML5 [Studio](../studio/accessing-studio),
- added various [debug endpoints](../server/troubleshooting/debug-endpoints) and [metrics](../studio/overview/status/index-stats-and-metrics),
- [Map-Reduce Visualizer](../studio/overview/status/map-reduce-visualizer),
- ability to view/kill [running tasks](../studio/overview/status/running-tasks),
- ability to create [debug info package](../studio/overview/status/gather-debug-info) that can be send with [support ticket](../server/troubleshooting/sending-support-ticket),
- ability to connect to [server logs](../studio/management/admin-logs),
- ability to view server traffic using [traffic watch](../studio/management/traffic-watch),
- added I/O performance test. More [here](../studio/management/io-test),
- ..and many more

{PANEL/}

