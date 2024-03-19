# Get Query Statistics

---

{NOTE: }

* Detailed __query statistics__ can be retrieved for every executed query using the `statistics` method.

* Stats such as query duration, number of results, index name used in the query, and more,  
  are returned in the `QueryStatistics` object.

* In This Page:
    * [Get query statistics](../../../client-api/session/querying/how-to-get-query-statistics#get-query-statistics)
    * [Syntax](../../../client-api/session/querying/how-to-get-query-statistics#syntax)

{NOTE/}

---

{PANEL: Get query statistics}

{CODE-TABS}
{CODE-TAB:nodejs:Query stats@client-api\session\querying\howToGetQueryStatistics.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" where FirstName == "Anne"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@client-api\session\querying\howToGetQueryStatistics.js /}

| Parameter         | Type              | Description                                                                                                                                                                       |
|-------------------|-------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __statsCallback__ | `(stats) => void` | <ul><li>A callback function with an output parameter.</li><li>The parameter passed to the callback will be filled with the `QueryStatistics` object when query returns.</li></ul> |

| `QueryStatistics`    |           |                                                                                                                                                                    |
|----------------------|-----------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __isStale__          | `boolean` | Are the results returned by the query potentially stale                                                                                                            |
| __durationInMs__     | `number`  | Query duration on the server side in Milliseconds                                                                                                                  |
| __totalResults__     | `number`  | The total count of results that matched the query                                                                                                                  |
| __longTotalResults__ | `number`  | The total count of results that matched the query (same as `totalResults`)                                                                                         |
| __skippedResults__   | `number`  | The number of results skipped by the server.<br>Learn more in [paging through tampered results](../../../indexes/querying/paging#paging-through-tampered-results). |
| __timestamp__        | `Date`    | The time when the query results were unstale                                                                                                                       |
| __indexName__        | `string`  | The name of the queried index                                                                                                                                      |
| __indexTimestamp__   | `Date`    | The timestamp of the queried index                                                                                                                                 |
| __lastQueryTime__    | `Date`    | The timestamp of the last time the index was queried                                                                                                               |
| __resultEtag__       | `number`  | Results Etag                                                                                                                                                       |
| __nodeTag__          | `string`  | Tag of the cluster node that responded to the query                                                                                                                |

{PANEL/}

## Related articles

### Session

- [How to query](../../../client-api/session/querying/how-to-query)
- [How to customize query?](../../../client-api/session/querying/how-to-customize-query)
- [Count query results](../../../client-api/session/querying/how-to-count-query-results)
