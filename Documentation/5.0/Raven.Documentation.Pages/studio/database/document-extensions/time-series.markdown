# Time Series
---

{NOTE: }

Time series are sets of numeric data associated with timestamps and ordered by time. The studio interface 
allows you to edit, query and index time series data, as well as view it as list of entries or as a graph.  

* In this page:
  * [Document View](../../../studio/database/document-extensions/time-series#document-view)
    * [Create New Time Series / New Entry](../../../studio/database/document-extensions/time-series#create-new-time-series)
  * [Time Series View](../../../studio/database/document-extensions/time-series#time-series-view)
    * [Edit Entry](../../../studio/database/document-extensions/time-series#editing-an-entry)
    * [Delete Range](../../../studio/database/document-extensions/time-series#deleting-a-range-of-entries)
  * [Graphical Display & Querying](../../../studio/database/document-extensions/time-series#querying-time-series)
    * [Graph View](../../../studio/database/document-extensions/time-series#time-series-graph-view)

{NOTE/}

---

{PANEL: Document View}

![](images/document-time-series.png)  

{WARNING: Actions}
1. To view a documents' time series, go to its page and click the time series tab on the right.  
2. Allows you to create a new time series, see more below.  
3. Allows you to view the time series data.  
{WARNING/}

{INFO: Info}
1. Displays the time series':  

* Name  
* The number of entries it contains  
* The range of time from the first to the last entry in the time series
{INFO/}  

---

#### Create new Time Series (by Creating a First Entry)

![](images/new-entry.png)  

{WARNING: }
A time series must contain at least one entry when it is created (and a time series is deleted when all 
of its entries are deleted).  
1. Create the new time series' name.  
2. Select a timestamp for the new entry.  
3. Create an optional tag.  
4. Specify one or more numerical values.  
{WARNING/}

{PANEL/}

{PANEL: Time Series View}

![](images/time-series-view.png)  

{WARNING: }
1. Create a new entry to add to this time series.  
2. Delete all entries from a specified range of time.  
{WARNING/}

{INFO: }
Displays the time series entrys':  

* Timestamp  
* The numerical data: between 1 and 32 `double` values.  
* Optional tag `string`  
{INFO/}  

---

#### Editing an Entry

![](images/time-series-entry.png)  

{WARNING: }
1. Edit the optional tag.  
2. Edit a numerical value.  
3. Add another value (up to 32).  
4. Delete value.  
{WARNING/}

---

#### Deleting a Range of Entries

![](images/delete-range.png)  

{WARNING: }
To specify a range of time series entries:  
1. Select the first entry's timestamp as the start of the range.  
2. Specify a time value for the start of the range. Opens the menu shown below.  
3. Select the last entry's timestamp as the start of the range.  
{WARNING/}

![](images/delete-range-2.png)  

{PANEL/}

{PANEL: Querying Time Series}

![](images/time-series-query.png)  

{INFO: }
1. A simple query for all time series in the collection `Companies` and the name `StockPrice`.
2. A list of time series that satisfy the query, with:  

  * The ID of the associated document.
  * Number of entries and time range.
{INFO/}  

{WARNING: }
1. Run the query.  
2. Go to this series' time series view, discussed above.  
3. Go to graph view, shown below.  
{WARNING/}

---

#### Time Series Graph View

![](images/time-series-graph.png)  

{WARNING: }
1. Toggle a certain value in the graph.  
2. Hover your mouse over the graph to view the data of one particular entry.  
3. This bottom display shows the entire time series. Drag and resize the pink box to select 
the range of time displayed in the main graph above.
{WARNING/}

{INFO: }
1. A legend for the different lines by their color and their corresponding value's place in the 
entry's list of values.  
2. A hint box the appears when you hover your mouse over the graph, listing the values of that entry.  
{INFO/}  

{PANEL/}




## Related articles
