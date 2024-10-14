# Count Query Results  

---

{NOTE: }

* The following options are available to **count query results**:

    * [`count`](../../../client-api/session/querying/how-to-count-query-results#count)
    * [Get number of results from query stats](../../../client-api/session/querying/how-to-count-query-results#get-count-from-query-stats)

{NOTE/}

---

{PANEL: `count`}

Count query results using the `count` method.  

{CODE-TABS}
{CODE-TAB:python:Query count_3@ClientApi\Session\Querying\CountQueryResults.py /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
where ship_to.country == "UK" limit 0, 0

// The RQL generated will trigger query execution
// however, no documents are returned (limit is set 0)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Get count from query stats}

When executing a query, you can retrieve query statistics that include the total number of results.  
To do this, define a callback function that takes `QueryStatistics` as an argument and applies whatever 
logic you want to apply.  

{CODE-BLOCK:python}
def **statistics_callback(statistics: QueryStatistics) -> None:
   # Read and interact with QueryStatistics here
   total_results = statistics.total_results
   duration_milliseconds = statistics.duration_in_ms
   ...
{CODE-BLOCK/}

Then pass your function as an argument to the `query.statistics` method and use the retrieved `QueryStatistics` object.  

{CODE-BLOCK:python}
   employees = list(
   session.query(object_type=Employee)
   .where_equals("first_name", "Robert")
   .statistics(**statistics_callback)
)
{CODE-BLOCK/}

Learn more in [Get Query Statistics](../../../client-api/session/querying/how-to-get-query-statistics).  

{PANEL/}

## Related Articles

### Client API

- [Query overview](../../../client-api/session/querying/how-to-query)  
- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)  
- [Filter by Field Presence](../../../client-api/session/querying/how-to-filter-by-field)  
- [Get Query Statistics](../../../client-api/session/querying/how-to-get-query-statistics)  

### Querying

- [Filtering](../../../indexes/querying/filtering)   
- [RQL - Raven Query Language](../../../client-api/session/querying/what-is-rql) 
