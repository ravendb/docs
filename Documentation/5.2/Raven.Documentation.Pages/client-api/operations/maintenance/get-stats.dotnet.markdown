# Get Statistics

---

{NOTE: }

* Statistics about your database and collections can be retrieved.  

* In this page:
    * [Get collection stats](../../../client-api/operations/maintenance/get-stats#get-collection-stats)
    * [Get detailed collection stats](../../../client-api/operations/maintenance/get-stats#get-detailed-collection-stats)
    * [Get database stats](../../../client-api/operations/maintenance/get-stats#get-database-stats)
    * [Get detailed database stats](../../../client-api/operations/maintenance/get-stats#get-detailed-database-stats)
{NOTE/}

---

{PANEL: Get collection stats}
{NOTE: }
Use `GetCollectionStatisticsOperation` to get __collection stats__.
{CODE stats_1@ClientApi\Operations\Maintenance\GetStats.cs /}
{NOTE/}
{NOTE: }
Stats are returned in the `CollectionStatistics` object.
{CODE stats_1_results@ClientApi\Operations\Maintenance\GetStats.cs /}
{NOTE/}
{PANEL/}

{PANEL: Get detailed collection stats}
{NOTE: }
Use `GetDetailedCollectionStatisticsOperation` to get __detailed collection stats__.
{CODE stats_2@ClientApi\Operations\Maintenance\GetStats.cs /}
{NOTE/}
{NOTE: }
Stats are returned in the `DetailedCollectionStatistics` object.
{CODE stats_2_results@ClientApi\Operations\Maintenance\GetStats.cs /}
{NOTE/}
{PANEL/}

{PANEL: Get database stats}
{NOTE: }
Use `GetStatisticsOperation` to get __database stats__.
{CODE stats_3@ClientApi\Operations\Maintenance\GetStats.cs /}
{NOTE/}
{NOTE: }
Stats are returned in the `DatabaseStatistics` object.
{CODE stats_3_results@ClientApi\Operations\Maintenance\GetStats.cs /}
{NOTE/}
{PANEL/}

{PANEL: Get detailed database stats}
{NOTE: }
Use `GetDetailedStatisticsOperation` to get __detailed database stats__.
{CODE stats_4@ClientApi\Operations\Maintenance\GetStats.cs /}
{NOTE/}
{NOTE: }
Stats are returned in the `DetailedDatabaseStatistics` object.
{CODE stats_4_results@ClientApi\Operations\Maintenance\GetStats.cs /}
{NOTE/}
{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)

### FAQ

- [What is a Collection](../../../client-api/faq/what-is-a-collection)
