# How to Check for Session Changes
---

{NOTE: }

* The Session [tracks all changes](../../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes) 
  made to all the entities it has either loaded, stored, deleted, or queried for,  
  and persists to the server only what is needed when `save_changes` is called.

* This article describes how to check for changes made to **all** tracked entities within the session.  
  To check for changes to a specific **entity**, see [Check for entity changes](../../../client-api/session/how-to/check-if-entity-has-changed).
 
* In this page:
  * [Check for session changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#check-for-session-changes)
  * [Get session changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#get-session-changes)
  * [Syntax](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#syntax)

{NOTE/}

---

{PANEL: Check for session changes }

* The advanced session `has_changes` property indicates whether any entities were added, modified, or deleted within the session.

* Note: The `has_changes` property is cleared (reset to `False`) after calling `save_changes`.

---

{CODE:python changes_1@ClientApi\Session\HowTo\SessionChanges.py /}

{PANEL/}

{PANEL: Get session changes }

* Use the session's advanced method `what_changed` to get all changes made to all the entities tracked by the session.

* For each entity that was modified, the details will include:  
  * The name and path of the changed field   
  * Its old and new values  
  * The type of change  

* Note: `what_changed` reports changes made prior to calling `save_changes`.  
  Calling it immediately after _save_changes_ will return no results, as the changes are cleared.

---

##### Example I

{CODE:python changes_2@ClientApi\Session\HowTo\SessionChanges.py /}

##### Example II

{CODE:python changes_3@ClientApi\Session\HowTo\SessionChanges.py /}

{PANEL/}

{PANEL: Syntax}

{CODE:python syntax_1@ClientApi\Session\HowTo\SessionChanges.py /}
{CODE:python syntax_2@ClientApi\Session\HowTo\SessionChanges.py /}

| ReturnValue                         |                                                       |
|-------------------------------------|-------------------------------------------------------|
| `Dict[str, List[DocumentsChanges]]` | Dictionary containing list of changes per document ID |

{CODE:python syntax_3@ClientApi\Session\HowTo\SessionChanges.py /}

{PANEL/}

## Related Articles

### Session

- [What is a session and how does it work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to check for entity changes](../../../client-api/session/how-to/check-if-entity-has-changed)
- [Evict entity from session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh entity](../../../client-api/session/how-to/refresh-entity)
