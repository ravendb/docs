# Migration: Configuration options

In RavenDB 4.0 the configuration settings are defined in `settings.json`. The file is created when running the server for the first time from the `settings.default.json` file which is located next to the server binaries and its default content is as follows:

{CODE-BLOCK:json}
{
    "ServerUrl": "http://127.0.0.1:0",
    "Setup.Mode": "Initial",
    "DataDir": "RavenData"
}
{CODE-BLOCK/}

{INFO: Setup Wizard}
The default `Setup.Mode: Initial` option will cause that the first run of RavenDB 4.0 server will launch the [Setup Wizard](../../start/installation/setup-wizard) and guide you through authentication and cluster setup.
{INFO/}

Below there are listings of 4.0 equivalents for 3.x settings. If there isn't an equivalent for 3.x setting it means the option isn't applicable in 4.0.

## Core settings

| 3.x | 4.0 |
|:---------------------|:---|
| Raven/DataDir | DataDir|
| Raven/Port | Defined using ServerUrl |
| Raven/HostName | Defined using ServerUrl and PublicServerUrl |
| Raven/RunInMemory | RunInMemory |

## Cluster settings

| 3.x | 4.0 |
|:---------------------|:---|
| Raven/Cluster/ElectionTimeout (default: 1200 ms) | Cluster.ElectionTimeoutInMs (default: 300 ms) |

## Data settings

| 3.x | 4.0 |
|:---------------------|:---|
| Raven/TransactionJournalsPath | See [Storing Data in Custom Locations](../../server/storage/directory-structure#storing-data-in-custom-locations) |
| Raven/Esent/LogsPath | See [Storing Data in Custom Locations](../../server/storage/directory-structure#storing-data-in-custom-locations) |

## Database settings

| 3.x | 4.0 |
|:---------------------|:---|
| Raven/DatabaseOperationTimeout (default: 5 min) | Databases.QueryTimeoutInSec (default: 300 sec) <br /> Databases.QueryOperationTimeoutInSec (default: 300 sec) <br /> Databases.OperationTimeoutInSec (default: 300 sec) <br /> Databases.CollectionOperationTimeoutInSec (default: 300 sec) |
| Raven/Tenants/ConcurrentResourceLoadTimeout (default: 15 sec) | Databases.ConcurrentLoadTimeoutInSec (default: 10 sec) |
| Raven/Tenants/MaxConcurrentResourceLoads (default: 8) | Databases.MaxConcurrentLoads (default: 8) |
| Raven/Tenants/MaxIdleTimeForTenantDatabase (default: 900 sec) | Databases.MaxIdleTimeInSec (default: 900 sec) |
| Raven/Tenants/FrequencyToCheckForIdleDatabases (default: 60 sec) | Databases.FrequencyToCheckForIdleInSec (default: 60 sec) |

## ETL settings

| 3.x | 4.0 |
|:---------------------|:---|
| Raven/SqlReplication/CommandTimeoutInSec (default: default SQL provider timeout) | ETL.SQL.CommandTimeoutInSec (default: default SQL provider timeout) |

## HTTP settings

| 3.x | 4.0 |
|:---------------------|:---|
| Raven/HttpCompression (default: true) | Http.UseResponseCompression (default: true) <br /> Http.AllowResponseCompressionOverHttps (default: false) |

## Indexing settings

| 3.x | 4.0 |
|:---------------------|:---|
| Raven/IndexingDisabled  (default: false) | Indexing.Disable (default: false) |
| Raven/TimeToWaitBeforeMarkingAutoIndexAsIdle (default: 1 hour) | Indexing.TimeToWaitBeforeMarkingAutoIndexAsIdleInMin (default: 30 min) |
| Raven/CreateAutoIndexesForAdHocQueriesIfNeeded (default: true) | Indexing.DisableQueryOptimizerGeneratedIndexes (default: false) |
| Raven/IndexStoragePath | See [Storing Data in Custom Locations](../../server/storage/directory-structure#storing-data-in-custom-locations) |

## License settings

| 3.x | 4.0 |
|:---------------------|:---|
| Raven/License | License |
| Raven/LicensePath | License.Path |

## Logs settings

Since version 4.0 the logging is configured via `settings.json` file only. The following options are available:

- Logs.Path (default: Logs)
- Logs.Mode (default: Operations) - available values: None, Operations (high level info), Information (low level debug info)


## Monitoring settings

| 3.x | 4.0 |
|:---------------------|:---|
| Raven/Monitoring/Snmp/Enabled (default: false)| Monitoring.Snmp.Enabled (default: false) |
| Raven/Monitoring/Snmp/Port (default: 161) | Monitoring.Snmp.Port (default: 161) |
| Raven/Monitoring/Snmp/Community (default: ravendb) | Monitoring.Snmp.Community (default: ravendb) |

## Patching settings

| 3.x | 4.0 |
|:---------------------|:---|
| Raven/MaxStepsForScript (default: 10000) | Patching.MaxStepsForScript (default: 10000) |

## Query settings

| 3.x | 4.0 |
|:---------------------|:---|
| Raven/MaxClauseCount (default: 1024) | Query.MaxClauseCount (default: 1024) |

## Replication settings

| 3.x | 4.0 |
|:---------------------|:---|
| Raven/Replication/MaxNumberOfItemsToReceiveInSingleBatch (default: 512) | Replication.MaxItemsCount (default: 16384) <br />  Replication.MaxSizeToSendInMb (default: 64) |

## Server settings

| 3.x | 4.0 |
|:---------------------|:---|
| Raven/ServerName | Server.Name |
| Raven/MaxSecondsForTaskToWaitForDatabaseToLoad (default: 5 sec) | Server.MaxTimeForTaskToWaitForDatabaseToLoadInSec (default: 30 sec) |

## Storage settings

Since version 4.0 the only storage engine is Voron. _Raven/Esent/*_ settings are not applicable.

| 3.x | 4.0 |
|:---------------------|:---|
| Raven/Voron/TempPath | Storage.TempPath |
| Raven/Voron/AllowOn32Bits (default: false) | Storage.ForceUsing32BitsPager (default: false) - in 4.0 Voron is supported on 32 bits |
