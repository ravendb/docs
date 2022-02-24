# Apply Database Configuration Settings

---

{NOTE: }

Use the following methods to change, save, and apply database configuration settings. 

* `PutDatabaseSettingsOperation` sets and saves new database configurations.
* `ToggleDatabasesStateOperation` disables/enables database.  
  This is used after `PutDatabaseSettingsOperation` to apply new settings because configurations are loaded on startup.  
  Database reboot is required for changes to take effect. See [code sample](../../../../client-api/operations/maintenance/configuration/database-settings-operation#toggledatabasesstateoperation-sample) below. 
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
  * [PutDatabaseSettingsOperation Sample](../../../../client-api/operations/maintenance/configuration/database-settings-operation#putdatabasesettingsoperation-sample)
  * [ToggleDatabasesStateOperation Sample](../../../../client-api/operations/maintenance/configuration/database-settings-operation#toggledatabasesstateoperation-sample)
  * [GetDatabaseSettingsOperation Sample](../../../../client-api/operations/maintenance/configuration/database-settings-operation#getdatabasesettingsoperation-sample)

{NOTE/}

{PANEL: Syntax}

---

### `PutDatabaseSettingsOperation`

See code sample for [PutDatabaseSettingsOperation()](../../../../client-api/operations/maintenance/configuration/database-settings-operation#putdatabasesettingsoperation-sample)  

{CODE signature-PutConfigurationSettings@ClientApi\Operations\ClientConfig.cs /}

| Parameters | Type | Description |
| -------- | ---- | -------------------|
| Database name | `string` | Select database to change settings for. |
| Configuration Settings | Dictionary<string, string> | Pass configuration settings. |

---


### `ToggleDatabasesStateOperation`

See code sample for [ToggleDatabasesStateOperation()](../../../../client-api/operations/maintenance/configuration/database-settings-operation#toggledatabasesstateoperation-sample)  

{CODE signature-ToggleDatabasesStateOperation@ClientApi\Operations\ClientConfig.cs /}

 | Parameters | Type | Description |
| -------- | ---- | -------------------|
| Database name | `string` | Select database to change settings for. |
| disable | `bool` | Expected state of database. |

 | Return Type | Description |
 | ---- | -------------------|
 | `DisableDatabaseToggleResult` | The result of a disable or enable database. |

---

### `GetDatabaseSettingsOperation`

See code sample for [GetDatabaseSettingsOperation()](../../../../client-api/operations/maintenance/configuration/database-settings-operation#getdatabasesettingsoperation-sample) 

{CODE signature-GetDatabaseSettingsOperation@ClientApi\Operations\ClientConfig.cs /}

 | Parameter | Type | Description |
| -------- | ---- | -------------------|
| Database name | `string` | Select database to view its new settings. |

  | Return Type | Description |
 | ---- | -------------------|
 | `DatabaseSettings` | View new database configuration settings |



 {PANEL/}

{PANEL: PutDatabaseSettingsOperation Sample}

The following sample shows how to use `PutDatabaseSettingsOperation` in the context of a generic database configuration. This method sets and saves new database configurations. 
To apply the new settings, use [ToggleDatabasesStateOperation](../../../../client-api/operations/maintenance/configuration/database-settings-operation#toggledatabasesstateoperation-sample).  

{CODE ApplyDatabaseSettings-PutDatabaseSettingsOperation@ClientApi\Operations\ClientConfig.cs /}

{PANEL/}

{PANEL: ToggleDatabasesStateOperation Sample}

The following sample shows how to apply new database configurations with `ToggleDatabasesStateOperation`.  

  * The first line should give the argument `true` so that the database is disabled.  
  * The next line should give the argument `false` to enable the database, thus applying the changes that were saved.

{CODE ApplyDatabaseSettings-PutDatabaseSettingsOperation@ClientApi\Operations\ClientConfig.cs /}

{PANEL/}

{PANEL: GetDatabaseSettingsOperation Sample}

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
