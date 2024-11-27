# Get Documents Command
---

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

{CODE:python get_interface_single@ClientApi\Commands\Documents\Get.py /}

| Parameter         | Type        | Description                                               |
|-------------------|-------------|-----------------------------------------------------------|
| **key**           | `str`       | ID of the documents to get.                               |
| **includes**      | `List[str]` | Related documents to fetch along with the document.       |
| **metadata_only** | `bool`      | Whether to fetch the whole document or just the metadata. |

---

#### Example:

{CODE:python get_sample_single@ClientApi\Commands\Documents\Get.py /}

{PANEL/}

{PANEL: Get multiple documents}

**GetDocumentsCommand** can also be used to retrieve a list of documents.

#### Syntax:

{CODE:python get_interface_multiple@ClientApi\Commands\Documents\Get.py /}

| Parameter         | Type        | Description                                            |
|-------------------|-------------|--------------------------------------------------------|
| **keys**          | `List[str]` | IDs of the documents to get.                           |
| **includes**      | `List[str]` | Related documents to fetch along with the documents.   |
| **metadata_only** | `bool`      | Whether to fetch whole documents or just the metadata. |

---

#### Example I

{CODE:python get_sample_multiple@ClientApi\Commands\Documents\Get.py /}

#### Example II - Using Includes

{CODE:python get_sample_includes@ClientApi\Commands\Documents\Get.py /}

#### Example III - Missing Documents

{CODE:python get_sample_missing@ClientApi\Commands\Documents\Get.py /}

{PANEL/}

{PANEL: Get paged documents}

**GetDocumentsCommand** can also be used to retrieve a paged set of documents.

#### Syntax:

{CODE:python get_interface_paged@ClientApi\Commands\Documents\Get.py /}

| Parameter     | Type  | Description                                         |
|---------------|-------|-----------------------------------------------------|
| **start**     | `int` | Number of documents that should be skipped.         |
| **page_size** | `int` | Maximum number of documents that will be retrieved. |

---

#### Example:

{CODE:python get_sample_paged@ClientApi\Commands\Documents\Get.py /}

{PANEL/}

{PANEL: Get documents by ID prefix}

**GetDocumentsCommand** can be used to retrieve multiple documents for a specified ID prefix.

#### Syntax:

{CODE:python get_interface_startswith@ClientApi\Commands\Documents\Get.py /}

| Parameter         | Type   | Description                                                                                                                                            |
|-------------------|--------|--------------------------------------------------------------------------------------------------------------------------------------------------------|
| **start_with**    | `str`  | Prefix for which documents should be returned.                                                                                                         |
| **start_after**   | `str`  | Skip 'document fetching' until the given ID is found, and return documents after that ID (default: None).                                              |
| **matches**       | `str`  | Pipe ('&#124;') separated values for which document IDs (after `start_with`) should be matched ('?' any single character, '*' any characters).         |
| **exclude**       | `str`  | Pipe ('&#124;') separated values for which document IDs (after `start_with`) should **not** be matched ('?' any single character, '*' any characters). |
| **start**         | `int`  | Number of documents that should be skipped.                                                                                                            |
| **page_size**     | `int`  | Maximum number of documents that will be retrieved.                                                                                                    |
| **metadata_only** | `bool` | Specifies whether or not only document metadata should be returned.                                                                                    |

---

#### Example I

{CODE:python get_sample_startswith@ClientApi\Commands\Documents\Get.py /}

#### Example II

{CODE:python get_sample_startswith_matches@ClientApi\Commands\Documents\Get.py /}

#### Example III

{CODE:python get_sample_startswith_matches_end@ClientApi\Commands\Documents\Get.py /}

{PANEL/}

## Related Articles

### Commands 

- [Put](../../../client-api/commands/documents/put)  
- [Delete](../../../client-api/commands/documents/delete)
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
