# Changes API: How to Subscribe to Operation Changes

The following methods allow you to subscribe to operation changes:

- [ForOperationId](../../client-api/changes/how-to-subscribe-to-operation-changes#foroperationid)
- [ForAllOperations](../../client-api/changes/how-to-subscribe-to-operation-changes#foralloperations)

{PANEL:ForOperationId}

Operation changes for one operation can be observed using the `ForOperationId` method.

{NOTE: }
Please note that from RavenDB 6.1 on, operation changes can be tracked only on a **specific node**.  
The purpose of this change is to improve results consistency, as an operation may behave very differently 
on different nodes and cross-cluster tracking of an operation may become confusing and ineffective if 
the operation fails over from one node to another.  
Tracking operations will therefore be possible only if the `Changes` API was 
[opened](../../client-api/changes/what-is-changes-api#accessing-changes-api) using a method that limits 
tracking to a single node: `store.Changes(dbName, nodeTag)`  
{NOTE/}

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

{NOTE: }
Please note that from RavenDB 6.1 on, operation changes can be tracked only on a **specific node**.  
The purpose of this change is to improve results consistency, as an operation may behave very differently 
on different nodes and cross-cluster tracking of an operation may become confusing and ineffective if 
the operation fails over from one node to another.  
Tracking operations will therefore be possible only if the `Changes` API was 
[opened](../../client-api/changes/what-is-changes-api#accessing-changes-api) using a method that limits 
tracking to a single node: `store.Changes(dbName, nodeTag)`  
{NOTE/}

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
| **ShouldPersist** | bool | determine whether or not the result should be saved in the storage |
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

{WARNING: }
To get more method overloads, especially ones supporting **delegates**, please add the 
[System.Reactive.Core](https://www.nuget.org/packages/System.Reactive.Core/) package to your project.  
{WARNING/}

## Related Articles

### Changes API

- [What is Changes API](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
