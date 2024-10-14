# Count Query Results  

---

{NOTE: }

* The following options are available to **count query results**:

    * [Count](../../../client-api/session/querying/how-to-count-query-results#count)
  
    * [LongCount](../../../client-api/session/querying/how-to-count-query-results#longcount)
  
    * [Get number of results from query stats](../../../client-api/session/querying/how-to-count-query-results#get-count-from-query-stats)

{NOTE/}

---

{PANEL: Count}

* When the number of resulting items is expected to be an **`Int32`** variable,  
  use `Count` in a synchronous session (or `CountAsync` in an async session).  
  
* `Count` is implemented in `System.Linq`.  
  `CountAsync` is implemented in `Raven.Client.Documents`.  

* An `OverflowException` will be thrown if the number of items exceeds **`Int32.MaxValue`**. 

{NOTE: }

{CODE-TABS}
{CODE-TAB:csharp:Query count_1@ClientApi\Session\Querying\CountQueryResultsUsingLinq.cs /}
{CODE-TAB:csharp:Query_async count_2@ClientApi\Session\Querying\CountQueryResults.cs /}
{CODE-TAB:csharp:DocumentQuery count_3@ClientApi\Session\Querying\CountQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
where ShipTo.Country == "UK" limit 0, 0

// The RQL generated will trigger query execution
// however, no documents are returned (limit is set 0)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: LongCount}

* When the number of resulting items is expected to be an **`Int64`** variable,  
  use `LongCount` in a synchronous session (or `LongCountAsync` in an async session).
  
* `LongCount` is implemented in both `Raven.Client.Documents` & `System.Linq` (use as needed).  
  `LongCountAsync` is implemented in `Raven.Client.Documents`.

{NOTE: }

{CODE-TABS}
{CODE-TAB:csharp:Query count_4@ClientApi\Session\Querying\CountQueryResults.cs /}
{CODE-TAB:csharp:Query_async count_5@ClientApi\Session\Querying\CountQueryResults.cs /}
{CODE-TAB:csharp:DocumentQuery count_6@ClientApi\Session\Querying\CountQueryResults.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
where ShipTo.Country == "UK" limit 0, 0

// The RQL generated will trigger query execution
// however, no documents are returned (limit is set 0)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Get count from query stats}

* When executing a query,  
  you can retrieve the query statistics which include the total number of results.

* The number of results is available in the `QueryStatistics` object as:  
  * `TotalResults` - an Int32 value  
  * `LongTotalResults` - an Int64 value  

* Learn more in [Get Query Statistics](../../../client-api/session/querying/how-to-get-query-statistics). 

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
