# Create Database Operation
---

{NOTE: }

* Use `CreateDatabaseOperation` to create a new database from the **Client API**, as described below.  
  To create a new database from the **Studio**, see [Create database](../../../studio/database/create-new-database/general-flow).

* This operation requires a client certificate with a security clearance of _Operator_ or _ClusterAdmin_.  
  To learn which operations are allowed at each level, see [Security clearance and permissions](../../../server/security/authorization/security-clearance-and-permissions).

* In this article:
    * [Create new database](../../../client-api/operations/server-wide/create-database#create-new-database)
        * [Example I - Create database](../../../client-api/operations/server-wide/create-database#example-i---create-non-sharded-database)
        * [Example II - Ensure database does not exist before creating](../../../client-api/operations/server-wide/create-database#example-ii---ensure-database-does-not-exist-before-creating)
    * [Syntax](../../../client-api/operations/server-wide/create-database#syntax)

{NOTE/}

---

{PANEL: Create new database}

{CONTENT-FRAME: }

##### Example I - Create database
---

* The following simple example creates a non-sharded database with the default replication factor of 1.

{CODE:java CreateDatabase@ClientApi\Operations\Server\CreateDeleteDatabase.java /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

##### Example II - Ensure database does not exist before creating
---

* To ensure the database does not already exist before creating it, follow this example:

{CODE:java EnsureDatabaseExists@ClientApi\Operations\Server\CreateDeleteDatabase.java /}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Syntax}

{CODE:java create_database_syntax@ClientApi\Operations\Server\CreateDeleteDatabase.java /}

| Parameter             | Type           | Description                                                                                                                                                                                                                                              |
|-----------------------|----------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **databaseRecord**    | DatabaseRecord | Instance of `DatabaseRecord` containing database configuration.                                                                                                                                                                                          |
| **replicationFactor** | int            | Number of nodes the database should be replicated to.<br><br>If not specified, the value is taken from `topology.replicationFactor`,<br>or defaults to **`1`** if that is not set.<br><br>If `topology` is provided, the `replicationFactor` is ignored. |

## DatabaseRecord

`DatabaseRecord` is a collection of database configurations.  

| constructor                           | Description                      |
|---------------------------------------|----------------------------------|
| DatabaseRecord(`string` databaseName) | Initialize a new database record |

{INFO: }

**Note:**

* Only the properties listed in the table below can be configured in the `DatabaseRecord` object passed to `CreateDatabaseOperation`.
* For example, although ongoing task definitions are public on the _DatabaseRecord_ class, setting them during database creation will result in an exception.
  To define ongoing tasks (e.g., backups, ETL, replication), use the appropriate dedicated operation after the database has been created.

{INFO/}

| Property                           | Type                                         | Description                                                                                                                                                                                                                                                                 |
|------------------------------------|----------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **analyzers**                      | `Map<String, AnalyzerDefinition>`            | A dictionary defining the [Custom analyzers](../../../indexes/using-analyzers#creating-custom-analyzers) available to the database.                                                                                                                                         |
| **autoIndexes**                    | `Map<String, AutoIndexDefinition>`           | Auto-index definitions for the database.                                                                                                                                                                                                                                    |
| **client**                         | `ClientConfiguration`                        | [Client behavior](../../../studio/server/client-configuration) configuration.                                                                                                                                                                                               |
| **conflictSolverConfig**           | `ConflictSolver`                             | Define the strategy used to resolve [Replication conflicts](../../../server/clustering/replication/replication-conflicts).                                                                                                                                                  |
| **dataArchival**                   | `DataArchivalConfiguration`                  | [Data archival](../../../server/extensions/archival) configuration for the database.                                                                                                                                                                                        |
| **databaseName**                   | `String`                                     | The database name.                                                                                                                                                                                                                                                          |
| **disabled**                       | `boolean` (default: false)                   | Set the database initial state.<br> `true` - disable the database.<br> `false` - (default) the database will be enabled.<br><br>This can be modified later via [ToggleDatabasesStateOperation](../../../client-api/operations/server-wide/toggle-databases-state).          |
| **documentsCompression**           | `DocumentsCompressionConfiguration`          | Configuration settings for [Compressing documents](../../../server/storage/documents-compression).                                                                                                                                                                          |
| **elasticSearchConnectionStrings** | `Map<String, ElasticSearchConnectionString>` | Define [ElasticSearch Connection String](../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-elasticsearch-connection-string), keyed by name.                                                                                        |
| **encrypted**                      | `boolean` (default: false)                   | `true` - create an [encrypted database](../../../server/security/encryption/database-encryption).<br><br>Note: Use `PutSecretKeyCommand` to send your secret key to the server BEFORE creating the database.<br><br>`false` - (default) the database will not be encrypted. |
| **expiration**                     | `ExpirationConfiguration`                    | [Expiration](../../../server/extensions/expiration) configuration for the database.                                                                                                                                                                                         |
| **indexes**                        | `Map<String, IndexDefinition>`               | Define [Indexes](../../../client-api/operations/maintenance/indexes/put-indexes) that will be created with the database - <br>no separate deployment needed.                                                                                                                |
| **integrations**                   | `IntegrationConfigurations`                  | Configuration for [Integrations](../../../integrations/postgresql-protocol/overview),<br>e.g. `PostgreSqlConfiguration`.                                                                                                                                                    |
| **lockMode**                       | `DatabaseLockMode`                           | Set the database lock mode.<br>(default: `Unlock`)<br><br>This can be modified later via `SetDatabasesLockOperation`.                                                                                                                                                       |
| **olapConnectionStrings**          | `Map<String, OlapConnectionString>`          | Define [OLAP Connection String](../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-olap-connection-string), keyed by name.                                                                                                          |
| **queueConnectionStrings**         | `Map<String, QueueConnectionString>`         | Define [Queue Connection String](../../../server/ongoing-tasks/etl/queue-etl/overview), keyed by name.                                                                                                                                                                      |
| **ravenConnectionStrings**         | `Map<String, RavenConnectionString>`         | Define [Raven Connection String](../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-a-ravendb-connection-string), keyed by name.                                                                                                       |
| **refresh**                        | `RefreshConfiguration`                       | [Refresh](../../../server/extensions/refresh) configuration for the database.                                                                                                                                                                                               |
| **revisions**                      | `RevisionsConfiguration`                     | [Revisions](../../../document-extensions/revisions/client-api/operations/configure-revisions) configuration for the database.                                                                                                                                               |
| **revisionsForConflicts**          | `RevisionsCollectionConfiguration`           | Set the revisions configuration for conflicting documents.                                                                                                                                                                                                                  |
| **rollingIndexes**                 | `Map<String, RollingIndex>`                  | Dictionary mapping index names to their deployment configurations.                                                                                                                                                                                                          |
| **settings**                       | `Map<String, String>`                        | [Configuration](../../../server/configuration/configuration-options) settings for the database.                                                                                                                                                                             |
| **sharding**                       | `ShardingConfiguration`                      | The sharding configuration.                                                                                                                                                                                                                                                 |
| **sorters**                        | `Map<String, SorterDefinition>`              | A dictionary defining the [Custom sorters](../../../studio/database/settings/custom-sorters) available to the database.                                                                                                                                                     |
| **sqlConnectionStrings**           | `Map<String, SqlConnectionString>`           | Define [SQL Connection String](../../../client-api/operations/maintenance/connection-strings/add-connection-string#add-an-sql-connection-string), keyed by name.                                                                                                            |
| **studio**                         | `StudioConfiguration`                        | [Studio Configuration](../../../studio/database/settings/studio-configuration).                                                                                                                                                                                             |
| **timeSeries**                     | `TimeSeriesConfiguration`                    | [Time series](../../../studio/database/settings/time-series-settings) configuration for the database.                                                                                                                                                                       |
| **topology**                       | `DatabaseTopology`                           | Optional topology configuration.<br><br>Defaults to `null`, in which case the server will determine which nodes to place the database on, based on the specified `ReplicationFactor`.                                                                                       |
| **unusedDatabaseIds**              | `Set<String>`                                | Set database IDs that will be excluded when creating new change vectors.                                                                                                                                                                                                    |

{PANEL/}

## Related Articles

- [Distributed Database](../../../server/clustering/distribution/distributed-database)
- [Create Database via Studio](../../../studio/database/create-new-database/general-flow)
