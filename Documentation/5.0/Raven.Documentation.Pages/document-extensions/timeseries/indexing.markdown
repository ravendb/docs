# Time Series Map Indexing  
---

{NOTE: }

Time-series indexes are used to index time-series data, as opposed to document fields. 
The API for creating time-series indexes is very similar to (and inherits from) the 
API for [creating document indexes](../../indexes/creating-and-deploying).  

{INFO: }
Unlike document indexes, time-series indexes currently support only LINQ syntax. 
JavaScript syntax is not supported.  
{INFO/}

* In this page:  
  * [Syntax](../../document-extensions/indexing#syntax)  
  * [Examples](../../document-extensions/indexing#examples)  

{NOTE/}

---

{PANEL: Syntax }

The are two main ways to create a time-series index:  

#### 1) Create a class that inherits from `AbstractTimeSeriesIndexCreationTask<>`  

{CODE-BLOCK: csharp}
public abstract class AbstractTimeSeriesIndexCreationTask : 
                      AbstractIndexCreationTaskBase<TimeSeriesIndexDefinition> 
{
    protected void AddMap(string timeSeries, 
                          Expression<Func<IEnumerable<TimeSeriesSegment>, 
                                          IEnumerable>> map);

    protected void AddMapForAll(Expression<Func<IEnumerable<TimeSeriesSegment>, 
                                                IEnumerable>> map);
}

// Index only time-series that belong to documents
// of a specified type
public class AbstractTimeSeriesIndexCreationTask<TDocument> { }

// Specify both a document type and a reduce type
public class AbstractTimeSeriesIndexCreationTask<TDocument, TReduceResult> { }
{CODE-BLOCK/}

| Method | Parameters | Description |
| - | - | - |
| `AddMap()` | `string timeseries`, `Expression map` | Sets a map function for all time-series in the database that have the specified name (the first parameter) |
| `AddMapForAll()` | `Expression map` | Sets a map function for all time-series in the database |

See the example [below]().

#### 2) Create a `TimeSeriesIndexDefinition`

{CODE-BLOCK: csharp}
public class TimeSeriesIndexDefinition : IndexDefinition
{CODE-BLOCK/}

For now, `TimeSeriesIndexDefinition` is functionally equivalent to 
[normal `IndexDefinition`](../../indexes/creating-and-deploying#using-maintenance-operations). 
Using it for time-series indexes is recommended - it exists in case additional functionality is 
added in future versions of RavenDB. See the example [below]().  

---

### `TimeSeriesSegment` object  

The LINQ syntax for the indexes themselves is the same, and in addition you can reference the low 
level `TimeSeriesSegment` object.  

Time-series are divided into **segments** in storage, each containing several consecutive entries. The 
number of entries per segment can vary widely, depending on the size and compressibility of the entries. 
Segments are up to 2 kb. If there is a gap of 25 days or more between two consecutive entries, they are 
always stored in different segments.  

Segments are useful because they can be referenced within time-series indexes to access the entries in 
the segment, as well as some aggregate values that summarize the data in the segment:  

| Property | Type | Description |
| - | - | - |
| DocumentId | string | The [ID](../../client-api/document-identifiers/working-with-document-identifiers) of the document this time-series belongs to |
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
Creating a time-series index using `TimeSeriesIndexDefinition`:
{CODE indexes_IndexDefinition@DocumentExtensions\TimeSeries\Indexing.cs /}

Creating a time-series index using `AbstractTimeSeriesIndexCreationTask`:
{CODE indexes_CreationTask@DocumentExtensions\TimeSeries\Indexing.cs /}

Creating a multi-map time-series index:
{CODE indexes_MultiMapCreationTask@DocumentExtensions\TimeSeries\Indexing.cs /}

Creating a map-reduce index:
{CODE indexes_MapReduce@DocumentExtensions\TimeSeries\Indexing.cs /}

Creating a builder for time-series indexes using `TimeSeriesIndexDefinitionBuilder`, and using it to create an index.
{CODE indexes_IndexDefinitionBuilder@DocumentExtensions\TimeSeries\Indexing.cs /}
{PANEL/}

## Related articles  
**Studio Articles**:  
[Studio Time Series Management]()  

**Indexes**:  
[What are Indexes](../../indexes/what-are-indexes)  
[Creating and Deploying Indexes](../../indexes/creating-and-deploying)  
[Map Indexes](../../indexes/map-indexes)  
[Creating and Deploying Indexes](../../indexes/creating-and-deploying)  
[Multi-Map Indexes](../../indexes/multi-map-indexes)  
[Map-Reduce Indexes](../../indexes/map-reduce-indexes)  
[Indexing Related Documents](../../indexes/indexing-related-documents)  

**Client-API**:  
[Working with Document IDs](../../client-api/document-identifiers/working-with-document-identifiers)  

[Time Series Operations]()  
