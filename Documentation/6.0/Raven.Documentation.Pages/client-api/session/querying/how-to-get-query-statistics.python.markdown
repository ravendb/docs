# Get Query Statistics

---

{NOTE: }

* Detailed **query statistics** can be retrieved for every executed query using the `statistics` method.  
  
* Stats such as query duration, number of results, index name used in the query, and more,  
  are returned in the `QueryStatistics` object.

* In This Page:  
   * [Get query statistics](../../../client-api/session/querying/how-to-get-query-statistics#get-query-statistics)  
   * [Syntax](../../../client-api/session/querying/how-to-get-query-statistics#syntax)  

{NOTE/}

---

{PANEL: Get query statistics}

{CODE-TABS}
{CODE-TAB:python:Query stats_2@ClientApi\Session\Querying\HowToGetQueryStatistics.py /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" where FirstName == "Anne"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:python stats_1@ClientApi\Session\Querying\HowToGetQueryStatistics.py /}

| Parameter          | Type                                | Description                                                  |
|--------------------|-------------------------------------|--------------------------------------------------------------|
| **stats_callback** | `Callable[[QueryStatistics], None]` | An _action_ that will be called with query statistics object |

| Property            | Type             | Description                                                                                                                                                                           |
|---------------------|------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **is_stale**        | `bool`           | Are the results returned by the query potentially stale                                                                                                                               |
| **duration_in_ms**  | `int`            | Query duration on the server side in Milliseconds                                                                                                                                     |
| **total_results**   | `int`            | The total count of results that matched the query<br>Matching query results can also be counted using [count](../../../client-api/session/querying/how-to-count-query-results#count). |
| **skipped_results** | `int`            | The number of results skipped by the server.<br>Learn more in [paging through tampered results](../../../indexes/querying/paging#paging-through-tampered-results).                    |
| **timestamp**       | `datetime`       | The time when the query results were unstale                                                                                                                                          |
| **index_name**      | `str`            | The name of the queried index                                                                                                                                                         |
| **index_timestamp** | `IndexTimestamp` | The timestamp of the queried index                                                                                                                                                    |
| **last_query_time** | `datetime`       | The timestamp of the last time the index was queried                                                                                                                                  |
| **result_etag**     | `int`            | Results Etag                                                                                                                                                                          |
| **node_tag**        | `str`            | Tag of the cluster node that responded to the query                                                                                                                                   |

{PANEL/}

## Related articles

### Session

- [How to query](../../../client-api/session/querying/how-to-query)
- [How to customize query](../../../client-api/session/querying/how-to-customize-query)
- [Count query results](../../../client-api/session/querying/how-to-count-query-results)
