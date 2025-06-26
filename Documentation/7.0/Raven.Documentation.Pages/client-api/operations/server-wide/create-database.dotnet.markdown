# Create Database Operation
---

{NOTE: }

* Use `CreateDatabaseOperation` to create a new database from the **Client API**, as described below.  
  To create a new database from the **Studio**, see [Create database](../../../studio/database/create-new-database/general-flow).

* This operation requires a client certificate with a security clearance of _Operator_ or _ClusterAdmin_.  
  To learn which operations are allowed at each level, see [Security clearance and permissions](../../../server/security/authorization/security-clearance-and-permissions).

* In this article:
   * [Create new database](../../../client-api/operations/server-wide/create-database#create-new-database)
     * [Example I - Create non-sharded database](../../../client-api/operations/server-wide/create-database#example-i---create-non-sharded-database)
     * [Example II - Create sharded database](../../../client-api/operations/server-wide/create-database#example-ii---create-sharded-database)
     * [Example III - Ensure database does not exist before creating](../../../client-api/operations/server-wide/create-database#example-iii---ensure-database-does-not-exist-before-creating)
   * [Syntax](../../../client-api/operations/server-wide/create-database#syntax)

{NOTE/}

---

{PANEL: Create new database}

{CONTENT-FRAME: }

##### Example I - Create non-sharded database
---

* The following simple example creates a non-sharded database with the default replication factor of 1.

{CODE-TABS}
{CODE-TAB:csharp:Create_db_sync create_database_1@ClientApi\Operations\Server\CreateDatabase.cs /}
{CODE-TAB:csharp:Create_db_async create_database_1_async@ClientApi\Operations\Server\CreateDatabase.cs /}
{CODE-TAB:csharp:Create_using_builder_sync create_using_builder_1@ClientApi\Operations\Server\CreateDatabase.cs /}
{CODE-TAB:csharp:Create_using_builder_async create_using_builder_1_async@ClientApi\Operations\Server\CreateDatabase.cs /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

##### Example II - Create sharded database
---

* The following example creates a sharded database with 3 shards, each with a replication factor of 2.
* In addition, it enables: 
   * revisions
   * document expiration
   * and applies some settings to the database.

{CODE-TABS}
{CODE-TAB:csharp:Create_db_sync create_database_2@ClientApi\Operations\Server\CreateDatabase.cs /}
{CODE-TAB:csharp:Create_db_async create_database_2_async@ClientApi\Operations\Server\CreateDatabase.cs /}
{CODE-TAB:csharp:Create_using_builder_sync create_using_builder_2@ClientApi\Operations\Server\CreateDatabase.cs /}
{CODE-TAB:csharp:Create_using_builder_async create_using_builder_2_async@ClientApi\Operations\Server\CreateDatabase.cs /}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

##### Example III - Ensure database does not exist before creating
---

* To ensure the database does not already exist before creating it, follow this example:

{CODE-TABS}
{CODE-TAB:csharp:Sync create_database_3@ClientApi\Operations\Server\CreateDatabase.cs /}
{CODE-TAB:csharp:Async create_database_3_async@ClientApi\Operations\Server\CreateDatabase.cs /}
{CODE-TABS/}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Syntax}

{CODE:csharp create_database_operation_syntax@ClientApi\Operations\Server\CreateDatabase.cs /}

| Parameter             | Description                                                                                                                                                                                                                                                                                                              |
|-----------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **databaseRecord**    | Instance of `DatabaseRecord` containing database configuration.<br>See [The Database Record](../../../client-api/operations/server-wide/create-database#the-database-record) below.                                                                                                                                      |
| **replicationFactor** | Number of nodes the database should be replicated to.<br><br>If not specified, the value is taken from `databaseRecord.Topology.ReplicationFactor`,<br>or defaults to **`1`** if that is not set.<br><br>If `Topology` is provided, the `replicationFactor` is ignored.                                                  |
| **builder**           | Callback used to initialize and fluently configure a new DatabaseRecord.<br>Receives an `IDatabaseRecordBuilderInitializer` on which you invoke builder methods to construct the record. See [The Database Record Builder](../../../client-api/operations/server-wide/create-database#the-database-record-builder) below. |

---

### The Database Record:

The `DatabaseRecord` is a collection of database configurations:  

| DatabaseRecord constructors           | Description                                                        |
|---------------------------------------|--------------------------------------------------------------------|
| DatabaseRecord()                      | Initialize a new database record.                                  |
| DatabaseRecord(`string` databaseName) | Initialize a new database record with the specified database name. |

{INFO: }

**Note:**  

* Only the properties listed in the table below can be configured in the `DatabaseRecord` object passed to `CreateDatabaseOperation`.
* For example, although ongoing task definitions are public on the _DatabaseRecord_ class, setting them during database creation will result in an exception.
  To define ongoing tasks (e.g., backups, ETL, replication), use the appropriate dedicated operation after the database has been created.

{INFO/}

| DatabaseRecord properties          | Type                                                | Description                                                                                                                                                                                                                                                                 |
|------------------------------------|-----------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **AiConnectionStrings**            | `Dictionary<string, AiConnectionString>`            | Define [Ai Connection Strings](../../../ai-integration/connection-strings/connection-strings-overview), keyed by name.                                                                                                                                                      |
| **Analyzers**                      | `Dictionary<string, AnalyzerDefinition>`            | A dictionary defining the [Custom Analyzers](../../../indexes/using-analyzers#creating-custom-analyzers) available to the database.                                                                                                                                         |
| **AutoIndexes**                    | `Dictionary<string, AutoIndexDefinition>`           | Auto-index definitions for the database.                                                                                                                                                                                                                                    |
| **Client**                         | `ClientConfiguration`                               | [Client behavior](../../../studio/server/client-configuration) configuration.                                                                                                                                                                                               |
| **ConflictSolverConfig**           | `ConflictSolver`                                    | Define the strategy used to resolve [Replication conflicts](../../../server/clustering/replication/replication-conflicts).                                                                                                                                                  |
| **DataArchival**                   | `DataArchivalConfiguration`                         | [Data Archival](../../../data-archival/overview) configuration for the database.                                                                                                                                                                                        |
| **DatabaseName**                   | `string`                                            | The database name.                                                                                                                                                                                                                                                          |
| **Disabled**                       | `bool`                                              | Set the database initial state.<br> `true` - disable the database.<br> `false` - (default) the database will be enabled.<br><br>This can be modified later via [ToggleDatabasesStateOperation](../../../client-api/operations/server-wide/toggle-databases-state).          |
| **DocumentsCompression**           | `DocumentsCompressionConfiguration`                 | Configuration settings for [Compressing documents](../../../server/storage/documents-compression).                                                                                                                                                                          |
| **ElasticSearchConnectionStrings** | `Dictionary<string, ElasticSearchConnectionString>` | Define [ElasticSearch Connection Strings](../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-elasticsearch-connection-string), keyed by name.                                                                                       |
| **Encrypted**                      | `bool`                                              | `true` - create an [Encrypted database](../../../server/security/encryption/database-encryption).<br><br>Note: Use `PutSecretKeyCommand` to send your secret key to the server BEFORE creating the database.<br><br>`false` - (default) the database will not be encrypted. |
| **Expiration**                     | `ExpirationConfiguration`                           | [Expiration](../../../server/extensions/expiration) configuration for the database.                                                                                                                                                                                         |
| **Indexes**                        | `Dictionary<string, IndexDefinition>`               | Define [Indexes](../../../client-api/operations/maintenance/indexes/put-indexes) that will be created with the database - no separate deployment needed.                                                                                                                    |
| **Integrations**                   | `IntegrationConfigurations`                         | Configuration for [Integrations](../../../integrations/postgresql-protocol/overview),<br>e.g. `PostgreSqlConfiguration`.                                                                                                                                                    |
| **LockMode**                       | `DatabaseLockMode`                                  | Set the database lock mode.<br>(default: `Unlock`)<br><br>This can be modified later via `SetDatabasesLockOperation`.                                                                                                                                                       |
| **OlapConnectionStrings**          | `Dictionary<string, OlapConnectionString>`          | Define [OLAP Connection Strings](../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-olap-connection-string), keyed by name.                                                                                                         |
| **QueueConnectionStrings**         | `Dictionary<string, QueueConnectionString>`         | Define [Queue Connection Strings](../../../server/ongoing-tasks/etl/queue-etl/overview), keyed by name.                                                                                                                                                                     |
| **RavenConnectionStrings**         | `Dictionary<string, RavenConnectionString>`         | Define [Raven Connection Strings](../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-ravendb-connection-string), keyed by name.                                                                                                      |
| **Refresh**                        | `RefreshConfiguration`                              | [Refresh](../../../server/extensions/refresh) configuration for the database.                                                                                                                                                                                               |
| **Revisions**                      | `RevisionsConfiguration`                            | [Revisions](../../../document-extensions/revisions/client-api/operations/configure-revisions) configuration for the database.                                                                                                                                               |
| **RevisionsBin**                   | `RevisionsBinConfiguration`                         | Configuration for the [Revisions Bin Cleaner](../../../document-extensions/revisions/revisions-bin-cleaner).                                                                                                                                                                |
| **RevisionsForConflicts**          | `RevisionsCollectionConfiguration`                  | Set the revisions configuration for conflicting documents.                                                                                                                                                                                                                  |
| **RollingIndexes**                 | `Dictionary<string, RollingIndex>`                  | Dictionary mapping index names to their deployment configurations.                                                                                                                                                                                                          |
| **Settings**                       | `Dictionary<string, string>`                        | [Configuration](../../../server/configuration/configuration-options) settings for the database.                                                                                                                                                                             |
| **Sharding**                       | `ShardingConfiguration`                             | The sharding configuration.                                                                                                                                                                                                                                                 |
| **SnowflakeConnectionStrings**     | `Dictionary<string, SnowflakeConnectionString>`     | Define [Snowflake Connection Strings](../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-snowflake-connection-string), keyed by name.                                                                                                |
| **Sorters**                        | `Dictionary<string, SorterDefinition>`              | A dictionary defining the [Custom Sorters](../../../studio/database/settings/custom-sorters) available to the database.                                                                                                                                                     |
| **SqlConnectionStrings**           | `Dictionary<string, SqlConnectionString>`           | Define [SQL Connection Strings](../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-sql-connection-string), keyed by name.                                                                                                           |
| **Studio**                         | `StudioConfiguration`                               | [Studio Configuration](../../../studio/database/settings/studio-configuration).                                                                                                                                                                                             |
| **TimeSeries**                     | `TimeSeriesConfiguration`                           | [Time series](../../../studio/database/settings/time-series-settings) configuration for the database.                                                                                                                                                                       |
| **Topology**                       | `DatabaseTopology`                                  | Optional topology configuration.<br><br>Defaults to `null`, in which case the server will determine which nodes to place the database on, based on the specified `ReplicationFactor`.                                                                                       |
| **UnusedDatabaseIds**              | `HashSet<string>`                                   | Set database IDs that will be excluded when creating new change vectors.                                                                                                                                                                                                    |

---

### The Database Record Builder:

{CODE:csharp builder_syntax@ClientApi\Operations\Server\CreateDatabase.cs /}

{PANEL/}

## Related Articles

- [Distributed Database](../../../server/clustering/distribution/distributed-database)
- [Create Database via Studio](../../../studio/database/create-new-database/general-flow)
