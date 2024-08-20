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
  * [Note](../../client-api/changes/what-is-changes-api#note)  
  * [Changes API -vs- Data Subscriptions](../../client-api/changes/what-is-changes-api#changes-api--vs--data-subscriptions)  
{NOTE/}

---

{PANEL: Accessing Changes API}

The changes subscription is accessible by a document store through its `IDatabaseChanges` interface.

{CODE:java changes_1@ClientApi\Changes\WhatIsChangesApi.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **database** | `String` | Name of database to open changes API for. If `null`, the `Database` configured in DocumentStore will be used. |

| Return value | |
| ------------- | ----- |
| IDatabaseChanges | Instance implementing IDatabaseChanges interface. |

{PANEL/}

{PANEL: Connection interface}

`IDatabaseChanges` inherits from `IConnectableChanges<TChanges>` interface that represent the connection.

{CODE:java connectable_changes@ClientApi\Changes\IConnectableChanges.java /}

{PANEL/}

{PANEL: Subscribing}

To receive notifications regarding server-side events, subscribe using one of the following methods.  

- [forAllDocuments](../../client-api/changes/how-to-subscribe-to-document-changes#foralldocuments)
- [forAllIndexes](../../client-api/changes/how-to-subscribe-to-index-changes#forallindexes)
- [forAllOperations](../../client-api/changes/how-to-subscribe-to-operation-changes#foralloperations)
- [forDocument](../../client-api/changes/how-to-subscribe-to-document-changes#fordocument)
- [forDocumentsInCollection](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsincollection)
- [forDocumentsStartingWith](../../client-api/changes/how-to-subscribe-to-document-changes#fordocumentsstartingwith)
- [forIndex](../../client-api/changes/how-to-subscribe-to-index-changes#forindex)
- [forOperationId](../../client-api/changes/how-to-subscribe-to-operation-changes#foroperation)

{PANEL/}

{PANEL: Unsubscribing}

To end a subscription (stop listening for particular notifications) you must 
`close` the subscription.  

{CODE:java changes_2@ClientApi\Changes\WhatIsChangesApi.java /}

{PANEL/}

{PANEL: Note}

{NOTE: }
One or more open Changes API connections will prevent a database from becoming 
idle and unloaded, regardless of [the configuration value for database idle timeout](../../server/configuration/database-configuration#databases.maxidletimeinsec).  
{NOTE/}

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
| What can the server Track | [Documents](../../client-api/data-subscriptions/what-are-data-subscriptions#documents-processing) <br> [Revisions](../../client-api/data-subscriptions/advanced-topics/subscription-with-revisioning) | [Documents](../../client-api/changes/how-to-subscribe-to-document-changes) <br> [Indexes](../../client-api/changes/how-to-subscribe-to-index-changes) <br> [Operations](../../client-api/changes/how-to-subscribe-to-operation-changes) <br> [Counters](../../client-api/changes/how-to-subscribe-to-counter-changes) <br> Time Series 
| What can the server Deliver | Documents <br> Revisions | Notifications 
| Management | Managed by the Server | Managed by the Client 


{PANEL/}

## Related Articles

### Changes API

- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)
