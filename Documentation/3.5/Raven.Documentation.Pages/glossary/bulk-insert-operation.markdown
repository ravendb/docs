# Glossary: BulkInsertOperation

### Related Delegates

| Signature |
| ----------|
| **delegate void BeforeEntityInsert(string id, RavenJObject data, RavenJObject metadata)** |

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **IsAborted** | bool | indicates if an operation has aborted. |
| **OperationId** | Guid | Unique operation Id. |

### Methods

| Signature | Description |
| ----------| ----- |
| **void Abort()** | Abort the operation |
| **void Store(object entity)** | store the entity, identifier will be generated automatically on the client-side |
| **void Store(object entity, string id)** | store the entity, with `id` parameter to explicitly declare the entity identifier |
| **void Dispose()** | Dispose an object |

### Events

| Signature | Description |
| ----------| ----- |
| **BeforeEntityInsert OnBeforeEntityInsert** | will be raised before entity will be processed |
| **Action&lt;string&gt; Report** |  will be raised every time a batch has finished processing and after the whole operation |
