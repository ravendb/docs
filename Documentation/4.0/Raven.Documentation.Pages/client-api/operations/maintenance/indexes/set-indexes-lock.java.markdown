# Operations : How to Change the Index Lock Mode

**SetIndexesLockOperation**  allows you to change index lock mode for a given index or indexes.

## Syntax

{CODE:java set_lock_mode_1@ClientApi\Operations\ChangeIndexLockMode.java /}

{CODE:java set_lock_mode_2@ClientApi\Operations\ChangeIndexLockMode.java /}

{CODE:java set_lock_mode_3@ClientApi\Operations\ChangeIndexLockMode.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | String | name of an index to change lock mode for |
| **lockMode** | IndexLockMode | new index lock mode |
| **parameters** | SetIndexesLockOperation.Parameters | list of indexes + new index lock mode |

## Example I

{CODE:java set_lock_mode_5@ClientApi\Operations\ChangeIndexLockMode.java /}

## Example II

{CODE:java set_lock_mode_5@ClientApi\Operations\ChangeIndexLockMode.java /}

## Related Articles

- [How to **change index priority**?](../../../../client-api/operations/maintenance/indexes/set-indexes-priority)
