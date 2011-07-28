# Configuration options

All the configuration options detailed below are defined in the app.config's <appSettings> as separate values.

## Core settings

* **Raven/BackgroundTasksPriority**  
    What thread priority to give the various background tasks RavenDB uses (mostly for indexing)  
    _Allowed values:_ Lowest, BelowNormal, Normal, AboveNormal, Highest  
    _Default:_ Normal

* **Raven/MaxPageSize**  
    The maximum allowed page size for queries.  
    _Default:_ 1024  
    _Minimum:_ 10

## Index settings

* **Raven/IndexStoragePath**  
    The path to the indexes that are kept on disk. Putting them in a different drive than the actual data will improve performance significantly.  
    _Default_: ~/Data/Indexes
    
* **Raven/SkipCreatingStudioIndexes**  
    Set to true, tells RavenDB NOT to create the indexes that are used by the Management Studio to provide some Collection related features.  
    _Allowed values:_ true/false  
    _Default:_ false  

* **Raven/MaxNumberOfParallelIndexTasks**  
    The maximum number of indexing tasks allowed to run in parallel  
    _Default:_ the number of processors in the current machine
    
* **Raven/MaxNumberOfItemsToIndexInSingleBatch**  
    Max number of items to take for indexing in a batch  
    _Default:_ 2500  
    _Minimum:_ 128  

* **Raven/TempIndexPromotionMinimumQueryCount**  
    How many times a temporary, auto-generated index has to be accessed before it can be promoted to be a permanent one  
    _Default:_ 100  

* **Raven/TempIndexPromotionThreshold**  
Time (in milliseconds) the index has to be queried at least once in order for it to become permanent  
    _Default:_ 60000 (once per minute)  

* **Raven/TempIndexCleanupPeriod**  
    How often to run the temporary index cleanup process (in seconds)  
    _Default:_ 600 (10 minutes)  

* **Raven/TempIndexCleanupThreshold**  
    How much time in seconds to wait after a temporary index has been used before removing it if no further calls were made to it during that time  
    _Default:_ 1200 (20 minutes)  

* **Raven/TempIndexInMemoryMaxMB**  
    Temp indexes are kept in memory until they reach this integer value in MB  
    _Default:_ 25 MB  
    _Minimum:_ 1 MB

## Data settings:
* **Raven/RunInMemory**  
    Should RavenDB's storage be in-memory. If set to true, Munin would be used as the storage engine, regardless of what was specified for StorageTypeName  
    _Allowed values:_ true/false  
    _Default:_ false  

* **Raven/DataDir**  
    The directory for the RavenDB database. You can use the ~\ prefix to refer to RavenDB's base directory.  
    _Default:_ ~\Data  

* **Raven/StorageTypeName**  
    What storage type to use (see: RavenDB Storage engines)  
    _Allowed values:_ esent, munin (at this point of time only Esent is fully supported by RavenDB)  
    _Default:_ esent  

* **Raven/TransactionMode**  
    What sort of transaction mode to use  
    _Allowed values:_  Lazy (faster, but can result in data loss in the case of server crash), Safe (slower, but will never lose data)  
    _Default:_ Safe

## Http settings

* **Raven/HostName**  
    The hostname to use when creating the http listener (null to accept any hostname or address)  
    _Default:_ none, binds to all host names  

* **Raven/Port**
    The port to use when creating the http listener.  
    _Default:_ 8080  

* **Raven/VirtualDirectory**  
    The virtual directory to use when creating the http listener.  
    _Default:_ /  

* **Raven/HttpCompression**  
    Whether to use http compression or not  
    _Allowed values:_ true/false  
    _Default:_ true  

* **Raven/AccessControlAllowOrigin**  
    Determine the value of the Access-Control-Allow-Origin header sent by the server  
    _Allowed values:_ null (don't send the header), *, http://example.org  

* **Raven/AnonymousUserAccessMode**  
    Defines which operations are allowed for anonymous users  
    _Allowed values:_ All, Get, None  
    _Default:_ Get  

## Misc settings

* **Raven/PluginsDirectory**  
    Where to look for plugins for RavenDB  
    _Default:_ ~\Plugins  

* **Raven/WebDir**  
    The directory to search for RavenDB's WebUI. This is usually only useful if you are debugging RavenDB's WebUI  
    _Default:_ ~/Raven/WebUI  

## Esent settings

* **Raven/Esent/CacheSizeMax**  
    The maximum size of the in memory cache that is used by the storage engine. The value is in megabytes.  
    _Default:_ 1024  

* **Raven/Esent/MaxVerPages**  
    The maximum size of version store (in memory modified data) available. The value is in megabytes.  
    _Default:_ 128  

* **Raven/Esent/DbExtensionSize**  
    The size that the database file will be enlarged with when the file is full. The value is in megabytes. Lower values will result in smaller file size, but slower performance.  
    _Default:_ 16  

* **Raven/Esent/LogFileSize**  
    The size of the database log file. The value is in megabytes.  
    _Default:_ 16  

* **Raven/Esent/LogBuffers**  
    The size of the in memory buffer for transaction log.  
    _Default:_ 16  

* **Raven/Esent/MaxCursors**  
    The maximum number of cursors allowed concurrently.  
    _Default:_ 2048  
    
* **Raven/Esent/LogsPath**  
    Where to keep the Esent transaction logs. Putting the logs in a different drive than the data and indexes will improve performance significantly.  
    _Default_: ~/Data/logs  