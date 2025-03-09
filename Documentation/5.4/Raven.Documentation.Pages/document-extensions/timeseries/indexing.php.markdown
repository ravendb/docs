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

    * Time series indexes process **segments** that contain time series entries.  
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
        * Properties of the containing segment

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
  The resulting objects are the document entities (unless results are projected).

{PANEL/}

{PANEL: Ways to create a time series index}

There are two main ways to create a time series index:

1. Create a class that inherits from one of the following abstract index creation task classes:
    * `AbstractTimeSeriesIndexCreationTask`  
      for [map](../../indexes/map-indexes) and [map-reduce](../../indexes/map-reduce-indexes) time series indexes.  
    * `AbstractMultiMapTimeSeriesIndexCreationTask`  
      for [multi-map](../../indexes/multi-map-indexes) time series indexes.  
    * `AbstractJavaScriptTimeSeriesIndexCreationTask`  
      for static javascript indexes.  

2. Deploy a time series index definition via [PutIndexesOperation](../../client-api/operations/maintenance/indexes/put-indexes):
   * Create a `TimeSeriesIndexDefinition` directly.  
   * Create a strongly typed index definition using `TimeSeriesIndexDefinitionBuilder`.  

{PANEL/}

{PANEL: Examples of time series indexes}

#### Map index - index single time series from single collection:

* In this index, we index data from the "StockPrices" time series entries in the "Companies" collection (`TradeVolume`, `Date`).   

* In addition, we index the containing document id (`DocumentID`), which is obtained from the segment,  
  and some content from the document referenced by the entry's Tag (`EmployeeName`).
 
* Each tab below presents one of the different [ways](../../document-extensions/timeseries/indexing#ways-to-create-a-time-series-index) the index can be defined.

    {CODE-TABS}
{CODE-TAB:php:Map_index index_1@DocumentExtensions\TimeSeries\Indexing.php /}
{CODE-TAB:php:JS_index index_3@DocumentExtensions\TimeSeries\Indexing.php /}
{CODE-TAB:php:IndexDefinition index_definition_1@DocumentExtensions\TimeSeries\Indexing.php /}
{CODE-TAB:php:IndexDefinition_builder index_definition_2@DocumentExtensions\TimeSeries\Indexing.php /}
    {CODE-TABS/}

* Querying this index, you can retrieve the indexed time series data while filtering by any of the index-fields.
 
    {CODE-TABS}
{CODE-TAB:php:Query_example_1 query_1@DocumentExtensions\TimeSeries\Indexing.php /}
{CODE-TAB-BLOCK:sql:RQL_1}
from index "StockPriceTimeSeriesFromCompanyCollection"
where "CompanyID" == "Comapnies/91-A"
{CODE-TAB-BLOCK/}
{CODE-TAB:php:Query_example_2 query_2@DocumentExtensions\TimeSeries\Indexing.php /}
{CODE-TAB-BLOCK:sql:RQL_2}
from index "StockPriceTimeSeriesFromCompanyCollection"
where "TradeVolume" > 150_000_000
select distinct CompanyID
{CODE-TAB-BLOCK/}
    {CODE-TABS/}

---

#### Multi-Map index - index time series from several collections:

{CODE-TABS}
{CODE-TAB:php:Multi_Map_index index_6@DocumentExtensions\TimeSeries\Indexing.php /}
{CODE-TABS/}

---

#### Map-Reduce index:

{CODE-TABS}
{CODE-TAB:php:Map_Reduce_index index_7@DocumentExtensions\TimeSeries\Indexing.php /}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

---

### `AbstractJavaScriptTimeSeriesIndexCreationTask`
 
{CODE-BLOCK:php}
class AbstractJavaScriptTimeSeriesIndexCreationTask(AbstractIndexCreationTaskBase[TimeSeriesIndexDefinition]):
    def __init__(
        self,
        conventions: DocumentConventions = None,
        priority: IndexPriority = None,
        lock_mode: IndexLockMode = None,
        deployment_mode: IndexDeploymentMode = None,
        state: IndexState = None,
    ):
        super().__init__(conventions, priority, lock_mode, deployment_mode, state)
        self._definition = TimeSeriesIndexDefinition()

    @property
    def maps(self) -> Set[str]:
        return self._definition.maps

    @maps.setter
    def maps(self, maps: Set[str]):
        self._definition.maps = maps

    @property
    def reduce(self) -> str:
        return self._definition.reduce

    @reduce.setter
    def reduce(self, reduce: str):
        self._definition.reduce = reduce
{CODE-BLOCK/}

---

### `TimeSeriesIndexDefinition`

{CODE-BLOCK:php}
class TimeSeriesIndexDefinition(IndexDefinition):
    @property
    def source_type(self) -> IndexSourceType:
        return IndexSourceType.TIME_SERIES
{CODE-BLOCK/}

While `TimeSeriesIndexDefinition` is currently functionally equivalent to the regular 
[`IndexDefinition`](../../indexes/creating-and-deploying#using-maintenance-operations) 
class from which it inherits, it is recommended to use `TimeSeriesIndexDefinition` when 
creating a time series index definition in case additional functionality is added in 
future versions of RavenDB.

---

### `TimeSeriesIndexDefinitionBuilder`

{CODE-BLOCK:php}
class TimeSeriesIndexDefinitionBuilder(AbstractIndexDefinitionBuilder[TimeSeriesIndexDefinition]):
    def __init__(self, index_name: Optional[str] = None):
        super().__init__(index_name)
        self.map: Optional[str] = None
{CODE-BLOCK/}

---

### `TimeSeriesSegment`

* Segment properties include the entries data and aggregated values that RavenDB automatically updates in the segment's header.

* The following segment properties can be indexed:

    {CODE-BLOCK:php}
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

    {CODE-BLOCK:php}
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
