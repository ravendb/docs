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
      The entries are indexed through the segment they are stored in.
    * The following items can be indexed per index-entry in a time series index:
        * Values & timestamp of a time series entry
        * The entry tag
        * Content from a document referenced by the tag
        * Properties of the containing segment (see **[segment properties](../../document-extensions/timeseries/indexing#segment-properties)**)

* Document index:

    * The index processes fields from your JSON documents.  
      Documents are indexed through the collection they belong to.

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

1. Create a class that inherits from the abstract index class [`AbstractRawJavaScriptTimeSeriesIndexCreationTask`](../../document-extensions/timeseries/indexing#section).

2. Create a [`TimeSeriesIndexDefinition`](../../document-extensions/timeseries/indexing#section-1) 
   and deploy the time series index definition via [PutIndexesOperation](../../client-api/operations/maintenance/indexes/put-indexes).

{PANEL/}

{PANEL: Examples of time series indexes}

#### Map index - index single time series from single collection:

* In this index, we index data from the "StockPrices" time series entries in the "Companies" collection (`tradeVolume`, `date`).   

* In addition, we index the containing document id (`documentID`), which is obtained from the segment,  
  and some content from the document referenced by the entry's Tag (`employeeName`).
 
    {CODE-TABS}
{CODE-TAB:nodejs:Map_index index_1@documentExtensions\timeSeries\indexing.js /}
{CODE-TAB:nodejs:IndexDefinition index_definition_1@documentExtensions\timeSeries\indexing.js /}
    {CODE-TABS/}

* Querying this index, you can retrieve the indexed time series data while filtering by any of the index-fields.
 
    {CODE-TABS}
{CODE-TAB:nodejs:Query_example_1 query_1@documentExtensions\timeSeries\indexing.js /}
{CODE-TAB-BLOCK:sql:RQL_1}
from index "StockPriceTimeSeriesFromCompanyCollection"
where "companyID" == "Comapnies/91-A"
{CODE-TAB-BLOCK/}
{CODE-TAB:nodejs:Query_example_2 query_2@documentExtensions\timeSeries\indexing.js /}
{CODE-TAB-BLOCK:sql:RQL_2}
from index "StockPriceTimeSeriesFromCompanyCollection"
where "tradeVolume" > 150_000_000
select distinct companyID
{CODE-TAB-BLOCK/}
    {CODE-TABS/}

---

#### Map index - index all time series from single collection:

{CODE-TABS}
{CODE-TAB:nodejs:Map_index index_2@documentExtensions\timeSeries\indexing.js /}
{CODE-TABS/}

---

#### Map index - index all time series from all collections:

{CODE-TABS}
{CODE-TAB:nodejs:Map_index index_3@documentExtensions\timeSeries\indexing.js /}
{CODE-TABS/}

---

#### Multi-Map index - index time series from several collections:

{CODE-TABS}
{CODE-TAB:nodejs:Multi_Map_index index_4@documentExtensions\timeSeries\indexing.js /}
{CODE-TABS/}

---

#### Map-Reduce index:

{CODE-TABS}
{CODE-TAB:nodejs:Map_Reduce_index index_5@documentExtensions\timeSeries\indexing.js /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

---

### `AbstractRawJavaScriptTimeSeriesIndexCreationTask`
 
{CODE-BLOCK: javascript}
// To define a raw JavaScript index extend the following class:
// ============================================================
abstract class AbstractRawJavaScriptTimeSeriesIndexCreationTask
{    
    // The set of JavaScript map functions for this index
    maps; // Set<string>

    // The JavaScript reduce function
    reduce; // string
}
{CODE-BLOCK/}

---

### `TimeSeriesIndexDefinition`

{CODE-BLOCK: javascript}
class TimeSeriesIndexDefinition extends IndexDefinition
{CODE-BLOCK/}

While `TimeSeriesIndexDefinition` is currently functionally equivalent to the regular [`IndexDefinition`](../../client-api/operations/maintenance/indexes/put-indexes#put-indexes-operation-with-indexdefinition) class from which it inherits,
it is recommended to use `TimeSeriesIndexDefinition` when creating a time series index definition in case additional functionality is added in future versions of RavenDB.

---

### Segment properties

* Segment properties include the entries data and aggregated values that RavenDB automatically updates in the segment's header.

* **Unlike the C# client**, class `TimeSeriesSegment` is Not defined in the Node.js client.  
  However, the following are the segment properties that can be indexed from your raw javascript index definition which the server recognizes:  
  
    {CODE-BLOCK: javascript}
// The ID of the document this time series belongs to
DocumentId; // string

// The name of the time series this segment belongs to
Name; // string

// The smallest values from all entries in the segment
// The first array item is the Min of all first values, etc.
Min; // number[]

// The largest values from all entries in the segment
// The first array item is the Max of all first values, etc.
Max; // number[]

// The sum of all values from all entries in the segment
// The first array item is the Sum of all first values, etc.
Sum; // number[]

// The number of entries in the segment
Count; // number

// The timestamp of the first entry in the segment
Start; // Date

// The timestamp of the last entry in the segment
End; // Date

// The segment's entries themselves
Entries; // TimeSeriesEntry[]
    {CODE-BLOCK/}

* These are the properties of a `TimeSeriesEntry` which can be indexed:

    {CODE-BLOCK: javascript}
class TimeSeriesEntry
{
    timestamp; // Date
    tag;       // string
    values;    // number[]

    // This is equivalent to values[0]
    value;     // number
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
