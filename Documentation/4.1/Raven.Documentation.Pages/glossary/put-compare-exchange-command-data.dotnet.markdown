# Glossary: PutCompareExchangeCommandData

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **key** | string | Object identifier under which _value_ is saved, unique in the database scope across the cluster. |
| **value** | BlittableJsonReaderObject | The value to be saved for the specified _key_. |
| **index** | long | The current version of _Value_ when updating a value for an existing key |

### Methods

| Signature | Description |
| ---------- | ----------- |
| **DynamicJsonValue ToJson(DocumentConventions conventions, JsonOperationContext context)** | Translate this instance to a Json. |
