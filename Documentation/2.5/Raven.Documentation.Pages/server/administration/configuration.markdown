# Configuration options

All the configuration options detailed below are defined in the <appSettings> section of your config file as separate values. When running RavenDB as a website (through IIS, or in Embedded mode), the config file is web.config; otherwise it is the Raven.Server.exe.config file.

Changes to the config file or additions / removal from the Plugins directory will not be picked up automatically by the RavenDB service. For your changes to be recognized you will need to restart the service. You can do so calling: <code>Raven.Server.exe /restart</code>.

If you are running in Embedded mode, or RavenDB is running as an IIS application, touching the web.config file will cause IIS to automatically restart RavenDB.

## Sample configurations file

This is the standard app.config XML file. The `appSettings` section is where the global configuration options go, also for web applications which have a web.config file instead.

{CODE-START:xml /}
<?xml version="1.0" encoding="utf-8" ?> 
<configuration> 
  <appSettings> 
    <add key="Raven/Port" value="*"/> 
    <add key="Raven/DataDir" value="~\Data"/> 
    <add key="Raven/AnonymousAccess" value="Get" /> 
  </appSettings> 
        <runtime> 
                <loadFromRemoteSources enabled="true"/> 
                <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1"> 
                        <probing privatePath="Analyzers"/> 
                </assemblyBinding> 
        </runtime> 
</configuration>
{CODE-END /}

## Configuration options

### Core settings

* **Raven/MaxPageSize**  
    The maximum page size that can be specified on this server.  
    _Default:_ 1024  
    _Minimum:_ 10
    
* **Raven/MemoryCacheExpiration**  
    The expiration value for documents in the internal document cache. Value is in seconds.   
    _Default:_ 5 minutes   

* **Raven/MemoryCacheLimitMegabytes**
	The max size in MB for the internal document cache inside RavenDB server.   
	_Default:_ 50% of the total system memory minus the size of the Esent cache.  

* **Raven/MemoryCacheLimitPercentage**
	The percentage of memory that the internal document cache inside RavenDB server will use.   
	_Default:_ 0 (auto)   

* **Raven/MemoryCacheLimitCheckInterval**
	The internal for checking that the internal document cache inside RavenDB server will be cleaned.   
	_Format:_ HH:MM:SS   
	_Default:_ depends on system polling interval   

* **Raven/MaxSecondsForTaskToWaitForDatabaseToLoad**   
	If the database is being loaded for the first time, this value indicates how many seconds will task wait for load completion before throwing exception.   
    _Default:_ 5  

* **Raven/MinThreadPoolWorkerThreads**   
Indicates minimum worker threads amount value for the .net thread pool. Might be usefull when one wants to help the system to deal with violent bursts of work.
_Default:_ ThreadPool current value
_Minimum:_ 2

* **Raven/MinThreadPoolCompletionThreads**   
Indicates minimum completion threads amount value for the .net thread pool. Might be usefull when one wants to help the system to deal with violent bursts of work.
_Default:_ ThreadPool current value  
_Minimum:_ 2

### Index settings

* **Raven/IndexStoragePath**  
    The path for the indexes on disk. Useful if you want to store the indexes on another HDD for performance reasons.  
    _Default:_ ~/Data/Indexes

* **Raven/MaxIndexWritesBeforeRecreate**   
    The number of writes before index writer will be recreated (to save memory).     
    _Default:_ 256 * 1024    

* **Raven/MaxNumberOfParallelIndexTasks**  
    The number of indexing tasks that can be run in parallel. There is usually one or two indexing tasks for each index.   
	_Default:_ the number of processors in the current machine
    
* **Raven/MaxNumberOfItemsToIndexInSingleBatch**  
	The max number of items that will be indexed in a single batch. Larger batch size result in faster indexing, but higher memory usage.   
	_Default:_ 128 * 1024 for 64-bit and 16 * 1024 for 32-bit  
	_Minimum:_ 128

* **Raven/MaxNumberOfItemsToReduceInSingleBatch**  
	The max number of items that will be indexed in a single batch. Larger batch size result in faster indexing, but higher memory usage.   
	_Default:_ 1/2 * `Raven/MaxNumberOfItemsToIndexInSingleBatch`
	_Minimum:_ 128

* **Raven/MaxNumberOfItemsToPreFetchForIndexing**  
	The max number of items that will be prefetched for indexing. Larger batch size result in faster indexing, but higher memory usage.   
	_Default:_ 128 * 1024 for 64-bit and 16 * 1024 for 32-bit  
	_Minimum:_ 128

* **Raven/InitialNumberOfItemsToIndexInSingleBatch**
	The number of items that will be indexed in a single batch. Larger batch size result in faster indexing, but higher memory usage.    
    _Default:_ 512 for 64-bit and 256 for 32-bit  

* **Raven/AvailableMemoryForRaisingIndexBatchSizeLimit**
	The minimum amount of memory available for us to double the size of InitialNumberOfItemsToIndexInSingleBatch if we need to.   
	_Default:_ 50% of total system memory  
	_Minimum:_ 768

* **Raven/ResetIndexOnUncleanShutdown**
	When the database is shut down rudely, determine whatever to reset the index or to check it. Note that checking the index may take some time on large databases.   
	_Default:_ false

* **Raven/MaxIndexingRunLatency**
	What is the suggested max latency for a single indexing run that allows the database to increase the indexing batch size.   
	_Default:_ 5 minutes 

* **Raven/TimeToWaitBeforeRunningIdleIndexes**
	Time that server is waiting before running idle indices.   
	_Default:_ 10 minutes    

* **Raven/TimeToWaitBeforeMarkingAutoIndexAsIdle**
	Time that server will wait before marking auto index as idle.    
	_Default:_ 1 hour   

* **Raven/TimeToWaitBeforeRunningAbandonedIndexes**
	Time that server will wait before running abandoned indices.      
	_Default:_ 3 hours    

* **Raven/TimeToWaitBeforeMarkingIdleIndexAsAbandoned**
	Time that server will wait before marking idle indices as abandoned.   
	_Default:_ 72 hours   

* **Raven/TaskScheduler**
	The TaskScheduler type to use for executing indexing.   

* **Raven/NewIndexInMemoryMaxMB**  
    The max size in MB of a new index held in memory. When the index exceeds that value, it will be using a disk rather than memory for indexing.   
    _Default:_ 64 MB   
    _Minimum:_ 1 MB   

* **Raven/CreateAutoIndexesForAdHocQueriesIfNeeded**  
	Whatever we allow creation of auto indexes on dynamic queries.   
	_Default:_ true

* **Raven/SkipCreatingStudioIndexes**  
	Control whatever the Studio default indexes will be created or not. These default indexes are only used by the UI, and are not required for RavenDB to operate.   
	_Default:_ false

* **Raven/LimitIndexesCapabilities**  
	Control whatever RavenDB limits what the indexes can do (to avoid potentially destabilizing operations).   
	_Default:_ false

* **Raven/CompiledIndexCacheDirectory**  
	Path to a directory used by index compilator.    
	_Default:_ ~\Raven\CompiledIndexCache

* **Raven/NumberOfItemsToExecuteReduceInSingleStep**  
	The number that controls if single step reduce optimization is performed. If the count of mapped results is less than this value then the reduce is executed in a single step.  
	_Default:_ 1024

* **Raven/DisableDocumentPreFetchingForIndexing**  
	Disables the document prefetcher.
	_Default:_ false

* **Raven/MaxIndexCommitPointStoreTimeInterval**  
	Maximum time interval for storing commit points for map indexes when new items were added. The commit points are used to restore index if unclean shutdown was detected.   
	_Default:_ 5 minutes

* **Raven/MaxNumberOfStoredCommitPoints**  
	Maximum number of kept commit points to restore map index after unclean shutdown.   
	_Default:_ 5

* **Raven/MinIndexingTimeIntervalToStoreCommitPoint**  
	Minimum interval between between successive indexing that will allow to store a  commit point.    
	_Default:_ 1 minute

* **Raven/DisableInMemoryIndexing**  
	Prevent all new created indexes from being kept in memory. In order to set this option per index you need to specify it in its [IdexDefinition](../../client-api/querying/static-indexes/defining-static-index).    
	_Default:_ false

* **Raven/MemoryLimitForIndexing**      
    Maximum number of megabytes that can be used by database to control the maximum size of the processing batches. Default: 1024 or 75% percent of available memory.   
    _Default:_ 1024 or 75% percent of available memory if 1GB is not available    

### Data settings:

* **Raven/RunInMemory**  
    Whatever the database should run purely in memory. When running in memory, nothing is written to disk and if the server is restarted all data will be lost. This is mostly useful for testing.   
    _Default:_ false  

* **Raven/DataDir**  
    The path for the database directory. Can use ~\ as the root, in which case the path will start from the server base directory.  
    _Default:_ ~\Data  

* **Raven/StorageTypeName**  
    What storage type to use (see: RavenDB Storage engines)  
    _Allowed values:_ esent, munin   
    _Default:_ esent  

* **Raven/TransactionMode**  
    What transaction mode to use. Safe transaction mode ensures data consistency, but is slower. Lazy is faster, but may result in a data loss if the server crashes.   
    _Allowed values:_  Lazy, Safe   
    _Default:_ Safe

### Http settings

* **Raven/HostName**  
    The hostname to bind the embedded http server to, if we want to bind to a specific hostname, rather than all.  
    _Default:_ none, binds to all host names  

* **Raven/Port**
    The port to use when creating the http listener. 
	_Allowed:_ 1 - 65,536 or * (find first available port from 8080 and upward)   
    _Default:_ 8080    

* **Raven/VirtualDirectory**  
    The virtual directory for the RavenDB server.  
    _Default:_ /  

* **Raven/HttpCompression**  
    Whatever http compression is enabled.   
    _Default:_ true  

* **Raven/UseSsl**
    Enable/disable SSL.   **Note: this only applies when RavenDB is run as a windows service.**
    _Default:_ false   

* **Raven/AccessControlAllowOrigin**  
    Configures the server to send Access-Control-Allow-Origin header with the specified value. If this value isn't specified, all the access control settings are ignored.   
    _Allowed values:_ null (don't send the header), *, http://example.org,   
	_Default:_ none  

* **Raven/AccessControlMaxAge**
	Configures the server to send Access-Control-Max-Age header with the specified value.  
	_Default:_ 1728000 (20 days)

* **Raven/AccessControlAllowMethods**
	Configures the server to send Access-Control-Allow-Methods header with the specified value.   
	_Default:_ PUT, PATCH, GET, DELETE, POST.

* **Raven/AccessControlRequestHeaders**
	Configures the server to send Access-Control-Request-Headers header with the specified value.   
	_Default:_ none

* **Raven/MaxConcurrentRequestsForDatabaseDuringLoad**   
    Maximum number of allowed request to databases that are being loaded before warning messages will be returned.
    _Default:_ 10

### Misc settings

* **Raven/License**
	The full license string for RavenDB. If Raven/License is specified, it overrides the Raven/LicensePath configuration.   

* **Raven/LicensePath**
	The path to the license file for RavenDB.   
	_Default:_ ~\license.xml

* **Raven/ServerName**
	Name of the server that will show up on `/admin/stats` endpoint.   

* **Raven/ClusterName**
	Name of the cluster that will show up on `/admin/stats` endpoint.   

### Bundles

* **Raven/ActiveBundles**
	Semicolon separated list of bundles names, such as: 'Replication;Versioning'. If the value is not specified, none of the bundles are installed.   
	_Default:_ none

* **Raven/BundlesSearchPattern**
	Allow to limit the loaded plugins by specifying a search pattern, such as Raven.*.dll. Multiple values can be specified, separated by a semicolon (;).   

* **Raven/PluginsDirectory**  
    The location of the plugins directory for this database.   
    _Default:_ ~\Plugins  

### Studio

* **Raven/WebDir**  
    The location of the web directory for known files that makes up the RavenDB internal website.   
    _Default:_ ~/Raven/WebUI

* **Raven/RedirectStudioUrl**
	The url to redirect the user to when then try to access the local studio.   

### Esent settings

* **Raven/Esent/CacheSizeMax**  
    The size in MB of the Esent page cache, which is the default storage engine.   
    _Default:_ 256 for 32-bit and 25% of total system memory for 64-bit  
	_Minimum:_ 256 for 32-bit and 1024 for 64-bit  

* **Raven/Esent/MaxVerPages**  
    The maximum size of version store (in memory modified data) available. The value is in megabytes.   
    _Default:_ 512  

* **Raven/Esent/DbExtensionSize**  
    The size that the database file will be enlarged with when the file is full. The value is in megabytes. Lower values will result in smaller file size, but slower performance.    
    _Default:_ 8  

* **Raven/Esent/LogFileSize**  
    The size of the database log file. The value is in megabytes.    
    _Default:_ 64  

* **Raven/Esent/LogBuffers**  
    The size of the in memory buffer for transaction log.   
    _Default:_ 16  

* **Raven/Esent/MaxCursors**  
    The maximum number of cursors allowed concurrently.  
    _Default:_ 2048  
    
* **Raven/Esent/LogsPath**  
    The path for the esent logs. Useful if you want to store the indexes on another HDD for performance reasons.     
    _Default_: ~/Data/Logs  

* **Raven/Esent/CircularLog**  
    Whatever circular logs will be used, defaults to true. If you want to use incremental backups, you need to turn this off, but logs will only be truncated on backup.  
    _Default_: true  

### Tenants

* **Raven/Tenants/MaxIdleTimeForTenantDatabase**
	The time in seconds to allow a tenant database to be idle. Value is in seconds.   
	_Default:_ 900

* **Raven/Tenants/FrequencyToCheckForIdleDatabases**
	The time in seconds to check for an idle tenant database. Value is in seconds.   
	_Default:_ 60

### Quotas

* **Raven/Quotas/Size/HardLimitInKB**
	The hard limit after which we refuse any additional writes.   
	_Default:_ none

* **Raven/Quotas/Size/SoftMarginInKB**
	The soft limit before which we will warn about the quota.   
	_Default:_ 1024

* **Raven/Quotas/Documents/HardLimit**
	The hard limit after which we refuse any additional documents.   
	_Default:_ Int64.MaxValue

* **Raven/Quotas/Documents/SoftLimit**
	The soft limit before which we will warn about the document limit quota.   
	_Default:_ Int64.MaxValue

### JavaScript parser

* **Raven/MaxStepsForScript**
	Maximum number of steps that javascripts functions can have (used for scripted patching).   
	_Default:_ 10000

* **Raven/AdditionalStepsForScriptBasedOnDocumentSize**
	Number that will expand `Raven/MaxStepsForScript` based on a document size. Formula is as follows: MaxStepsForScript = `Raven/MaxStepsForScript` + (documentSize * `Raven/AdditionalStepsForScriptBasedOnDocumentSize`)
	_Default:_ 5

### [Authorization & Authentication](../authentication)

* **Raven/AnonymousAccess**
	Determines what actions an anonymous user can do. Get - read only, All - read & write, None - allows access to only authenticated users, Admin - all (including administrative actions).   
	_Default:_ Get

{WARNING If your database instance does not have a valid license then `Admin` is the only available option to set. In a commercial system it should not be used. It is only for testing and development purposes, since it grants to **ANY** user administrative rights.  /}

* **Raven/AllowLocalAccessWithoutAuthorization**
	If set local request don't require authentication.   
	_Default:_ Get

* **Raven/OAuthTokenServer**
	The url clients should use for authenticating when using OAuth mode.  
	_Default:_ http://RavenDB-Server-Url/OAuth/AccessToken - the internal OAuth server.

* **Raven/OAuthTokenCertificatePath**
	The path to the OAuth certificate.  
	_Default:_ none. If no certificate is specified, one will be automatically created.

* **Raven/OAuthTokenCertificatePassword**
	The password for the OAuth certificate.  
	_Default:_ none

### [Encryption](../extending/bundles/encryption)

* **Raven/Encryption/Algorithm**   
	[AssemblyQualifiedName](https://msdn.microsoft.com/en-us/library/system.type.assemblyqualifiedname.aspx) value. Additionaly provided type must be a subclass of [SymmetricAlgorithm](https://msdn.microsoft.com/en-us/library/system.security.cryptography.symmetricalgorithm.aspx) from `System.Security.Cryptography` namespace and must not be an abstract class.     
	_Default:_ "System.Security.Cryptography.AesManaged, System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"    
* **Raven/Encryption/Key**   
	Key used for encryption purposes with minimum length of 8 characters, base64 encoded.    
* **Raven/Encryption/EncryptIndexes**   
	Boolean value indicating if the indexes should be encrypted.  
	_Default:_ True   

##Availability of configuration options

Many of the configuration options described in section above can be used both in global and per database context. If you want to set configuration per database, please refer to [this page](../../server/multiple-databases).

| Configuration option | Database | Global |
|:---------------------|:--------:|:------:|
| **Raven/MaxPageSize** | ![Yes](images\tick.png) | ![Yes](images\tick.png)  |
| **Raven/MemoryCacheExpiration** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MemoryCacheLimitMegabytes** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MemoryCacheLimitPercentage** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MemoryCacheLimitCheckInterval** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MaxSecondsForTaskToWaitForDatabaseToLoad** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/IndexStoragePath** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MaxIndexWritesBeforeRecreate** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MaxNumberOfParallelIndexTasks** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MaxNumberOfItemsToIndexInSingleBatch** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MaxNumberOfItemsToReduceInSingleBatch** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MaxNumberOfItemsToPreFetchForIndexing** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/InitialNumberOfItemsToIndexInSingleBatch** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/AvailableMemoryForRaisingIndexBatchSizeLimit** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/ResetIndexOnUncleanShutdown** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MaxIndexingRunLatency** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/TimeToWaitBeforeRunningIdleIndexes** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/TimeToWaitBeforeMarkingAutoIndexAsIdle** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/TimeToWaitBeforeRunningAbandonedIndexes** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/TimeToWaitBeforeMarkingIdleIndexAsAbandoned** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/TaskScheduler** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/NewIndexInMemoryMaxMB** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/CreateAutoIndexesForAdHocQueriesIfNeeded** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/SkipCreatingStudioIndexes** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/LimitIndexesCapabilities** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/CompiledIndexCacheDirectory** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/NumberOfItemsToExecuteReduceInSingleStep** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/DisableDocumentPreFetchingForIndexing** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MaxIndexCommitPointStoreTimeInterval** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MaxNumberOfStoredCommitPoints** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MinIndexingTimeIntervalToStoreCommitPoint** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/DisableInMemoryIndexing** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MemoryLimitForIndexing** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/RunInMemory** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/DataDir** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/StorageTypeName** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/TransactionMode** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/HostName** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/Port** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/UseSSL** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/VirtualDirectory** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/HttpCompression** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/AccessControlAllowOrigin** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/AccessControlMaxAge** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/AccessControlAllowMethods** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/AccessControlRequestHeaders** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/MaxConcurrentRequestsForDatabaseDuringLoad** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/License** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/LicensePath** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/ServerName** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/ClusterName** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/ActiveBundles** | ![Yes](images\tick.png)* | ![Yes](images\tick.png) |
| **Raven/BundlesSearchPattern** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/PluginsDirectory** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/Esent/CacheSizeMax** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Esent/MaxVerPages** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Esent/DbExtensionSize** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Esent/LogFileSize** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Esent/LogBuffers** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Esent/MaxCursors** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Esent/LogsPath** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Esent/CircularLog** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/Tenants/MaxIdleTimeForTenantDatabase** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/Tenants/FrequencyToCheckForIdleDatabases** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/Quotas/Size/HardLimitInKB** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Quotas/Size/SoftMarginInKB** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Quotas/Documents/HardLimit** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Quotas/Documents/SoftLimit** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/MaxStepsForScript** | ![Yes](images\tick.png) | ![No](images\tick.png) |
| **Raven/AdditionalStepsForScriptBasedOnDocumentSize** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/AnonymousAccess** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/AllowLocalAccessWithoutAuthorization** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Authorization/Windows/RequiredGroups** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Authorization/Windows/RequiredUsers** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/OAuthTokenServer** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/OAuthTokenCertificatePath** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/OAuthTokenCertificatePassword** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/Encryption/Algorithm** | ![Yes](images\tick.png)** | ![Yes](images\tick.png) |
| **Raven/Encryption/Key** | ![Yes](images\tick.png)** | ![Yes](images\tick.png) |
| **Raven/Encryption/EncryptIndexes** | ![Yes](images\tick.png)** | ![Yes](images\tick.png) |

{NOTE **Raven/ActiveBundles** can be changed after database has been created, but any changes may cause unexpected stability issues and are HIGHLY unrecommended. Please activate bundles only when creating new database. /}

{NOTE **Raven/Encryption** settings can only be provided when database is being created. Changing them later will cause DB malfunction. More about `Encryption` bundle can be found [here](../../server/extending/bundles/encryption). /}