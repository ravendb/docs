# Switch Operations to a Different Database

---

{NOTE: }

* By default, all operations work on the default database defined in the [document store](../../../client-api/creating-document-store).

* **To operate on a different database**, use the `forDatabase` method.  
  If the requested database doesn't exist on the server, an exception will be thrown.

* In this page:
    * [Common operation: `operations.forDatabase`](../../../client-api/operations/how-to/switch-operations-to-a-different-database#common-operation:-operations.fordatabase)
    * [Maintenance operation: `maintenance.forDatabase`](../../../client-api/operations/how-to/switch-operations-to-a-different-database#maintenance-operation:-maintenance.fordatabase)
{NOTE/}

---

{PANEL: Common operation: `operations.forDatabase`}

* For reference, all common operations are listed [here](../../../client-api/operations/what-are-operations#common-operations).

{CODE:nodejs for_database_1@client-api\Operations\HowTo\switchOperationsToDifferentDatabase.js /}

---

**Syntax**:

{CODE:nodejs syntax_1@client-api\Operations\HowTo\switchOperationsToDifferentDatabase.js /}

| Parameters | Type | Description |
| - | - | - |
| **databaseName** | `string` | Name of the database to operate on |

| Return Value | Description |
| - | - |
| `OperationExecutor` | New instance of Operation Executor that is scoped to the requested database |

{PANEL/}

{PANEL: Maintenance operation: `maintenance.forDatabase`}

* For reference, all maintenance operations are listed [here](../../../client-api/operations/what-are-operations#maintenance-operations).

{CODE:nodejs for_database_2@client-api\Operations\HowTo\switchOperationsToDifferentDatabase.js /}

---

**Syntax**:

{CODE:nodejs syntax_2@client-api\Operations\HowTo\switchOperationsToDifferentDatabase.js /}

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
