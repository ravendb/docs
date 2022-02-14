# Session: Subscribing to Session Events

* **Events** allow users to perform custom actions in response to operations made in 
  a `Document Store` or a `Session`.  

* An event is invoked when the selected action is executed on an entity, or querying is performed.  

* Subscribing to an event in a `Session` is valid only for this session.  

* Subscribing to an event at the `DocumentStore` level subscribes to this 
  event in all subsequent sessions.  
  Read more about `DocumentStore` events [here](../../../client-api/how-to/subscribe-to-store-events).  

{PANEL:OnBeforeStore}

This event is invoked as a part of `SaveChanges` but before it is actually sent to the server.  
It should be defined with this signature:  

{CODE-BLOCK: csharp}
private void OnBeforeStoreEvent(object sender, BeforeStoreEventArgs args);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **sender** | `IDocumentSession` | The session on which `SaveChanges()` has been called, triggering this event |
| **args** | `BeforeStoreEventArgs` | `args` contains the session on which `SaveChanges()` has been called, the ID of the document being Stored, the document's metadata, and the document itself. |

The class `BeforeStoreEventArgs`:  

{CODE-BLOCK: csharp}
public class BeforeStoreEventArgs
{
    public InMemoryDocumentSessionOperations Session;
    public string DocumentId;
    public object Entity;
    public IMetadataDictionary DocumentMetadata;
    internal bool MetadataAccessed;
}
{CODE-BLOCK/}

### Example

Say we want to discontinue all of the products that are not in stock.  

{CODE on_before_store_event@ClientApi\Session\Events.cs /}

After we subscribe to the event, every stored entity will invoke the method.  

{CODE store_session@ClientApi\Session\Events.cs /}

{PANEL/}

{PANEL:OnBeforeDelete}

This event is invoked by `Delete(id)` or `Delete(entity)`. It is only executed when `SaveChanges()` 
is called, but before the commands are actually sent to the server.  
It should be defined with this signature:  

{CODE-BLOCK: csharp}
private void OnBeforeDeleteEvent(object sender, BeforeDeleteEventArgs args);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **sender** | `IDocumentSession` | The session on which `SaveChanges()` has been called, triggering this event |
| **args** | `BeforeDeleteEventArgs` | `args` contains the session on which `SaveChanges()` has been called, the ID of the document being deleted, the document's metadata, and the document itself. |

The class `BeforeDeleteEventArgs`:  

{CODE-BLOCK: csharp}
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

{CODE on_before_delete_event@ClientApi\Session\Events.cs /}

and subscribe it to the session:

{CODE delete_session@ClientApi\Session\Events.cs /}

{PANEL/}

{PANEL:OnAfterSaveChanges}

This event is invoked after the `SaveChanges` is returned.  
It should be defined with this signature:  

{CODE-BLOCK: csharp}
private void OnAfterSaveChangesEvent(object sender, AfterSaveChangesEventArgs args);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **sender** | `IDocumentSession` | The session on which `SaveChanges()` has been called, triggering this event |
| **args** | `AfterSaveChangesEventArgs` | `args` contains the session on which `SaveChanges()` has been called, the ID of the document being deleted, and the document itself. |

The class `AfterSaveChangesEventArgs`:

{CODE-BLOCK: csharp}
public class AfterSaveChangesEventArgs
{
    public InMemoryDocumentSessionOperations Session;
    public string DocumentId;
    public object Entity;
}
{CODE-BLOCK/}

### Example

If we want to log each entity that was saved, we can create a method as follows:  

{CODE on_after_save_changes_event@ClientApi\Session\Events.cs /}

{PANEL/}

{PANEL:OnBeforeQuery}

This event is invoked just before the query is sent to the server. 
It should be defined with this signature:  

{CODE-BLOCK: csharp}
private void OnBeforeQueryEvent(object sender, BeforeQueryEventArgs args);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **sender** | `IDocumentSession` | The session on which `SaveChanges()` has been called, triggering this event |
| **args** | `BeforeQueryEventArgs` | `args` contains the session on which `SaveChanges()` has been called, and the query's [query customizations](../../../client-api/session/querying/how-to-customize-query). |

The class `BeforeQueryEventArgs`:  

{CODE-BLOCK: csharp}
public class BeforeQueryEventArgs
{
    public InMemoryDocumentSessionOperations Session;
    public IDocumentQueryCustomization queryCustomization;
}
{CODE-BLOCK/}

### Example I

If you want to disable caching of all query results, you can implement the method as follows:  

{CODE on_before_query_execute_event@ClientApi\Session\Events.cs /}

### Example II

If you want each query to [wait for non-stale results](../../../indexes/stale-indexes) you can create an event as follows:  

{CODE on_before_query_execute_event_2@ClientApi\Session\Events.cs /}

{PANEL/}

{PANEL:OnBeforeConversionToDocument}

This event is invoked before conversion of an entity to blittable JSON document. E.g. it's called when sending a document to a server.  
It should be defined with this signature:  

{CODE-BLOCK: csharp}
private void OnBeforeConversionToDocumentEvent(object sender, BeforeConversionToDocumentEventArgs args);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **sender** | `IDocumentSession` | The session on which `SaveChanges()` has been called, triggering this event |
| **args** | `BeforeConversionToDocumentEventArgs` | `args` contains the session on which `SaveChanges()` has been called, the ID of the document being ConversionToDocumentd, and the document itself. |

The class `BeforeConversionToDocumentEventArgs`:  

{CODE-BLOCK: csharp}
public class BeforeConversionToDocumentEventArgs
{
    public InMemoryDocumentSessionOperations Session;
    public string DocumentId;
    public object Entity;
}
{CODE-BLOCK/}

### Example

{CODE on_before_conversion_to_document@ClientApi\Session\Events.cs /}

{PANEL/}

{PANEL:OnAfterConversionToDocument}

This event is invoked after conversion of an entity to blittable JSON document.  
It should be defined with this signature:  

{CODE-BLOCK: csharp}
private void OnAfterConversionToDocumentEvent(object sender, AfterConversionToDocumentEventArgs args);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **sender** | `IDocumentSession` | The session on which `SaveChanges()` has been called, triggering this event |
| **args** | `AfterConversionToDocumentEventArgs` | `args` contains the session on which `SaveChanges()` has been called, the ID of the document being ConversionToDocumentd, and the document itself. |

The class `AfterConversionToDocumentEventArgs`:  

{CODE-BLOCK: csharp}
public class AfterConversionToDocumentEventArgs
{
    public InMemoryDocumentSessionOperations Session;
    public string DocumentId;
    public object Entity;
}
{CODE-BLOCK/}

### Example

{CODE on_after_conversion_to_document@ClientApi\Session\Events.cs /}

{PANEL/}

{PANEL:OnBeforeConversionToEntity}

This event is invoked before conversion of a JSON document to an entity. E.g. it's called when loading a document.  

It takes the argument `BeforeConversionToEntityEventArgs`, that consists of a JSON document, its ID and type, and the session instance.  

{CODE on_before_conversion_to_entity@ClientApi\Session\Events.cs /}


{PANEL/}

{PANEL:OnAfterConversionToEntity}

This event is invoked after conversion of a JSON document to an entity. It takes the argument `AfterConversionToEntityEventArgs`, that consists of a JSON document, its ID, the session instance and a converted entity.  

{CODE on_after_conversion_to_entity@ClientApi\Session\Events.cs /}

{PANEL/}

## Related articles

### Document Store

- [What is a Document Store](../../../client-api/what-is-a-document-store)  

### Session

- [What is a Session and How does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)  
- [Subscribe to Store Events](../../../client-api/how-to/subscribe-to-store-events)  
