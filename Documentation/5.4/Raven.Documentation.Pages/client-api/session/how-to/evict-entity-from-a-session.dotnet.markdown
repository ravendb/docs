# Session: How to Evict Single Entity from a Session

We can clear all session operations and stop tracking of all entities by the using [Clear](../../../client-api/session/how-to/clear-a-session) method, but sometimes there is need to only to do a cleanup only for one entity. For this purpose `Evict` was introduced.

## Syntax

{CODE evict_1@ClientApi\Session\HowTo\Evict.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | T | Instance of an entity that will be evicted |

## Example I

{CODE evict_2@ClientApi\Session\HowTo\Evict.cs /}

## Example II

{CODE evict_3@ClientApi\Session\HowTo\Evict.cs /}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Clear a Session](../../../client-api/session/how-to/clear-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
