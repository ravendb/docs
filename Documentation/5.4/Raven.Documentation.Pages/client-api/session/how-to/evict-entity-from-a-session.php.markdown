# Session: How to Evict Single Entity from a Session

We can clear all session operations and stop tracking of all entities using the 
[clear](../../../client-api/session/how-to/clear-a-session) method, but sometimes 
there's a need to do cleanup only for one entity. This is what `evict` is for.

## Syntax

{CODE:php evict_1@ClientApi\Session\HowTo\Evict.php /}

| Parameter | Type | Description |
| --------- | ---- | ----------- |
| **$entity** | `object` | Instance of an entity that will be evicted |

## Example I

{CODE:php evict_2@ClientApi\Session\HowTo\Evict.php /}

## Example II

{CODE:php evict_3@ClientApi\Session\HowTo\Evict.php /}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Clear a Session](../../../client-api/session/how-to/clear-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
