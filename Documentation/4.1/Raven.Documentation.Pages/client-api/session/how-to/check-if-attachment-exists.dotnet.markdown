# Session: How to Check if an Attachment Exists

---

{NOTE: }

* To check whether a document contains a certain attachment, use the method `Exists()` from the `session.Advanced.Attachments` 
operations.  

* This does not [load the document](../../../client-api/session/loading-entities) or [the attachment](../../../client-api/session/attachments/loading) 
from the server, and it does not cause the session to track the document.  

* In this page:  
  * [Syntax](../../../client-api/session/how-to/check-if-attachment-exists#syntax)  
  * [Example](../../../client-api/session/how-to/check-if-attachment-exists#example)  
{NOTE/}

---

{PANEL: Syntax}

{CODE-TABS}
{CODE-TAB:csharp:Sync exists_1@ClientApi\Session\HowTo\AttachmentExists.cs /}
{CODE-TAB:csharp:Async exists_1_async@ClientApi\Session\HowTo\AttachmentExists.cs /}
{CODE-TABS/}

| Parameter | Type | Description |
| - | - | - |
| **documentId** | `string` | The ID of the document you want to check for the attachment |
| **name** | `string` | The name of the attachment you want to check the document for |

| Return Value | Description |
| - | - |
| `bool` | Indicates whether the document has an attachment with the specified name |

{PANEL/}

{PANEL: Example}

{CODE-TABS}
{CODE-TAB:csharp:Sync exists_2@ClientApi\Session\HowTo\AttachmentExists.cs /}
{CODE-TAB:csharp:Async exists_2_async@ClientApi\Session\HowTo\AttachmentExists.cs /}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Loading Entities](../../../client-api/session/loading-entities)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
- [How to Check if There are Any Changes on a Session](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
