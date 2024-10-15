# How to Check if an Attachment Exists

---

{NOTE: }

* To check whether a document contains a certain attachment,  
  use the `session.advanced` operations `attachments.exists` method.  

* Calling `exists` does not [load](../../../client-api/session/loading-entities) 
  the document or the attachment to the session, and the session will not track them.

* In this page:  
  * [Check if attachment exists](../../../client-api/session/how-to/check-if-attachment-exists#check-if-attachment-exists)  
  * [Syntax](../../../client-api/session/how-to/check-if-attachment-exists#syntax)  
{NOTE/}

---

{PANEL: Check if attachment exists}

{CODE-TABS}
{CODE-TAB:php exists@ClientApi\Session\HowTo\AttachmentExists.php /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE-TABS}
{CODE-TAB:php syntax@ClientApi\Session\HowTo\AttachmentExists.php/}
{CODE-TABS/}

| Parameter | Type | Description |
| - | - | - |
| **$documentId** | `?string` | The ID of the document you want to check |
| **$name** | `?string` | The name of the attachment you are looking for |

| Return Value | Description |
| - | - |
| `bool` | `true` - The specified attachment exists for this document<br>`false` - The attachment doesn't exist for the document |

{PANEL/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Loading Entities](../../../client-api/session/loading-entities)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
- [How to Check if There are Any Changes on a Session](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
