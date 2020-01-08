# Session: How to Check if an Attachment Exists

---

{NOTE: }

* To check whether a document contains a certain attachment, use the method `exists()` from the `session.advanced().attachments()` 
operations.  

* This does not [load the document](../../../client-api/session/loading-entities) or [the attachment](../../../client-api/session/attachments/loading) 
from the server, and it does not cause the session to track the document.  

* In this page:  
  * [Syntax](../../../client-api/session/how-to/check-if-attachment-exists#syntax)  
  * [Example](../../../client-api/session/how-to/check-if-attachment-exists#example)  
{NOTE/}

---

{PANEL: Syntax}

{CODE:java exists_1@ClientApi\Session\HowTo\AttachmentExists.java /}

| Parameter | Type | Description |
| - | - | - |
| **documentId** | `String` | The ID of the document you want to check for the attachment |
| **name** | `String` | The name of the attachment you want to check the document for |

| Return Value | Description |
| - | - |
| `boolean` | Indicates if a document with the specified ID exists in the database |

{PANEL/}

{PANEL: Example}

{CODE:java exists_2@ClientApi\Session\HowTo\AttachmentExists.java /}

{PANEL/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Loading Entities](../../../client-api/session/loading-entities)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
- [How to Check if There are Any Changes on a Session](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
