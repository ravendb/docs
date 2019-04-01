# Commands: How to compact database?

To compact a database, please use `compactDatabase` command available in `GlobalAdmin`.

## Syntax

{CODE:java compact_1@ClientApi\Commands\HowTo\Compact.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **databaseName** | String | Name of a database to compact |

## Example

{CODE:java compact_2@ClientApi\Commands\HowTo\Compact.java /}

## Remarks

Compacting operation is executed **asynchronously** and during this operation the **database** will be **offline**.

## Related articles

- [How to **create** or **delete database**?](../../../client-api/commands/how-to/create-delete-database)     
- [How to get database and server **statistics**?](../../../client-api/commands/how-to/get-database-and-server-statistics)   
- [How to start **backup** or **restore** operations?](../../../client-api/commands/how-to/start-backup-restore-operations)   

