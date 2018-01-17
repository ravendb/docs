## Server Configuration : Database

{PANEL:Databases.QueryTimeoutInSec}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Databases.QueryTimeoutInSec | The time in seconds to wait before canceling query | `300` | Server-wide or per database |

This timeout refers to queries and streamed queries.

If a query exceeds the specified time, an *OperationCanceledException* will be thrown. For the streaming queries the timeout is delayed every time a
query result is pushed to the stream. The timeout will be exceeded only if no result is streamed within that time.

{PANEL/}

{PANEL:Databases.QueryOperationTimeoutInSec}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Databases.QueryOperationTimeoutInSec | The time in seconds to wait before canceling query related operation (patch/delete query) | `300` | Server-wide or per database |

{PANEL/}

{PANEL:Databases.OperationTimeoutInSec}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Databases.OperationTimeoutInSec | The time in seconds to wait before canceling specific operations (such as indexing terms) | `300` | Server-wide or per database |

{PANEL/}

{PANEL:Databases.CollectionOperationTimeoutInSec}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Databases.CollectionOperationTimeoutInSec | The time in seconds to wait before canceling several collection operations (such as batch delete documents from studio) | `300` | Server-wide or per database |

Set timeout for some operations on collections (such as batch delete documents from studio) requiring their own timeout settings.

If an operation exceeds the specified time, an *OperationCanceledException* will be thrown

{PANEL/}

{PANEL:Databases.ConcurrentLoadTimeoutInSec}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Databases.ConcurrentLoadTimeoutInSec | The time in seconds to wait for a database to start loading when under load | `10` | Server-wide or per database |

Set how much time has to wait for the database to become available when too much different resources get loaded at the same time

{PANEL/}

{PANEL:Databases.MaxConcurrentLoads}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Databases.MaxConcurrentLoads | Specifies the maximum amount of databases that can be loaded simultaneously | `8` | Server-wide or per database |

{PANEL/}

{PANEL:Databases.MaxIdleTimeInSec}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Databases.MaxIdleTimeInSec | Set time in seconds for max idle time for database | `900` | Server-wide or per database |

After this time, and idle database will be unloaded from memory. Use lower time period if memory resource limited

{PANEL/}

{PANEL:Databases.FrequencyToCheckForIdleDatabasesInSec}

| Configuration Key | Description | Default | Scope |
|:------------------|:------------|:--------|:------|
| Databases.FrequencyToCheckForIdleDatabasesInSec | The time in seconds to check for an idle tenant database | `60` | Server-wide or per database |

{PANEL/}
