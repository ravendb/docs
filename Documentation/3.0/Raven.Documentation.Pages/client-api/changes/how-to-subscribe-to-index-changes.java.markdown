# Changes API: How to subscribe to index changes?

Following methods allow you to subscribe to index changes:

- [forIndex](../../client-api/changes/how-to-subscribe-to-index-changes#forindex)
- [forAllIndexes](../../client-api/changes/how-to-subscribe-to-index-changes#forallindexes)

{PANEL:ForIndex}

Index changes for one index can be observed using `forIndex` method.

### Syntax

{CODE:java index_changes_1@ClientApi\Changes\HowToSubscribeToIndexChanges.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | String | Name of an index for which notifications will be processed. |

| Return value | |
| ------------- | ----- |
| IObservable<[IndexChangeNotification](../../glossary/index-change-notification)> | Observable that allows to add subscriptions to notifications for index with given name. |

### Example

{CODE:java index_changes_2@ClientApi\Changes\HowToSubscribeToIndexChanges.java /}

{PANEL/}

{PANEL:ForAllIndexes}

Index changes for all indexes can be observed using `forAllIndexes` method.

| Return value | |
| ------------- | ----- |
| IObservable<[IndexChangeNotification](../../glossary/index-change-notification)> | Observable that allows to add subscriptions to notifications for all indexes. |

### Syntax

{CODE:java index_changes_3@ClientApi\Changes\HowToSubscribeToIndexChanges.java /}

### Example

{CODE:java index_changes_4@ClientApi\Changes\HowToSubscribeToIndexChanges.java /}

{PANEL/}
