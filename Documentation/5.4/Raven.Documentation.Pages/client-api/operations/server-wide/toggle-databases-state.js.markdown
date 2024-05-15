# Toggle Databases State Operation <br> (Enable / Disable)
---

{NOTE: }

* Use `ToggleDatabasesStateOperation` to enable/disable a single database or multiple databases.

* The database will be enabled/disabled on all nodes in the [database-group](../../../studio/database/settings/manage-database-group).

* In this page:

    * [Enable/Disable database from the Client API](../../../client-api/operations/server-wide/toggle-databases-state#enable/disable-database-from-the-client-api)
        * [Enable database](../../../client-api/operations/server-wide/toggle-databases-state#enable-database)
        * [Disable database](../../../client-api/operations/server-wide/toggle-databases-state#disable-database)
        * [Syntax](../../../client-api/operations/server-wide/toggle-databases-state#syntax)
    * [Disable database via the file system](../../../client-api/operations/server-wide/toggle-databases-state#disable-database-via-the-file-system)

{NOTE/}

---


{PANEL: Enable/Disable database from the Client API}

{NOTE: }

<a id="enable-database" /> **Enable database**:  

{CODE:nodejs enable@client-api\Operations\Server\toggleDatabasesState.js /}

{NOTE/}
{NOTE: }

<a id="disable-database" /> **Disable database**: 

{CODE:nodejs disable@client-api\Operations\Server\toggleDatabasesState.js /}

{NOTE/}
{NOTE: }

<a id="syntax" /> **Syntax**: 

{CODE:nodejs syntax_1@client-api\Operations\Server\toggleDatabasesState.js /}

| Parameter         | Type     | Description                                                                                 |
|-------------------|----------|---------------------------------------------------------------------------------------------|
| **databaseName**  | `string`   | Name of database for which to toggle state                                                |
| **databaseNames** | `string[]` | List of database names for which to toggle state                                          |
| **disable**       | `boolean`  | `true` - request to disable the database(s)<br>`false`- request to enable the database(s) |

{CODE:nodejs syntax_2@client-api\Operations\Server\toggleDatabasesState.js /}

{NOTE/}
{PANEL/}

{PANEL: Disable database via the file system}

It may sometimes be useful to disable a database manually, through the file system.

* To **manually disable** a database:

    * Place a file named `disable.marker` in the [database directory](../../../server/storage/directory-structure).
    * The `disable.marker` file can be empty,  
      and can be created by any available method, e.g. using the File Explorer, a terminal, or code.

* Attempting to use a manually disabled database will generate the following exception:

         Unable to open database: '{DatabaseName}', 
         it has been manually disabled via the file: '{disableMarkerPath}'. 
         To re-enable, remove the disable.marker and reload the database.

* To **enable** a manually disabled database:

    * First, remove the `disable.marker` file from the database directory.
    * Then, [reload the database](../../../studio/database/settings/database-settings#how-to-reload-the-database).

{PANEL/}

## Related Articles

### Operations
- [Database settings operations](../../../client-api/operations/maintenance/configuration/database-settings-operation)  

### Studio
- [Database-group](../../../studio/database/settings/manage-database-group)
