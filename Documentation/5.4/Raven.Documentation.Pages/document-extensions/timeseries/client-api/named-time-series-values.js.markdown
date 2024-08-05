# Named Time Series Values

{NOTE: }

* A time series entry consists of a **timestamp**, one or more **values**, and an optional **tag**.  
  Each value can be given a name to indicate what it represents, such as "Temperature", "Humidity", "Pressure", etc.
  
* Referring to these values by their names in time series methods (such as `append`, `get`, etc.)  
  makes your code more readable and easier to manage.

* In order for the Studio to present the time series values by their names, as can be seen [here](../../../studio/database/document-extensions/time-series#time-series-view),  
  you need to register the named values on the server. 

* In this page:  
  * [Named values](../../../document-extensions/timeseries/client-api/named-time-series-values#named-values)  
     * [Define time series class with named values](../../../document-extensions/timeseries/client-api/named-time-series-values#define-time-series-class-with-named-values)  
     * [Examples](../../../document-extensions/timeseries/client-api/named-time-series-values#examples)  
  * [Register time series named values](../../../document-extensions/timeseries/client-api/named-time-series-values#register-time-series-named-values)  
     * [Usage](../../../document-extensions/timeseries/client-api/named-time-series-values#usage)
     * [Syntax](../../../document-extensions/timeseries/client-api/named-time-series-values#syntax)

{NOTE/}

---

{PANEL: Named values}

* Many time series are populated with multiple values for each measurement.  
  For example, each GPS measurement in a route-tracking time series would include at least two values:  
  latitude and longitude.

* You can ease the management of multi-value time series by -  
  * Naming time series values in custom classes.  
  * Calling time series methods with your custom types to address and manage values by name.  

---

#### Define time series class with named values

To define a class with named values, add the static property `TIME_SERIES_VALUES` to the class.  
E.g.:

{CODE:nodejs routePoint_class@documentExtensions\timeSeries\client-api\namedValues.js /}  

The class can then be used by time series methods like _append_:  

{CODE:nodejs named_values_1@documentExtensions\timeSeries\client-api\namedValues.js /}

---

#### Examples

* In this example, we define a StockPrice class and use it when appending StockPrice entries.
  {CODE:nodejs stockPrice_class@documentExtensions\timeSeries\client-api\namedValues.js /}
  {CODE:nodejs named_values_2@documentExtensions\timeSeries\client-api\namedValues.js /}

* In this example, we get StockPrice values by name and check whether a stock's closing-time prices are ascending over time.
  {CODE:nodejs named_values_3@documentExtensions\timeSeries\client-api\namedValues.js /}

* In this query, we use the custom StockPrice type so we can address trade Volume by name.
  {CODE-TABS}
{CODE-TAB:nodejs:Query named_values_4@documentExtensions\timeSeries\client-api\namedValues.js /}  
{CODE-TAB-BLOCK:sql:RQL}
from "companies"
where address.city = $p0
select timeseries(
    from StockPrices
    between $start and $end
    where Tag == "AppleTech")
{
   "p0":"New York",
   "start":"2024-06-04T06:02:39.826Z",
   "end":"2024-06-07T06:02:39.826Z"
}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Register time series named values}

Registering a custom time series type on the server stores this information in the [database record](../../../studio/database/settings/database-record).  
This allows the Studio to present time series values by name when you view and manage them.

---

#### Usage

To register a time series type, call `documentStore.timeSeries.register`, e.g.:

{CODE:nodejs named_values_5@documentExtensions\timeSeries\client-api\namedValues.js /}

<br>
The time series entries will be listed in the Studio under their corresponding named values:

!["Time series entries"](images/time-series-entries-js.png "Time series entries with named values")

<br>
The named values can be managed from the [Time Series Settings View](../../../studio/database/settings/time-series-settings) in the Studio:

!["Time series settings view"](images/time-series-settings-view-js.png "The time series settings view")

---

#### Syntax

  {CODE:nodejs syntax@documentExtensions\timeSeries\client-api\namedValues.js /}

<br>

| Parameter                | Type       | Description                        |
|--------------------------|------------|------------------------------------|
| **collection**           | `string`   | The time series collection name    |
| **name**                 | `string `  | Time series name                   |
| **valueNames**           | `string[]` | Names to register (name per value) |
| **collectionClass**      | `object`   | The collection class               |
| **timeSeriesEntryClass** | `object`   | The custom time series entry class |

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
