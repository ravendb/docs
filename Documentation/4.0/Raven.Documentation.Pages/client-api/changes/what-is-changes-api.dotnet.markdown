# Changes API 

The RavenDB client offers a push notification feature that allows you to receive messages from a server about events that occurred there.
You are able to subscribe to events for all documents, indexes, and operations as well as to indicate a particular one that you are interested in. 
This mechanism lets you notify users if something has changed without the need to do any expensive polling. 

{DANGER:Changes API on Secured Server}

Changes API uses WebSockets under the covers. Due to [the lack of support for client certificates in WebSockets implementation in .NET Core 2.0](https://github.com/dotnet/corefx/issues/5120#issuecomment-348557761)
the Changes API won't work for secured servers accessible over HTTPS.

This issue will be fixed in the final version of .NET Core 2.1 (Q2 2018). The fix is already available in [daily builds of .NET Core 2.1](https://github.com/dotnet/core-setup#daily-builds). 
In order to workaround this you can switch your application to use .NET Core 2.1 (build `2.1.0-preview2-26308-01` or newer). 

The issue affects only the RavenDB client, the server can be running on .NET Core 2.0.
{DANGER/}

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
- [ForDocumentsStartingWith](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsstartingwith)
- [ForIndex](../../client-api/changes/how-to-subscribe-to-index-changes#forindex)
- [ForOperationId](../../client-api/changes/how-to-subscribe-to-operation-changes#foroperation)

## Unsubscribing

In order to end subscription (stop listening for particular notifications) you must `Dispose` it.

{CODE changes_2@ClientApi\Changes\WhatIsChangesApi.cs /}

## Remarks

{NOTE One or more open Changes API connections will prevent a database from becoming idle and unloaded, regardless of [configuration value for database idle timeout](../../server/configuration/database-configuration#databases.maxidletimeinsec) /}

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions Core](https://www.nuget.org/packages/System.Reactive.Core/) package to your project. /}

## Related Articles

### Changes API

- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)
