# Operations : How to change index priority?

**SetIndexesPriorityOperation**  allows you to change index priority for a given index or indexes.

## Syntax

{CODE set_priority_1@ClientApi\Operations\ChangeIndexPriority.cs /}

{CODE set_priority_2@ClientApi\Operations\ChangeIndexPriority.cs /}

{CODE set_priority_3@ClientApi\Operations\ChangeIndexPriority.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | string | name of an index to change priority for |
| **priority** | IndexingPriority | new index priority |
| **parameters** | SetIndexesPriorityOperation.Parameters | list of indexes + new index priority |

## Example I

{CODE set_priority_4@ClientApi\Operations\ChangeIndexPriority.cs /}

## Example II

{CODE set_priority_5@ClientApi\Operations\ChangeIndexPriority.cs /}

## Related articles

- [How to **change index lock mode**?](../../../../client-api/operations/set-indexes-lock-operation)  
