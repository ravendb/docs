# What Is the Changes API

---

{NOTE: }

* The Changes API is a Push Notifications service, that allows a RavenDB Client to 
  receive messages from a RavenDB Server regarding events that occurred on the server.  
* A client can subscribe to events related to documents, indexes, operations, counters, and time series.  
* Using the Changes API allows you to notify users of various changes without requiring 
  any expensive polling.  

* In this page:  
  * [Accessing Changes API](../../client-api/changes/what-is-changes-api#accessing-changes-api)  
  * [Connection interface](../../client-api/changes/what-is-changes-api#connection-interface)  
  * [Subscribing](../../client-api/changes/what-is-changes-api#subscribing)  
  * [Unsubscribing](../../client-api/changes/what-is-changes-api#unsubscribing)  
  * [FAQ](../../client-api/changes/what-is-changes-api#faq)  
     * [Changes API and Database Timeout](../../client-api/changes/what-is-changes-api#changes-api-and-database-timeout)  
     * [Changes API and Method Overloads](../../client-api/changes/what-is-changes-api#changes-api-and-method-overloads)  
  * [Changes API -vs- Data Subscriptions](../../client-api/changes/what-is-changes-api#changes-api--vs--data-subscriptions)  
{NOTE/}

---

{PANEL: Accessing Changes API}

The changes subscription is accessible by a document store through its `IDatabaseChanges` 
or `ISingleNodeDatabaseChanges` interfaces.

{CODE changes_1@ClientApi\Changes\WhatIsChangesApi.cs /}
{CODE changes_ISingleNodeDatabaseChanges@ClientApi\Changes\WhatIsChangesApi.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | `string` | Name of database to open changes API for. If `null`, the `Database` configured in DocumentStore will be used. |
| **nodeTag** | `string` | Tag of the cluster node to open changes API for. |

| Return value | |
| ------------- | ----- |
| `IDatabaseChanges` | Instance implementing `IDatabaseChanges` interface. |
| `ISingleNodeDatabaseChanges` | Instance implementing `ISingleNodeDatabaseChanges` interface. |

* Use `IDatabaseChanges` to subscribe to database changes.  
* Use `ISingleNodeDatabaseChanges` to limit tracking to a specific node.  
  {NOTE: }
  Note that from RavenDB 6.1 on, some changes can be tracked not cross-cluster but only 
  **on a specific node**. In these cases, it is required that you open the Changes API using 
  the second overload, passing both a database name and a node tag: `store.Changes(dbName, nodeTag)`  
  {NOTE/}

{PANEL/}

{PANEL: Connection interface}

`IDatabaseChanges` inherits from `IConnectableChanges<TChanges>` interface that represent the connection.

{CODE connectable_changes@ClientApi\Changes\IConnectableChanges.cs /}

{PANEL/}

{PANEL: Subscribing}

To receive notifications regarding server-side events, subscribe using one of the following methods.  

* **For Document Changes:**  
   - [ForAllDocuments](../../client-api/changes/how-to-subscribe-to-document-changes#foralldocuments)  
     Track changes for all document  
   - [ForDocument](../../client-api/changes/how-to-subscribe-to-document-changes#fordocument)  
     Track changes for a given document (by Doc ID)  
   - [ForDocumentsInCollection](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsincollection)  
     Track changes for all documents in a given collection  
   - [ForDocumentsStartingWith](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsstartingwith)  
     Track changes for documents whose ID contains a given prefix  

* **For Index Changes:**  
   - [ForAllIndexes](../../client-api/changes/how-to-subscribe-to-index-changes#forallindexes)  
     Track changes for all indexes  
   - [ForIndex](../../client-api/changes/how-to-subscribe-to-index-changes#forindex)  
     Track changes for a given index (by Index Name)   

* **For Operation Changes:**  
  Operation changes can be tracked only [on a specific node](../../client-api/changes/what-is-changes-api#accessing-changes-api).  
   - [ForAllOperations](../../client-api/changes/how-to-subscribe-to-operation-changes#foralloperations)  
     Track changes for all operation  
   - [ForOperationId](../../client-api/changes/how-to-subscribe-to-operation-changes#foroperationid)  
     Track changes for a given operation (by Operation ID)  

* **For Counter Changes:**  
   - [ForAllCounters](../../client-api/changes/how-to-subscribe-to-counter-changes#forallcounters)  
     Track changes for all counters  
   - [ForCounter](../../client-api/changes/how-to-subscribe-to-counter-changes#forcounter)  
     Track changes for a given counter (by Counter Name)  
   - [ForCounterOfDocument](../../client-api/changes/how-to-subscribe-to-counter-changes#forcounterofdocument)  
     Track changes for a specific counter of a chosen document (by Doc ID and Counter Name)  
   - [ForCountersOfDocument](../../client-api/changes/how-to-subscribe-to-counter-changes#forcountersofdocument)  
     Track changes for all counters of a chosen document (by Doc ID)  

* **For Time Series Changes:**  
   - [ForAllTimeSeries](../../client-api/changes/how-to-subscribe-to-time-series-changes#foralltimeseries)  
     Track changes for all time series  
   - [ForTimeSeries](../../client-api/changes/how-to-subscribe-to-time-series-changes#fortimeseries)  
     Track changes for all time series with a given name  
   - [ForTimeSeriesOfDocument](../../client-api/changes/how-to-subscribe-to-time-series-changes#fortimeseriesofdocument)  
     Track changes for -  
      * a **specific time series** of a given document (by Doc ID and Time Series Name)  
      * **any time series** of a given document (by Doc ID)  

{PANEL/}

{PANEL: Unsubscribing}

To end a subscription (stop listening for particular notifications) you must 
`Dispose` of the subscription.  

{CODE changes_2@ClientApi\Changes\WhatIsChangesApi.cs /}

{PANEL/}

{PANEL: FAQ}

#### Changes API and Database Timeout

One or more open Changes API connections will prevent a database from becoming 
idle and unloaded, regardless of [the configuration value for database idle timeout](../../server/configuration/database-configuration#databases.maxidletimeinsec).  

---

#### Changes API and Method Overloads

{WARNING: }
To get more method overloads, especially ones supporting **delegates**, please add the 
[System.Reactive.Core](https://www.nuget.org/packages/System.Reactive.Core/) package to your project.  
{WARNING/}

{PANEL/}

{PANEL: Changes API -vs- Data Subscriptions}

**Changes API** and [Data Subscription](../../client-api/data-subscriptions/what-are-data-subscriptions) 
are services that a RavenDB Server provides subscribing clients.  
Both services respond to events that take place on the server, by sending updates 
to their subscribers.  

* **Changes API is a Push Notifications Service**.  
   * Changes API subscribers receive **notifications** regarding events that 
     took place on the server, without receiving the actual data entities 
     affected by these events.  
     For the modification of a document, for example, the client will receive 
     a [DocumentChange](../../client-api/changes/how-to-subscribe-to-document-changes#documentchange) 
     object with details like the document's ID and collection name.  

   * The server does **not** keep track of sent notifications or 
     checks clients' usage of them. It is a client's responsibility 
     to manage its reactions to such notifications.  

* **Data Subscription is a Data Consumption Service**.  
   * A Data Subscription task keeps track of document modifications in the 
     database and delivers the documents in an orderly fashion when subscribers 
     indicate they are ready to receive them. 
   * The process is fully managed by the server, leaving very little for 
     the subscribers to do besides consuming the delivered documents.  

---

|    | Data Subscriptions | Changes API 
| -- | -- | 
| What can the server Track | [Documents](../../client-api/data-subscriptions/what-are-data-subscriptions#documents-processing) <br> [Revisions](../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning) <br> [Counters](../../client-api/data-subscriptions/creation/examples#including-counters) <br> Time Series | [Documents](../../client-api/changes/how-to-subscribe-to-document-changes) <br> [Indexes](../../client-api/changes/how-to-subscribe-to-index-changes) <br> [Operations](../../client-api/changes/how-to-subscribe-to-operation-changes) <br> [Counters](../../client-api/changes/how-to-subscribe-to-counter-changes) <br> [Time Series](../../client-api/changes/how-to-subscribe-to-time-series-changes) 
| What can the server Deliver | Documents <br> Revisions <br> Counters <br> Time Series | Notifications 
| Management | Managed by the Server | Managed by the Client 


{PANEL/}

## Related Articles

### Changes API

- [How to Subscribe to Document Changes](../../client-api/changes/how-to-subscribe-to-document-changes)  
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)  
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)  
- [How to Subscribe to Counter Changes](../../client-api/changes/how-to-subscribe-to-counter-changes)  
- [How to Subscribe to Time Series Changes](../../client-api/changes/how-to-subscribe-to-time-series-changes)  

### Data Subscriptions

- [What Are Data Subscriptions](../../client-api/data-subscriptions/what-are-data-subscriptions)  
