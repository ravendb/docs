# Session: How to Check if a Document Exists

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

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
- [How to Check if There are Any Changes on a Session](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
