# Session: Querying: How to get query statistics?

Query statistics can provide important information about query e.g. duration, total number of results, staleness information, etc. To access statistics use `statistics` method.

## Syntax

{CODE:java stats_1@ClientApi\Session\Querying\HowToGetQueryStatistics.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **stats** | [RavenQueryStatistics](../../../glossary/raven-query-statistics) | Statistics for query. |

| Return Value | |
| ------------- | ----- |
| [RavenQueryStatistics](../../../glossary/raven-query-statistics) | Statistics for query. |

## Example

{CODE:java stats_2@ClientApi\Session\Querying\HowToGetQueryStatistics.java /}

## Related articles

- [How to **show** detailed **timings** in query statistics?](../../../client-api/session/querying/how-to-customize-query#showtimings)
