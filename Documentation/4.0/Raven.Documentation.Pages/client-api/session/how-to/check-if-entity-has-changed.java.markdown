# Session: How to Check if Entity has Changed

To check if a specific entity differs from the one downloaded from server, the `hasChanged` method from the `advanced` session operations can be used.

## Syntax

{CODE:java has_changed_1@ClientApi\Session\HowTo\HasChanged.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | Objec | Instance of entity for which changes will be checked. |

| Return Value | |
| ------------- | ----- |
| boolean | Indicated if given entity has changed. |

## Example

{CODE:java has_changed_2@ClientApi\Session\HowTo\HasChanged.java /}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Check if There are Any Changes on a Session](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
