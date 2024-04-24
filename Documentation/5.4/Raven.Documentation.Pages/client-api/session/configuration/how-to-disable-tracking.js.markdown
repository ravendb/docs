# Disable Entity Tracking

---

{NOTE: }

* By default, each session tracks changes to all entities it has either stored, loaded, or queried for.  
  All changes are then persisted when `saveChanges` is called.  

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
  * Any changes made to this entity will be ignored for `saveChanges`.  
  * Performing another `load` for this entity will Not generate another call to the server.
  
__Example__

{CODE:nodejs disable_tracking_1@client-api\session\Configuration\disableTracking.js /}

__Syntax__

{CODE:nodejs syntax_1@client-api\session\Configuration\disableTracking.js /}

| Parameters | Type | Description |
| - | - | - |
| **entity** | `object` | Instance of entity for which changes will be ignored |

{PANEL/}

{PANEL: Disable tracking all entities in session}

* Tracking can be disabled for all entities in the session's options.  
* When tracking is disabled for the session:  
  * Method `store` will Not be available (an exception will be thrown if used).
  * Calling `load` or `query` will generate a call to the server and create new entities instances.

{CODE:nodejs disable_tracking_2@client-api\session\Configuration\disableTracking.js /}

{PANEL/}

{PANEL: Disable tracking query results}

* Tracking can be disabled for all entities resulting from a query.

{CODE:nodejs disable_tracking_3@client-api\session\Configuration\disableTracking.js /}

{PANEL/}

{PANEL: Customize tracking in conventions}

* You can further customize and fine-tune which entities will not be tracked  
  by configuring the `shouldIgnoreEntityChanges` convention method on the document store.
* This customization rule will apply to all sessions opened for this document store.

__Example__

{CODE:nodejs disable_tracking_4@client-api\session\Configuration\disableTracking.js /}

__Syntax__

{CODE:nodejs syntax_2@client-api\session\Configuration\disableTracking.js /}

| Parameter | Type | Description |
| - | - | - |
| sessionOperations | `InMemoryDocumentSessionOperations` | The session for which tracking is to be disabled |
| entity | `object` | The entity for which tracking is to be disabled |
| documentId | `string` | The entity's document ID |

| Return Type | Description |
| - | - |
| `boolean` | `true` - Entity will Not be tracked<br>`false` - Entity will be tracked |

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
