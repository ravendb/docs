# Switch Operations to a Different Database

---

{NOTE: }

* By default, all operations work on the default database defined in the [Document Store](../../../client-api/creating-document-store).

* To operate on a different database, use the `ForDatabase` method.  
  (An exception is thrown if that database doesn't exist on the server).

* In this page:
    * [Common operations](../../../client-api/operations/how-to/switch-operations-to-a-different-database#common-operations)
    * [Maintenance operations](../../../client-api/operations/how-to/switch-operations-to-a-different-database#maintenance-operations)
{NOTE/}

---

{PANEL: Common operations}

{CODE for_database_1@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.cs /}

__Syntax__:

{CODE syntax_1@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.cs /}

| Parameters | Type | Description |
| - | - | - |
| **databaseName** | string | Name of a database to operate on |

| Return Value | |
| - | - |
| `OperationExecutor` | New instance of Operation Executor that is scoped to the requested database |

{PANEL/}

{PANEL: Maintenance operations}

{CODE for_database_2@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.cs /}

__Syntax__:

{CODE syntax_2@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.cs /}

| Parameters | Type | Description |
| - | - | - |
| **databaseName** | string | Name of a database to operate on |

| Return Value | |
| - | - |
| `MaintenanceOperationExecutor` | New instance of Maintenance Operation Executor that is scoped to the requested database |

{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
