# Set Index Priority Operation

**SetIndexesPriorityOperation**  allows you to change an index priority for a given index or indexes.  

Setting the priority will affect the indexing thread priority at the operating system level:  

| Priority value | Indexing thread priority<br> at OS level | |
| - | - | - |
| __Low__ | Lowest | <ul><li>Having the `Lowest` priority at the OS level, indexes will run only when there's a capacity for them, when the system is not occupied with higher-priority tasks.</li><li>Requests to the database will complete faster.<br>Use when querying the server is more important to you than indexing.</li></ul> |
| __Normal__ (default) | Below normal | <ul><li>Requests to the database are still preferred over the indexing process.</li><li>The indexing thread priority at the OS level is `Below normal`<br>while Requests processes have a `Normal` priority.</li></ul> |
| __High__ | Normal | <ul><li>Requests and Indexing will have the same priority at the OS level.</li></ul> |

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

- [Index Administration](../../../../indexes/index-administration)

### Operations

- [How to Change Index Lock Mode](../../../../client-api/operations/maintenance/indexes/set-index-lock)
