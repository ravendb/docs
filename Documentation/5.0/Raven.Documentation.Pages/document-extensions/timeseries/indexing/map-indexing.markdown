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
    Re-indexing due to time series name modification is rare enough to cause no performance issues.  
    To index a document's time series by name, use [TimeSeriesFor]().  
  
* **Time Series Values indexing**  
    Indexing by values contained in time series is **not** provided, changing values will not trigger indexing of the document.  
    Time series are designed for high-throughput scenarios, and performance cannot afford the re-indexing cost of each change.  

**Map Index sample here**

{PANEL/}

{PANEL: }
Use these time-series specific alternatives to the standard [indexing API]()

`AbstractTimeSeriesIndexCreationTask : AbstractIndexCreationTask[Base]`

This one really makes no difference, exists so features can be filled in in the future.  
`TimeSeriesIndexDefinition : IndexDefinition`

Show in one example
`TimeSeriesIndexDefinitionBuilder<TDocument, TReduceResult> : AbstractIndexDefinitionBuilder<TDocument, TReduceResult, TimeSeriesIndexDefinition>`


####`TimeSeriesSegment` object

Time-series are divided into **segments** in storage, each containing several consecutive entries. 
Segments can be referenced within the index syntax. This exposes aggregate values.  

| Property | Type | Description |
| - | - | - |
| DocumentId | string | The [ID]() of the document this time-series belongs to |
| Name | string | The name of the time-series this segment belongs to |
| Min | double[] | The smallest values from all entries in the segment. `Min[0]` is the smallest of all the *first* values in the entries of this segment, `Min[1]` is the smallest second value, and so on. |
| Max | double[] | The largest values from all entries in the segment |
| Sum | double[] | The sums of values from all entries in the segment. The first `Sum` is the sum of all first values, and so on. |
| Count | int | The number of entries in this segment |
| Start | DateTime | The timestamp of the first entry in the segment |
| End | DateTime | The timestamp of the last entry in the segment |
| Entries | `TimeSeriesEntry[]` | The segment's entries themselves |
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
