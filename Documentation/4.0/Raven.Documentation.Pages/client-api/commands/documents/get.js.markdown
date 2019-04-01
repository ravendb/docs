# Commands: Documents: Get

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
| **options** | object | |
| &nbsp;&nbsp;*id* | string | ID of the document to get |
| &nbsp;&nbsp;*includes* | string[] | Related documents to fetch along with the document |
| &nbsp;&nbsp;*metadataOnly* | boolean | Whether to fetch the whole document or just the metadata. |
| &nbsp;&nbsp;*conventions* | DocumentConventions | Document conventions |

### Example

{CODE:nodejs get_sample_single@client-api\commands\documents\get\get.js /}

{PANEL/}

{PANEL:Get multiple documents}

**GetDocumentsCommand** can also be used to retrieve a list of documents.

### Syntax

{CODE:nodejs get_interface_multiple@client-api\commands\documents\get\get.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | object | |
| &nbsp;&nbsp;*ids* | string[] | IDs of the documents to get |
| &nbsp;&nbsp;*includes* | string[] | Related documents to fetch along with the documents |
| &nbsp;&nbsp;*metadataOnly* | boolean | Whether to fetch whole documents or just the metadata |
| &nbsp;&nbsp;*conventions* | DocumentConventions | Document conventions |

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

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | object | |
| &nbsp;&nbsp;*start* | number | number of documents that should be skipped  |
| &nbsp;&nbsp;*pageSize* | number | maximum number of documents that will be retrieved |
| &nbsp;&nbsp;*conventions* | DocumentConventions | Document conventions |

### Example

{CODE:nodejs get_sample_paged@client-api\commands\documents\get\get.js /}

{PANEL/}

{PANEL:Get by starts with}

**GetDocumentsCommand** can be used to retrieve multiple documents for a specified ID prefix.

### Syntax

{CODE:nodejs get_interface_startswith@client-api\commands\documents\get\get.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **options** | object | |
| &nbsp;&nbsp;*startsWith* | string | prefix for which documents should be returned |
| &nbsp;&nbsp;*startAfter* | string | skip 'document fetching' until the given ID is found, and return documents after that ID (default: null) |
| &nbsp;&nbsp;*matches* | string | pipe ('&#124;') separated values for which document IDs (after 'startsWith') should be matched ('?' any single character, '*' any characters) |
| &nbsp;&nbsp;*exclude* | string | pipe ('&#124;') separated values for which document IDs (after 'startsWith') should **not** be matched ('?' any single character, '*' any characters) |
| &nbsp;&nbsp;*start* | number | number of documents that should be skipped |
| &nbsp;&nbsp;*pageSize* | number | maximum number of documents that will be retrieved |
| &nbsp;&nbsp;*metadataOnly* | boolean | specifies whether or not only document metadata should be returned |
| &nbsp;&nbsp;*conventions* | DocumentConventions | Document conventions |

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
