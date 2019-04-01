# Changes API: How to subscribe to bulk insert operation changes?

`ForBulkInsert` method can be used to acquire notifications for bulk insert operations.

## Syntax

{CODE bulk_insert_changes_1@ClientApi\Changes\HowToSubscribeToBulkInsertChanges.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **operationId** | Guid | Id of bulk insert operation, generated when bulk insert operation is created. |

| ReturnValue | |
| ------------- | ----- |
| IObservableWithTask<[BulkInsertChangeNotification](../../glossary/bulk-insert-change-notification)> | Observable that allows to add subscriptions to notifications for bulk insert operation with given id. |

## Example

{CODE bulk_insert_changes_2@ClientApi\Changes\HowToSubscribeToBulkInsertChanges.cs /}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions](https://www.nuget.org/packages/Rx-Main) package to your project. /}

## Related articles

- [How to work with bulk insert operation?](../bulk-insert/how-to-work-with-bulk-insert-operation)
