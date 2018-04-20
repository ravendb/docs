# Session : How to Ignore Entity Changes

In order to mark an entity as one that should be ignored for change tracking purposes, use the `ignoreChangesFor` method from the `advanced` session operations.  

Unlike the `evict` method, performing another `Load` of that entity won't create a call to the server.  

The entity will still take part in the session, but will be ignored for `saveChanges`.  

## Syntax

{CODE:java ignore_changes_1@ClientApi\Session\HowTo\IgnoreChanges.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | Object | Instance of entity for which changes will be ignored. |


## Example

{CODE:java ignore_changes_2@ClientApi\Session\HowTo\IgnoreChanges.java /}
