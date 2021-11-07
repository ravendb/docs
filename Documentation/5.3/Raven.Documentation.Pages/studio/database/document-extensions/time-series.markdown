# Time Series
---

{NOTE: }

* **Time series** are sets of numeric data items, associated with timestamps and ordered 
  by time.  
* The studio interface allows you to edit, query and index time series data, as well as 
  view it as a list of entries or as a graph.  
* An **Incremental Time Series** is a special type of time series that allows you to 
  handle time series values as counters.  
  Read more about incremental time series [here](../../../document-extensions/timeseries/incremental-time-series/overview), 
  and learn how to define them through Studio [below](../../../studio/database/document-extensions/time-series#incremental-time-series).  

* In this page:
  * [Document View](../../../studio/database/document-extensions/time-series#document-view)
  * [Time Series View](../../../studio/database/document-extensions/time-series#time-series-view)
     * [Create new Time Series (by Creating the First Entry)](../../../studio/database/document-extensions/time-series#create-new-time-series-by-creating-its-first-entry)
     * [Editing an Entry](../../../studio/database/document-extensions/time-series#editing-an-entry)
     * [Deleting a Range of Entries](../../../studio/database/document-extensions/time-series#deleting-a-range-of-entries)
  * [Querying Time Series](../../../studio/database/document-extensions/time-series#querying-time-series)
     * [Results in Graph View](../../../studio/database/document-extensions/time-series#results-in-graph-view)
  * [Incremental Time Series](../../../studio/database/document-extensions/time-series#incremental-time-series)
     * [Creating a new Incremental Time Series (by Creating its First Entry)](../../../studio/database/document-extensions/time-series#creating-a-new-incremental-time-series-by-creating-its-first-entry)
     * [Editing an Incremental Time Series Entry](../../../studio/database/document-extensions/time-series#editing-an-incremental-time-series-entry)
  

{NOTE/}

---

{PANEL: Document View}

![Time Series - Document View](images/time-series/document-time-series.png "Time Series - Document View")

{WARNING: Actions}

1. To view a document's time series, open its [document view](../../../studio/database/documents/document-view) 
   and click the time series tab on the right.  
2. Click 'Add Time Series' to create a new time series [see more below](../../../studio/database/document-extensions/time-series#create-new-time-series-by-creating-its-first-entry).  
3. Click to view and modify time series data.  

{WARNING/}

{INFO: Info}

* A. Time series info:  
  * B. **Name**  
    {NOTE: }
    [Incremental Time Series](../../../studio/database/document-extensions/time-series#incremental-time-series) 
    names always begin with `INC:`  
    E.g. **INC: Downloads** in the first time series listed above.  
    {NOTE/}
  * C. **Number of entries**  
  * D. **Range of time** from first to last entry  

{INFO/}  

{PANEL/}

{PANEL: Time Series View}

![Time Series View](images/time-series/time-series-view.png "Time Series View")

{WARNING: Actions}

1. Click to select another time series.  
2. Click to add a new entry to this time series (StockPrices), or click the dropdown to create a [new time series](../../../studio/database/document-extensions/time-series#create-new-time-series-by-creating-the-first-entry).  
3. Delete all entries from a specified time range.  
4. Edit entry.

{WARNING/}

{INFO: Info}

1. Time series entry info:  
    * Timestamp  
    * Numerical data (1-32 `double` values)  
    * Optional tag `string`

{INFO/}  

---

### Create new Time Series (by Creating its First Entry)

* A time series is created upon the creation of its first entry (and deleted 
  once all entries have been deleted).  

* Click the **Add Time Series** button from the 
  [Document View](../../../studio/database/document-extensions/time-series#document-view) 
  to create the time series' first entry.  

![Add Time Series](images/time-series/new-time-series.png "Add Time Series")

---

![Add Time Series Entry](images/time-series/new-entry.png "Add Time Series Entry")

{WARNING: }

1. **Time Series Type**  
    * **Diasble** to create a **non-incremental time series**.  
      (Learn now to create an incremental time series [here](../../../studio/database/document-extensions/time-series#incremental-time-series))  
2. **Time Series Name**  
    * Enter time series' name.  
    * Name cannot start with 'INC:' as this prefix is reserved for incremental time series.  
3. **Entry Timestamp**  
    * Select a [timestamp](../../../document-extensions/timeseries/overview#timestamps) for the new entry.  
4. **Tag** (Optional)  
    * Optionally give the entry a [tag](../../../document-extensions/timeseries/overview#tags).
5. **Values**  
    * Add one or more numerical [values](../../../document-extensions/timeseries/overview#values) (up to 32 values).  
      ![Named Values](images/time-series/named-values.png "Named Values")
    * Time series entry values can be given meaningful names rather than labels like Value #0 and Value #1.  
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

    {NOTE: }
     Make sure that the time series Name is placed within double quotes (e.g. "StockPrices", 
     "INC:Downloads", etc.), so special characters like ":" would not interfere with the query.  
    {NOTE/}
   

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
    * A. Select multiple documents to plot their time series.  
    * B. Click to view the selected documents' time series results in a unified graph.  
      ![Unified Graph](images/time-series/unified-graph.png "Unified Graph")

{WARNING/}

---

### Results in Graph View

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

{PANEL: Incremental Time Series}

* **Incremental Time Series** are time series whose values 
  behave as [counters](../../../document-extensions/counters/overview).  

* Similar to Counters, an incremental-time-series value can be increased/decreased by some 
  delta on any node.  
  Each node manages and stores its own accumulated local changes per value.  

* Simultaneous updates to the same value from multiple nodes do not create any conflict.  
  The value's total contents is simply the accumulation of that value's contents stored 
  per node in the cluster for the same timestamp.  

* This is opposed to non-incremental time series, where a value that is modified 
  is intended to replace the existing value's contents on all other nodes.  

* learn more in [Incremental Time Series -vs- Non-incremental Time Series](../../../document-extensions/timeseries/incremental-time-series/overview#incremental-time-series--vs--non-incremental-time-series).  

---

### Creating a new Incremental Time Series (by Creating its First Entry)

* An incremental time series is created upon the creation of its first entry (and deleted 
  once all entries have been deleted).  

* Click the **Add Time Series** button from the 
  [Document View](../../../studio/database/document-extensions/time-series#document-view) 
  to create the incremental time series' first entry.  

![Add Time Series](images/time-series/new-time-series.png "Add Time Series")

---

![Add Incremental Time Series Entry](images/time-series/new-incremental-entry.png "Add Incremental Time Series Entry")

{WARNING: }

1. **Time Series Type**  
    * Enable to create an Incremental time series.  
2. **Time Series Name**  
    * Enter time series' name.  
    * Incremental time series names **must** start with **INC:** (in either upper 
      or lower case characters).  
3. **Entry Timestamp**  
    * Select a [timestamp](../../../document-extensions/timeseries/overview#timestamps) for the new entry.  
4. **Values**  
    * Add one or more numerical [values](../../../document-extensions/timeseries/overview#values) (up to 32 values).  
      ![Named Values](images/time-series/incremental-named-values.png "Named Values")
    * Time series entry values can be given meaningful names rather than labels like Value #0 and Value #1.  
      To set entry values' names, use Studio's **Settings > Time Series** view.  

{WARNING/}

---

### Editing an Incremental Time Series Entry

* Click the **Edit Item** button from the 
  [Time Series View](../../../studio/database/document-extensions/time-series#time-series-view) 
  to edit a time series' entry.  

![Edit Time Series](images/time-series/edit-incremental-time-series.png "Edit Time Series")

---

![Incrementall Time Series Entry Accumulated Value](images/time-series/edit-incremental-time-series-entry-unified.png "Incrementall Time Series Entry Accumulated Value")

{WARNING: }

1. **Show values per node**  
    * Enable to view the value's contents distribution per cluster node (see below).  
      When disabled, only the current value (the value's total contents) is visible.  
2. **Values**  
    * **Current Value**  
      This is the value's total contents, which is the accumulation of this value's contents from all cluster nodes.  
    * **Increment By**  
      The number you enter here is the **delta** that will be added to the value's 
      contents on the current node.  
      Enter a positive number to increase the value or a negative number to decrease it.  
      The change in the value's total contents will be visible after you click the Save button.  
3. **Add Value**  
   Add an additional value (up to 32 values).  
4. **Save**  
   Click to save your changes.  

{WARNING/}

---

![Incrementall Time Series Entry Node Values](images/time-series/edit-incremental-time-series-entry-values.png "Incrementall Time Series Entry Node Values")

{WARNING: }

1. **Show values per node**  
    * Enable to view the value's contents distribution per cluster node.  
2. **Values**  
    * **Current Value**  
      This is the value's total contents, which is the accumulation of this value's contents from all cluster nodes.  
    * **Increment By**  
      The number you enter here is the **delta** that will be added to the value's 
      contents on the current node.  
      Enter a positive number to increase the value or a negative number to decrease it.  
      After clicking **Save**, both the current node and the total value will be updated 
      with the new accumulated value.  
      The value will be modified only when you click the **Save** button.  
3. **Add Value**  
   Add an additional value (up to 32 values).  
4. **Save**  
   Click to save your changes.  

{WARNING/}

{PANEL/}


## Related articles

**Document Extensions**:  
[Incremental Time Series: Overview](../../../document-extensions/timeseries/incremental-time-series/overview)  
[Incremental Time Series: Client API](../../../document-extensions/timeseries/incremental-time-series/client-api)  
[Time Series Overview](../../../document-extensions/timeseries/overview)  
[Time Series Queries](../../../document-extensions/timeseries/querying/overview-and-syntax)  

**Querying**  
[What is RQL](../../../indexes/querying/what-is-rql)
