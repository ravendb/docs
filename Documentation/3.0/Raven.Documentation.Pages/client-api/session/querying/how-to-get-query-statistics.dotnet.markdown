# Session: Querying: How to get query statistics?

Query statistics can provide important information about query e.g. duration, total number of results, staleness information, etc. To access statistics use `Statistics` method.

## Syntax

{CODE stats_1@ClientApi\Session\Querying\HowToGetQueryStatistics.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **stats** | [RavenQueryStatistics](../../../glossary/raven-query-statistics) | Statistics for query. |

| Return Value | |
| ------------- | ----- |
| [RavenQueryStatistics](../../../glossary/raven-query-statistics) | Statistics for query. |

## Example

{CODE stats_2@ClientApi\Session\Querying\HowToGetQueryStatistics.cs /}

## Related articles

- [How to **show** detailed **timings** in query statistics?](../../../client-api/session/querying/how-to-customize-query#showtimings)
