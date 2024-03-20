# Delete Attachment Operation
---

{NOTE: }

* Use the `DeleteAttachmentOperation` to delete an attachment from a document.

* In this page:

    * [Delete attachment example](../../../client-api/operations/attachments/delete-attachment#delete-attachment-example)
    * [Syntax](../../../client-api/operations/attachments/delete-attachment#syntax)

{NOTE/}

---

{PANEL: Delete attachment example}

{CODE:nodejs delete_attachment@client-api\operations\attachments\deleteAttachment.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@client-api\operations\attachments\deleteAttachment.js /}

| Parameter        | Type     | Description                                                                       |
|------------------|----------|-----------------------------------------------------------------------------------|
| __documentId__   | `string` | ID of document from which attachment will be removed                              |
| __name__         | `string` | Name of attachment to delete                                                      |
| __changeVector__ | `string` | ChangeVector of attachment,<br>used for concurrency checks (`null` to skip check) |

{PANEL/}

## Related Articles

### Operations

- [What are operations](../../../client-api/operations/what-are-operations)
- [Put attachment](../../../client-api/operations/attachments/put-attachment) 
- [Get attachment](../../../client-api/operations/attachments/get-attachment)
