# Changes API: How to Subscribe to Operation Changes

The following methods allow you to subscribe to operation changes:

- [forOperationId()](../../client-api/changes/how-to-subscribe-to-operation-changes#foroperation)
- [forAllOperations()](../../client-api/changes/how-to-subscribe-to-operation-changes#foralloperations)

{PANEL:forOperation}

Operation changes for one operation can be observed using the `forOperationId()` method.

### Syntax

{CODE:nodejs operation_changes_1@client-api\changes\howToSubscribeToOperationChanges.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **operationId** | number | ID of an operation for which notifications will be processed. |

| Return value | |
| ------------- | ----- |
| IChangesObservable<[OperationStatusChange](../../client-api/changes/how-to-subscribe-to-operation-changes#operationchange)> | Observable that allows you to add subscriptions to notifications for an operation with a given ID. |

### Example

{CODE:nodejs operation_changes_2@client-api\changes\howToSubscribeToOperationChanges.js /}

{PANEL/}

{PANEL:forAllOperations}

Operations changes for all Operations can be observed using the `forAllOperations()` method.

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[OperationStatusChange](../../client-api/changes/how-to-subscribe-to-operation-changes#operationchange)> | Observable that allows to add subscriptions to notifications for all operations. |

### Syntax

{CODE:nodejs operation_changes_3@client-api\changes\howToSubscribeToOperationChanges.js /}

### Example

{CODE:nodejs operation_changes_4@client-api\changes\howToSubscribeToOperationChanges.js /}

{PANEL/}

{PANEL:OperationChange}

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **state** | object | Operation state |
| **operationId** | number | Operation ID |

{PANEL/}


## Related Articles

### Changes API

- [What is Changes API](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
