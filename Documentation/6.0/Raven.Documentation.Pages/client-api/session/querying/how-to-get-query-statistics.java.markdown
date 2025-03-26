# Get Query Statistics

Query statistics can provide important information about a query like duration, total number of results, staleness information, etc. 
To access statistics use the `statistics` method.

## Example

{CODE:java stats_3@ClientApi\Session\Querying\HowToGetQueryStatistics.java /}

## Syntax

{CODE:java stats_1@ClientApi\Session\Querying\HowToGetQueryStatistics.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **stats** | `QueryStatistics` | Statistics for query. |

{CODE:java stats_2@ClientApi\Session\Querying\HowToGetQueryStatistics.java /}


| Property           | Type      | Description                                                                                                                                                        |
|--------------------|-----------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **IsStale**        | `boolean` | Are the results returned by the query potentially stale                                                                                                            |
| **DurationInMs**   | `long`    | Query duration on the server side in Milliseconds                                                                                                                  |
| **TotalResults**   | `long`    | The total count of results that matched the query as `int`                                                                                                         |
| **SkippedResults** | `long`    | The number of results skipped by the server.<br>Learn more in [paging through tampered results](../../../indexes/querying/paging#paging-through-tampered-results). |
| **ScannedResults** | `long`    | The number of results scanned by the query.<br>Relevant only when using a filter clause in the query.                                                              |
| **Timestamp**      | `Date`    | The time when the query results were unstale                                                                                                                       |
| **IndexName**      | `string`  | The name of the queried index                                                                                                                                      |
| **indexTimestamp** | `Date`    | The timestamp of the queried index                                                                                                                                 |
| **LastQueryTime**  | `Date`    | The timestamp of the last time the index was queried                                                                                                               |
| **ResultEtag**     | `Long`    | Results Etag                                                                                                                                                       |
| **NodeTag**        | `String`  | Tag of the cluster node that responded to the query                                                                                                                |

## Related articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [How to **Customize** Query?](../../../client-api/session/querying/how-to-customize-query)
