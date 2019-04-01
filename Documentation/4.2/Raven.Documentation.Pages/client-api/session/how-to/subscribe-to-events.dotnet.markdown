# Session: How to Subscribe to Events

The concept of events provides users with a mechanism to perform custom actions in response to operations taken in a session. 

The event is invoked when a particular action is executed on an entity or querying is performed.

{INFO:Subscribing to an event}
Subscribing an event can be done in the `DocumentStore` object, which will be valid for all future sessions or subscribing in an already opened session with `session.Advanced` which will overwrite the existing event for the current session. 
{INFO/}

{PANEL:OnBeforeStore}

This event is invoked as a part of `SaveChanges` but before it is actually sent to the server.

It takes the argument `BeforeStoreEventArgs` that consists of the `Session` entity's ID and the entity itself.

### Example

Say we want to discontinue all of the products that are not in stock. 

{CODE on_before_store_event@ClientApi\Session\Events.cs /}

After we subscribe to the event, every stored entity will invoke the method.

{CODE store_session@ClientApi\Session\Events.cs /}

{PANEL/}

{PANEL:OnBeforeDelete}

This event is invoked as a part of `SaveChanges`, but before it actually sends the deleted entities to the server.

It takes the argument `BeforeDeleteEventArgs`, that consists of the `Session` entity's ID and the entity itself.

### Example

To prevent anyone from deleting entities we can create a method as follows:

{CODE on_before_delete_event@ClientApi\Session\Events.cs /}

and subscribe it to the session:

{CODE delete_session@ClientApi\Session\Events.cs /}

{PANEL/}

{PANEL:OnAfterSaveChanges}

This event is invoked after the `SaveChanges` is returned. It takes the argument `AfterSaveChangesEventArgs` that consists of the `Session` entity's ID and the entity itself with the updated metadata from the server.

### Example

If we want to log each entity that was saved, we can create a method as follows:

{CODE on_after_save_changes_event@ClientApi\Session\Events.cs /}

{PANEL/}

{PANEL:OnBeforeQuery}

This event is invoked just before the query is sent to the server.

It takes the argument `BeforeQueryEventArgs`, that consists of the `Session` and the `IDocumentQueryCustomization`.

### Example I

If you want to disable caching of all query results, you can implement the method as follows:

{CODE on_before_query_execute_event@ClientApi\Session\Events.cs /}

### Example II

If you want each query to [wait for non-stale results](../../../indexes/stale-indexes) you can create an event as follows:

{CODE on_before_query_execute_event_2@ClientApi\Session\Events.cs /}

{PANEL/}

{PANEL:OnBeforeConversionToDocument}

This event is invoked before conversion of an entity to blittable JSON document. E.g. it's called when sending a document to a server.

It takes the argument `BeforeConversionToDocumentEventArgs`, that consists of an entity, its ID and the session instance. 

{CODE on_before_conversion_to_document@ClientApi\Session\Events.cs /}


{PANEL/}

{PANEL:OnAfterConversionToDocument}

This event is invoked after conversion of an entity to blittable JSON document.

It takes the argument `AfterConversionToDocumentEventArgs `, that consists of an entity, its ID, the session instance and converted JSON document.

{CODE on_after_conversion_to_document@ClientApi\Session\Events.cs /}

{PANEL/}

{PANEL:OnBeforeConversionToEntity}

This event is invoked before conversion of a JSON document to an entity. E.g. it's called when loading a document.

It takes the argument `BeforeConversionToEntityEventArgs`, that consists of a JSON document, its ID and type, and the session instance. 

{CODE on_before_conversion_to_entity@ClientApi\Session\Events.cs /}


{PANEL/}

{PANEL:OnAfterConversionToEntity}

This event is invoked after conversion of a JSON document to an entity.

{CODE on_after_conversion_to_entity@ClientApi\Session\Events.cs /}

It takes the argument `AfterConversionToEntityEventArgs`, that consists of a JSON document, its ID, the session instance and a converted entity.


{PANEL/}

## Related articles

### Document Store

- [What is a Document Store](../../../client-api/what-is-a-document-store)

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
