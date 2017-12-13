## Server Configuration : Database Options

<br><br>

#### Databases.QueryOperationTimeoutInSec
###### The number of seconds to wait before canceling a query operation
###### Default Value: 300
If an operation exeeds the specified time, an *OperationCanceledException* will be thrown

Example:

```
Databases.QueryOperationTimeoutInSec=600
```

<br><br>

#### Databases.IndexTermsOperationTimeoutInSec
###### The number of seconds to wait before canceling an indexing terms operation
###### Default Value: 300
If an operation exeeds the specified time, an *OperationCanceledException* will be thrown

Example:

```
Databases.IndexTermsOperationTimeoutInSec=600
```

<br><br>

#### Databases.DeleteDocsOperationTimeoutInSec
###### The number of seconds to wait before canceling a documents deletion operation
###### Default Value: 300
If an operation exeeds the specified time, an *OperationCanceledException* will be thrown

Example:

```
Databases.DeleteDocsOperationTimeoutInSec=600
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
