# Commands: Get Documents

{NOTE: }

* Use `GetDocumentsCommand` to retrieve documents from the database.

* In this page:
   - [Get single document](../../../client-api/commands/documents/get#get-single-document)   
   - [Get multiple documents](../../../client-api/commands/documents/get#get-multiple-documents)   
   - [Get paged documents](../../../client-api/commands/documents/get#get-paged-documents)   
   - [Get documents by ID prefix](../../../client-api/commands/documents/get#get-documents-by-id-prefix)  

{NOTE/}

---

{PANEL: Get single document}

**GetDocumentsCommand** can be used to retrieve a single document

#### Syntax:

{CODE:php get_interface_single@ClientApi\Commands\Documents\Get.php /}

| Parameters | Type | Description |
|------------|------|-------------|
| **id** | `string` | ID of the documents to get |
| **includes** | `StringArray` or `array` or `null` | Related documents to fetch along with the document |
| **metadataOnly** | `bool` | Whether to fetch the whole document or just its metadata. |

---

#### Example:

{CODE:php get_sample_single@ClientApi\Commands\Documents\Get.php /}

{PANEL/}

{PANEL: Get multiple documents}

**GetDocumentsCommand** can also be used to retrieve a list of documents.

#### Syntax:

{CODE:php get_interface_multiple@ClientApi\Commands\Documents\Get.php /}

| Parameters | Type | Description |
|------------|------|-------------|
| **ids** | `StringArray` or `array` or `null` | IDs of the documents to get |
| **includes** | `StringArray` or `array` or `null` | Related documents to fetch along with the documents |
| **metadataOnly** | `bool` | Whether to fetch whole documents or just the metadata |

---

#### Example I

{CODE:php get_sample_multiple@ClientApi\Commands\Documents\Get.php /}

#### Example II - Using Includes

{CODE:php get_sample_includes@ClientApi\Commands\Documents\Get.php /}

#### Example III - Missing Documents

{CODE:php get_sample_missing@ClientApi\Commands\Documents\Get.php /}

{PANEL/}

{PANEL: Get paged documents}

**GetDocumentsCommand** can also be used to retrieve a paged set of documents.

#### Syntax:

{CODE:php get_interface_paged@ClientApi\Commands\Documents\Get.php /}

| Parameters | Type | Description |
|------------|------|-------------|
| **start** | `int` | number of documents that should be skipped  |
| **pageSize** | `int` | maximum number of documents that will be retrieved |

---

#### Example:

{CODE:php get_sample_paged@ClientApi\Commands\Documents\Get.php /}

{PANEL/}

{PANEL: Get documents by ID prefix}

**GetDocumentsCommand** can be used to retrieve multiple documents for a specified ID prefix.

#### Syntax:

{CODE:php get_interface_startswith@ClientApi\Commands\Documents\Get.php /}

| Parameters | Type | Description |
|------------|------|-------------|
| **startWith** | `?string` | prefix for which documents should be returned |
| **startAfter** | `?string` | skip 'document fetching' until the given ID is found, and return documents after that ID (default: None) |
| **matches** | `?string` | pipe ('&#124;') separated values for which document IDs (after `startWith`) should be matched ('?' any single character, '*' any characters) |
| **exclude** | `?string` | pipe ('&#124;') separated values for which document IDs (after `startWith`) should **not** be matched ('?' any single character, '*' any characters) |
| **start** | `int` | number of documents that should be skipped |
| **pageSize** | `int` | maximum number of documents that will be retrieved |
| **metadataOnly** | `bool` | specifies whether or not only document metadata should be returned |

---

#### Example I

{CODE:php get_sample_startswith@ClientApi\Commands\Documents\Get.php /}

#### Example II

{CODE:php get_sample_startswith_matches@ClientApi\Commands\Documents\Get.php /}

#### Example III

{CODE:php get_sample_startswith_matches_end@ClientApi\Commands\Documents\Get.php /}

{PANEL/}

## Related Articles

### Commands 

- [Put](../../../client-api/commands/documents/put)  
- [Delete](../../../client-api/commands/documents/delete)
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
