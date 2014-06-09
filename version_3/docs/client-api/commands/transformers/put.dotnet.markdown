# Client API : Transformers : Put

**PutTransformer** is used to remove a transformer from a database.

## Syntax

{CODE put_1@ClientApi\Commands\Transformers\Put.cs /}

**Parameters**   

{CODE transformerdefinition@Common.cs /}

- name - name of a transformer to delete   
- transformerDef - definition of a transformer   

**Return Value**

This methods returns a transformer **name** as a result.

## Example

{CODE put_2@ClientApi\Commands\Transformers\Put.cs /}

#### Related articles

- [GetTransformer](../../../client-api/commands/transformers/get)  
- [DeleteTransformer](../../../client-api/commands/transformers/delete)  