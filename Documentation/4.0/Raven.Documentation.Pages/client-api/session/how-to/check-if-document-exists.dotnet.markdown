# Session : How to check if a document exists?

In order to check if a document with specific Id exists in the database, use the `Exists` method from the `Advanced` session operations.

## Syntax

{CODE exists_1@ClientApi\Session\HowTo\Exists.cs /}

| Parameters | | |
| ---------- | ---------- | ----- |
| **id** | string | Id of the document to check the existence of. |

| Return Value | |
| ------------- | ----- |
| bool | Indicates if a document with the given Id exists. |

## Example

{CODE exists_2@ClientApi\Session\HowTo\Exists.cs /}

## Related Articles

- [How to check if there are any changes on a session?](./check-if-there-are-any-changes-on-a-session)
