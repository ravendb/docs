# Get Connection String Operation
---

{NOTE: }

* Use `GetConnectionStringsOperation` to retrieve properties for a specific connection string  
  or for all connection strings defined in the databse.

* To learn how to create a new connection string, see [Add Connection String Operation](../../../../client-api/operations/maintenance/connection-strings/add-connection-string).

* In this page:
  * [Get connection string by name and type](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#get-connection-string-by-name-and-type)
  * [Get all connection strings](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#get-all-connnection-strings)  
  * [Syntax](../../../../client-api/operations/maintenance/connection-strings/get-connection-string#syntax)

{NOTE/}

---

{PANEL: Get connection string by name and type}

The following example retrieves a RavenDB Connection String:

{CODE get_connection_string_by_name@ClientApi\Operations\Maintenance\ConnectionStrings\GetConnectionStrings.cs /}

{PANEL/}

{PANEL: Get all connnection strings}

{CODE get_all_connection_strings@ClientApi\Operations\Maintenance\ConnectionStrings\GetConnectionStrings.cs /}

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Operations\Maintenance\ConnectionStrings\GetConnectionStrings.cs /}

| Parameter                | Type                   | Description                                                                    |
|--------------------------|------------------------|--------------------------------------------------------------------------------|
| **connectionStringName** | `string`               | Connection string name                                                         |
| **type**                 | `ConnectionStringType` | Connection string type:<br>`Raven`, `Sql`, `Olap`, `ElasticSearch`, or `Queue` |

{CODE syntax_2@ClientApi\Operations\Maintenance\ConnectionStrings\GetConnectionStrings.cs /}

| Return value of `store.Maintenance.Send(GetConnectionStringsOperation)`  |                                                               |
|--------------------------------------------------------------------------|---------------------------------------------------------------|
| `GetConnectionStringsResult`                                             | Class with all connection strings are defined on the database |

{CODE syntax_3@ClientApi\Operations\Maintenance\ConnectionStrings\GetConnectionStrings.cs /}

{NOTE: }
A detailed syntax for each connection string type is available in article [Add connection string](../../../../client-api/operations/maintenance/connection-strings/add-connection-string).
{NOTE/}

{PANEL/}

## Related Articles

### Connection Strings

- [Add](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)
- [Remove](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string)
