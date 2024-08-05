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

    * Time series indexes process [segments](../../document-extensions/timeseries/design#segmentation) that contain time series entries.  
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
      for static [javascript indexes](../../indexes/javascript-indexes).  

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
{CODE-TAB:python:Map_index index_1@DocumentExtensions\TimeSeries\Indexing.py /}
{CODE-TAB:python:JS_index index_3@DocumentExtensions\TimeSeries\Indexing.py /}
{CODE-TAB:python:IndexDefinition index_definition_1@DocumentExtensions\TimeSeries\Indexing.py /}
{CODE-TAB:python:IndexDefinition_builder index_definition_2@DocumentExtensions\TimeSeries\Indexing.py /}
    {CODE-TABS/}

* Querying this index, you can retrieve the indexed time series data while filtering by any of the index-fields.
 
    {CODE-TABS}
{CODE-TAB:python:Query_example_1 query_1@DocumentExtensions\TimeSeries\Indexing.py /}
{CODE-TAB-BLOCK:sql:RQL_1}
from index "StockPriceTimeSeriesFromCompanyCollection"
where "CompanyID" == "Comapnies/91-A"
{CODE-TAB-BLOCK/}
{CODE-TAB:python:Query_example_2 query_2@DocumentExtensions\TimeSeries\Indexing.py /}
{CODE-TAB-BLOCK:sql:RQL_2}
from index "StockPriceTimeSeriesFromCompanyCollection"
where "TradeVolume" > 150_000_000
select distinct CompanyID
{CODE-TAB-BLOCK/}
    {CODE-TABS/}

---

#### Multi-Map index - index time series from several collections:

{CODE-TABS}
{CODE-TAB:python:Multi_Map_index index_6@DocumentExtensions\TimeSeries\Indexing.py /}
{CODE-TABS/}

---

#### Map-Reduce index:

{CODE-TABS}
{CODE-TAB:python:Map_Reduce_index index_7@DocumentExtensions\TimeSeries\Indexing.py /}
{CODE-TABS/}

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
