# Session: How to check if entity has changed?

To check if specific entity differs from the one downloaded from server `HasChanged` method from `Advanced` session operations has been introduced.

## Syntax

{CODE has_changed_1@ClientApi\Session\HowTo\HasChanged.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **entity** | object | Instance of entity for which changes will be checked. |

| Return Value | |
| ------------- | ----- |
| bool | Indicated if given entity has changed. |

## Example

{CODE has_changed_2@ClientApi\Session\HowTo\HasChanged.cs /}

## Related articles

- [How to check if there are any changes on a session?](./check-if-there-are-any-changes-on-a-session)
