# Time-Series Design



---

{NOTE: }

* ...  

* In this page:  
  * [Time-Series Structure](../../document-extensions/timeseries/design#time-series-structure)  
  * [Success, Failure and Conflicts](../../document-extensions/timeseries/design#success,-failure-and-conflicts)  
  * [The `HasTimeSeries` Flag](../../document-extensions/timeseries/design#the-hastimeseries-flag)  

  * []()  
{NOTE/}

---

{PANEL: Time-Series Structure}

A time-series is an array of `IEnumerable`-type time-series entries.  
Each entry consists of a **timestamp**, a **tag**, and an array of **values**.  

* `TimeSeriesEntry` 

| Parameters | Type | Description |
|:-------------|:-------------|:-------------|
| `Timestamp` | DateTime | Entry's time signature |
| `Tag` | string | Entry's tag |
| `Values` | double[] | Entry's values array  |

* A time series includes a reference to the document it extends, by the document's ID.  

* Time-series are encoded using Facebook Gorilla encoding.  

---

####Segments

Time-series may be constructed of segments.  

* A time-series of up to 32k entries is contained in a single segment.  
* A time-series longet than 32k entries is contained in multiple segments.  
* A new segment is also created when a period of more than 25 days has passed 
  since the last measurement.  

{PANEL/}

{PANEL: Success, Failure and Conflicts}

---

####Success

As long as its owner document exists, updating a Time-Serie will always succeed.  

---

####Transactions

When a session transaction that includes a time-series modification fails for any 
reason, the time-series modification is reverted.  

---

####No Conflicts

Time-series actions do not cause conflicts.  

* **When a time-series is updated by multiple cluster Nodes**  
   * When a time-series' data is replicated by multiple nodes, the data 
     from all nodes is merged into a single series.  
   * When multiple nodes append **different values** at the same timestamp, 
     the **bigger value** is chosen for this entry.  
   * When multiple nodes apppend **different amount of values** for the same 
     timestamp, the **bigger amount of values** is chosen for this entry.  
   * When an existing value at a certain timestamp is deleted by a node 
     and updated by another node, the deletion is chosen.  

* **When a time-series is updated By multiple clients of the same node**  
   * When a time-series' value at a certain timestamp is appended by 
     multiple clients more or less simultaneously, the last one is chosen.  
   * When an existing value at a certain timestamp is deleted by a client 
     and updated by another client, the last action is chosen.  

{PANEL/}


{PANEL: The `HasTimeSeries` Flag}

* `HasTimeSeries` Flag
    * When the first time-series is added to a document, RavenDB automatically sets 
      a `HasTimeSeries` Flag in the document's metadata.  
    * When all time-series have been removed from a document, RavenDB automatically 
      removes the flag.  

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
