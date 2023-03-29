﻿# Sharding: Unsupported Features
---

{NOTE: }

* A sharded RavenDB database generally provides the same services that 
  a non-sharded database offers, so clients of older versions and non-sharded 
  database are supported and existing queries, subscriptions, patches, 
  and so on, require no modification.  
* Find below a list of yet unimplemented features, that are currently 
  supported by non-sharded RavenDB databases but not by sharded ones.  
  
* In this page:  
   * [Unsupported Features](../sharding/unsupported#unsupported-features)  
      * [Unsupported Indexing Features](../sharding/unsupported#unsupported-indexing-features)  
      * [Unsupported Querying Features](../sharding/unsupported#unsupported-querying-features)  
      * [Unsupported Document Extensions Features](../sharding/unsupported#unsupported-document-extensions-features)  
      * [Unsupported Backup Features](../sharding/unsupported#unsupported-backup-features)  
      * [Unsupported Import & Export Features](../sharding/unsupported#unsupported-import--export-features)  
      * [Unsupported Migration Features](../sharding/unsupported#unsupported-migration-features)  
      * [Unsupported Subscriptions Features](../sharding/unsupported#unsupported-subscriptions-features)  
      * [Unsupported Integrations Features](../sharding/unsupported#unsupported-integrations-features)  
      * [Unsupported Patching Features](../sharding/unsupported#unsupported-patching-features)  
      * [Unsupported Replication Features](../sharding/unsupported#unsupported-replication-features)  
  
{NOTE/}

---
{PANEL: Unsupported Features}

## Unsupported Indexing Features

| Unsupported Feature | Comment | 
| ------------- | ------------- | 
| [Rolling index deploymeny](../indexes/rolling-index-deployment) |  | 
| [Load Document from another shard](../sharding/indexing#unsupported-indexing-features) | Loading a document during indexing is possible only if the document resides on the shard. | 
| **Map-Reduce Output Documents** | Using [OutputReduceToCollection](../indexes/map-reduce-indexes#map-reduce-output-documents) to output the results of a map-reduce index to a collection is not supported in a Sharded Database. | 
| [Custom Sorters](../indexes/querying/sorting#creating-a-custom-sorter) |  | 
| **Testing Javascript indexes** |  | 

## Unsupported Querying Features

| Unsupported Feature | Comment | 
| ------------- | ------------- | 
| [Load Document from another shard](../sharding/indexing#unsupported-indexing-features) | An index or a query can only load a document if it resides on the same shard. | 
| [Load Document within a map-reduce projection](../sharding/querying#projection) |  | 
| **Stream Map-Reduce results** | [Streaming](../client-api/session/querying/how-to-stream-query-results#stream-an-index-query) map-reduce results is not supported in a Sharded Database. | 
| Use `limit` with [PatchByQueryOperation](../client-api/operations/patching/set-based#patchbyqueryoperation) or [DeleteByQueryOperation](../client-api/operations/delete-by-query) | [Unsupported Querying Features](../sharding/querying#unsupported-querying-features) | 
| [MoreLikeThis](../client-api/session/querying/how-to-use-morelikethis) |  | 
| **Order by score** |  | 

## Unsupported Document Extensions Features

| Unsupported Feature | Comment | 
| ------------- | ------------- | 
| **Move Attachments** | E.g. `session.Advanced.Attachments.Move("users/1","foo","users/2","bar");` is not supported. | 
| **Copy Attachments** | E.g. `session.Advanced.Attachments.Copy("users/1","foo","users/2","bar");` is not supported. | 
| **Get multiple Attachments** | E.g. `session.Advanced.Attachments.Get(attachmentNames)` is not supported. | 
| **Copy Time Series** | E.g. `session.Advanced.Defer(new CopyTimeSeriesCommandData(id,  "Count", id2, "Count"));` is not supported. | 

## Unsupported Backup Features

| Unsupported Feature | Comment | 
| ------------- | ------------- | 
| [Create a Snapshot Backup](../sharding/backup-and-restore/backup#backup-type) |  | 
| [Restore from a Snapshot Backup](../sharding/backup-and-restore/restore#sharding-restore) |  | 

## Unsupported Import & Export Features

| Unsupported Feature | Comment | 
| ------------- | ------------- | 
| [Import from a CSV file](../studio/database/tasks/import-data/import-from-csv) |  | 
| **Import from an S3 Bucket** | using GET, Studio, smuggler, import s3 dir | 


## Unsupported Migration Features

| Unsupported Feature | Comment | 
| ------------- | ------------- | 
| [Migrate from RavenDB](../studio/database/tasks/import-data/import-from-ravendb) | By POST, Studio, smuggler | 
| [Migrate from SQL DB](../studio/database/tasks/import-data/import-from-sql) |  | 

## Unsupported Subscriptions Features

| Unsupported Feature | Comment | 
| ------------- | ------------- | 
| [Concurrent Subscriptions](../client-api/data-subscriptions/concurrent-subscriptions) |  | 
| [Data Subscriptions Revisions Support](../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning) | Subscribing to document revisions | 
| [SubscriptionCreationOptions.ChangeVector](../sharding/subscriptions#unsupported-features) | Providing a change vector to start the processing from <br> except for special cases: `"LastDocument"`, `"BeginningOfTime"` |

## Unsupported Integrations Features

| Unsupported Unsupported Feature | Comment | 
| ------------- | ------------- | 
| [PostgreSQL](../integrations/postgresql-protocol/overview) |  | 
| [Queue ETL](../server/ongoing-tasks/etl/queue-etl/overview) | [Kafka](../server/ongoing-tasks/etl/queue-etl/kafka), [RabbitMQ](../server/ongoing-tasks/etl/queue-etl/rabbit-mq) | 

## Unsupported Patching Features

| Unsupported Feature | Comment | 
| ------------- | ------------- | 
| [JSON patch](../client-api/operations/patching/json-patch-syntax) |  | 

## Unsupported Replication Features

| Unsupported Feature | Comment | 
| ------------- | ------------- | 
| [Filtered Replication](../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview#filtered-replication) |  | 
| [Hub/Sink Replication](../studio/database/tasks/ongoing-tasks/hub-sink-replication/overview) |  | 
| **Legacy replication** | From RavenDB 3.x instances | 

{PANEL/}

## Related articles

**Sharding**  
[Overview](../sharding/overview)  
[Indexing](../sharding/indexing)  
[Querying](../sharding/querying)  
[Document Extensions](../sharding/document-extensions)  
[Backup](../sharding/backup-and-restore/backup)  
[Restore](../sharding/backup-and-restore/restore)  
[Import & Export](../sharding/import-and-export)  
[External Replication](../sharding/external-replication)  
[ETL](../sharding/etl)  
[Subscriptions](../sharding/subscriptions)  
