# Changes API: How to Subscribe to Index Changes

Following methods allow you to subscribe to index changes:

- [forIndex](../../client-api/changes/how-to-subscribe-to-index-changes#forindex)
- [forAllIndexes](../../client-api/changes/how-to-subscribe-to-index-changes#forallindexes)

{PANEL:forIndex}

Index changes for one index can be observed using `forIndex` method.

### Syntax

{CODE:java index_changes_1@ClientApi\Changes\HowToSubscribeToIndexChanges.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | String | Name of an index for which notifications will be processed. |

| Return value | |
| ------------- | ----- |
| IChangesObservable<[IndexChange](../../client-api/changes/how-to-subscribe-to-index-changes#indexchange)> | Observable that allows to add subscriptions to notifications for index with given name. |

### Example

{CODE:java index_changes_2@ClientApi\Changes\HowToSubscribeToIndexChanges.java /}

{PANEL/}

{PANEL:forAllIndexes}

Index changes for all indexex can be observed using `forAllIndexes` method.

| Return value | |
| ------------- | ----- |
| IChangesObservable<[IndexChange](../../client-api/changes/how-to-subscribe-to-index-changes#indexchange)> | Observable that allows to add subscriptions to notifications for all indexes. |

### Syntax

{CODE:java index_changes_3@ClientApi\Changes\HowToSubscribeToIndexChanges.java /}

### Example

{CODE:java index_changes_4@ClientApi\Changes\HowToSubscribeToIndexChanges.java /}

{PANEL/}

{PANEL:IndexChange}

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Type** | [IndexChangeTypes](../../client-api/changes/how-to-subscribe-to-index-changes#indexchangetypes) | Change type |
| **Name** | String | Index name |
| **Etag** | Long | Index Etag |

{PANEL/}

{PANEL:IndexChangeTypes}

| Name |
| ---- |
| **NONE** |
| **BATCH_COMPLETED** |
| **INDEX_ADDED** |
| **INDEX_REMOVED** |
| **INDEX_DEMOTED_TO_IDLE** |
| **INDEX_PROMOTED_TO_IDLE** |
| **INDEX_DEMOTED_TO_DISABLED** |
| **INDEX_MARKED_AS_ERRORED** |
| **SIDE_BY_SIDE_REPLACE** |
| **RENAMED** |
| **INDEX_PAUSED** |
| **LOCK_MODE_CHANGED** |
| **PRIORITY_CHANGED** |

{PANEL/}

## Related Articles

### Changes API

- [What is Changes API](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)
