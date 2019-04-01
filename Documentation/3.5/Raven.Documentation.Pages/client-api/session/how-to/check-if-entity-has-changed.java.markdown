# Session: How to check if entity has changed?

To check if specific entity differs from the one downloaded from server `hasChanged` method from `advanced` session operations has been introduced.

## Syntax

{CODE:java has_changed_1@ClientApi\Session\HowTo\HasChanged.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | Object | Instance of entity for which changes will be checked. |

| Return Value | |
| ------------- | ----- |
| boolean | Indicated if given entity has changed. |

## Example

{CODE:java has_changed_2@ClientApi\Session\HowTo\HasChanged.java /}

## Related articles

- [How to check if there are any changes on a session?](./check-if-there-are-any-changes-on-a-session)
