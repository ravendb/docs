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
{CODE-TAB:php:Query stats_3@ClientApi\Session\Querying\HowToGetQueryStatistics.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" where FirstName == "Robert"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:php stats_1@ClientApi\Session\Querying\HowToGetQueryStatistics.php /}

| Parameter | Type              | Description                                     |
|-----------|-------------------|-------------------------------------------------|
| **$stats** | `QueryStatistics` | An 'out' param for getting the query statistics |

<br> 

{CODE:php stats_2@ClientApi\Session\Querying\HowToGetQueryStatistics.php /}

| Property             | Type             | Description                                              |
|----------------------|------------------|----------------------------------------------------------|
| **$isStale** | `bool` | Are the results returned by the query potentially stale |
| **$durationInMs** | `int` | Query duration on the server side in Milliseconds |
| **$totalResults**,<br>**$longTotalResults** | `int` | The total count of results that matched the query.<br>Matching query results can also be counted using [Count](../../../client-api/session/querying/how-to-count-query-results#count). |
| **$skippedResults** | `int` | The number of results skipped by the server.<br>Learn more in [paging through tampered results](../../../indexes/querying/paging#paging-through-tampered-results). |
| **$timestamp** | `?DateTimeInterface` | The time when the query results were unstale |
| **$indexName** | `?string` | The name of the queried index |
| **$indexTimestamp** | `?DateTimeInterface` | The timestamp of the queried index |
| **$lastQueryTime** | `?DateTimeInterface` | The timestamp of the last time the index was queried |
| **$resultEtag** | `?int` | Results Etag |
| **$nodeTag** | `?string` | Tag of the cluster node that responded to the query |

{PANEL/}

## Related articles

### Session

- [How to query](../../../client-api/session/querying/how-to-query)
- [How to customize query](../../../client-api/session/querying/how-to-customize-query)
- [Count query results](../../../client-api/session/querying/how-to-count-query-results)
