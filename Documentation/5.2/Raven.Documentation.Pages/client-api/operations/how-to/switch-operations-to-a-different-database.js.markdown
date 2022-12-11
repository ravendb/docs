# Switch Operations to a Different Database

---

{NOTE: }

* By default, all operations work on the default database defined in the [document store](../../../client-api/creating-document-store).

* __To operate on a different database__, use the `forDatabase` method.  
  (An exception is thrown if that database doesn't exist on the server).

* In this page:
    * [Common operations - forDatabase](../../../client-api/operations/how-to/switch-operations-to-a-different-database#common-operations---fordatabase)
    * [Maintenance operations - forDatabase](../../../client-api/operations/how-to/switch-operations-to-a-different-database#maintenance-operations---fordatabase)
{NOTE/}

---

{PANEL: Common operations - forDatabase}

{CODE:nodejs for_database_1@ClientApi\Operations\HowTo\switchOperationsToDifferentDatabase.js /}

__Syntax__:

{CODE:nodejs syntax_1@ClientApi\Operations\HowTo\switchOperationsToDifferentDatabase.js /}

| Parameters | Type | Description |
| - | - | - |
| **databaseName** | string | Name of a database to operate on |

| Return Value | |
| - | - |
| `OperationExecutor` | New instance of Operation Executor that is scoped to the requested database |

{PANEL/}

{PANEL: Maintenance operations - forDatabase}

{CODE:nodejs for_database_2@ClientApi\Operations\HowTo\switchOperationsToDifferentDatabase.js /}

__Syntax__:

{CODE:nodejs syntax_2@ClientApi\Operations\HowTo\switchOperationsToDifferentDatabase.js /}

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
