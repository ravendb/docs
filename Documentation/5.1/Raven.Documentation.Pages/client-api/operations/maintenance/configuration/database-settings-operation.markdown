# Apply Database Configuration Settings

---

{NOTE: }

Use the following methods to change, save, and apply database configuration settings. 

* `PutDatabaseSettingsOperation` sets and saves new database configurations.
* `ToggleDatabasesStateOperation` disables/enables database.  
  This is used after `PutDatabaseSettingsOperation` to apply new settings because configurations are loaded on startup. See [code sample](../../../../client-api/operations/maintenance/configuration/database-settings-operation#toggledatabasesstateoperation) below. 
* `GetDatabaseSettingsOperation` shows a dictionary of newly configured settings.

{INFO: }

* Database settings can also be [configured via the Studio](../../../../studio/database/settings/database-settings#database-settings)

* After editing & saving a configuration, the change does not take effect 
  until the database is [disabled and enabled again](../../../../client-api/operations/maintenance/configuration/database-settings-operation#toggledatabasesstateoperation).  

{INFO/}

{WARNING: Warning}
Do not modify the database settings unless you are an expert and know what you're doing.  
{WARNING/}

In this page:

  * [PutDatabaseSettingsOperation](../../../../client-api/operations/maintenance/configuration/database-settings-operation#putdatabasesettingsoperation)
  * [ToggleDatabasesStateOperation](../../../../client-api/operations/maintenance/configuration/database-settings-operation#toggledatabasesstateoperation)
  * [GetDatabaseSettingsOperation](../../../../client-api/operations/maintenance/configuration/database-settings-operation#getdatabasesettingsoperation)

{NOTE/}

{PANEL: PutDatabaseSettingsOperation}

This method sets and saves new database configurations.  
To apply the new settings, use [ToggleDatabasesStateOperation](../../../../client-api/operations/maintenance/configuration/database-settings-operation#toggledatabasesstateoperation).  

**Syntax:**

{CODE signature-PutDatabaseSettingsOperation@ClientApi\Operations\ClientConfig.cs /}

| Parameters | Type | Description |
| -------- | ---- | -------------------|
| databaseName | `string` | Select database to change settings for. |
| configurationSettings | Dictionary<string, string> | Pass configuration settings. |

---

**Sample:** 

The following sample shows how to use `PutDatabaseSettingsOperation` to set the database configuration. 
{CODE ApplyDatabaseSettings-PutDatabaseSettingsOperation@ClientApi\Operations\ClientConfig.cs /}


{PANEL/}

{PANEL: ToggleDatabasesStateOperation}

Use `ToggleDatabasesStateOperation` to disable/enable the database, e.g. to apply new settings after [PutDatabaseSettingsOperation](../../../../client-api/operations/maintenance/configuration/database-settings-operation#putdatabasesettingsoperation) is called.

**Syntax:**

{CODE signature-ToggleDatabasesStateOperation@ClientApi\Operations\ClientConfig.cs /}

 | Parameters | Type | Description |
| -------- | ---- | -------------------|
| databaseName | `string` | Database name. |
| disable | `bool` | `true` - disable the database <br> `false`" - enable the database |

---

**Sample:** 

The following sample uses `ToggleDatabasesStateOperation` to disable and then enable the database, effectively reloading 
the database to apply a new configuration.  

{CODE ApplyDatabaseSettings-PutDatabaseSettingsOperation@ClientApi\Operations\ClientConfig.cs /}


{PANEL/}

{PANEL: GetDatabaseSettingsOperation}

Use `GetDatabaseSettingsOperation` to retrieve a dictionary of the changes made in the database settings. Only settings that have changed will be retrieved.

**Syntax:**

{CODE signature-GetDatabaseSettingsOperation@ClientApi\Operations\ClientConfig.cs /}

 | Parameter | Type | Description |
| -------- | ---- | -------------------|
| databaseName | `string` | Database name whose settings you want to retrieve. |

  | Return Type | Description |
 | ---- | -------------------|
 | `DatabaseSettings` | A key/value dictionary of database settings. <br> Only settings that have changed are retrieved. |

`DatabaseSettings`:

{CODE DatabaseSettingsDefinition@ClientApi\Operations\ClientConfig.cs /}

---

**Sample:** 

Get a dictionary of newly configured settings with `GetDatabaseSettingsOperation`.

{CODE SeeNewDatabaseSettings-GetDatabaseSettingsOperation@ClientApi\Operations\ClientConfig.cs /}


{PANEL/}



## Related Articles

### Studio

- [Client Configuration](../../../../studio/server/client-configuration)
- [Database Settings](../../../../studio/database/settings/database-settings#database-settings)

### Operations

- [What are Operations](../../../../client-api/operations/what-are-operations)
- [How to Get Client Configuration](../../../../client-api/operations/maintenance/configuration/get-client-configuration)
- [How to Get Server-Wide Client Configuration](../../../../client-api/operations/server-wide/configuration/get-serverwide-client-configuration)
- [How to Put Server-Wide Client Configuration](../../../../client-api/operations/server-wide/configuration/put-serverwide-client-configuration)
