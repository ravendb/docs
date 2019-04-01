# Commands: Transformers: Put

**PutTransformer** is used to add a transformer to a database.

## Syntax

{CODE:java put_1@ClientApi\Commands\Transformers\Put.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | String | name of a transformer |
| **transformerDef** | [TransformerDefinition](../../../glossary/transformer-definition) | definition of a transformer |

| Return Value | |
| ------------- | ----- |
| String | Transformer **name**. |

## Example

{CODE:java put_2@ClientApi\Commands\Transformers\Put.java /}

## Related articles

- [GetTransformer](../../../client-api/commands/transformers/get)  
- [DeleteTransformer](../../../client-api/commands/transformers/delete)  
