# How to Check for Session Changes
---

{NOTE: }

* The Session [tracks all changes](../../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes) 
  made to all the entities it has either loaded, stored, deleted, or queried for,  
  and persists to the server only what is needed when `saveChanges()` is called.

* This article describes how to check for changes made to **all** tracked entities within the session.  
  To check for changes to a specific **entity**, see [Check for entity changes](../../../client-api/session/how-to/check-if-entity-has-changed).

* In this page:
    * [Check for session changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#check-for-session-changes)
    * [Get session changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#get-session-changes)
    * [Syntax](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#syntax)

{NOTE/}

---

{PANEL: Check for session changes}

* The session's advanced property `hasChanges` indicates whether any entities were added, modified, or deleted within the session.

* Note: The _hasChanges_ property is cleared after calling `saveChanges()`.

---

{CODE:nodejs changes_1@client-api\session\howTo\sessionChanges.js /}

{PANEL/}

{PANEL: Get session changes}

* Use the session's advanced method `whatChanged()` to get all changes made to all the entities tracked by the session.

* For each entity that was modified, the details will include:
    * The name and path of the changed field
    * Its old and new values
    * The type of change

---

##### Example I

{CODE:nodejs changes_2@client-api\session\howTo\sessionChanges.js /}

##### Example II

{CODE:nodejs changes_3@client-api\session\howTo\sessionChanges.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@client-api\session\howTo\sessionChanges.js /}
{CODE:nodejs syntax_2@client-api\session\howTo\sessionChanges.js /}

| ReturnValue                        |                                                       |
|------------------------------------|-------------------------------------------------------|
| `Record<string, DocumentsChanges>` | Dictionary containing list of changes per document ID |

| `DocumentsChanges`  | Type    | Description                                                                                                                                                                                      |
|---------------------|---------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **fieldOldValue**   | object  | Previous field value                                                                                                                                                                             |
| **fieldNewValue**   | object  | Current field value                                                                                                                                                                              |
| **change**          | string  | Type of change that occurred. Can be: <br>`"DocumentDeleted"`, `"DocumentAdded"`,`"FieldChanged"`, `"NewField"`, `"RemovedField"`, `"ArrayValueChanged"`, `"ArrayValueAdded"`, `"ArrayValueRemoved"` |
| **fieldName**       | string  | Name of field on which the change occurred                                                                                                                                                       |
| **fieldPath**       | string  | Path of field on which the change occurred                                                                                                                                                       |
| **fieldFullName**   | string  | Path + Name of field on which the change occurred                                                                                                                                                |

{PANEL/}

## Related Articles

### Session

- [What is a session and how does it work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to check for entity changes](../../../client-api/session/how-to/check-if-entity-has-changed)
- [Evict entity from session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh entity](../../../client-api/session/how-to/refresh-entity)
