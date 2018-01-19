# Changes in Conventions

All conventions needs to be set before `DocumentStore.Initialize` is called.   
Otherwise, an `InvalidOperationException` will be thrown.

## Namespace

`Raven.Client.Document.DocumentConvention` has been renamed and moved to `Raven.Client.Documents.Conventions.DocumentConventions`.

## Entity Serialization and Deserialization

Serialization and deserialization of entities can be customized using:

- `CustomizeJsonSerializer`
- `DeserializeEntityFromBlittable`

{CODE serialization_1@Migration\ClientApi\Conventions.cs /}   

In order to customize bulk insert serialization you can use `BulkInsert.TrySerializeEntityToJsonStream` convention:

{CODE serialization_2@Migration\ClientApi\Conventions.cs /}   

## DocumentKeyGenerator

The `DocumentKeyGenerator` has been removed. Use `AsyncDocumentIdGenerator` instead.

## DefaultQueryingConsistency

`DefaultQueryingConsistency` convention has been removed. Check the related article discussing [how to deal with non stale results](../../indexes/stale-indexes).

## DefaultUseOptimisticConcurrency

The `DefaultUseOptimisticConcurrency` has been renamed to `UseOptimisticConcurrency`.

## FindIdentityPropertyNameFromEntityName

The `FindIdentityPropertyNameFromEntityName` has been renamed to `FindIdentityPropertyNameFromCollectionName`.

## FindTypeTagName and FindDynamicTagName

Renamed to `FindCollectionName` and `FindCollectionNameForDynamic`.

## IdentityTypeConvertors

It has been removed. Only string identifiers are supported.

## FailoverBehavior

The client will failover automatically to different node in the cluster. You can customize the reads by using the `ReadBalanceBehavior` convention.

## EnlistInDistributedTransactions

The support for DTC transactions has been dropped.

## UseParallelMultiGet

Removed. The multi gets are processed in async manner on the server side.

## TransformTypeTagNameToDocumentKeyPrefix

Renamed to `TransformTypeCollectionNameToDocumentIdPrefix`.

## RegisterIdConvention

Renamed to `RegisterAsyncIdConvention`

## RegisterIdLoadConvention

Removed. Use `RegisterAsyncIdConvention`.

## ReplicationInformerFactory

Removed. Check [How Client Integrates With Replication and Cluster](../../client-api/cluster/how-client-integrates-with-replication-and-cluster) article.

## ShouldAggressiveCacheTrackChanges and ShouldSaveChangesForceAggressiveCacheCheck

The client listens to changes by default and evicts cached items if they change on the server side.

## IndexAndTransformerReplicationMode

Removed. Indexes are automatically replicated within the cluster. Transformers has been removed completely.

## RequestTimeSlaThresholdInMilliseconds

Use `ReadBalanceBehavior.FastestNode` to if you need to ensure that get requests will executed against the fastest node in the cluster.

## TimeToWaitBetweenReplicationTopologyUpdates

Removed. Check [How Client Integrates With Replication and Cluster](../../client-api/cluster/how-client-integrates-with-replication-and-cluster) article.
