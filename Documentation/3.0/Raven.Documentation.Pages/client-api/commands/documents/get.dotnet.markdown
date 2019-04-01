# Commands: Documents: Get

There are few methods that allow you to retrieve documents from a database:   
- [Get](../../../client-api/commands/documents/get#get)   
- [Get - multiple documents](../../../client-api/commands/documents/get#get---multiple-documents)   
- [GetDocuments](../../../client-api/commands/documents/get#getdocuments)   
- [StartsWith](../../../client-api/commands/documents/get#startswith)  

{PANEL:Get}

**Get** can be used to retrieve a single document.

### Syntax

{CODE get_1_0@ClientApi\Commands\Documents\Get.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | string | key of the document you want to retrieve |

| Return Value | |
| ------------- | ----- |
| [JsonDocument](../../../glossary/json-document) | Object representing the retrieved document. |

### Example

{CODE get_1_2@ClientApi\Commands\Documents\Get.cs /}

{PANEL/}

{PANEL:Get - multiple documents}

**Get** can also be used to retrieve a list of documents.

### Syntax

{CODE get_2_0@ClientApi\Commands\Documents\Get.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **keys** | string[] | array of keys of the documents you want to retrieve |
| **includes** | string[] | array of paths in documents in which server should look for a 'referenced' document (check example) |
| **transformer** | string | name of a transformer that should be used to transform the results |
| **transformerParameters** | Dictionary<string, RavenJToken> | parameters that will be passed to transformer |
| **metadataOnly** | bool | specifies if only document metadata should be returned |

<hr />

{CODE multiloadresult@Common.cs /}

| Return Value | | |
| ------------- | ------------- | ----- |
| **Results** | List&lt;RavenJObject&gt; | list of documents in **exact** same order as in **keys** parameter |
| **Includes** | List&lt;RavenJObject&gt; | list of documents that were found in specified paths that were passed in **includes** parameter |

### Example I

{CODE get_2_2@ClientApi\Commands\Documents\Get.cs /}

### Example II - using includes

{CODE get_2_3@ClientApi\Commands\Documents\Get.cs /}

### Example III - missing documents

{CODE get_2_4@ClientApi\Commands\Documents\Get.cs /}

{PANEL/}

{PANEL:GetDocuments}

**GetDocuments** can be used to retrieve multiple documents.

### Syntax

{CODE get_3_0@ClientApi\Commands\Documents\Get.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | number of documents that should be skipped  |
| **pageSize** | int | maximum number of documents that will be retrieved |
| **metadataOnly** | bool | specifies if only document metadata should be returned |  

| Return Value | |
| ------------- | ----- |
| [JsonDocument](../../../glossary/json-document) | Object representing retrieved document. |

### Example

{CODE get_3_1@ClientApi\Commands\Documents\Get.cs /}

{PANEL/}

{PANEL:StartsWith}

**StartsWith** can be used to retrieve multiple documents for a specified key prefix.

### Syntax

{CODE get_4_0@ClientApi\Commands\Documents\Get.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **keyPrefix** | string | prefix for which documents should be returned |
| **matches** | string | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters) |
| **start** | int | number of documents that should be skipped |
| **pageSize** | int | maximum number of documents that will be retrieved |
| **pagingInformation** | RavenPagingInformation | used to perform rapid pagination on a server side  |
| **metadataOnly** | bool | specifies if only document metadata should be returned |
| **exclude** | string | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should **not** be matched ('?' any single character, '*' any characters) |
| **transformer** | string | name of a transformer that should be used to transform the results |
| **transformerParameters** | Dictionary<string, RavenJToken> | parameters that will be passed to transformer |
| **skipAfter** | string | skip document fetching until given key is found and return documents after that key (default: `null`) |

| Return Value | |
| ------------- | ----- |
| [JsonDocument](../../../glossary/json-document) | Object representing retrieved document. |

### Example I

{CODE get_4_1@ClientApi\Commands\Documents\Get.cs /}

### Example II

{CODE get_4_2@ClientApi\Commands\Documents\Get.cs /}

### Example III

{CODE get_4_3@ClientApi\Commands\Documents\Get.cs /}

{PANEL/}

## Related articles

- [How to **get** document **metadata** only?](../../../client-api/commands/documents/how-to/get-document-metadata-only)  
- [Put](../../../client-api/commands/documents/put)  
- [Delete](../../../client-api/commands/documents/delete)   
- [How to **stream** documents?](../../../client-api/commands/documents/stream)   
