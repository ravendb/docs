# How to Check for Session Changes
---

{NOTE: }

* The Session [tracks all changes](../../../client-api/session/what-is-a-session-and-how-does-it-work#tracking-changes) made to all entities that it has either loaded, stored, or queried for,  
  and persists to the server only what is needed when `saveChanges()` is called.

* This article describes how to check for changes made to all tracked entities within the __session__.  
  To check for changes on a specific __entity__ see [Check for entity changes](../../../client-api/session/how-to/check-if-entity-has-changed).

* In this page:
    * [Check for Session Changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#check-for-session-changes)
    * [Get Session Changes](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#get-session-changes)
    * [Syntax](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session#syntax)

{NOTE/}

---

{PANEL: Check for Session Changes}

* The session's advanced property `hasChanges` indicates whether any entities were added, modified, or deleted within the session.

* Note: The _hasChanges_ property is cleared after calling `saveChanges()`.

---

{CODE:nodejs changes_1@client-api\session\howTo\sessionChanges.js /}

{PANEL/}

{PANEL: Get Session Changes}

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
| __fieldOldValue__   | object  | Previous field value                                                                                                                                                                             |
| __fieldNewValue__   | object  | Current field value                                                                                                                                                                              |
| __change__          | string  | Type of change that occurred. Can be: <br>`"DocumentDeleted"`, `"DocumentAdded"`,`"FieldChanged"`, `"NewField"`, `"RemovedField"`, `"ArrayValueChanged"`, `"ArrayValueAdded"`, `"ArrayValueRemoved"` |
| __fieldName__       | string  | Name of field on which the change occurred                                                                                                                                                       |
| __fieldPath__       | string  | Path of field on which the change occurred                                                                                                                                                       |
| __fieldFullName__   | string  | Path + Name of field on which the change occurred                                                                                                                                                |

{PANEL/}

## Related Articles

### Session

- [What is a session and how does it work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to check for entity changes](../../../client-api/session/how-to/check-if-entity-has-changed)
- [Evict entity from session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh entity](../../../client-api/session/how-to/refresh-entity)
