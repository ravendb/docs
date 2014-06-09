# Client API : Documents : Get

There are few methods that allow you to retrieve documents from a database:   
- [Get](../../../client-api/commands/documents/get#get)   
- [Get - multiple documents](../../../client-api/commands/documents/get#get---multiple-documents)   
- [GetDocuments](../../../client-api/commands/documents/get#getdocuments)   
- [StartsWith](../../../client-api/commands/documents/get#startswith)  

## Get

**Get** can be used to retrieve a single document.

### Syntax

{CODE get_1_0@ClientApi\Commands\Documents\Get.cs /}

**Parameters**   

- key - key of the document you want to retrieve   

**Return Value**

{CODE jsondocument@Common.cs /}

- DataAsJson - document data   
- Metadata - document metadata  
- Etag - current document ETag   
- Key - document key  
- LastModified - last modified date of document   
- NonAuthoritativeInformation - indicates if document is non authoritative (modified by uncommited transaction)   
- TempIndexScore - ranking of document in current query   

### Example

{CODE get_1_2@ClientApi\Commands\Documents\Get.cs /}

## Get - multiple documents

**Get** can also be used to retrieve a list of documents.

### Syntax

{CODE get_2_0@ClientApi\Commands\Documents\Get.cs /}

**Parameters**   

- keys - array of keys of the documents you want to retrieve   
- includes - array of paths in documents in which server should look for a 'referenced' document (check example)   
- transformer - name of a transformer that should be used to transform the results   
- queryInputs - inputs (parameters) that will can be used by transformer
- metadataOnly - specifies if only document metadata should be returned   

**Return Value**

{CODE multiloadresult@Common.cs /}

- Results - list of documents in **exact** same order as in **keys** parameter      
- Includes - list of documents that were found in specified paths that were passed in **includes** parameter      

### Example I

{CODE get_2_2@ClientApi\Commands\Documents\Get.cs /}

### Example II - using includes

{CODE get_2_3@ClientApi\Commands\Documents\Get.cs /}

### Example III - missing documents

{CODE get_2_4@ClientApi\Commands\Documents\Get.cs /}

## GetDocuments

**GetDocuments** can be used to retrieve multiple documents.

### Syntax

{CODE get_3_0@ClientApi\Commands\Documents\Get.cs /}

**Parameters**   

- start - number of documents that should be skipped   
- pageSize - maximum number of documents that will be retrieved   
- metadataOnly - specifies if only document metadata should be returned   

**Return Value**

{CODE jsondocument@Common.cs /}

- DataAsJson - document data   
- Metadata - document metadata  
- Etag - current document ETag   
- Key - document key  
- LastModified - last modified date of document   
- NonAuthoritativeInformation - indicates if document is non authoritative (modified by uncommited transaction)   
- TempIndexScore - ranking of document in current query   

### Example

{CODE get_3_1@ClientApi\Commands\Documents\Get.cs /}

## StartsWith

**StartsWith** can be used to retrieve multiple documents for a specified key prefix.

### Syntax

{CODE get_4_0@ClientApi\Commands\Documents\Get.cs /}

**Parameters**   

- keyPrefix - prefix for which to documents should be returned   
- matches - pipe ('|') separated values for which document keys (after 'keyPrefix') should be matched ('?' any single character, '*' any characters)    
- start - number of documents that should be skipped   
- pageSize - maximum number of documents that will be retrieved   
- pagingInformation - used to perform rapid pagination on server side      
- metadataOnly - specifies if only document metadata should be returned   
- exclude - pipe ('|') separated values for which document keys (after 'keyPrefix') should **not** be matched ('?' any single character, '*' any characters)       
- transformer - name of a transformer that should be used to transform the results   
- queryInputs - inputs (parameters) that will can be used by transformer   

**Return Value**

{CODE jsondocument@Common.cs /}

- DataAsJson - document data   
- Metadata - document metadata  
- Etag - current document ETag   
- Key - document key  
- LastModified - last modified date of document   
- NonAuthoritativeInformation - indicates if document is non authoritative (modified by uncommited transaction)   
- TempIndexScore - ranking of document in current query   

### Example I

{CODE get_4_1@ClientApi\Commands\Documents\Get.cs /}

### Example II

{CODE get_4_2@ClientApi\Commands\Documents\Get.cs /}

### Example III

{CODE get_4_3@ClientApi\Commands\Documents\Get.cs /}

#### Related articles

- [How to **get** document **metadata** only?](../../../client-api/commands/documents/how-to/get-document-metadata-only)  
- [Put](../../../client-api/commands/documents/put)  
- [Delete](../../../client-api/commands/documents/delete)   
- [How to **stream** documents?](../../../client-api/commands/documents/stream)   