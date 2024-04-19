# Session: How to Evict Single Entity from a Session

We can clear all session operations and stop tracking of all entities using the 
[session.advanced.clear()](../../../client-api/session/how-to/clear-a-session) method, 
there's a need to do cleanup only for one entity. This is what `evict()` is for.

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
