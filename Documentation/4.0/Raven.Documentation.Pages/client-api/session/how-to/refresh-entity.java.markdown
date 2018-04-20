# Session : How to Refresh an Entity

To update an entity with the latest changes from the server, use the `refresh` method from `advanced` session operations.

## Syntax

{CODE:java refresh_1@ClientApi\Session\HowTo\Refresh.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | T | Instance of an entity that will be refreshed |

## Example

{CODE:java refresh_2@ClientApi\Session\HowTo\Refresh.java /}

## Remarks

Refreshing a transient entity (not attached) or an entity that was deleted on server-side will result in a `IllegalStateException`.
