# Changes API : What is Changes API?

The RavenDB client offers a push notification feature that allows you to receive messages from a server about events that occurred there.
You are able to subscribe to events for all documents, indexes and operations as well as to indicate a particular one that you are interested in. 
This mechanism lets you notify users if something has changed without the need to do any expensive polling. 

## Accessing Changes API

The changes subscription is accessible by a document store through its `IDatabaseChanges` interface.

{CODE changes_1@ClientApi\Changes\WhatIsChangesApi.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | string | Name of database to open changes API for. If `null`, the `Database` configured in DocumentStore will be used. |

| Return value | |
| ------------- | ----- |
| IDatabaseChanges | Instance implementing IDatabaseChanges interface. |

## Connection interface

`IDatabaseChanges` inherits from `IConnectableChanges<TChanges>` interface that represent the connection.

{CODE connectable_changes@ClientApi\Changes\IConnectableChanges.cs /}

## Subscriptions

In order to retrieve notifications you have to subscribe to server-side events by using one of the following methods:

- [ForAllDocuments](../../client-api/changes/how-to-subscribe-to-document-changes#foralldocuments)
- [ForAllIndexes](../../client-api/changes/how-to-subscribe-to-index-changes#forallindexes)
- [ForAllOperations](../../client-api/changes/how-to-subscribe-to-operation-changes#foralloperations)
- [ForDocument](../../client-api/changes/how-to-subscribe-to-document-changes#fordocument)
- [ForDocumentsInCollection](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsincollection)
- [ForDocumentsOfType](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsoftype)
- [ForDocumentsStartingWith](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsstartingwith)
- [ForIndex](../../client-api/changes/how-to-subscribe-to-index-changes#forindex)
- [ForOperationId](../../client-api/changes/how-to-subscribe-to-operation-changes#foroperation)

## Unsubscribing

In order to end subscription (stop listening for particular notifications) you must `Dispose` it.

{CODE changes_2@ClientApi\Changes\WhatIsChangesApi.cs /}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions Core](https://www.nuget.org/packages/System.Reactive.Core/) package to your project. /}


