# Session: How to check if there are any changes on a session?

Single entity can be checked for changes using [HasChanged](../../../client-api/session/how-to/check-if-entity-has-changed) method, but there is also a possibility to check if there are any changes on a session or even what has changed. Both `hasChanges` and `whatChanged` methods are available in `advanced` session operations.

{PANEL:HasChanges}

Property indicates if session contains any changes. That is if there are any new, changed or deleted entities.

### Syntax

{CODE:java what_changed_1@ClientApi\Session\HowTo\WhatChanged.java /}

### Example

{CODE:java what_changed_2@ClientApi\Session\HowTo\WhatChanged.java /}

{PANEL/}

{PANEL:WhatChanged}

Method returns all changes for each entity stored within session. Including name of the field/property that changed, its old and new value and change type. 

### Syntax

{CODE:java what_changed_3@ClientApi\Session\HowTo\WhatChanged.java /}

| ReturnValue | |
| ------------- | ----- |
| Map<string, DocumentsChanges[]> | Dictionary containing list of changes per document key. |

### Example I

{CODE:java what_changed_4@ClientApi\Session\HowTo\WhatChanged.java /}

### Example II

{CODE:java what_changed_5@ClientApi\Session\HowTo\WhatChanged.java /}

{PANEL/}

## Related articles

- [How to check if entity has changed?](./check-if-entity-has-changed)
