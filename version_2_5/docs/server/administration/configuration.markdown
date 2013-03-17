﻿# Configuration options

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

### Index settings

* **Raven/IndexStoragePath**  
    The path for the indexes on disk. Useful if you want to store the indexes on another HDD for performance reasons.  
    _Default:_ ~/Data/Indexes

* **Raven/MaxNumberOfParallelIndexTasks**  
    The number of indexing tasks that can be run in parallel. There is usually one or two indexing tasks for each index.   
	_Default:_ the number of processors in the current machine
    
* **Raven/MaxNumberOfItemsToIndexInSingleBatch**  
	The max number of items that will be indexed in a single batch. Larger batch size result in faster indexing, but higher memory usage.   
	_Default:_ 128 * 1024 for 64-bit and 64 * 1024 for 32-bit  
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

* **Raven/TaskScheduler**
	The TaskScheduler type to use for executing indexing.   

* **Raven/TempIndexPromotionMinimumQueryCount**  
    The number of times a temporary index has to be queried during the promotion threshold to become a permanent auto index.    
    _Default:_ 100   
	_Minimum:_ 1  

* **Raven/TempIndexPromotionThreshold**  
	The promotion threshold for promoting a temporary dynamic index into a permanent auto index. The value is in second and refer to the length of time that the index have to get to the minimum query count value.   
    _Default:_ 60000 (once per minute)  

* **Raven/TempIndexCleanupPeriod**  
    How often will temp dynamic indexes be purged from the system. The value is in seconds.   
    _Default:_ 600 (10 minutes)  

* **Raven/TempIndexCleanupThreshold**  
    How long does a temporary index hang around if there are no queries made to it. The value is in seconds.   
    _Default:_ 1200 (20 minutes)  

* **Raven/TempIndexInMemoryMaxMB**  
    The max size in MB of a temporary index held in memory. When a temporary dynamic index exceeds that value, it will be using on disk indexing, rather then RAM indexing.   
    _Default:_ 25 MB  
    _Minimum:_ 1 MB

* **Raven/CreateTemporaryIndexesForAdHocQueriesIfNeeded**
	Whatever we allow creation of temporary indexes on dynamic queries.   
	_Default:_ true

* **Raven/SkipCreatingStudioIndexes**
	Control whatever the Studio default indexes will be created or not. These default indexes are only used by the UI, and are not required for RavenDB to operate.   
	_Default:_ false

* **Raven/LimitIndexesCapabilities**
	Control whatever RavenDB limits what the indexes can do (to avoid potentially destabilizing operations).   
	_Default:_ false

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
    Enable/disable SSL.   **Note: this only applies when RavenDB is run as a windows service**
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

### Misc settings

* **Raven/License**
	The full license string for RavenDB. If Raven/License is specified, it overrides the Raven/LicensePath configuration.   

* **Raven/LicensePath**
	The path to the license file for RavenDB.   
	_Default:_ ~\license.xml

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

* **Raven/Tenants/FrequnecyToCheckForIdleDatabases**
	The time in seconds to check for an idle tenant database. Value is in seconds.   
	_Default:_ 60

### Quotas

* **Raven/Quotas/Size/HardLimitInKB**
	The hard limit after which we refuse any additional writes.   
	_Default:_ none

* **Raven/Quotas/Size/SoftMarginInKB**
	The soft limit before which we will warn about the quota.   
	_Default:_ 1024

### [Authorization & Authentication](../authentication/index)

* **Raven/AnonymousAccess**
	Determines what actions an anonymous user can do. Get - read only, All - read & write, None - allows access to only authenticated users.   
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
	[AssemblyQualifiedName](http://msdn.microsoft.com/en-us/library/system.type.assemblyqualifiedname.aspx) value. Additionaly provided type must be a subclass of [SymmetricAlgorithm](http://msdn.microsoft.com/en-us/library/system.security.cryptography.symmetricalgorithm.aspx) from `System.Security.Cryptography` namespace and must not be an abstract class.     
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
| &nbsp; |||
| **Raven/IndexStoragePath** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MaxNumberOfParallelIndexTasks** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MaxNumberOfItemsToIndexInSingleBatch** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/InitialNumberOfItemsToIndexInSingleBatch** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/AvailableMemoryForRaisingIndexBatchSizeLimit** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/ResetIndexOnUncleanShutdown** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/MaxIndexingRunLatency** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/TaskScheduler** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/TempIndexPromotionMinimumQueryCount** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/TempIndexPromotionThreshold** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/TempIndexCleanupPeriod** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/TempIndexCleanupThreshold** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/TempIndexInMemoryMaxMB** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/CreateTemporaryIndexesForAdHocQueriesIfNeeded** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/SkipCreatingStudioIndexes** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/LimitIndexesCapabilities** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/RunInMemory** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/DataDir** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/StorageTypeName** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/TransactionMode** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/HostName** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/Port** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/VirtualDirectory** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/HttpCompression** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/AccessControlAllowOrigin** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/AccessControlMaxAge** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/AccessControlAllowMethods** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/AccessControlRequestHeaders** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/License** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| **Raven/LicensePath** | ![No](images\delete.png) | ![Yes](images\tick.png) |
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
| **Raven/Tenants/FrequnecyToCheckForIdleDatabases** | ![No](images\delete.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/Quotas/Size/HardLimitInKB** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| **Raven/Quotas/Size/SoftMarginInKB** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
| &nbsp; |||
| **Raven/AnonymousAccess** | ![Yes](images\tick.png) | ![Yes](images\tick.png) |
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

{NOTE **Raven/Encryption** settings can only be provided when database is being created. Changing them later will cause DB malfunction. More about `Encryption` bundle can be found [here](../bundles/encryption). /}