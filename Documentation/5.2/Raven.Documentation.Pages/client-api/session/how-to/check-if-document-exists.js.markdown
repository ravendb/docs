# How to Check if a Document Exists

---

{NOTE: }
 
* To check whether the database contains a certain document,  
  use the method `exists()` from the `advanced` session operations.

* Calling _'exists'_ does not [load](../../../client-api/session/loading-entities) the document entity to the session,  
  and the session will not track it.

* In this page:
    * [Check if document exists](../../../client-api/session/how-to/check-if-document-exists#check-if-document-exists)
    * [Syntax](../../../client-api/session/how-to/check-if-document-exists#syntax)
      {NOTE/}

---

{PANEL: Check if document exists}

{CODE:nodejs exists@ClientApi\Session\HowTo\documentExists.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Session\HowTo\documentExists.js /}

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
