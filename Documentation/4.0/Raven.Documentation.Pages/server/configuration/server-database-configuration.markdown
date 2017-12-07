## Server Configuration : Database Options

<br><br>

#### Databases.OperationTimeoutInMin
###### The number of minutes to wait before canceling a database operation such as load (many) or query
###### Default Value: 5
If an operation exeeds the specified time, an *OperationCanceledException* will be thrown

Example:

```
Databases.OperationTimeoutInMin=8
```

<br><br>

#### Databases.ConcurrentResourceLoadTimeoutInSec
###### Set time in seconds to wait for a database to start loading when under load
###### Default Value: 10
Set how much time has to wait for the resource to become available when too much different resources get loaded at the same time

Example:

```
Databases.ConcurrentResourceLoadTimeoutInSec=30
```

<br><br>

#### Databases.MaxConcurrentResourceLoads
###### Specifies the maximum amount of databases that can be loaded simultaneously
###### Default Value: 8

Example:

```
Databases.MaxConcurrentResourceLoads=4
```

<br><br>

#### Databases.MaxIdleTimeForTenantDatabaseInSec
###### Set time in seconds for max idle time for database
###### Default Value: 900
After this time, and idle database will be unloaded from memory. Use lower time period if memory resource limited

Example:
```
Databases.MaxIdleTimeForTenantDatabaseInSec=300
```

<br><br>

#### Databases.FrequencyToCheckForIdleDatabasesInSec
###### The time in seconds to check for an idle tenant database
###### Default Value: 60

Example:
```
Databases.FrequencyToCheckForIdleDatabasesInSec=15
```
