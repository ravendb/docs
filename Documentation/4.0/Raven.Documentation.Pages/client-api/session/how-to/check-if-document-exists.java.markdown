# Session : How to check if a document exists?

In order to check if a document with specific ID exists in the database, use the `exists` method from the `advanced` session operations.

## Syntax

{CODE:java exists_1@ClientApi\Session\HowTo\Exists.java /}

| Parameters | | |
| ---------- | ---------- | ----- |
| **id** | String | ID of the document to check the existence of. |

| Return Value | |
| ------------- | ----- |
| boolean | Indicates if a document with the given ID exists. |

## Example

{CODE:java exists_2@ClientApi\Session\HowTo\Exists.java /}

## Related Articles

- [How to check if there are any changes on a session?](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
