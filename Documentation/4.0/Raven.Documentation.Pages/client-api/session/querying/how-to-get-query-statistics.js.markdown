# Session : Querying : How to Get Query Statistics

Query statistics can provide important information about a query like duration, total number of results, staleness information, etc. To access statistics use the `statistics()` method.

## Syntax

{CODE:nodejs stats_1@client-api\session\querying\howToGetQueryStatistics.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **stats** | `QueryStatistics` | Statistics for query. |

## Example

{CODE:nodejs stats_2@client-api\session\querying\howToGetQueryStatistics.js /}

## Related articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [How to **Customize** Query?](../../../client-api/session/querying/how-to-customize-query)
