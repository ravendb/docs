# Append Time-Series Entries

---

{NOTE: }

A time-series is an array of time-series entries. Each entry is 
populated with a **timestamp**, a **value (or values)**, and an 
optional **tag**.  

You can use the session `TimeSeriesFor.Append` method to add 
a single time-series entry, or update an existing entry.  

You can use the store `TimeSeriesBatchOperation` operation to 
add or update time-series entries in bulk.  

{INFO: }

* When time-series entries are appended to a **non-existing time-series**, 
  the **time-series is created**.  
* When a **non-existing time-series entry** is appended, the **time-series entry 
  is created**.  
* When **an existing time-series entry** is appended, the **entry 
  is updated** with the appended data.  

{INFO/}

* In this page:  
   * [Appending Time-Series Entries Using `session.TimeSeriesFor.Append`](../../../document-extensions/timeseries/client-api/append-time-series-entries#appending-time-series-entries-using-session.timeseriesfor.append)  
   * [Appending Time-Series Entries Using `TimeSeriesBatchOperation`](../../../document-extensions/timeseries/client-api/append-time-series-entries#appending-time-series-entries-using-timeseriesbatchoperation)  
{NOTE/}

---

{PANEL: Appending Time-Series Entries Using `session.TimeSeriesFor.Append`}

---

#### `TimeSeriesFor.Append` Definition

To append a time-series entry, use one of the two `TimeSeriesFor.Append` methods.  
One method updates the time-series entry with a value of type `double`.  
The second method updates the entry with a value of type `IEnumerable double`.  

* **First Overload**: Value type is `double`.  
  Use this method to update the time-series entry with a single value.  
     {CODE TimeSeriesFor-Append-definition-double@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

     | Parameters | Type | Description |
     |:-------------|:-------------|:-------------|
     | `timestamp` | DateTime | Time-series entry's timestamp |
     | `value` | double | Update the time-series entry with this value. <br> For a new time-series entry, this will be its initial value. |
     | `tag` | string | Time-series entry's tag <br> The tag is optional. |

* **Second Overload**: Value type is `IEnumerable double`.  
  Use this method to update the time-series entry with multiple values 
  of different types.  
     {CODE TimeSeriesFor-Append-definition-inum@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

     | Parameters | Type | Description |
     |:-------------|:-------------|:-------------|
     | `timestamp` | DateTime | Time-series entry's timestamp |
     | `values` | IEnumerable<double> | Update the time-series entry with these values. <br> For a new time-series entry, these will be its initial values. |
     | `tag` | string | Time-series entry's tag <br> The tag is optional. |

---

#### Usage Flow  

* Open a session.  
* Create an instance of `TimeSeriesFor`.  
    * Either pass `TimeSeriesFor` an explicit document ID, -or-  
    * Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
* Call `TimeSeriesFor.Append`.  
* Call `session.SaveChanges` for the action to take effect on the server.  

---

#### Code Samples  

Here, we pass `TimeSeriesFor.Append` the value using a `double`, 
and repeat it in a loop to append multiple time-series entries.  
Note that the session will still execute all actions in a single transaction.  
If you prefer a bulk operation, use 
[TimeSeriesBatchOperation](../../../document-extensions/timeseries/client-api/api-overview#managing-time-series-using-store-operations).  
{CODE timeseries_region_TimeSeriesFor-Append-TimeSeries-Range@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

Here, we pass an IEnumerable with three values to `TimeSeriesFor.Append`.  
The three values will be appended at the same timestamp.  
{CODE timeseries_region_Append-With-IEnumerable@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: Appending Time-Series Entries Using `TimeSeriesBatchOperation`}

`TimeSeriesBatchOperation` can append or remove multiple time-series entries.  
To instruct it which operations to perform, provide it with `TimeSeriesOperation` constructs.  

* `TimeSeriesOperation`  
  {CODE TimeSeriesOperation-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

---

#### Usage Flow  

* Create an instance of `TimeSeriesOperation`  
    * Populate it with a new `AppendOperation` list for every time-series entry you want to append.  
    * Populate each `AppendOperation` list with the properties of the time-series 
      entry that you want to append.  
* Create a `TimeSeriesBatchOperation` instance.  
    * Pass its constructor the document ID and the `TimeSeriesOperation` instance you've created.  
* Call `store.Operations.Send` to execute the operation.  
    * Pass it the `TimeSeriesBatchOperation` instance you've created.  

---

#### Code Sample

Here, we append a time-series two entries using `TimeSeriesBatchOperation`.  
{CODE timeseries_region_Append-Using-TimeSeriesBatchOperation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}

## Related articles
**Studio Articles**:  
[Studio Time-Series Management]()  

**Client-API - Session Articles**:  
[Time-Series Overview]()  
[Creating and Modifying Time-Series]()  
[Deleting Time-Series]()  
[Retrieving Time-Series Values]()  
[Time-Series and Other Features]()  

**Client-API - Operations Articles**:  
[Time-Series Operations]()  
