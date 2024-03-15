# Attachments: Copy, Move, Rename
---

{NOTE: }

* Attachments can be copied, moved, or renamed using built-in session methods.  

* All of those actions are executed when `saveChanges` is called and take place on the server-side,  
  removing the need to transfer the entire attachment binary data over the network in order to perform the action.

* In this page:  
  * [Copy attachment](../../document-extensions/attachments/copying-moving-renaming#copy-attachment)
  * [Move attachment](../../document-extensions/attachments/copying-moving-renaming#move-attachment)
  * [Rename attachment](../../document-extensions/attachments/copying-moving-renaming#rename-attachment)
  * [Syntax](../../document-extensions/attachments/copying-moving-renaming#syntax)

{NOTE/}

---

{PANEL: Copy attachment}

Use `session.advanced.attachments.copy` to copy an attachment from one document to another.

{CODE-TABS}
{CODE-TAB:nodejs:Copy_with_entity copy_1@documentExtensions\attachments\copyMoveRename.js /}
{CODE-TAB:nodejs:Copy_with_document_ID copy_2@documentExtensions\attachments\copyMoveRename.js /}
{CODE-TABS/}

{PANEL/}

{PANEL: Move attachment}

Use `session.advanced.attachments.move` to move an attachment from one document to another.

{CODE-TABS}
{CODE-TAB:nodejs:Move_with_entity move_1@documentExtensions\attachments\copyMoveRename.js /}
{CODE-TAB:nodejs:Move_with_document_ID move_2@documentExtensions\attachments\copyMoveRename.js /}
{CODE-TABS/}

{PANEL/}

{PANEL: Rename attachment}

Use `session.advanced.attachments.rename` to rename an attachment.

{CODE-TABS}
{CODE-TAB:nodejs:Rename_with_entity rename_1@documentExtensions\attachments\copyMoveRename.js /}
{CODE-TAB:nodejs:Rename_with_document_ID rename_2@documentExtensions\attachments\copyMoveRename.js /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_copy@documentExtensions\attachments\copyMoveRename.js /}

{CODE:nodejs syntax_move@documentExtensions\attachments\copyMoveRename.js /}

| Parameter                 | Type     | Description                 |
|---------------------------|----------|-----------------------------|
| __sourceEntity__          | `object` | Source entity               |
| __destinationEntity__     | `object` | Destination entity          |
| __sourceDocumentId__      | string   | Source document Id          |
| __destinationDocumentId__ | string   | Destination document Id     |
| __sourceName__            | string   | Source attachment name      |
| __destinationName__       | string   | Destination attachment name |


{CODE:nodejs syntax_rename@documentExtensions\attachments\copyMoveRename.js /}

| Parameter      | Type     | Description                     |
|----------------|----------|---------------------------------|
| __entity__     | `object` | The document entity             |
| __documentId__ | string   | The document Id                 |
| __name__       | string   | Current name of attachment      |
| __newName__    | string   | The new name for the attachment |

{PANEL/}

## Related Articles

### Attachments

- [What are Attachments](../../document-extensions/attachments/what-are-attachments)
- [Storing](../../document-extensions/attachments/storing)
- [Loading](../../document-extensions/attachments/loading)
- [Deleting](../../document-extensions/attachments/deleting)
