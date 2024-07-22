# Named Time Series Values

{NOTE: }

* A time series entry consists of a **timestamp**, one or more **values**, and an optional **tag**.  
  Each value can be given a name to indicate what it represents, such as "Temperature", "Humidity", "Pressure", etc.
  
* Referring to these values by their names in time series methods (such as `Append`, `Get`, etc.)  
  makes your code more readable and easier to manage.

* In order for the Studio to present the time series values by their names, as can be seen [here](../../../studio/database/document-extensions/time-series#time-series-view),  
  you need to register the time series types on the server. 

* In this page:  
  * [Named values](../../../document-extensions/timeseries/client-api/named-time-series-values#named-values)  
     * [Define time series type](../../../document-extensions/timeseries/client-api/named-time-series-values#define-time-series-type)  
     * [Examples](../../../document-extensions/timeseries/client-api/named-time-series-values#examples)  
  * [Register time series type](../../../document-extensions/timeseries/client-api/named-time-series-values#register-time-series-type)  
     * [Usage](../../../document-extensions/timeseries/client-api/named-time-series-values#usage)
     * [Syntax](../../../document-extensions/timeseries/client-api/named-time-series-values#syntax)

{NOTE/}

---

{PANEL: Named values}

* Many time series are populated with multiple values for each measurement.  
  For example, each GPS measurement in a route-tracking time series would include at least two values:  
  latitude and longitude.

* You can ease the management of multi-value time series by -  
  * Naming time series values in model classes that can be used as time series types.  
  * Calling time series methods with your custom types to address and manage values by name.  

---

#### Define time series type

To define a class for use as a time series type, mark the class properties (which represent the values)  
with consecutive `TimeSeriesValue` attributes: `TimeSeriesValue(0)`, `TimeSeriesValue(1)`, etc. 

E.g.:

{CODE Custom-Data-Type-2@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

The class can then be used by time series methods like _Append_:  

{CODE timeseries_region_Append-Named-Values-1@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{INFO: }
A quick way of retrieving a time series entry's value, timestamp, and tag is to use `Deconstruct()`:  

{CODE-BLOCK:csharp}
public void Deconstruct(out DateTime timestamp, out T value);
public void Deconstruct(out DateTime timestamp, out T value, out string tag);
{CODE-BLOCK/}
{INFO/}

---

#### Examples

* In this example, we define a StockPrice type and use it when appending StockPrice entries.
  {CODE Custom-Data-Type-1@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
  {CODE timeseries_region_Append-Named-Values-2@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* In this example, we get StockPrice values by name and check whether a stock's closing-time prices are ascending over time.
  {CODE timeseries_region_Get-Named-Values@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* In this query, we use the custom StockPrice type so we can address trade Volume by name.
  {CODE-TABS}
{CODE-TAB:csharp:Query timeseries_region_Named-Values-Query@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "companies" as c
where Address.City = $p0
select timeseries(
    from c.StockPrices
    between $p1 and $p2
    where (Tag == $p3))
{
   "p0":"New York",
   "p1":"2024-06-03T10:47:00.7880000Z",
   "p2":"2024-06-06T10:47:00.7880000Z",
   "p3":"companies/kitchenAppliances"
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Register time series type}

Registering a custom time series type on the server stores this information in the [database record](../../../studio/database/settings/database-record).  
This allows the Studio to present time series values by name when you view and manage them.

---

#### Usage

To register a time series type, call `store.TimeSeries.Register`, e.g.:

{CODE timeseries_region_Named-Values-Register@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

<br>
The time series entries will be listed in the Studio under their corresponding named values:

!["Time series entries"](images/time-series-entries.png "Time series entries with named values")

<br>
The named values can be managed from the [Time Series Settings View](../../../studio/database/settings/time-series-settings) in the Studio:

!["Time series settings view"](images/time-series-settings-view.png "The time series settings view")

---

#### Syntax

  {CODE Register-Definition-1@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
  {CODE Register-Definition-2@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
  {CODE Register-Definition-3@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

<br>

| Parameter            | Type             | Description                                                             |
|----------------------|------------------|-------------------------------------------------------------------------|
| **TCollection**      | Collection type  | The time series collection                                              |
| **TTimeSeriesEntry** | Time series type | The custom time series type                                             |
| **collection**       | `string`         | The time series collection name<br>(when `TCollection` is not provided) |
| **name**             | `string `        | Time series name                                                        |
| **valueNames**       | `string[]`       | Names to register (name per value)                                      | 

{PANEL/}

## Related articles

**Time Series Overview**  
[Time Series Overview](../../../document-extensions/timeseries/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../studio/database/document-extensions/time-series)  
[Time Series Settings View](../../../studio/database/settings/time-series-settings)  

**Querying and Indexing**  
[Time Series Querying](../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../document-extensions/timeseries/rollup-and-retention)  
