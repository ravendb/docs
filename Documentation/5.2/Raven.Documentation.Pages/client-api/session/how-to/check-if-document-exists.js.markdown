# How to Check if a Document Exists

---

{NOTE: }

* To check whether the database contains a certain document,  
  use method `exists()` from the `advanced` session operations.

* Calling _'exists'_ does Not [load](../../../client-api/session/loading-entities) the document from the server or cause the session to
  track it.

* In this page:
    * [Check if document exists](../../../client-api/session/how-to/check-if-document-exists#check-if-document-exists)
    * [Syntax](../../../client-api/session/how-to/check-if-document-exists#syntax)
      {NOTE/}

---

{PANEL: Check if document exists}

{CODE:nodejs exists_2@ClientApi\Session\HowTo\exists.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs exists_1@ClientApi\Session\HowTo\exists.js /}

| Parameter | Type | Description |
| - | - | - |
| **id** | `string` | The ID of the document to check |

| Return Value | Description |
| - | - |
| `Promise<boolean>` | `true` - the document exists in the database.<br>`false` - The document does Not exist in the database |

{PANEL/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Loading Entities](../../../client-api/session/loading-entities)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
- [How to Check if There are Any Changes on a Session](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
