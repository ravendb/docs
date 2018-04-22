# Operations : Server : How to Create a Database

Create a new database on a server.

## Syntax

{CODE:csharp create_database_syntax@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}

| Parameters | Type | Description |
| ------------- | ------------- | ----- |
| **databaseRecord** | string | instance of `DatabaseRecord` containing database configuration |
| **replicationFactor** | int | indicates how many nodes should contain the database |


## DatabaseRecrod

Once there is a [consensus](../../../server/clustering/rachis/consensus-operations) on the database creation, every database is stored in on _all_ cluster nodes as an object of the `DatabaseRecord` class.

### Constructor

| Name | Description |
| - | - |
| DatabaseRecord(`string` databaseName) | Initialize a new database record. |

### Properties
| Name | Type | Description |
| - | - | - |
| Disabled | `bool` (default: false) | [Disable](to-do) the database. |
| Encrypted | `bool` (default: false) | Enables database [encryption](../../../server/security/encryption/database-encryption). |
| DeletionInProgess | `Dictionary<string, DeletionInProgess>` | Mark the deletion of the database from specific nodes. |
| Topology | `DatabaseTopology` | Specify the database topology. |
| ConflictSolverConfig | `ConflictSolver` | Specify the strategy to resolve [Conflicts](../replication/replication-conflicts). |
| Indexes | `Dictionary<string, IndexDefinition>` | Define [Indexes](../../../Indexes/creating-and-deploying#using-maintenance-operations) |
| AutoIndexes | `Dictionary<string, AutoIndexDefinition>` | Define [Auto Indexes](../../../Indexes/creating-and-deploying#using-maintenance-operations) |
| Settings | `Dictionary<string, string>` | Provide [Configuration](../../../server/configuration) settings. |
| Revisions | `RevisionsConfiguration` | Set [Revision](../../../server/extenstions/revisions) configuration. |
| Expiration | `ExpirationConfiguration` | Set [Expiration](../../../server/extenstions/expiration) configuration. |
| RavenConnectionStrings | `Dictionary<string, RavenConnectionString>` | Add [Raven Connection String](to-do)|
| SqlConnectionStrings | `Dictionary<string, SqlConnectionString>` | Add [SQL Connection String](to-do) |
| PeriodicBackups | `List<PeriodicBackupConfiguration>` | Add [Backup](to-do) tasks. |
| ExternalReplications | `List<ExternalReplications>` | Add [External Replication](../../../server/ongoing-tasks/external-replication) tasks. |
| RavenEtls | `List<RavenEtlConfiguration>` | Add [Raven ETL](../../../server/ongoing-tasks/etl/raven) tasks. |
| SqlEtls | `List<SqlEtlConfiguration>` | Add [SQL ETL](../../../server/ongoing-tasks/etl/sql) tasks. |
| Client | `ClientConfiguration` | Set [Client Configuration](to-do) |

#### Remarks
If `Topology` is specified the `replicationFactor` will be ignored.

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync CreateDatabase@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TAB:csharp:Async CreateDatabaseAsync@ClientApi\Operations\Server\CreateDeleteDatabase.cs /}
{CODE-TABS/}

## Related articles
- [Distributed Database](../../../server/clustering/distribution/distributed-database)
- [Create Database via Studio](../../../studio/server/databases/create-new-database/general-flow)