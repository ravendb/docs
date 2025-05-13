# Disable Entity Tracking
---

{NOTE: }

* By default, each session tracks changes to all entities it has either stored, loaded, or queried for.  
  All changes are then persisted when `saveChanges` is called.  

* Tracking can be disabled at various scopes:  
  for a specific entity, for entities returned by a query, for all entities in a session, or globally using conventions.

* In this article:
  * [Disable tracking changes for a specific entity](../../../client-api/session/configuration/how-to-disable-tracking#disable-tracking-changes-for-a-specific-entity)
  * [Disable tracking all entities in session](../../../client-api/session/configuration/how-to-disable-tracking#disable-tracking-all-entities-in-session)
  * [Disable tracking query results](../../../client-api/session/configuration/how-to-disable-tracking#disable-tracking-query-results)
  * [Customize tracking in conventions](../../../client-api/session/configuration/how-to-disable-tracking#customize-tracking-in-conventions)
  * [Using 'Include' in a NoTracking session will throw](../../../client-api/session/configuration/how-to-disable-tracking#using-)

{NOTE/}

---

{PANEL: Disable tracking changes for a specific entity}

* You can prevent the session from persisting changes made to a specific entity by using `ignoreChangesFor`.
* Once changes are ignored for the entity:
    * Any modifications made to the entity will be ignored by `saveChanges`.
    * The session will still keep a reference to the entity to avoid repeated server requests.  
      Performing another `load` for the same entity will Not generate another call to the server.
  
**Example**

{CODE:nodejs disable_tracking_1@client-api\session\Configuration\disableTracking.js /}

**Syntax**

{CODE:nodejs syntax_1@client-api\session\Configuration\disableTracking.js /}

| Parameter  | Type     | Description                                          |
|------------|----------|------------------------------------------------------|
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

**Example**

{CODE:nodejs disable_tracking_4@client-api\session\Configuration\disableTracking.js /}

**Syntax**

{CODE:nodejs syntax_2@client-api\session\Configuration\disableTracking.js /}

| Parameter         | Type                                | Description                                      |
|-------------------|-------------------------------------|--------------------------------------------------|
| sessionOperations | `InMemoryDocumentSessionOperations` | The session for which tracking is to be disabled |
| entity            | `object`                            | The entity for which tracking is to be disabled  |
| documentId        | `string`                            | The entity's document ID                         |

| Return Type   | Description                                                             |
|---------------|-------------------------------------------------------------------------|
| `boolean`     | `true` - Entity will Not be tracked<br>`false` - Entity will be tracked |

{PANEL/}

{PANEL: Using 'include' in a noTracking session will throw}

* Attempting to use `include` in a `noTracking` session will throw an exception.

* Like other entities in a _noTracking_ session,
  the included items are not tracked and will not prevent additional server requests during subsequent _load_ operations for the same data.
  To avoid confusion, _include_ operations are disallowed during non-tracking session actions such as `load` or `query`.

* This applies to all items that can be included -  
  e.g., documents, compare-exchange items, counters, revisions, and time series.

---

**Include when loading**:

{CODE:nodejs disable_tracking_5@client-api\session\Configuration\disableTracking.js /}

**Include when querying**:

{CODE:nodejs disable_tracking_6@client-api\session\Configuration\disableTracking.js /}

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
