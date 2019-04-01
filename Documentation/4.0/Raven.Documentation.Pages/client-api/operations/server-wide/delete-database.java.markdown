# Operations: Server: How to delete a database?

This operation is used to delete databases from a server, with a possibility to remove all the data from hard drive.

## Syntax

{CODE:java delete_database_syntax@ClientApi\Operations\Server\CreateDeleteDatabase.java /}

{CODE:java delete_parameters@ClientApi\Operations\Server\CreateDeleteDatabase.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **DatabaseName** | String | Name of a database to delete |
| **HardDelete** | boolean | Should all data be removed (data files, indexing files, etc.). |
| **FromNode** | String | Remove the database just from a specific node. Default: `null` which would delete from all |
| **TimeToWaitForConfirmation** | Duration | Time to wait for confirmation. Default: `null` will user server default (15 seconds) |

## Example I

{CODE:java DeleteDatabases@ClientApi\Operations\Server\CreateDeleteDatabase.java /}

## Example II

In order to delete just one database from a server, you can also use this constructor

{CODE:java DeleteDatabase@ClientApi\Operations\Server\CreateDeleteDatabase.java /}
