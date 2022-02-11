# Session: Subscribing to Session Events

* **Events** allow users to perform custom actions in response to operations made in 
  a `Document Store` or a `Session`.  

* An event is invoked when the selected action is executed on an entity, or querying is performed.  

* An event subscribed to in the `DocumentStore` level is valid for all succeeding sessions.  

* An event subscribed to in a `Session` is valid in this session.  
  Subscribing to an event within a session overrides subscribing 
  to it in the `DocumentStore` level.  
  Read more about `DocumentStore` events [here](../../../client-api/how-to/subscribe-to-store-events).  

{PANEL:OnBeforeStore}

This event is invoked as a part of `saveChanges()` but before it is actually sent to the server.  
It should be defined with this signature:  

{CODE:nodejs OnBeforeStore@ClientApi\Session\events.js /}

The usage of `SessionBeforeStoreEventArgs`:  

{CODE:nodejs beforeStoreEventArgs@ClientApi\Session\events.js /}

### Example

Say we want to discontinue all of the products that are not in stock.  

{CODE:nodejs on_before_store_event@ClientApi\Session\events.js /}

After we subscribe to the event, every stored entity will invoke the method.  

{CODE:nodejs store_session@ClientApi\Session\events.js /}

{PANEL/}

{PANEL:OnBeforeDelete}

This event is invoked by `delete(id)` or `delete(entity)`. It is only executed when `saveChanges()` 
is called, but before the commands are actually sent to the server.  
It should be defined with this signature:  

{CODE:nodejs before_delete_event@ClientApi\Session\events.js /}

The usage of `SessionBeforeDeleteEventArgs`:  

{CODE:nodejs beforeDeleteEventArgs@ClientApi\Session\events.js /}

### Example

To prevent anyone from deleting entities we can create a method as follows:

{CODE:nodejs on_Before_Delete_Event@ClientApi\Session\events.js /}

and subscribe it to the session:

{CODE:nodejs delete_session@ClientApi\Session\events.js /}

{PANEL/}

{PANEL:OnAfterSaveChanges}

This event is invoked after the `saveChanges` is returned.  
It should be defined with this signature:  

{CODE:nodejs after_save_event@ClientApi\Session\events.js /}


The usage of `SessionAfterSaveChangesEventArgs`:

{CODE:nodejs afterSaveChangesEventArgs@ClientApi\Session\events.js /}


### Example

If we want to log each entity that was saved, we can create a method as follows:  

{CODE:nodejs on_after_save_changes_event@ClientApi\Session\events.js /}

{PANEL/}

{PANEL:OnBeforeQuery}

This event is invoked just before the query is sent to the server. 
It should be defined with this signature:  

{CODE:nodejs before_query_event@ClientApi\Session\events.js /}

The usage of `SessionBeforeQueryEventArgs`:  

{CODE:nodejs beforeQueryEventArgs@ClientApi\Session\events.js /}

### Example I

If you want to disable caching of all query results, you can implement the method as follows:  

{CODE:nodejs on_before_query_execute_event@ClientApi\Session\events.js /}

### Example II

If you want each query to [wait for non-stale results](../../../indexes/stale-indexes) you can create an event as follows:  

{CODE:nodejs on_before_query_execute_event_2@ClientApi\Session\events.js /}

{PANEL/}

{PANEL:OnBeforeConversionToDocument}

This event is invoked before conversion of an entity to blittable JSON document. E.g. it's called when sending a document to a server.  
It should be defined with this signature:  

{CODE:nodejs beforeConversionToDocument_event@ClientApi\Session\events.js /}

The usage of `BeforeConversionToDocumentEventArgs`:  

{CODE:nodejs beforeConversionToDocument@ClientApi\Session\events.js /}

### Example

{CODE:nodejs on_before_conversion_to_document@ClientApi\Session\events.js /}

{PANEL/}

{PANEL:OnAfterConversionToDocument}

This event is invoked after conversion of an entity to blittable JSON document.  
It should be defined with this signature:  

{CODE:nodejs afterConversionToDocument_event@ClientApi\Session\events.js /}

The class `AfterConversionToDocumentEventArgs`:  

{CODE:nodejs afterConversionToDocument@ClientApi\Session\events.js /}


### Example

{CODE:nodejs on_after_conversion_to_document@ClientApi\Session\events.js /}

{PANEL/}

{PANEL:OnBeforeConversionToEntity}

This event is invoked before conversion of a JSON document to an entity. E.g. it's called when loading a document.  

It takes the argument `BeforeConversionToEntityEventArgs`, that consists of a JSON document, its ID and type, and the session instance.  

{CODE:nodejs on_before_conversion_to_entity@ClientApi\Session\events.js /}


{PANEL/}

{PANEL:OnAfterConversionToEntity}

This event is invoked after conversion of a JSON document to an entity. It takes the argument `AfterConversionToEntityEventArgs`, that consists of a JSON document, its ID, the session instance and a converted entity.  

{CODE:nodejs on_after_conversion_to_entity@ClientApi\Session\events.js /}

{PANEL/}

## Related articles

### Document Store

- [What is a Document Store](../../../client-api/what-is-a-document-store)  

### Session

- [What is a Session and How does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)  
- [Subscribe to Store Events](../../../client-api/how-to/subscribe-to-store-events)  
