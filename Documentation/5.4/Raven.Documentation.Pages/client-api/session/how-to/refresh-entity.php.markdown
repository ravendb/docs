# Session: How to Refresh an Entity

To update an entity with the latest changes from the server, use the `refresh` method from `advanced` session operations.

## Syntax

{CODE:php refresh_1@ClientApi\Session\HowTo\Refresh.php /}

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **$entity** | `object` | An entity that will be refreshed |

## Example

{CODE:php refresh_2@ClientApi\Session\HowTo\Refresh.php /}

## Remarks

Refreshing a transient entity (not attached) or an entity that was deleted on server-side will result in a `InvalidOperationException`.

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Clear a Session](../../../client-api/session/how-to/clear-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
