# Monitoring and the RavenDB Telegraf Plugin

---

{NOTE: }

* The endpoints listed in this page provide a wide variety of performance metrics for a RavenDB 
instance - things like indexing, communication inside a cluster, or the server's memory usage.  

* These metrics can be collected with the [RavenDB Telegraf Plugin](https://docs.influxdata.com/telegraf/v1.18/plugins/#ravendb) 
and displayed as live graphs with [Grafana](https://grafana.com/).  \

{NOTE/}

---

{PANEL: }

[Telegraf](https://www.influxdata.com/time-series-platform/telegraf/) is a popular data collection 
and processing agent designed to work with time series data. Version 1.18 of Telegraf has a new 
plugin for RavenDB that collects data from RavenDB's monitoring endpoints. The recommended use 
for the RavenDB plugin is to have Telegraf output to [InfluxDB](https://www.influxdata.com/products/influxdb/), 
and from there the data can be queried by [Grafana](https://grafana.com/) and displayed on your own 
data tracking dashboard. But this feature is flexible - Telegraf can output data to other destinations. 

The monitoring endpoints output data in JSON format. There are three endpoints: 
* `<your server URL>/admin/monitoring/v1/server`
* `<your server URL>/admin/monitoring/v1/databases`
* `<your server URL>/admin/monitoring/v1/indexes`

The root OID for these endpoints is: `1.3.6.1.4.1.45751.1.1`.  

The following is a list of JSON fields returned by the endpoints:  

| Endpoint suffix | OID suffix | Field Name | Description |
| - | - | - | - |
| `server` | `1.1.1` | `ServerUrls` | Server URL |
| `server` | `1.1.2` | `ServerPublicUrl` | Server Public URL |
| `server` | `1.1.3` | `TcpServerUrls` | Server TCP URL |
| `server` | `1.1.4` | `PublicTcpServerUrls` | Server Public TCP URL |
| `server` | `1.2.1` | `ServerVersion` | Server version |
| `server` | `1.2.2` | `ServerFullVersion` | Server full version |
| `server` | `1.3` | `UpTimeInSec` | Server up-time |
| `server` | `1.4` | `ServerProcessId` | Server process ID |
| `server` | `1.5.1` | `ProcessUsage` | Process CPU usage in % |
| `server` | `1.5.2` | `MachineUsage` | Machine CPU usage in % |
| `server` | `1.5.3.1` | `` | ~CPU Credits Base~ |
| `server` | `1.5.3.2` | `` | ~CPU Credits Max~ |
| `server` | `1.5.3.3` | `` | ~CPU Credits Remaining~ |
| `server` | `1.5.3.4` | `` | ~CPU Credits Gained Per Second~ |
| `server` | `1.5.3.5` | `` | ~CPU Credits Background Tasks Alert Raised~ |
| `server` | `1.5.3.6` | `` | ~CPU Credits Failover Alert Raised~ |
| `server` | `1.5.3.7` | `` | ~CPU Credits Any Alert Raised~ |
| `server` | `1.5.4` | `MachineIoWait` | IO wait in % |
| `server` | `` | `PhysicalMemoryInMb` | PhysicalMemory |
| `server` | `` | `InstalledMemoryInMb` | InstalledMemory |
| `server` | `1.6.1` | `AllocatedMemoryInMb` | Server allocated memory in MB |
| `server` | `1.6.2` | `LowMemoryServerity` | Server low memory flag value |
| `server` | `1.6.3` | `TotalSwapSizeInMb` | Server total swap size in MB |
| `server` | `1.6.4` | `TotalSwapUsageInMb` | Server total swap usage in MB |
| `server` | `1.6.5` | `WorkingSetSwapUsageInMb` | Server working set swap usage in MB |
| `server` | `1.6.6` | `TotalDirtyInMb` | Dirty Memory that is used by the scratch buffers in MB |
| `server` | `1.7.1` | `ConcurrentRequestsCount` | Number of concurrent requests |
| `server` | `1.7.2` | `TotalRequests` | Total number of requests since server startup |
| `server` | `1.7.3` | `RequestsPerSec` | Number of requests per second (one minute rate) |
| `server` | `1.8` | `LastRequestTimeInSec` | Server last request time |
| `server` | `1.8.1` | `LastAuthorizedNonClusterAdminRequestTimeInSec` | Server last authorized non cluster admin request time |
| `server` | `1.9.1` | `Type` | Server license type |
| `server` | `1.9.2` | `Expiration` | Server license expiration date |
| `server` | `1.9.3` | `ExpirationLeftInSec` | Server license expiration left |
| `server` | `1.9.4` | `UtilizedCpuCores` | Server license utilized CPU cores |
| `server` | `1.9.5` | `MaxCores` | Server license max CPU cores |
| `server` | `1.10.1` | `SystemStoreUsedDataFileSizeInMb` | Server storage used size in MB |
| `server` | `1.10.2` | `SystemStoreTotalDataFileSizeInMb` | Server storage total size in MB |
| `server` | `1.10.3` | `TotalFreeSpaceInMb` | Remaining server storage disk space in MB |
| `server` | `1.10.4` | `RemainingStorageSpacePercentage` | Remaining server storage disk space in % |
| `server` | `1.11.1` | `` | Server certificate expiration date |
| `server` | `1.11.2` | `ServerCertificateExpirationLeftInSec` | Server certificate expiration left |
| `server` | `1.11.3` | `WellKnownAdminCertificates` | List of well known admin certificate thumbprints |
| `server` | `1.12.1` | `ProcessorCount` | Number of processor on the machine |
| `server` | `1.12.2` | `AssignedProcessorCount` | Number of assigned processors on the machine |
| `server` | `1.13.1` | `CurrentNumberOfRunningBackups` | Number of backups currently running |
| `server` | `1.13.2` | `MaxNumberOfConcurrentBackups` | Max number of backups that can run concurrently |
| `server` | `1.14.1` | `ThreadPoolAvailableWorkerThreads` | Number of available worker threads in the thread pool |
| `server` | `1.14.2` | `ThreadPoolAvailableCompletionPortThreads` | Number of available completion port threads in the thread pool |
| `server` | `1.15.1` | `TcpActiveConnections` | Number of active TCP connections |
| `server` | `3.1.1` | `NodeTag` | Current node tag |
| `server` | `3.1.2` | `NodeState` | Current node state |
| `server` | `3.2.1` | `CurrentTerm` | Cluster term |
| `server` | `3.2.2` | `Index` | Cluster index |
| `server` | `3.2.3` | `Id` | Cluster ID |
| `server` | `5.1.1` | `TotalCount` | Number of all databases |
| `server` | `5.1.2` | `LoadedCount` | Number of loaded databases |
| `server` | `5.1.3` | `` | ~Time since oldest backup~ |
| `databases` | `5.2.X1.1` | `Name` | Database name |
| `databases` | `5.2.X1.2` | `Count` | Number of indexes |
| `databases` | `5.2.X1.3` | `StaleCount` | Number of stale indexes |
| `databases` | `5.2.X1.4` | `Documents` | Number of documents |
| `databases` | `5.2.X1.5` | `Revisions` | Number of revision documents |
| `databases` | `5.2.X1.6` | `Attachments` | Number of attachments |
| `databases` | `5.2.X1.7` | `UniqueAttachments` | Number of unique attachments |
| `databases` | `5.2.X1.10` | `Alerts` | Number of alerts |
| `databases` | `5.2.X1.11` | `DatabaseId` | Database ID |
| `databases` | `5.2.X1.12` | `UptimeInSec` | Database up-time |
| `databases` | `5.2.X1.13` | `` | ~Indicates if database is loaded~ |
| `databases` | `5.2.X1.14` | `Rehabs` | Number of rehabs |
| `databases` | `5.2.X1.15` | `PerformanceHints` | Number of performance hints |
| `databases` | `5.2.X1.16` | `ErrorsCount` | Number of indexing errors |
| `databases` | `5.2.X2.1` | `DocumentsAllocatedDataFileInMb` | Documents storage allocated size in MB |
| `databases` | `5.2.X2.2` | `DocumentsUsedDataFileInMb` | Documents storage used size in MB |
| `databases` | `5.2.X2.3` | `IndexesAllocatedDataFileInMb` | Index storage allocated size in MB |
| `databases` | `5.2.X2.4` | `IndexesUsedDataFileInMb` | Index storage used size in MB |
| `databases` | `5.2.X2.5` | `TotalAllocatedStorageFileInMb` | Total storage size in MB |
| `databases` | `5.2.X2.6` | `TotalFreeSpaceInMb` | Remaining storage disk space in MB |
| `databases` | `5.2.X3.1` | `DocPutsPerSec` | Number of document puts per second (one minute rate) |
| `databases` | `5.2.X3.2` | `MapIndexIndexesPerSec` | Number of indexed documents per second for map indexes (one minute rate) |
| `databases` | `5.2.X3.3` | `MapReduceIndexMappedPerSec` | Number of maps per second for map-reduce indexes (one minute rate) |
| `databases` | `5.2.X3.4` | `MapReduceIndexReducedPerSec` | Number of reduces per second for map-reduce indexes (one minute rate) |
| `databases` | `5.2.X3.5` | `RequestsPerSec` | Number of requests per second (one minute rate) |
| `databases` | `5.2.X3.6` | `RequestsCount` | Number of requests from database start |
| `databases` | `5.2.X3.7` | `RequestAverageDurationInMs` | Average request time in milliseconds |
| `databases` | `5.2.X5.1` | `` | ~Number of indexes~ (duplicate) |
| `databases` | `5.2.X5.2` | `StaticCount` | Number of static indexes |
| `databases` | `5.2.X5.3` | `AutoCount` | Number of auto indexes |
| `databases` | `5.2.X5.4` | `IdleCount` | Number of idle indexes |
| `databases` | `5.2.X5.5` | `DisabledCount` | Number of disabled indexes |
| `databases` | `5.2.X5.6` | `ErroredCount` | Number of error indexes |
| `databases` | `5.2.X4.Y.1` | `` | ~Indicates if index exists~ |
| `indexes` | `5.2.X4.Y.2` | `IndexName` | Index name |
| `indexes` | `5.2.X4.Y.4` | `Priority` | Index priority |
| `indexes` | `5.2.X4.Y.5` | `State` | Index state |
| `indexes` | `5.2.X4.Y.6` | `Errors` | Number of index errors |
| `indexes` | `5.2.X4.Y.7` | `` | Last query time |
| `indexes` | `5.2.X4.Y.8` | `TimeSinceLastQueryInSec` | Index indexing time |
| `indexes` | `5.2.X4.Y.9` | `` | Time since last query |
| `indexes` | `5.2.X4.Y.10` | `TimeSinceLastIndexingInSec` | Time since last indexing |
| `indexes` | `5.2.X4.Y.11` | `LockMode` | Index lock mode |
| `indexes` | `5.2.X4.Y.12` | `IsInvalid` | Indicates if index is invalid |
| `indexes` | `5.2.X4.Y.13` | `Status` | Index status |
| `indexes` | `5.2.X4.Y.14` | `MappedPerSec` | Number of maps per second (one minute rate) |
| `indexes` | `5.2.X4.Y.15` | `ReducedPerSec` | Number of reduces per second (one minute rate) |
| `indexes` | `5.2.X4.Y.16` | `Type` | Index type |
| `indexes` | N/A | `Lagtime` | Indexing Lag Time |
| `indexes` | `` | `EntriesCount` | Number of entries in the index |
| `databases` | `` | `TimeSinceLastBackupInSec` | LastBackup |

{PANEL/}
