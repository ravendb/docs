# Count Query Results  

---

{NOTE: }

* The following options are available to __count query results__:

    * [count](../../../client-api/session/querying/how-to-count-query-results#count)
    * [Get number of results from query stats](../../../client-api/session/querying/how-to-count-query-results#get-count-from-query-stats)

{NOTE/}

---

{PANEL: count}

Count query results using the `count` method.  

{CODE-TABS}
{CODE-TAB:python:Query count_3@ClientApi\Session\Querying\CountQueryResults.py /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
where ShipTo.Country == "UK" limit 0, 0

// The RQL generated will trigger query execution
// however, no documents are returned (limit is set 0)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Get count from query stats}

When executing a query, you can retrieve the query statistics which include the total number of results.
The number of results is available in the `QueryStatistics` object.  
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
