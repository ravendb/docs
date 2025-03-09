# Stream Time Series Data

---

{NOTE: }

* This page explains how time series data can be streamed:  
  1. Stream a time series directly.
  2. Stream time series query results.

* In this page:
  * [Syntax](../../../document-extensions/timeseries/querying/stream-timeseries#syntax)
  * [Examples](../../../document-extensions/timeseries/querying/stream-timeseries#examples)

{NOTE/}

---

{PANEL: Syntax}

### Stream a time series directly:  

Get a time series (using `timeSeriesFor().get()`, and call `stream()`).  

{CODE-BLOCK: java}
<T> CloseableIterator<StreamResult<T>> stream(IRawDocumentQuery<T> query);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **query** | `IRawDocumentQuery<T>` | Raw data |


### Stream results of time series queries:  

This syntax is the same as the syntax for streaming query results in general, 
found [here](../../../client-api/session/querying/how-to-stream-query-results).  

{CODE:java stream_methods@DocumentExtensions/TimeSeries/StreamTimeSeries.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | [IDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.documentquery) or [IRawDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.rawquery) | Query to stream results for. |
| `Reference` **streamQueryStats** | StreamQueryStatistics | Information about performed query. |

| Return Value | |
| ------------- | ----- |
| CloseableIterator<StreamResult> | Iterator with entities. |

{PANEL/}

{PANEL: Examples}

[comment]: <
### Example I

Using `timeSeriesFor`:

{CODE:java direct@DocumentExtensions/TimeSeries/StreamTimeSeries.java /}

> (There isn't get to stream, a sample couldn't exists like that)

### Example 

Using a `rawQuery`:

{CODE:java query@DocumentExtensions/TimeSeries/StreamTimeSeries.java /}

{PANEL/}

## Related articles

### Document Extensions
- [Time Series Overview](../../../document-extensions/timeseries/overview)  
- [Range Selection](../../../document-extensions/timeseries/querying/choosing-query-range)  
- [Filtering](../../../document-extensions/timeseries/querying/filtering)  
- [Aggregation and Projection](../../../document-extensions/timeseries/querying/aggregation-and-projections)  
- [Get Time Series Entries](../../../document-extensions/timeseries/client-api/session/get/get-entries)  

### Client API
- [How To Stream Query Results](../../../client-api/session/querying/how-to-stream-query-results)  
