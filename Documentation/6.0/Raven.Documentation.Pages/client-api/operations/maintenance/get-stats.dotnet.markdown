# Get Statistics

---

{NOTE: }

* Statistics can be retrieved for the database and for collections.  

* By default, statistics are retrieved for the database defined in the Document Store.   
  To get database and collection statistics for another database use [ForDatabase](../../../client-api/operations/maintenance/get-stats#get-stats-for-another-database).  

* In this page:
    * [Get collection statistics](../../../client-api/operations/maintenance/get-stats#get-collection-statistics)
    * [Get detailed collection statistics](../../../client-api/operations/maintenance/get-stats#get-detailed-collection-statistics)
    * [Get database statistics](../../../client-api/operations/maintenance/get-stats#get-database-statistics)
    * [Get detailed database statistics](../../../client-api/operations/maintenance/get-stats#get-detailed-database-statistics)
    * [Get statistics for another database](../../../client-api/operations/maintenance/get-stats#get-statistics-for-another-database)
{NOTE/}

---

{PANEL: Get collection statistics}

To get **collection statistics**, use `GetCollectionStatisticsOperation`:  
{CODE stats_1@ClientApi\Operations\Maintenance\GetStats.cs /}

---

Statistics are returned in the `CollectionStatistics` object.
{CODE stats_1_results@ClientApi\Operations\Maintenance\GetStats.cs /}

{PANEL/}

{PANEL: Get detailed collection statistics}

To get **detailed collection statistics**, use `GetDetailedCollectionStatisticsOperation`:  
{CODE stats_2@ClientApi\Operations\Maintenance\GetStats.cs /}

---

Statistics are returned in the `DetailedCollectionStatistics` object.
{CODE stats_2_results@ClientApi\Operations\Maintenance\GetStats.cs /}

{PANEL/}

{PANEL: Get database statistics}

To get **database statistics**, use `GetStatisticsOperation`:  
{CODE stats_3@ClientApi\Operations\Maintenance\GetStats.cs /}

---

Statistics are returned in the `DatabaseStatistics` object.
{CODE stats_3_results@ClientApi\Operations\Maintenance\GetStats.cs /}

{PANEL/}

{PANEL: Get detailed database statistics}

To get **detailed database statistics**, use `GetDetailedStatisticsOperation`:  
{CODE stats_4@ClientApi\Operations\Maintenance\GetStats.cs /}

---

Statistics are returned in the `DetailedDatabaseStatistics` object.
{CODE stats_4_results@ClientApi\Operations\Maintenance\GetStats.cs /}

{PANEL/}

{PANEL: Get statistics for another database}

* By default, you get statistics for the database defined in your Document Store.  
* Use `ForDatabase` to get database and collection statistics for another database.  
* `ForDatabase` can be used with **any** of the above statistics options.

{CODE stats_5@ClientApi\Operations\Maintenance\GetStats.cs /}

* Learn more about switching operations to another database [here](../../../client-api/operations/how-to/switch-operations-to-a-different-database).

{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
- [Switch operation to another database](../../../client-api/operations/how-to/switch-operations-to-a-different-database)

### FAQ

- [What is a Collection](../../../client-api/faq/what-is-a-collection)

### Client API

- [Get Query Statistics](../../../client-api/session/querying/how-to-get-query-statistics)  
