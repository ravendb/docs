# Disable Entity Tracking
---

{NOTE: }

* By default, each session tracks changes to all entities it has either stored, loaded, or queried for.  
  All changes are then persisted when `SaveChanges` is called.  

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

* You can prevent the session from persisting changes made to a specific entity by using `IgnoreChangesFor`.
* Once changes are ignored for the entity:
  * Any modifications made to the entity will be ignored by `SaveChanges`.
  * The session will still keep a reference to the entity to avoid repeated server requests.  
    Performing another `Load` for the same entity will Not generate another call to the server.

**Example**

{CODE-TABS}
{CODE-TAB:csharp:Sync disable_tracking_1@ClientApi\Session\Configuration\DisableTracking.cs /}
{CODE-TAB:csharp:Async disable_tracking_1_async@ClientApi\Session\Configuration\DisableTracking.cs /}
{CODE-TABS/}

**Syntax**

{CODE syntax_1@ClientApi\Session\Configuration\DisableTracking.cs /}

| Parameter  | Type     | Description                                          |
|------------|----------|------------------------------------------------------|
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

**Example**

{CODE:csharp disable_tracking_4@ClientApi\Session\Configuration\DisableTracking.cs /}

// todo .. async...

**Syntax**

{CODE syntax_2@ClientApi\Session\Configuration\DisableTracking.cs /}

| Parameter                           | Description                                      |
|-------------------------------------|--------------------------------------------------|
| `InMemoryDocumentSessionOperations` | The session for which tracking is to be disabled |
| `object`                            | The entity for which tracking is to be disabled  |
| `string`                            | The entity's document ID                         |

| Return Type  | Description                                                             |
|--------------|-------------------------------------------------------------------------|
| `bool`       | `true` - Entity will Not be tracked<br>`false` - Entity will be tracked |

{PANEL/}

{PANEL: Using 'Include' in a NoTracking session will throw}
 
* Attempting to use `Include` in a `NoTracking` session will throw an exception.

* Like other entities in a _NoTracking_ session, 
  the included items are not tracked and will not prevent additional server requests during subsequent _Load_ operations for the same data.
  To avoid confusion, _Include_ operations are disallowed during non-tracking session actions such as `Load` or `Query`. 

* This applies to all items that can be included -  
  e.g., documents, compare-exchange items, counters, revisions, and time series.

---

**Include when loading**:

{CODE-TABS}
{CODE-TAB:csharp:Sync disable_tracking_5@ClientApi\Session\Configuration\DisableTracking.cs /}
{CODE-TAB:csharp:Async disable_tracking_5_async@ClientApi\Session\Configuration\DisableTracking.cs /}
{CODE-TABS/}

**Include when querying**:

{CODE-TABS}
{CODE-TAB:csharp:Sync disable_tracking_6@ClientApi\Session\Configuration\DisableTracking.cs /}
{CODE-TAB:csharp:Async disable_tracking_6_async@ClientApi\Session\Configuration\DisableTracking.cs /}
{CODE-TABS/}

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
