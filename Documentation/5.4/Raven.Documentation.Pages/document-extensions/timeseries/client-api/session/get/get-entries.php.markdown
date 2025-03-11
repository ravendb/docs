# Get Time Series Entries 
---

{NOTE: }

* Use `timeSeriesFor.get` to retrieve a range of entries from a **single** time series.  
  To retrieve a range of entries from **multiple** series, 
  use the `GetMultipleTimeSeriesOperation`operation.

* The retrieved data can be paged to get the time series entries gradually, one custom-size page at a time.

* By default, the session will track the retrieved time series data. 
  See [disable tracking](../../../../../client-api/session/configuration/how-to-disable-tracking) to learn how to disable.
  
* When getting the time series entries,  
  you can also _include_ the series' **parent document** and/or **documents referred to by the entry tag**.  
  Learn more [below](../../../../../document-extensions/timeseries/client-api/session/get/get-entries#include-parent-and-tagged-documents).

* Calling `timeSeriesFor.get` will result in a trip to the server unless the series' parent document was loaded  
  (or queried for) with the time series included beforehand.  

* In this page:  
  * [`get` usage](../../../../../document-extensions/timeseries/client-api/session/get/get-entries#get-usage)
  * [Examples](../../../../../document-extensions/timeseries/client-api/session/get/get-entries#examples)
     * [Get all entries](../../../../../document-extensions/timeseries/client-api/session/get/get-entries#get-all-entries)
     * [Get range of entries](../../../../../document-extensions/timeseries/client-api/session/get/get-entries#get-range-of-entries)
     * [Get entries with multiple values](../../../../../document-extensions/timeseries/client-api/session/get/get-entries#get-entries-with-multiple-values)
  * [Include parent and tagged documents](../../../../../document-extensions/timeseries/client-api/session/get/get-entries#include-parent-and-tagged-documents)
  * [Syntax](../../../../../document-extensions/timeseries/client-api/session/get/get-entries#syntax)

{NOTE/}

---

{PANEL: `get` usage }

* Open a session.  
* Create an instance of `timeSeriesFor` and pass it the following:
    * Provide an explicit document ID, -or-  
      pass an [entity tracked by the session](../../../../../client-api/session/what-is-a-session-and-how-does-it-work#unit-of-work-pattern), 
      e.g. a document object returned from [session.query](../../../../../client-api/session/querying/how-to-query) 
      or from [session.load](../../../../../client-api/session/loading-entities#load).
    * Specify the time series name.
* Call `timeSeriesFor.get`.  

{PANEL/}

{PANEL: Examples}

#### Get all entries:

In this example, we retrieve all entries of the "Heartrate" time series.  
The ID of the parent document is explicitly specified.  

{CODE:php timeseries_region_Get-All-Entries-Using-Document-ID@DocumentExtensions\TimeSeries\TimeSeriesTests.php /}

---

#### Get range of entries:

In this example, we query for a document and get its "Heartrate" time series data.

{CODE:php timeseries_region_Pass-TimeSeriesFor-Get-Query-Results@DocumentExtensions\TimeSeries\TimeSeriesTests.php /}

---

#### Get entries with multiple values:

* Here, we check whether a stock's closing-time price is rising from day to day (over three days).  
  This example is based on the sample entries that were entered in [this example](../../../../../document-extensions/timeseries/client-api/session/append#append-entries-with-multiple-values).
 
* Since each time series entry contains multiple StockPrice values, 
  we include a sample that uses [named](../../../../../document-extensions/timeseries/client-api/session/append#append-entries-with-multiple-values) 
  time series values to make the code easier to read.  

{CODE:php timeseries_region_Get-Named-Values@DocumentExtensions\TimeSeries\TimeSeriesTests.php /}

{PANEL/}

{PANEL: Include parent and tagged documents}

* When retrieving time series entries using `timeSeriesFor.get`,  
  you can include the series parent document and/or documents referred to by the entries 
  [tags](../../../../../document-extensions/timeseries/overview#tags).  

* The included documents will be cached in the session, and instantly retrieved from memory if loaded by the user.

* Use the following syntax to include the parent or tagged documents:

{CODE:php IncludeParentAndTaggedDocuments@DocumentExtensions\TimeSeries\TimeSeriesTests.php /}

{PANEL/}

{PANEL: Syntax}

{CODE:php TimeSeriesFor-Get-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.php /}

{CODE:php TimeSeriesFor-Get-Named-Values@DocumentExtensions\TimeSeries\TimeSeriesTests.php /}

| Parameter | Type | Description |
|-----------|------|-------------|
| **from** (Optional) | `DateTimeInterface` | Get the range of time series entries starting from this timestamp (inclusive). |
| **to** (Optional) | `DateTimeInterface` | Get the range of time series entries ending at this timestamp (inclusive). |
| **start** | `int` | Paging first entry.<br>E.g. 50 means the first page would start at the 50th time series entry. <br> Default: `0`, for the first time-series entry. |
| **pageSize** | `int` | Paging page-size.<br>E.g. set `page_size` to 10 to retrieve pages of 10 entries.<br>Default: `PHP_INT_MAX`, for all time series entries. |

**Return Values (Optional)**
 
* `?TimeSeriesEntryArray` - an array of time series entry classes.
  {CODE:php TimeSeriesEntry-Definition@DocumentExtensions\TimeSeries\TimeSeriesTests.php /}

{PANEL/}

## Related articles

**Client API**  
[Time Series API Overview](../../../../../document-extensions/timeseries/client-api/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../../document-extensions/timeseries/rollup-and-retention)  
