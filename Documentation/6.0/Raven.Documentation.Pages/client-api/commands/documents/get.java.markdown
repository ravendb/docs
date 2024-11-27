# Get Documents Command

There are a few methods that allow you to retrieve documents from a database:   

- [Get single document](../../../client-api/commands/documents/get#get-single-document)   
- [Get multiple documents](../../../client-api/commands/documents/get#get-multiple-documents)   
- [Get paged documents](../../../client-api/commands/documents/get#get-paged-documents)   
- [Get documents by starts with](../../../client-api/commands/documents/get#get-by-starts-with)  
- [Get metadata only](../../../client-api/commands/documents/get#get-metadata-only)  

{PANEL: Get single document}

**GetDocumentsCommand** can be used to retrieve a single document

### Syntax

{CODE:java get_interface_single@ClientApi\Commands\Documents\Get.java /}

| Parameter        | Type       | Description                                               |
|------------------|------------|-----------------------------------------------------------|
| **id**           | `String`   | ID of the documents to get.                               |
| **includes**     | `String[]` | Related documents to fetch along with the document.       |
| **metadataOnly** | `boolean`  | Whether to fetch the whole document or just the metadata. |

### Example

{CODE:java get_sample_single@ClientApi\Commands\Documents\Get.java /}

{PANEL/}

{PANEL: Get multiple documents}

**GetDocumentsCommand** can also be used to retrieve a list of documents.

### Syntax

{CODE:java get_interface_multiple@ClientApi\Commands\Documents\Get.java /}

| Parameter        | Type       | Description                                            |
|------------------|------------|--------------------------------------------------------|
| **ids**          | `String[]` | IDs of the documents to get.                           |
| **includes**     | `String`   | Related documents to fetch along with the documents.   |
| **metadataOnly** | `boolean`  | Whether to fetch whole documents or just the metadata. |

### Example I

{CODE:java get_sample_multiple@ClientApi\Commands\Documents\Get.java /}

### Example II - Using Includes

{CODE:java get_sample_includes@ClientApi\Commands\Documents\Get.java /}

### Example III - Missing Documents

{CODE:java get_sample_missing@ClientApi\Commands\Documents\Get.java /}

{PANEL/}

{PANEL: Get paged documents}

**GetDocumentsCommand** can also be used to retrieve a paged set of documents.

### Syntax

{CODE:java get_interface_paged@ClientApi\Commands\Documents\Get.java /}

| Parameter    | Type  | Description                                          |
|--------------|-------|------------------------------------------------------|
| **start**    | `int` | Number of documents that should be skipped.          |
| **pageSize** | `int` | Maximum number of documents that will be retrieved.  |

### Example

{CODE:java get_sample_paged@ClientApi\Commands\Documents\Get.java /}

{PANEL/}

{PANEL: Get by starts with}

**GetDocumentsCommand** can be used to retrieve multiple documents for a specified ID prefix.

### Syntax

{CODE:java get_interface_startswith@ClientApi\Commands\Documents\Get.java /}

| Parameter        | Type      | Description                                                                                                                                            |
|------------------|-----------|--------------------------------------------------------------------------------------------------------------------------------------------------------|
| **startsWith**   | `String`  | Prefix for which documents should be returned.                                                                                                         |
| **startAfter**   | `String`  | Skip 'document fetching' until the given ID is found, and return documents after that ID (default: null).                                              |
| **matches**      | `String`  | Pipe ('&#124;') separated values for which document IDs (after 'startsWith') should be matched ('?' any single character, '*' any characters).         |
| **exclude**      | `String`  | Pipe ('&#124;') separated values for which document IDs (after 'startsWith') should **not** be matched ('?' any single character, '*' any characters). |
| **start**        | `int`     | Number of documents that should be skipped.                                                                                                            |
| **pageSize**     | `int`     | Maximum number of documents that will be retrieved.                                                                                                    |
| **metadataOnly** | `boolean` | Specifies whether or not only document metadata should be returned.                                                                                    |

### Example I

{CODE:java get_sample_startswith@ClientApi\Commands\Documents\Get.java /}

### Example II

{CODE:java get_sample_startswith_matches@ClientApi\Commands\Documents\Get.java /}

### Example III

{CODE:java get_sample_startswith_matches_end@ClientApi\Commands\Documents\Get.java /}

{PANEL/}

{PANEL: Get metadata only}

**GetDocumentsCommand** can be used to retrieve the metadata of documents.

### Example

{CODE:java get_metadata_only@ClientApi\Commands\Documents\Get.java /}

{PANEL/}

## Related Articles

### Commands 

- [Put](../../../client-api/commands/documents/put)  
- [Delete](../../../client-api/commands/documents/delete)
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
