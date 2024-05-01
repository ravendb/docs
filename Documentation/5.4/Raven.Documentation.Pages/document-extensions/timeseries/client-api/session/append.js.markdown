# Append & Update Time Series

---

{NOTE: }

* Use `timeSeriesFor.append` for the following actions:

  * __Create a new time series__:  
    Appending an entry to a time series that doesn't exist yet  
    will create the time series and add the new entry to it.
  
  * __Create a new time series entry__:  
    Appending a new entry to an existing time series  
    will add the entry to the series at the specified timestamp.  

  * __Modify an existing time series entry__:  
    Use _append_ to update the data of an existing entry with the specified timestamp.

* Each call to `append` handles a single [time series entry](../../../../document-extensions/timeseries/design#time-series-entries).  

* To append multiple entries in a single transaction,  
  call `append` as many times as needed before calling `session.saveChanges`.

---

* In this page:  
  * [`append` usage](../../../../document-extensions/timeseries/client-api/session/append#append-usage)  
  * [Examples](../../../../document-extensions/timeseries/client-api/session/append#examples)
      * [Append entries with single value](../../../../document-extensions/timeseries/client-api/session/append#append-entries-with-single-value)  
      * [Append entries with multiple values](../../../../document-extensions/timeseries/client-api/session/append#append-entries-with-multiple-values)  
  * [Syntax](../../../../document-extensions/timeseries/client-api/session/append#syntax)
  
{NOTE/}

---

{PANEL: `append` usage}

__Flow__:  

* Open a session.
* Create an instance of `timeSeriesFor` and pass it the following:
    * Provide an explicit document ID, -or-  
      pass an [entity tracked by the session](../../../../client-api/session/loading-entities), 
      e.g. a document object returned from [session.query](../../../../client-api/session/querying/how-to-query) or from [session.load](../../../../client-api/session/loading-entities#load).  
    * Specify the time series name.
* Call `timeSeriesFor.append` and pass it the time series entry details.
* Call `session.saveChanges` for the action to take effect on the server.

__Note__:  

* A `DocumentDoesNotExistException` exception is thrown if the specified document does not exist.

{PANEL/}

{PANEL: Examples}

{NOTE: }

<a id="append-entries-with-single-value" /> __Append entries with single value__:

---

* In this example, entries are appended with a single value.

* Although a loop is used to append multiple entries,  
  all entries are appended in a single transaction when `saveChanges` is executed.  

{CODE:nodejs append_1@documentExtensions\timeSeries\client-api\appendTimeSeries.js /}

{NOTE/}

{NOTE: }

<a id="append-entries-with-multiple-values" /> __Append entries with multiple values__:

---

* In this example, we append multi-value StockPrice entries.  

* Notice the clarity gained by [naming the values](../../../../document-extensions/timeseries/client-api/named-time-series-values).

{CODE-TABS}
{CODE-TAB:nodejs:Native append_2@documentExtensions\timeSeries\client-api\appendTimeSeries.js /}
{CODE-TAB:nodejs:Using_named_values append_3@documentExtensions\timeSeries\client-api\appendTimeSeries.js /}
{CODE-TAB:nodejs:StockPrice_class stockPrice_class@documentExtensions\timeSeries\client-api\appendTimeSeries.js /}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@documentExtensions\timeSeries\client-api\appendTimeSeries.js /}
      
{CODE:nodejs syntax_2@documentExtensions\timeSeries\client-api\appendTimeSeries.js /}

{CODE:nodejs syntax_3@documentExtensions\timeSeries\client-api\appendTimeSeries.js /}

| Parameter     | Type     | Description                    |
|---------------|----------|--------------------------------|
| __timestamp__ | Date     | Time series entry's timestamp  |
| __value__     | number   | Entry's value                  |
| __values__    | number[] | Entry's values                 |
| __tag__       | string   | An optional tag for the entry  |
| __entry__     | object   | object with the entry's values |

{PANEL/}

## Related articles

**Client API**  
[Time Series API Overview](../../../../document-extensions/timeseries/client-api/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../document-extensions/timeseries/rollup-and-retention)  
