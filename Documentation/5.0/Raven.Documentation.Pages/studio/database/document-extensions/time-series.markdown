# Time Series
---

{NOTE: }

Time series are sets of numeric data associated with timestamps and ordered by time. The studio interface 
allows you to edit, query and index time series data, as well as view it as list of entries or as a graph.  

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
2. Click to create a new time series, see more below.  
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

1. Add a new entry to this time series (StockPrices) or click the dropdown to create a new time series.  
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

#### Create new Time Series (by Creating the First Entry)

![New Time Series Entry](images/time-series/new-entry.png "New Time Series Entry")

A time series must contain at least one entry when it is created (and a time series is deleted 
when all of its entries are deleted).  

{WARNING: Actions}

1. Select a [timestamp](../../../document-extensions/timeseries/overview#timestamps) for the new entry.  
2. Create an optional [tag](../../../document-extensions/timeseries/overview#tags).  
3. Add one or more numerical [values](../../../document-extensions/timeseries/overview#values).  
   ![Named Values](images/time-series/named-values.png "Named Values")
   Time series entries can be automatically named.  
   In the entry shown above, the first value was automatically named "BPM1", and the second "BPM2".  
   To set the names given to entry values, use Studio's **Settings > Time Series** view.  

{WARNING/}

{INFO: Info}

1. Time series' document ID  
2. Time series' name  

{INFO/}

---

#### Editing an Entry

![Edit Time Series Entry](images/time-series/time-series-entry.png "Edit Time Series Entry")

{WARNING: }

1. Edit the optional tag.  
2. Edit a numerical value.  
3. Add an additional value (up to 32 values).  
4. Delete value.  

{WARNING/}

---

#### Deleting a Range of Entries

![Delete Range](images/time-series/delete-range.png "Delete Range")

{WARNING: }

To specify a range of time series entries:  

1. Check **Use minimum** to use the first entry's timestamp as the start of the range,  
   -or-  
   Click the time bar to open the menu shown below and specify some other start date.  
2. Check **Use maximum** to use the last entry's time stamp as the end of the range,  
   -or-  
   Click the time bar to open the menu shown below and specify some other end date.  

{WARNING/}

![Delete Range - Pick Date and Time](images/time-series/delete-range-2.png "Delete Range - Pick Date and Time")

{PANEL/}

{PANEL: Querying Time Series}

![Time Series Query](images/time-series/time-series-query.png "Time Series Query")

{INFO: }

1. A query to get all the **StockPrices** time series values from documents in the Companies collection.  
   Learn more about time series queries [here](../../../document-extensions/timeseries/querying/overview-and-syntax).  
2. A list of time series that satisfy the query, with:  
    * The ID of the associated document.  
    * Number of entries and time range.  
3. Multiple selection checkbox  
   ![Multiple Selection](images/time-series/multiple-selection.png "Multiple Selection")
    * a. Check to select the time series you want to plot.  
    * b. Click to view the selected time series' results in a unified graph.  
      ![Unified Graph](images/time-series/unified-graph.png "Unified Graph")

{INFO/}  

---

![Time Series Query - Actions](images/time-series/time-series-query-actions.png "Time Series Query - Actions")

{WARNING: }

1. Run the query.  
2. Click to open a tab with the time series query results shown in a table.  
   ![Time Series Query Results Table](images/time-series/query-results-table.png "Time Series Query Results Table")
3. Click to open a tab with the time series query results shown in a graph (see below).  

{WARNING/}

---

#### Results in Graph View

![Time series results in a graph](images/time-series/time-series-graph-info.png "Time series results in a graph")

{WARNING: Actions}

1. Click for the textual query results view.  
2. Click to view time series values' results in a graph.  

{WARNING/}  

{INFO: Info}

1. A graph of a **selected time range** within the time series results.  
2. A graph of the **entire time series** results, with a time range selection frame.  
3. A legend of time series entries' values.  
   (The entry values in the above example were named "Open", "Close" and so on in 
   Studio's **Settings > Time Series** view.)  

{INFO/}

---

![Time series results in a graph - Actions](images/time-series/time-series-graph-actions.png "Time series results in a graph - Actions")

{WARNING: }

1. Drag or resize the time range selection frame to change the selection.  
2. Hover your mouse over the graph to view the data of one particular entry.  
3. Toggle individual dots / single line view.  
4. Check/Uncheck to plot or hide time series entries' values on the graph.  

{WARNING/}  

{PANEL/}

## Related articles

**Document Extensins**:  
[Time Series Overview](../../../document-extensions/timeseries/overview)  
[Time Series Queries](../../../document-extensions/timeseries/querying/overview-and-syntax)  

**Querying**  
[What is RQL](../../../indexes/querying/what-is-rql)
