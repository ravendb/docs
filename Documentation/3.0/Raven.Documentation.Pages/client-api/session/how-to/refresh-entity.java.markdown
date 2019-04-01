# Session: How to refresh entity?

To update entity with latest changes from server `refresh` method from `advanced()` session operations can be used.

## Syntax

{CODE:java refresh_1@ClientApi\Session\HowTo\Refresh.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | T | Instance of an entity that will be refreshed |

## Example

{CODE:java refresh_2@ClientApi\Session\HowTo\Refresh.java /}

## Remarks

Refreshing transient entity (not attached) or entity that was deleted on server-side will result in `InvalidOperationException`.
