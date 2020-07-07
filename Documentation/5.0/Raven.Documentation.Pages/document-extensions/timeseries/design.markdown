# Design: Time Series

---

{NOTE: }

* Time series are sequences of numerical values, associated with timestamps 
  and sorted chronologically.  

* RavenDB Time Series are stored and managed as document extensions, gaining 
  much greater speed and efficiency than they would have had as JSON-formatted 
  data within a document.  

* In this page:  
  * [Time Series Structure](../../document-extensions/timeseries/design#time-series-structure)  
     * [Document Extension](../../document-extensions/timeseries/design#document-extension)  
     * [Time Series Entries](../../document-extensions/timeseries/design#time-series-entries)  
     * [The `HasTimeSeries` Flag](../../document-extensions/timeseries/design#the--flag)  
     * [Segmentation](../../document-extensions/timeseries/design#segmentation)  
     * [Compression](../../document-extensions/timeseries/design#compression)
  * [Updating Time Series](../../document-extensions/timeseries/design#updating-time-series)  
     * [Document Change](../../document-extensions/timeseries/design#document-change)  
     * [Success](../../document-extensions/timeseries/design#success)  
     * [No Conflicts](../../document-extensions/timeseries/design#no-conflicts)  
     * [Transactions](../../document-extensions/timeseries/design#transactions)  

{NOTE/}

---

{PANEL: Time Series Structure}

---

#### Document Extension  

Each time series belongs to, or _extends_, one particular document. The 
document and the time series reference each other through:  

* A **reference to the time series** in the document's metadata.  
  The time series' **name** is kept in the document's metadata.  
  The time series' **data** is stored in a separate location.  
* A **reference to the document** in the time series data.  

---

#### Time Series Entries

Each time series entry is composed of:  

* `TimeSeriesEntry` 

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `Timestamp` | DateTime (UTC) | The time of the event or data represented by the entry. Time is measured up to millisecond resolution. |
    | `Tag` | string | An optional tag for an entry. Can be any string up to 255 bytes. Possible uses for the tag: descriptions or metadata for individual entries; storing a document id, which can then be referenced when querying a time series. This is the only component of the entry that is not numerical. |
    | `Values` | double[] | An array of up to 32 `double` values |

{NOTE: }
Doubles with higher precision - i.e. more digits after the decimal point, 
are much less compressible.  
In other words, `1.672` takes up more space than `1672`.
{NOTE/}

---

#### The `HasTimeSeries` Flag

* When a document has one or more time series, RavenDB automatically adds  
  a `HasTimeSeries` flag in the document's metadata under `@flags`:

{CODE-BLOCK: JSON}
{
    "Name": "Paul",
    "@metadata": {
        "@collection": "Users",
        "@timeseries": [
            "my time series"
        ]
        "@flags": "HasTimeSeries"
    }
}
{CODE-BLOCK/}

* When all time series are removed from a document, RavenDB 
automatically removes the flag.  

---

#### Segmentation

At the server storage level, time series data is divided into **segments**.  

* Segments contain a number of consecutive entries from the same 
time series.  
* Segments have a maximum size of 2 KB.  
* The 2 KB limit means that a segment can only contain up to 32k entries,
so a time series with more than 32k entries will always be stored in
multiple segments.  
* In practice, segments usually contain far fewer than 32k entries, 
the amount depends on the size of the entries (after compression).  
  * As an example - in the [Northwind sample dataset](../../studio/database/tasks/create-sample-data), 
  the _Companies_ documents all have a time series called _StockPrice_. 
  These time series are stored in segments that have ~10-20 entries each.  
* The maximum gap between the first and last entries in a segment is 
is ~24.86 days (`int.MaxValue` milliseconds). So if an entry is 24.86 days 
or later than the first entry in the current segment, it will become the 
first entry in a new segment. A consquence of this is that in a sparse time 
series segments can be significantly smaller than 2 KB.  

Segments also store the aggregate values that summarize the data they 
contain, such as the averages of entry values. For this reason, it's 
often useful to reference individual segments in indexes and queries. 
Read more about how to use segments [here](../../document-extensions/timeseries/indexing#object)

---

#### Compression

Time series data is stored using a format called [Gorilla compression](https://www.vldb.org/pvldb/vol8/p1816-teller.pdf). 
On top of the Gorilla compression, the time series segments are compressed 
using the [LZ4 algorithm](https://lz4.github.io/lz4/).

{PANEL/}

{PANEL: Updating Time Series}

---

#### Document Change  

* **Name Change**  
  **Creating** or **removing** a time series adds or removes its name 
  from the metadata of the document it extends.  
  This modification counts as document-change event, and so it triggers 
  processes such as revisions.  
* **Data Updates**  
  Modifying time series data does **not** invoke a document-change event, 
  as long as it doesn't create a new time series or remove an existing one.  

---

#### Success

Updating a time series is designed to succeed without causing a concurrency conflict.  
As long as the document it extends exists, updating a time series will always succeed.  

---

#### No Conflicts

Time series actions do not cause conflicts.  

**Time series updated concurrently by multiple cluster nodes**:  

* When a time series' data is replicated by multiple nodes, the data 
from all nodes is merged into a single series.  
* When multiple nodes append **different values** at the same timestamp: 
  * If the nodes try to append a **different number of values** for the same 
    timestamp, the **bigger amount of values** is applied.  
  * If the nodes to append the same number of values, the first values from 
    each node are compared. The append whose first value sorts higher 
    [_lexicographically_](https://mathworld.wolfram.com/LexicographicOrder.html) 
    (not numerically) is applied.  
    For example, lexicographic order would sort numbers like this: 
    `1 < 10 < 100 < 2 < 21 < 22 < 3`  
  * If an existing value at a certain timestamp is deleted by one node 
    and updated by another node, the deletion is applied.  

**Time series update By multiple clients of the same node**:  

* When a time series' value at a certain timestamp is appended by 
multiple clients more or less simultaneously, RavenDB uses the last-write 
strategy.  
* When an existing value at a certain timestamp is deleted by a client 
and updated by another client, RavenDB still uses the last-write 
strategy.  

---

#### Transactions

When a session transaction that includes a time series modification 
fails for any reason, the time series modification is reverted.  

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
