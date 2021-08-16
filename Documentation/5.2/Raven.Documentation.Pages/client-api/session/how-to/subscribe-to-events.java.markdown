# Session: How to Subscribe to Events

The concept of events provides users with a mechanism to perform custom actions in response to operations taken in a session. 

The event is invoked when a particular action is executed on an entity or querying is performed.

{INFO:Subscribing to an event}
Subscribing an event can be done in the `DocumentStore` object, which will be valid for all future sessions or subscribing in an already opened session with `session.advanced()` which will overwrite the existing event for the current session. 
{INFO/}

{PANEL:OnBeforeStore}

This event is invoked as a part of `saveChanges` but before it is actually sent to the server.  
It should be defined with this signature:  

{CODE-BLOCK: java}
private void onBeforeStoreEvent(object sender, BeforeStoreEventArgs args);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **sender** | `DocumentSession` | The session on which `saveChanges()` has been called, triggering this event |
| **args** | `BeforeStoreEventArgs` | `args` contains the session on which `saveChanges()` has been called, the ID of the document being Stored, the document's metadata, and the document itself. |

The class `BeforeStoreEventArgs`:  

{CODE-BLOCK: java}
public class BeforeStoreEventArgs
{
    private IMetadataDictionary _documentMetadata;
    private final InMemoryDocumentSessionOperations session;
    private final String documentId;
    private final Object entity;
}
{CODE-BLOCK/}

### Example

Say we want to discontinue all of the products that are not in stock.  

{CODE:Java on_before_store_event@ClientApi\Session\Events.java /}

After we subscribe to the event, every stored entity will invoke the method.  

{CODE:Java store_session@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:OnBeforeDelete}

This event is invoked by `delete(id)` or `delete(entity)`. It is only executed when `saveChanges()` 
is called, but before the commands are actually sent to the server.  
It should be defined with this signature:  

{CODE-BLOCK: java}
private void onBeforeDeleteEvent(object sender, BeforeDeleteEventArgs args);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **sender** | `DocumentSession` | The session on which `saveChanges()` has been called, triggering this event |
| **args** | `BeforeDeleteEventArgs` | `args` contains the session on which `saveChanges()` has been called, the ID of the document being deleted, the document's metadata, and the document itself. |

The class `BeforeDeleteEventArgs`:  

{CODE-BLOCK: java}
public class BeforeDeleteEventArgs
{
    public InMemoryDocumentSessionOperations Session;
    public string DocumentId;
    public object Entity;
    public IMetadataDictionary DocumentMetadata;
    internal bool MetadataAccessed;
}
{CODE-BLOCK/}

### Example

To prevent anyone from deleting entities we can create a method as follows:

{CODE:Java on_before_delete_event@ClientApi\Session\Events.java /}

and subscribe it to the session:

{CODE:Java delete_session@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:OnAfterSaveChanges}

This event is invoked after the `saveChanges` is returned.  
It should be defined with this signature:  

{CODE-BLOCK: java}
private void OnAfterSaveChangesEvent(object sender, AfterSaveChangesEventArgs args);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **sender** | `DocumentSession` | The session on which `saveChanges()` has been called, triggering this event |
| **args** | `AfterSaveChangesEventArgs` | `args` contains the session on which `saveChanges()` has been called, the ID of the document being deleted, and the document itself. |

The class `AfterSaveChangesEventArgs`:

{CODE-BLOCK: java}
public class AfterSaveChangesEventArgs
{
     private IMetadataDictionary _documentMetadata;
    private final InMemoryDocumentSessionOperations session;
    private final String documentId;
    private final Object entity;
}
{CODE-BLOCK/}

### Example

If we want to log each entity that was saved, we can create a method as follows:  

{CODE:Java on_after_save_changes_event@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:OnBeforeQuery}

This event is invoked just before the query is sent to the server. 
It should be defined with this signature:  

{CODE-BLOCK: java}
private void OnBeforeQueryEvent(object sender, BeforeQueryEventArgs args);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **sender** | `DocumentSession` | The session on which `saveChanges()` has been called, triggering this event |
| **args** | `BeforeQueryEventArgs` | `args` contains the session on which `saveChanges()` has been called, and the query's [query customizations](../../../client-api/session/querying/how-to-customize-query). |

The class `BeforeQueryEventArgs`:  

{CODE-BLOCK: java}
public class BeforeQueryEventArgs
{
    private final InMemoryDocumentSessionOperations session;
    private final IDocumentQueryCustomization queryCustomization;
{CODE-BLOCK/}

### Example I

If you want to disable caching of all query results, you can implement the method as follows:  

{CODE:Java on_before_query_execute_event@ClientApi\Session\Events.java /}

### Example II

If you want each query to [wait for non-stale results](../../../indexes/stale-indexes) you can create an event as follows:  

{CODE:Java on_before_query_execute_event_2@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:OnBeforeConversionToDocument}

This event is invoked before conversion of an entity to blittable JSON document. E.g. it's called when sending a document to a server.  
It should be defined with this signature:  

{CODE-BLOCK: java}
private void onBeforeConversionToDocumentEvent(object sender, BeforeConversionToDocumentEventArgs args);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **sender** | `DocumentSession` | The session on which `saveChanges()` has been called, triggering this event |
| **args** | `BeforeConversionToDocumentEventArgs` | `args` contains the session on which `saveChanges()` has been called, the ID of the document being ConversionToDocumentd, and the document itself. |

The class `BeforeConversionToDocumentEventArgs`:  

{CODE-BLOCK: java}
public class BeforeConversionToDocumentEventArgs
{
    private String _id;
    private Object _entity;
    private InMemoryDocumentSessionOperations _session;
}
{CODE-BLOCK/}

### Example

{CODE:Java on_before_conversion_to_document@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:OnAfterConversionToDocument}

This event is invoked after conversion of an entity to blittable JSON document.  
It should be defined with this signature:  

{CODE-BLOCK: java}
private void OnAfterConversionToDocumentEvent(object sender, AfterConversionToDocumentEventArgs args);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **sender** | `DocumentSession` | The session on which `saveChanges()` has been called, triggering this event |
| **args** | `AfterConversionToDocumentEventArgs` | `args` contains the session on which `saveChanges()` has been called, the ID of the document being ConversionToDocumentd, and the document itself. |

The class `AfterConversionToDocumentEventArgs`:  

{CODE-BLOCK: java}
public class AfterConversionToDocumentEventArgs
{
    private String _id;
    private Object _entity;
    private Reference<ObjectNode> _document;
    private InMemoryDocumentSessionOperations _session;
}
{CODE-BLOCK/}

### Example

{CODE:Java on_after_conversion_to_document@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:OnBeforeConversionToEntity}

This event is invoked before conversion of a JSON document to an entity. E.g. it's called when loading a document.  

It takes the argument `BeforeConversionToEntityEventArgs`, that consists of a JSON document, its ID and type, and the session instance.  

{CODE:Java on_before_conversion_to_entity@ClientApi\Session\Events.java /}


{PANEL/}

{PANEL:OnAfterConversionToEntity}

This event is invoked after conversion of a JSON document to an entity. It takes the argument `AfterConversionToEntityEventArgs`, that consists of a JSON document, its ID, the session instance and a converted entity.  

{CODE:Java on_after_conversion_to_entity@ClientApi\Session\Events.java /}

{PANEL/}

## Related articles

### Document Store

- [What is a Document Store](../../../client-api/what-is-a-document-store)

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
