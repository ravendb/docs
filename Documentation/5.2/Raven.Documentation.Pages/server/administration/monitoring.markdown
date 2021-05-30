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

The monitoring endpoints output data in JSON format. There are four endpoints: 
* `<your server URL>/admin/monitoring/v1/server`
* `<your server URL>/admin/monitoring/v1/databases`
* `<your server URL>/admin/monitoring/v1/indexes`
* `<your server URL>/admin/monitoring/v1/collections`

The following is a list of JSON fields returned by the endpoints:  

| Endpoint suffix | Field Name | Description |
| - | - | - |
| `server` | `ServerUrls` | Server URL |
| `server` | `ServerPublicUrl` | Server Public URL |
| `server` | `TcpServerUrls` | Server TCP URL |
| `server` | `PublicTcpServerUrls` | Server Public TCP URL |
| `server` | `ServerVersion` | Server version |
| `server` | `ServerFullVersion` | Server full version |
| `server` | `UpTimeInSec` | Server up-time |
| `server` | `ServerProcessId` | Server process ID |
| `server` | `ProcessUsage` | Process CPU usage in % |
| `server` | `MachineUsage` | Machine CPU usage in % |
| `server` | `MachineIoWait` | IO wait in % |
| `server` | `PhysicalMemoryInMb` | PhysicalMemory |
| `server` | `InstalledMemoryInMb` | InstalledMemory |
| `server` | `AllocatedMemoryInMb` | Server allocated memory in MB |
| `server` | `LowMemoryServerity` | Server low memory flag value |
| `server` | `TotalSwapSizeInMb` | Server total swap size in MB |
| `server` | `TotalSwapUsageInMb` | Server total swap usage in MB |
| `server` | `WorkingSetSwapUsageInMb` | Server working set swap usage in MB |
| `server` | `TotalDirtyInMb` | Dirty Memory that is used by the scratch buffers in MB |
| `server` | `ConcurrentRequestsCount` | Number of concurrent requests |
| `server` | `TotalRequests` | Total number of requests since server startup |
| `server` | `RequestsPerSec` | Number of requests per second (one minute rate) |
| `server` | `LastRequestTimeInSec` | Server last request time |
| `server` | `LastAuthorizedNonClusterAdminRequestTimeInSec` | Server last authorized non cluster admin request time |
| `server` | `Type` | Server license type |
| `server` | `Expiration` | Server license expiration date |
| `server` | `ExpirationLeftInSec` | Server license expiration left |
| `server` | `UtilizedCpuCores` | Server license utilized CPU cores |
| `server` | `MaxCores` | Server license max CPU cores |
| `server` | `SystemStoreUsedDataFileSizeInMb` | Server storage used size in MB |
| `server` | `SystemStoreTotalDataFileSizeInMb` | Server storage total size in MB |
| `server` | `TotalFreeSpaceInMb` | Remaining server storage disk space in MB |
| `server` | `RemainingStorageSpacePercentage` | Remaining server storage disk space in % |
| `server` | `` | Server certificate expiration date |
| `server` | `ServerCertificateExpirationLeftInSec` | Server certificate expiration left |
| `server` | `WellKnownAdminCertificates` | List of well known admin certificate thumbprints |
| `server` | `ProcessorCount` | Number of processor on the machine |
| `server` | `AssignedProcessorCount` | Number of assigned processors on the machine |
| `server` | `CurrentNumberOfRunningBackups` | Number of backups currently running |
| `server` | `MaxNumberOfConcurrentBackups` | Max number of backups that can run concurrently |
| `server` | `ThreadPoolAvailableWorkerThreads` | Number of available worker threads in the thread pool |
| `server` | `ThreadPoolAvailableCompletionPortThreads` | Number of available completion port threads in the thread pool |
| `server` | `TcpActiveConnections` | Number of active TCP connections |
| `server` | `NodeTag` | Current node tag |
| `server` | `NodeState` | Current node state |
| `server` | `CurrentTerm` | Cluster term |
| `server` | `Index` | Cluster index |
| `server` | `Id` | Cluster ID |
| `server` | `TotalCount` | Number of all databases |
| `server` | `LoadedCount` | Number of loaded databases |
| `server` | `` | ~Time since oldest backup~ |
| `databases` | `Name` | Database name |
| `databases` | `Count` | Number of indexes |
| `databases` | `StaleCount` | Number of stale indexes |
| `databases` | `Documents` | Number of documents |
| `databases` | `Revisions` | Number of revision documents |
| `databases` | `Attachments` | Number of attachments |
| `databases` | `UniqueAttachments` | Number of unique attachments |
| `databases` | `Alerts` | Number of alerts |
| `databases` | `DatabaseId` | Database ID |
| `databases` | `UptimeInSec` | Database up-time |
| `databases` | `Rehabs` | Number of rehabs |
| `databases` | `PerformanceHints` | Number of performance hints |
| `databases` | `ErrorsCount` | Number of indexing errors |
| `databases` | `DocumentsAllocatedDataFileInMb` | Documents storage allocated size in MB |
| `databases` | `DocumentsUsedDataFileInMb` | Documents storage used size in MB |
| `databases` | `IndexesAllocatedDataFileInMb` | Index storage allocated size in MB |
| `databases` | `IndexesUsedDataFileInMb` | Index storage used size in MB |
| `databases` | `TotalAllocatedStorageFileInMb` | Total storage size in MB |
| `databases` | `TotalFreeSpaceInMb` | Remaining storage disk space in MB |
| `databases` | `DocPutsPerSec` | Number of document puts per second (one minute rate) |
| `databases` | `MapIndexIndexesPerSec` | Number of indexed documents per second for map indexes (one minute rate) |
| `databases` | `MapReduceIndexMappedPerSec` | Number of maps per second for map-reduce indexes (one minute rate) |
| `databases` | `MapReduceIndexReducedPerSec` | Number of reduces per second for map-reduce indexes (one minute rate) |
| `databases` | `RequestsPerSec` | Number of requests per second (one minute rate) |
| `databases` | `RequestsCount` | Number of requests from database start |
| `databases` | `RequestAverageDurationInMs` | Average request time in milliseconds |
| `databases` | `` | ~Number of indexes~ (duplicate) |
| `databases` | `StaticCount` | Number of static indexes |
| `databases` | `AutoCount` | Number of auto indexes |
| `databases` | `IdleCount` | Number of idle indexes |
| `databases` | `DisabledCount` | Number of disabled indexes |
| `databases` | `ErroredCount` | Number of error indexes |
| `indexes` | `IndexName` | Index name |
| `indexes` | `Priority` | Index priority |
| `indexes` | `State` | Index state |
| `indexes` | `Errors` | Number of index errors |
| `indexes` | `` | Last query time |
| `indexes` | `TimeSinceLastQueryInSec` | Index indexing time |
| `indexes` | `` | Time since last query |
| `indexes` | `TimeSinceLastIndexingInSec` | Time since last indexing |
| `indexes` | `LockMode` | Index lock mode |
| `indexes` | `IsInvalid` | Indicates if index is invalid |
| `indexes` | `Status` | Index status |
| `indexes` | `MappedPerSec` | Number of maps per second (one minute rate) |
| `indexes` | `ReducedPerSec` | Number of reduces per second (one minute rate) |
| `indexes` | `Type` | Index type |
| `indexes` | `Lagtime` | Indexing Lag Time |
| `indexes` | `EntriesCount` | Number of entries in the index |
| `databases` | `TimeSinceLastBackupInSec` | LastBackup |

{PANEL/}
