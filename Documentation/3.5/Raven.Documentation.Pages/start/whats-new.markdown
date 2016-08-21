# What's new

{PANEL:Upcoming}

### Server

- `[Licensing]` HotSpare license support,
- `[Global Configuration]` Added feature,
- `[Clustering]` Added feature,
- `[ETL]` Added feature,
- `[Indexing]` Implemented a custom ThreadPool for better indexing thread management,
- `[Indexing]` Added support for AlphaNumeric sorting,
- `[Indexing]` Enhanced index recovery,
- `[Querying]` Increased Lucene Query parsing performance,
- `[Monitoring]` Added SNMP Monitoring support,
- `[Replication]` 'Raven/ConflictDocuments' and 'Raven/ConflictDocumentsTransformer' are now automatically deployed when replication is turned on,
- `[Transformer]` Added support for more than two SelectMany
- `[JavaScript]` Updated Jint to 2.8,
- `[JavaScript]` Updated LoDash to 4.13.1,
- `[Configuration]` Exposed `Raven/TempPath` setting,
- `[Configuration]` Exposed `Raven/Indexing/MaxNumberOfStoredIndexingBatchInfoElements` setting,
- `[Configuration]` Alert when a value is out of the expected range,
- `[Endpoint]` Moved `/debug/indexing-perf-stats` to /debug/indexing-perf-stats-with-timings
- `[Endpoint]` Changed `/debug/indexing-perf-stats`, gives a new output
- `[Endpoint]` Added `/admin/detailed-storage-breakdown`,
- `[Endpoint]` Added `/admin/activate-hotspare`,
- `[Endpoint]` Added `/admin/test-hotspare`,
- `[Endpoint]` Added `/admin/get-hotspare-information`,
- `[Endpoint]` Added `/admin/dump`,
- `[Periodic Export]` Support export over 64MB when using Azure Storage Containers,
- `[Data Subscription]` Deleting a subscription will kill the connection if it is active,
- `[Logging]` server metrics will be written to log periodically,
- `[Voron]` Performance improvements,
- `[NuGet]` Semantic versioning for NuGet packages,
- General performance improvements and bug fixes

<hr />

### Client

- `[Bulk Insert]` Can change format to BSON or JSON,
- `[Bulk Insert]` Can change compression to GZIP or None,
- Added SLA request time guarantees,
- Added new FailoverBehavior for handling Clustering scenarios,
- Added `PotentialShardsFor` overload to DefaultShardResolutionStrategy for easier configuration,
- Added `GetUserInfo` and `GetUserPermission` commands,
- Support for `SkipDuplicateChecking` in IndexQuery,
- Support for artificial documents on MoreLikeThis,
- `DeleteByIndex` now supports LINQ statement,
- `StreamDocs` now supports transformers,
- `[Save Changes]` Can wait for indexes to finish, read [here](../client-api/session/saving-changes) for more information,
- `[Save Changes]` Can wait for replication to finish, read [here](../server/scaling-out/replication/write-assurance) for more information

<hr />

### RavenFS

- `[Client]` Added `CopyAsync` and `RetryCopyingAsync` for server-side copy operation support,
- `[Client]` Added support for getting `FileQueryStatistics` when using `QueryAsync`,
- `[Client]` Unified FS creation method, now it matches the DB one,
- `[Client]` Added support for query streaming,
- `[Client]` Exposed file streaming under session.Advanced,

<hr />

## Smuggler

- Added `no-compression-on-import` option,
- Added `max-split-export-file-size` to enable splitting export to multiple files

<hr />

### Studio

- `[RavenFS]` Allow to configure synchronization settings,
- `[RavenFS]` Added the ability to Strip synchronization information from files metadata,
- `[RavenFS]` Added better search capabilities,
- `[Export/Import]` Added server-wide smuggling feature,
- `[Export/Import]` Added disk space verification before importing data (DB and FS),
- `[Export/Import]` When exporting database the equivalent Smuggler parameters will be shown,
- `[Export/Import]` Changed export file extensions (ravendbdump, ravenfsdump),
- `[Export/Import]` Added the ability to disable versioning bundle during import (DB and FS),
- `[Backup/Restore]` Better errors when backup operation have failed,
- `[Replication]` Added the ability to resolve all conflicts at once,
- `[Replication]` Added the ability to enable replication on an existing database,
- `[Replication]` Added server-wide replication topology view,
- `[Patching]` Patching now shows progress bar,
- `[Patching]` Added recent patches to Patching view,
- `[Patching]` Added ongoing patches to Patching view,
- `[Indexing]` Added the ability to change lock mode for all indexes at once,
- `[Indexing]` Added the ability to toggle reduction process,
- `[Indexing]` Added the ability to rename an index without re-running it,
- `[Indexing]` Indexing performance graph now contains prefetching time and deletions,
- Added a LINQ-based Data Exploration feature,
- Enhancements in CSV export (custom columns),
- Tasks in Tasks View now contain more descriptive errors,
- Added AdministratorJS Console,
- Added I/O performance statistics report (requires to run external tool - Raven.Monitor),
- Added debug view that allows to turn on Query Timing globally,
- Data Subscriptions debug view now contains more detailed information,
- Added license and support coverage information,
- Unified L&F,
- General performance improvements and bug fixes

<hr />

### Tools

- `[Raven.Monitor]` Added I/O performance statistics support,
- `[Raven.ApiToken]` Added tool to generate API Tokens,
- `[IndexCleaner]` Added tool that allows to reset all index related data at once during offline mode,
- `[SQL Replication]` Better support for complex data types when replicating to PgSQL

{PANEL/}

