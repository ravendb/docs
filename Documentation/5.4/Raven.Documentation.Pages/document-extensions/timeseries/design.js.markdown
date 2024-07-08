# Time Series Design

---

{NOTE: }

* Time series are sequences of numerical values, associated with timestamps and sorted chronologically.  
 
* RavenDB time series are stored and managed as document extensions, achieving much greater speed and efficiency compared to storing them as JSON-formatted data within a document.

* In this page:  
  * [Time series architecture](../../document-extensions/timeseries/design#time-series-architecture)  
      * [Time series as a document extension](../../document-extensions/timeseries/design#time-series-as-a--document-extension)
      * [The `HasTimeSeries` flag](../../document-extensions/timeseries/design#the--flag)
      * [The time series entry](../../document-extensions/timeseries/design#the-time-series-entry)
      * [Segmentation](../../document-extensions/timeseries/design#segmentation)
      * [Compression](../../document-extensions/timeseries/design#compression)
  * [Updating time series](../../document-extensions/timeseries/design#updating-time-series)  
      * [Document change](../../document-extensions/timeseries/design#document-change)  
      * [No conflicts](../../document-extensions/timeseries/design#no-conflicts)  
      * [Transactions](../../document-extensions/timeseries/design#transactions)  
      * [Case insensitive](../../document-extensions/timeseries/design#case-insensitive)  

{NOTE/}

---

{PANEL: Time series architecture}

---

### Time series as a document extension  

* Each time series belongs to, or _extends_, one particular document.  

* The document and the time series reference each another:
  * The document's metadata keeps a reference to the time series **name**.  
    The time series **data** itself is stored in a separate location.
  * The [segments](../../document-extensions/timeseries/design#segmentation) containing the time series data keep a reference to the document ID.

---

### The&nbsp;`HasTimeSeries`&nbsp;flag

* When a document has one or more time series,  
  RavenDB automatically adds the `HasTimeSeries` flag to the document's metadata under `@flags`.

* When all time series are deleted from the document, RavenDB automatically removes the flag.
 
{CODE-BLOCK: JSON}
{
    "Name": "Paul",
    "@metadata": {
        "@collection": "Users",
        "@timeseries": [
            "my time series name"
    ]
    "@flags": "HasTimeSeries"
    }
}
{CODE-BLOCK/}

---

### The time series entry

Each time series entry is composed of a `TimeSeriesEntry` object which contains:

| Parameter     | Type       | Description                                                                                                                                                                                                                                                                      |
|---------------|------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **timestamp** | `Date`     | <ul><li>The time of the event represented by the entry.</li><li>Time is measured up to millisecond resolution.</li></ul>                                                                                                                                                         |
| **tag**       | `string`   | <ul><li>An optional tag for the entry.</li><li>Can be any string up to 255 bytes.</li><li>Possible uses for the tag:<br>Descriptions or metadata for individual entries.<br>Storing a related document id, which can then be referenced when querying the time series.</li></ul> |
| **values**    | `number[]` | <ul><li>An array of up to 32 values.</li></ul>                                                                                                                                                                                                                          |
| **value**     | `number`   | <ul><li>equivalent to values[0].</li></ul>                                                                                                                                                                                                                                       |

{NOTE: }

Doubles with higher precision - i.e. more digits after the decimal point, are much less compressible.  
In other words, `1.672` takes up more space than `1672`.

{NOTE/}

---

### Segmentation

* At the server storage level, time series data is divided into **segments**.  

* Each segment contains a number of consecutive entries from the same time series and aggregated values that RavenDB automatically updates in the segment's header. 
  See section [Segment properties](../../document-extensions/timeseries/indexing#segment-properties) for more details.

* **Segments size and limitations**:  
    * Segments have a maximum size of 2 KB.  
      What this limit practically means, is that a segment can only contain up to 32k entries.   
      Time series larger than that would always be stored in multiple segments.  
    * In practice, segments usually contain far less than 32k entries, depending on the size of the entries (after compression).  
      For example, in the [Northwind sample dataset](../../studio/database/tasks/create-sample-data), the _Companies_ documents all have a time series called _StockPrice_.  
      These time series are stored in segments that have ~10-20 entries each.  
    * The maximum time gap between the first and last entries in a segment is ~24.86 days  
      (equivalent to 2147483647 milliseconds, the maximum value of a 32-bit signed integer in C#).  
      Adding an entry that is further than that from the first segment entry, would add it as the first entry of a new segment.  
      As a consequence, segments of sparsely-updated time series can be significantly smaller than 2 KB.  
    * The maximum number of unique tags allowed per segment, is 127.  
      A higher number than that, would cause the creation of a new segment.  

* **Aggregated values**:  
  RavenDB automatically stores and updates aggregated values in each segment's header.   
  These values summarize commonly-used values regarding the segment, including -  
   - The segment's **Max** value  
   - The segment's **Min** value   
   - The segment's values **Sum**
   - The segment's **Count** of entries
   - The segment's **First** timestamp
   - The segment's **Last** timestamp

     {NOTE: }
     When segment entries store multiple values, e.g. each entry contains a _Latitude_ value and a _Longitude_ value,
     the six aggregated values are stored for each value separately.  
     {NOTE/}

---

### Compression

Time series data is stored using a format called [Gorilla compression](https://www.vldb.org/pvldb/vol8/p1816-teller.pdf).  
On top of the Gorilla compression, the time series segments are compressed using the [LZ4 algorithm](https://lz4.github.io/lz4/).

{PANEL/}

{PANEL: Updating Time Series}

---

### Document-change event  

* **Time series name update**:  
  Creating/deleting a time series adds/removes its name to/from the metadata of the document it belongs to.  
  This modification triggers a document-change event, thereby initiating various processes within RavenDB such as ongoing tasks, revision creation, subscriptions, etc.

* **Time series entries updates**:  
  As long as a new time series is not created, or an existing one is not removed,  
  modifying time series entries does Not invoke a document-change event, 

---

### No conflicts

Time series actions do not cause conflicts, updating a time series is designed to succeed without causing a concurrency conflict; 
as long as the document it extends exists, updating a time series will always succeed.  

* **Updating time series concurrently by multiple cluster nodes**:  
    
    When a time series' data is replicated by multiple nodes, the data from all nodes is merged into a single series.      
    
    When multiple nodes append **different values** at the same timestamp:  
      * If the nodes try to append a **different number of values** for the same timestamp, the greater number of values is applied.  
      * If the nodes try to append the **same number of values**, the first values from each node are compared.  
        The append whose first value sorts higher [_lexicographically_](https://mathworld.wolfram.com/LexicographicOrder.html) (not numerically) is applied.  
        For example, lexicographic order would sort numbers like this: `1 < 10 < 100 < 2 < 21 < 22 < 3`  
      * If an existing value at a certain timestamp is deleted by one node and updated by another node, the deletion is applied.  

* **Updating time series by multiple clients to the same node**:  
    * When a time series' value at a certain timestamp is appended by multiple clients more or less simultaneously,  
      RavenDB uses the last-write strategy.  
    * When an existing value at a certain timestamp is deleted by a client and updated by another client,  
      RavenDB still uses the last-write strategy.  

---

### Transactions

When a session transaction that includes a time series modification fails for any reason,  
the time series modification is reverted.  

---

### Case insensitive

All time series operations are case insensitive. E.g. -

{CODE-BLOCK:JSON}
session.timeSeriesFor("users/john", "HeartRate")
                        .deleteAt(timeStampOfEntry);
{CODE-BLOCK/}

is equivalent to

{CODE-BLOCK:JSON}
session.timeSeriesFor("users/john", "HEARTRATE")
                        .deleteAt(timeStampOfEntry);
{CODE-BLOCK/}

{PANEL/}

## Related articles

**Client API**  
[Time Series API Overview](../../document-extensions/timeseries/client-api/overview)  

**Studio Articles**  
[Studio Time Series Management](../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../document-extensions/timeseries/rollup-and-retention)
