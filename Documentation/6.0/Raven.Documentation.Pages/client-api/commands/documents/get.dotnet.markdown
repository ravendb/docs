# Get Documents Command
---

{NOTE: }

* Use the low-level `GetDocumentsCommand` to retrieve documents from the database.

* To retrieve documents using a higher-level method, see [loading entities](../../../client-api/session/loading-entities) or [query for documents](../../../client-api/session/querying/how-to-query).  

* In this page:
   - [Get single document](../../../client-api/commands/documents/get#get-single-document)   
   - [Get multiple documents](../../../client-api/commands/documents/get#get-multiple-documents)   
   - [Get metadata only](../../../client-api/commands/documents/get#get-metadata-only)    
   - [Get paged documents](../../../client-api/commands/documents/get#get-paged-documents)   
   - [Get documents - by ID prefix](../../../client-api/commands/documents/get#get-documents---by-id-prefix)  
   - [Get documents - with includes](../../../client-api/commands/documents/get#get-documents---with-includes)  
   - [Syntax](../../../client-api/commands/documents/get#syntax)  

{NOTE/}

---

{PANEL: Get single document}

* The following examples demonstrate how to retrieve a document using either the _Store's request executor_  
  or the _Session's request executor_.
* The examples in the rest of the article use the _Store's request executor_, but you can apply the Session's implementation shown here to ALL cases.

---

{CONTENT-FRAME: }

**Get document command - using the Store's request executor**:

---

{CODE-TABS}
{CODE-TAB:csharp:Get_document get_1_storeContext@ClientApi\Commands\Documents\Get.cs /}
{CODE-TAB:csharp:Get_document_async get_1_storeContext_async@ClientApi\Commands\Documents\Get.cs /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Get document command - using the Session's request executor**:

---

{CODE-TABS}
{CODE-TAB:csharp:Get_document get_1_sessionContext@ClientApi\Commands\Documents\Get.cs /}
{CODE-TAB:csharp:Get_document_async get_1_sessionContext_async@ClientApi\Commands\Documents\Get.cs /}
{CODE-TABS/}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Get multiple documents}

{CONTENT-FRAME: }

**Get multiple documents**:

---

{CODE get_2_storeContext@ClientApi\Commands\Documents\Get.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Get multiple documents - missing documents**:

---

{CODE get_3_storeContext@ClientApi\Commands\Documents\Get.cs /}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Get metadata only}

{CODE get_4_storeContext@ClientApi\Commands\Documents\Get.cs /}

{PANEL/}

{PANEL: Get paged documents}

* You can retrieve documents in pages by specifying how many documents to skip and how many to fetch.
* Using this overload, no specific collection is specified, the documents will be fetched from ALL collections.

{CODE get_5_storeContext@ClientApi\Commands\Documents\Get.cs /}

{PANEL/}

{PANEL: Get documents - by ID prefix}

{CONTENT-FRAME: }

**Retrieve documents that match a specified ID prefix**:

---

{CODE get_6_storeContext@ClientApi\Commands\Documents\Get.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Retrieve documents that match a specified ID prefix - with "matches" pattern**:

---

{CODE get_7_storeContext@ClientApi\Commands\Documents\Get.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Retrieve documents that match a specified ID prefix - with "exclude" pattern**:

---

{CODE get_8_storeContext@ClientApi\Commands\Documents\Get.cs /}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Get documents - with includes}

{CONTENT-FRAME: }

**Include related documents**:

---

{CODE get_9_storeContext@ClientApi\Commands\Documents\Get.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Include counters**:

---

{CODE get_10_storeContext@ClientApi\Commands\Documents\Get.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Include time series**:

---

{CODE get_11_storeContext@ClientApi\Commands\Documents\Get.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Include revisions**:

---

{CODE get_12_storeContext@ClientApi\Commands\Documents\Get.cs /}
{CODE get_13_storeContext@ClientApi\Commands\Documents\Get.cs /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Include compare-exchange values**:

---

{CODE get_14_storeContext@ClientApi\Commands\Documents\Get.cs /}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Commands\Documents\Get.cs /}

| Parameter                           | Type                        | Description                                                                                                                                            |
|-------------------------------------|-----------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------|
| **conventions**                     | `DocumentConventions`       | The store's conventions.                                                                                                                               |
| **id**                              | `string`                    | ID of the document to get.                                                                                                                             |
| **ids**                             | `string[]`                  | IDs of the documents to get.                                                                                                                           |
| **includes**                        | `string[]`                  | Related documents to fetch along with the document.                                                                                                    |
| **counterIncludes**                 | `string[]`                  | Counters to fetch along with the document.                                                                                                             |
| **includeAllCounters**              | `bool`                      | Whether to include all counters.                                                                                                                       |
| **timeSeriesIncludes**              | `AbstractTimeSeriesRange[]` | Time series to fetch along with the document.                                                                                                          |
| **compareExchangeValueIncludes**    | `string[]`                  | List of document fields containing cmpXchg keys of the compare-exchange items you wish to include.                                                     |
| **revisionsIncludesByChangeVector** | `string[]`                  | List of document fields containing change-vectors of the revisions you wish to include.                                                                |
| **revisionIncludeByDateTimeBefore** | `DateTime`                  | When this date is provided, retrieve the most recent revision that was created before this date value.                                                 |
| **metadataOnly**                    | `bool`                      | Whether to fetch the whole document or just the metadata.                                                                                              |
| **start**                           | `int`                       | Number of documents that should be skipped.                                                                                                            |
| **pageSize**                        | `int`                       | Maximum number of documents that will be retrieved.                                                                                                    |
| **startsWith**                      | `string`                    | Fetch only documents with this prefix.                                                                                                                 |
| **startAfter**                      | `string`                    | Skip 'document fetching' until the given ID is found, and return documents after that ID (default: null).                                              |
| **matches**                         | `string`                    | Pipe ('&#124;') separated values for which document IDs (after `startsWith`) should be matched.<br>(`?` any single character, `*` any characters).     |
| **exclude**                         | `string`                    | Pipe ('&#124;') separated values for which document IDs (after `startsWith`) should Not be matched.<br>(`?` any single character, `*` any characters). |

{CODE syntax_2@ClientApi\Commands\Documents\Get.cs /}

{PANEL/}

## Related Articles

### Commands 

- [Put document](../../../client-api/commands/documents/put)  
- [Delete document](../../../client-api/commands/documents/delete)
- [Send multiple commands using a batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)

