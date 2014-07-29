# Client API : What is Changes API?

The RavenDB client offers a push notification feature that allows you to receive messages from a server about events that occurred there.
You are able to subscribe to events for all documents or indexes as well as to indicate a particular one that you are interested in. 
This mechanism lets you to notify users if something has changed without the need to do any expensive pooling. 

## Accessing Changes API

The changes subscription is accessible by a document store. Depending on the type of the store you use (`DocumentStore`, `ShardedDocumentStore` or `EmbeddableDocumentStore`) you will get an appropriate instance
which is an implementation of a common `IDatabaseChanges` interface.

{CODE changes_1@ClientApi\Changes\WhatIsChangesApi.cs /}

**Parameters**   

database
:   Type: string   
Name of database to open changes API for. If `null`, default database configured in DocumentStore will be used.

**Return value**

Type: IDocumentChanges   
Instance implementing IDocumentChanges interface appropriate to store type.

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

## Subscribtions

In order to retrieve notifications you have to subscribe to server-side events by using one of the following methods:

- [ForAllDocuments]()
- [ForAllIndexes]()
- [ForAllReplicationConflicts]()
- [ForAllTransformers]()
- [ForBulkInsert]()
- [ForDocument]()
- [ForDocumentsInCollection]()
- [ForDocumentsOfType]()
- [ForDocumentsStartingWith]()
- [ForIndex]()

## Remarks

{INFO To achieve better experience of subscribing methods by using delegates, please add [Reactive Extensions](http://nuget.org/packages/Rx-Main) package to your project. /}

#### Related articles

TODO
