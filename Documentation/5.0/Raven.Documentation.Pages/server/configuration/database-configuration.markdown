# Configuration: Database

After editing & saving a configuration key, the change does not take effect 
  until the database is [reloaded via the Studio](../../studio/database/settings/database-settings#how-to-reload-the-database)
  or via [API operations](../../client-api/operations/maintenance/configuration/database-settings-operation).  

{WARNING: Warning}
Do not modify the database settings unless you are an expert and know what you're doing.  
{WARNING/}

{PANEL:Databases.QueryTimeoutInSec}

The time in seconds to wait before canceling a query.

- **Type**: `int`
- **Default**: `300`
- **Scope**: Server-wide or per database

This timeout refers to queries and streamed queries.

If a query exceeds the specified time, an *OperationCanceledException* will be thrown. For the streaming queries the timeout is delayed every time a
query result is pushed to the stream. The timeout will be exceeded only if no result is streamed within that time.

{PANEL/}

{PANEL:Databases.QueryOperationTimeoutInSec}

The time in seconds to wait before canceling a query related operation (patch/delete query).

- **Type**: `int`
- **Default**: `300`
- **Scope**: Server-wide or per database

The timeout is delayed every time a query result is processed. The timeout will be exceeded only if no document is processed within that time.

{PANEL/}

{PANEL:Databases.OperationTimeoutInSec}

The time in seconds to wait before canceling specific operations (such as indexing terms).

- **Type**: `int`
- **Default**: `300`
- **Scope**: Server-wide or per database

{PANEL/}

{PANEL:Databases.CollectionOperationTimeoutInSec}

The time in seconds to wait before canceling several collection operations (such as batch delete documents from Studio).

- **Type**: `int`
- **Default**: `300`
- **Scope**: Server-wide or per database

Set timeout for some operations on collections (such as batch delete documents from studio) requiring their own timeout settings.

If an operation exceeds the specified time, an *OperationCanceledException* will be thrown

{PANEL/}

{PANEL:Databases.Compression.CompressRevisionsDefault}

Whether revision compression should be on by default or not on newly created databases.  

- **Type**: `bool`  
- **Default**: `true`  
- **Scope**: Server-wide only  

Determines whether [revisions](../../server/extensions/revisions) should be compressed.  
Applies to all databases created on this server.  

{PANEL/}

{PANEL:Databases.ConcurrentLoadTimeoutInSec}

The time in seconds to wait for a database to start loading when under load.

- **Type**: `int`
- **Default**: `60`
- **Scope**: Server-wide only

Set how much time has to wait for the database to become available when too much different resources get loaded at the same time

{PANEL/}

{PANEL:Databases.MaxConcurrentLoads}

Specifies the maximum amount of databases that can be loaded simultaneously.

- **Type**: `int`
- **Default**: `8`
- **Scope**: Server-wide only

{PANEL/}

{PANEL:Databases.MaxIdleTimeInSec}

Set time in seconds for max idle time for database.

- **Type**: `int`
- **Default**: `900`
- **Scope**: Server-wide only

After this time, and idle database will be unloaded from memory. Use lower time period if memory resource limited

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
