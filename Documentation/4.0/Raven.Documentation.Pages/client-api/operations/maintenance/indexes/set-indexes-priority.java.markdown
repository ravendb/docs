# Operations : How to Change Index Priorities

**SetIndexesPriorityOperation**  allows you to change an index priority for a given index or indexes.

## Syntax

{CODE:java set_priority_1@ClientApi\Operations\ChangeIndexPriority.java /}

{CODE:java set_priority_2@ClientApi\Operations\ChangeIndexPriority.java /}

{CODE:java set_priority_3@ClientApi\Operations\ChangeIndexPriority.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | String | name of an index to change priority for |
| **priority** | IndexingPriority | new index priority |
| **parameters** | SetIndexesPriorityOperation.Parameters | list of indexes + new index priority |

## Example I

{CODE:java set_priority_4@ClientApi\Operations\ChangeIndexPriority.java /}

## Example II

{CODE:java set_priority_5@ClientApi\Operations\ChangeIndexPriority.java /}

## Related Articles

- [How to **change index lock mode**?](../../../../client-api/operations/maintenance/indexes/set-indexes-lock)
