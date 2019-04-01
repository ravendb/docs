# Session: How to evict single entity from a session?

We can clear all session operations and stop tracking of all entities by using [Clear](../../../client-api/session/how-to/clear-a-session) method, but sometimes there is need to only to do a cleanup only for one entity. For this purpose `Evict` was introduced.

## Syntax

{CODE evict_1@ClientApi\Session\HowTo\Evict.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | T | Instance of an entity that will be evicted |

## Example I

{CODE evict_2@ClientApi\Session\HowTo\Evict.cs /}

## Example II

{CODE evict_3@ClientApi\Session\HowTo\Evict.cs /}
