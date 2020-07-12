# Strongly-Typed API

{NOTE: }

* Use the strongly-typed API to define time series types that can be used 
  by the time series methods (e.g. Append and Get) to address and manage 
  time series values by name.

* In this page:  
  * [The Strongly-Typed API](../../../document-extensions/timeseries/client-api/strongly-typed-api#the-strongly-typed-api)  
  * [Defining a Time Series Type](../../../document-extensions/timeseries/client-api/strongly-typed-api#defining-a-time-series-type)  
  * [Usage Samples](../../../document-extensions/timeseries/client-api/strongly-typed-api#usage-samples)  

{NOTE/}

---

{PANEL: The Strongly-Typed API}

Many time series are populated with multiple values with each measurement.  
Each GPS measurement, for example, would be appended to a route-tracking 
time series with two values at least: the latitude and the longitude.  

Using the strongly-typed API to define time series types, and addressing 
values by name, can create a clearer code and ease its management.  

* Name time series values in model classes that can be used as time series types.  
* Call time series methods with your custom types, to address and manage values by name.  

{PANEL/}

{PANEL: Defining a Time Series Type}

To define a class that can be used as a time series type, mark the class 
properties (values) with consecutive TimeSeriesValue indexes: TimeSeriesValue[0], 
TimeSeriesValue[1], etc.  

E.g, -  
{CODE Custom-Data-Type-2@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

The class can then be used by time series methods like Append.  
{CODE timeseries_region_Append-Strongly-Typed-1@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: Usage Samples}

* In this sample we define a StockPrice type, and then use it while 
  appending StockPrice entries.
  {CODE Custom-Data-Type-1@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
  {CODE timeseries_region_Append-Strongly-Typed-2@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* And in this sample we Get StockPrice values by name to check whether a stock's closing-time 
  price over a three-day period indicates that the price is ascending.
   {CODE timeseries_region_Get-Strongly-Typed@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* We use the custom StockPrice type in this query so we'd be able to 
  address retrieved values by name.
   {CODE timeseries_region_Strongly-Typed-Query@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

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
[Registering Time Series Types](../../../document-extensions/timeseries/client-api/register-time-series-types)  
