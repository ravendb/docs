# How to Check for Entity Changes
---

{NOTE: }

* The Session [tracks all changes](../../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes) made to all entities that it has either loaded, stored, or queried for,  
  and persists to the server only what is needed when `saveChanges` is called.

* This article describes how to check for changes made in a specific **entity** within a session.  
  To check for changes on all tracked entities, see [Check for session changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session).

* In this page:
    * [Check for entity changes](../../../client-api/session/how-to/check-if-entity-has-changed#check-for-entity-changes)
    * [Get entity changes](../../../client-api/session/how-to/check-if-entity-has-changed#get-entity-changes)
    * [Syntax](../../../client-api/session/how-to/check-if-entity-has-changed#syntax)

{NOTE/}

---

{PANEL: Check for entity changes }

* The session's advanced property `hasChanged` indicates whether the specified entity was added, modified, or deleted within the session.

* Note: The `hasChanged` property is cleared (reset to `false`) after calling `saveChanges`.

---

{CODE:php changes_1@ClientApi\Session\HowTo\EntityChanges.php /}

{PANEL/}

{PANEL: Get entity changes }

* Use the advanced session `whatChangedFor` method to get all changes made in the specified entity  
  within the session.

* Details will include:
    * The name and path of the changed field
    * Its old and new values
    * The type of change

---

##### Example I

{CODE:php changes_2@ClientApi\Session\HowTo\EntityChanges.php /}

##### Example II

{CODE:php changes_3@ClientApi\Session\HowTo\EntityChanges.php /}

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax_1@ClientApi\Session\HowTo\EntityChanges.php /}
{CODE:php syntax_2@ClientApi\Session\HowTo\EntityChanges.php /}

| ReturnValue        |                                    |
|--------------------|------------------------------------|
| `DocumentsChanges` | List of changes made in the entity (see `ChangeType` class below for available change types) |

{CODE:php syntax_3@ClientApi\Session\HowTo\EntityChanges.php /}

{PANEL/}

## Related Articles

### Session

- [What is a session and how does it work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to check for session changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict entity from session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh entity](../../../client-api/session/how-to/refresh-entity)
