# Changes API: How to Subscribe to Time Series Changes

---

{NOTE: }

* Use the following methods to subscribe to Time Series Changes:  
   * `ForTimeSeries`  
     Track **all** time series with a given name  
   * `ForTimeSeriesOfDocument`  
     Overload #1: Track **a specific** time series of a chosen document  
     Overload #2: Track **any** time series of a chosen document  
   * `ForAllTimeSeries`  
     Track **all** time series  

* In this page:  
   * [ForTimeSeries](../../client-api/changes/how-to-subscribe-to-time-series-changes#fortimeseries)
   * [ForTimeSeriesOfDocument](../../client-api/changes/how-to-subscribe-to-time-series-changes#fortimeseriesofdocument)
   * [ForAllTimeSeries](../../client-api/changes/how-to-subscribe-to-time-series-changes#foralltimeseries)

{NOTE/}

---




{PANEL: ForTimeSeries}

Subscribe to changes in **all time-series with a given name**, no matter which document they belong to, 
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

Use `ForTimeSeriesOfDocument` to subscribe to changes in **time series of a chosen document**.  

* Two overload methods allow you to  
   * Track **a specific** time series of the chosen document  
   * Track **any** time series of the chosen document  

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

Subscribe to changes in **all time-series** using the `ForAllTimeSeries` method.  

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

{WARNING: }
To get more method overloads, especially ones supporting **delegates**, please add the 
[System.Reactive.Core](https://www.nuget.org/packages/System.Reactive.Core/) package to your project.  
{WARNING/}

## Related Articles

### Changes API

- [What is Changes API](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Document Changes](../../client-api/changes/what-is-changes-api)
- [How to Subscribe to Index Changes](../../client-api/changes/how-to-subscribe-to-index-changes)
- [How to Subscribe to Operation Changes](../../client-api/changes/how-to-subscribe-to-operation-changes)
