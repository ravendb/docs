# Session: How to evict single entity from a session?

We can clear all session operations and stop tracking of all entities by using [clear](../../../client-api/session/how-to/clear-a-session) method, but sometimes there is need to only to do a cleanup only for one entity. For this purpose `evict` was introduced.

## Syntax

{CODE:java evict_1@ClientApi\Session\HowTo\Evict.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | T | Instance of an entity that will be evicted |

## Example I

{CODE:java evict_2@ClientApi\Session\HowTo\Evict.java /}

## Example II

{CODE:java evict_3@ClientApi\Session\HowTo\Evict.java /}
