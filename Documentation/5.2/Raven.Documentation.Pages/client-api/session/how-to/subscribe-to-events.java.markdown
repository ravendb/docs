# Session: How to Subscribe to Events

The concept of events provides users with a mechanism to perform custom actions in response to operations taken in a session. 

The event is invoked when a particular action is executed on an entity or querying is performed.

{INFO:Subscribing to an event}
Subscribing an event can be done in the `DocumentStore` object, which will be valid for all future sessions or subscribing in an already opened session with `session.advanced()` which will overwrite the existing event for the current session. 
{INFO/}

{PANEL:beforeStoreListener}

This event is invoked as a part of `saveChanges` but before it is actually sent to the server.  
It should be defined with this signature:  

{CODE-BLOCK: java}
public void addBeforeStoreListener(EventHandler<BeforeStoreEventArgs> handler);

public void removeBeforeStoreListener(EventHandler<BeforeStoreEventArgs> handler);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **handler** | `EventHandler<BeforeStoreEventArgs>` | handle this event |

The class `BeforeStoreEventArgs`:  

{CODE-BLOCK: java}
public BeforeStoreEventArgs(InMemoryDocumentSessionOperations session, String documentId, Object entity)
{CODE-BLOCK/}

{CODE-BLOCK: java}
public class BeforeStoreEventArgs
{
    private IMetadataDictionary _documentMetadata;
    private final InMemoryDocumentSessionOperations session;
    private final String documentId;
    private final Object entity;


    public InMemoryDocumentSessionOperations getSession() {
        return session;
    }

    public String getDocumentId() {
        return documentId;
    }

    public Object getEntity() {
        return entity;
    }

    public boolean isMetadataAccessed() {
        return _documentMetadata != null;
    }

    public IMetadataDictionary getDocumentMetadata() {
        if (_documentMetadata == null) {
            _documentMetadata = session.getMetadataFor(entity);
        }

        return _documentMetadata;
    }
{CODE-BLOCK/}

### Example

Say we want to discontinue all of the products that are not in stock.  

{CODE:Java on_before_store_event@ClientApi\Session\Events.java /}

After we subscribe to the event, every stored entity will invoke the method.  

{CODE:Java store_session@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:beforeDeleteListener}

This event is invoked by `delete(id)` or `delete(entity)`. It is only executed when `saveChanges()` 
is called, but before the commands are actually sent to the server.  
It should be defined with this signature:  

{CODE-BLOCK: java}
public void addBeforeDeleteListener(EventHandler<BeforeDeleteEventArgs> handler);

public void removeBeforeDeleteListener(EventHandler<BeforeDeleteEventArgs> handler);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **handler** | `EventHandler<BeforeDeleteEventArgs>` | handle this event |

The class `BeforeDeleteEventArgs`:  

{CODE-BLOCK: java}
 public BeforeDeleteEventArgs(InMemoryDocumentSessionOperations session, String documentId, Object entity)
{CODE-BLOCK/}

{CODE-BLOCK: java}
public class BeforeDeleteEventArgs
{
    private IMetadataDictionary _documentMetadata;
    private final InMemoryDocumentSessionOperations session;
    private final String documentId;
    private final Object entity;

    public InMemoryDocumentSessionOperations getSession() {
        return session;
    }

    public String getDocumentId() {
        return documentId;
    }

    public Object getEntity() {
        return entity;
    }

    public IMetadataDictionary getDocumentMetadata() {
        if (_documentMetadata == null) {
            _documentMetadata = session.getMetadataFor(entity);
        }

        return _documentMetadata;
    }

{CODE-BLOCK/}

### Example

To prevent anyone from deleting entities we can create a method as follows:

{CODE:Java on_before_delete_event@ClientApi\Session\Events.java /}

and subscribe it to the session:

{CODE:Java delete_session@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:afterSaveChangesListener}

This event is invoked after the `saveChanges` is returned.  
It should be defined with this signature:  

{CODE-BLOCK: java}
public void addAfterSaveChangesListener(EventHandler<AfterSaveChangesEventArgs> handler);

public void removeAfterSaveChangesListener(EventHandler<AfterSaveChangesEventArgs> handler);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **handler** | `EventHandler<AfterSaveChangesEventArgs>` | handle this event |


The class `AfterSaveChangesEventArgs`:

{CODE-BLOCK: java}
public class AfterSaveChangesEventArgs
{
    private IMetadataDictionary _documentMetadata;
    private final InMemoryDocumentSessionOperations session;
    private final String documentId;
    private final Object entity;

    public AfterSaveChangesEventArgs(InMemoryDocumentSessionOperations session, String documentId, Object entity) {
        this.session = session;
        this.documentId = documentId;
        this.entity = entity;
    }

    public InMemoryDocumentSessionOperations getSession() {
        return session;
    }

    public String getDocumentId() {
        return documentId;
    }

    public Object getEntity() {
        return entity;
    }

    public IMetadataDictionary getDocumentMetadata() {
        if (_documentMetadata == null) {
            _documentMetadata = session.getMetadataFor(entity);
        }

        return _documentMetadata;
    }
}


{CODE-BLOCK/}

### Example

If we want to log each entity that was saved, we can create a method as follows:  

{CODE:Java on_after_save_changes_event@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:beforeQueryListener}

This event is invoked just before the query is sent to the server. 
It should be defined with this signature:  

{CODE-BLOCK: java}
public void addBeforeQueryListener(EventHandler<BeforeQueryEventArgs> handler);

public void removeBeforeQueryListener(EventHandler<BeforeQueryEventArgs> handler);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **handler** | `EventHandler<BeforeQueryEventArgs>` | handle this event |

The class `BeforeQueryEventArgs`:  

{CODE-BLOCK: java}
public class BeforeQueryEventArgs
{
    private final InMemoryDocumentSessionOperations session;
    private final IDocumentQueryCustomization queryCustomization;

    public InMemoryDocumentSessionOperations getSession() {
        return session;
    }

    public IDocumentQueryCustomization getQueryCustomization() {
        return queryCustomization;
    }
{CODE-BLOCK/}

### Example I

If you want to disable caching of all query results, you can implement the method as follows:  

{CODE:Java on_before_query_execute_event@ClientApi\Session\Events.java /}

### Example II

If you want each query to [wait for non-stale results](../../../indexes/stale-indexes) you can create an event as follows:  

{CODE:Java on_before_query_execute_event_2@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:beforeConversionToDocumentListener}

This event is invoked before conversion of an entity to blittable JSON document. E.g. it's called when sending a document to a server.  
It should be defined with this signature:  

{CODE-BLOCK: java}
public void addBeforeConversionToDocumentListener(EventHandler<BeforeConversionToDocumentEventArgs> handler);

public void removeBeforeConversionToDocumentListener(EventHandler<BeforeConversionToDocumentEventArgs> handler);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **handler** | `EventHandler<BeforeConversionToDocumentEventArgs>` | handle this event |

The class `BeforeConversionToDocumentEventArgs`:  

{CODE-BLOCK: java}
public class BeforeConversionToDocumentEventArgs
{
    private String _id;
    private Object _entity;
    private InMemoryDocumentSessionOperations _session;

    public BeforeConversionToDocumentEventArgs(InMemoryDocumentSessionOperations session, String id, Object entity) {
        _session = session;
        _id = id;
        _entity = entity;
    }

    public String getId() {
        return _id;
    }

    public Object getEntity() {
        return _entity;
    }

    public InMemoryDocumentSessionOperations getSession() {
        return _session;
    }
}
{CODE-BLOCK/}

### Example

{CODE:Java on_before_conversion_to_document@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:afterConversionToDocumentListener}

This event is invoked after conversion of an entity to blittable JSON document.  
It should be defined with this signature:  

{CODE-BLOCK: java}
public void addAfterConversionToDocumentListener(EventHandler<AfterConversionToDocumentEventArgs> handler);

public void removeAfterConversionToDocumentListener(EventHandler<AfterConversionToDocumentEventArgs> handler);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **handler** | `EventHandler<AfterConversionToDocumentEventArgs>` | handle this event |

The class `AfterConversionToDocumentEventArgs`:  

{CODE-BLOCK: java}
public class AfterConversionToDocumentEventArgs
{
    private String _id;
    private Object _entity;
    private Reference<ObjectNode> _document;
    private InMemoryDocumentSessionOperations _session;

    public AfterConversionToDocumentEventArgs(InMemoryDocumentSessionOperations session, String id, Object entity, Reference<ObjectNode> document) {
        _session = session;
        _id = id;
        _entity = entity;
        _document = document;
    }

    public String getId() {
        return _id;
    }

    public Object getEntity() {
        return _entity;
    }

    public Reference<ObjectNode> getDocument() {
        return _document;
    }

    public InMemoryDocumentSessionOperations getSession() {
        return _session;
    }
}
{CODE-BLOCK/}

### Example

{CODE:Java on_after_conversion_to_document@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:beforeConversionToEntityListener}

This event is invoked before conversion of a JSON document to an entity. E.g. it's called when loading a document.  

It takes the argument `BeforeConversionToEntityEventArgs`, that consists of a JSON document, its ID and type, and the session instance.  

{CODE-BLOCK: java}
public void addBeforeConversionToEntityListener(EventHandler<BeforeConversionToEntityEventArgs> handler);

public void removeBeforeConversionToEntityListener(EventHandler<BeforeConversionToEntityEventArgs> handler);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **handler** | `EventHandler<BeforeConversionToEntityEventArgs>` | handle this event |

{CODE-BLOCK: java}
public class BeforeConversionToEntityEventArgs{

    private String _id;
    private Class _type;
    private Reference<ObjectNode> _document;
    private InMemoryDocumentSessionOperations _session;

    public String getId() {
        return _id;
    }

    public Class getType() {
        return _type;
    }

    public Reference<ObjectNode> getDocument() {
        return _document;
    }

    public InMemoryDocumentSessionOperations getSession() {
        return _session;
    }
}
{CODE-BLOCK/}

{CODE:Java on_before_conversion_to_entity@ClientApi\Session\Events.java /}

{PANEL/}

{PANEL:afterConversionToEntityListener}

This event is invoked after conversion of a JSON document to an entity. It takes the argument `AfterConversionToEntityEventArgs`, that consists of a JSON document, its ID, the session instance and a converted entity.  

{CODE-BLOCK: java}
public void addAfterConversionToEntityListener(EventHandler<AfterConversionToEntityEventArgs> handler);

public void removeAfterConversionToEntityListener(EventHandler<AfterConversionToEntityEventArgs> handler);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **handler** | `EventHandler<AfterConversionToEntityEventArgs>` | handle this event |

{CODE-BLOCK: java}
public class AfterConversionToEntityEventArgs {

    private String _id;
    private ObjectNode _document;
    private Object _entity;
    private InMemoryDocumentSessionOperations _session;

    public String getId() {
        return _id;
    }

    public ObjectNode getDocument() {
        return _document;
    }

    public Object getEntity() {
        return _entity;
    }

    public InMemoryDocumentSessionOperations getSession() {
        return _session;
    }
}
{CODE-BLOCK/}

{CODE:Java on_after_conversion_to_entity@ClientApi\Session\Events.java /}

{PANEL/}

## Related articles

### Document Store

- [What is a Document Store](../../../client-api/what-is-a-document-store)

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
