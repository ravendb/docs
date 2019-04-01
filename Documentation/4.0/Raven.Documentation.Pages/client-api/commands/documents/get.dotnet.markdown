# Commands: Documents: Get

There are a few methods that allow you to retrieve documents from a database:   

- [Get single document](../../../client-api/commands/documents/get#get-single-document)   
- [Get multiple documents](../../../client-api/commands/documents/get#get-multiple-documents)   
- [Get paged documents](../../../client-api/commands/documents/get#get-paged-documents)   
- [Get documents by starts with](../../../client-api/commands/documents/get#get-by-starts-with)  

{PANEL:Get single document}

**GetDocumentsCommand** can be used to retrieve a single document

### Syntax

{CODE get_interface_single@ClientApi\Commands\Documents\Get.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** string | ID of the documents to get |
| **includes** | string[] | Related documents to fetch along with the document |
| **metadataOnly** | boolean | Whether to fetch the whole document or just the metadata. |

### Example

{CODE get_sample_single@ClientApi\Commands\Documents\Get.cs /}

{PANEL/}

{PANEL:Get multiple documents}

**GetDocumentsCommand** can also be used to retrieve a list of documents.

### Syntax

{CODE get_interface_multiple@ClientApi\Commands\Documents\Get.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **ids** string[] | IDs of the documents to get |
| **includes** | string | Related documents to fetch along with the documents |
| **metadataOnly** | boolean | Whether to fetch whole documents or just the metadata |

### Example I

{CODE get_sample_multiple@ClientApi\Commands\Documents\Get.cs /}

### Example II - Using Includes

{CODE get_sample_includes@ClientApi\Commands\Documents\Get.cs /}

### Example III - Missing Documents

{CODE get_sample_missing@ClientApi\Commands\Documents\Get.cs /}

{PANEL/}

{PANEL:Get paged documents}

**GetDocumentsCommand** can also be used to retrieve a paged set of documents.

### Syntax

{CODE get_interface_paged@ClientApi\Commands\Documents\Get.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | number of documents that should be skipped  |
| **pageSize** | int | maximum number of documents that will be retrieved |

### Example

{CODE get_sample_paged@ClientApi\Commands\Documents\Get.cs /}

{PANEL/}

{PANEL:Get by starts with}

**GetDocumentsCommand** can be used to retrieve multiple documents for a specified ID prefix.

### Syntax

{CODE get_interface_startswith@ClientApi\Commands\Documents\Get.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **startsWith** | string | prefix for which documents should be returned |
| **startAfter** | string | skip 'document fetching' until the given ID is found, and return documents after that ID (default: null) |
| **matches** | string | pipe ('&#124;') separated values for which document IDs (after 'startsWith') should be matched ('?' any single character, '*' any characters) |
| **exclude** | string | pipe ('&#124;') separated values for which document IDs (after 'startsWith') should **not** be matched ('?' any single character, '*' any characters) |
| **start** | int | number of documents that should be skipped |
| **pageSize** | int | maximum number of documents that will be retrieved |
| **metadataOnly** | bool | specifies whether or not only document metadata should be returned |

### Example I

{CODE get_sample_startswith@ClientApi\Commands\Documents\Get.cs /}

### Example II

{CODE get_sample_startswith_matches@ClientApi\Commands\Documents\Get.cs /}

### Example III

{CODE get_sample_startswith_matches_end@ClientApi\Commands\Documents\Get.cs /}

{PANEL/}

## Related Articles

### Commands 

- [Put](../../../client-api/commands/documents/put)  
- [Delete](../../../client-api/commands/documents/delete)
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
