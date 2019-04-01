# Changes API: How to subscribe to index changes?

Following methods allow you to subscribe to index changes:

- [ForIndex](../../client-api/changes/how-to-subscribe-to-index-changes#forindex)
- [ForAllIndexes](../../client-api/changes/how-to-subscribe-to-index-changes#forallindexes)

{PANEL:ForIndex}

Index changes for one index can be observed using `ForIndex` method.

### Syntax

{CODE index_changes_1@ClientApi\Changes\HowToSubscribeToIndexChanges.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | string | Name of an index for which notifications will be processed. |

| Return value | |
| ------------- | ----- |
| IObservableWithTask<[IndexChangeNotification](../../glossary/index-change-notification)> | Observable that allows to add subscriptions to notifications for index with given name. |

### Example

{CODE index_changes_2@ClientApi\Changes\HowToSubscribeToIndexChanges.cs /}

{PANEL/}

{PANEL:ForAllIndexes}

Index changes for all indexex can be observed using `ForAllIndexes` method.

| Return value | |
| ------------- | ----- |
| IObservableWithTask<[IndexChangeNotification](../../glossary/index-change-notification)> | Observable that allows to add subscriptions to notifications for all indexes. |

### Syntax

{CODE index_changes_3@ClientApi\Changes\HowToSubscribeToIndexChanges.cs /}

### Example

{CODE index_changes_4@ClientApi\Changes\HowToSubscribeToIndexChanges.cs /}

{PANEL/}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions](https://www.nuget.org/packages/Rx-Main) package to your project. /}

