# How to Check for Session Changes
---

{NOTE: }

* The Session [tracks all changes](../../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes) made to all entities that it has either loaded, stored, or queried for,  
  and persists to the server only what is needed when `SaveChanges()` is called.

* This article describes how to check for changes made to all tracked entities within the __session__.  
  To check for changes on a specific __entity__ see [Check for entity changes](../../../client-api/session/how-to/check-if-entity-has-changed).
 
* In this page:
  * [Check for Session Changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#check-for-session-changes)
  * [Get Session Changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#get-session-changes)
  * [Syntax](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#syntax)

{NOTE/}

---

{PANEL: Check for Session Changes}

* The session's advanced property `HasChanges` indicates whether any entities were added, modified, or deleted within the session.

* Note: The _HasChanges_ property is cleared after calling `SaveChanges()`.

---

{CODE changes_1@ClientApi\Session\HowTo\SessionChanges.cs /}

{PANEL/}

{PANEL: Get Session Changes}

* Use the session's advanced method `WhatChanged()` to get all changes made to all the entities tracked by the session.

* For each entity that was modified, the details will include the name of the changed field, its old and new values,  
  and the type of change.

---

##### Example I

{CODE changes_2@ClientApi\Session\HowTo\SessionChanges.cs /}

##### Example II

{CODE changes_3@ClientApi\Session\HowTo\SessionChanges.cs /}

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Session\HowTo\SessionChanges.cs /}
{CODE syntax_2@ClientApi\Session\HowTo\SessionChanges.cs /}

| ReturnValue                               |                                                        |
|-------------------------------------------|--------------------------------------------------------|
| `IDictionary<string, DocumentsChanges[]>` | Dictionary containing list of changes per document ID. |
{CODE syntax_3@ClientApi\Session\HowTo\SessionChanges.cs /}

{PANEL/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
