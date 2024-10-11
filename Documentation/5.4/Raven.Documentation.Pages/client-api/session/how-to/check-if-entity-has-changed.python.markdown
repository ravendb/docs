# How to Check for Entity Changes
---

{NOTE: }

* The Session [tracks all changes](../../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes) made to all entities that it has either loaded, stored, deleted, or queried for,  
  and persists to the server only what is needed when `save_changes` is called.

* This article describes how to check for changes made to a specific **entity** within a session.  
  To check for changes to **all** tracked entities, see [Check for session changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session).

* In this page:
    * [Check for entity changes](../../../client-api/session/how-to/check-if-entity-has-changed#check-for-entity-changes)
    * [Get entity changes](../../../client-api/session/how-to/check-if-entity-has-changed#get-entity-changes)
    * [Syntax](../../../client-api/session/how-to/check-if-entity-has-changed#syntax)

{NOTE/}

---

{PANEL: Check for entity changes }

* The session's advanced property `has_changed` indicates whether the specified entity was added, modified, or deleted within the session.

* Note: The `has_changed` property is cleared (reset to `False`) after calling `save_changes`.

---

{CODE:python changes_1@ClientApi\Session\HowTo\EntityChanges.py /}

{PANEL/}

{PANEL: Get entity changes }

* Use the advanced session `what_changed` method to get all changes made to the specified entity  
  within the session.

* Details will include:
    * The name and path of the changed field
    * Its old and new values
    * The type of change

* Note: `what_changed` reports changes made prior to calling `save_changes`.  
  Calling it immediately after _save_changes_ will return no results, as the changes are cleared.

---

##### Example I

{CODE:python changes_2@ClientApi\Session\HowTo\EntityChanges.py /}

##### Example II

{CODE:python changes_3@ClientApi\Session\HowTo\EntityChanges.py /}

{PANEL/}

{PANEL: Syntax}

{CODE:python syntax_1@ClientApi\Session\HowTo\EntityChanges.py /}
{CODE:python syntax_2@ClientApi\Session\HowTo\EntityChanges.py /}

| ReturnValue        |                                    |
|--------------------|------------------------------------|
| `DocumentsChanges` | List of changes made in the entity (see `ChangeType` class below for available change types) |

{CODE:python syntax_3@ClientApi\Session\HowTo\EntityChanges.py /}

{PANEL/}

## Related Articles

### Session

- [What is a session and how does it work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to check for session changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict entity from session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh entity](../../../client-api/session/how-to/refresh-entity)
