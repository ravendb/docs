# Session: How to Check if a Document Exists

---

{NOTE: }

* To check whether the database contains a certain document, use the method `exists()` from the `advanced` session operations.  

* This does not [load](../../../client-api/session/loading-entities) the document from the server or cause the session to 
track it.  

* In this page:  
  * [Syntax](../../../client-api/session/how-to/check-if-document-exists#syntax)  
  * [Example](../../../client-api/session/how-to/check-if-document-exists#example)  
{NOTE/}

---

{PANEL: Syntax}

{CODE:java exists_1@ClientApi\Session\HowTo\Exists.java /}

| Parameter | Type | Description |
| - | - | - |
| **id** | `String` | The ID of the document you want to check the database for |

| Return Value | Description |
| - | - |
| `boolean` | Indicates whether a document with the specified ID exists in the database |

{PANEL/}

{PANEL: Example}

{CODE:java exists_2@ClientApi\Session\HowTo\Exists.java /}

{PANEL/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Loading Entities](../../../client-api/session/loading-entities)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
- [How to Check if There are Any Changes on a Session](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
