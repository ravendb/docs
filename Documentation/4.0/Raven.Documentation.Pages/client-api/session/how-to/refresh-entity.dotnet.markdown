# Session: How to Refresh an Entity

To update an entity with the latest changes from the server, use the `Refresh` method from `Advanced` session operations.

## Syntax

{CODE refresh_1@ClientApi\Session\HowTo\Refresh.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | T | Instance of an entity that will be refreshed |

## Example

{CODE refresh_2@ClientApi\Session\HowTo\Refresh.cs /}

## Remarks

Refreshing a transient entity (not attached) or an entity that was deleted on server-side will result in a `InvalidOperationException`.

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Clear a Session](../../../client-api/session/how-to/clear-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
