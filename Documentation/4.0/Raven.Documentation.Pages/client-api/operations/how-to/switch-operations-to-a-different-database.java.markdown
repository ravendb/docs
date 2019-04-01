# Operations: How to Switch Operations to a Different Database

By default, the operations available directly in store are working on a default database that was setup for that store. To switch operations to a different database that is available on that server use the **forDatabase** method.

{PANEL:Operations.forDatabase}

{CODE:java for_database_1@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **databaseName** | String | Name of a database for which you want to get new Operations |

| Return Value | |
| ------------- | ----- |
| OperationExecutor | New instance of Operations that is scoped to the requested database |

### Example

{CODE:java for_database_3@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.java /}

{PANEL/}

## How to Switch Maintenance Operations to a Different Database

As with `operations`, by default the `maintenance` operations available directly in store are working on a default database that was setup for that store. To switch maintenance operations to a different database use the **forDatabase** method.

{PANEL:Maintenance.forDatabase}

{CODE:java for_database_2@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **databaseName** | String | Name of a database for which you want to get new maintenance operations |

| Return Value | |
| ------------- | ----- |
| MaintenanceOperationExecutor | New instance of maintenance that is scoped to the requested database |

### Example

{CODE:java for_database_4@ClientApi\Operations\HowTo\SwitchOperationsToDifferentDatabase.java /}

{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
