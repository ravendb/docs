# Glossary: ScriptedPatchCommandData

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Patch** | [ScriptedPatchRequest](../client-api/commands/patches/how-to-use-javascript-to-patch-your-documents) | The ScriptedPatchRequest (using JavaScript) that is used to patch the document |
| **PatchIfMissing** | [ScriptedPatchRequest](../client-api/commands/patches/how-to-use-javascript-to-patch-your-documents) | The ScriptedPatchRequest (using JavaScript) that is used to patch a default document if the document is missing |
| **Key** | string | Document key |
| **Method** | string | The HTTP method (`EVAL`) |
| **Etag** | Etag | Document etag (used for concurrency checking) |
| **TransactionInformation** | TransactionInformation | Transactional information |
| **Metadata** | RavenJObject | Document metadata |
| **AdditionalData** | RavenJObject | Additional metadata |
| **DebugMode** | bool | Debug mode |

### Methods

| Signature | Description |
| ---------- | ----------- |
| **RavenJObject ToJson()** | Translate this instance to a Json object. |
