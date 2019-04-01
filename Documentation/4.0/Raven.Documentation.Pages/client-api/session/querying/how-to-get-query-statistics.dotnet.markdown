# Session: Querying: How to Get Query Statistics

Query statistics can provide important information about a query like duration, total number of results, staleness information, etc. To access statistics use the `Statistics` method.

## Syntax

{CODE stats_1@ClientApi\Session\Querying\HowToGetQueryStatistics.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **stats** | `QueryStatistics` | Statistics for query. |

## Example

{CODE stats_2@ClientApi\Session\Querying\HowToGetQueryStatistics.cs /}

## Related articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [How to **Customize** Query?](../../../client-api/session/querying/how-to-customize-query)
