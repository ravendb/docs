# How to Get Tracked Entities
---

{NOTE: }

* The Session [tracks all changes](../../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes) 
  made to all the entities it has either loaded, stored, deleted, or queried for,  
  and persists to the server only what is needed when `SaveChanges()` is called.

* You can use the session's advanced `GetTrackedEntities()` method  
  to retrieve the **list of all entities tracked by the session**.

* To check what is the actual type of change made to the entities, see:  
  [Get entity changes](../../../client-api/session/how-to/check-if-entity-has-changed#get-entity-changes), or
  [Get session changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#get-session-changes).
 
* In this page:
  * [Get tracked entities](../../../client-api/session/how-to/get-tracked-entities#get-tracked-entities)
  * [Syntax](../../../client-api/session/how-to/get-tracked-entities#syntax)

{NOTE/}

---

{PANEL: Get tracked entities }

**Tracking stored entities**:
{CODE get_tracked_entities_1@ClientApi\Session\HowTo\GetTrackedEntities.cs /}

**Tracking loaded and deleted entities**:
{CODE get_tracked_entities_2@ClientApi\Session\HowTo\GetTrackedEntities.cs /}

**Tracking queried entities**:
{CODE get_tracked_entities_3@ClientApi\Session\HowTo\GetTrackedEntities.cs /}

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Session\HowTo\GetTrackedEntities.cs /}
{CODE syntax_2@ClientApi\Session\HowTo\GetTrackedEntities.cs /}

{PANEL/}

## Related Articles

### Session

- [What is a session and how does it work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to check for entity changes](../../../client-api/session/how-to/check-if-entity-has-changed)
- [Evict entity from session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh entity](../../../client-api/session/how-to/refresh-entity)
