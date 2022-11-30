# Disable Entity Tracking

---

{NOTE: }

* By default, each session tracks changes to all entities it has either stored, loaded, or queried for.  
  All changes are then persisted when `SaveChanges` is called.  

* __Tracking can be disabled__ by any of the following:  
    * [Disable tracking a specific entity in session](../../../client-api/session/configuration/how-to-disable-tracking#disable-tracking-a-specific-entity-in-session)
    * [Disable tracking all entities in session](../../../client-api/session/configuration/how-to-disable-tracking#disable-tracking-all-entities-in-session)
    * [Disable tracking query results](../../../client-api/session/configuration/how-to-disable-tracking#disable-tracking-query-results)
    * [Customize tracking in conventions](../../../client-api/session/configuration/how-to-disable-tracking#customize-tracking-in-conventions)
{NOTE/}

---

{PANEL: Disable tracking a specific entity in session}

* Tracking can be disabled for a specific entity in the session.  
* Once tracking is disabled for the entity then:
  * Any changes made to this entity will be ignored for `SaveChanges`.  
  * Performing another `Load` for this entity will Not generate another call to the server.
  
__Example__

{CODE-TABS}
{CODE-TAB:csharp:Sync disable_tracking_1@ClientApi\Session\Configuration\DisableTracking.cs /}
{CODE-TAB:csharp:Async disable_tracking_1_async@ClientApi\Session\Configuration\DisableTracking.cs /}
{CODE-TABS/}

__Syntax__

{CODE syntax_1@ClientApi\Session\Configuration\DisableTracking.cs /}

| Parameters | Type | Description |
| - | - | - |
| **entity** | `object` | Instance of entity for which changes will be ignored |

{PANEL/}

{PANEL: Disable tracking all entities in session}

* Tracking can be disabled for all entities in the session's options.  
* When tracking is disabled for the session:  
  * Method `Store` will Not be available (an exception will be thrown if used).
  * Calling `Load` or `Query` will generate a call to the server and create new entities instances.  

{CODE-TABS}
{CODE-TAB:csharp:Sync disable_tracking_2@ClientApi\Session\Configuration\DisableTracking.cs /}
{CODE-TAB:csharp:Async disable_tracking_2_async@ClientApi\Session\Configuration\DisableTracking.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Disable tracking query results}

* Tracking can be disabled for all entities resulting from a query.

{CODE-TABS}
{CODE-TAB:csharp:Query-Sync disable_tracking_3@ClientApi\Session\Configuration\DisableTracking.cs /}
{CODE-TAB:csharp:Query-Async disable_tracking_3_async@ClientApi\Session\Configuration\DisableTracking.cs /}
{CODE-TAB:csharp:DocumentQuery-Sync disable_tracking_3_documentQuery@ClientApi\Session\Configuration\DisableTracking.cs /}
{CODE-TAB:csharp:DocumentQuery-Async disable_tracking_3_documentQuery_async@ClientApi\Session\Configuration\DisableTracking.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Customize tracking in conventions}

* You can further customize and fine-tune which entities will not be tracked  
  by configuring the `ShouldIgnoreEntityChanges` convention method on the document store.
* This customization rule will apply to all sessions opened for this document store.

__Example__

{CODE:csharp disable_tracking_4@ClientApi\Session\Configuration\DisableTracking.cs /}

__Syntax__

{CODE syntax_2@ClientApi\Session\Configuration\DisableTracking.cs /}

| Parameter | Description |
| - | - |
| `InMemoryDocumentSessionOperations` | The session for which tracking is to be disabled |
| `object` | The entity for which tracking is to be disabled |
| `string` | The entity's document ID |

| Return Type | Description |
| - | - |
| `bool` | `true` - Entity will Not be tracked<br>`false` - Entity will be tracked |

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
