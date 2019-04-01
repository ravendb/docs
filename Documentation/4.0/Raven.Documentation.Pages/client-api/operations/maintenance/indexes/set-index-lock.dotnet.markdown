# Operations: How to Change the Index Lock Mode

**SetIndexesLockOperation**  allows you to change index lock mode for a given index or indexes.

## Syntax

{CODE set_lock_mode_1@ClientApi\Operations\ChangeIndexLockMode.cs /}

{CODE set_lock_mode_2@ClientApi\Operations\ChangeIndexLockMode.cs /}

{CODE set_lock_mode_3@ClientApi\Operations\ChangeIndexLockMode.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | string | name of an index to change lock mode for |
| **lockMode** | IndexLockMode | new index lock mode |
| **parameters** | SetIndexesLockOperation.Parameters | list of indexes + new index lock mode |

## Example I

{CODE set_lock_mode_5@ClientApi\Operations\ChangeIndexLockMode.cs /}

## Example II

{CODE set_lock_mode_5@ClientApi\Operations\ChangeIndexLockMode.cs /}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../server/administration/index-administration)

### Operations

- [How to Change Index Priority](../../../../client-api/operations/maintenance/indexes/set-index-priority)
