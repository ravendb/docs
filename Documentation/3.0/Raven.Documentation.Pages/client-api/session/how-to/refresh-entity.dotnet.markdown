# Session: How to refresh entity?

To update entity with latest changes from server `Refresh` method from `Advanced` session operations can be used.

## Syntax

{CODE refresh_1@ClientApi\Session\HowTo\Refresh.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | T | Instance of an entity that will be refreshed |

## Example

{CODE refresh_2@ClientApi\Session\HowTo\Refresh.cs /}

## Remarks

Refreshing transient entity (not attached) or entity that was deleted on server-side will result in `InvalidOperationException`.
