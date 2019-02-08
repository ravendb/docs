# Changes API

The RavenDB client offers a push notification feature that allows you to receive messages from a server about events that occurred there.
You are able to subscribe to events for all documents or indexes as well as to indicate a particular one that you are interested in. 
This mechanism lets you to notify users if something has changed without the need to do any expensive pooling. 

## IDatabaseChanges interface

The changes subscription is accessible by a document store. Depending on the type of the store you use (`DocumentStore`, `ShardedDocumentStore` or `EmbeddableDocumentStore`) you will get an appropriate instance
which is an implementation of a common `IDatabaseChanges` interface.

{CODE getting_database_changes_instance@ClientApi\ChangesApi.cs /}

The parameter of the `Changes` method is an optional database name that you want get notifications from. If it is not provided the default database will be chosen.

## Connection properties

`IDatabaseChanges` has three properties that are related to the server connection:

{CODE-START: csharp /}
// represents the task responsible for establishing connection with the server
Task Task { get; }

// returns state of the connection
bool Connected { get; }

// the event raised if a connection state is changed
event EventHandler ConnectionStatusChanged
{CODE-END /}

## Subscribing methods

In order to retrieve notifications you have to subscribe server-side events by using either document or index related methods.

{INFO To achieve a better experience of subsribing methods by using delegates, please add [Reactive Extensions](https://www.nuget.org/packages/Rx-Main) package to your project. /}

## Document notifications

You are able to subscribe changes of all documents in database. For example to get information from the server about all document changes use the following code:

{CODE subscribe_for_all_documents@ClientApi\ChangesApi.cs /}


The very common scenario is to subscribe changes for documents within the same collection. To achieve that you should use the `ForDocumentsStartingWith` method:

{CODE subscribe_documents_starting_with@ClientApi\ChangesApi.cs /}

You can also observe the particular document by specifing its identifier:

{CODE subscribe_document_delete@ClientApi\ChangesApi.cs /}

The result of subscription methods above is `DocumentChangeNotification` object. It consists of the following properties:
{CODE-START: csharp /}
DocumentChangeTypes Type { get; set; }
string Id { get; set; }
Etag Etag { get; set; }
{CODE-END /}

where `DocumentChangeTypes` is the enum with the values as follow:
{CODE-START: csharp /}
None,
Put,
Delete,
ReplicationConflict,
AttachmentReplicationConflict,
BulkInsertStarted,
BulkInsertEnded,
BulkInsertError
Common (Put | Delete)
{CODE-END /}

## Index notifications

The same way like observing changes for documents you are allowed to grab information about indexes from the server. In order to look for changes for all indexes
you should use the `ForAllIndexes` method. For example to get info about newly created indexes use the code:

{CODE subscribe_indexes@ClientApi\ChangesApi.cs /}

If you are interested in observing changes of a specified index only, you can pass the index name:

{CODE subscribe_index_reduce_completed@ClientApi\ChangesApi.cs /}

This sample shows how to get information about completed reduce work by the map/reduce index.


As the result of subscribing the index notifications you will get `IndexChangeNotification` object that contains the following properties:
{CODE-START: csharp /}
IndexChangeTypes Type { get; set; }
string Name { get; set; }
Etag Etag { get; set; }
{CODE-END /}

`IndexChangeTypes` is the enum that has the flags:
{CODE-START: csharp /}
None,
MapCompleted,
ReduceCompleted,
RemoveFromIndex,
IndexAdded,
IndexRemoved,
IndexDemotedToIdle,
IndexPromotedFromIdle,
IndexDemotedToAbandoned
{CODE-END /}

## Replication conflict notifications

With the changes API you also can listen to replication conflicts, for both documents and attachements. Here is the sample code:

{CODE subscribe_documents_replication_conflict@ClientApi\ChangesApi.cs /}

After subscribe you will be getting `ReplicationConflictNotification` objects, which look as follow:

{CODE-START: csharp /}
ReplicationConflictTypes ItemType { get; set; }
string Id { get; set; }
Etag Etag { get; set; }
ReplicationOperationTypes OperationType { get; set; }
string[] Conflicts { get; set; }
{CODE-END /}

`ReplicationConflictTypes` is the enum that determines the type of conflicted item:

{CODE-START: csharp /}
DocumentReplicationConflict = 1,
AttachmentReplicationConflict = 2
{CODE-END /}

There are two `ReplicationOperationTypes` available:

{CODE-START: csharp /}
Put = 1,
Delete = 2
{CODE-END /}

### Automatic document conflict resolution

In RavenDB client you have an opportunity to register [a conflict listeners](advanced/client-side-listeners#document-conflict-listener) which are used to resolve conflicted document. However this can happen
only if you get the conflicted document. The ability to subscribe to the replication conflicts gives the client more power. Now if you listen to the conflicts and have any conflict listener
registered then the client will automatically resolve the conflict right after the arrival of the notification.


## BulkInsert notifications

To observe the bulk insert operations, you can subscribe using the `ForBulkInsert` method from the API with bulk insert operation id as a parameter.

In a result of subscribing to bulk insert notifications you will get `BulkInsertChangeNotification` objects that contain same properties as `DocumentChangeNotification` with additional `OperationId` property to mark for which bulk insert notification the notification occured.

{CODE subscribe_bulk_insert@ClientApi\ChangesApi.cs /}


