# Session: How to Check if There are Any Changes on a Session

Single entity can be checked for changes using [hasChanged()](../../../client-api/session/how-to/check-if-entity-has-changed) method, but there is also a possibility to check if there are any changes on a session or even what has changed. Both the `hasChanges()`  method and the `whatChanged()` method are available in the `advanced` session operations.

{PANEL:HasChanges}

Property indicates if the session contains any changes. If there are any new, changed or deleted entities.

{CODE:nodejs what_changed_1@client-api\session\howTo\whatChanged.js /}

### Example

{CODE:nodejs what_changed_2@client-api\session\howTo\whatChanged.js /}

{PANEL/}

{PANEL:WhatChanged}

Method returns all changes for each entity stored within the session. Including name of the field/property that changed, its old and new value, and change type. 

{CODE:nodejs what_changed_3@client-api\session\howTo\whatChanged.js /}

| ReturnValue | |
| ------------- | ----- |
| `{ [id]: DocumentsChanges[] }` | Object containing list of changes per document ID. |

### Example I

{CODE:nodejs what_changed_4@client-api\session\howTo\whatChanged.js /}

### Example II

{CODE:nodejs what_changed_5@client-api\session\howTo\whatChanged.js /}

{PANEL/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
