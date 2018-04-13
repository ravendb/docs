# Glossary : PatchCommandData

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **patch** | [PatchRequest](../client-api/commands/patches/how-to-work-with-patch-requests) | Patch to apply |
| **PatcheIfMissing** | [PatchRequest](../client-api/commands/patches/how-to-work-with-patch-requests) | Patch to apply if document is missing |
| **Id** | string | Document ID |
| **Type** | CommandType | The Command Type (`PATCH`) |
| **ChangeVector** | string | Document change-vector |

### Methods

| Signature | Description |
| ---------- | ----------- |
| **DynamicJsonValue ToJson(DocumentConventions conventions, JsonOperationContext context)** | Translate this instance to Json. |
