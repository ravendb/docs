# Commands : Documents : How to get document metadata only?

**GetDocumentCommand** can be used to retrieve the metadata of a document.

## Syntax

{CODE head_1@ClientApi\Commands\Documents\HowTo\Head.cs /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | ID of a document to get metadata for |
| **includes** | string | Related documents to fetch along with the document |
| **metadataOnly** | boolean | Whether to fetch the whole document or just the metadata. |

## Example

{CODE head_2@ClientApi\Commands\Documents\HowTo\Head.cs /}

## Related articles

- [Get](../../../../client-api/commands/documents/get)  
- [Put](../../../../client-api/commands/documents/put)  
- [Delete](../../../../client-api/commands/documents/delete)
