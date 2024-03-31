# Get Attachment Operation
---

{NOTE: }

* Use the `GetAttachmentOperation` to retrieve an attachment from a document. 

* In this page:  

  * [Get attachment example](../../../client-api/operations/attachments/get-attachment#get-attachment-example)  
  * [Syntax](../../../client-api/operations/attachments/get-attachment#syntax)
   
{NOTE/}

---

{PANEL: Get attachment example}

{CODE:nodejs get_attachment@client-api\operations\attachments\getAttachment.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@client-api\operations\attachments\getAttachment.js /}

| Parameter        | Type     | Description                                                                                                                                                                                               |
|------------------|----------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __documentId__   | `string` | Document ID that contains the attachment.                                                                                                                                                                 |
| __name__         | `string` | Name of attachment to get.                                                                                                                                                                                |
| __type__         | `string` | Specify whether getting an attachment from a document or from a revision.<br>(`"Document"` or `"Revision"`).                                                                                              |
| __changeVector__ | `string` | The ChangeVector of the document or the revision to which the attachment belongs.<br>Mandatory when getting an attachment from a revision.<br>Used for concurrency checks (use `null` to skip the check). |

| Return Value of `store.operations.send(getAttachmentOp)`  |                                         |
|-----------------------------------------------------------|-----------------------------------------|
| `AttachmentResult`                                        | An instance of class `AttachmentResult` |

{CODE:nodejs syntax_2@client-api\operations\attachments\getAttachment.js /}

{PANEL/}

## Related Articles

### Operations

- [What are operations](../../../client-api/operations/what-are-operations)
- [Put attachment](../../../client-api/operations/attachments/put-attachment) 
- [Delete attachment](../../../client-api/operations/attachments/delete-attachment)
