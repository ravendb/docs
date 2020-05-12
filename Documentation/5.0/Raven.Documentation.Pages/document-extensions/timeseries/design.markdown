# Time-Series Design

---

{NOTE: }

A time-series is an array of Gorilla-encoded `IEnumerable` entries 
that extends a specific single document.  

* In this page:  
  * [Time-Series Structure](../../document-extensions/timeseries/design#time-series-structure)  
     * [Document Extension](../../document-extensions/timeseries/design#document-extension)  
     * [The `HasTimeSeries` Flag](../../document-extensions/timeseries/design#the--flag)  
     * [Segmentation](../../document-extensions/timeseries/design#segmentation)  
     * [Time-Series Entries](../../document-extensions/timeseries/design#time-series-entries)  
  * [Updating Time-Series](../../document-extensions/timeseries/design#updating-time-series)  
     * [Document Change](../../document-extensions/timeseries/design#document-change)  
     * [Success](../../document-extensions/timeseries/design#success)  
     * [No Conflicts](../../document-extensions/timeseries/design#no-conflicts)  
     * [Transactions](../../document-extensions/timeseries/design#transactions)  

{NOTE/}

---

{PANEL: Time-Series Structure}

---

#### Document Extension  

A time-series is related to the document it extsnds, by -  

* A **reference to the time-series** in the document's metadata.  
  The time-series' **name** is kept in the document's metadata.  
  The time-series' **data** is stored in a different location.  '
* A **reference to the document** in the time-series' data.  

---

#### The `HasTimeSeries` Flag

* When the first time-series is added to a document, RavenDB automatically sets 
  a `HasTimeSeries` Flag in the document's metadata.
  {CODE-BLOCK: JSON}
{
    "Name": "Paul",
    "@metadata": {
        "@collection": "Users",
        "@flags": "HasTimeSeries"
    }
}
{CODE-BLOCK/}


* When all time-series have been removed from a document, RavenDB will 
  automatically remove the flag.  

---

#### Segmentation

A time-series array is constructed of segments.  

* A time-series of up to 32k entries is contained in a single segment.  
* A time-series longet than 32k entries is contained in multiple segments.  
* A new segment is also created when a period of more than 25 days has passed 
  since the last measurement.  

---

#### Time-Series Entries

Each time-series entry is an `IEnumerable` with three values.  

* `TimeSeriesEntry` 

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `Timestamp` | DateTime | Time signature |
    | `Tag` | string | Optional tag |
    | `Values` | double[] | An array of up to 32 values |

{PANEL/}

{PANEL: Updating Time-Series}

---

#### Document Change  

* **Name Change**  
  **Creating** or **removing** a time-series adds or removes its name 
  to/from the metadata of the document it extends.  
  These actions **will** invoke a document-change event.  
* **Data Updates**  
  Modifying time-series data does **not** invoke a document-change event, 
  as long as it doesn't create a new time-series or remove an existing one.  

---

#### Success

Updating a time-series is designed to succeed without causing any conflict.  
As long as the document it extends exists, updating a Time-Serie will always succeed.  

---

#### No Conflicts

Time-series actions do not cause conflicts.  

* **Time-series update by multiple cluster Nodes**  
   * When a time-series' data is replicated by multiple nodes, the data 
     from all nodes is merged into a single series.  
   * When multiple nodes append **different values** at the same timestamp, 
     the **bigger value** is chosen for this entry.  
   * When multiple nodes apppend **different amount of values** for the same 
     timestamp, the **bigger amount of values** is chosen for this entry.  
   * When an existing value at a certain timestamp is deleted by a node 
     and updated by another node, the deletion is chosen.  

* **Time-series update By multiple clients of the same node**  
   * When a time-series' value at a certain timestamp is appended by 
     multiple clients more or less simultaneously, the last one is chosen.  
   * When an existing value at a certain timestamp is deleted by a client 
     and updated by another client, the last action is chosen.  

---

#### Transactions

When a session transaction that includes a time-series modification 
fails for any reason, the time-series modification is reverted.  

{PANEL/}

## Related articles
**Studio Articles**:  
[Studio Time Series Management]()  

**Client-API - Session Articles**:  
[Time Series Overview]()  
[Creating and Modifying Time Series]()  
[Deleting Time Series]()  
[Retrieving Time Series Values]()  
[Time Series and Other Features]()  

**Client-API - Operations Articles**:  
[Time Series Operations]()  
