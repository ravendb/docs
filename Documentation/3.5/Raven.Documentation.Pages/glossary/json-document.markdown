# Glossary: JsonDocument

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **DataAsJson** | RavenJObject | Document data |
| **Metadata** | RavenJObject | Document metadata |
| **Etag** | Etag | Document ETag |
| **Key** | string | Document key |
| **LastModified** | DateTime? | Date of last modification |
| **NonAuthoritativeInformation** | bool? | indicates if document is non authoritative (modified by uncommited transaction) |
| **TempIndexScore** | float? | ranking of document in query |

### Methods

| Signature | Description |
| --------- | ----------- |
| **RavenJObject ToJson()** | Translate the json document to a RavenJObject |

### Articles

- [Client API : Commands : Documents : Get](../client-api/commands/documents/get#get)
- [Client API : Commands : Documents : GetDocuments](../client-api/commands/documents/get#getdocuments)
- [Client API : Commands : Documents : StartsWith](../client-api/commands/documents/get#startswith)

