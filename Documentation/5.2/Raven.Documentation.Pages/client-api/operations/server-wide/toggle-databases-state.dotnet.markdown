# Toggle Databases State Operation <br> (Enable / Disable)
---

{NOTE: }

* Use `ToggleDatabasesStateOperation` to enable/disable a single database or multiple databases.

* The database will be enabled/disabled on all nodes in the [database-group](../../../studio/database/settings/manage-database-group).

* In this page:

  * [Enable Database](../../../client-api/operations/server-wide/toggle-databases-state#enable-database)
  * [Disable Database](../../../client-api/operations/server-wide/toggle-databases-state#disable-database)
  * [Syntax](../../../client-api/operations/server-wide/toggle-databases-state#syntax)
  * [Disable a Database Manually](../../../client-api/operations/server-wide/toggle-databases-state#disable-a-database-manually)

{NOTE/}

---

{PANEL: Enable Database}

{CODE-TABS}
{CODE-TAB:csharp:Sync enable@ClientApi\Operations\Server\ToggleDatabasesState.cs /}
{CODE-TAB:csharp:Async enable_async@ClientApi\Operations\Server\ToggleDatabasesState.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Disable Database}

{CODE-TABS}
{CODE-TAB:csharp:Sync disable@ClientApi\Operations\Server\ToggleDatabasesState.cs /}
{CODE-TAB:csharp:Async disable_async@ClientApi\Operations\Server\ToggleDatabasesState.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Operations\Server\ToggleDatabasesState.cs /}

| Parameter         | Type     | Description                                                                               |
|-------------------|----------|-------------------------------------------------------------------------------------------|
| __databaseName__  | string   | Name of database for which to toggle state                                                |
| __databaseNames__ | string[] | List of database names for which to toggle state                                          |
| __disable__       | bool     | `true` - request to disable the database(s)<br>`false`- request to enable the database(s) |

{CODE syntax_2@ClientApi\Operations\Server\ToggleDatabasesState.cs /}

{PANEL/}

{PANEL: Disable a Database Manually}

A database can be **manually disabled** by keeping a `disable.marker` file in its file-system path.  
{CODE:csharp disable-database-via-file-system@ClientApi\Operations\Server\ToggleDatabasesState.cs /}

Accessing the database will be prevented until `disable.marker` is removed and the database 
is reloaded, with the following exception:  
`Unable to open database: '{store.Database}', it has been manually disabled via the file: '{disableMarkerPath}'. 
To re-enable, remove the disable.marker and reload the database.`  

{PANEL/}

## Related Articles

### Operations
- [Database settings operations](../../../client-api/operations/maintenance/configuration/database-settings-operation)  

### Studio
- [Database-group](../../../studio/database/settings/manage-database-group)
