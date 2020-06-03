# Time-Series Queries Overview
---

{NOTE: }

* Time-series data can be aggregated and/or queried, to show the behavior of 
  the processes that produce this data.  

* Data can be aggregated using `group by`and `select` in a query.  
  (Data can also be aggregated using [rollup policies](../../../document-extensions/timeseries/rollup-and-retention); 
  this is not related to queries.)  

* In this page:  
  * [Server and Client Queries](../../../document-extensions/timeseries/querying/queries-overview#server-and-client-queries)  
  * [Server and Client Queries](../../../document-extensions/timeseries/querying/queries-overview#dynamic-queries-and-indexed-queries)  
  * [Query Range Selection](../../../document-extensions/timeseries/querying/queries-overview#query-range-selection)  
  * [Filtering](../../../document-extensions/timeseries/querying/queries-overview#filtering)  
  * [Aggregation and Projection](../../../document-extensions/timeseries/querying/queries-overview#aggregation-and-projection)  

{NOTE/}

---

{PANEL: Time-Series Queries}

---

#### Server and Client Queries  

Time-series queries require very little client computation resources.  
They are executed by the server in full (including document query, 
time-series range selection, results filtering and aggregation) and 
their results are projected to the client.  

The server runs RQL queries.  
In your client code, you can send the server a raw RQL query 
or phrase your queries using **LINQ expressions** (which will 
also be translated to RQL before their execution by the server).  

---

#### Dynamic Queries and Indexed Queries

* As long as a time-series is unindexed, you can query it using 
  [dynamic queries](../../../document-extensions/timeseries/querying/dynamic-queries).  

    {INFO: }
    Time-series are **not automatically indexed**.  
    {INFO/}

* Indexed time-series can be queried using 
  [indexed queries](../../../document-extensions/timeseries/querying/indexed-queries).  

    {INFO: }
    You can create **static indexes** for your time-series.  
    {INFO/}

---

#### Query Range Selection

Queries can be performed over a whole series or over a smaller range of time-series 
entries, e.g. only on entries collected during the last 7 days.  

---

#### Filtering

Time-series entries can be filtered by -  

* by their **Tags**  

    E.g. retrieve all the entries whose tag is "Thermometer C".  

* by their **Values**  

    E.g. retrieve all the entries whose measurements exceed 32 Celsius degrees.  

* by the **contents of a document they refer to**  
  A time-series entry's tag can contain the **ID of a document**.  
  In your query, you can **load the document** the entry refers to.  
  Once the document is loaded, you can **filter your results by its contents**.  

    E.g. retrieve entries whose tags refer to a device-specification document, 
    where the specifications indicate that the device has been manufactured 
    in Sweden.  

---

#### Aggregation and Projection

* **Aggregation**  
  Query result-sets can be aggregated (grouped) by a chosen time frame, 
  e.g. a day (grouping the results in multiple day-long data frames).  

* **Projection**  
  Various criteria (like `min`, `max` and others) can be used to 
  `select` the results that would be sent to the client.  
  Projecting selected results from a **grouped** result-set, will 
  send the client  matching results from each group.  


{PANEL/}

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
