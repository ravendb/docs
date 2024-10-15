# How to Check for Session Changes
---

{NOTE: }

* The Session [tracks all changes](../../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes) 
  made in all the entities it has either loaded, stored, or queried for,  
  and persists to the server only what is needed when `saveChanges` is called.

* This article describes how to check for changes made in all tracked entities within the **session**.  
  To check for changes on a specific **entity**, see [Check for entity changes](../../../client-api/session/how-to/check-if-entity-has-changed).
 
* In this page:
  * [Check for session changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#check-for-session-changes)
  * [Get session changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#get-session-changes)
  * [Syntax](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#syntax)

{NOTE/}

---

{PANEL: Check for session changes }

* The advanced session `hasChanges` property indicates whether any entities were added, modified, or deleted within the session.

* Note: The `hasChanges` property is cleared (reset to `false`) after calling `saveChanges`.

---

{CODE:php changes_1@ClientApi\Session\HowTo\SessionChanges.php /}

{PANEL/}

{PANEL: Get session changes }

* Use the session's advanced method `whatChanged` to get all changes made to all the entities tracked by the session.

* For each entity that was modified, the details will include:  
   * The name and path of the changed field   
   * Its old and new values  
   * The type of change  

---

##### Example I

{CODE:php changes_2@ClientApi\Session\HowTo\SessionChanges.php /}

##### Example II

{CODE:php changes_3@ClientApi\Session\HowTo\SessionChanges.php /}

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax_1@ClientApi\Session\HowTo\SessionChanges.php /}
{CODE:php syntax_2@ClientApi\Session\HowTo\SessionChanges.php /}

| ReturnValue | Description |
|-------------|-------------|
| `hasChanges(): bool;` | Indicates whether there were changes during the session |
| `whatChanged(): array;` | Returns an array of changes per document ID |
| `DocumentsChanges` | A list of changes made in an entity (see `ChangeType` class below for available change types) |

{CODE:php syntax_3@ClientApi\Session\HowTo\SessionChanges.php /}

{PANEL/}

## Related Articles

### Session

- [What is a session and how does it work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to check for entity changes](../../../client-api/session/how-to/check-if-entity-has-changed)
- [Evict entity from session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh entity](../../../client-api/session/how-to/refresh-entity)
