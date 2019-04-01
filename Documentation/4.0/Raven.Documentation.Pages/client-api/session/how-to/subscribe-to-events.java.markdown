# Session: How to Subscribe to Events

The concept of events provides users with a mechanism to perform custom actions in response to operations taken in a session. 

The listener is invoked when a particular action is executed on an entity or querying is performed.

{INFO:Subscribing to an event}
Subscribing an event can be done in the `DocumentStore` object, which will be valid for all future sessions or subscribing in an already opened session with `session.advanced` which will overwrite the existing event for the current session. 
{INFO/}

{PANEL:beforeStoreListener}

This event is invoked as a part of `saveChanges` but before it is actually sent to the server.

It takes the argument `BeforeStoreEventArgs` that consists of the `Session` entity's ID and the entity itself.

### Example

Say we want to discontinue all of the products that are not in stock. 

{CODE:java on_before_store_event@ClientApi\Session\Events.java /}

After we subscribe to the event, every stored entity will invoke the method.

{CODE:java store_session@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:beforeDeleteListener}

This event is invoked as a part of `saveChanges`, but before it actually sends the deleted entities to the server.

It takes the argument `BeforeDeleteEventArgs`, that consists of the `Session` entity's ID and the entity itself.

### Example

To prevent anyone from deleting entities we can create a method as follows:

{CODE:java on_before_delete_event@ClientApi\Session\Events.java /}

and subscribe it to the session:

{CODE:java delete_session@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:afterSaveChangesListener}

This event is invoked after the `saveChanges` is returned. It takes the argument `AfterSaveChangesEventArgs` that consists of the `Session` entity's ID and the entity itself with the updated metadata from the server.

### Example

If we want to log each entity that was saved, we can create a method as follows:

{CODE:java on_after_save_changes_event@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:beforeQueryListener}

This event is invoked just before the query is sent to the server.

It takes the argument `BeforeQueryEventArgs`, that consists of the `Session` and the `IDocumentQueryCustomization`.

### Example I

If you want to disable caching of all query results, you can implement the method as follows:

{CODE:java on_before_query_execute_event@ClientApi\Session\Events.java /}

### Example II

If you want each query to [wait for non-stale results](../../../indexes/stale-indexes) you can create an event as follows:

{CODE:java on_before_query_execute_event_2@ClientApi\Session\Events.java /}

{PANEL/}

## Related articles

### Document Store

- [What is a Document Store](../../../client-api/what-is-a-document-store)

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
