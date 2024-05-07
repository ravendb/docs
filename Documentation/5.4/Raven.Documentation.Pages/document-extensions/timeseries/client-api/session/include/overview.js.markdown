# Including Time Series

---

{NOTE: }

* When retrieving documents that contain time series, you can request to _include_ their time series data.

* The included time series data is held by the client's session, so it can be handed to the user instantly when requested without issuing an additional request to the server.  

* Time series data can be _included_ when -  
   * [Loading a document using `session.load`](../../../../../document-extensions/timeseries/client-api/session/include/with-session-load)  
   * [Loading a document by query via `session.query`](../../../../../document-extensions/timeseries/client-api/session/include/with-session-query)  
   * [Loading a document by raw query via `session.advanced.rawQuery`](../../../../../document-extensions/timeseries/client-api/session/include/with-raw-queries)  

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
