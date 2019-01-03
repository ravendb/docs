# Operations : Server : How to Create a Database

Create a new database on a server.

## Syntax

{CODE:java create_database_syntax@ClientApi\Operations\Server\CreateDeleteDatabase.java /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **databaseRecord** | DatabaseRecord | instance of `DatabaseRecord` containing database configuration |
| **replicationFactor** | int | indicates how many nodes should contain the database |

## Example

{CODE:java CreateDatabase@ClientApi\Operations\Server\CreateDeleteDatabase.java /}

{INFO:Information}
To ensure database exists before creating it we can use the following example

###Example - EnsureDatabaseExists

{CODE:java EnsureDatabaseExists@ClientApi\Operations\Server\CreateDeleteDatabase.java /}

{INFO/}

{NOTE Creation of a database requires admin certificate /}

## Related Articles
- [Distributed Database](../../../server/clustering/distribution/distributed-database)
- [Create Database via Studio](../../../studio/server/databases/create-new-database/general-flow)
