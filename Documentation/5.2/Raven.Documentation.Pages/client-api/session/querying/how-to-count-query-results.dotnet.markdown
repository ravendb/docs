# Count Query Results  

---

{NOTE: }

To count query results, use the `Count` and `LongCount` methods 
as demonstrated below.  

{INFO: }
Read [here](../../../client-api/session/querying/how-to-get-query-statistics) 
how to retrieve query statistics, including the counting statistics described 
here and many others, using the `Statistics` method and references to 
`QueryStatistics` properties.  
{INFO/}

* In This Page:  
    * [Count](../../../client-api/session/querying/how-to-count-query-results#count)  
    * [LongCount](../../../client-api/session/querying/how-to-count-query-results#longcount)  
    * [Get count from stats](../../../client-api/session/querying/how-to-count-query-results#get-count-from-stats)

{NOTE/}

---

{PANEL: Count}

To count the number of items returned by a query where an `Int32` 
variable is expected to be sufficient for the resulting number, use 
`Count` in a synchronous session or `CountAsync` in an async session.  

{NOTE: }
`Count` and `CountAsync` are implemented in `System.Linq`.  
`CountAsync` is also implemented by RavenDB.  
Make sure you include in your project the library whose 
`CountAsync` version you want to use.  
{NOTE/}

### Exception
If the number of items returned by the query exceeds `Int32.MaxValue`, 
an `OverflowException` will be thrown.  

### Example 
{CODE-TABS}
{CODE-TAB:csharp:Count Count@ClientApi\Session\Querying\CountQueryResults.cs /}
{CODE-TAB:csharp:Count_async CountAsync@ClientApi\Session\Querying\CountQueryResults.cs /}
{CODE-TABS/}

{NOTE: }
The query results count provided by `Count` can also be provided using 
`QueryStatistics.TotalResults`, as demonstrated [here](../../../client-api/session/querying/how-to-get-query-statistics#example).  
{NOTE/}

{PANEL/}

{PANEL: LongCount}

To count query results where an `Int64` variable is needed for 
the result, use `LongCount` in a synchronous session or `LongCountAsync` 
in an async session.  

{NOTE: }
`LongCount` and `LongCountAsync` are implemented in `System.Linq` 
as well as by RavenDB. Make sure you include in your project the 
library whose `LongCount` and `LongCountAsync` versions you 
want to use.  
{NOTE/}

### Example
{CODE-TABS}
{CODE-TAB:csharp:LongCount LongCount@ClientApi\Session\Querying\CountQueryResults.cs /}
{CODE-TAB:csharp:LongCount_async LongCountAsync@ClientApi\Session\Querying\CountQueryResults.cs /}
{CODE-TABS/}

{NOTE: }
The query results count provided by `LongCount` can also be provided using 
`QueryStatistics.LongTotalResults`, as explained [here](../../../client-api/session/querying/how-to-get-query-statistics).  
{NOTE/}

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
