# Switch Operations to a Different Database

---

{NOTE: }

* By default, all operations work on the default database defined in the [Document Store](../../../client-api/creating-document-store).

* **To operate on a different database**, use the `ForDatabase` method.  
  If the requested database doesn't exist on the server, an exception will be thrown.

* In this page:
    * [Common operation: `Operations.ForDatabase`](../../../client-api/operations/how-to/switch-operations-to-a-different-database#common-operation:-operations.fordatabase)
    * [Maintenance operation: `Maintenance.ForDatabase`](../../../client-api/operations/how-to/switch-operations-to-a-different-database#maintenance-operation:-maintenance.fordatabase)
{NOTE/}

---

{PANEL: Common operation: `Operations.ForDatabase`}

* For reference, all common operations are listed [here](../../../client-api/operations/what-are-operations#common-operations).

{CODE for_database_1@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.cs /}

---

**Syntax**:

{CODE syntax_1@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.cs /}

| Parameters | Type | Description |
| - | - | - |
| **databaseName** | `string` | Name of the database to operate on |

| Return Value | Description |
| - | - |
| `OperationExecutor` | New instance of Operation Executor that is scoped to the requested database |

{PANEL/}

{PANEL: Maintenance operation: `Maintenance.ForDatabase`}

* For reference, all maintenance operations are listed [here](../../../client-api/operations/what-are-operations#maintenance-operations).

{CODE for_database_2@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.cs /}

---

**Syntax**:

{CODE syntax_2@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.cs /}

| Parameters | Type | Description |
| - | - | - |
| **databaseName** | `string` | Name of the database to operate on |

| Return Value | Description |
| - | - |
| `MaintenanceOperationExecutor` | New instance of Maintenance Operation Executor that is scoped to the requested database |

{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
