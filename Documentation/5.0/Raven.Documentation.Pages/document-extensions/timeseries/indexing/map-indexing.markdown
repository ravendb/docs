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

### Indexing Time Series  

Time-series indexes are used to index time-series data, as opposed to documents. 
The API for creating time-series indexes is very similar to creating any other index, 
with a few minor differences described here:  

The two main ways to create time-series indexes are:
1. Create a class that inherits from `AbstractTimeSeriesIndexCreationTask`, or in the 
case of a multi-map index, use `AbstractMultiMapTimeSeriesIndexCreationTask`.
2. Create a new `TimeSeriesIndexDefinition`.

This one really makes no difference, exists so features can be filled in in the future.  
`TimeSeriesIndexDefinition : IndexDefinition`

Show in one example
`TimeSeriesIndexDefinitionBuilder<TDocument, TReduceResult> : AbstractIndexDefinitionBuilder<TDocument, TReduceResult, TimeSeriesIndexDefinition>`

####`TimeSeriesSegment` object

Time-series are divided into **segments** in storage, each containing several consecutive entries. The 
number of entries per segment can vary widely depending on the size and compressibility of the entries. 
Entries that are more than 25 days apart will be stored in different segments.  

Segments are useful because they can be referenced within time-series indexes to access the entries in 
the segment, as well as some aggregate values that summarize the data in the segment:  

| Property | Type | Description |
| - | - | - |
| DocumentId | string | The [ID]() of the document this time-series belongs to |
| Name | string | The name of the time-series this segment belongs to |
| Min | double[] | The smallest values from all entries in the segment. The places in this array correspond to the  |
| Max | double[] | The largest values from all entries in the segment |
| Sum | double[] | The sums of values from all entries in the segment. The first `Sum` is the sum of all first values, and so on. |
| Count | int | The number of entries in this segment |
| Start | DateTime | The timestamp of the first entry in the segment |
| End | DateTime | The timestamp of the last entry in the segment |
| Entries | `TimeSeriesEntry[]` | The segment's entries themselves |
{PANEL/}

{PANEL: Examples}

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
