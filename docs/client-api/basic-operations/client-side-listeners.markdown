# Client-side listeners #
The RavenDB .NET Client API includes the following interfaces that define client-side event listeners:

- <em>IDocumentConversionListener</em>
- <em>IDocumentDeleteListener</em>
- <em>IDocumentQueryListener</em>
- <em>IDocumentStoreListener</em>

## IDocumentConversionListener ##
The `IDocumentConversionListener` is used to provide additional logic for converting an entity into a document and its associated metadata or for converting a document/metadata pair into an entity. Its `EntityToDocument` method is called when an entity is converted into a document and metadata, and its `DocumentToEntity` method is called when a document and metadata are converted into an entity. Both methods have the following three parameters:

- <em>entity</em>. An object that defines the applicable entity.
- <em>document</em>. A RavenDB JSON object that defines the applicable doccument.
- <em>metadata</em>. A RavenDB JSON object that contains the metadata associated with the entity.

## IDocumentDeleteListener ##
The `IDocumentDeleteListener` is used to provide additional logic when delete operations occur. It is invoked before the delete request is sent to the server. Its `BeforeDelete` method has the following three parameters:

- <em>key</em>. A string that contains the applicable key.
- <em>entityInstance</em>. An object that defines the applicable entity.
- <em>metadata</em>. A RavenDB JSON object that contains the metadata associated with the entity.

## IDocumentQueryListener ##
The `IDocumentQueryListener` is used to modify all queries globally. Its `BeforeQueryExecuted` method has the following  parameter:

- <em>queryCustomization</em>. An implementation of the `IDocumentQueryCustomization` interface that defines the global modification.

## IDocumentStoreListener ##
The `IDocumentStoreListener` is used to provide additional logic when store operations occur.

Its `BeforeStore` method is invoked before the store request is sent to the server and has the following four parameters:

- <em>key</em>. A string that contains the applicable key.
- <em>entityInstance</em>. An object that defines the applicable entity.
- <em>metadata</em>. A RavenDB JSON object that contains the metadata associated with the entity.
- <em>original</em>. A RavenDB JSON object that contains original document that was loaded from the server.

The `BeforeStore` method returns a boolean value that indicates whether the entity was modified and requires re-serialization. When `true` is returned, the entity is re-serialized, and when `false` is returned, any changes to the entity are ignored in the current call to `SaveChanges`.

Its `AfterStore` method is invoked after the store request is sent to the server and has the following three parameters:

- <em>key</em>. A string that contains the applicable key.
- <em>entityInstance</em>. An object that defines the applicable entity.
- <em>metadata</em>. A RavenDB JSON object that contains the metadata associated with the entity.

The `AfterStore` method does not return a value.



