# Commands : Documents : Get

There are a few methods that allow you to retrieve documents from a database:   

- [Get single document](../../../client-api/commands/documents/get#get-single-document)   
- [Get multiple documents](../../../client-api/commands/documents/get#get-multiple-documents)   
- [Get paged documents](../../../client-api/commands/documents/get#get-paged-documents)   
- [Get documents by starts with](../../../client-api/commands/documents/get#get-by-starts-with)  

{PANEL:Get single document}

**GetDocumentsCommand** can be used to retrieve a single document

### Syntax

{CODE:nodejs get_interface_single@client-api\commands\documents\get\get.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **id** | string | ID of the document to get |
| **includes** | string | Related documents to fetch along with the document |
| **metadataOnly** | boolean | Whether to fetch the whole document or just the metadata. |
| **conventions** | DocumentConventions | Document conventions |

### Example

{CODE:nodejs get_sample_single@client-api\commands\documents\get\get.js /}

{PANEL/}

{PANEL:Get multiple documents}

**GetDocumentsCommand** can also be used to retrieve a list of documents.

### Syntax

{CODE:nodejs get_interface_multiple@client-api\commands\documents\get\get.js /}

| Options | | |
| ------------- | ------------- | ----- |
| **ids** | string[] | IDs of the documents to get |
| **includes** | string | Related documents to fetch along with the documents |
| **metadataOnly** | boolean | Whether to fetch whole documents or just the metadata |
| **conventions** | DocumentConventions | Document conventions |

### Example I

{CODE:nodejs get_sample_multiple@client-api\commands\documents\get\get.js /}

### Example II - Using Includes

{CODE:nodejs get_sample_includes@client-api\commands\documents\get\get.js /}

### Example III - Missing Documents

{CODE:nodejs get_sample_missing@client-api\commands\documents\get\get.js /}

{PANEL/}

{PANEL:Get paged documents}

**GetDocumentsCommand** can also be used to retrieve a paged set of documents.

### Syntax

{CODE:nodejs get_interface_paged@client-api\commands\documents\get\get.js /}

| Options | | |
| ------------- | ------------- | ----- |
| **start** | number | number of documents that should be skipped  |
| **pageSize** | number | maximum number of documents that will be retrieved |
| **conventions** | DocumentConventions | Document conventions |

### Example

{CODE:nodejs get_sample_paged@client-api\commands\documents\get\get.js /}

{PANEL/}

{PANEL:Get by starts with}

**GetDocumentsCommand** can be used to retrieve multiple documents for a specified ID prefix.

### Syntax

{CODE:nodejs get_interface_startswith@client-api\commands\documents\get\get.js /}

| Options | | |
| ------------- | ------------- | ----- |
| **startsWith** | string | prefix for which documents should be returned |
| **startAfter** | string | skip 'document fetching' until the given ID is found, and return documents after that ID (default: null) |
| **matches** | string | pipe ('&#124;') separated values for which document IDs (after 'startsWith') should be matched ('?' any single character, '*' any characters) |
| **exclude** | string | pipe ('&#124;') separated values for which document IDs (after 'startsWith') should **not** be matched ('?' any single character, '*' any characters) |
| **start** | number | number of documents that should be skipped |
| **pageSize** | number | maximum number of documents that will be retrieved |
| **metadataOnly** | boolean | specifies whether or not only document metadata should be returned |
| **conventions** | DocumentConventions | Document conventions |

### Example I

{CODE:nodejs get_sample_startswith@client-api\commands\documents\get\get.js /}

### Example II

{CODE:nodejs get_sample_startswith_matches@client-api\commands\documents\get\get.js /}

### Example III

{CODE:nodejs get_sample_startswith_matches_end@client-api\commands\documents\get\get.js /}

{PANEL/}

## Related Articles

### Commands 

- [Put](../../../client-api/commands/documents/put)  
- [Delete](../../../client-api/commands/documents/delete)
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
