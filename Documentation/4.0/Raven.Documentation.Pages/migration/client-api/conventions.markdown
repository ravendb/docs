# Migration: Changes in Conventions

All conventions needs to be set before `DocumentStore.Initialize` is called.   
Otherwise, an `InvalidOperationException` will be thrown.

{PANEL:Namespace}

`Raven.Client.Document.DocumentConvention` has been renamed and moved to `Raven.Client.Documents.Conventions.DocumentConventions`.

{PANEL/}

{PANEL:Entity Serialization and Deserialization}

Serialization and deserialization of entities can be customized using:

- `CustomizeJsonSerializer`
- `DeserializeEntityFromBlittable`

{CODE serialization_1@Migration\ClientApi\Conventions.cs /}   

In order to customize bulk insert serialization you can use `BulkInsert.TrySerializeEntityToJsonStream` convention:

{CODE serialization_2@Migration\ClientApi\Conventions.cs /}   

{PANEL/}

{PANEL:DocumentKeyGenerator}

The `DocumentKeyGenerator` has been removed. Use `AsyncDocumentIdGenerator` instead.

{PANEL/}

{PANEL:DefaultQueryingConsistency}

`DefaultQueryingConsistency` convention has been removed. Check the related article discussing [how to deal with non stale results](../../indexes/stale-indexes).

{PANEL/}

{PANEL:DefaultUseOptimisticConcurrency}

The `DefaultUseOptimisticConcurrency` has been renamed to `UseOptimisticConcurrency`.

{PANEL/}

{PANEL:FindIdentityPropertyNameFromEntityName}

The `FindIdentityPropertyNameFromEntityName` has been renamed to `FindIdentityPropertyNameFromCollectionName`.

{PANEL/}

{PANEL:FindTypeTagName and FindDynamicTagName}

Renamed to `FindCollectionName` and `FindCollectionNameForDynamic`.

{PANEL/}

{PANEL:IdentityTypeConvertors}

It has been removed. Only string identifiers are supported.

{PANEL/}

{PANEL:FailoverBehavior}

The client will failover automatically to a different node in the cluster.  
The ***Read*** requests can be customized by using the [`ReadBalanceBehavior` convention](../../client-api/configuration/load-balance-and-failover).  

{PANEL/}

{PANEL:EnlistInDistributedTransactions}

The support for DTC transactions has been dropped.

{PANEL/}

{PANEL:UseParallelMultiGet}

Removed. The multi gets are processed in async manner on the server side.

{PANEL/}

{PANEL:TransformTypeTagNameToDocumentKeyPrefix}

Renamed to `TransformTypeCollectionNameToDocumentIdPrefix`.

{PANEL/}

{PANEL:RegisterIdConvention}

Renamed to `RegisterAsyncIdConvention`

{PANEL/}

{PANEL:RegisterIdLoadConvention}

Removed. Use `RegisterAsyncIdConvention`.

{PANEL/}

{PANEL:ReplicationInformerFactory}

Removed. Check [How Client Integrates With Replication and Cluster](../../client-api/cluster/how-client-integrates-with-replication-and-cluster) article.

{PANEL/}

{PANEL:ShouldAggressiveCacheTrackChanges and ShouldSaveChangesForceAggressiveCacheCheck}

The client listens to changes by default and evicts cached items if they change on the server side.

{PANEL/}

{PANEL:IndexAndTransformerReplicationMode}

Removed. Indexes are automatically replicated within the cluster. Transformers has been removed completely.

{PANEL/}

{PANEL:RequestTimeSlaThresholdInMilliseconds}

Use `ReadBalanceBehavior.FastestNode` to if you need to ensure that get requests will executed against the fastest node in the cluster.

{PANEL/}

{PANEL:TimeToWaitBetweenReplicationTopologyUpdates}

Removed. Check [How Client Integrates With Replication and Cluster](../../client-api/cluster/how-client-integrates-with-replication-and-cluster) article.

{PANEL/}

{PANEL:AllowQueriesOnId}

Removed. In 4.0 queries on IDs are supported by default.

{PANEL/}

{PANEL:ShouldCacheRequest}

Removed. There is no longer option to determine caching per request. Cache size can be controlled using `MaxHttpCacheSize` convention.

{PANEL/}


