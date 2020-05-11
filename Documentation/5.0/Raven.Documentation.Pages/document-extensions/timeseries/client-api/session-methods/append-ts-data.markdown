# Append Time-Series Data

---

{NOTE: }

Append time-series data using the session method `TimeSeriesFor.Append`.  
You can append a single time-series entry at a time.  
An entry contains a **timestamp** that marks the entry's location in the 
time-series, 1 to 32 `double`-type **values**, and optionally a **tag**.  

* **Creating a time-series**  
  Appending a time-series entry to a non-existing time-series, 
  **creates the time-series** and adds it the new entry.  
* **Creating a time-series entry**  
  Appending an existing time-series a new entry, **adds the entry** 
  to the series at the provided timestamp.  
* **Updating a time-series entry**  
  Appending a time-series entry to a time-series that already contains 
  an entry at this timestamp, **updates the existing entry** with the 
  appended data. 

* In this page:  
   * [`TimeSeriesFor.Append` Definition](../../../../document-extensions/timeseries/client-api/session-methods/append-ts-data#timeseriesfor.append-definition)  
   * [Usage Flow](../../../../document-extensions/timeseries/client-api/session-methods/append-ts-data#usage-flow)  
   * [Usage Samples](../../../../document-extensions/timeseries/client-api/session-methods/append-ts-data#usage-samples)  
{NOTE/}

---

{PANEL: `TimeSeriesFor.Append` Definition}

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

{PANEL/}

{PANEL: Usage Flow}

* Open a session.  
* Create an instance of `TimeSeriesFor`.  
    * Either pass `TimeSeriesFor` an explicit document ID, -or-  
    * Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
* Call `TimeSeriesFor.Append`.  
* Call `session.SaveChanges` for the action to take effect on the server.  

{PANEL/}

{PANEL: Usage Samples}

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
