# Glossary: DatabaseStatistics

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **LastDocEtag** | Etag | The Etag of last document |
| **LastAttachmentEtag** | Etag | The Etag of last attachment |
| **CountOfIndexes** | int | Amount of indexes |
| **CountOfResultTransformers** | int | Amount of result transformers |
| **InMemoryIndexingQueueSizes** | int[] | In memory indexing queue sizes |
| **ApproximateTaskCount** | long | Approximate amount of tasks |
| **CountOfDocuments** | long | Amount of documents |
| **CountOfAttachments** | long | Amount of attachments |
| **StaleIndexes** | string[] | List of stale indexes names |
| **CurrentNumberOfItemsToIndexInSingleBatch** | int | Amount of items to index in single batch |
| **CurrentNumberOfItemsToReduceInSingleBatch** | int | Amount of items to reduce in single batch |
| **DatabaseTransactionVersionSizeInMB** | decimal | Size of database transactional version in MB |
| **Indexes** | [IndexStats](../glossary/database-statistics#indexstats)[] | Indexes statistics |
| **Errors** | [IndexingError](../glossary/database-statistics#indexingerror)[] | Indexing errors |
| **IndexingBatchInfo** | [IndexingBatchInfo](../glossary/database-statistics#indexingbatchinfo)[] | Information about indexing batch |
| **Prefetches** | [FutureBatchStats](../glossary/database-statistics#futurebatchstats)[] | Details about prefetching |
| **DatabaseId** | Guid | Database identifier |
| **SupportsDtc** | bool | whether database supports DTC |

<hr />

# IndexStats

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Id** | int | Index id |
| **Name** | string | Index name |
| **IndexingAttempts** | int | Indexing attempts |
| **IndexingSuccesses** | int | Amount of indexing successes |
| **IndexingErrors** | int | Amount of indexing errors |
| **LastIndexedEtag** | Etag | Last indexed etag |
| **IndexingLag** | int? | Estimated amount of document changes since last indexing |
| **LastIndexedTimestamp** | DateTime | Time of last indexing |
| **LastQueryTimestamp** | DateTime? | Time of last query to this index |
| **TouchCount** | int | Touch count |
| **Priority** | IndexingPriority | Indexing priority |
| **ReduceIndexingAttempts** | int? | Amount of index reduce attempts |
| **ReduceIndexingSuccesses** | int? | Amount of index reduce successes |
| **ReduceIndexingErrors** | int? | Amount of index reduce errors |
| **LastReducedEtag** | Etag | Last reduced etag |
| **LastReducedTimestamp** | DateTime? | Time of last reduce operation |
| **CreatedTimestamp** | DateTime | Date of index creation |
| **LastIndexingTime** | DateTime | Date of last indexing |
| **IsOnRam** | string | Whether index is stored on RAM. If `true` it also prints memory usage |
| **LockMode** | IndexLockMode | Index lock mode |
| **ForEntityName** | List&lt;string&gt; | Entities names referenced by this index |
| **Performance** | IndexingPerformanceStats[] | Index performance statistics |
| **DocsCount** | int | Amount of documents in this index |
| **IsInvalidIndex** | bool | Whether index is invalid |

<hr />

# IndexingError

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Id** | long | Error id |
| **Index** | int | Index id |
| **IndexName** | string | Index name |
| **Error** | string | Error message |
| **Timestamp** | DateTime | Time of error |
| **Document** | string | Document source |
| **Action** | string | Action during which an error occured |

<hr />

# IndexingBatchInfo

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **TotalDocumentCount** | int | Total document count |
| **TotalDocumentSize** | long | Total document size |
| **Timestamp** | DateTime | Date of event |


<hr />

# FutureBatchStats

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Timestamp** | DateTime | Timestamp of operation |
| **Duration** | TimeSpan? | Operation duration |
| **Size** | int? | Amount of documents used during prefetching |
| **Retries** | int | Retries count |
| **PrefetchingUser** | PrefetchingUser | `Indexer` = 1, `Replicator` = 2,  `SqlReplicator` = 3 |
