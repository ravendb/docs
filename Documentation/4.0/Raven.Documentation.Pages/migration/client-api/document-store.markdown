# Changes in DocumentStore

This article describes the changes in public API of `DocumentStore`.

## Namespace

`IDocumentStore` is can be referenced using `Raven.Client.Documents` (previously `Raven.Client`).

## DefaultDatabase

`DefaultDatabase` property has been renamed to `Database`.

## Url and FailoverServers

When initializing `DocumentStore` you can provide multiple URLs to RavenDB servers holding your database in the cluster. It will grab the cluster topology from the first accessible server. 

{CODE urls_1@Migration\ClientApi\DocumentStoreChanges.cs /}  

## ConnectionStringName

As the configuration system has been changed in .NET Core we removed `ConnectionStringName` property. Instead you can use .NET core configuration mechanism, retrieve the connection string
entry from `appsettings.json`, convert it and manually set `Urls` and `Database` properties.

## Listeners

All listeners have been removed in favour of events.

#### Recommended changes

- Usage of the events:   

{CODE events_1@Migration\ClientApi\DocumentStoreChanges.cs /}   

- Document conflicts are solved only on the server side using conflict resolution scripts defined in a database.

## JsonRequestFactory

Instead of `JsonRequestFactory` `IDocumentStore` instance has `RequestExecutor`. Using it you can:

  - send any command:

{CODE request_executor_1@Migration\ClientApi\DocumentStoreChanges.cs /}   

  - check the number of cached items or clear the cache (cache size capacity can be configured via `MaxHttpCacheSize` convention):

{CODE request_executor_2@Migration\ClientApi\DocumentStoreChanges.cs /}   

  - check the number of sent requests:

{CODE request_executor_3@Migration\ClientApi\DocumentStoreChanges.cs /}   

- change default request timeout:

{CODE request_executor_4@Migration\ClientApi\DocumentStoreChanges.cs /}  

## Conventions

All conventions needs to be set before `DocumentStore.Initialize` is called. Otherwise `InvalidOperationException` will be thrown.

### Entity serialization and deserialization

Serialization and deserialization of entities can be customized using:

- `CustomizeJsonSerializer`
- `DeserializeEntityFromBlittable`

{CODE serialization_1@Migration\ClientApi\DocumentStoreChanges.cs /}   

In order to customize bulk insert serialization you can use the following conventions:

- `BulkInsert.TrySerializeEntityToJsonStream`
- `BulkInsert.TrySerializeMetadataToJsonStream`

{CODE serialization_2@Migration\ClientApi\DocumentStoreChanges.cs /}   

### DocumentKeyGenerator

`DocumentKeyGenerator` has been removed. Use `AsyncDocumentIdGenerator` instead.


### DefaultQueryingConsistency

`DefaultQueryingConsistency` convention has been removed. Check related article discussing [how to deal with non stale results](../../indexes/stale-indexes).


### FindTypeTagName and FindDynamicTagName

Renamed to `FindCollectionName` and `FindCollectionNameForDynamic`.


### IdentityTypeConvertors

It hass been removed. Only string identifiers are supported.


## FailoverBehavior

The client will failover automatically to different node in the cluster. You can customize the reads by using `ReadBalanceBehavior` convention.


