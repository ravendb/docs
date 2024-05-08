# Get Statistics

---

{NOTE: }

* Statistics about your database and collections can be retrieved.

* By default, you get stats for the database defined in your Document Store.   
  To get database and collection stats for another database use [forDatabase](../../../client-api/operations/maintenance/get-stats#get-stats-for-another-database).

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
Use `GetCollectionStatisticsOperation` to get __collection stats__.
{CODE:nodejs stats_1@client-api\Operations\Maintenance\getStats.js /}
{NOTE/}
{NOTE: }
Collection stats results:
{CODE:nodejs stats_1_results@client-api\Operations\Maintenance\getStats.js /}
{NOTE/}
{PANEL/}

{PANEL: Get detailed collection stats}
{NOTE: }
Use `GetDetailedCollectionStatisticsOperation` to get __detailed collection stats__.
{CODE:nodejs stats_2@client-api\Operations\Maintenance\getStats.js /}
{NOTE/}
{NOTE: }
Detailed collection stats results:
{CODE:nodejs stats_2_results@client-api\Operations\Maintenance\getStats.js /}
{NOTE/}
{PANEL/}

{PANEL: Get database stats}
{NOTE: }
Use `GetStatisticsOperation` to get __database stats__.
{CODE:nodejs stats_3@client-api\Operations\Maintenance\getStats.js /}
{NOTE/}
{NOTE: }
Database stats results:
{CODE:nodejs stats_3_results@client-api\Operations\Maintenance\getStats.js /}
{NOTE/}
{PANEL/}

{PANEL: Get detailed database stats}
{NOTE: }
Use `GetDetailedStatisticsOperation` to get __detailed database stats__.
{CODE:nodejs stats_4@client-api\Operations\Maintenance\getStats.js /}
{NOTE/}
{NOTE: }
Detailed database stats results:
{CODE:nodejs stats_4_results@client-api\Operations\Maintenance\getStats.js /}
{NOTE/}
{PANEL/}

{PANEL: Get stats for another database}
{NOTE: }

* By default, you get stats for the database defined in your Document Store.
* Use `forDatabase` to get database & collection stats for another database.
* 'forDatabase' can be used with __any__ of the above stats options.
 
{CODE:nodejs stats_5@client-api\Operations\Maintenance\getStats.js /}

* Learn more about switching operations to another database [here](../../../client-api/operations/how-to/switch-operations-to-a-different-database).

{NOTE/}
{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)
- [Switch operation to another database](../../../client-api/operations/how-to/switch-operations-to-a-different-database)

### FAQ

- [What is a Collection](../../../client-api/faq/what-is-a-collection)

### Client API

- [Get Query Statistics](../../../client-api/session/querying/how-to-get-query-statistics)  
