# Delete Time Series

---

{NOTE: }

* Use `timeSeriesFor.delete` to delete time series entries.  
* A time series is removed when all of its entries are deleted.  

* In this page:  
    * [usage](../../../../document-extensions/timeseries/client-api/session/delete#usage)
    * [Example](../../../../document-extensions/timeseries/client-api/session/delete#example)
  
{NOTE/}

---

{PANEL: usage}

**Flow**:

* Open a session.  
* Create an instance of `timeSeriesFor` and pass it the following:
    * Provide an explicit document ID, or -   
      pass an [entity tracked by the session](../../../../client-api/session/what-is-a-session-and-how-does-it-work#unit-of-work-pattern),
      e.g. a document object returned from [session.query](../../../../client-api/session/querying/how-to-query) 
      or from [session.load](../../../../client-api/session/loading-entities#load).
    * Specify the time series name.
* Call `timeSeriesFor.delete` and provide the **timestamps range** of the entries you want to delete.  
* Call `session.SaveChanges` for the action to take effect on the server.  

**Note**:

* If the specified document doesn't exist, a `DocumentDoesNotExistException` will be thrown.  
* Attempting to delete nonexistent entries results in a no-op and generates no exception.  
* To delete a whole time series simply delete all its entries.  
  The series is removed when all its entries are deleted.  
* Deleting a document deletes all its time series as well.  

{PANEL/}

{PANEL: Example}

In the following example we delete a time series entry appended by sample code in the 
[append](../../../../document-extensions/timeseries/client-api/session/append#examples) article.  
{CODE:php timeseries_region_Delete-TimeSeriesFor-Single-Time-Point@DocumentExtensions\TimeSeries\TimeSeriesTests.php /}

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
