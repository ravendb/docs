# Changes API : How to subscribe to operation changes?

Following methods allow you to subscribe to operation changes:

- [ForOperationId](../../client-api/changes/how-to-subscribe-to-operation-changes#foroperation)
- [ForAllOperations](../../client-api/changes/how-to-subscribe-to-operation-changes#foralloperations)

{PANEL:ForOperation}

Operation changes for one operation can be observed using `ForOperationId` method.

### Syntax

{CODE operation_changes_1@ClientApi\Changes\HowToSubscribeToOperationChanges.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **operationId** | long | Id of an operation for which notifications will be processed. |

| Return value | |
| ------------- | ----- |
| IChangesObservable<[OperationStatusChange](../../client-api/changes/how-to-subscribe-to-operation-changes#operationchange)> | Observable that allows to add subscriptions to notifications for operation with given id. |

### Example

{CODE operation_changes_2@ClientApi\Changes\HowToSubscribeToOperationChanges.cs /}

{PANEL/}

{PANEL:ForAllOperations}

Operations changes for all Operations can be observed using `ForAllOperations` method.

| Return value | |
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
| **OperationId** | long | Operation id |

<hr />
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
| **InProgress** | `Indicates that the operation made prgoress` |
| **Completed** | `Indicates that the operation had completed` |
| **Faulted** | `Indicates that the operation is faulted` |
| **Canceled** | `Indicates that the operation had been Canceled` |
{PANEL/}
## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions Core](https://www.nuget.org/packages/System.Reactive.Core/) package to your project. /}

