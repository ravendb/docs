# Disable Entity Tracking
---

{NOTE: }

* By default, each session tracks changes to all entities it has either stored, loaded, or queried for.  
  All changes are then persisted when `save_changes` is called.  

* Tracking can be disabled at various scopes:  
  for a specific entity, for entities returned by a query, for all entities in a session, or globally using conventions.

* In this article:
    * [Disable tracking changes for a specific entity](../../../client-api/session/configuration/how-to-disable-tracking#disable-tracking-changes-for-a-specific-entity)
    * [Disable tracking all entities in session](../../../client-api/session/configuration/how-to-disable-tracking#disable-tracking-all-entities-in-session)
    * [Disable tracking query results](../../../client-api/session/configuration/how-to-disable-tracking#disable-tracking-query-results)
    * [Customize tracking in conventions](../../../client-api/session/configuration/how-to-disable-tracking#customize-tracking-in-conventions)

{NOTE/}

---

{PANEL: Disable tracking changes for a specific entity}

* You can prevent the session from persisting changes made to a specific entity by using `ignore_changes_for`.
* Once changes are ignored for the entity:
    * Any modifications made to the entity will be ignored by `save_changes`.
    * The session will still keep a reference to the entity to avoid repeated server requests.  
      Performing another `load` for the same entity will Not generate another call to the server.
  
**Example**

{CODE:python disable_tracking_1@ClientApi\Session\Configuration\DisableTracking.py /}

**Syntax**

{CODE:python syntax_1@ClientApi\Session\Configuration\DisableTracking.py /}

| Parameter  | Type     | Description                                          |
|------------|----------|------------------------------------------------------|
| **entity** | `object` | Instance of entity for which changes will be ignored |

{PANEL/}

{PANEL: Disable tracking for all entities in a session}

* Tracking can be disabled for all entities in the session's options.  
* When tracking is disabled for the session:  
  * Method `store` will Not be available (an exception will be thrown if used).
  * Calling `load` or `query` will generate a call to the server and create new entities instances.  

{CODE:python disable_tracking_2@ClientApi\Session\Configuration\DisableTracking.py /}

{PANEL/}

{PANEL: Disable tracking for query results}

* Tracking can be disabled for all entities resulting from a query.

{CODE-TABS}
{CODE-TAB:python disable_tracking_3@ClientApi\Session\Configuration\DisableTracking.py /}
{CODE-TABS/}

{PANEL/}

{PANEL: Customize tracking in conventions}

* You can further customize and fine-tune which entities will not be tracked  
  by configuring the `should_ignore_entity_changes` convention method on the document store.
* This customization rule will apply to all sessions opened for this document store.
* Implement rules under your [ShouldIgnoreEntityChanges](../../../client-api/session/configuration/how-to-disable-tracking#syntax) subclass.  
  Apply the class's `check` method to control the ignore flow.

**Example**:

{CODE:python disable_tracking_4@ClientApi\Session\Configuration\DisableTracking.py /}

**Syntax**:

{CODE:python syntax_2@ClientApi\Session\Configuration\DisableTracking.py /}

| Parameter   | Type     | Description                                     |
|-------------|----------|-------------------------------------------------|
| entity      | `object` | The entity for which tracking is to be disabled |
| document_id | `str`    | The entity's document ID                        |

| Return Type   | Description                                                             |
|---------------|-------------------------------------------------------------------------|
| `bool`        | `True` - Entity will Not be tracked<br>`False` - Entity will be tracked |

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
