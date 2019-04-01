# Changes API: How to Subscribe to Index Changes

Following methods allow you to subscribe to index changes:

- [forIndex()](../../client-api/changes/how-to-subscribe-to-index-changes#forindex)
- [forAllIndexes()](../../client-api/changes/how-to-subscribe-to-index-changes#forallindexes)

{PANEL:forIndex}

Index changes for one index can be observed using `forIndex()` method.

### Syntax

{CODE:nodejs index_changes_1@client-api\changes\howToSubscribeToIndexChanges.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **indexName** | string | Name of an index for which notifications will be processed. |

| Return value | |
| ------------- | ----- |
| IChangesObservable<[IndexChange](../../client-api/changes/how-to-subscribe-to-index-changes#indexchange)> | Observable that allows to add subscriptions to notifications for index with given name. |

### Example

{CODE:nodejs index_changes_2@client-api\changes\howToSubscribeToIndexChanges.js /}

{PANEL/}

{PANEL:forAllIndexes}

Index changes for all indexex can be observed using `forAllIndexes()` method.

| Return value | |
| ------------- | ----- |
| IChangesObservable<[IndexChange](../../client-api/changes/how-to-subscribe-to-index-changes#indexchange)> | Observable that allows to add subscriptions to notifications for all indexes. |

### Syntax

{CODE:nodejs index_changes_3@client-api\changes\howToSubscribeToIndexChanges.js /}

### Example

{CODE:nodejs index_changes_4@client-api\changes\howToSubscribeToIndexChanges.js /}

{PANEL/}

{PANEL:IndexChange}

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **type** | [IndexChangeTypes](../../client-api/changes/how-to-subscribe-to-index-changes#indexchangetypes) | Change type |
| **name** | string | Index name |
| **etag** | number | Index Etag |

{PANEL/}

{PANEL:IndexChangeTypes}

| Name |
| ---- |
| **None** |
| **BatchCompleted** |
| **IndexAdded** |
| **IndexRemoved** |
| **IndexDemotedToIdle** |
| **IndexPromotedToIdle** |
| **IndexDemotedToDisabled** |
| **IndexMarkedAsErrored** |
| **SideBySideReplace** |
| **Renamed** |
| **IndexPaused** |
| **LockModeChanged** |
| **PriorityChanged** |

{PANEL/}

## Related Articles

### Changes API

- [What is Changes API](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)
