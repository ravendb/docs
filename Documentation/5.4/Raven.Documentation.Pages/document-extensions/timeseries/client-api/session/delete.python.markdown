# Delete Time Series

---

{NOTE: }

* Use `time_series_for.delete_at` to delete a time series entry.  
* Use `time_series_for.delete` to delete a range of time series entries.  
* A time series is removed when all of its entries are deleted.  

* In this page:  
    * [usage](../../../../document-extensions/timeseries/client-api/session/delete#usage)
    * [Example](../../../../document-extensions/timeseries/client-api/session/delete#example)
    * [Syntax](../../../../document-extensions/timeseries/client-api/session/delete#syntax)
  
{NOTE/}

---

{PANEL: usage}

**Flow**:

* Open a session.  
* Create an instance of `time_series_for` and pass it the following:
    * Provide an explicit document ID, or -   
      pass an [entity tracked by the session](../../../../client-api/session/what-is-a-session-and-how-does-it-work#unit-of-work-pattern),
      e.g. a document object returned from [session.query](../../../../client-api/session/querying/how-to-query) 
      or from [session.load](../../../../client-api/session/loading-entities#load).
    * Specify the time series name.
* Call `time_series_for.delete_at` and provide the **timestamp** of an entry you want to delete,  
  -or-  
  Call `time_series_for.delete` and provide the **timestamps range** of the entries you want to delete.  
* Call `session.save_changes` for the action to take effect on the server.  

**Note**:

* If the specified document doesn't exist, a `DocumentDoesNotExistException` will be thrown.  
* Attempting to delete nonexistent entries results in a no-op and generates no exception.  
* To delete a whole time series simply delete all its entries.  
  The series is removed when all its entries are deleted.  
* Deleting a document deletes all its time series as well.  

{PANEL/}

{PANEL: Example}

In the following example we delete a time series entry appended by sample code in the 
[Append](../../../../document-extensions/timeseries/client-api/session/append) article.  
{CODE:python timeseries_region_Delete-TimeSeriesFor-Single-Time-Point@DocumentExtensions\TimeSeries\TimeSeriesTests.py /}

{PANEL/}

{PANEL: Syntax}

{CODE:python TimeSeriesFor-Delete-definition-single-timepoint@DocumentExtensions\TimeSeries\TimeSeriesTests.py /}

{CODE:python TimeSeriesFor-Delete-definition-range-of-timepoints@DocumentExtensions\TimeSeries\TimeSeriesTests.py /}

| Parameter | Type        | Description                                |
|-----------|-------------|:-------------------------------------------|
| **at**    | `datetime`  | Timestamp of a time series entry to delete |
| **datetime_from** (Optional) | `datetime` | Delete the time series entries range that starts at this timestamp (inclusive) |
| **datetime_to** (Optional) | `datetime` | Delete the time series entries range that ends at this timestamp (inclusive)     |
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
