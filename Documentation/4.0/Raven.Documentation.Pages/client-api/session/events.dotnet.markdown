# Events

The concept of events provides users with a mechanism to perform custom actions, in response to operations taken in a session. 
The event is invoked when a particular action is executed on an entity or querying is run.

Subscribing an event can be done in `DocumentStore` object, which will be valid for all future sessions or subscribing in an already opened session with `session.Advanced` which will overwrite the existing event for the current session. 

There are four available events:

* `OnBeforeStore` 
* `OnBeforeDelete`
* `OnBeforeQueryExecuted`
* `OnAfterStore`

## OnBeforeStore 

This event is invoked as a part of `SaveChanges` but before it actually sent to the server.

It takes the argument `BeforeStoreEventArgs`, that consists of the `Session`, entity's ID and the entity itself.

Let say we want to discontinue all of the products that are not in stock. 

{CODE on_before_store_event@ClientApi\Session\Events.cs /}

After we subscribe the event, every stored entity will invoke the method.

{CODE store_session@ClientApi\Session\Events.cs /}

## OnAfterStore

This event is invoked after the `SaveChanges` is returned.

it takes the argument `AfterStoreEventArgs`, that consists of the `Session`, entity's ID and the entity itself with the updated metadata from the server.

## OnBeforeDelete

This event is invoked as a part of `SaveChanges` but before it actually sent the delete entities to the server.

It takes the argument `BeforeDeleteEventArgs`, that consists of the `Session`, entity's ID and the entity itself.

## OnBeforeQueryExecuted

This event is invoked as a part of `SaveChanges` but before it actually sent to the server.

It takes the argument `BeforeQueryExecutedEventArgs`, that consists of the `Session` and the `IDocumentQueryCustomization`.

To store the recent queries, we could write an event like this:

{CODE on_before_query_execute_event@ClientApi\Session\Events.cs /}
