# Glossary: MoveAttachmentCommandData

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **documentId** | string | Document id |
| **name** | string | Attachment name |
| **destinationDocumentId** | string | Destination document id |
| **destinationName** | string | Attachment destination name. |
| **changeVector** | string | Document change-vector |

### Methods

| Signature | Description |
| ---------- | ----------- |
| **DynamicJsonValue ToJson(DocumentConventions conventions, JsonOperationContext context)** | Translate this instance to a Json. |
