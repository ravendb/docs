﻿# Custom Sorters
---

{NOTE: }

* The Lucene indexing engine allows you to create your own __Custom Sorters__  
  where you can define how query results will be ordered based on your specific requirements.

* Custom sorters can be defined either at: 
  * the __database level__, where they can only be used with queries on that database  
  * or at the __server-wide level__, where they can be used with queries made on all databases in your cluster.  

* If a database custom sorter and a server-wide custom sorter share the same name,  
  the database custom sorter will be the one used for the query.  

* This view lists all custom sorters that were uploaded via the Studio 
  and from the [client API](../../../client-api/operations/maintenance/sorters/put-sorter).

* Once deployed, you can order your query results using the custom sorter.  
  A query example is available [here](../../../client-api/session/querying/sort-query-results#custom-sorters).

---

* In this page:
  * [Database custom sorter view](../../../studio/database/settings/custom-sorters#database-custom-sorter-view)  
  * [Server-wide custom sorter view](../../../studio/database/settings/custom-sorters#server-wide-custom-sorter-view)  
  * [Add a custom sorter](../../../studio/database/settings/custom-sorters#add-a-custom-sorter)  
  * [Test the custom sorter](../../../studio/database/settings/custom-sorters#test-the-custom-sorter)  

{NOTE/}

---

{PANEL: Database custom sorter view}

![Figure 1. Database custom sorter view](images/custom-sorter-1.png "Figure 1. Database custom sorter view")

{NOTE: }

1. Navigate to __Settings > Custom Sorters__.

2. Click to add a new database custom sorter.  
   See the sorter edit view [below](../../../studio/database/settings/custom-sorters#add-a-custom-sorter).  

3. The custom sorters deployed for this __database__ are listed here.

4. Click to test this custom sorter.  
   See the test view [below](../../../studio/database/settings/custom-sorters#test-the-custom-sorter).

5. Click to edit this custom sorter.

6. Delete the custom sorter.

7. The custom sorters deployed __server-wide__ are listed here.
 
8. Follow this link to manage the server-wide custom sorters.
 
{NOTE/}

{PANEL/}

{PANEL: Server-wide custom sorter view}

![Figure 2. Server-wide custom sorter view](images/custom-sorter-2.png "Figure 2. Server-wide custom sorter view")

{NOTE: }

1. Navigate to __Manage Server > Server-Wide Sorters__.

2. Click to add a new server-wide custom sorter.  
   See the sorter edit view [below](../../../studio/database/settings/custom-sorters#add-a-custom-sorter).

3. The custom sorters deployed __server-wide__ are listed here.

4. Click to edit this server-wide custom sorter.

5. Delete this server-wide custom sorter.

{NOTE/}

{NOTE: }

Note:  
To test a server-wide sorter go to the Custom Sorters View in any database.

{NOTE/}

{PANEL/}

{PANEL: Add a custom sorter}

![Figure 3. Add a custom sorter](images/custom-sorter-3.png "Figure 3. Add a custom sorter")

{NOTE: }

1. Either load the sorter's code from a `*.cs` file or enter the code manually in this editor.

2. The __sorter name__ must be the same as the class name of your sorter.

3. Replace the sample code in the image with your own implementation.  
   The __sorter code__ must be compilable and include all necessary `using` statements.  
   The sorter's class should inherit from [Lucene.Net.Search.FieldComparator](https://lucenenet.apache.org/docs/3.0.3/df/d91/class_lucene_1_1_net_1_1_search_1_1_field_comparator.html).  

4. Save your sorter.

{NOTE/}

{PANEL/}

{PANEL: Test the custom sorter}

![Figure 4. Test the custom sorter](images/custom-sorter-4.png "Figure 3. Test the custom sorter")

{NOTE: }

1. Click to open the test view.

2. Enter an RQL query to test.  
   To order by your sorter use `order by custom(<field name>, <your sorter name>)`

3. Click Run test.

{NOTE/}

{PANEL/}

## Related Articles

### Client API
- [Sort query results](../../../client-api/session/querying/sort-query-results)

### Operations
- [Put custom sorter](../../../client-api/operations/maintenance/sorters/put-sorter)
- [Put custom sorter (server-wide)](../../../client-api/operations/server-wide/sorters/put-sorter-server-wide)
