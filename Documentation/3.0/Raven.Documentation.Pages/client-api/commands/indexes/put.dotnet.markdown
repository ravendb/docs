# Commands : Indexes : Put

There are few methods that allow you to insert index into a database:   
- [PutIndex](../../../client-api/commands/indexes/put#putindex)   
- [PutIndex - using IndexDefinitionBuilder](../../../client-api/commands/indexes/put#putindex---using-indexdefinitionbuilder)   

## PutIndex

**PutIndex** is used to insert an index into database.

### Syntax

{CODE put_1_0@ClientApi\Commands\Indexes\Put.cs /}

**Parameters**   

name
:   Type: string   
Name of an index

indexDef
:   Type: [IndexDefinition](../../../glossary/indexes/index-definition)    
Definition of an index  

overwrite
:   Type: bool   
Indicates if index can be overwritten (if 'false' then exception will be thrown if index exists)  

**Return Value**

Type: string   
Index **name**.

### Example

{CODE put_1_1@ClientApi\Commands\Indexes\Put.cs /}

## PutIndex - using IndexDefinitionBuilder

**PutIndex** is used to insert an index into database. 

To help users create their indexes, `IndexDefinitionBuilder` was created that enables users to create indexes using strongly-typed syntax.

### Syntax

{CODE put_2_0@ClientApi\Commands\Indexes\Put.cs /}

**Parameters**   

name
:   Type: string   
Name of an index

indexDef
:   Type: IndexDefinitionBuilder<TDocument, TReduceResult>   
strongly-typed index definition

overwrite
:   Type: bool   
Indicates if index can be overwritten (if 'false' then exception will be thrown if index exists)  

**Return Value**

Type: string   
Index **name**.

### Example

{CODE put_2_1@ClientApi\Commands\Indexes\Put.cs /}

## Remarks

If **overwrite** is set to **true** and `IndexDefinition` haven't changed, no action will be taken on server-side and no indexing data will be lost.

[SAFE BY DEFAULT] By default, **PutIndex** methods does **not allow** indexes to be **overwritten**, because this causes all previous indexing data to be lost, which in many cases is not desired.

#### Related articles

- [How to **reset index**?](../../../client-api/commands/indexes/reset-index)  
- [GetIndex](../../../client-api/commands/indexes/get)  
- [DeleteIndex](../../../client-api/commands/indexes/delete)  
