# Time-Series Indexing  
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
  * [Syntax](../../document-extensions/timeseries/indexing#syntax)  
      * [`AbstractTimeSeriesIndexCreationTask`](../../document-extentions/timeseries/indexes#section)  
      * [`AbstractMultiMapMultiMapIndexCreationTask`](../../document-extensions/timeseries/indexing#section-1)
      * [`TimeSeriesIndexDefinition`](../../document-extensions/timeseries/indexing#section-2)
      * [`TimeSeriesSegment` Object](../../document-extensions/timeseries/indexing#object)
  * [Examples](../../document-extensions/timeseries/indexing#examples)  

{NOTE/}

---

{PANEL: Syntax }

The are two main ways to create a time-series index:  

1. Create a class that inherits from   
  * [`AbstractTimeSeriesIndexCreationTask`](../../document-extensions/timeseries/indexing#section) for [map](../../indexes/map-indexes) and 
  [map-reduce](../../indexes/map-reduce-indexes) time-series indexes.  
  * [`AbstractMultiMapMultiMapIndexCreationTask`](../../document-extensions/timeseries/indexing#section-1) for [multi-map](../../indexes/multi-map-indexes) time-series indexes.  

2. Create a [`TimeSeriesIndexDefinition`](../../document-extensions/timeseries/indexing#section-2).

---

### `AbstractTimeSeriesIndexCreationTask`  

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
| `AddMap()` | `string timeseries`, `Expression map` | Sets a map function for all time-series in the database with specified name (the first parameter) |
| `AddMapForAll()` | `Expression map` | Sets a map function for all time-series in the database |

See the example [below](../../document-extensions/timeseries/indexing#section-4).

---

### `AbstractMultiMapTimeSeriesIndexCreationTask`

{CODE-BLOCK: csharp}
public abstract class AbstractMultiMapTimeSeriesIndexCreationTask
{
    protected void AddMap<TSource>(string timeSeries, 
                          Expression<Func<IEnumerable<TimeSeriesSegment>, 
                                          IEnumerable>> map);

    protected void AddMapForAll<TBase>(
                          Expression<Func<IEnumerable<TimeSeriesSegment>, 
                                          IEnumerable>> map);
}

// Specify a type for the reduce result  
public abstract class AbstractMultiMapTimeSeriesIndexCreationTask<TReduceResult> { }
{CODE-BLOCK/}

| Method | Parameters | Description |
| - | - | - |
| `AddMap<TSource>()` | `string timeseries`, `Expression map` | Sets a map function for all time-series with the specified name (the first parameter) that belong to documents with the type `TSource` |
| `AddMapForAll<TBase>()` | `Expression map` | Sets a map function for all time-series that belong to documents with either the type `TBase` _or_ any type that inherits from `TBase` |

See the example [below](../../document-extensions/timeseries/indexing#section-5).

---

### `TimeSeriesIndexDefinition`

{CODE-BLOCK: csharp}
public class TimeSeriesIndexDefinition : IndexDefinition
{CODE-BLOCK/}

For now, `TimeSeriesIndexDefinition` is functionally equivalent to the 
[normal `IndexDefinition`](../../indexes/creating-and-deploying#using-maintenance-operations). 
Using it for time-series indexes is recommended - it exists in case additional functionality is 
added in future versions of RavenDB. 

See the example [below](../../document-extensions/timeseries/indexing#section-3).  

---

### `TimeSeriesSegment` object  

When indexing time-series entries, they are accessed through a subdivision of the time-series 
called a _segment_. In general the LINQ syntax looks somethintg like this:  

{CODE-BLOCK: sql}
from segment in timeseries
from entry in segment
{CODE-BLOCK/}

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

These are the properties of a `TimeSeriesEntry`, which are exposed in the LINQ syntax:  

{CODE-BLOCK: csharp}
public class TimeSeriesEntry
{
    public DateTime Timestamp;
    public string Tag;
    public double[] Values;

    // This is exactly equivalent to Values[0]
    public double Value;
}
{CODE-BLOCK/}
{PANEL/}

{PANEL: Examples}
Creating a time-series index using `TimeSeriesIndexDefinition`:
{CODE indexes_IndexDefinition@DocumentExtensions\TimeSeries\Indexing.cs /}

#### `AbstractTimeSeriesIndexCreationTask`
{CODE indexes_CreationTask@DocumentExtensions\TimeSeries\Indexing.cs /}

#### `AbstractMultiMapTimeSeriesIndexCreationTask`
{CODE indexes_MultiMapCreationTask@DocumentExtensions\TimeSeries\Indexing.cs /}

#### Map-Reduce Time-Series Index
{CODE indexes_MapReduce@DocumentExtensions\TimeSeries\Indexing.cs /}

Yet another way to create a time-series index is to create a 
`TimeSeriesIndexDefinitionBuilder`, and use it to create a 
`TimeSeriesIndexDefinition`.  
{CODE indexes_IndexDefinitionBuilder@DocumentExtensions\TimeSeries\Indexing.cs /}
{PANEL/}

## Related articles  

### Indexes  
[What are Indexes](../../indexes/what-are-indexes)  
[Creating and Deploying Indexes](../../indexes/creating-and-deploying)  
[Map Indexes](../../indexes/map-indexes)  
[Creating and Deploying Indexes](../../indexes/creating-and-deploying)  
[Multi-Map Indexes](../../indexes/multi-map-indexes)  
[Map-Reduce Indexes](../../indexes/map-reduce-indexes)  
[Indexing Related Documents](../../indexes/indexing-related-documents)  

### Client-API**  
[Working with Document IDs](../../client-api/document-identifiers/working-with-document-identifiers)  
