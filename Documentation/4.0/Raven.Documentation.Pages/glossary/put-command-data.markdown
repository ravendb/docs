# Glossary : PutCommandData

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Id** | string | Document id |
| **ChangeVector** | string | Document change-vector |
| **Document** | DynamicJsonValue | Document to put |
| **Type** | CommandType | The Commad Type (PUT) |

### Methods

| Signature | Description |
| ---------- | ----------- |
| **DynamicJsonValue ToJson(DocumentConventions conventions, JsonOperationContext context)** | Translate this instance to Json. |
