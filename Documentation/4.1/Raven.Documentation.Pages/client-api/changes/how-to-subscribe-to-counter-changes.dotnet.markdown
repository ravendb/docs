# Changes API: How to Subscribe to Counter Changes

Following methods allow you to subscribe to counter changes:

- [ForCounter](../../client-api/changes/how-to-subscribe-to-counter-changes#forcounter)
- [ForCounterOfDocument](../../client-api/changes/how-to-subscribe-to-counter-changes#forcounterofdocument)
- [ForCountersOfDocument](../../client-api/changes/how-to-subscribe-to-counter-changes#forcountersofdocument)
- [ForAllCounters](../../client-api/changes/how-to-subscribe-to-counter-changes#forallcounters)

{PANEL:ForCounter}

Counter changes can be observed using `ForCounter` method. This will subscribe changes from all counters with a given name, no matter in what document counter was changed.

### Syntax

{CODE counter_changes_1@ClientApi\Changes\HowToSubscribeToCounterChanges.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **counterName** | string | Name of a counter to subscribe to. |

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[CounterChange](../../client-api/changes/how-to-subscribe-to-counter-changes#counterchange)> | Observable that allows to add subscriptions to counter notifications. |

### Example

{CODE counter_changes_1_1@ClientApi\Changes\HowToSubscribeToCounterChanges.cs /}

{PANEL/}

{PANEL:ForCounterOfDocument}

Specific counter changes of a given document can be observed using `ForCounterOfDocument` method.

### Syntax

{CODE counter_changes_2@ClientApi\Changes\HowToSubscribeToCounterChanges.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentId** | string | ID of a document to subscribe to. |
| **counterName** | string | Name of a counter to subscribe to. |

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[CounterChange](../../client-api/changes/how-to-subscribe-to-counter-changes#counterchange)> | Observable that allows to add subscriptions to counter notifications. |

### Example

{CODE counter_changes_2_1@ClientApi\Changes\HowToSubscribeToCounterChanges.cs /}

{PANEL/}

{PANEL:ForCountersOfDocument}

Counter changes of a specified document can be observed using `ForCountersOfDocument` method.

### Syntax

{CODE counter_changes_3@ClientApi\Changes\HowToSubscribeToCounterChanges.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentId** | string | ID of a document to subscribe to. |

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[CounterChange](../../client-api/changes/how-to-subscribe-to-counter-changes#counterchange)> | Observable that allows to add subscriptions to counter notifications. |

### Example

{CODE counter_changes_3_1@ClientApi\Changes\HowToSubscribeToCounterChanges.cs /}

{PANEL/}

{PANEL:ForAllCounters}

Changes for all counters can be observed using `ForAllCounters` method.

### Syntax

{CODE counter_changes_4@ClientApi\Changes\HowToSubscribeToCounterChanges.cs /}

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[CounterChange](../../client-api/changes/how-to-subscribe-to-counter-changes#counterchange)> | Observable that allows to add subscriptions to counter notifications. |

### Example

{CODE counter_changes_4_1@ClientApi\Changes\HowToSubscribeToCounterChanges.cs /}

{PANEL/}

{PANEL:CounterChange}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Type** | [CounterChangeTypes](../../client-api/changes/how-to-subscribe-to-counter-changes#counterchangetypes) | Counter change type enum |
| **Name** | string | Counter name |
| **Value** | long | Counter value after the change |
| **DocumentId** | string | Counter document identifier |
| **ChangeVector** | string | Counter's ChangeVector|

{PANEL/}

{PANEL:CounterChangeTypes}

| Name | Value |
| ---- | ----- |
| **None** | `0` |
| **Put** | `1` |
| **Delete** | `2` |
| **Increment** | `4` |

{PANEL/}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions Core](https://www.nuget.org/packages/System.Reactive.Core/) package to your project. /}

## Related Articles

### Changes API

- [What is Changes API](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)
