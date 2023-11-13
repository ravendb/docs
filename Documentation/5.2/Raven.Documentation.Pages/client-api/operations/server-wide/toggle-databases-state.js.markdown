# Toggle Databases State Operation <br> (Enable / Disable)
---

{NOTE: }

* Use `ToggleDatabasesStateOperation` to enable/disable a single database or multiple databases.

* The database will be enabled/disabled on all nodes in the [database-group](../../../studio/database/settings/manage-database-group).

* In this page:

  * [Enable database](../../../client-api/operations/server-wide/toggle-databases-state#enable-database)
  * [Disable database](../../../client-api/operations/server-wide/toggle-databases-state#disable-database)
  * [Syntax](../../../client-api/operations/server-wide/toggle-databases-state#syntax)

{NOTE/}

---

{PANEL: Enable database}

{CODE:nodejs enable@ClientApi\Operations\Server\toggleDatabasesState.js /}

{PANEL/}

{PANEL: Disable database}

{CODE:nodejs disable@ClientApi\Operations\Server\toggleDatabasesState.js /}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@ClientApi\Operations\Server\toggleDatabasesState.js /}

| Parameter         | Type     | Description                                                                               |
|-------------------|----------|-------------------------------------------------------------------------------------------|
| __databaseName__  | string   | Name of database for which to toggle state                                                |
| __databaseNames__ | string[] | List of database names for which to toggle state                                          |
| __disable__       | boolean  | `true` - request to disable the database(s)<br>`false`- request to enable the database(s) |

{CODE:nodejs syntax_2@ClientApi\Operations\Server\toggleDatabasesState.js /}

{PANEL/}

## Related Articles

### Operations
- [Database settings operations](../../../client-api/operations/maintenance/configuration/database-settings-operation)  

### Studio
- [Database-group](../../../studio/database/settings/manage-database-group)
