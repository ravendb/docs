# Client API : How to subscribe to bulk insert operation changes?

`ForBulkInsert` method can be used to aquire notifications for bulk insert operations.

## Syntax

{CODE bulk_insert_changes_1@ClientApi\Changes\HowToSubscribeToBulkInsertChanges.cs /}

**Parameters**   

operationId
:   Type: Guid   
Id of bulk insert operation, generated when bulk insert operation is created.

**Return value**

Type: IObservableWithTask<[BulkInsertChangeNotification](../../glossary/client-api/changes/bulk-insert-change-notification)>   
Observable that allows to add subscribtions to notifications for bulk insert operation with given id.

## Example

{CODE bulk_insert_changes_2@ClientApi\Changes\HowToSubscribeToBulkInsertChanges.cs /}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions](http://nuget.org/packages/Rx-Main) package to your project. /}

#### Related articles

TODO