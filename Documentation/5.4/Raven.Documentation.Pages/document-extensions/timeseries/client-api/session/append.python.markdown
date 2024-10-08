﻿# Append & Update Time Series

---

{NOTE: }

* Use `time_series_for.append_single` or `time_series_for.append` for the following actions:
    * **Creating a new time series**  
      Appending an entry to a time series that doesn't exist yet  
      will create the time series and add it the new entry.
    * **Creating a new time series entry**  
      Appending a new entry to an existing time series  
      will add the entry to the series at the specified timestamp.
    * **Modifying an existing time series entry**  
      Use `append_single` or `append` to update the data of an existing entry with the specified timestamp.

* Each call to `append_single` handles a **single** time series value at 
  a **single** [time series entry](../../../../document-extensions/timeseries/design#time-series-entries).  
  Each call to `append` can handle **multiple** time series values at 
  a **single** [time series entry](../../../../document-extensions/timeseries/design#time-series-entries).  

* To append **multiple** entries in a single transaction you can:  
   * Call `append_single` or `append` as many times as needed before calling `session.save_changes`, as shown in the examples below.  
   * Use patching to update the time series. Learn more in [Patch time series entries](../../../../document-extensions/timeseries/client-api/session/patch).  
   * Append entries directly on the _Store_ via [Operations](../../../../client-api/operations/what-are-operations). 
     Learn more in [Append time series operations](../../../../document-extensions/timeseries/client-api/operations/append-and-delete).  

* In this page:
    * [Usage](../../../../document-extensions/timeseries/client-api/session/append#usage)
    * [Examples](../../../../document-extensions/timeseries/client-api/session/append#examples)
       * [Append entries with a single value](../../../../document-extensions/timeseries/client-api/session/append#append-entries-with-a-single-value)
       * [Append entries with multiple values](../../../../document-extensions/timeseries/client-api/session/append#append-entries-with-multiple-values)
    * [Syntax](../../../../document-extensions/timeseries/client-api/session/append#syntax)

{NOTE/}

---

{PANEL: Usage}

**Flow**:

* Open a session.
* Create an instance of `time_series_for` and pass it the following:
    * Provide an explicit document ID, -or-  
      pass an [entity tracked by the session](../../../../client-api/session/what-is-a-session-and-how-does-it-work#unit-of-work-pattern),
      e.g. a document object returned from [session.query](../../../../client-api/session/querying/how-to-query) 
      or from [session.load](../../../../client-api/session/loading-entities#load).
    * Specify the time series name.
* Call `time_series_for.append_single` or `time_series_for.append` and pass it the time series entry details.
* Call `session.save_changes` for the action to take effect on the server.

**Note**:

* A `DocumentDoesNotExistException` exception is thrown if the specified document does not exist.

{PANEL/}

{PANEL: Examples}

#### Append entries with a single value:

* In this example, entries are appended with a single value.
* Although a loop is used to append multiple entries,  
  all entries are appended in a single transaction when `save_changes` is executed.

{CODE:python timeseries_region_TimeSeriesFor-Append-TimeSeries-Range@DocumentExtensions\TimeSeries\TimeSeriesTests.py /}

---

#### Append entries with multiple values:

* In this example, we append multi-value StockPrice entries.  
* Notice the clarity gained by naming the values.  

{CODE-TABS}
{CODE-TAB:python:Native timeseries_region_Append-Unnamed-Values-2@DocumentExtensions\TimeSeries\TimeSeriesTests.py /}
{CODE-TAB:python:Using_named_values timeseries_region_Append-Named-Values-2@DocumentExtensions\TimeSeries\TimeSeriesTests.py /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:python TimeSeriesFor-Append-definition-double@DocumentExtensions\TimeSeries\TimeSeriesTests.py /}

{CODE:python TimeSeriesFor-Append-definition-inum@DocumentExtensions\TimeSeries\TimeSeriesTests.py /}

| Parameter | Type | Description |
|-----------|------|-------------|
| **timestamp** | `datetime` | Time series entry's timestamp |
| **value** | `float` | Entry's value |
| **values** | `List[float]` | Entry's values |
| **tag** (Optional) | `str` | An optional tag for the entry |

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
