# Commands : Documents : Get

There are few methods that allow you to retrieve documents from a database:   
- [Get](../../../client-api/commands/documents/get#get)   
- [Get - multiple documents](../../../client-api/commands/documents/get#get---multiple-documents)   
- [GetDocuments](../../../client-api/commands/documents/get#getdocuments)   
- [StartsWith](../../../client-api/commands/documents/get#startswith)  

{PANEL:Get}

**Get** can be used to retrieve a single document.

### Syntax

{CODE get_1_0@ClientApi\Commands\Documents\Get.cs /}

**Parameters**   

key
:   Type: string   
key of the document you want to retrieve  

**Return Value**

Type: [JsonDocument](../../../glossary/json/json-document)   
Object representing retrieved document.

### Example

{CODE get_1_2@ClientApi\Commands\Documents\Get.cs /}

{PANEL/}

{PANEL:Get - multiple documents}

**Get** can also be used to retrieve a list of documents.

### Syntax

{CODE get_2_0@ClientApi\Commands\Documents\Get.cs /}

**Parameters**   

keys
:   Type: string[]   
array of keys of the documents you want to retrieve 

includes
:   Type: string[]   
array of paths in documents in which server should look for a 'referenced' document (check example)  

transformer
:   Type: string   
name of a transformer that should be used to transform the results    

transformerParameters
:   Type: Dictionary<string, RavenJToken>   
inputs (parameters) that will can be used by transformer

metadataOnly
:   Type: bool   
specifies if only document metadata should be returned

**Return Value**

{CODE multiloadresult@Common.cs /}

Results
:   Type: List<RavenJObject>   
list of documents in **exact** same order as in **keys** parameter

Includes
:   Type: List<RavenJObject>   
list of documents that were found in specified paths that were passed in **includes** parameter     

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

**Parameters**   

start
:   Type: int   
number of documents that should be skipped 

pageSize
:   Type: int   
maximum number of documents that will be retrieved 

metadataOnly
:   Type: bool   
specifies if only document metadata should be returned   

**Return Value**

Type: [JsonDocument](../../../glossary/json/json-document)   
Object representing retrieved document.

### Example

{CODE get_3_1@ClientApi\Commands\Documents\Get.cs /}

{PANEL/}

{PANEL:StartsWith}

**StartsWith** can be used to retrieve multiple documents for a specified key prefix.

### Syntax

{CODE get_4_0@ClientApi\Commands\Documents\Get.cs /}

**Parameters**   

keyPrefix
:   Type: string   
prefix for which documents should be returned 

matches
:   Type: string   
pipe ('|') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters)  

start
:   Type: int   
number of documents that should be skipped 

pageSize
:   Type: int   
maximum number of documents that will be retrieved

pagingInformation
:   Type: RavenPagingInformation   
used to perform rapid pagination on server side 

metadataOnly
:   Type: bool   
specifies if only document metadata should be returned   

exclude
:   Type: string   
pipe ('|') separated values for which document keys (after 'keyPrefix') should **not** be matched ('?' any single character, '*' any characters)       

transformer
:   Type: string   
name of a transformer that should be used to transform the results  

transformerParameters
:   Type: Dictionary<string, RavenJToken>      
inputs (parameters) that will can be used by transformer   

**Return Value**

Type: [JsonDocument](../../../glossary/json/json-document)   
Object representing retrieved document.

### Example I

{CODE get_4_1@ClientApi\Commands\Documents\Get.cs /}

### Example II

{CODE get_4_2@ClientApi\Commands\Documents\Get.cs /}

### Example III

{CODE get_4_3@ClientApi\Commands\Documents\Get.cs /}

{PANEL/}

#### Related articles

- [How to **get** document **metadata** only?](../../../client-api/commands/documents/how-to/get-document-metadata-only)  
- [Put](../../../client-api/commands/documents/put)  
- [Delete](../../../client-api/commands/documents/delete)   
- [How to **stream** documents?](../../../client-api/commands/documents/stream)   