# Glossary: PutCommandData

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Id** | string | Document ID |
| **ChangeVector** | string | Document change-vector |
| **Document** | DynamicJsonValue | Document to put |
| **Type** | CommandType | The Command Type (PUT) |

### Methods

| Signature | Description |
| ---------- | ----------- |
| **DynamicJsonValue ToJson(DocumentConventions conventions, JsonOperationContext context)** | Translate this instance to Json. |
