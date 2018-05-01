# Operations : Server : How to Create a Database

Create a new database on a server.

## Syntax

{CODE:java create_database_syntax@ClientApi\Operations\Server\CreateDeleteDatabase.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **databaseRecord** | DatabaseRecord | instance of `DatabaseRecord` containing database configuration |
| **replicationFactor** | int | indicates how many nodes should contain the database |

## Example

{CODE:java CreateDatabase@ClientApi\Operations\Server\CreateDeleteDatabase.java /}
