# Session : How to Refresh an Entity

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
