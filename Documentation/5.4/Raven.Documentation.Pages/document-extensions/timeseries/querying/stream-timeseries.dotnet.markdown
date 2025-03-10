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

Get a time series (e.g. [using `TimeSeriesFor.Get`](../../../document-extensions/timeseries/client-api/session/get/get-entries)), 
and call `Stream()`/`StreamAsync()`.  

{CODE-BLOCK: csharp}
IEnumerator<T> Stream(DateTime? from = null, DateTime? to = null, TimeSpan? offset = null);

Task<IAsyncEnumerator<T>> StreamAsync(DateTime? from = null, DateTime? to = null, TimeSpan? offset = null);
{CODE-BLOCK/}

| Parameters | Type | Description |
| - | - | - |
| **from** | `DateTime?` | Start the stream from a certain time. If null, stream starts from the beginning of the time series. |
| **to** | `DateTime?` | Stop the stream at a certain time. If null, stream stops at the end of the time series. |
| **offset** | `TimeSpan?` | Change the timestamp of the streamed time series entries by adding this amount of time. |

### Stream results of time series queries:  

This syntax is the same as the syntax for streaming query results in general, 
found [here](../../../client-api/session/querying/how-to-stream-query-results).  

{CODE stream_methods@DocumentExtensions/TimeSeries/StreamTimeSeries.cs /}

| Parameters | Type | Description |
| - | - | - |
| **query** | [IQueryable](../../../client-api/session/querying/how-to-query#session.query), [IDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.documentquery) or [IRawDocumentQuery](../../../client-api/session/querying/how-to-query#session.advanced.rawquery) | Query to stream results for. |
| **streamQueryStats** | `out` [StreamQueryStatistics](../../../glossary/stream-query-statistics) | Information about performed query. |

| Return Value | Description |
| - | - |
| IEnumerator<[StreamResult](../../../glossary/stream-result)> | Enumerator with entities. |

{PANEL/}

{PANEL: Examples}

### Example I

Using `TimeSeriesFor`:

{CODE-TABS}
{CODE-TAB:csharp:Sync direct@DocumentExtensions/TimeSeries/StreamTimeSeries.cs /}
{CODE-TAB:csharp:Async direct_async@DocumentExtensions/TimeSeries/StreamTimeSeries.cs /}
{CODE-TABS/}

### Example II

Using a `RawQuery`:

{CODE-TABS}
{CODE-TAB:csharp:Sync query@DocumentExtensions/TimeSeries/StreamTimeSeries.cs /}
{CODE-TAB:csharp:Async query_async@DocumentExtensions/TimeSeries/StreamTimeSeries.cs /}
{CODE-TABS/}
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
