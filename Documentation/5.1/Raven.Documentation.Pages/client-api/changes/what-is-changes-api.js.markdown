# Changes API 

The RavenDB client offers a push notification feature that allows you to receive messages from a server about events that occurred there.
You are able to subscribe to events for all documents, indexes, and operations as well as to indicate a particular one that you are interested in. 
This mechanism lets you notify users if something has changed without the need to do any expensive polling. 

{DANGER:Changes API on Secured Server}

Changes API uses WebSockets under the covers. Due to [the lack of support for client certificates in WebSockets implementation in .NET Core 2.0](https://github.com/dotnet/corefx/issues/5120#issuecomment-348557761)
the Changes API won't work for secured servers accessible over HTTPS.

This issue is fixed in the final version of .NET Core 2.1 available [here](https://dotnet.microsoft.com/download). In order to workaround this you can switch your application to use .NET Core 2.1.

The issue affects only the RavenDB client.
{DANGER/}

## Accessing Changes API

The changes subscription is accessible by a document store through its `IDatabaseChanges` interface.

{CODE:nodejs changes_1@client-api\changes\whatIsChangesApi.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | string | Name of database to open changes API for. If `null`, the `database` configured in DocumentStore will be used. |

| Return value | |
| ------------- | ----- |
| `IDatabaseChanges` object | Instance implementing IDatabaseChanges interface. |

## Connection interface

Changes object interface extends `IConnectableChanges` interface that represents the connection. It exposes the following properties, methods and events.

| Properties and methods | | |
| ------------- | ------------- | ----- |
| **connected** | boolean | Indicates whether it's connected or not |
| **on("connectionStatus")** | method | Adds a listener for 'connectionStatus' event |
| **on("error")** | method | Adds a listener for 'error' event | 
| **ensureConnectedNow()** | method | Returns a `Promise` resolved once connection to the server is established. | 

## Subscriptions

In order to retrieve notifications you have to subscribe to server-side events by using one of the following methods:

- [forAllDocuments()](../../client-api/changes/how-to-subscribe-to-document-changes#foralldocuments)
- [forAllIndexes()](../../client-api/changes/how-to-subscribe-to-index-changes#forallindexes)
- [forAllOperations()](../../client-api/changes/how-to-subscribe-to-operation-changes#foralloperations)
- [forDocument()](../../client-api/changes/how-to-subscribe-to-document-changes#fordocument)
- [forDocumentsInCollection()](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsincollection)
- [forDocumentsStartingWith()](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsstartingwith)
- [forIndex()](../../client-api/changes/how-to-subscribe-to-index-changes#forindex)
- [forOperationId()](../../client-api/changes/how-to-subscribe-to-operation-changes#foroperation)

## Unsubscribing

In order to end subscription (stop listening for particular notifications) you must `dispose()` it.

{CODE:nodejs changes_2@client-api\changes\whatIsChangesApi.js /}

## Remarks

{NOTE One or more open Changes API connections will prevent a database from becoming idle and unloaded, regardless of [configuration value for database idle timeout](../../server/configuration/database-configuration#databases.maxidletimeinsec)/}

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions Core](https://www.nuget.org/packages/System.Reactive.Core/) package to your project. /}


## Related Articles

### Changes API

- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)
