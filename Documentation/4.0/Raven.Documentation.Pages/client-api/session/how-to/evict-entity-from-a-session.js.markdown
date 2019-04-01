# Session: How to Evict Single Entity from a Session

We can clear all session operations and stop tracking of all entities by the using [session.advanced.clear()](../../../client-api/session/how-to/clear-a-session) method, but sometimes there is a need to only to do a cleanup for a single entity. For this purpose `evict()` was introduced.

## Syntax

{CODE:nodejs evict_1@client-api\session\howTo\evict.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | object | Entity that will be evicted |

## Example I

{CODE:nodejs evict_2@client-api\session\howTo\evict.js /}

## Example II

{CODE:nodejs evict_3@client-api\session\howTo\evict.js /}

## Related articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Clear a Session](../../../client-api/session/how-to/clear-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
