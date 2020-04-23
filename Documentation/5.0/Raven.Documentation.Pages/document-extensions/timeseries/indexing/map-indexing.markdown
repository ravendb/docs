# Time Series Map Indexing  
---

{NOTE: }

*  
  
* In this page:  
  * []()  
  * []()  
{NOTE/}

---

{PANEL: }

### Time Series and Indexing  

Indexing time series can speed-up finding them and the documents that contain them.  
RavenDB supports time series indexing **by name**, but **not by value**.  

* **Time Series indexing**  
    Re-indexing due to time series name modification is rare enough to pause no performance issues.  
    To index a document's time series by name, use [TimeSeriesFor]().  
  
* **Time Series Values indexing**  
    Indexing by values contained in time series is **not** provided, changing values will not trigger indexing of the document.  
    Time series are designed for high-throughput scenarios, and performance cannot afford the re-indexing cost of each change.  

**Map Index sample here**

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
