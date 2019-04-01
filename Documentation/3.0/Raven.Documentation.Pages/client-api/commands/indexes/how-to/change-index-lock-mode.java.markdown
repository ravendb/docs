# Commands: Indexes: How to change index lock mode?

**setIndexLock** method allows you to change index lock mode for a given index.

## Syntax

{CODE:java change_index_lock_1@ClientApi\Commands\Indexes\HowTo\ChangeIndexLockMode.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | string | name of an index to change lock mode for |
| **lockMode** | IndexLockMode | new index lock mode |

## Example

{CODE:java change_index_lock_2@ClientApi\Commands\Indexes\HowTo\ChangeIndexLockMode.java /}

## Related articles

- [How to **change index priority**?](../../../../client-api/commands/indexes/how-to/change-index-priority)  
