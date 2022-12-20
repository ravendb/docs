# Switch Operations to a Different Database

---

{NOTE: }

* By default, all operations work on the default database defined in the [Document Store](../../../client-api/creating-document-store).

* __To operate on a different database__, use the `ForDatabase` method.  
  (An exception is thrown if that database doesn't exist on the server).

* In this page:
    * [Common operations - ForDatabase](../../../client-api/operations/how-to/switch-operations-to-a-different-database#common-operations---fordatabase)
    * [Maintenance operations - ForDatabase](../../../client-api/operations/how-to/switch-operations-to-a-different-database#maintenance-operations---fordatabase)
{NOTE/}

---

{PANEL: Common operations - ForDatabase}

* For reference, all common operations are listed [here](../../../client-api/operations/what-are-operations#common-operations).

{CODE for_database_1@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.cs /}

__Syntax__:

{CODE syntax_1@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.cs /}

| Parameters | Type | Description |
| - | - | - |
| **databaseName** | string | Name of the database to operate on |

| Return Value | |
| - | - |
| `OperationExecutor` | New instance of Operation Executor that is scoped to the requested database |

{PANEL/}

{PANEL: Maintenance operations - ForDatabase}

* For reference, all maintenance operations are listed [here](../../../client-api/operations/what-are-operations#maintenance-operations).

{CODE for_database_2@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.cs /}

__Syntax__:

{CODE syntax_2@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.cs /}

| Parameters | Type | Description |
| - | - | - |
| **databaseName** | string | Name of the database to operate on |

| Return Value | |
| - | - |
| `MaintenanceOperationExecutor` | New instance of Maintenance Operation Executor that is scoped to the requested database |

{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
