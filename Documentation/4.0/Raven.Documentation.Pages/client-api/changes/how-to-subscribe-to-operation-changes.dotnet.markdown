# Changes API: How to Subscribe to Operation Changes

The following methods allow you to subscribe to operation changes:

- [ForOperationId](../../client-api/changes/how-to-subscribe-to-operation-changes#foroperation)
- [ForAllOperations](../../client-api/changes/how-to-subscribe-to-operation-changes#foralloperations)

{PANEL:ForOperation}

Operation changes for one operation can be observed using the `ForOperationId` method.

### Syntax

{CODE operation_changes_1@ClientApi\Changes\HowToSubscribeToOperationChanges.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **operationId** | long | ID of an operation for which notifications will be processed. |

| Return value | |
| ------------- | ----- |
| IChangesObservable<[OperationStatusChange](../../client-api/changes/how-to-subscribe-to-operation-changes#operationchange)> | Observable that allows you to add subscriptions to notifications for an operation with a given ID. |

### Example

{CODE operation_changes_2@ClientApi\Changes\HowToSubscribeToOperationChanges.cs /}

{PANEL/}

{PANEL:ForAllOperations}

Operations changes for all Operations can be observed using the `ForAllOperations` method.

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[OperationStatusChange](../../client-api/changes/how-to-subscribe-to-operation-changes#operationchange)> | Observable that allows to add subscriptions to notifications for all operations. |

### Syntax

{CODE operation_changes_3@ClientApi\Changes\HowToSubscribeToOperationChanges.cs /}

### Example

{CODE operation_changes_4@ClientApi\Changes\HowToSubscribeToOperationChanges.cs /}

{PANEL/}

{PANEL:OperationChange}

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **State** | [OperationState](../../client-api/changes/how-to-subscribe-to-operation-changes#operationstate) | Operation state |
| **OperationId** | long | Operation ID |

{PANEL/}

{PANEL:OperationState}

### Members

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Result** | [IOperationResult](../../client-api/changes/how-to-subscribe-to-operation-changes#operationresult) | Operation result |
| **Progress** | IOperationProgress| Instance of IOperationProgress (json representation of the progress) |
| **Status** | [OperationStatus](../../client-api/changes/how-to-subscribe-to-operation-changes#operationstatus) | Operation status |
{PANEL/}

{PANEL:OperationResult}

### Members

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Message** | string | Operation message |
| **ShouldPersist** | bool | determine weather or not the result should be saved in the storage |
{PANEL/}

{PANEL:OperationStatus}

# OperationStatus (enum)

| Name | Description |
| ---- | ----- |
| **InProgress** | `Indicates that the operation made progress` |
| **Completed** | `Indicates that the operation has completed` |
| **Faulted** | `Indicates that the operation is faulted` |
| **Canceled** | `Indicates that the operation has been Canceled` |
{PANEL/}
## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add the [Reactive Extensions Core](https://www.nuget.org/packages/System.Reactive.Core/) package to your project. /}

## Related Articles

### Changes API

- [What is Changes API](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
