# Commands: Documents: How to Get Document Metadata Only

**GetDocumentsCommand** can be used to retrieve the metadata of documents.

## Syntax

{CODE head_1@ClientApi\Commands\Documents\HowTo\Head.cs /}
{CODE head_3@ClientApi\Commands\Documents\HowTo\Head.cs /}


| Parameters | | |
| ------------- | ------------- | ----- |
| **id** / **ids** | string / string[] | IDs of documents to get metadata for |
| **includes** | string | Related documents to fetch along with the document |
| **metadataOnly** | boolean | Whether to fetch the whole document or just the metadata. |

## Example

{CODE head_2@ClientApi\Commands\Documents\HowTo\Head.cs /}

## Related Articles

### Commands 

- [Get](../../../../client-api/commands/documents/get)  
- [Put](../../../../client-api/commands/documents/put)  
- [Delete](../../../../client-api/commands/documents/delete)
- [How to Send Multiple Commands Using a Batch](../../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
