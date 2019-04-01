# Session: How to Check if a Document Exists

In order to check if a document with specific ID exists in the database, use the `Exists` method from the `Advanced` session operations.

## Syntax

{CODE-TABS}
{CODE-TAB:csharp:Sync exists_1@ClientApi\Session\HowTo\Exists.cs /}
{CODE-TAB:csharp:Async asyn_exists_1@ClientApi\Session\HowTo\Exists.cs /}
{CODE-TABS/}

| Parameters | | |
| ---------- | ---------- | ----- |
| **id** | string | ID of the document to check the existence of. |

| Return Value | |
| ------------- | ----- |
| bool | Indicates if a document with the given ID exists. |

## Example

{CODE exists_2@ClientApi\Session\HowTo\Exists.cs /}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
- [How to Check if There are Any Changes on a Session](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
