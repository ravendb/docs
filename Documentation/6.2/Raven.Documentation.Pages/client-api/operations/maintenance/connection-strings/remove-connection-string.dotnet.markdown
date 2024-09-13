# Remove Connection String Operation
---

{NOTE: }

* Use `RemoveConnectionStringOperation` to remove a connection string definition from the database.

* In this page:
  * [Remove connection string](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string#remove-connecion-string)
  * [Syntax](../../../../client-api/operations/maintenance/connection-strings/remove-connection-string#syntax)

{NOTE/}

---

{PANEL: Remove connection string}

The following example removes a RavenDB Connection String.

{CODE remove_raven_connection_string@ClientApi\Operations\Maintenance\ConnectionStrings\RemoveConnectionStrings.cs /}

{PANEL/}

{PANEL: Syntax}

{CODE remove_connection_string@ClientApi\Operations\Maintenance\ConnectionStrings\RemoveConnectionStrings.cs /}

| Parameter            | Type  | Description                                                                                                                                                              |
|----------------------|-------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **connectionString** | `T`   | Connection string to remove:<br>`RavenConnectionString`<br>`SqlConnectionString`<br>`OlapConnectionString`<br>`ElasticSearchConnectionString`<br>`QueueConnectionString` |

{PANEL/}

## Related Articles

### Connection Strings

- [Get](../../../../client-api/operations/maintenance/connection-strings/get-connection-string)
- [Add](../../../../client-api/operations/maintenance/connection-strings/add-connection-string)
