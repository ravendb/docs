# Put Attachment Operation
---

{NOTE: }

* Use the `PutAttachmentOperation` to add an attachment to a document.

* In this page:
  
  * [Put attachment example](../../../client-api/operations/attachments/put-attachment#put-attachment-example)
  * [Syntax](../../../client-api/operations/attachments/put-attachment#syntax)
  
{NOTE/}

---

{PANEL: Put attachment example}

{CODE:nodejs put_attachment@client-api\operations\attachments\putAttachment.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@client-api\operations\attachments\putAttachment.js /}

| Parameter        | Type                         | Description                                                                       |
|------------------|------------------------------|-----------------------------------------------------------------------------------|
| __documentId__   | `string`                     | Document ID to which the attachment will be added                                 |
| __name__         | `string`                     | Name of attachment to put                                                         |
| __stream__       | `stream.Readable` / `Buffer` | A stream that contains the raw bytes of the attachment                            |
| __contentType__  | `string`                     | Content type of attachment                                                        |
| __changeVector__ | `string`                     | ChangeVector of attachment,<br>used for concurrency checks (`null` to skip check) |

| Return Value of `store.operations.send(putAttachmentOp)` |                                             |
|----------------------------------------------------------|---------------------------------------------|
| `object`                                                 | An object with the new attachment's details |

{CODE:nodejs syntax_2@client-api\operations\attachments\putAttachment.js /}

{PANEL/}

## Related Articles

### Operations

- [What are operations](../../../client-api/operations/what-are-operations)
- [Get attachment](../../../client-api/operations/attachments/get-attachment)
- [Delete attachment](../../../client-api/operations/attachments/delete-attachment)
