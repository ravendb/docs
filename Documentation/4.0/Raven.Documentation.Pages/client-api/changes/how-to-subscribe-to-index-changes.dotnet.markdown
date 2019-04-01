# Changes API: How to Subscribe to Index Changes

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
| IChangesObservable<[IndexChange](../../client-api/changes/how-to-subscribe-to-index-changes#indexchange)> | Observable that allows to add subscriptions to notifications for index with given name. |

### Example

{CODE index_changes_2@ClientApi\Changes\HowToSubscribeToIndexChanges.cs /}

{PANEL/}

{PANEL:ForAllIndexes}

Index changes for all indexex can be observed using `ForAllIndexes` method.

| Return value | |
| ------------- | ----- |
| IChangesObservable<[IndexChange](../../client-api/changes/how-to-subscribe-to-index-changes#indexchange)> | Observable that allows to add subscriptions to notifications for all indexes. |

### Syntax

{CODE index_changes_3@ClientApi\Changes\HowToSubscribeToIndexChanges.cs /}

### Example

{CODE index_changes_4@ClientApi\Changes\HowToSubscribeToIndexChanges.cs /}

{PANEL/}

{PANEL:IndexChange}

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Type** | [IndexChangeTypes](../../client-api/changes/how-to-subscribe-to-index-changes#indexchangetypes) | Change type |
| **Name** | string | Index name |
| **Etag** | long? | Index Etag |

{PANEL/}

{PANEL:IndexChangeTypes}

| Name | Value |
| ---- | ----- |
| **None** | `0` |
| **BatchCompleted** | `1` |
| **IndexAdded** | `8` |
| **IndexRemoved** | `16` |
| **IndexDemotedToIdle** | `32` |
| **IndexPromotedFromIdle** | `64` |
| **IndexDemotedToDisabled** | `256` |
| **IndexMarkedAsErrored** | `512` |
| **SideBySideReplace** | `1024` |
| **Renamed** | `2048` |
| **IndexPaused** | `4096` |
| **LockModeChanged** | `8192` |
| **PriorityChanged** | `16384` |

{PANEL/}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions Core](https://www.nuget.org/packages/System.Reactive.Core/) package to your project. /}

## Related Articles

### Changes API

- [What is Changes API](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)
