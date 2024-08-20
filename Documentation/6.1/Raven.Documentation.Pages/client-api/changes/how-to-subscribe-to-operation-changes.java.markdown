# Changes API: How to Subscribe to Operation Changes

The following methods allow you to subscribe to operation changes:

- [forOperationId](../../client-api/changes/how-to-subscribe-to-operation-changes#foroperation)
- [forAllOperations](../../client-api/changes/how-to-subscribe-to-operation-changes#foralloperations)

{PANEL:forOperation}

Operation changes for one operation can be observed using the `forOperationId` method.

### Syntax

{CODE:java operation_changes_1@ClientApi\Changes\HowToSubscribeToOperationChanges.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **operationId** | long | ID of an operation for which notifications will be processed. |

| Return value | |
| ------------- | ----- |
| IChangesObservable<[OperationStatusChange](../../client-api/changes/how-to-subscribe-to-operation-changes#operationchange)> | Observable that allows you to add subscriptions to notifications for an operation with a given ID. |

### Example

{CODE:java operation_changes_2@ClientApi\Changes\HowToSubscribeToOperationChanges.java /}

{PANEL/}

{PANEL:forAllOperations}

Operations changes for all Operations can be observed using the `forAllOperations` method.

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[OperationStatusChange](../../client-api/changes/how-to-subscribe-to-operation-changes#operationchange)> | Observable that allows to add subscriptions to notifications for all operations. |

### Syntax

{CODE:java operation_changes_3@ClientApi\Changes\HowToSubscribeToOperationChanges.java /}

### Example

{CODE:java operation_changes_4@ClientApi\Changes\HowToSubscribeToOperationChanges.java /}

{PANEL/}

{PANEL:OperationChange}

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **State** | ObjectNode | Operation state |
| **OperationId** | long | Operation ID |

{PANEL/}


## Related Articles

### Changes API

- [What is Changes API](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
