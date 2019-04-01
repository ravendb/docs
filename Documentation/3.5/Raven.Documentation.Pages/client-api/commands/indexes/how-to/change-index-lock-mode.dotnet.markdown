# Commands: Indexes: How to change index lock mode?

**SetIndexLock** method allows you to change index lock mode for a given index.

## Syntax

{CODE change_index_lock_1@ClientApi\Commands\Indexes\HowTo\ChangeIndexLockMode.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | string | name of an index to change lock mode for |
| **lockMode** | IndexLockMode | new index lock mode |

## Example

{CODE change_index_lock_2@ClientApi\Commands\Indexes\HowTo\ChangeIndexLockMode.cs /}

## Related articles

- [How to **change index priority**?](../../../../client-api/commands/indexes/how-to/change-index-priority)  
- [How to **change transformer lock mode**?](../../../../client-api/commands/transformers/how-to/change-transformer-lock-mode)  
