# Changes API: How to Subscribe to Time Series Changes

The following methods allow you to subscribe to time series changes:

* [ForTimeSeries - Track All Time Series with a Given Name](../../client-api/changes/how-to-subscribe-to-time-series-changes#fortimeseries)
* [ForTimeSeriesOfDocument](../../client-api/changes/how-to-subscribe-to-time-series-changes#fortimeseriesofdocument)
    * [Track a Specific Time Series of a Chosen Document](../../client-api/changes/how-to-subscribe-to-time-series-changes#overload-1)  
    * [Track Any Time Series of a Chosen Document](../../client-api/changes/how-to-subscribe-to-time-series-changes#overload-2)  
* [ForAllTimeSeries - Track All Time Series](../../client-api/changes/how-to-subscribe-to-time-series-changes#foralltimeseries)

{PANEL: ForTimeSeries}

Subscribe to changes in **all time series with a given name**, no matter which document they belong to, 
using the `ForTimeSeries` method.  

#### Syntax

{CODE timeseries_changes_1@ClientApi\Changes\HowToSubscribeToTimeSeriesChanges.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **timeSeriesName** | string | Name of a time series to subscribe to. |

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[TimeSeriesChange](../../client-api/changes/how-to-subscribe-to-time-series-changes#timeserieschange)> | Observable that allows to add subscriptions to time series notifications. |

#### Example

{CODE timeseries_changes_1_1@ClientApi\Changes\HowToSubscribeToTimeSeriesChanges.cs /}

{PANEL/}

{PANEL: ForTimeSeriesOfDocument}

Subscribe to changes in **time series of a chosen document** using the `ForCountersOfDocument` method.  

---

### Overload #1
Use this `ForTimeSeriesOfDocument` overload to track changes in a **specific time** series of the chosen document.  

#### Syntax

{CODE timeseries_changes_2@ClientApi\Changes\HowToSubscribeToTimeSeriesChanges.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentId** | string | ID of a document to subscribe to. |
| **timeSeriesName** | string | Name of a time series to subscribe to. |

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[TimeSeriesChange](../../client-api/changes/how-to-subscribe-to-time-series-changes#timeserieschange)> | Observable that allows to add subscriptions to time series notifications. |

#### Example

{CODE timeseries_changes_2_1@ClientApi\Changes\HowToSubscribeToTimeSeriesChanges.cs /}

---

### Overload #2
Use this `ForTimeSeriesOfDocument` overload to track changes in **any time series** of the chosen document.  

#### Syntax

{CODE timeseries_changes_3@ClientApi\Changes\HowToSubscribeToTimeSeriesChanges.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentId** | string | ID of a document to subscribe to. |

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[TimeSeriesChange](../../client-api/changes/how-to-subscribe-to-time-series-changes#timeserieschange)> | Observable that allows to add subscriptions to time series notifications. |

#### Example

{CODE timeseries_changes_3_1@ClientApi\Changes\HowToSubscribeToTimeSeriesChanges.cs /}

{PANEL/}

{PANEL: ForAllTimeSeries}

Subscribe to changes in **all time series** using the `ForAllTimeSeries` method.  

#### Syntax

{CODE timeseries_changes_4@ClientApi\Changes\HowToSubscribeToTimeSeriesChanges.cs /}

| Return Value | |
| ------------- | ----- |
| IChangesObservable<[TimeSeriesChange](../../client-api/changes/how-to-subscribe-to-time-series-changes#timeserieschange)> | Observable that allows to add subscriptions to time series notifications. |

#### Example

{CODE timeseries_changes_4_1@ClientApi\Changes\HowToSubscribeToTimeSeriesChanges.cs /}

{PANEL/}

{PANEL:TimeSeriesChange}

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Type** | [TimeSeriesChangeTypes](../../client-api/changes/how-to-subscribe-to-time-series-changes#timeserieschangetypes) | Time series change type enum |
| **Name** | string | Time Series Name |
| **DocumentId** | string | Time series Document Identifier |
| **CollectionName** | string | Time series document Collection Name |
| **From** | DateTime | Time series values From date |
| **To** | DateTime | Time series values To date |
| **ChangeVector** | string | Time series Change Vector |

{PANEL/}

{PANEL:TimeSeriesChangeTypes}

| Name | Value |
| ---- | ----- |
| **None** | `0` |
| **Put** | `1` |
| **Delete** | `2` |
| **Mixed** | `3` |

{PANEL/}

## Remarks

{INFO To get more method overloads, especially the ones supporting delegates, please add [Reactive Extensions Core](https://www.nuget.org/packages/System.Reactive.Core/) package to your project. /}

## Related Articles

### Changes API

- [What is Changes API](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)
