# Session : How to subscribe to events?

The concept of events provides users with a mechanism to perform custom actions, in response to operations taken in a session. 
The event is invoked when a particular action is executed on an entity or querying is performed.

{INFO:Subscribing to an event}
Subscribing an event can be done in `DocumentStore` object, which will be valid for all future sessions or subscribing in an already opened session with `session.Advanced` which will overwrite the existing event for the current session. 
{INFO/}

{PANEL:OnBeforeStore}

This event is invoked as a part of `SaveChanges` but before it actually sent to the server.

It takes the argument `BeforeStoreEventArgs`, that consists of the `Session`, entity's ID and the entity itself.

### Example

Let say we want to discontinue all of the products that are not in stock. 

{CODE on_before_store_event@ClientApi\Session\Events.cs /}

After we subscribe the event, every stored entity will invoke the method.

{CODE store_session@ClientApi\Session\Events.cs /}

{PANEL/}

{PANEL:OnBeforeDelete}

This event is invoked as a part of `SaveChanges` but before it actually sent the delete entities to the server.

It takes the argument `BeforeDeleteEventArgs`, that consists of the `Session`, entity's ID and the entity itself.

### Example

To prevent anyone from deleting entities we can create method as follows:

{CODE on_before_delete_event@ClientApi\Session\Events.cs /}

and subscribe it to the session:

{CODE delete_session@ClientApi\Session\Events.cs /}

{PANEL/}

{PANEL:OnAfterSaveChanges}

This event is invoked after the `SaveChanges` is returned, it takes the argument `AfterSaveChangesEventArgs`, that consists of the `Session`, entity's ID and the entity itself with the updated metadata from the server.

### Example

If we want to log each entity that was saved then we can create method as follows:

{CODE on_after_save_changes_event@ClientApi\Session\Events.cs /}

{PANEL/}

{PANEL:OnBeforeQuery}

This event is invoked just before the query is sent to the server.

It takes the argument `BeforeQueryEventArgs`, that consists of the `Session` and the `IDocumentQueryCustomization`.

### Example I

If we want to disable caching of all query results, you can implement method as follows:

{CODE on_before_query_execute_event@ClientApi\Session\Events.cs /}

### Example II

If we want each query to [wait for non-stale results](../../../indexes/stale-indexes) we can create event as follows:

{CODE on_before_query_execute_event_2@ClientApi\Session\Events.cs /}

{PANEL/}
