# Time Series
---

{NOTE: }

* Time series are sets of numeric data, associated with timestamps and ordered by time.  
* The studio interface allows you to edit, query and index time series data, as well as 
  view it as a list of entries or as a graph.  

* In this page:
  * [Document View](../../../studio/database/document-extensions/time-series#document-view)
  * [Time Series View](../../../studio/database/document-extensions/time-series#time-series-view)
     * [Create new Time Series (by Creating the First Entry)](../../../studio/database/document-extensions/time-series#create-new-time-series-by-creating-the-first-entry)
     * [Editing an Entry](../../../studio/database/document-extensions/time-series#editing-an-entry)
     * [Deleting a Range of Entries](../../../studio/database/document-extensions/time-series#deleting-a-range-of-entries)
  * [Querying Time Series](../../../studio/database/document-extensions/time-series#querying-time-series)
     * [Results in Graph View](../../../studio/database/document-extensions/time-series#results-in-graph-view)

{NOTE/}

---

{PANEL: Document View}

![Time Series - Document View](images/time-series/document-time-series.png "Time Series - Document View")

{WARNING: Actions}

1. To view a document's time series, open its [document view](../../../studio/database/documents/document-view) 
   and click the time series tab on the right.  
2. Click to create a new time series, [see more below](../../../studio/database/document-extensions/time-series#create-new-time-series-by-creating-the-first-entry).  
3. Click to view and modify time series data.  

{WARNING/}

{INFO: Info}

1. Displays the time series':  
   * Name  
   * The number of entries it contains  
   * The range of time from the first to the last entry in the time series

{INFO/}  

{PANEL/}

{PANEL: Time Series View}

![Time Series View](images/time-series/time-series-view.png "Time Series View")

{WARNING: Actions}

1. Click to add a new entry to this time series (StockPrices), or click the dropdown to create a [new time series](../../../studio/database/document-extensions/time-series#create-new-time-series-by-creating-the-first-entry).  
2. Delete all entries from a specified time range.  
3. Edit entry.

{WARNING/}

{INFO: Info}

1. Displays time series entries' data, including -  
    * Timestamp  
    * Numerical data (1-32 `double` values)  
    * Optional tag `string`  

{INFO/}  

---

### Create new Time Series (by Creating the First Entry)

* A time series is created upon the creation of its first entry (and deleted 
  once all entries have been deleted).  

* Click the **Add Time Series** button from the 
  [Document View](../../../studio/database/document-extensions/time-series#document-view) 
  to create the time series' first entry.  

![Add Time Series](images/time-series/new-time-series.png "Add Time Series")

---

![Add Time Series Entry](images/time-series/new-entry.png "Add Time Series Entry")

{WARNING: }

1. Enter time series' name.  
2. Select a [timestamp](../../../document-extensions/timeseries/overview#timestamps) for the new entry.  
3. Create an optional [tag](../../../document-extensions/timeseries/overview#tags).  
4. Add one or more numerical [values](../../../document-extensions/timeseries/overview#values) (up to 32 values).  
   ![Named Values](images/time-series/named-values.png "Named Values")
   Time series entry values can be given meaningful names rather than labels like Value #0 and Value #1.  
   To set entry values' names, use Studio's **Settings > Time Series** view.  

{WARNING/}

---

### Editing an Entry

* Click the **Edit Item** button from the 
  [Time Series View](../../../studio/database/document-extensions/time-series#time-series-view) 
  to edit a time series' entry.  

![Edit Time Series](images/time-series/edit-time-series.png "Edit Time Series")

---

![Edit Time Series Entry](images/time-series/edit-time-series-entry.png "Edit Time Series Entry")

{WARNING: }

1. Edit the optional tag.  
2. Edit a numerical value.  
3. Delete value.  
4. Add an additional value (up to 32 values).  

{WARNING/}

---

### Deleting a Range of Entries

* Click the **Delete Range** button from the 
  [Time Series View](../../../studio/database/document-extensions/time-series#time-series-view) 
  to delete a range of time series entries.  

![Delete Range Button](images/time-series/delete-range-button.png "Delete Range Button")

---

![Delete Range](images/time-series/delete-range.png "Delete Range")

{WARNING: }

To specify a range of time series entries:  

1. **Start Date**  
   Check **Use minimum** to use the first entry's timestamp as the start of the range,  
2. **End Date**  
   Check **Use maximum** to use the last entry's timestamp as the end of the range.  

---

* For either option, you can click the input bar and specify some other date in the 
  date & time dialog shown below.  

{WARNING/}

![Delete Range - Pick Date and Time](images/time-series/delete-range-2.png "Delete Range - Pick Date and Time")

{PANEL/}

{PANEL: Querying Time Series}

![Time Series Query](images/time-series/time-series-query.png "Time Series Query")

{WARNING: Actions}

1. Enter your RQL query in the query box.  
   Depicted here is a query to get all the **StockPrices** time series values from documents in the Companies collection.  
   Learn more about time series queries [here](../../../document-extensions/timeseries/querying/overview-and-syntax).  

2. Click to run the query.  

{WARNING/}

{INFO: Info}

1. Query Results - A list of time series that satisfy the query, with:  
    * The ID of the associated document.  
    * Number of entries and time range.  

{INFO/}  


---

![Time Series Query - Actions](images/time-series/time-series-query-actions.png "Time Series Query - Actions")

{WARNING: }

1. Click to open a tab with the time series query results shown in a table.  
   ![Time Series Query Results Table](images/time-series/query-results-table.png "Time Series Query Results Table")
2. Click to open a tab with the time series query results shown in a graph (see below).  
3. Multiple documents selection  
   ![Multiple Selection](images/time-series/multiple-selection.png "Multiple Selection")
    * a. Select multiple documents to plot their time series.  
    * b. Click to view the selected documents' time series results in a unified graph.  
      ![Unified Graph](images/time-series/unified-graph.png "Unified Graph")

{WARNING/}

---

#### Results in Graph View

![Time series results in a graph](images/time-series/time-series-graph-info.png "Time series results in a graph")

{INFO: }

1. A graph of time series results in a selected time frame.  
2. A graph showing all the time series results over time.  
3. The selected time frame.  
4. A legend of time series entries' values.  
   The entry values names are set in Studio's **Settings > Time Series** view.  

{INFO/}

---

![Time series results in a graph - Actions](images/time-series/time-series-graph-actions.png "Time series results in a graph - Actions")

{WARNING: }

1. Click to go back to the query results tab.  
2. Display tabs with graph or table results.  
3. Drag or resize the selected time frame to view the corresponding results.  
4. Hover your mouse over the graph to view the data of one particular entry.  
5. Toggle viewing points or a continuous line.  
6. Check/Uncheck to plot or hide time series entries' values on the graph.  

{WARNING/}  

{PANEL/}

## Related articles

**Document Extensins**:  
[Time Series Overview](../../../document-extensions/timeseries/overview)  
[Time Series Queries](../../../document-extensions/timeseries/querying/overview-and-syntax)  

**Querying**  
[What is RQL](../../../indexes/querying/what-is-rql)
