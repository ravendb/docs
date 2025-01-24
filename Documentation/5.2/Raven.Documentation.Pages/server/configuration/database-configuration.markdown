# Configuration: Database

---

{NOTE: }

* This article describes database configuration settings. 

* After editing & saving a configuration key, the change will only take effect **after** the database is reloaded.  
  See [How to reload the database](../../studio/database/settings/database-settings#how-to-reload-the-database).

{WARNING: }
**Warning**:  
Do not modify the database settings unless you are an expert and know what you're doing.  
{WARNING/}

{NOTE/}

---

{PANEL:Databases.QueryTimeoutInSec}

The time in seconds to wait before canceling a query.  
This timeout refers to queries and streamed queries.  

If a query exceeds the specified time, an *OperationCanceledException* will be thrown.  
For the streaming queries, the timeout is delayed every time a query result is pushed to the stream.  
The timeout will be exceeded only if no result is streamed within that time.

- **Type**: `int`
- **Default**: `300`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Databases.QueryOperationTimeoutInSec}

The time in seconds to wait before canceling a query related operation (patch/delete query).  
The timeout is delayed every time a query result is processed.  
The timeout will be exceeded only if no document is processed within that time.  

- **Type**: `int`
- **Default**: `300`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Databases.OperationTimeoutInSec}

The time in seconds to wait before canceling specific operations (such as indexing terms).

- **Type**: `int`
- **Default**: `300`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Databases.CollectionOperationTimeoutInSec}

The time in seconds to wait before canceling several collection operations  
(such as batch delete documents from Studio).  
If an operation exceeds the specified time, an *OperationCanceledException* will be thrown.  

- **Type**: `int`
- **Default**: `300`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Databases.Compression.CompressAllCollectionsDefault}

Set whether [documents compression](../../server/storage/documents-compression) should be turned on by default for ALL COLLECTIONS  
on newly created databases.  
This does not prevent you from enabling this after the database is created.  

- **Type**: `bool`
- **Default**: `false`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Databases.Compression.CompressRevisionsDefault}

Set whether [documents compression](../../server/storage/documents-compression) should be turned on by default for REVISIONS  
on newly created databases.  
It is useful to turn this option off if you are expected to run on very low end hardware.  
That does not prevent you from enabling this after the database is created.  

- **Type**: `bool`  
- **Default**: `true`  
- **Scope**: Server-wide only  

{PANEL/}

{PANEL:Databases.ConcurrentLoadTimeoutInSec}

The time in seconds to wait for a database to start loading when under load.  
It is the time to wait for the database to become available when too many different databases get loaded at the same time.

- **Type**: `int`
- **Default**: `60`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Databases.MaxConcurrentLoads}

Specifies the maximum amount of databases that can be loaded simultaneously.

- **Type**: `int`
- **Default**: `8`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Databases.MaxIdleTimeInSec}

Set time in seconds for max idle time for the database.  
After this time an idle database will be unloaded from memory.  
Use a lower time period if memory resources are limited.

- **Type**: `int`
- **Default**: `900`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Databases.FrequencyToCheckForIdleDatabasesInSec}

 The time in seconds to check for an idle tenant database.

- **Type**: `int`
- **Default**: `60`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Databases.PulseReadTransactionLimitInMb}

Number of megabytes occupied by encryption buffers (if database is encrypted) or mapped 32 bites buffers (when running on 32 bits) 
after which a read transaction will be renewed to reduce memory usage during long running operations like backups or streaming.  

- **Type**: `int`
- **Default**: The default value is determined by the amount of RAM on your machine:  
    * 32 bit platforms: `16 MB`  
    * Less than 4 GB RAM: `32 MB`  
    * Less than 16 GB RAM: `64 MB`  
    * Less than 64 GB RAM: `128 MB`  
    * More than 64 GB RAM: `256 MB`  
- **Scope**: Server-wide or per database  

{PANEL/}

{PANEL:Databases.DeepCleanupThresholdInMin}

EXPERT ONLY.  
A deep database cleanup will be done when this number of minutes has passed since the last time work was done on the database.

- **Type**: `int`
- **Default**: `5`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Databases.RegularCleanupThresholdInMin}

EXPERT ONLY.  
A regular database cleanup will be done when this number of minutes has passed since the last database idle time.

- **Type**: `int`
- **Default**: `10`
- **Scope**: Server-wide or per database

{PANEL/}
