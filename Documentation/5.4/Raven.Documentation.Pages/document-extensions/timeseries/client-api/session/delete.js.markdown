# Delete Time Series

---

{NOTE: }

* Use `timeSeriesFor.delete` for the following actions:

    * __Delete a single time series entry__
  
    * __Delete a range of entries__
  
    * __Delete the whole time series__

* In this page:  
    * [`delete` usage](../../../../document-extensions/timeseries/client-api/session/delete#delete-usage)
    * [Examples](../../../../document-extensions/timeseries/client-api/session/delete#examples)
      * [Delete single entry](../../../../document-extensions/timeseries/client-api/session/delete#delete-single-entry)
      * [Delete range of entries](../../../../document-extensions/timeseries/client-api/session/delete#delete-range-of-entries)
      * [Delete time series](../../../../document-extensions/timeseries/client-api/session/delete#delete-time-series)
    * [Syntax](../../../../document-extensions/timeseries/client-api/session/delete#syntax)
  
{NOTE/}

---

{PANEL: `Delete` usage}

__Flow__:

* Open a session.  
* Create an instance of `timeSeriesFor` and pass it the following:
    * Provide an explicit document ID, or -   
      pass an [entity tracked by the session](../../../../client-api/session/what-is-a-session-and-how-does-it-work#unit-of-work-pattern),
      e.g. a document object returned from [session.query](../../../../client-api/session/querying/how-to-query) or from [session.load](../../../../client-api/session/loading-entities#load).
    * Specify the time series name.
* Call `TimeSeriesFor.Delete`:
    * Provide a single timestamp to delete a specific entry, or -
    * Specify a range of timestamps to delete multiple entries.
* Call `session.saveChanges` for the action to take effect on the server.  

__Note__:

* If the specified document doesn't exist, a `DocumentDoesNotExistException` is thrown.
* Attempting to delete nonexistent entries results in a no-op and generates no exception.
* To delete the whole time series simply delete all its entries.  
  The series is removed when all its entries are deleted.
* Deleting a document deletes all its time series as well.

{PANEL/}

{PANEL: Examples}

The following examples delete the time series entries that were appended in the [Append](../../../../document-extensions/timeseries/client-api/session/append) article:

{NOTE: }

<a id="delete-single-entry" /> __Delete single entry__:

---

{CODE:nodejs delete_1@documentExtensions\timeSeries\client-api\deleteTimeSeries.js /}

{NOTE/}

{NOTE: }

<a id="delete-range-of-entries" /> __Delete range of entries__:

---
 
{CODE:nodejs delete_2@documentExtensions\timeSeries\client-api\deleteTimeSeries.js /}

{NOTE/}

{NOTE: }

<a id="delete-time-series" /> __Delete time series__:

---

{CODE:nodejs delete_3@documentExtensions\timeSeries\client-api\deleteTimeSeries.js /}

{NOTE/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@documentExtensions\timeSeries\client-api\deleteTimeSeries.js /}

| Parameter | Type   | Description                                                                                                                     |
|-----------|--------|:--------------------------------------------------------------------------------------------------------------------------------|
| __at__    | `Date` | Timestamp of the time series entry to delete.                                                                                   |
| __from__  | `Date` | Delete the range of time series entries starting from this timestamp (inclusive).<br>Pass `null` to use the minimum date value. |
| __to__    | `Date` | Delete the range of time series entries ending at this timestamp (inclusive).<br>Pass `null` to use the maximum date value.     |

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
