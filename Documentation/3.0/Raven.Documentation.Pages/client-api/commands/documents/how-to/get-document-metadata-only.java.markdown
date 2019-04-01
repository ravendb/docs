# Commands: Documents: How to get document metadata only?

**Head** is used to retrieve document metadata from a database.

## Syntax

{CODE:java head_1@ClientApi\Commands\Documents\HowTo\Head.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | String | key of a document to get metadata for |

| Return Value | |
| ------------- | ----- |
| [JsonDocumentMetadata](../../../../glossary/json-document-metadata) | Metadata information for document with given key. |

## Example

{CODE:java head_2@ClientApi\Commands\Documents\HowTo\Head.java /}

## Related articles

- [Get](../../../../client-api/commands/documents/get)  
- [Put](../../../../client-api/commands/documents/put)  
- [Delete](../../../../client-api/commands/documents/delete)  
- [Revisions and concurrency with ETags](../../../../client-api/concurrency/revisions-and-concurrency-with-etags)   
