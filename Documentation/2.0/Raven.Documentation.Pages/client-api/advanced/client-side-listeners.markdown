# Client-side listeners

To perform various custom document store actions such as automatic conflict resolution or global query customization, we introduced concept of listeners to RavenDB client.

Currently there are five types of listeners that allow user to perform custom actions:

* Document Conflict listeners (**IDocumentConflictListener**),
* Document Conversion listeners (**IDocumentConversionListener**)
* Document Delete listeners (**IDocumentDeleteListener**)
* Document Query listeners (**IDocumentQueryListener**)
* Document Store listeners (**IDocumentStoreListener**)

and all of them are registered by using `RegisterListener` method in `DocumentStore`.

## Document Conflict listener

To allow users to handle document replication conflicts automatically, we introduced a Document Conflict listener. 
To create your own listener of this type, just implement `IDocumentConflictListener` interface.

{CODE document_conflict_interface@ClientApi\Advanced\ClientSideListeners.cs /}

### Example

This example shows how to create `TakeNewestConflictResolutionListener`, which will pick newest item from list of conflicted documents.

{CODE document_conflict_example@ClientApi\Advanced\ClientSideListeners.cs /}

## Document Conversion listener

Conversion listeners provide users with hook for additional logic when converting entities to documents and metadata and backwards. Just implement `IDocumentConversionListener` with any logic that you need.

{CODE document_conversion_interface@ClientApi\Advanced\ClientSideListeners.cs /}

### Example

Lets consider a case when we want to convert one of the metadata values to one of our `Custom` class properties. To achieve this we created `MetadataToPropertyConversionListener`.

{CODE document_conversion_example@ClientApi\Advanced\ClientSideListeners.cs /}

More sophisticated example usage can be found [here](https://ravendb.net/kb/16/using-optimistic-concurrency-in-real-world-scenarios).

## Document Delete listener

We introduced `IDocumentDeleteListener` interface which needs to be implemented if users wants to perform custom actions when delete operations are executed. Currently the interface contains only one method that is invoked before the delete request is sent to the server.

{CODE document_delete_interface@ClientApi\Advanced\ClientSideListeners.cs /}

### Example

To prevent anyone from deleting documents we can create `PreventDeleteListener` with implementation as follows:

{CODE document_delete_example@ClientApi\Advanced\ClientSideListeners.cs /}

## Document Query listener

To modify all queries globally, users need to create their own implementation of a `IDocumentQueryListener`.

{CODE document_query_interface@ClientApi\Advanced\ClientSideListeners.cs /}

### Example

If we want to have all results non stale, one can implement `NonStaleDocumentQueryListener` which will add `WaitForNonStaleResults` to every query executed.

{CODE document_query_example@ClientApi\Advanced\ClientSideListeners.cs /}

## Document Store listener

To execute any custom actions before of after document is stored  the `IDocumentStoreListener` needs to be implemented.

{CODE document_store_interface@ClientApi\Advanced\ClientSideListeners.cs /}

### Example

To prevent anyone from adding documents with certain key, one can create `FilterForbiddenKeysDocumentListener`.

{CODE document_store_example@ClientApi\Advanced\ClientSideListeners.cs /}