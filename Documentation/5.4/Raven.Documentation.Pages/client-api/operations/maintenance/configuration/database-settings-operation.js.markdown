# Database Settings Operations

---

{NOTE: }
  
* The default database configuration settings can be customized:

  * From the Client API - as described in this article.

  * From Studio - via the [Database Settings](../../../../studio/database/settings/database-settings#database-settings) view.

* In this page:  

  * [Put database settings operation](../../../../client-api/operations/maintenance/configuration/database-settings-operation#put-database-settings-operation)

  * [Get database settings operation](../../../../client-api/operations/maintenance/configuration/database-settings-operation#get-database-settings-operation)

{WARNING: }
Do not modify the database settings unless you are an expert and know what you're doing.  
{WARNING/}

{NOTE/}

{PANEL: Put database settings operation}

* Use `PutDatabaseSettingsOperation` to modify the default database configuration.

* Only **database-level** settings can be customized using this operation.  
  See the [Configuration overview](../../../../server/configuration/configuration-options) article to learn how to customize the **server-level** settings.  

* Note: for the changes to take effect, the database must be **reloaded**.  
  Reloading is accomplished by disabling and enabling the database using [ToggleDatabasesStateOperation](../../../../client-api/operations/server-wide/toggle-databases-state).  
  See the following example:

{CODE:nodejs put_database_settings@client-api\Operations\Maintenance\Configuration\databaseSettings.js /}

---

**Syntax**:

{CODE:nodejs syntax_1@client-api\Operations\Maintenance\Configuration\databaseSettings.js /}

| Parameter             | Type      | Description                                        |
|-----------------------|-----------|----------------------------------------------------|
| databaseName          | `string`  | Name of database for which to change the settings. |
| configurationSettings | `object` | The configuration settings to set.                 |


{PANEL/}

{PANEL: Get database settings operation}

* Use `GetDatabaseSettingsOperation` to get the configuration settings that were customized for the database.  

* Only settings that have been changed will be retrieved.

{CODE:nodejs get_database_settings@client-api\Operations\Maintenance\Configuration\databaseSettings.js /}

---

**Syntax**:

{CODE:nodejs syntax_2@client-api\Operations\Maintenance\Configuration\databaseSettings.js /}

| Parameter    | Type     | Description                                                 |
|--------------|----------|-------------------------------------------------------------|
| databaseName | `string` | The database name for which to get the customized settings. |


{CODE:nodejs syntax_3@client-api\Operations\Maintenance\Configuration\databaseSettings.js /}

{PANEL/}

## Related Articles

### Studio

- [Database settings view](../../../../studio/database/settings/database-settings#database-settings)

### Operations

- [What are Operations](../../../../client-api/operations/what-are-operations)
- [How to Get Client Configuration](../../../../client-api/operations/maintenance/configuration/get-client-configuration)
- [How to Get Server-Wide Client Configuration](../../../../client-api/operations/server-wide/configuration/get-serverwide-client-configuration)
- [Toggle database state](../../../../client-api/operations/server-wide/toggle-databases-state)
