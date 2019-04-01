# Session: How to Subscribe to Events

The concept of events provides users with a mechanism to perform custom actions in response to operations taken in a session. 

The listener is invoked when a particular action is executed on an entity or querying is performed.

{INFO:Subscribing to an event}
Subscribing an event can be done in the `DocumentStore` object, which will be valid for all future sessions or subscribing in an already opened session with `session.advanced` which will overwrite the existing event for the current session. 
{INFO/}

{PANEL:"beforeStore" event}

This event is invoked as a part of `saveChanges()`, but before it is actually sent to the server.

The callback passes the argument `BeforeStoreEventArgs` that consists of the `Session` entity's ID and the entity itself.

### Example

Say we want to discontinue all of the products that are not in stock. After we add a listener to the event, it's going to emit on every stored entity.

{CODE:nodejs store_session@client-api\session\events.js /}

{PANEL/}

{PANEL:"beforeDelete" event}

This event is invoked as a part of `saveChanges()`, but before it actually sends the deleted entities to the server.

It takes the argument `BeforeDeleteEventArgs`, that consists of the `Session` entity's ID and the entity itself.

### Example

To prevent anyone from deleting entities we can create a listener as follows:

{CODE:nodejs delete_session@client-api\session\events.js /}

{PANEL/}

{PANEL:"afterSaveChanges" event}

This event is invoked after the `saveChanges()` is returned. It takes the argument `AfterSaveChangesEventArgs` that consists of the `Session` entity's ID and the entity itself with the updated metadata from the server.

### Example

If we want to log each entity that was saved, we can create a method as follows:

{CODE:nodejs on_after_save_changes_event@client-api\session\events.js /}

{PANEL/}

{PANEL:"beforeQuery" event}

This event is invoked just before the query is sent to the server.

It takes the argument `BeforeQueryEventArgs`, that consists of the `Session` and the `IDocumentQueryCustomization`.

### Example I

If you want to disable caching of all query results, you can implement the method as follows:

{CODE:nodejs on_before_query_execute_event@client-api\session\events.js /}

### Example II

If you want each query to [wait for non-stale results](../../../indexes/stale-indexes) you can create an event as follows:

{CODE:nodejs on_before_query_execute_event_2@client-api\session\events.js /}

{PANEL/}

## Related articles

### Document Store

- [What is a Document Store](../../../client-api/what-is-a-document-store)

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
