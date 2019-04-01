# Glossary: PatchCommandData

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **id** | string | Document ID |
| **changeVector** | string | Document Change Vector |
| **patch** | [PatchRequest](../client-api/operations/patching/single-document) | Patch to apply |
| **patcheIfMissing** | [PatchRequest](../client-api/operations/patching/single-document) | Patch to apply if document is missing |

### Methods

| Signature | Description |
| ---------- | ----------- |
| **DynamicJsonValue ToJson(DocumentConventions conventions, JsonOperationContext context)** | Translate this instance to Json. |
