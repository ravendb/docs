# Query View
---

{NOTE: }

* Use the **Query View** to run RQL queries, view and store their results.  
* Read more about RQL (Raven Query Language) [here](../../../indexes/querying/what-is-rql).  

* In this page:  
  * [Query View](../../../studio/database/queries/query-view#query-view)  
  * [Query Results](../../../studio/database/queries/query-view#query-results)  
      * [Result Columns](../../../studio/database/queries/query-view#result-columns)  

{NOTE/}

---

{PANEL: Query View}

![Figure 1. Query View](images/query-view-query-1.png "Figure 1. Query View")

1. **Query View**  
   Click to open the query view.  
2. **Database**  
   Click to choose the database you want to query.  
3. **Syntax Samples**  
   Click to display [RQL](../../../indexes/querying/basics) syntax samples.  
4. **Save Query**  
   Click to save the query currently displayed in the query box.  
   ![Save Query](images/query-view-save-query.png "Save Query")  
   Enter a name, and click the **Save** button again to save the query.  
5. **Load Query**  
   Click to load a stored query.  
   ![Load Query](images/query-view-load-query.png "Load Query")  
   Hover over a query name to display its preview.  
   Click a query name or the preview **Load** button to load the query.  
6. **Query Settings**  
   Click to set query settings.  
   ![Query Settings](images/query-view-settings.png "Query Settings")  
    a. Toggle to enable or disable query caching.  
    b. Toggle to show the full results or just stored index fields.  
    c. Toggle to display **raw index entries** or **matching documents** in the query results.  
7. **Query Box**  
   Enter your RQL query in the query box.  
8. **Run Query**  
   Click to run your query.  
9. **Query Results**  
   The results box displays selected columns of results retrieved by your query (see [below](../../../studio/database/queries/query-view#query-results)).  

{PANEL/}

---

{PANEL: Query Results}

![Figure 2. Query Results](images/query-view-query-2.png "Figure 2. Query Results")

1. **Index Used**  
   The index used by this query.  
2. **Results Retrieval Time**  
   The time it took to retrieve the results.  
   {NOTE: }
   [Include Query Timings](../../../client-api/session/querying/debugging/query-timings) 
   to display additional timing details.  
   {NOTE/}
3. **Delete Documents**  
   Click to delete the documents found by the query.  
4. **Statistics**  
   Click to view query atatistics, including -  
    * Number of results  
    * Results status  
    * Query duration  
    * The index used by this query  
    * ETag  
5. **Export Results as CSV File**  
    * Click to store **all data** to a CSV file.  
    * Click the drop down menu and choose "Export visible columns only" 
      to store in a CSV file only the result columns that are currently displayed.  
      ![Store Selected Columns In CSV](images/query-view-export-selected-columns-to-csv.png "Store Selected Columns In CSV")
6. **Display**  
   Click to choose which columns would be displayed in the results box.  
7. **Toggle Expanded/Collapsed View**  
   Click to expand a larger results view or collapse it to its original size.  

---

### Result Columns

![Figure 3. Result Columns](images/query-view-query-3.png "Figure 3. Result Columns")

1. **Preview**  
   Click for a preview of the retrieved item.  
2. **Document ID**  
   Click the ID to open this document in the [Document View](../../../studio/database/documents/document-view),  
   -or-  
   Hover over the ID to display it in text format and optionally copy it to the clipboard.  
    ![Hover ID](images/query-view-ID-column.png "Hover ID")  
3. **Multiple Records Result**  
   Hover over the brackets to display the retrieved records and optionally copy them to the clipboard.  
    ![Multiple Records Result](images/query-view-multiple-records-result.png "Multiple Records Result")  
4. **Text String Result**  
   Hover over the text to display and optionally copy it to the clipboard.  
    ![Text String Result](images/query-view-text-string-result.png "Text String Result")  

{NOTE: }

Some RavenDB features enhance the results view.  

* [Time Series](../../../document-extensions/timeseries/overview) query results include additional columns and 
  a graphical view. Learn more about it [here](../../../studio/database/document-extensions/time-series#querying-time-series).  
* [Spatial Queries](../../../studio/database/queries/spatial-queries-map-view) results include a graphical tab 
  that [displays geographical locations](../../../studio/database/queries/spatial-queries-map-view#spatial-map-view) 
  on the world map.  
* [Graph Queries](../../../indexes/querying/graph/graph-queries-overview) also present query results 
  [graphically](../../../indexes/querying/graph/graph-queries-overview#creating-graph-queries) in an additional tab.  

{NOTE/}

{PANEL/}

## Related Articles

### Queries
- [RQL - Raven Query Language](../../../indexes/querying/what-is-rql)  
- [Basics](../../../indexes/querying/basics)  

### Indexes
- [Indexing Basics](../../../indexes/indexing-basics)  

### Client API
- [How to Query](../../../client-api/session/querying/how-to-query)  
