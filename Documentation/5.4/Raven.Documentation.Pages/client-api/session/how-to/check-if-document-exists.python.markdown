# How to Check if a Document Exists

---

{NOTE: }

* To check whether the database contains a certain document,  
  use the `session.advanced` operations `exists` method.  

* Calling `exists` does not [load](../../../client-api/session/loading-entities) 
  the document entity to the session, and the session will not track it.

* In this page:
    * [Check if document exists](../../../client-api/session/how-to/check-if-document-exists#check-if-document-exists)
    * [Syntax](../../../client-api/session/how-to/check-if-document-exists#syntax)
{NOTE/}

---

{PANEL: Check if document exists}

{CODE-TABS}
{CODE-TAB:python exists_2@ClientApi\Session\HowTo\DocumentExists.py /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE-TABS}
{CODE-TAB:python exists_1@ClientApi\Session\HowTo\DocumentExists.py /}
{CODE-TABS/}

| Parameter | Type | Description |
| - | - | - |
| **key** | `str` | The ID of the document to look for |

| Return Value | Description |
| - | - |
| `bool` | `True` - this document exists in the database.<br>`False` - The document does Not exist in the database |

{PANEL/}

## Related Articles

### Session

- [What is a Session and How Does it Work](../../../client-api/session/what-is-a-session-and-how-does-it-work)
- [Loading Entities](../../../client-api/session/loading-entities)
- [How to Check if Entity has Changed](../../../client-api/session/how-to/check-if-entity-has-changed)
- [How to Check if There are Any Changes on a Session](../../../client-api/session/how-to/check-if-there-are-any-changes-on-a-session)
- [Evict Entity From a Session](../../../client-api/session/how-to/evict-entity-from-a-session)
- [Refresh Entity](../../../client-api/session/how-to/refresh-entity)
