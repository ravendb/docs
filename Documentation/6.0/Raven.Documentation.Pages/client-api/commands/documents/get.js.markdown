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

{CODE:nodejs get_1_storeContext@client-api\commands\documents\get.js /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Get document command - using the Session's request executor**:

---

{CODE:nodejs get_1_sessionContext@client-api\commands\documents\get.js /}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Get multiple documents}

{CONTENT-FRAME: }

**Get multiple documents**:

---

{CODE:nodejs get_2@client-api\commands\documents\get.js /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Get multiple documents - missing documents**:

---

{CODE:nodejs get_3@client-api\commands\documents\get.js /}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Get metadata only}

{CODE:nodejs get_4@client-api\commands\documents\get.js /}

{PANEL/}

{PANEL: Get paged documents}

* You can retrieve documents in pages by specifying how many documents to skip and how many to fetch.
* Using this overload, no specific collection is specified, the documents will be fetched from ALL collections.

{CODE:nodejs get_5@client-api\commands\documents\get.js /}

{PANEL/}

{PANEL: Get documents - by ID prefix}

{CONTENT-FRAME: }

**Retrieve documents that match a specified ID prefix**:

---

{CODE:nodejs get_6@client-api\commands\documents\get.js /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Retrieve documents that match a specified ID prefix - with "matches" pattern**:

---

{CODE:nodejs get_7@client-api\commands\documents\get.js /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Retrieve documents that match a specified ID prefix - with "exclude" pattern**:

---

{CODE:nodejs get_8@client-api\commands\documents\get.js /}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Get documents - with includes}

{CONTENT-FRAME: }

**Include related documents**:

---

{CODE:nodejs get_9@client-api\commands\documents\get.js /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Include counters**:

---

{CODE:nodejs get_10@client-api\commands\documents\get.js /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Include time series**:

---

{CODE:nodejs get_11@client-api\commands\documents\get.js /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Include revisions**:

---

{CODE:nodejs get_12@client-api\commands\documents\get.js /}
{CODE:nodejs get_13@client-api\commands\documents\get.js /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

**Include compare-exchange values**:

---

{CODE:nodejs get_14@client-api\commands\documents\get.js /}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@client-api\commands\documents\get.js /}

| Parameter                           | Type                        | Description                                                                                                                                            |
|-------------------------------------|-----------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------|
| **conventions**                     | `DocumentConventions`       | The store's conventions.                                                                                                                               |
| **id**                              | `string`                    | ID of the document to get.                                                                                                                             |
| **ids**                             | `string[]`                  | IDs of the documents to get.                                                                                                                           |
| **includes**                        | `string[]`                  | Related documents to fetch along with the document.                                                                                                    |
| **counterIncludes**                 | `string[]`                  | Counters to fetch along with the document.                                                                                                             |
| **includeAllCounters**              | `boolean`                   | Whether to include all counters.                                                                                                                       |
| **timeSeriesIncludes**              | `AbstractTimeSeriesRange[]` | Time series to fetch along with the document.                                                                                                          |
| **compareExchangeValueIncludes**    | `string[]`                  | List of document fields containing cmpXchg keys of the compare-exchange items you wish to include.                                                                                                                                                   |
| **revisionsIncludesByChangeVector** | `string[]`                  | List of document fields containing the change-vectors of the revisions you wish to include.                                                            |
| **revisionIncludeByDateTimeBefore** | `Date`                      | When this date is provided, retrieve the most recent revision that was created before this date value.                                                 |
| **metadataOnly**                    | `boolean`                   | Whether to fetch the whole document or just the metadata.                                                                                              |
| **start**                           | `number`                    | Number of documents that should be skipped.                                                                                                            |
| **pageSize**                        | `number`                    | Maximum number of documents that will be retrieved.                                                                                                    |
| **startsWith**                      | `string`                    | Fetch only documents with this prefix.                                                                                                                 |
| **startAfter**                      | `string`                    | Skip 'document fetching' until the given ID is found, and return documents after that ID (default: null).                                              |
| **matches**                         | `string`                    | Pipe ('&#124;') separated values for which document IDs (after `startsWith`) should be matched.<br>(`?` any single character, `*` any characters).     |
| **exclude**                         | `string`                    | Pipe ('&#124;') separated values for which document IDs (after `startsWith`) should Not be matched.<br>(`?` any single character, `*` any characters). |

{CODE:nodejs syntax_2@client-api\commands\documents\get.js /}

{PANEL/}

## Related Articles

### Commands

- [Put](../../../client-api/commands/documents/put)
- [Delete](../../../client-api/commands/documents/delete)
- [How to Send Multiple Commands Using a Batch](../../../client-api/commands/batches/how-to-send-multiple-commands-using-a-batch)
