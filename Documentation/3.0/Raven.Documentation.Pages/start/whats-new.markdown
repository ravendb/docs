# What's new

{PANEL:Upcoming}

### Server

- preventing, by default, unrestricted access (`Raven/AnonymousAccess` set to `Admin`) to server when license is used. More [here](../server/configuration/license-registration).

#### Configuration

- added `Raven/Indexing/MaxNumberOfItemsToProcessInTestIndexes`
- added `Raven/Licensing/AllowAdminAnonymousAccessForCommercialUse`
- added `Raven/IncrementalBackup/AlertTimeoutHours`,
- added `Raven/IncrementalBackup/RecurringAlertTimeoutDays`

#### Indexes

- test indexes. More [here](../indexes/testing-indexes),
- side-by-side indexes. More [here](../indexes/side-by-side-indexes),
- added safe number parsing methods. More [here](../indexes/indexing-linq-extensions#parsing-numbers),

<hr />

### Client API

- added `PreserveDocumentPropertiesNotFoundOnModel` convention. More [here](../client-api/configuration/conventions/request-handling#preservedocumentpropertiesnotfoundonmodel).
- **highlights** can be accessed when performing **projection** or querying **map-reduce** index. More [here](../indexes/querying/highlights#highlights--projections).

{PANEL/}

{PANEL:3.0.3525 - 2014/11/25}

### Server

- Voron - new [storage engine](../server/configuration/storage-engines),
- [RavenFS](http://ayende.com/blog/168323/what-is-new-in-ravendb-3-0-ravenfs),
- FIPS encryption support. More [here](../server/configuration/enabling-fips-compliant-encryption-algorithms),
- low memory notifications,
- simpler deployment (less files),
- using OWIN/Web API,
- added various [debug endpoints](../server/troubleshooting/debug-endpoints) and [metrics](../studio/overview/status/index-stats-and-metrics)

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

