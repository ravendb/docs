# Apply Database Configuration Settings

* `PutDatabaseSettingsOperation` saves new database settings.
* `ToggleDatabasesStateOperation` disables/enables database to apply new settings because changes 
  only take effect when a database is loading or enabled.  See [code sample](../../../../client-api/operations/maintenance/configuration/database-settings-operation#toggledatabasesstateoperation) below. 
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

| Method | Type | Arguments | Description |
| -------- | ---- | ------ | -------------------|
| `PutDatabaseSettingsOperation` | `string` | `store.Database, settings` | Saves new database settings |
| `ToggleDatabasesStateOperation` | `bool` | `store.Database, disable: true` | Disables database |
| `ToggleDatabasesStateOperation` | `bool` | `store.Database, disable: false` | Enables (reloads) database to apply saved settings |
| `GetDatabaseSettingsOperation` | `string` | `store.Database`, `Assert.NotNull(settings)` | Shows newly configured settings |

### Using Statements

{CODE using@ClientApi\Operations\ClientConfig.cs /}

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
