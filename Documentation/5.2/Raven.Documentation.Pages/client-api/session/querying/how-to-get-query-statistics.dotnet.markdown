# Querying: How to Get Query Statistics

---

{NOTE: }

* Detailed **Query Statistics** can be retrieved for every executed query 
  using the `Statistics` method.  
* Statistics are returned in a `QueryStatistics` instance, including query 
  duration, total results number, and various other details.  

* In This Page:  
   * [QueryStatistics](../../../client-api/session/querying/how-to-get-query-statistics#querystatistics)  

{NOTE/}

---

{PANEL: QueryStatistics}

### Syntax

{CODE stats_1@ClientApi\Session\Querying\HowToGetQueryStatistics.cs /}

| Parameters | Type | Details |
| ------------- | ------------- | ----- |
| **stats** | `QueryStatistics` | Query Statistics |


### `QueryStatistics`
{CODE QueryStatisticsDefinition@ClientApi\Session\Querying\HowToGetQueryStatistics.cs /}

| Property | Type | Details |
| ------------- | ------------- | ----- |
| **IsStale** | `bool` | Are the results returned by the query potentially stale |
| **DurationInMs** | `long` | Query duration on the server side in Milliseconds |
| **TotalResults** | `int` | The total count of results that matched the query. <br> Matching query results can also be counted using [Count](../../../client-api/session/querying/how-to-count-query-results#count). |
| **LongTotalResults** | `long` | The total count of the results that matched the query as `int64`. <br> Matching query results can also be counted as `int64` using [LongCount](../../../client-api/session/querying/how-to-count-query-results#longcount). |
| **SkippedResults** | `int` | Gets or sets the [skipped results](../../../indexes/querying/paging#paging-through-tampered-results) |
| **Timestamp** | `DateTime` | The time when the query results were unstale |
| **IndexName** | `string` | The name of the queried index |
| **DateTime** | `IndexTimestamp` | The timestamp of the queried index |
| **LastQueryTime** | `DateTime` | The timestamp of the last time the index was queried |
| **ResultEtag** | `long?` | Results Etag |
| **NodeTag** | `string` | Tag of a cluster node that responded to the query |

### Example

{CODE stats_2@ClientApi\Session\Querying\HowToGetQueryStatistics.cs /}

{PANEL/}

## Related articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [How to **Customize** Query?](../../../client-api/session/querying/how-to-customize-query)
