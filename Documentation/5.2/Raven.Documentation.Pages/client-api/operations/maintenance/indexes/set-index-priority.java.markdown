# Set Index Priority Operation

**SetIndexesPriorityOperation**  allows you to change an index priority for a given index or indexes.

## Syntax

{CODE:java set_priority_1@ClientApi\Operations\Maintenance\Indexes\SetPriority.java /}

{CODE:java set_priority_2@ClientApi\Operations\Maintenance\Indexes\SetPriority.java /}

{CODE:java set_priority_3@ClientApi\Operations\Maintenance\Indexes\SetPriority.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | String | name of an index to change priority for |
| **priority** | IndexingPriority | new index priority |
| **parameters** | SetIndexesPriorityOperation.Parameters | list of indexes + new index priority |

## Example I

{CODE:java set_priority_4@ClientApi\Operations\Maintenance\Indexes\SetPriority.java /}

## Example II

{CODE:java set_priority_5@ClientApi\Operations\Maintenance\Indexes\SetPriority.java /}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)

### Operations

- [How to Change Index Lock Mode](../../../../client-api/operations/maintenance/indexes/set-index-lock)
