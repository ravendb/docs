# What Is the Changes API

---

{NOTE: }

* The RavenDB client offers a push notification feature that allows you to receive messages from a server about events that occurred there.
  You are able to subscribe to events for all documents, indexes, and operations as well as to indicate a particular one that you are interested in. 
  This mechanism lets you notify users if something has changed without the need to do any expensive polling. 

* {DANGER:Changes API on a Secure Server}
  Changes API uses WebSockets under the covers. Due to [the lack of support for client certificates in WebSockets implementation in .NET Core 2.0](https://github.com/dotnet/corefx/issues/5120#issuecomment-348557761)
  the Changes API won't work for secured servers accessible over HTTPS.
  This issue is fixed in the final version of .NET Core 2.1 available [here](https://dotnet.microsoft.com/download). In order to workaround this you can switch your application to use .NET Core 2.1.
  The issue affects only the RavenDB client.
  {DANGER/}

* In this page:  
  * [Accessing Changes API](../../)  
  * [](../../)  
  * [](../../)  
  * [](../../)  
  * [](../../)  
  * [](../../)  
{NOTE/}

---

{PANEL: Accessing Changes API}

The changes subscription is accessible by a document store through its `IDatabaseChanges` interface.

{CODE changes_1@ClientApi\Changes\WhatIsChangesApi.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | string | Name of database to open changes API for. If `null`, the `Database` configured in DocumentStore will be used. |

| Return value | |
| ------------- | ----- |
| IDatabaseChanges | Instance implementing IDatabaseChanges interface. |

{PANEL/}

{PANEL: Connection interface}

`IDatabaseChanges` inherits from `IConnectableChanges<TChanges>` interface that represent the connection.

{CODE connectable_changes@ClientApi\Changes\IConnectableChanges.cs /}

{PANEL/}

{PANEL: Subscriptions}

In order to retrieve notifications you have to subscribe to server-side events by using one of the following methods:

- [ForAllDocuments](../../client-api/changes/how-to-subscribe-to-document-changes#foralldocuments)
- [ForAllCounters](../../client-api/changes/how-to-subscribe-to-counter-changes#forallcounters)
- [ForAllIndexes](../../client-api/changes/how-to-subscribe-to-index-changes#forallindexes)
- [ForAllOperations](../../client-api/changes/how-to-subscribe-to-operation-changes#foralloperations)
- [ForDocument](../../client-api/changes/how-to-subscribe-to-document-changes#fordocument)
- [ForDocumentsInCollection](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsincollection)
- [ForDocumentsStartingWith](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsstartingwith)
- [ForCounter](../../client-api/changes/how-to-subscribe-to-counter-changes#forcounter)
- [ForCounterOfDocument](../../client-api/changes/how-to-subscribe-to-counter-changes#forcounterofdocument)
- [ForCountersOfDocument](../../client-api/changes/how-to-subscribe-to-counter-changes#forcountersofdocument)
- [ForIndex](../../client-api/changes/how-to-subscribe-to-index-changes#forindex)
- [ForOperationId](../../client-api/changes/how-to-subscribe-to-operation-changes#foroperation)

{PANEL/}

{PANEL: Unsubscribing}

In order to end subscription (stop listening for particular notifications) you must `Dispose` it.

{CODE changes_2@ClientApi\Changes\WhatIsChangesApi.cs /}

{PANEL/}

{PANEL: Remarks}

{NOTE One or more open Changes API connections will prevent a database from becoming idle and unloaded, regardless of [configuration value for database idle timeout](../../server/configuration/database-configuration#databases.maxidletimeinsec) /}

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions Core](https://www.nuget.org/packages/System.Reactive.Core/) package to your project. /}

{PANEL/}

## Related Articles

### Changes API

- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Counter Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)
