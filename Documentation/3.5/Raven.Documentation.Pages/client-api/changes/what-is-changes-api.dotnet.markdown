# Changes API: What is Changes API?

The RavenDB client offers a push notification feature that allows you to receive messages from a server about events that occurred there.
You are able to subscribe to events for all documents or indexes as well as to indicate a particular one that you are interested in. 
This mechanism lets you notify users if something has changed without the need to do any expensive polling. 

## Accessing Changes API

The changes subscription is accessible by a document store. Depending on the type of the store you use (`DocumentStore`, `ShardedDocumentStore` or `EmbeddableDocumentStore`) you will get an appropriate instance which is an implementation of a common `IDatabaseChanges` interface.

{CODE changes_1@ClientApi\Changes\WhatIsChangesApi.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | string | Name of the database to open changes API for. If `null`, the default database configured in DocumentStore will be used. |

| Return value | |
| ------------- | ----- |
| IDocumentChanges | Instance implementing IDocumentChanges interface appropriate to store type. |

## Connection Properties

`IDatabaseChanges` has three properties that are related to the server connection:

{CODE-BLOCK:csharp}
// represents the task responsible for establishing connection with the server
Task Task { get; }

// returns state of the connection
bool Connected { get; }

// the event raised if a connection state is changed
event EventHandler ConnectionStatusChanged
{CODE-BLOCK/}

## Subscriptions

In order to retrieve notifications, you have to subscribe to server-side events by using one of the following methods:

- [ForAllDocuments](../../client-api/changes/how-to-subscribe-to-document-changes#foralldocuments)
- [ForAllIndexes](../../client-api/changes/how-to-subscribe-to-index-changes#forallindexes)
- [ForAllReplicationConflicts](../../client-api/changes/how-to-subscribe-to-replication-conflicts)
- [ForAllTransformers](../../client-api/changes/how-to-subscribe-to-transformer-changes)
- [ForBulkInsert](../../client-api/changes/how-to-subscribe-to-bulk-insert-operation-changes)
- [ForDocument](../../client-api/changes/how-to-subscribe-to-document-changes#fordocument)
- [ForDocumentsInCollection](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsincollection)
- [ForDocumentsOfType](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsoftype)
- [ForDocumentsStartingWith](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsstartingwith)
- [ForIndex](../../client-api/changes/how-to-subscribe-to-index-changes#forindex)
- [ForAllDataSubscriptions](../../client-api/changes/how-to-subscribe-to-data-subscription-changes)

## Unsubscribing

In order to end a subscription (stop listening for particular notifications), you must `Dispose` it.

{CODE changes_2@ClientApi\Changes\WhatIsChangesApi.cs /}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions](https://www.nuget.org/packages/Rx-Main) package to your project. /}

