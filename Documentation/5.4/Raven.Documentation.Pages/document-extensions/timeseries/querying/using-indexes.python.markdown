# Querying Time Series Indexes

---

{NOTE: }

* **Time series index**:

    * STATIC-time-series-indexes can be defined from the [Client API](../../../document-extensions/timeseries/indexing) 
      or using [Studio](../../../studio/database/indexes/create-map-index).  
      Such an index can be queried in the same way as a regular index that indexes documents.  
      (See [Querying an index](../../../indexes/querying/query-index)).
    
    * AUTO-time-series-indexes are Not generated automatically by the server when making a time series query.

* **The contents of the query results**:

    * Unlike a document index, where the source data are your JSON documents,  
      the source data for a time series index are the time series entries within the documents.

    * When querying a **document index**:  
      the resulting objects are the document entities (unless results are projected).
  
    * When querying a **time series index**:  
      each item in the results is of the type defined by the **index-entry** in the index definition,  
      (unless results are projected). 
      The documents themselves are not returned.

* In this page:
    * [Sample index](../../../document-extensions/timeseries/querying/using-indexes#sample-index)
    * [Querying the index](../../../document-extensions/timeseries/querying/using-indexes#querying-the-index)
        * [Query all time series entries](../../../document-extensions/timeseries/querying/using-indexes#query-all-time-series-entries)
        * [Filter query results](../../../document-extensions/timeseries/querying/using-indexes#filter-query-results)
        * [Order query results](../../../document-extensions/timeseries/querying/using-indexes#order-query-results)
        * [Project results](../../../document-extensions/timeseries/querying/using-indexes#project-results)
    * [Syntax](../../../document-extensions/timeseries/querying/using-indexes#syntax)

{NOTE/}

---

{PANEL: Sample Index}

* The following is a time series map-index that will be used in the query examples throughout this article.

* Each **index-entry** consists of:
  * Three index-fields obtained from the "HeartRates" time series entries: `BPM`, `Date`, and `Tag`.
  * One index-field obtained from the time series [segment](../../../document-extensions/timeseries/indexing#timeseriessegment-object) header: `EmployeeID`.
  * One index-field obtained from the loaded employee document: `EmployeeName`.

* When querying this time series index:  
  * The resulting items correspond to the time series entries that match the query predicate.  
  * Each item in the results will be of type `TsIndex.IndexEntry`, which is the index-entry.  
    Different result types may be returned when the query [projects the results](../../../document-extensions/timeseries/querying/using-indexes#project-results).

{CODE:python sample_ts_index@DocumentExtensions\TimeSeries\Querying\QueryingTsIndex.py /}

{PANEL/}

{PANEL: Querying the index} 
 
#### Query all time series entries:

No filtering is applied in this query.  
Results will include ALL entries from time series "HeartRates".

{CODE-TABS}
{CODE-TAB:python:Query query_index_1@DocumentExtensions\TimeSeries\Querying\QueryingTsIndex.py /}
{CODE-TAB:python:RawQuery query_index_4@DocumentExtensions\TimeSeries\Querying\QueryingTsIndex.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index "TsIndex"
{CODE-TAB-BLOCK/} 
{CODE-TABS/}

---

#### Filter query results:

In this example, time series entries are filtered by the query.  
The query predicate is applied to the index-fields.

{CODE-TABS}
{CODE-TAB:python:Query query_index_5@DocumentExtensions\TimeSeries\Querying\QueryingTsIndex.py /}
{CODE-TAB:python:RawQuery query_index_8@DocumentExtensions\TimeSeries\Querying\QueryingTsIndex.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index "TsIndex"
where EmployeeName == "Robert King" and BPM > 85.0
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Order query results:

Results can be ordered by any of the index-fields.

{CODE-TABS}
{CODE-TAB:python:Query query_index_9@DocumentExtensions\TimeSeries\Querying\QueryingTsIndex.py /}
{CODE-TAB:python:RawQuery query_index_12@DocumentExtensions\TimeSeries\Querying\QueryingTsIndex.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index "TsIndex"
where BPM < 58.0
order by Date desc
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Project results:

* Instead of returning the entire `TsIndex.IndexEntry` object for each result item,  
  you can return only partial fields.

* In this example, we query for time series entries with a very high BPM value.  
  We retrieve entries with BPM value > 100 but return only the _EmployeeID_ for each entry.

{CODE-TABS}
{CODE-TAB:python:Query query_index_13@DocumentExtensions\TimeSeries\Querying\QueryingTsIndex.py /}
{CODE-TAB:python:Projection_class employee_details_class@DocumentExtensions\TimeSeries\Querying\QueryingTsIndex.py /}
{CODE-TAB:python:RawQuery query_index_16@DocumentExtensions\TimeSeries\Querying\QueryingTsIndex.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index "TsIndex"
where BPM > 100.0
select distinct EmployeeID
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}
   
* [query](../../../client-api/session/querying/how-to-query#query-overview)  

    {CODE-BLOCK: JSON}
    def query(
    self, source: Optional[Query] = None, object_type: Optional[Type[_T]] = None
    ) -> DocumentQuery[_T]:
    ...
    {CODE-BLOCK/}

* [document_query](../../../client-api/session/querying/how-to-query#query-overview)  
    
    {CODE-BLOCK: JSON}
    def document_query(
    self,
    index_name: str = None,
    collection_name: str = None,
    object_type: Type[_T] = None,
    is_map_reduce: bool = False,
    ) -> DocumentQuery[_T]:
    ...
    {CODE-BLOCK/}

{PANEL/}

## Related articles

**Time Series Overview**  
[Time Series Overview](../../../document-extensions/timeseries/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../studio/database/document-extensions/time-series)  

**Time Series Indexing**  
[Time Series Indexing](../../../document-extensions/timeseries/indexing)  

**Time Series Queries**  
[Range Selection](../../../document-extensions/timeseries/querying/choosing-query-range)  
[Filtering](../../../document-extensions/timeseries/querying/filtering)  
[Aggregation and Projection](../../../document-extensions/timeseries/querying/aggregation-and-projections)  

**Policies**  
[Time Series Rollup and Retention](../../../document-extensions/timeseries/rollup-and-retention)  
