# Delete Time Series

---

{NOTE: }

* Use `TimeSeriesFor.Delete` for the following actions:

    * __Delete a single time series entry__
  
    * __Delete a range of entries__
  
    * __Delete the whole time series__:  
      To remove the whole time series simply delete all its entries.

* In this page:  
    * [`Delete` usage](../../../../document-extensions/timeseries/client-api/session/delete#delete-usage)
    * [Examples](../../../../document-extensions/timeseries/client-api/session/delete#examples)
      * [Delete single entry](../../../../document-extensions/timeseries/client-api/session/delete#delete-single-entry)
      * [Delete range of entries](../../../../document-extensions/timeseries/client-api/session/delete#delete-range-of-entries)
    * [Syntax](../../../../document-extensions/timeseries/client-api/session/delete#syntax)
  
{NOTE/}

---

{PANEL: `Delete` usage}

__Flow__:

* Open a session.  
* Create an instance of `TimeSeriesFor` and pass it the following:
    * Provide an explicit document ID, or -   
      pass an [entity tracked by the session](../../../../client-api/session/loading-entities),
      e.g. a document object returned from [session.Query](../../../../client-api/session/querying/how-to-query) or from [session.Load](../../../../client-api/session/loading-entities#load).
    * Specify the time series name.
* Call `TimeSeriesFor.Delete`:
    * Provide a single timestamp to delete a specific entry, or -  
    * Specify a range of timestamps to delete multiple entries.
* Call `session.SaveChanges` for the action to take effect on the server.  

__Note__:

* If the specified document doesn't exist, a `DocumentDoesNotExistException` is thrown.
* Attempting to delete nonexistent entries results in a no-op and generates no exception.
* To delete the whole time series simply delete all its entries.  
  The series is removed when all its entries are deleted.
* Deleting a document deletes all its time series as well.

{PANEL/}

{PANEL: Examples}

The following examples delete the time series entries that were appended in the [Append](../../../../document-extensions/timeseries/client-api/session/append) article:

{NOTE: }

<a id="delete-single-entry" /> __Delete single entry__:

---

{CODE timeseries_region_Delete-TimeSeriesFor-Single-Time-Point@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{NOTE/}

{NOTE: }

<a id="delete-range-of-entries" /> __Delete range of entries__:

---
 
{CODE timeseries_region_TimeSeriesFor-Delete-Time-Points-Range@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{NOTE/}

{PANEL/}

{PANEL: Syntax}

{CODE TimeSeriesFor-Delete-definition-single-timepoint@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{CODE TimeSeriesFor-Delete-definition-range-of-timepoints@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

| Parameter | Type        | Description                                                                                                       |
|-----------|-------------|:------------------------------------------------------------------------------------------------------------------|
| __at__    | `DateTime`  | Timestamp of the time series entry to delete.                                                                     |
| __from__  | `DateTime?` | Delete the range of time series entries starting from this timestamp (inclusive).<br>Default: `DateTime.MinValue` |
| __to__    | `DateTime?` | Delete the range of time series entries ending at this timestamp (inclusive).<br>Default: `DateTime.MaxValue`     |

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
