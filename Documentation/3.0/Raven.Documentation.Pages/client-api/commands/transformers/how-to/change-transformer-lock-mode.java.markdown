# Commands: Transformers: How to change transformer lock mode?

**SetTransformerLock** method allows you to change transformer lock mode for a given transformer.

## Syntax

{CODE:java change_transformer_lock_1@ClientApi\Commands\Transformers\HowTo\ChangeTransformerLockMode.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | String | name of a transformer to change lock mode for |
| **lockMode** | TransformerLockMode | new transformer lock mode |

## Example

{CODE:java change_transformer_lock_2@ClientApi\Commands\Transformers\HowTo\ChangeTransformerLockMode.java /}

## Related articles

- [How to **change index lock mode**?](../../../../client-api/commands/indexes/how-to/change-index-lock-mode)  
