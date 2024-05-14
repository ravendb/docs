# Session: How to Ignore Entity Changes

To indicate that an entity should be ignored when tracking changes, 
use the `advanced` session `ignoreChangesFor` method.  

Using `Load` again to retrieve this entity will not initiate a server call.  

The entity will still take part in the session, but be ignored when `save_changes` is called.  

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
