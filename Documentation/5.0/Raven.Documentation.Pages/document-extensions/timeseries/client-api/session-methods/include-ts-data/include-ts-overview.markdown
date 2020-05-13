# Include Time-Series Data: Overview 

---

{NOTE: }

You can [Include](../../../client-api/session/loading-entities#load-with-includes) 
documents' time-series data while retrieving the documents.  
The included time-series data is held by the client's session, 
so it can be handed to the user the instant it is required - 
with no additional trip to the server.  

* Time-series data can be included while -  
   * [Loading a Document Using `session.Load`](../../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-session-load)  
   * [Loading a Document By Query Via `session.Query`](../../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-session-query)  
   * [Loading a Document By a Raw RQL Query Via `session.Advanced.RawQuery`](../../../../../document-extensions/timeseries/client-api/session-methods/include-ts-data/with-raw-queries)  

{NOTE/}

## Related articles
**Studio Articles**:  
[Studio Time Series Management]()  

**Client-API - Session Articles**:  
[Time Series Overview]()  
[Creating and Modifying Time Series]()  
[Deleting Time Series]()  
[Retrieving Time Series Values]()  
[Time Series and Other Features]()  

**Client-API - Operations Articles**:  
[Time Series Operations]()  
