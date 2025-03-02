# Get Statistics

---

{NOTE: }

* Statistics can be retrieved for the database and for collections.

* By default, statistics are retrieved for the database defined in the Document Store.   
  To get database and collection statistics for another database use [ForDatabase](../../../client-api/operations/maintenance/get-stats#get-stats-for-another-database).

* In this page:
    * [Get collection stats](../../../client-api/operations/maintenance/get-stats#get-collection-stats)
    * [Get detailed collection stats](../../../client-api/operations/maintenance/get-stats#get-detailed-collection-stats)
    * [Get database stats](../../../client-api/operations/maintenance/get-stats#get-database-stats)
    * [Get detailed database stats](../../../client-api/operations/maintenance/get-stats#get-detailed-database-stats)
    * [Get stats for another database](../../../client-api/operations/maintenance/get-stats#get-stats-for-another-database)
{NOTE/}

---

{PANEL: Get collection stats}
{NOTE: }
Use `GetCollectionStatisticsOperation` to get **collection stats**.
{CODE:java stats_1@ClientApi\Operations\Maintenance\GetStats.java /}
{NOTE/}
{NOTE: }
Stats are returned in the `CollectionStatistics` object.
{CODE:java stats_1_results@ClientApi\Operations\Maintenance\GetStats.java /}
{NOTE/}
{PANEL/}

{PANEL: Get detailed collection stats}
{NOTE: }
Use `GetDetailedCollectionStatisticsOperation` to get **detailed collection stats**.
{CODE:java stats_2@ClientApi\Operations\Maintenance\GetStats.java /}
{NOTE/}
{NOTE: }
Stats are returned in the `DetailedCollectionStatistics` object.
{CODE:java stats_2_results@ClientApi\Operations\Maintenance\GetStats.java /}
{NOTE/}
{PANEL/}

{PANEL: Get database stats}
{NOTE: }
Use `GetStatisticsOperation` to get **database stats**.
{CODE:java stats_3@ClientApi\Operations\Maintenance\GetStats.java /}
{NOTE/}
{NOTE: }
Stats are returned in the `DatabaseStatistics` object.
{CODE:java stats_3_results@ClientApi\Operations\Maintenance\GetStats.java /}
{NOTE/}
{PANEL/}

{PANEL: Get detailed database stats}
{NOTE: }
Use `GetDetailedStatisticsOperation` to get **detailed database stats**.
{CODE:java stats_4@ClientApi\Operations\Maintenance\GetStats.java /}
{NOTE/}
{NOTE: }
Stats are returned in the `DetailedDatabaseStatistics` object.
{CODE:java stats_4_results@ClientApi\Operations\Maintenance\GetStats.java /}
{NOTE/}
{PANEL/}

{PANEL: Get stats for another database}
{NOTE: }

* By default, you get stats for the database defined in your Document Store.
* Use `forDatabase` to get database & collection stats for another database.
* 'ForDatabase' can be used with **any** of the above stats options.
 
{CODE:java stats_5@ClientApi\Operations\Maintenance\GetStats.java /}

* Learn more about switching operations to another database [here](../../../client-api/operations/how-to/switch-operations-to-a-different-database).

{NOTE/}
{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
- [Switch operation to another database](../../../client-api/operations/how-to/switch-operations-to-a-different-database)

### FAQ

- [What is a Collection](../../../client-api/faq/what-is-a-collection)
