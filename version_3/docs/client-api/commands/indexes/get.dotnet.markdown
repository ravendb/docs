# Client API : Indexes : Get

There are few methods that allow you to retrieve index from a database:   
- [GetIndex](../../../client-api/commands/indexes/get#getindex)   
- [GetIndexes](../../../client-api/commands/indexes/get#getindexes)   
- [GetIndexNames](../../../client-api/commands/indexes/get#getindexnames)   

## GetIndex

**GetIndex** is used to retrieve an index definition from a database.

### Syntax

{CODE get_1_0@ClientApi\Commands\Indexes\Get.cs /}

**Parameters**   

name
:   Type: string   
name of an index

**Return Value**

Type: [IndexDefinition](../../../glossary/indexes/index-definition)   
Instance of IndexDefinition repesenting index.

### Example

{CODE get_1_1@ClientApi\Commands\Indexes\Get.cs /}

## GetIndexes

**GetIndexes** is used to retrieve multiple index definitions from a database.

### Syntax

{CODE get_2_0@ClientApi\Commands\Indexes\Get.cs /}

**Parameters**   

start
:   Type: string   
Number of documents that should be skipped

pageSize
:   Type: int   
Maximum number of documents that will be retrieved  

**Return Value**

Type: [IndexDefinition](../../../glossary/indexes/index-definition)   
Instance of IndexDefinition repesenting index.

### Example

{CODE get_2_1@ClientApi\Commands\Indexes\Get.cs /}

## GetIndexNames

**GetIndexes** is used to retrieve multiple index names from a database.

### Syntax

{CODE get_3_0@ClientApi\Commands\Indexes\Get.cs /}

**Parameters**  
 
start
:   Type: string   
Number of documents that should be skipped

pageSize
:   Type: int   
Maximum number of documents that will be retrieved  

**Return Value**

Type: string[]   
This methods returns an array of index **name** as a result.

### Example

{CODE get_3_1@ClientApi\Commands\Indexes\Get.cs /}

#### Related articles

- [PutIndex](../../../client-api/commands/indexes/put)  
- [DeleteIndex](../../../client-api/commands/indexes/delete)  