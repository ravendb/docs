# Changes API: How to subscribe to bulk insert operation changes?

`forBulkInsert` method can be used to acquire notifications for bulk insert operations.

## Syntax

{CODE:java bulk_insert_changes_1@ClientApi\Changes\HowToSubscribeToBulkInsertChanges.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **operationId** | UUID | Id of bulk insert operation, generated when bulk insert operation is created. |

| ReturnValue | |
| ------------- | ----- |
| IObservable<[BulkInsertChangeNotification](../../glossary/bulk-insert-change-notification)> | Observable that allows to add subscriptions to notifications for bulk insert operation with given id. |

## Example

{CODE:java bulk_insert_changes_2@ClientApi\Changes\HowToSubscribeToBulkInsertChanges.java /}

## Related articles

- [How to work with bulk insert operation?](../bulk-insert/how-to-work-with-bulk-insert-operation)
