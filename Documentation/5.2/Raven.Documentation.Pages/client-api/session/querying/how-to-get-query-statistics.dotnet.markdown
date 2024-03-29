# Get Query Statistics

---

{NOTE: }

* Detailed __query statistics__ can be retrieved for every executed query using the `Statistics` method.  
  
* Stats such as query duration, number of results, index name used in the query, and more,  
  are returned in the `QueryStatistics` object.

* In This Page:  
   * [Get query statistics](../../../client-api/session/querying/how-to-get-query-statistics#get-query-statistics)  
   * [Syntax](../../../client-api/session/querying/how-to-get-query-statistics#syntax)  

{NOTE/}

---

{PANEL: Get query statistics}

{CODE-TABS}
{CODE-TAB:csharp:Query stats_1@ClientApi\Session\Querying\HowToGetQueryStatistics.cs /}
{CODE-TAB:csharp:Query_async stats_2@ClientApi\Session\Querying\HowToGetQueryStatistics.cs /}
{CODE-TAB:csharp:DocumentQuery stats_3@ClientApi\Session\Querying\HowToGetQueryStatistics.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" where FirstName == "Anne"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Session\Querying\HowToGetQueryStatistics.cs /}

| Parameter | Type              | Description                                     |
|-----------|-------------------|-------------------------------------------------|
| __stats__ | `QueryStatistics` | An 'out' param for getting the query statistics |

<br> 

{CODE syntax_2@ClientApi\Session\Querying\HowToGetQueryStatistics.cs /}

| Property             | Type             | Description                                                                                                                                                                                                              |
|----------------------|------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __IsStale__          | `bool`           | Are the results returned by the query potentially stale                                                                                                                                                                  |
| __DurationInMs__     | `long`           | Query duration on the server side in Milliseconds                                                                                                                                                                        |
| __TotalResults__     | `int`            | The total count of results that matched the query as `Int32`.<br>Matching query results can also be counted as `Int32` using [Count](../../../client-api/session/querying/how-to-count-query-results#count).             |
| __LongTotalResults__ | `long`           | The total count of the results that matched the query as `Int64`.<br>Matching query results can also be counted as `Int64` using [LongCount](../../../client-api/session/querying/how-to-count-query-results#longcount). |
| __SkippedResults__   | `int`            | The number of results skipped by the server.<br>Learn more in [paging through tampered results](../../../indexes/querying/paging#paging-through-tampered-results).                                                       |
| __Timestamp__        | `DateTime`       | The time when the query results were unstale                                                                                                                                                                             |
| __IndexName__        | `string`         | The name of the queried index                                                                                                                                                                                            |
| __IndexTimestamp__   | `IndexTimestamp` | The timestamp of the queried index                                                                                                                                                                                       |
| __LastQueryTime__    | `DateTime`       | The timestamp of the last time the index was queried                                                                                                                                                                     |
| __ResultEtag__       | `long?`          | Results Etag                                                                                                                                                                                                             |
| __NodeTag__          | `string`         | Tag of the cluster node that responded to the query                                                                                                                                                                      |

{PANEL/}

## Related articles

### Session

- [How to query](../../../client-api/session/querying/how-to-query)
- [How to customize query](../../../client-api/session/querying/how-to-customize-query)
- [Count query results](../../../client-api/session/querying/how-to-count-query-results)
