# Operations: How to Get Detailed Database Statistics

**GetDetailedStatisticsOperation** will retrieve the same database statistics as [GetStatisticsOperation](../../../client-api/operations/maintenance/get-statistics) plus total count of identities and counters. 

## Syntax

{CODE stats_1@ClientApi\Operations\DetailedStatistics.cs /}

| Return Value | |
| ------------- | ----- |
| `DetailedDatabaseStatistics` | Detailed database statistics |

## Example

{CODE stats_2@ClientApi\Operations\DetailedStatistics.cs /}

## Related Articles 

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
- [How to Get Collection Statistics](../../../client-api/operations/maintenance/get-collection-statistics)
- [How to Get Statistics](../../../client-api/operations/maintenance/get-statistics)

### FAQ

- [Database Statistics](../../../server/administration/statistics#database-statistics)
