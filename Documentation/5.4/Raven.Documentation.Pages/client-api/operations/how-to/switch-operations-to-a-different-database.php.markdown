# Switch Operations to a Different Database

---

{NOTE: }

* By default, all operations work on the default database defined in the [Document Store](../../../client-api/creating-document-store).

* **To operate on a different database**, use the `for_database` method.  
  If the requested database doesn't exist on the server, an exception will be thrown.

* In this page:
    * [Common operation: `forDatabase`](../../../client-api/operations/how-to/switch-operations-to-a-different-database#common-operation:-fordatabase)
    * [Maintenance operation: `maintenance.for_database`](../../../client-api/operations/how-to/switch-operations-to-a-different-database#maintenance-operation:-maintenance.for_database)
{NOTE/}

---

{PANEL: Common operation: `forDatabase`}

* For reference, all common operations are listed [here](../../../client-api/operations/what-are-operations#common-operations).

{CODE:php for_database_1@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.php /}

---

**Syntax**:

{CODE:php syntax_1@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.php /}

| Parameters | Type | Description |
| - | - | - |
| **$databaseName** | `?string` | Name of the database to operate on |

| Return Value | Description |
| - | - |
| `OperationExecutor` | New instance of Operation Executor that is scoped to the requested database |

{PANEL/}

{PANEL: Maintenance operation: `maintenance.for_database`}

* For reference, all maintenance operations are listed [here](../../../client-api/operations/what-are-operations#maintenance-operations).

{CODE:php for_database_2@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.php /}

---

**Syntax**:

{CODE:php syntax_2@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.php /}

| Parameters | Type | Description |
| - | - | - |
| **$databaseName** | `?string` | Name of the database to operate on |

| Return Value | Description |
| - | - |
| `MaintenanceOperationExecutor` | New instance of Maintenance Operation Executor that is scoped to the requested database |

{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
