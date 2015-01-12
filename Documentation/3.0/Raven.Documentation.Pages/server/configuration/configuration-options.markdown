# Configuration : Options

All the configuration options detailed below are defined in the <appSettings> section of your config file as separate values. When running RavenDB as a website (through IIS, or in Embedded mode), the config file is `web.config`; otherwise it is the `Raven.Server.exe.config` file.

Changes to the config file or additions / removal from the Plugins directory will not be picked up automatically by the RavenDB service. For your changes to be recognized you will need to restart the service. You can do so calling: <code>Raven.Server.exe /restart</code>.

If you are running in Embedded mode, or RavenDB is running as an IIS application, touching the web.config file will cause IIS to automatically restart RavenDB.

## Sample configurations file

This is the standard app.config XML file. The `appSettings` section is where the global configuration options go, also for web applications which have a web.config file instead.

{CODE-BLOCK:xml}
<?xml version="1.0" encoding="utf-8" ?> 
<configuration> 
  <appSettings> 
    <add key="Raven/Port" value="*"/> 
    <add key="Raven/DataDir/Legacy" value="~\Database\System"/> 
    <add key="Raven/DataDir" value="~\Databases\System"/> 
	<add key="Raven/AnonymousAccess" value="Get" /> 
	<add key="Raven/Licensing/AllowAdminAnonymousAccessForCommercialUse" value="false" />
	<add key="Raven/AccessControlAllowOrigin" value="*" />
  </appSettings> 
        <runtime> 
                <loadFromRemoteSources enabled="true"/> 
                <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1"> 
                        <probing privatePath="Analyzers"/> 
                </assemblyBinding> 
        </runtime> 
</configuration>
{CODE-BLOCK/}

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
	The max size (in megabytes) for the internal document cache inside RavenDB server.   
	_Default:_ 50% of the total system memory minus the size of the Esent cache.  

* **Raven/MemoryCacheLimitPercentage**
	The percentage of memory that the internal document cache inside RavenDB server will use.   
	_Default:_ 0 (auto)   

* **Raven/MemoryCacheLimitCheckInterval**
	The internal for checking that the internal document cache inside RavenDB server will be cleaned.   
	_Format:_ HH:MM:SS   
	_Default:_ depends on system polling interval   

* **Raven/MemoryLimitForProcessing** (previously Raven/MemoryLimitForIndexing)   
    Maximum number of megabytes that can be used by database to control the maximum size of the processing batches. Default: 1024 or 75% percent of available memory.   
    _Default:_ 1024 or 75% percent of available memory if 1GB is not available    

### Index settings

* **Raven/IndexStoragePath**  
    The path for the indexes on a disk. Useful if you want to store the indexes on another HDD for performance reasons.  
    _Default:_ ~/Data/Indexes

* **Raven/MaxIndexWritesBeforeRecreate**   
    The number of writes before the index writer will be recreated (to save memory).     
    _Default:_ 256 * 1024    

* **Raven/MaxNumberOfParallelIndexTasks**  
    The number of the indexing tasks that can be run in parallel. There is usually one or two indexing tasks for each index.   
	_Default:_ the number of processors in the current machine
    
* **Raven/MaxNumberOfItemsToIndexInSingleBatch**   
	The max number of items that will be indexed in a single batch. Larger batch size results in faster indexing, but higher memory usage.   
	_Default:_ 128 * 1024 for 64-bit and 16 * 1024 for 32-bit  
	_Minimum:_ 128

* **Raven/MaxNumberOfItemsToReduceInSingleBatch**   
	The max number of items that will be indexed in a single batch. Larger batch size results in faster indexing, but higher memory usage.   
	_Default:_ 1/2 * `Raven/MaxNumberOfItemsToIndexInSingleBatch`
	_Minimum:_ 128

* **Raven/MaxNumberOfItemsToPreFetchForIndexing**   
	The max number of items that will be prefetched for indexing. Larger batch size results in faster indexing, but higher memory usage.   
	_Default:_ 128 * 1024 for 64-bit and 16 * 1024 for 32-bit  
	_Minimum:_ 128

* **Raven/InitialNumberOfItemsToIndexInSingleBatch**   
	The number of items that will be indexed in a single batch. Larger batch size results in faster indexing, but higher memory usage.    
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
	The time for which the server is waiting before running idle indexes.   
	_Default:_ 10 minutes    

* **Raven/TimeToWaitBeforeMarkingAutoIndexAsIdle**   
	The time for which the server is waiting before marking auto index as idle.    
	_Default:_ 1 hour   

* **Raven/TimeToWaitBeforeRunningAbandonedIndexes**   
	The time for which the server is waiting before running abandoned indexes.      
	_Default:_ 3 hours    

* **Raven/TimeToWaitBeforeMarkingIdleIndexAsAbandoned**   
	The time for which the server is waiting before marking idle indexes as abandoned.   
	_Default:_ 72 hours   

* **Raven/TaskScheduler**   
	The TaskScheduler type to use for executing indexing.   

* **Raven/NewIndexInMemoryMaxMB**   
    The max size (in megabytes) of a new index held in memory. When the index exceeds that value, it will be using a disk rather than memory for indexing.   
    _Default:_ 64 MB   
    _Minimum:_ 1 MB  
    _Maximum:_ 1024 MB   

* **Raven/CreateAutoIndexesForAdHocQueriesIfNeeded**   
	Whether we allow creation of auto indexes on dynamic queries.   
	_Default:_ true

* **Raven/SkipCreatingStudioIndexes**   
	Control whether the Studio default indexes will be created or not. These default indexes are only used by the UI, and are not required for RavenDB to operate.   
	_Default:_ false

* **Raven/LimitIndexesCapabilities**   
	Control whether RavenDB limits what the indexes can do (to avoid potentially destabilizing operations).   
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
	Maximum time interval for storing commit points for map indexes when new items were added. The commit points are used to restore index if the unclean shutdown was detected.   
	_Default:_ 5 minutes

* **Raven/MaxNumberOfStoredCommitPoints**  
	Maximum number of kept commit points to restore map index after unclean shutdown.   
	_Default:_ 5

* **Raven/MinIndexingTimeIntervalToStoreCommitPoint**  
	Minimum interval between successive indexing that will allow storing a commit point.    
	_Default:_ 1 minute

* **Raven/DisableInMemoryIndexing**   
	Prevent all newly created indexes from being kept in memory. In order to set this option for an index you need to specify it in its [IndexDefinition](../../glossary/index-definition).    
	_Default:_ false

* **Raven/Indexing/FlushIndexToDiskSizeInMb**   
    Number of megabytes after which indexes are flushed to a disk.   
    _Default:_ 5

* **Raven/MaxSimpleIndexOutputsPerDocument**   
	Limits the number of map outputs that a simple index is allowed to create for a one source document. If a map operation applied to the one document
	produces more outputs than this number then an index definition will be considered as a suspicious, the indexing of this document will be skipped and
	the appropriate error message will be added to the indexing errors.   
	_Default:_ 15. In order to disable this check set value to -1.

* **Raven/MaxMapReduceIndexOutputsPerDocument**   
	Limits the number of map outputs that a map-reduce index is allowed to create for a one source document. If a map operation applied to the one document
	produces more outputs than this number then an index definition will be considered as a suspicious, the indexing of this document will be skipped and
	the appropriate error message will be added to the indexing errors.   
    _Default:_ 50. In order to disable this check set value to -1.

* **Raven/Indexing/MaxNumberOfItemsToProcessInTestIndexes**   
	Maximum number of items that will be passed to [test indexes](../../indexes/testing-indexes)   
	_Default:_ 512

* **Raven/NewIndexInMemoryMaxTime**   
	Indicates how long can we keep the new index in memory before we have to flush it (timespan).   
	_Default:_ 15 minutes  

### Data settings:

* **Raven/RunInMemory**   
    Whether the database should run purely in memory. When running in memory, nothing is written on a disk and if the server is restarted, all data will be lost. Useful mostly for testing.   
    _Default:_ false  

* **Raven/DataDir**  
    The path for the database directory. Can use ~\ as the root, in which case the path will start from the server base directory.  
    _Default:_ ~\Data  

* **Raven/DataDir/Legacy**  
    The path for the legacy database directory (prior 3.0). Can use ~\ as the root, in which case the path will start from the server base directory.  
    _Default:_ null  

* **Raven/StorageEngine** or **Raven/StorageTypeName**   
    What storage type to use (see: [Storage Engines](../../server/configuration/storage-engines))  
    _Allowed values:_ esent, voron   
    _Default:_ esent  

* **Raven/TransactionMode**   
    Esent only. What transaction mode to use. Safe transaction mode ensures data consistency, but is slower. Lazy is faster, but may result in a data loss if the server crashes.   
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

* **Raven/IgnoreSslCertificateErrors**   
    When set to *true*, RavenDB will ignore **all** SSL certificate validation errors. More [here](../../server/configuration/enabling-ssl#ignoring-ssl-errors).   
    _Default:_ false   

* **Raven/AccessControlAllowOrigin**   
    Configures the server to send Access-Control-Allow-Origin header with the specified value. If this value isn't specified, all the access control settings are ignored.   
    _Allowed values:_ null (don't send the header), `*`, http://example.org,   
	_Default:_ none (if this value isn't specified, all the access control settings are ignored)  

* **Raven/AccessControlMaxAge**   
	Configures the server to send Access-Control-Max-Age header with the specified value.  
	_Default:_ 1728000 (20 days)

* **Raven/AccessControlAllowMethods**   
	Configures the server to send Access-Control-Allow-Methods header with the specified value.   
	_Default:_ PUT, PATCH, GET, DELETE, POST.

* **Raven/AccessControlRequestHeaders**   
	Configures the server to send Access-Control-Request-Headers header with the specified value.   
	_Default:_ none

* **Raven/Headers/Ignore**   
    Semicolon separated list of the headers that server should ignore. e.g. Header-To-Ignore-1;Header-To-Ignore-2
    _Default:_ `null`

### Misc settings

* **Raven/License**   
	The full license string for RavenDB. If Raven/License is specified, it overrides the Raven/LicensePath configuration.   

* **Raven/LicensePath**   
	The path to the license file for RavenDB.   
	_Default:_ ~\license.xml

* **Raven/Licensing/AllowAdminAnonymousAccessForCommercialUse**   
	Indicates if `Raven/AnonymousAccess` can be set to `Admin` when commercial license is registered.   
	_Default:_ false

* **Raven/ServerName**   
	Name of the server that will show up on `/admin/stats` endpoint.   

* **Raven/ClusterName**   
	Name of the cluster that will show up on `/admin/stats` endpoint.   

### Bundles

* **Raven/ActiveBundles**   
	Semicolon separated list of the bundles' names, such as: 'Replication;Versioning'. If the value is not specified, none of the bundles are installed.   
	_Default:_ none

* **Raven/BundlesSearchPattern**   
	Allows limiting the loaded plugins by specifying a search pattern, such as Raven.*.dll. Multiple values can be specified, separated by a semicolon (;).   

* **Raven/PluginsDirectory**    
    The location of the plugins directory for this database.   
    _Default:_ ~\Plugins  

### Studio

* **Raven/WebDir**   
    The location of the web directory for the known files that make up the RavenDB internal website.   
    _Default:_ ~/Raven/WebUI

* **Raven/RedirectStudioUrl**   
	The url to redirect users to when they try to access the local Studio.   

### Esent settings

* **Raven/Esent/CacheSizeMax**    
    The size (in megabytes) of the Esent page cache, which is the default storage engine.   
    _Default:_ 256 for 32-bit and 25% of total system memory for 64-bit  
	_Minimum:_ 256 for 32-bit and 1024 for 64-bit  

* **Raven/Esent/MaxVerPages**   
    The maximum size (in megabytes) of a version store (in memory modified data) available.   
    _Default:_ 512  

* **Raven/Esent/PreferredVerPages**   
    The preferred size (in megabytes) of version store (in memory modified data) available. If the value exceed that level, optional background tasks data are removed from the version store.   
    _Default:_ 472  

* **Raven/Esent/DbExtensionSize**   
    The size (in megabytes) that the database file will be enlarged to when the file is full. Lower values will result in smaller file size, but slower performance.    
    _Default:_ 8  

* **Raven/Esent/LogFileSize**   
    The size (in megabytes) of the database log file.  
    _Default:_ 64  

* **Raven/Esent/LogBuffers**   
    The size of the in memory buffer for transaction log.   
    _Default:_ 8192  

* **Raven/Esent/MaxCursors**   
    The maximum number of the concurrently allowed cursors.  
    _Default:_ 2048  
    
* **Raven/Esent/LogsPath**   
    The path for the esent logs. Useful if you want to store the indexes on another HDD for performance reasons.     
    _Default_: ~/Data/Logs  

* **Raven/Esent/CircularLog**   
    Whatever circular logs will be used, it is true by default. If you want to use incremental backups, you need to turn this off, but logs will only be truncated on backup.  
    _Default_: true  

## Voron settings

* **Raven/Voron/AllowIncrementalBackups**   
    If you want to use incremental backups, you need to turn this to true, but then journal files will not be deleted after applying them to the data file. They will be deleted only after a successful backup. Default: false.     
    _Default_: false  

* **Raven/Voron/TempPath**   
    You can use this setting to specify different paths to temporary files. By default it is empty, which means that temporary files will be created at the same location as data file.  
    _Default_: `null`  

* **Raven/Voron/MaxBufferPoolSize**   
    You can use this setting to specify the maximum buffer pool size that can be used for transactional storage (in gigabytes). By default it is 4. Minimum value is 2.  
    _Default_: 4   

* **Raven/Voron/InitialSize**   
    You can use this setting to specify an initial file size for data file (in bytes).   

* **Raven/Voron/MaxScratchBufferSize**   
    The maximum scratch buffer (modified data by active transactions) size that can be used by Voron (in megabytes).
    _Default_: 1024

### Backup

* **Raven/IncrementalBackup/AlertTimeoutHours**   
    Number of hours after which incremental backup alert will be issued.
    _Default_: 24

* **Raven/IncrementalBackup/RecurringAlertTimeoutDays**   
    Number of days after which incremental backup alert will be shown again.
    _Default_: 7

### Tenants

* **Raven/Tenants/MaxIdleTimeForTenantDatabase**   
	The time (in seconds) for which a tenant database is allowed to be idle.   
	_Default:_ 900

* **Raven/Tenants/FrequencyToCheckForIdleDatabases**   
	The time (in seconds) after which a check for an idle tenant database should be run.  
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
	The soft limit before which we will warn about the document's limit quota.   
	_Default:_ Int64.MaxValue

### JavaScript parser

* **Raven/MaxStepsForScript**   
	Maximum number of steps that javascripts functions can have (used for scripted patching).   
	_Default:_ 10000

* **Raven/AdditionalStepsForScriptBasedOnDocumentSize**   
	Number that will expand `Raven/MaxStepsForScript`, based on a document size. Formula is as follows: MaxStepsForScript = `Raven/MaxStepsForScript` + (documentSize * `Raven/AdditionalStepsForScriptBasedOnDocumentSize`)
	_Default:_ 5

### [Authorization & Authentication](../../server/configuration/authentication-and-authorization)

* **Raven/AnonymousAccess**   
	Determines what actions an anonymous user can perform. Get - read only, All - read & write, None - allows access only to authenticated users, Admin - all (including administrative actions).   
	_Default:_ Get

{WARNING If your database instance does not have a valid license, then the `Admin` is the only available option to set. In a commercial system it should not be used. It is used only for testing and development purposes, since it grants administrative rights to **ANY** user.  /}

* **Raven/AllowLocalAccessWithoutAuthorization**   
	If set local request don't require authentication.   
	_Default:_ Get

* **Raven/OAuthTokenServer**   
	The url clients should use for authenticating when using OAuth mode.  
	_Default:_ http://RavenDB-Server-Url/OAuth/AccessToken - the internal OAuth server.

* **Raven/OAuthTokenCertificate**   
    The base 64 to the OAuth key use to communicate with the server.   
    _Default:_ `null` (if no key is specified, one will be automatically created).   

* **Raven/OAuthTokenCertificatePath**   
	The path to the OAuth certificate.  
	_Default:_ none. If no certificate is specified, one will be automatically created.

* **Raven/OAuthTokenCertificatePassword**   
	The password for the OAuth certificate.  
	_Default:_ none

### [Encryption](../../server/bundles/encryption)

* **Raven/Encryption/Algorithm**     
	[AssemblyQualifiedName](http://msdn.microsoft.com/en-us/library/system.type.assemblyqualifiedname.aspx) value. Additionaly provided type must be a subclass of [SymmetricAlgorithm](http://msdn.microsoft.com/en-us/library/system.security.cryptography.symmetricalgorithm.aspx) from `System.Security.Cryptography` namespace and must not be an abstract class.     
	_Default:_ "System.Security.Cryptography.AesManaged, System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"    

* **Raven/Encryption/Key**    
	Key used for encryption purposes, with minimum length of 8 characters, base64 encoded.    

* **Raven/Encryption/EncryptIndexes**    
	Boolean value indicating if the indexes should be encrypted.  
	_Default:_ True   

* **Raven/Encryption/FIPS**   
    Use FIPS compliant encryption algorithms. Read more [here](../../server/configuration/enabling-fips-compliant-encryption-algorithms).
    _Default:_ False

### Replication

* **Raven/Replication/FetchingFromDiskTimeout**   
    Number of seconds after which replication will stop reading documents/attachments from disk.
    _Default:_ 30

* **Raven/Replication/ReplicationRequestTimeout**   
    Number of milliseconds before replication requests will timeout.
    _Default:_ 60 * 1000

### Prefetcher

* **Raven/Prefetcher/FetchingDocumentsFromDiskTimeout**   
    Number of seconds after which prefetcher will stop reading documents from disk.
    _Default:_ 5

* **Raven/Prefetcher/MaximumSizeAllowedToFetchFromStorage**   
    Maximum number of megabytes after which prefetcher will stop reading documents from disk.
    _Default:_ 256
    

##Availability of configuration options

Many of the configuration options described in the section above can be used both in global and per database context. If you want to set configuration per database, please refer to [this](../../server/administration/multiple-databases) page.

| Configuration option | Database | Global |
|:---------------------|:--------:|:------:|
| **Raven/MaxPageSize** | ![Yes](images\tick.png) | ![Yes](images\tick.png)  |
| **Raven/MemoryCacheExpiration** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MemoryCacheLimitMegabytes** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MemoryCacheLimitPercentage** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MemoryCacheLimitCheckInterval** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MemoryLimitForProcessing** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
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
| **Raven/Indexing/FlushIndexToDiskSizeInMb** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Indexing/MaxNumberOfItemsToProcessInTestIndexes** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/NewIndexInMemoryMaxTime** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/RunInMemory** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/DataDir** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/StorageTypeName** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/TransactionMode** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/HostName** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/Port** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/UseSSL** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/IgnoreSslCertificateErrors** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/VirtualDirectory** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/HttpCompression** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/AccessControlAllowOrigin** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/AccessControlMaxAge** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/AccessControlAllowMethods** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/AccessControlRequestHeaders** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/Headers/Ignore** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/License** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/LicensePath** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/Licensing/AllowAdminAnonymousAccessForCommercialUse** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/ServerName** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/ClusterName** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/ActiveBundles** | ![Yes](images\tick.png)* | ![Yes](images\tick.png) |
| **Raven/BundlesSearchPattern** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/PluginsDirectory** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/Esent/CacheSizeMax** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Esent/MaxVerPages** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Esent/PreferredVerPages** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Esent/DbExtensionSize** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Esent/LogFileSize** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Esent/LogBuffers** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Esent/MaxCursors** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Esent/LogsPath** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Esent/CircularLog** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/Voron/AllowIncrementalBackups** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Voron/TempPath** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Voron/MaxBufferPoolSize** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Voron/InitialSize** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Voron/MaxScratchBufferSize** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/IncrementalBackup/AlertTimeoutHours** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/IncrementalBackup/RecurringAlertTimeoutDays** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/Tenants/MaxIdleTimeForTenantDatabase** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/Tenants/FrequencyToCheckForIdleDatabases** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/Quotas/Size/HardLimitInKB** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Quotas/Size/SoftMarginInKB** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Quotas/Documents/HardLimit** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Quotas/Documents/SoftLimit** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/MaxStepsForScript** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/AdditionalStepsForScriptBasedOnDocumentSize** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/AnonymousAccess** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/AllowLocalAccessWithoutAuthorization** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Authorization/Windows/RequiredGroups** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Authorization/Windows/RequiredUsers** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/OAuthTokenServer** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/OAuthTokenCertificate** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/OAuthTokenCertificatePath** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/OAuthTokenCertificatePassword** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/Encryption/Algorithm** | ![Yes](images\tick.png)** | ![Yes](images\tick.png) |
| **Raven/Encryption/Key** | ![Yes](images\tick.png)** | ![Yes](images\tick.png) |
| **Raven/Encryption/EncryptIndexes** | ![Yes](images\tick.png)** | ![Yes](images\tick.png) |
| **Raven/Encryption/FIPS** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/Replication/FetchingFromDiskTimeout** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Replication/ReplicationRequestTimeout** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/Prefetcher/FetchingDocumentsFromDiskTimeout** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Prefetcher/MaximumSizeAllowedToFetchFromStorage** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |

{INFO:Information}

- **Raven/ActiveBundles** can be changed after database has been created, but any changes may cause unexpected stability issues and are HIGHLY unrecommended. Please activate bundles only when creating new databases.
- **Raven/Encryption** settings can only be provided when a database is being created. Changing them later will cause DB malfunction. More about `Encryption` bundle can be found [here](../bundles/encryption).

{INFO/}
