# Loading and Including Time Series  
---

{NOTE: }

* Use [session.TimeSeriesFor.`Get`]() to retrieve the value of a time point or a range of timepoints.  
  [is there a `GetAll` method?]

* Use []() to include a time series in a query before it is requested and minimize 
  clients' wait time.  

* In this page:  
  * [Loading Time Series]()  
  * [Loading Selected Time Points]()  
  * [Including Time Series]()  
{NOTE/}

---

{PANEL: Loading Time Series}

#### `Get` syntax:  

* **code sample**  
* **syntax (table)**  

---

#### `Get` usage Flow:

  * Open a session  
  * Create an instance of `TimeSeriesFor`.  
      * Either pass `TimeSeriesFor` an explicit document ID, -or-  
      * Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
  * Execute `TimeSeriesFor.Get`

---

#### Retrieve a single Time Point:
**Code Sample**  

---

#### Retrieve a Range of Time Points
**Code Sample**  

{PANEL/}

{PANEL: Including Time Series}
  
You can [include](../../../client-api/how-to/handle-document-relationships#includes) time series while loading a document.  
An included time series is retrieved in the same request as its owner-document and is held by the session, 
so it can be immediately retrieved when needed with no additional remote calls.


* **Including time series when using [Session.Load](../../../client-api/session/loading-entities#session--loading-entities)**:  
    * Include a single time series using [?]  
    * Include multiple time series using [?]  

   **code sample here**

* **Including time series when using [Session.Query](../../../client-api/session/querying/how-to-query#session--querying--how-to-query)**:  
    * Include a single time series using [?]  
    * Include multiple time series using [?]  

   **code sample here**

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
