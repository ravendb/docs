# Operations: Server: How to Create a Database

Create a new database on a server.

## Syntax

{CODE:csharp create_database_syntax@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}

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
| Disabled | `bool` (default: false) | [Disable](../../../client-api/operations/server-wide/toggle-databases-state) the database. |
| Encrypted | `bool` (default: false) | Enables database [encryption](../../../server/security/encryption/database-encryption). |
| DeletionInProgress | `Dictionary<string, DeletionInProgress>` | Mark the deletion of the database from specific nodes. |
| Topology | `DatabaseTopology` | By default it is `null` and the server will decided on which nodes to place the database according to the `Replication Factor`. |
| ConflictSolverConfig | `ConflictSolver` | Specify the strategy to resolve [Conflicts](../../../server/clustering/replication/replication-conflicts). |
| Indexes | `Dictionary<string, IndexDefinition>` | Define [Indexes](../../../Indexes/creating-and-deploying#using-maintenance-operations) |
| AutoIndexes | `Dictionary<string, AutoIndexDefinition>` | Define [Auto Indexes](../../../Indexes/creating-and-deploying#using-maintenance-operations) |
| Settings | `Dictionary<string, string>` | Provide [Configuration](../../../server/configuration/configuration-options) settings. |
| Revisions | `RevisionsConfiguration` | Set [Revision](../../../server/extensions/revisions) configuration. |
| Expiration | `ExpirationConfiguration` | Set [Expiration](../../../server/extensions/expiration) configuration. |
| RavenConnectionStrings | `Dictionary<string, RavenConnectionString>` | Add [Raven Connection String](../../../client-api/operations/maintenance/connection-strings/add-connection-string)|
| SqlConnectionStrings | `Dictionary<string, SqlConnectionString>` | Add [SQL Connection String](../../../client-api/operations/maintenance/connection-strings/add-connection-string) |
| PeriodicBackups | `List<PeriodicBackupConfiguration>` | Add [Backup](../../../server/ongoing-tasks/backup-overview) tasks. |
| ExternalReplications | `List<ExternalReplications>` | Add [External Replication](../../../server/ongoing-tasks/external-replication) tasks. |
| RavenEtls | `List<RavenEtlConfiguration>` | Add [Raven ETL](../../../server/ongoing-tasks/etl/raven) tasks. |
| SqlEtls | `List<SqlEtlConfiguration>` | Add [SQL ETL](../../../server/ongoing-tasks/etl/sql) tasks. |
| Client | `ClientConfiguration` | Set [Client Configuration](../../../studio/server/client-configuration) |

#### Remarks
If `Topology` is specified, the `replicationFactor` will be ignored.

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync CreateDatabase@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TAB:csharp:Async CreateDatabaseAsync@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TABS/}

{INFO:Information}
To ensure database exists before creating it we can use the following example

###Example - EnsureDatabaseExists
{CODE-TABS}
{CODE-TAB:csharp:Sync EnsureDatabaseExists@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TAB:csharp:Async EnsureDatabaseExistsAsync@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TABS/}

{INFO/}

{NOTE Creation of a database requires admin certificate /}

## Related Articles
- [Distributed Database](../../../server/clustering/distribution/distributed-database)
- [Create Database via Studio](../../../studio/server/databases/create-new-database/general-flow)
