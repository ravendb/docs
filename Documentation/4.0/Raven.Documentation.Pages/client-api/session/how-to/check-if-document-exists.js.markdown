# Session: How to Check if a Document Exists

In order to check if a document with specific ID exists in the database, use the `exists()` method from the `advanced` session operations.

## Syntax

{CODE:nodejs exists_1@client-api\session\howTo\exists.js /}

| Parameters | | |
| ---------- | ---------- | ----- |
| **id** | string | ID of the document to check the existence of. |

| Return Value | |
| ------------- | ----- |
| `Promise<boolean>` | Indicates if a document with the given ID exists. |

## Example

{CODE:nodejs exists_2@client-api\session\howTo\exists.js /}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
- [How to Check if There are Any Changes on a Session](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
