# Get Time Series Names
---

{NOTE: }

* Use `advanced.get_time_series_for` to get the names of all time series for the specified entity.

* In this page:   
  * [`get_time_series_for` usage](../../../../../document-extensions/timeseries/client-api/session/get/get-names#get_time_series_for-usage)
  * [Example](../../../../../document-extensions/timeseries/client-api/session/get/get-names#example)  

{NOTE/}

---

{PANEL: `get_time_series_for` usage}

**Flow**:  

* Open a session.
* Load an entity to the session either using [session.load](../../../../../client-api/session/loading-entities#load) 
  or by querying for the document via [session.query](../../../../../client-api/session/querying/how-to-query).  
  In both cases, the resulting entity will be tracked by the session.
* Call `advanced.get_time_series_for`, pass the tracked entity.

**Note**:  

* If the entity is Not tracked by the session, an `ArgumentException` exception is thrown.

{PANEL/}

{PANEL: Example}

{CODE:python timeseries_region_Retrieve-TimeSeries-Names@DocumentExtensions\TimeSeries\TimeSeriesTests.py /}  

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
