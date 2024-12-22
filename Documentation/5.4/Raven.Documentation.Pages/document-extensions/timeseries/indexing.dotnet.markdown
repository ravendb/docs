# Indexing Time Series
---

{NOTE: }

* [Static](../../studio/database/indexes/indexes-overview#index-types) time series indexes can be created from your client application or from the Studio.

* Indexing allows for fast retrieval of the indexed time series data when querying a time series.

* In this page:  
  * [Time series indexes vs Document indexes](../../document-extensions/timeseries/indexing#time-series-indexes-vs-document-indexes)
  * [Ways to create a time series index](../../document-extensions/timeseries/indexing#ways-to-create-a-time-series-index)
  * [Examples of time series indexes](../../document-extensions/timeseries/indexing#examples-of-time-series-indexes)
      * [Map index - index single time series from single collection](../../document-extensions/timeseries/indexing#map-index---index-single-time-series-from-single-collection)
      * [Map index - index all time series from single collection](../../document-extensions/timeseries/indexing#map-index---index-all-time-series-from-single-collection)
      * [Map index - index all time series from all collections](../../document-extensions/timeseries/indexing#map-index---index-all-time-series-from-all-collections)
      * [Multi-Map index - index time series from several collections](../../document-extensions/timeseries/indexing#multi-map-index---index-time-series-from-several-collections)
      * [Map-Reduce index](../../document-extensions/timeseries/indexing#map-reduce-index)
  * [Syntax](../../document-extensions/timeseries/indexing#syntax)

{NOTE/}

---

{PANEL: Time series indexes vs Document indexes}

#### Auto-Indexes:

* Time series index:  
  Dynamic time series indexes are Not created in response to queries.

* Document index:  
  [Auto-indexes](../../studio/database/indexes/indexes-overview#indexes-types) are created in response to dynamic queries.

---

#### Data source:

* Time series index:

    * Time series indexes process **[segments](../../document-extensions/timeseries/design#segmentation)** that contain time series entries.  
      The entries are indexed through the segment they are stored in, for example, using a LINQ syntax that resembles this one:

      {CODE-BLOCK:sql}
from segment in timeseries
from entry in segment
...
      {CODE-BLOCK/}

    * The following items can be indexed per index-entry in a time series index:
        * Values & timestamp of a time series entry
        * The entry tag
        * Content from a document referenced by the tag
        * Properties of the containing segment (see **[`TimeSeriesSegment`](../../document-extensions/timeseries/indexing#section-5)**)

* Document index:

    * The index processes fields from your JSON documents.  
      Documents are indexed through the collection they belong to, for example, using this LINQ syntax:

      {CODE-BLOCK:sql}
from employee in employees
...
      {CODE-BLOCK/}

---

#### Query results:

* Time series index:  
  When [querying](../../document-extensions/timeseries/querying/using-indexes) a time series index, each result item corresponds to the type defined by the **index-entry** in the index definition,
  (unless results are [projected](../../document-extensions/timeseries/querying/using-indexes#project-results)). The documents themselves are not returned.

* Document index:  
  The resulting objects are the document entities (unless results are [projected](../../indexes/querying/projections)).

{PANEL/}

{PANEL: Ways to create a time series index}

There are two main ways to create a time series index:

1. Create a class that inherits from one of the following abstract index creation task classes:
    * [`AbstractTimeSeriesIndexCreationTask`](../../document-extensions/timeseries/indexing#section)
      for [map](../../indexes/map-indexes) and [map-reduce](../../indexes/map-reduce-indexes) time series indexes.
    * [`AbstractMultiMapTimeSeriesIndexCreationTask`](../../document-extensions/timeseries/indexing#section-1)
      for [multi-map](../../indexes/multi-map-indexes) time series indexes.
    * [`AbstractJavaScriptTimeSeriesIndexCreationTask`](../../document-extensions/timeseries/indexing#section-2)
      for static [javascript indexes](../../indexes/javascript-indexes).

2. Deploy a time series index definition via [PutIndexesOperation](../../client-api/operations/maintenance/indexes/put-indexes):
   * Create a [`TimeSeriesIndexDefinition`](../../document-extensions/timeseries/indexing#section-3) directly.  
   * Create a strongly typed index definition using [`TimeSeriesIndexDefinitionBuilder`](../../document-extensions/timeseries/indexing#section-4).  

{PANEL/}

{PANEL: Examples of time series indexes}

#### Map index - index single time series from single collection:

* In this index, we index data from the "StockPrices" time series entries in the "Companies" collection (`TradeVolume`, `Date`).   

* In addition, we index the containing document id (`DocumentID`), which is obtained from the segment,  
  and some content from the document referenced by the entry's Tag (`EmployeeName`).
 
* Each tab below presents one of the different [ways](../../document-extensions/timeseries/indexing#ways-to-create-a-time-series-index) the index can be defined.

    {CODE-TABS}
{CODE-TAB:csharp:Map_index index_1@DocumentExtensions\TimeSeries\Indexing.cs /}
{CODE-TAB:csharp:NonTyped_index index_2@DocumentExtensions\TimeSeries\Indexing.cs /}
{CODE-TAB:csharp:JS_index index_3@DocumentExtensions\TimeSeries\Indexing.cs /}
{CODE-TAB:csharp:IndexDefinition index_definition_1@DocumentExtensions\TimeSeries\Indexing.cs /}
{CODE-TAB:csharp:IndexDefinition_builder index_definition_2@DocumentExtensions\TimeSeries\Indexing.cs /}
    {CODE-TABS/}

* Querying this index, you can retrieve the indexed time series data while filtering by any of the index-fields.
 
    {CODE-TABS}
{CODE-TAB:csharp:Query_example_1 query_1@DocumentExtensions\TimeSeries\Indexing.cs /}
{CODE-TAB-BLOCK:sql:RQL_1}
from index "StockPriceTimeSeriesFromCompanyCollection"
where "CompanyID" == "Comapnies/91-A"
{CODE-TAB-BLOCK/}
{CODE-TAB:csharp:Query_example_2 query_2@DocumentExtensions\TimeSeries\Indexing.cs /}
{CODE-TAB-BLOCK:sql:RQL_2}
from index "StockPriceTimeSeriesFromCompanyCollection"
where "TradeVolume" > 150_000_000
select distinct CompanyID
{CODE-TAB-BLOCK/}
    {CODE-TABS/}

---

#### Map index - index all time series from single collection:

{CODE-TABS}
{CODE-TAB:csharp:Map_index index_4@DocumentExtensions\TimeSeries\Indexing.cs /}
{CODE-TABS/}

---

#### Map index - index all time series from all collections:

{CODE-TABS}
{CODE-TAB:csharp:Map_index index_5@DocumentExtensions\TimeSeries\Indexing.cs /}
{CODE-TABS/}

---

#### Multi-Map index - index time series from several collections:

{CODE-TABS}
{CODE-TAB:csharp:Multi_Map_index index_6@DocumentExtensions\TimeSeries\Indexing.cs /}
{CODE-TABS/}

---

#### Map-Reduce index:

{CODE-TABS}
{CODE-TAB:csharp:Map_Reduce_index index_7@DocumentExtensions\TimeSeries\Indexing.cs /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

---

### `AbstractTimeSeriesIndexCreationTask`

{CODE-BLOCK: csharp}
// To define a Map index inherit from:
// ===================================
public abstract class AbstractTimeSeriesIndexCreationTask<TDocument> { }
// Time series that belong to documents of the specified `TDocument` type will be indexed. 

// To define a Map-Reduce index inherit from:
// ==========================================
public abstract class AbstractTimeSeriesIndexCreationTask<TDocument, TReduceResult> { }
// Specify both the document type and the reduce type

// Methods available in AbstractTimeSeriesIndexCreationTask class:
// ===============================================================

// Set a map function for the specified time series
protected void AddMap(string timeSeries,
    Expression<Func<IEnumerable<TimeSeriesSegment>, IEnumerable>> map);

// Set a map function for all time series 
protected void AddMapForAll(
    Expression<Func<IEnumerable<TimeSeriesSegment>, IEnumerable>> map);
{CODE-BLOCK/}

---

### `AbstractMultiMapTimeSeriesIndexCreationTask`

{CODE-BLOCK: csharp}
// To define a Multi-Map index inherit from:
// =========================================
public abstract class AbstractMultiMapTimeSeriesIndexCreationTask { }

// Methods available in AbstractMultiMapTimeSeriesIndexCreationTask class:
// =======================================================================

// Set a map function for all time series with the specified name
// that belong to documents of type `TSource`
protected void AddMap<TSource>(string timeSeries,
    Expression<Func<IEnumerable<TimeSeriesSegment>, IEnumerable>> map);

// Set a map function for all time series that belong to documents of type `TBase`
// or any type that inherits from `TBase`
protected void AddMapForAll<TBase>(
    Expression<Func<IEnumerable<TimeSeriesSegment>,IEnumerable>> map);
{CODE-BLOCK/}

---

### `AbstractJavaScriptTimeSeriesIndexCreationTask`
 
{CODE-BLOCK: csharp}
// To define a JavaScript index inherit from:
// ==========================================
public abstract class AbstractJavaScriptTimeSeriesIndexCreationTask
{    
    public HashSet<string> Maps; // The set of JavaScript map functions for this index
    protected string Reduce;     // The JavaScript reduce function
}
{CODE-BLOCK/}

Learn more about JavaScript indexes in [JavaScript Indexes](../../indexes/javascript-indexes).

---

### `TimeSeriesIndexDefinition`

{CODE-BLOCK: csharp}
public class TimeSeriesIndexDefinition : IndexDefinition
{CODE-BLOCK/}

While `TimeSeriesIndexDefinition` is currently functionally equivalent to the regular [`IndexDefinition`](../../client-api/operations/maintenance/indexes/put-indexes#put-indexes-operation-with-indexdefinition) class from which it inherits,
it is recommended to use `TimeSeriesIndexDefinition` when creating a time series index definition in case additional functionality is added in future versions of RavenDB.

---

### `TimeSeriesIndexDefinitionBuilder`

{CODE-BLOCK: csharp}
public class TimeSeriesIndexDefinitionBuilder<TDocument>
{ 
    public TimeSeriesIndexDefinitionBuilder(string indexName = null)  
}
{CODE-BLOCK/}

{WARNING: }
**Note**:  

* Currently, class `TimeSeriesIndexDefinitionBuilder` does Not support API methods from abstract class `AbstractCommonApiForIndexes`,
  such as `LoadDocument` or `Recurse`.

* Use one of the other index creation methods if needed.   
{WARNING/}

---

### `TimeSeriesSegment`

* Segment properties include the entries data and aggregated values that RavenDB automatically updates in the segment's header.

* The following segment properties can be indexed:

    {CODE-BLOCK: csharp}
public sealed class TimeSeriesSegment
{
    // The ID of the document this time series belongs to
    public string DocumentId { get; set; }
 
    // The name of the time series this segment belongs to
    public string Name { get; set; }
  
    // The smallest values from all entries in the segment
    // The first array item is the Min of all first values, etc.
    public double[] Min { get; set; }

    // The largest values from all entries in the segment
    // The first array item is the Max of all first values, etc.
    public double[] Max { get; set; }
  
    // The sum of all values from all entries in the segment 
    // The first array item is the Sum of all first values, etc.
    public double[] Sum { get; set; }
  
    // The number of entries in the segment
    public int Count { get; set; }
  
    // The timestamp of the first entry in the segment
    public DateTime Start { get; set; }
  
    // The timestamp of the last entry in the segment
    public DateTime End { get; set; }
  
    // The segment's entries themselves
    public TimeSeriesEntry[] Entries { get; set; }
}
    {CODE-BLOCK/}

* These are the properties of a `TimeSeriesEntry` which can be indexed:

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
