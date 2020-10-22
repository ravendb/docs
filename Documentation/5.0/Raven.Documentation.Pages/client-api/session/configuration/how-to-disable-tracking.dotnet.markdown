# Session: How to Disable Entity Tracking

---

{NOTE: }

* By default, each session tracks changes to all entities it has either stored 
or loaded. These changes are all persisted when `SaveChanges()` is called.  

* Tracking can be disabled per session using the session options, or an ignore 
function can be created for the entire document store using the document store 
conventions.  

* See [Session: How to Ignore Entity Changes](../../../client-api/session/how-to/ignore-entity-changes) 
to disable tracking for an individual entity.  

* In this page:  
  * [Disable Tracking per Session](../../../client-api/session/configuration/how-to-disable-tracking#disable-tracking-per-session)
  * [ShouldIgnoreEntityChanges Method](../../../client-api/session/configuration/how-to-disable-tracking#shouldignoreentitychanges-method)
{NOTE/}

--

{PANEL: Disable Tracking per Session}

Tracking can be turned off by setting the `SessionOptions.NoTracking` property 
to `true`. When turned off, the `Store()` and `SaveChanges()` methods will no 
longer be available (an exception will be thrown if they are used). Each call 
to one of the `Load` methods will create a new instance of the entity.  

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync open_session_tracking_1@ClientApi\Session\OpeningSession.cs /}
{CODE-TAB:csharp:Async open_session_tracking_2@ClientApi\Session\OpeningSession.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: ShouldIgnoreEntityChanges Method}

In the [Document Store conventions](../../../client-api/configuration/conventions), 
you can configure the `ShouldIgnoreEntityChanges` method to fine-tune which 
entities to ignore and when. The function can take several parameters including a 
session and an entity, and returns a boolean. If the function returns `true`, 
changes tothe given entity will not be tracked. Changes to that entity will be 
ignored and will not be persisted on `SaveChanges()`.  

{CODE-BLOCK: csharp}
public Func<InMemoryDocumentSessionOperations, object, string, bool> ShouldIgnoreEntityChanges;
{CODE-BLOCK/}

| Parameter Type | Description |
| - | - |
| `InMemoryDocumentSessionOperations` | The session for which tracking is to be disabled |
| `object` | The entity for which tracking is to be disabled |
| `string` | The entity's document ID |

| Return Type | Description |
| - | - |
| `bool` | If `true`, the entity will not be tracked.  

### Example

In this example the `ShouldIgnoreEntityChanges` function returns true for 
any entity that is of type `Employee` and whose `FirstName` property is 'Bob'. 
An employee with the first name 'Bob' is not persisted, but as soon as their 
`FirstName` is changed to something else, their changes are tracked and will 
be persisted.  

{CODE:csharp ignore_entity_function@ClientApi\Session\OpeningSession.cs /}

{PANEL/}

## Related Articles

### Client API

- [Document Store Conventions](../../../client-api/configuration/conventions)

### Session

- [How to Ignore Entity Changes](../../../client-api/session/how-to/ignore-entity-changes)
- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work) 
- [Opening a Session](../../../client-api/session/opening-a-session)
- [Storing Entities](../../../client-api/session/storing-entities)
- [Loading Entities](../../../client-api/session/loading-entities)
- [Saving Changes](../../../client-api/session/saving-changes)
