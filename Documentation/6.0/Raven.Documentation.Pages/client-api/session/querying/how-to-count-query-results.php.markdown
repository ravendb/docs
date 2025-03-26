# Count Query Results  

---

{NOTE: }

* The following options are available to **count query results**:

    * [`Count`](../../../client-api/session/querying/how-to-count-query-results#count)
    * [Get number of results from query stats](../../../client-api/session/querying/how-to-count-query-results#get-count-from-query-stats)

{NOTE/}

---

{PANEL: `Count`}

Count query results using the `Count` method.  

{CODE-TABS}
{CODE-TAB:php:documentQuery count_3@ClientApi\Session\Querying\CountQueryResults.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
where ShipTo.Country == "UK" limit 0, 0

// The RQL generated will trigger query execution
// however, no documents are returned (limit is set 0)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Get count from query stats}

When executing a query, you can retrieve the query statistics that include the total number of results.  
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
