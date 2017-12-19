# Changes in DocumentStore

This article describes the changes in public API of `DocumentStore`.

## Listeners

All listeners have been removed in favour of events.

#### Recommended changes

- Usage of the events:   

{CODE events_1@Migration\ClientApi\DocumentStoreChanges.cs /}   

- Document conflicts are solved only on the server side using conflict resolution scripts defined in a database.

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
