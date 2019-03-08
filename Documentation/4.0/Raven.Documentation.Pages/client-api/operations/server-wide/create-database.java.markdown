# Operations : Server : How to Create a Database

Create a new database on a server.

## Syntax

{CODE:java create_database_syntax@ClientApi\Operations\Server\CreateDeleteDatabase.java /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **databaseRecord** | DatabaseRecord | instance of `DatabaseRecord` containing database configuration |
| **replicationFactor** | int | indicates how many nodes should contain the database |


## DatabaseRecord

`DatabaseRecord` is a collection of database configurations.  

### Constructor

| Name | Description |
| - | - |
| DatabaseRecord(`string` databaseName) | Initialize a new database record. |

### Properties
| Name | Type | Description |
| - | - | - |
| disabled | `boolean` (default: false) | [Disable](to-do) the database. |
| encrypted | `boolean` (default: false) | Enables database [encryption](../../../server/security/encryption/database-encryption). |
| deletionInProgress | `Map<String, DeletionInProgress>` | Mark the deletion of the database from specific nodes. |
| topology | `DatabaseTopology` | By default it is `null` and the server will decided on which nodes to place the database according to the `Replication Factor`. |
| conflictSolverConfig | `ConflictSolver` | Specify the strategy to resolve [Conflicts](../replication/replication-conflicts). |
| indexes | `Map<String, IndexDefinition>` | Define [Indexes](../../../Indexes/creating-and-deploying#using-maintenance-operations) |
| autoIndexes | `Map<String, AutoIndexDefinition>` | Define [Auto Indexes](../../../Indexes/creating-and-deploying#using-maintenance-operations) |
| settings | `Map<String, String>` | Provide [Configuration](../../../server/configuration) settings. |
| revisions | `RevisionsConfiguration` | Set [Revision](../../../server/extensions/revisions) configuration. |
| expiration | `ExpirationConfiguration` | Set [Expiration](../../../server/extensions/expiration) configuration. |
| ravenConnectionStrings | `Map<String, RavenConnectionString>` | Add [Raven Connection String](to-do)|
| sqlConnectionStrings | `Map<String, SqlConnectionString>` | Add [SQL Connection String](to-do) |
| periodicBackups | `List<PeriodicBackupConfiguration>` | Add [Backup](to-do) tasks. |
| externalReplications | `List<ExternalReplications>` | Add [External Replication](../../../server/ongoing-tasks/external-replication) tasks. |
| ravenEtls | `List<RavenEtlConfiguration>` | Add [Raven ETL](../../../server/ongoing-tasks/etl/raven) tasks. |
| sqlEtls | `List<SqlEtlConfiguration>` | Add [SQL ETL](../../../server/ongoing-tasks/etl/sql) tasks. |
| client | `ClientConfiguration` | Set [Client Configuration](../../../studio/server/client-configuration) |

#### Remarks
If `Topology` is specified, the `replicationFactor` will be ignored.

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
