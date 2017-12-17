## Server Configuration : Database Options

<br><br>

#### Databases.QueryTimeoutInSec
###### The time in seconds to wait before canceling query
###### Default Value: 300

This timeout refers to queries and streamed queries operations.

If an operation exeeds the specified time, an *OperationCanceledException* will be thrown

Example:

```
Databases.QueryTimeoutInSec=600
```

<br><br>

#### Databases.QueryOperationTimeout
###### The time in seconds to wait before canceling query related operation
###### Default Value: 300

This timeout refers to patch/delete query operations.

If an operation exeeds the specified time, an *OperationCanceledException* will be thrown

Example:

```
Databases.QueryOperationTimeout=600
```

<br><br>

#### Databases.OperationTimeout
###### The time in seconds to wait before canceling specific operations
###### Default Value: 300

Set timeout for some general operations (such as indexing terms) requirring their own timeout settings.

If an operation exeeds the specified time, an *OperationCanceledException* will be thrown

Example:

```
Databases.OperationTimeout=600
```

<br><br>

#### Databases.CollectionOperationTimeoutInSec
###### The time in seconds to wait before canceling several collection operations
###### Default Value: 300

Set timeout for some operations on collections (such as batch delete documents from studio) requirring their own timeout settings.

If an operation exeeds the specified time, an *OperationCanceledException* will be thrown

Example:

```
Databases.CollectionOperationTimeoutInSec=600
```

<br><br>


#### Databases.ConcurrentDatabaseLoadTimeoutInSec
###### Set time in seconds to wait for a database to start loading when under load
###### Default Value: 10
Set how much time has to wait for the database to become available when too much different resources get loaded at the same time

Example:

```
Databases.ConcurrentDatabaseLoadTimeoutInSec=30
```

<br><br>

#### Databases.MaxConcurrentDatabaseLoads
###### Specifies the maximum amount of databases that can be loaded simultaneously
###### Default Value: 8

Example:

```
Databases.MaxConcurrentDatabaseLoads=4
```

<br><br>

#### Databases.MaxIdleTimeForDatabaseInSec
###### Set time in seconds for max idle time for database
###### Default Value: 900
After this time, and idle database will be unloaded from memory. Use lower time period if memory resource limited

Example:
```
Databases.MaxIdleTimeForDatabaseInSec=300
```

<br><br>

#### Databases.FrequencyToCheckForIdleDatabasesInSec
###### The time in seconds to check for an idle tenant database
###### Default Value: 60

Example:
```
Databases.FrequencyToCheckForIdleDatabasesInSec=15
```
