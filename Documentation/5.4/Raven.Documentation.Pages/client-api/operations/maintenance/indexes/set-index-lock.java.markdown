# Set Index Lock Mode Operation

**SetIndexesLockOperation**  allows you to change index lock mode for a given index or indexes.

## Syntax

{CODE:java set_lock_mode_1@ClientApi\Operations\Maintenance\Indexes\SetLockMode.java /}

{CODE:java set_lock_mode_2@ClientApi\Operations\Maintenance\Indexes\SetLockMode.java /}

{CODE:java set_lock_mode_3@ClientApi\Operations\Maintenance\Indexes\SetLockMode.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **name** | String | name of an index to change lock mode for |
| **lockMode** | IndexLockMode | new index lock mode |
| **parameters** | SetIndexesLockOperation.Parameters | list of indexes + new index lock mode |

## Example I

{CODE:java set_lock_mode_4@ClientApi\Operations\Maintenance\Indexes\SetLockMode.java /}

## Example II

{CODE:java set_lock_mode_5@ClientApi\Operations\Maintenance\Indexes\SetLockMode.java /}

## Related Articles

### Indexes

- [What are Indexes](../../../../indexes/what-are-indexes)
- [Creating and Deploying Indexes](../../../../indexes/creating-and-deploying)

### Server

- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Change Index Priority](../../../../client-api/operations/maintenance/indexes/set-index-priority)
