# Operations: Server: How to Compact a Database

To compact database, please use **CompactDatabaseOperation**. You can choose what should be compacted: documents and/or listed indexes.

## Syntax

{CODE:java compact_1@ClientApi\Operations\Server\Compact.java /}

{CODE:java compact_2@ClientApi\Operations\Server\Compact.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **DatabaseName** | String | Name of a database to compact |
| **Documents** | boolean | Indicates if documents should be compacted |
| **Indexes** | String[] | List of index names to compact |

## Example I

{CODE:java compact_3@ClientApi\Operations\Server\Compact.java /}


## Example II

{CODE:java compact_4@ClientApi\Operations\Server\Compact.java /}


## Remarks

The compacting operation is executed **asynchronously** and during this operation the **database** will be **offline**.

## Related Articles

- [How to **create** database?](../../../client-api/operations/server-wide/create-database) 
- [How to get database **statistics**?](../../../client-api/operations/maintenance/get-statistics)
- [How to start **restore** operation?](../../../client-api/operations/server-wide/restore-backup)

