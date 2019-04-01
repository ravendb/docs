# Session: How to Check if Entity has Changed

To check if a specific entity differs from the one downloaded from server, the `hasChanged()` method from the `advanced` session operations can be used.

## Syntax

{CODE:nodejs has_changed_1@client-api\session\howTo\hasChanged.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | object | Entity for which changes will be checked. |

| Return Value | |
| ------------- | ----- |
| boolean | Indicates whether given entity has changed. |

## Example

{CODE:nodejs has_changed_2@client-api\session\howTo\hasChanged.js /}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Check if There are Any Changes on a Session](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
