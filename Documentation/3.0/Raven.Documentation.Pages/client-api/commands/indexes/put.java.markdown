# Commands : Indexes : Put

There are few methods that allow you to insert index into a database:   
- [PutIndex](../../../client-api/commands/indexes/put#putindex)   
- [PutIndex - using IndexDefinitionBuilder](../../../client-api/commands/indexes/put#putindex---using-indexdefinitionbuilder)   

{PANEL:PutIndex}

**PutIndex** is used to insert an index into database.

### Syntax

{CODE:java put_1_0@ClientApi\commands\indexes\Put.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | String | Name of an index |
| **indexDef** | [IndexDefinition](../../../glossary/indexes/index-definition) | Definition of an index |
| **overwrite** | boolean | Indicates if index can be overwritten (if `false` then exception will be thrown if index exists) |

| Return Value | |
| ------------- | ----- |
| String | Index **name** |

### Example

{CODE:java put_1_1@ClientApi\Commands\Indexes\Put.java /}

{PANEL/}

{PANEL:PutIndex - using IndexDefinitionBuilder}

**PutIndex** is used to insert an index into database. 

To help users create their indexes, `IndexDefinitionBuilder` was created that enables users to create indexes using QueryDSL expressions syntax. 

### Syntax

{CODE:java put_2_0@ClientApi\Commands\Indexes\Put.java /}  

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | String | Name of an index |
| **indexDef** | IndexDefinitionBuilder | index definition |
| **overwrite** | bool | Indicates if index can be overwritten (if `false` then exception will be thrown if index exists) |

| Return Value | |
| ------------- | ----- |
| String | Index **name**. |

### Example

{CODE:java put_2_1@ClientApi\commands\indexes\Put.java /}

{PANEL/}

## Remarks

If **overwrite** is set to **true** and `IndexDefinition` haven't changed, no action will be taken on server-side and no indexing data will be lost.

{SAFE By default, **PutIndex** methods does **not allow** indexes to be **overwritten**, because this causes all previous indexing data to be lost, which in many cases is not desired. /}

## Related articles

- [How to **reset index**?](../../../client-api/commands/indexes/reset-index)  
- [GetIndex](../../../client-api/commands/indexes/get)  
- [DeleteIndex](../../../client-api/commands/indexes/delete)  
