# What Is the Changes API

---

{NOTE: }

* The Changes API is a push notification feature, that allows a client to 
  receive messages from the server regarding events that occurred on the server.  
* A client can subscribe to events related to **all** documents, indexes, operations, 
  counters, or time series, as well as to a **particular event** of interest.  
* The Changes API enables you to notify users of various changes, without requiring 
  any expensive polling.  

* In this page:  
  * [Accessing Changes API](../../client-api/changes/what-is-changes-api#accessing-changes-api)  
  * [Connection interface](../../client-api/changes/what-is-changes-api#connection-interface)  
  * [Subscribing](../../client-api/changes/what-is-changes-api#subscribing)  
  * [Unsubscribing](../../client-api/changes/what-is-changes-api#unsubscribing)  
  * [FAQ](../../client-api/changes/what-is-changes-api#faq)  
     * [Changes API and Database Timeout](../../client-api/changes/what-is-changes-api#changes-api-and-database-timeout)  
     * [Changes API and Method Overloads](../../client-api/changes/what-is-changes-api#changes-api-and-method-overloads)  
     * [Changes API and .NET Core 2.0](../../client-api/changes/what-is-changes-api#changes-api-and-.net-core-2.0)  
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

{PANEL: Subscribing}

To receive notifications regarding server-side events, subscribe using one of the following methods.  

* **For Document Changes:**
   - [ForAllDocuments](../../client-api/changes/how-to-subscribe-to-document-changes#foralldocuments)
   - [ForDocument](../../client-api/changes/how-to-subscribe-to-document-changes#fordocument)
   - [ForDocumentsInCollection](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsincollection)
   - [ForDocumentsStartingWith](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsstartingwith)

* **For Index Changes:**
   - [ForAllIndexes](../../client-api/changes/how-to-subscribe-to-index-changes#forallindexes)
   - [ForIndex](../../client-api/changes/how-to-subscribe-to-index-changes#forindex)

* **For Operation Changes:**
   - [ForAllOperations](../../client-api/changes/how-to-subscribe-to-operation-changes#foralloperations)
   - [ForOperationId](../../client-api/changes/how-to-subscribe-to-operation-changes#foroperation)

* **For Counter Changes:**
   - [ForAllCounters](../../client-api/changes/how-to-subscribe-to-counter-changes#forallcounters)
   - [ForCounter](../../client-api/changes/how-to-subscribe-to-counter-changes#forcounter)
   - [ForCounterOfDocument](../../client-api/changes/how-to-subscribe-to-counter-changes#forcounterofdocument)
   - [ForCountersOfDocument](../../client-api/changes/how-to-subscribe-to-counter-changes#forcountersofdocument)

* **For Time Series Changes:**
   - [ForAllTimeSeries](../../client-api/changes/how-to-subscribe-to-time-series-changes#foralltimeseries)  
   - [ForTimeSeries](../../client-api/changes/how-to-subscribe-to-time-series-changes#fortimeseries)  
   - [ForTimeSeriesOfDocument](../../client-api/changes/how-to-subscribe-to-time-series-changes#fortimeseriesofdocument)  

{PANEL/}

{PANEL: Unsubscribing}

To end a subscription (stop listening for particular notifications) you must `Dispose` of it.

{CODE changes_2@ClientApi\Changes\WhatIsChangesApi.cs /}

{PANEL/}

{PANEL: FAQ}

#### Changes API and Database Timeout

One or more open Changes API connections will prevent a database from becoming 
idle and unloaded, regardless of [the configuration value for database idle timeout](../../server/configuration/database-configuration#databases.maxidletimeinsec).  

---

#### Changes API and Method Overloads

To get more method overloads, especially ones supporting delegates, please add the 
[Reactive Extensions Core](https://www.nuget.org/packages/System.Reactive.Core/) package to your project.  

---

#### Changes API and .NET Core 2.0

Under the hood, the Changes API uses WebSockets.  
Due to the 
[lack of client certificates support in the implementation of WebSockets by .NET Core 2.0](https://github.com/dotnet/corefx/issues/5120#issuecomment-348557761), 
the Changes API will **not work** under this .Net version while accessing a secure server over HTTPS.  

* This issue was fixed in the final version of .NET Core 2.1.  
  To resolve it, make sure your application uses .NET Core 2.1 or higher.  
* The issue affects only the RavenDB Client.

{PANEL/}

## Related Articles

### Changes API

- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Counter Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)
