# Glossary : AdminStatistics

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **ServerName** | string | The name of the server |
| **TotalNumberOfRequests** | int | Total number of requests that was made to server |
| **Uptime** | TimeSpan | Server uptime |
| **Memory** | AdminMemoryStatistics | Admin memory statistics (described below) |
| **LoadedDatabases** | IEnumerable&lt;LoadedDatabaseStatistics&gt; | Information about loaded databases (described below) |
| **LoadedFilesystems** | IEnumerable&lt;FileSystemStats&gt; | Information about loaded filesystes (described below) |

<hr />

## AdminMemoryStatistics

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **DatabaseCacheSizeInMB** | decimal | The size of database cache in MB |
| **ManagedMemorySizeInMB** | decimal | The size of managed memory in MB |
| **TotalProcessMemorySizeInMB** | decimal | The total memory size of progess in MB |

<hr />

## LoadedDatabaseStatistics

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Name** | string | Database name |
| **LastActivity** | DateTime | Time of last activity |
| **TransactionalStorageAllocatedSize** | long | Allocated size of transactional store |
| **TransactionalStorageAllocatedSizeHumaneSize** | string | Allocated size of transactional store (formatted) |
| **TransactionalStorageUsedSize** | long | Used size of transactional store |
| **TransactionalStorageUsedSizeHumaneSize** | string | Used size of transactional store (formatted) |
| **IndexStorageSize** | long | The size of index storage |
| **IndexStorageHumaneSize** | string | The size of index storage (formatted) |
| **TotalDatabaseSize** | long | Total database size |
| **TotalDatabaseHumaneSize** | string | Total database size (formatted) |
| **CountOfDocuments** | long | The amount of documents in database |
| **CountOfAttachments** | long | The amount of attachments in database |
| **DatabaseTransactionVersionSizeInMB** | decimal | The database transaction version size in MB |
| **Metrics** | DatabaseMetrics | Metrics Details |
| **StorageStats** | StorageStats | Storage Details |

<hr />		 

## FileSystemStats

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Name** | string | Filesystem name |
| **FileCount**| long | The amount of files |
| **Metrics** | FileSystemMetrics | Metrics Details |
| **ActiveSyncs** | IList&lt;SynchronizationDetails&gt; | Information about active syncs |
| **PendingSyncs** | IList&lt;SynchronizationDetails&gt; | Information about pending syncs |