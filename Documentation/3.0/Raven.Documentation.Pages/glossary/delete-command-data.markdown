# Glossary: DeleteCommandData

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Key** | string | Document key |
| **Method** | string | The HTTP method (`DELETE`) |
| **Etag** | Etag | Document etag (used for concurrency checking) |
| **TransactionInformation** | TransactionInformation | Transactional information |
| **Metadata** | RavenJObject | Document metadata |
| **AdditionalData** | RavenJObject | Additional metadata |

### Methods

| Signature | Description |
| ---------- | ----------- |
| **RavenJObject ToJson()** | Translate this instance to a Json object. |

