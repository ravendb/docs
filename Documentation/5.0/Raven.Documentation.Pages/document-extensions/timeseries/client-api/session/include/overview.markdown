# Session: Include Time Series

---

{NOTE: }

You can [Include](../../../../../client-api/session/loading-entities#load-with-includes) 
documents' time series data while retrieving the documents.  
The included data is held by the client's session, so it can 
be handed to the user instantly when requested without issuing 
an additional request to the server.  

* Time series data can be included while -  
   * [Loading a Document Using `session.Load`](../../../../../document-extensions/timeseries/client-api/session/include/with-session-load)  
   * [Loading a Document By Query Via `session.Query`](../../../../../document-extensions/timeseries/client-api/session/include/with-session-query)  
   * [Loading a Document By a Raw RQL Query Via `session.Advanced.RawQuery`](../../../../../document-extensions/timeseries/client-api/session/include/with-raw-queries)  

{NOTE/}

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
