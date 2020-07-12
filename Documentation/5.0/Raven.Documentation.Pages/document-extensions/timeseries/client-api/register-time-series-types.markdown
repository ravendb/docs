# Registering Time Series Types

{NOTE: }

* Register your [time series types](../../../document-extensions/timeseries/client-api/strongly-typed-api) 
  with the [database record](../../../studio/database/settings/database-record), so the Studio would be able 
  to present your time series values by their names.  

* In this page:  
   * [Registering a Time Series Type](../../../document-extensions/timeseries/client-api/register-time-series-types#registering-a-time-series-type)  
   * [Syntax](../../../document-extensions/timeseries/client-api/register-time-series-types#syntax)  
   * [Usage Samples](../../../document-extensions/timeseries/client-api/register-time-series-types#usage-sample)  

{NOTE/}

---

{PANEL: Registering a Time Series Type}

Registering a custom time series type with the database record acquaints 
this type to the Studio, so when you view and manage time series values 
via the Studio they would be presented by name.  

{PANEL/}

{PANEL: Syntax}
To register a time series type, call `store.TimeSeries.Register`.  

* `store.TimeSeries.Register` definition  
   {CODE Register-Definitions@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* Parameters  

       | Parameter | Type | Explanation 
       | --- | --- | --- |
       | `TCollection` | Collection type | The time-series' collection  
       | `collection` | `string` | collection (when `TCollection` is not provided)
       | `TTimeSeriesEntry` | Time series type | The custom time-series type 
       | `name` | `string ` | Time series name 
       | `valueNames` | `string[]` | Names (name per value) 

{PANEL/}

{PANEL: Usage Sample}

* Here, we define and register a `RoutePoint` type.
  {CODE Custom-Data-Type-2@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
  {CODE timeseries_region_Strongly-Typed-Register@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
* And this is the Studio **Time Series view** after appending a few RoutePoint coordinates.
!["Studio Time Series View"](images/time-series-view-coordinates.png "Studio Time Series View")
{PANEL/}

## Related articles

**Time Series Overview**  
[Time Series Overview](../../../document-extensions/timeseries/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../document-extensions/timeseries/rollup-and-retention)  

**Time Series Types**  
[Strongly-Types API](../../../document-extensions/timeseries/client-api/strongly-typed-api)  
