# Glossary : DatabaseStatistics

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
| **Indexes** | IndexStats[] | Indexes statistics |
| **Errors** | IndexingError[] | Indexing errors |
| **IndexingBatchInfo** | IndexingBatchInfo[] | Information about indexing batch |
| **Prefetches** | [FutureBatchStats](../glossary/database-statistics#futurebatchstats)[] | Details about prefetching |
| **DatabaseId** | Guid | Database identifier |
| **SupportsDtc** | bool | whether database supports DTC |

//TODO: indexstats, indexingerror, indexing batch info ??


# FutureBatchStats

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Timestamp** | DateTime | Timestamp of operation |
| **Duration** | TimeSpan? | Operation duration |
| **Size** | int? | Amount of documents used during prefetching |
| **Retries** | int | Retries count |
| **PrefetchingUser** | PrefetchingUser | `Indexer` = 1, `Replicator` = 2,  `SqlReplicator` = 3 |
