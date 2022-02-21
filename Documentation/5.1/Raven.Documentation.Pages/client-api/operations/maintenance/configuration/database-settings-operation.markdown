# Apply Database Configuration Settings

* `PutDatabaseSettingsOperation` saves new database settings.
* `ToggleDatabasesStateOperation` disables/enables database. This can be used to apply new settings because configurations 
  are loaded on startup. A database reboot is required for changes to take effect. See [code sample](../../../../client-api/operations/maintenance/configuration/database-settings-operation#toggledatabasesstateoperation) below. 
  * The first line should give the argument `true` so that the database is disabled.  
  * The next line should give the argument `false` to enable the database, thus applying the changes that were saved.
* `GetDatabaseSettingsOperation` shows a dictionary of newly configured settings.

* Database settings can also be [configured via the Studio](../../../../studio/database/settings/database-settings#database-settings)

* After editing & saving a configuration, the change does not take effect 
  until the database is [disabled and enabled again](../../../../client-api/operations/maintenance/configuration/database-settings-operation#toggledatabasesstateoperation).  

{WARNING: Warning}
Do not modify the database settings unless you are an expert and know what you're doing.  
{WARNING/}

In this page:

* [Syntax](../../../../client-api/operations/maintenance/configuration/database-settings-operation#syntax)
  * [PutDatabaseSettingsOperation](../../../../client-api/operations/maintenance/configuration/database-settings-operation#putdatabasesettingsoperation)
  * [ToggleDatabasesStateOperation](../../../../client-api/operations/maintenance/configuration/database-settings-operation#toggledatabasesstateoperation)
  * [GetDatabaseSettingsOperation](../../../../client-api/operations/maintenance/configuration/database-settings-operation#getdatabasesettingsoperation)

## Syntax

{NOTE: }
[PutDatabaseSettingsOperation()](../../../../client-api/operations/maintenance/configuration/database-settings-operation#putdatabasesettingsoperation)  

`public PutDatabaseSettingsOperation(string databaseName, Dictionary<string, string> configurationSettings)`

| Parameters | Type | Description |
| -------- | ---- | -------------------|
| Database name | `string` | Select database to change settings for. |
| Configuration Settings | Dictionary<string, string> | Pass configuration settings. |


 {NOTE/}

{NOTE: }

[ToggleDatabasesStateOperation()](../../../../client-api/operations/maintenance/configuration/database-settings-operation#toggledatabasesstateoperation)  

`public ToggleDatabasesStateOperation(string databaseName, bool disable)`  

 | Parameters | Type | Description |
| -------- | ---- | -------------------|
| Database name | `string` | Select database to change settings for. |
| disable: `true` | `bool` | Disable database. |
| disable: `false` | `bool` | Enable database - applies new configurations. |

 | Return Type | Description |
 | ---- | -------------------|
 | `DisableDatabaseToggleResult` | The result of a disable or enable database. |

{NOTE/}

{NOTE: }

[GetDatabaseSettingsOperation()](../../../../client-api/operations/maintenance/configuration/database-settings-operation#getdatabasesettingsoperation)  

`public GetDatabaseSettingsOperation(string databaseName)`

 | Parameters | Type | Description |
| -------- | ---- | -------------------|
| Database name | `string` | Select database to view its new settings. |

  | Return Type | Description |
 | ---- | -------------------|
 | `DatabaseSettings` | View new database configuration settings |

 {NOTE/}

{PANEL: PutDatabaseSettingsOperation}

The following sample shows how to use `PutDatabaseSettingsOperation` in the context of a generic database configuration.  

{CODE ApplyDatabaseSettings-PutDatabaseSettingsOperation@ClientApi\Operations\ClientConfig.cs /}

{PANEL/}

{PANEL: ToggleDatabasesStateOperation}

The following sample shows how to apply new database configurations with `ToggleDatabasesStateOperation`.  

  * The first line should give the argument `true` so that the database is disabled.  
  * The next line should give the argument `false` to enable the database, thus applying the changes that were saved.

{CODE ApplyDatabaseSettings-PutDatabaseSettingsOperation@ClientApi\Operations\ClientConfig.cs /}

{PANEL/}

{PANEL: GetDatabaseSettingsOperation}

The following sample shows a dictionary of newly configured settings with `GetDatabaseSettingsOperation`.

{CODE SeeNewDatabaseSettings-GetDatabaseSettingsOperation@ClientApi\Operations\ClientConfig.cs /}

{PANEL/}



## Related Articles

### Studio

- [Client Configuration](../../../../studio/server/client-configuration)

### Operations

- [What are Operations](../../../../client-api/operations/what-are-operations)
- [How to Get Client Configuration](../../../../client-api/operations/maintenance/configuration/get-client-configuration)
- [How to Get Server-Wide Client Configuration](../../../../client-api/operations/server-wide/configuration/get-serverwide-client-configuration)
- [How to Put Server-Wide Client Configuration](../../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration)
