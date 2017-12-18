# Session : How to Check if Entity has Changed

To check if a specific entity differs from the one downloaded from server, the `HasChanged` method from the `Advanced` session operations can be used.

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

## Related Articles

- [How to check if there are any changes on a session?](./check-if-there-are-any-changes-on-a-session)
