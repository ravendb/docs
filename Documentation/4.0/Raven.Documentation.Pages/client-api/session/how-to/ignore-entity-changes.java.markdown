# Session: How to Ignore Entity Changes

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

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
- [How to Check if There are Any Changes on a Session](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
