# Changes API 

The RavenDB client offers a push notification feature that allows you to receive messages from a server about events that occurred there.
You are able to subscribe to events for all documents, indexes, and operations as well as to indicate a particular one that you are interested in. 
This mechanism lets you notify users if something has changed without the need to do any expensive polling. 

## Accessing Changes API

The changes subscription is accessible by a document store through its `IDatabaseChanges` interface.

{CODE:java changes_1@ClientApi\Changes\WhatIsChangesApi.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | String | Name of database to open changes API for. If `null`, the `Database` configured in DocumentStore will be used. |

| Return value | |
| ------------- | ----- |
| IDatabaseChanges | Instance implementing IDatabaseChanges interface. |

## Connection interface

`IDatabaseChanges` inherits from `IConnectableChanges<TChanges>` interface that represent the connection.

{CODE:java connectable_changes@ClientApi\Changes\IConnectableChanges.java /}

## Subscriptions

In order to retrieve notifications you have to subscribe to server-side events by using one of the following methods:

- [forAllDocuments](../../client-api/changes/how-to-subscribe-to-document-changes#foralldocuments)
- [forAllIndexes](../../client-api/changes/how-to-subscribe-to-index-changes#forallindexes)
- [forAllOperations](../../client-api/changes/how-to-subscribe-to-operation-changes#foralloperations)
- [forDocument](../../client-api/changes/how-to-subscribe-to-document-changes#fordocument)
- [forDocumentsInCollection](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsincollection)
- [forDocumentsStartingWith](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsstartingwith)
- [forIndex](../../client-api/changes/how-to-subscribe-to-index-changes#forindex)
- [forOperationId](../../client-api/changes/how-to-subscribe-to-operation-changes#foroperation)

## Unsubscribing

In order to end subscription (stop listening for particular notifications) you must `close` it.

{CODE:java changes_2@ClientApi\Changes\WhatIsChangesApi.java /}

## Remarks

{NOTE One or more open Changes API connections will prevent a database from becoming idle and unloaded, regardless of [configuration value for database idle timeout](../../server/configuration/database-configuration#databases.maxidletimeinsec) /}

## Related Articles

### Changes API

- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)
