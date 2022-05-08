﻿# Studio: Query View
---

{NOTE: }

* Use the **Query View** to run [RQL](../../../indexes/querying/what-is-rql) queries and view their results.  
* Queries can be executed either on a **Collection** or on an existing **Static-Index**.  
* RavenDB always uses an index to fetch the query results.  
  When a query is executed over a Collection (without any filtering condition),  
  RavenDB uses an internal index that is kept in its internal storage.  
* A query on a collection with some filtering condition will create an **Auto-Index** (Dynamic Query).  
  See [Index Types](../../../studio/database/indexes/indexes-overview#index-types) for details.  
* Query results can be saved/exported to a CSV file.  

* In this page:  
  * [Query View](../../../studio/database/queries/query-view#query-view)  
  * [Query Results](../../../studio/database/queries/query-view#query-results)  
      * [Select Visible Columns](../../../studio/database/queries/query-view#select-visible-columns)  
      * [Result Columns](../../../studio/database/queries/query-view#result-columns)  
  * [Additional Query Options](../../../studio/database/queries/query-view#additional-query-options)  

{NOTE/}

---

{PANEL: Query View}

![Query View](images/query-view.png "Query View")

1. **Query View**  
   Click to open the query view.  
2. **Database**  
   Click to select the database you want to query.  
3. **Syntax Samples**  
   Click to display [RQL](../../../indexes/querying/basics) syntax samples.  
4. **Query Box**  
   Enter your RQL query in the query box.  
5. **Save Query**  
   Click to save the currently displayed query in the local browser storage.  
   ![Save Query](images/query-view-save-query.png "Save Query")  
   Enter a name, and click the **Save** button again to save the query.  
6. **Load Query**  
   Click to load a stored query.  
   Recent (unsaved) queries will also be listed here.  
   ![Load Query](images/query-view-load-query.png "Load Query")  
   Hover over a query name to display its preview.  
   Click the query name or the preview **Load** button to load the query.  
7. **Query Settings**  
   Click to set query settings.  
   ![Query Settings](images/query-view-settings.png "Query Settings")  
     * a. **Cache enabled**  
          Toggle to enable or disable caching of query results.  
     * b. **Disable creating new Auto-Indexes**  
          Toggle to disable Auto-Index creation for this query.
          If no index exists to satisfy this query, an exception will be thrown.
          * Toggling this ON will not affect Auto-Index creation in future Studio queries.
            To disable all future Auto-Index creation from Studio queries, change the default setting in [Studio Configuration](../../../studio/database/settings/studio-configuration#disabling-auto-index-creation-on-studio-queries-or-patches)
     * c. **Show stored index fields only**  
          Toggle to show the query results or only the stored index fields.  
          (Relevant only when querying an index)  
     * d. **Show raw index entries instead of index results**  
          Toggle to display **raw index entries** or **matching documents** in the query results.  
          (Relevant only when querying an index)  
8. **Run Query**  
   Click to run the query.  
9. **Query Results**  
   The results area displays selected columns of results retrieved by your query (see [below](../../../studio/database/queries/query-view#query-results)).  

{PANEL/}

---

{PANEL: Query Results}

![Query Results](images/query-view-query-results.png "Query Results")

1. **Index or Collection Used**  
   The index or the collection that was used by the query.  
2. **Results Retrieval Time**  
   The time it took to retrieve the results.  
   {NOTE: }
   Add `include timings()` to your RQL to display additional timing details in the results.  
   Learn more in [Include Query Timings](../../../client-api/session/querying/debugging/query-timings).  
   {NOTE/}
3. **Delete Documents**  
   Click to delete all documents that match the query.  
4. **Statistics**  
   Click to view query statistics, including -  
    * Number of results  
    * Results status  
    * Query duration  
    * The index used by this query.  
      Either the index that was explicitly used by the query, or the internal RavenDB index that is 
      used [for collection queries](../../../client-api/faq/what-is-a-collection#collection-usages).  
    * An ETag representing the current state of the index used by the query.  
      (The ETag value changes with every change made to the documents)  
5. **Export Results as CSV File**  
    * Click to store **all query results columns** in a CSV file.  
      ![Export to CSV File](images/query-view-export-to-csv-file.png "Export to CSV File")
    * Click the drop-down and select "Export visible columns only" 
      to store **only the columns that are currently displayed**.  
      ![Export Visible Columns Only](images/query-view-export-visible-columns-only.png "Export Visible Columns Only")
6. **Display**  
   Click to open the Display drop-down dialog, where you can select which columns 
   to display and add custom columns. (See **Select Visible Columns** below).
7. **Toggle Expanded/Collapsed View**  
   Click to expand or collapse the results view.  

---

### Select Visible Columns

To modify the displayed result columns, open the Display drop-down dialog from the [Query Results](../../../studio/database/queries/query-view#query-results) view.  
Custom result columns can be added, edited and removed.  

![Display Dialog](images/query-view-display-dialog.png "Display Dialog")

1. **A query results column**
2. **A custom results column**

---

![Display Dialog - Actions](images/query-view-display-dialog-actions.png "Display Dialog - Actions")

{INFO: }
Changes made in the Display dialog will take effect only when the **Apply** button is clicked.  
{INFO/}

1. **Display/Hide all columns**  
   Check or uncheck to display or hide all columns.  
2. **Display/Hide column**  
   Check or uncheck to display or hide this column.  
3. **Relocate Column**  
   Click and drag to relocate the column.  
4. **Edit Custom Column**  
   Click to edit the custom column's **field** and **alias** (see **Add Custom Column** below).  
5. **Remove Custom Column**  
   Click to remove the custom column.  
6. **Add Custom Column**  
   ![Add Custom Column](images/query-view-add-custom-column.png "Add Custom Column")  
    * a. Identify a results field that will be displayed in the custom column.  
      E.g. `this.Address.Country`  
    * b. Give the custom column an alias that will be displayed as the column's title.  
      E.g. `Country`  
    * c. Click to add the new column and close the custom column dialog.  
    * d. Click to close the custom column dialog without adding the new column.  
7. **Reset to default**  
   Reset result columns to their original state (determined by the query).  
8. **Close**  
   Close the Display dialog without applying your changes.  
9. **Apply**  
   Apply your changes and close the Display dialog.  

---

### Result Columns

![Result Columns](images/query-view-result-columns.png "Result Columns")

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
4. **Text Result**  
   Hover over the text to display and optionally copy it to the clipboard.  
    ![Text String Result](images/query-view-text-string-result.png "Text String Result")  

{PANEL/}

{PANEL: Additional Query Options}

Some RavenDB features enhance the queries options.  

* [Time Series Query](../../../document-extensions/timeseries/overview) results provide insight 
  into your time-series data, including a graph view over time.  
  Learn more about it [here](../../../studio/database/document-extensions/time-series#querying-time-series).  
* [Spatial Query](../../../studio/database/queries/spatial-queries-map-view) results include 
  a Spatial Map Tab that [displays geographical locations](../../../studio/database/queries/spatial-queries-map-view#spatial-map-view) 
  on the world map.  
* [Graph Query](../../../indexes/querying/graph/graph-queries-overview) also present query results 
  [graphically](../../../indexes/querying/graph/graph-queries-overview#creating-graph-queries) in an additional tab.  
* [Counter Query](../../../document-extensions/counters/counters-and-other-features#counters-and-queries) can be used to get counters data.  

{PANEL/}

## Related Articles

### Queries
- [RQL - Raven Query Language](../../../indexes/querying/what-is-rql)  
- [Basics](../../../indexes/querying/basics)  

### Indexes
- [Indexing Basics](../../../indexes/indexing-basics)  

### Client API
- [How to Query](../../../client-api/session/querying/how-to-query)  
