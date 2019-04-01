# Commands: Transformers: How to change transformer lock mode?

**SetTransformerLock** method allows you to change transformer lock mode for a given transformer.

## Syntax

{CODE change_transformer_lock_1@ClientApi\Commands\Transformers\HowTo\ChangeTransformerLockMode.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | string | name of a transformer to change lock mode for |
| **lockMode** | TransformerLockMode | new transformer lock mode |

## Example

{CODE change_transformer_lock_2@ClientApi\Commands\Transformers\HowTo\ChangeTransformerLockMode.cs /}

## Related articles

- [How to **change index lock mode**?](../../../../client-api/commands/indexes/how-to/change-index-lock-mode)  
