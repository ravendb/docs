# Session: How to Refresh an Entity

To update an entity with the latest changes from the server, use the `refresh()` method from `advanced` session operations.

## Syntax

{CODE:nodejs refresh_1@client-api\session\howTo\refresh.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | object | Instance of an entity that will be refreshed |

## Example

{CODE:nodejs refresh_2@client-api\session\howTo\refresh.js /}

## Remarks

Refreshing a transient entity (not attached) or an entity that was deleted server-side will result in an `InvalidOperationException` error.

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Clear a Session](../../../client-api/session/how-to/clear-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
