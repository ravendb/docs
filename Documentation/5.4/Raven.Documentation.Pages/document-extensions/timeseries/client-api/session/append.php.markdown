# Append & Update Time Series

---

{NOTE: }

* Use `timeSeriesFor.append` to:
   * **Create a new time series**  
     Appending an entry to a time series that doesn't exist yet  
     will create the time series and add it the new entry.
   * **Create a new time series entry**  
     Appending a new entry to an existing time series  
     will add the entry to the series at the specified timestamp.
   * **Modify an existing time series entry**  
     Use `append` to update the data of an existing entry with the specified timestamp.

* To append multiple entries in a single transaction you can:  
   * Call `append` as many times as needed before calling `session.saveChanges`, as shown in the examples below.  
   * Use patching to update the time series. Learn more in [Patch time series entries](../../../../document-extensions/timeseries/client-api/session/patch).  
   * Append entries directly on the _Store_ via [Operations](../../../../client-api/operations/what-are-operations).  

* In this page:
    * [Usage](../../../../document-extensions/timeseries/client-api/session/append#usage)
    * [Examples](../../../../document-extensions/timeseries/client-api/session/append#examples)
       * [Append entries with a single value](../../../../document-extensions/timeseries/client-api/session/append#append-entries-with-a-single-value)
       * [Append entries with multiple values](../../../../document-extensions/timeseries/client-api/session/append#append-entries-with-multiple-values)

{NOTE/}

---

{PANEL: Usage}

**Flow**:

* Open a session.
* Create an instance of `timeSeriesFor` and pass it the following:
    * Provide an explicit document ID, -or-  
      pass an [entity tracked by the session](../../../../client-api/session/what-is-a-session-and-how-does-it-work#unit-of-work-pattern),
      e.g. a document object returned from [session.query](../../../../client-api/session/querying/how-to-query) 
      or from [session.load](../../../../client-api/session/loading-entities#load).
    * Specify the time series name.
* Call `timeSeriesFor.append` and pass it the time series entry details.
* Call `session.saveChanges` for the action to take effect on the server.

**Note**:

* A `DocumentDoesNotExistException` exception is thrown if the specified document does not exist.

{PANEL/}

{PANEL: Examples}

#### Append entries with a single value:

* In this example, entries are appended with a single value.
* Although a loop is used to append multiple entries,  
  all entries are appended in a single transaction when `saveChanges` is executed.

{CODE-TABS}
{CODE-TAB:php:append timeseries_region_TimeSeriesFor-Append-TimeSeries-Range@DocumentExtensions\TimeSeries\TimeSeriesTests.php /}
{CODE-TABS/}

---

#### Append entries with multiple values:

* In this example, we append multi-value StockPrice entries.  
* Notice the clarity gained by naming the values.  

{CODE-TABS}
{CODE-TAB:php:Native timeseries_region_Append-Unnamed-Values-2@DocumentExtensions\TimeSeries\TimeSeriesTests.php /}
{CODE-TAB:php:Using_named_values timeseries_region_Append-Named-Values-2@DocumentExtensions\TimeSeries\TimeSeriesTests.php /}
{CODE-TABS/}

{PANEL/}

## Related articles

**Client API**  
[Time Series API Overview](../../../../document-extensions/timeseries/client-api/overview)

**Studio Articles**  
[Studio Time Series Management](../../../../studio/database/document-extensions/time-series)

**Querying and Indexing**  
[Time Series Querying](../../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../../document-extensions/timeseries/indexing)

**Policies**  
[Time Series Rollup and Retention](../../../../document-extensions/timeseries/rollup-and-retention)  
