# Indexing: Time Series

---

{NOTE: }

Time series indexes process time series [segments](../../document-extensions/timeseries/design#segmentation), 
rather than document fields.  
The API for creating time series indexes is very similar to (and it inherits 
from) the API for [creating document indexes](../../indexes/creating-and-deploying).  

{INFO: }
Unlike document indexes, time series indexes currently support only LINQ syntax. 
JavaScript syntax is not supported.  

RavenDB does not create [dynamic](../../studio/database/indexes/indexes-overview#indexes-types) 
time series indexes in response to queries, but can be created as 
[static](../../studio/database/indexes/indexes-overview#indexes-types) indexes from a 
client application or from the Studio.  
{INFO/}

* In this page:  
  * [Syntax](../../document-extensions/timeseries/indexing#syntax)  
      * [`AbstractTimeSeriesIndexCreationTask`](../../document-extensions/timeseries/indexing#abstracttimeseriesindexcreationtask)  
      * [`AbstractMultiMapTimeSeriesIndexCreationTask`](../../document-extensions/timeseries/indexing#abstractmultimaptimeseriesindexcreationtask)  
      * [`AbstractJavaScriptTimeSeriesIndexCreationTask`](../../document-extensions/timeseries/indexing#abstractjavascripttimeseriesindexcreationtask)  
      * [`TimeSeriesIndexDefinition`](../../document-extensions/timeseries/indexing#timeseriesindexdefinition)  
      * [`TimeSeriesSegment` Object](../../document-extensions/timeseries/indexing#timeseriessegment-object)  
  * [Samples](../../document-extensions/timeseries/indexing#samples)  

{NOTE/}

---

{PANEL: Syntax }

There are two main ways to create a time series index:  

1. Create a class that inherits from one of the abstract index creation task classes:   
  * [`AbstractTimeSeriesIndexCreationTask`](../../document-extensions/timeseries/indexing#abstracttimeseriesindexcreationtask) for [map](../../indexes/map-indexes) and 
  [map-reduce](../../indexes/map-reduce-indexes) time series indexes.  
  * [`AbstractMultiMapTimeSeriesIndexCreationTask`](../../document-extensions/timeseries/indexing#abstractmultimaptimeseriesindexcreationtask) for [multi-map](../../indexes/multi-map-indexes) time series indexes.  
  * [`AbstractJavaScriptTimeSeriesIndexCreationTask`](../../document-extensions/timeseries/indexing#abstractjavascripttimeseriesindexcreationtask) for static javascript indexes.  

2. Create a [`TimeSeriesIndexDefinition`](../../document-extensions/timeseries/indexing#timeseriesindexdefinition).

---

### AbstractTimeSeriesIndexCreationTask  

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

// Index only time series that belong to documents
// of a specified type
public class AbstractTimeSeriesIndexCreationTask<TDocument> { }

// Specify both a document type and a reduce type
public class AbstractTimeSeriesIndexCreationTask<TDocument, TReduceResult> { }
{CODE-BLOCK/}

| Method | Parameters | Description |
| - | - | - |
| `AddMap()` | `string timeseries`, `Expression map` | Sets a map function for all time series in the database with specified name (the first parameter) |
| `AddMapForAll()` | `Expression map` | Sets a map function for all time series in the database |

See the example [below](../../document-extensions/timeseries/indexing#abstracttimeseriesindexcreationtask-1).

---

### AbstractMultiMapTimeSeriesIndexCreationTask  

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
| `AddMap<TSource>()` | `string timeseries`, `Expression map` | Sets a map function for all time series with the specified name (the first parameter) that belong to documents with the type `TSource` |
| `AddMapForAll<TBase>()` | `Expression map` | Sets a map function for all time series that belong to documents with either the type `TBase` _or_ any type that inherits from `TBase` |

See the example [below](../../document-extensions/timeseries/indexing#abstractmultimaptimeseriesindexcreationtask-1).

---

### AbstractJavaScriptTimeSeriesIndexCreationTask

{CODE-BLOCK: csharp}
public abstract class AbstractJavaScriptTimeSeriesIndexCreationTask : AbstractTimeSeriesIndexCreationTask
{
    public HashSet<string> Maps;
    protected string Reduce;
}
{CODE-BLOCK/}

| Property | Type | Description |
| - | - | - |
| `Maps` | `HashSet<string>` | The set of javascript map functions |
| `Reduce` | `string` | The javascript reduce function |

See the example [below](../../document-extensions/timeseries/indexing#abstractjavascripttimeseriesindexcreationtask-1).  
Learn more about [JavaScript indexes](../../indexes/javascript-indexes).  

---

### TimeSeriesIndexDefinition  

{CODE-BLOCK: csharp}
public class TimeSeriesIndexDefinition : IndexDefinition
{CODE-BLOCK/}

For now, `TimeSeriesIndexDefinition` is functionally equivalent to the 
[normal `IndexDefinition`](../../indexes/creating-and-deploying#using-maintenance-operations). 
Using it for time series indexes is recommended - it exists in case additional functionality is 
added in future versions of RavenDB. 

See the example [below](../../document-extensions/timeseries/indexing#samples).  

---

### TimeSeriesSegment Object  

Time series entries are indexes through the [segment](../../document-extensions/timeseries/design#segmentation) 
they are stored in, using LINQ syntax that resembles this one:  

{CODE-BLOCK: sql}
from segment in timeseries
from entry in segment
{CODE-BLOCK/}

Segment properties include the entries data, and the aggregated values that 
RavenDB automatically updates in the segment's header.  

| Property | Type | Description |
| - | - | - |
| DocumentId | string | The [ID](../../client-api/document-identifiers/working-with-document-identifiers) of the document this time series belongs to |
| Name | string | The name of the time series this segment belongs to |
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

{PANEL: Samples}
<br/>
Creating a time series index using `TimeSeriesIndexDefinition`:  
{CODE indexes_IndexDefinition@DocumentExtensions\TimeSeries\Indexing.cs /}

#### AbstractTimeSeriesIndexCreationTask  
{CODE indexes_CreationTask@DocumentExtensions\TimeSeries\Indexing.cs /}

#### AbstractMultiMapTimeSeriesIndexCreationTask  
{CODE indexes_MultiMapCreationTask@DocumentExtensions\TimeSeries\Indexing.cs /}

#### AbstractJavaScriptTimeSeriesIndexCreationTask  
{CODE indexes_AbstractJavaScriptCreationTask@DocumentExtensions\TimeSeries\Indexing.cs /}

#### Map-Reduce Time Series Index  
{CODE indexes_MapReduce@DocumentExtensions\TimeSeries\Indexing.cs /}

Yet another way to create a time series index is to create a 
`TimeSeriesIndexDefinitionBuilder`, and use it to create a 
`TimeSeriesIndexDefinition`.  
{CODE indexes_IndexDefinitionBuilder@DocumentExtensions\TimeSeries\Indexing.cs /}
{PANEL/}

## Related articles  

### Time Series  
[Time Series Overview](../../document-extensions/timeseries/overview)  
[API Overview](../../document-extensions/timeseries/client-api/overview)  

### Indexes  
[What are Indexes](../../indexes/what-are-indexes)  
[Creating and Deploying Indexes](../../indexes/creating-and-deploying)  
[Map Indexes](../../indexes/map-indexes)  
[Multi-Map Indexes](../../indexes/multi-map-indexes)  
[Map-Reduce Indexes](../../indexes/map-reduce-indexes)  
[JavaScript Indexes](../../indexes/javascript-indexes)  
[Indexing Related Documents](../../indexes/indexing-related-documents)  

### Client-API  
[Working with Document IDs](../../client-api/document-identifiers/working-with-document-identifiers)  
