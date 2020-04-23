# Dynamic Time Series Queries 
---

{NOTE: }

*  
  
* In this page:  
  * []()  
  * []()  
{NOTE/}

---

{PANEL: }

###Time Series and Queries  

Create queries **using code**, or send the server **raw queries** for execution.  

* Either way, you can query time series **by name** but **not by value**.  
  This is because queries are generally [based on indexes](), and time series are [not indexed]().  
* Time series **can** be [projected](../../../indexes/querying/projections) from query results, as demonstrated in the following examples.  
  This way a client can get TS values from a query without downloading whole documents.  

* Use [Session.Query](../../../client-api/session/querying/how-to-query#session.query) to code queries yourself.  
   * **Returned Time Series Value**  
     ...  
     **query sample here**  

* Use [RawQuery](../../../client-api/session/querying/how-to-query#session.advanced.rawquery) to send the server raw RQL expressions for execution.  
   * You can use the [**TS method name here**] method.  
     **Returned Value**  

   [**if there are TS and raw TS methods - samples of both here**]  
   [**sample of a TS query using a filter**]  
   [**sample of a TS query using projection**]  

{PANEL/}

{PANEL: }
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
