# Named Time Series Values

{NOTE: }

* Define time series types and name their values, so various time series methods (like `Append` and `Get`) 
  would be able to address the values by their names, and help you create a clearer code.  
* Register your time series types with the [database record](../../../studio/database/settings/database-record), 
  so the Studio would be able to present your time series values by their names.  

* In this page:  
  * [Named Values](../../../document-extensions/timeseries/client-api/named-time-series-values#named-values)  
     * [Defining a Time Series Type](../../../document-extensions/timeseries/client-api/named-time-series-values#defining-a-time-series-type)  
     * [Usage Samples](../../../document-extensions/timeseries/client-api/named-time-series-values#usage-samples)  
  * [Registering a Time Series Type](../../../document-extensions/timeseries/client-api/named-time-series-values#registering-a-time-series-type)  
     * [Syntax](../../../document-extensions/timeseries/client-api/named-time-series-values#syntax)  
     * [Usage Sample](../../../document-extensions/timeseries/client-api/named-time-series-values#usage-sample)  

{NOTE/}

---

{PANEL: Named Values}

Many time series are populated with multiple values with each measurement.  
Each GPS measurement, for example, would be appended to a route-tracking 
time series with two values at least: the latitude and the longitude.  

* You can ease the management of multi-value time series by -  
   * Naming time series values in model classes that can be used as time series types.  
   * Calling time series methods with your custom types, to address and manage values by name.  

---

#### Defining a Time Series Type

To define a class that can be used as a time series type, mark the class 
properties (values) with consecutive TimeSeriesValue indexes: TimeSeriesValue[0], 
TimeSeriesValue[1], etc.  

E.g, -  
{CODE Custom-Data-Type-2@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

The class can then be used by time series methods like Append.  
{CODE timeseries_region_Append-Named-Values-1@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

---

#### Usage Samples

* In this sample we define a StockPrice type, and use it while appending StockPrice entries.
  {CODE Custom-Data-Type-1@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
  {CODE timeseries_region_Append-Named-Values-2@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* Here we Get StockPrice values by name, to check whether a stock's closing-time price is ascending over time.
   {CODE timeseries_region_Get-Named-Values@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* In this query we use the custom StockPrice type, so we can address trade Volume by name.
   {CODE timeseries_region_Named-Values-Query@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: Registering a Time Series Type}

Registering a custom time series type with the database record acquaints 
this type to the Studio, so when you view and manage time series values 
via the Studio they would be presented by name.  

---

#### Syntax

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

---

#### Usage Sample

* Here, we define and register a `RoutePoint` type.
  {CODE Custom-Data-Type-2@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
  {CODE timeseries_region_Named-Values-Register@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
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
