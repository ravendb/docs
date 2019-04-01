# Commands: Documents: Get

There are few methods that allow you to retrieve documents from a database:   
- [Get](../../../client-api/commands/documents/get#get)   
- [Get - multiple documents](../../../client-api/commands/documents/get#get---multiple-documents)   
- [GetDocuments](../../../client-api/commands/documents/get#getdocuments)   
- [StartsWith](../../../client-api/commands/documents/get#startswith)  

{PANEL:Get}

**Get** can be used to retrieve a single document.

### Syntax

{CODE:java get_1_0@ClientApi\commands\documents\get.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **key** | String | key of the document you want to retrieve |

| Return Value | |
| ------------- | ----- |
| [JsonDocument](../../../glossary/json-document) | Object representing the retrieved document. |

### Example

{CODE:java get_1_2@ClientApi\commands\documents\get.java /}

{PANEL/}

{PANEL:Get - multiple documents}

**Get** can also be used to retrieve a list of documents.

### Syntax

{CODE:java get_2_0@clientApi\commands\documents\Get.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **keys** | String[] | array of keys of the documents you want to retrieve |
| **includes** | String[] | array of paths in documents in which server should look for a 'referenced' document (check example) |
| **transformer** | String | name of a transformer that should be used to transform the results (default: `null`) |
| **transformerParameters** | Map<string, RavenJToken> | inputs (parameters) that will be used by transformer (default: `null`) |
| **metadataOnly** | boolean | specifies if only document metadata should be returned (default: `false`) |

<hr />

{CODE:java multiloadresult@Common.java /}

| Return Value | | |
| ------------- | ------------- | ----- |
| **results** | List&lt;RavenJObject&gt; | list of documents in **exact** same order as in **keys** parameter |
| **includes** | List&lt;RavenJObject&gt; | list of documents that were found in specified paths that were passed in **includes** parameter |

### Example I

{CODE:java get_2_2@ClientApi\Commands\Documents\Get.java /}

### Example II - using includes

{CODE:java get_2_3@ClientApi\Commands\Documents\Get.java /}

### Example III - missing documents

{CODE:java get_2_4@ClientApi\Commands\Documents\Get.java /}

{PANEL/}

{PANEL:GetDocuments}

**GetDocuments** can be used to retrieve multiple documents.

### Syntax

{CODE:java get_3_0@ClientApi\commands\documents\Get.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **start** | int | number of documents that should be skipped  |
| **pageSize** | int | maximum number of documents that will be retrieved |
| **metadataOnly** | boolean | specifies if only document metadata should be returned (default: `false`) |  

| Return Value | |
| ------------- | ----- |
| [JsonDocument](../../../glossary/json-document) | Object representing retrieved document. |

### Example

{CODE:java get_3_1@ClientApi\commands\documents\Get.java /}

{PANEL/}

{PANEL:StartsWith}

**StartsWith** can be used to retrieve multiple documents for a specified key prefix.

### Syntax

{CODE:java get_4_0@ClientApi\commands\documents\Get.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **keyPrefix** | String | prefix for which documents should be returned |
| **matches** | String | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters) |
| **start** | int | number of documents that should be skipped |
| **pageSize** | int | maximum number of documents that will be retrieved |
| **pagingInformation** | RavenPagingInformation | used to perform rapid pagination on a server side (default: `null`) |
| **metadataOnly** | boolean | specifies if only document metadata should be returned (default : `false`) |
| **exclude** | String | pipe ('&#124;') separated values for which document keys (after 'keyPrefix') should **not** be matched ('?' any single character, '*' any characters) (default: `null`) |
| **transformer** | String | name of a transformer that should be used to transform the results (default: `null`) |
| **transformerParameters** | Map<String, RavenJToken> | inputs (parameters) that will be used by transformer (default: `null`) |
| **skipAfter** | String | skip document fetching until given key is found and return documents after that key (default: `null`) |

| Return Value | |
| ------------- | ----- |
| [JsonDocument](../../../glossary/json-document) | Object representing retrieved document. |

### Example I

{CODE:java get_4_1@ClientApi\Commands\Documents\Get.java /}

### Example II

{CODE:java get_4_2@ClientApi\Commands\Documents\Get.java /}

### Example III

{CODE:java get_4_3@ClientApi\Commands\Documents\Get.java /}

{PANEL/}

## Related articles

- [How to **get** document **metadata** only?](../../../client-api/commands/documents/how-to/get-document-metadata-only)  
- [Put](../../../client-api/commands/documents/put)  
- [Delete](../../../client-api/commands/documents/delete)   
- [How to **stream** documents?](../../../client-api/commands/documents/stream)   
