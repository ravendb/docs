## `TimeSeriesFor.Append`
# Append Time-Series Data

---

{NOTE: }

* Create and update time-series and their entries using `TimeSeriesFor.Append`.  

* You can append a single [time-series entry](../../../../document-extensions/timeseries/design#time-series-entries) at a time.  
  Note, however, that you can `Append` as many times as you need to before calling 
  `session.SaveChanges`, to append multiple entries in a single transaction.  

* In this page:  
      * [`TimeSeriesFor.Append`](../../../../document-extensions/timeseries/client-api/session-methods/append-ts-data#timeseriesfor.append)  
      * [Syntax](../../../../document-extensions/timeseries/client-api/session-methods/append-ts-data#syntax)  
         * [Overload 1 - Append Entry With Single Value](../../../../document-extensions/timeseries/client-api/session-methods/append-ts-data#overload-1)  
         * [Overload 2 - Append Entry With Multiple Values](../../../../document-extensions/timeseries/client-api/session-methods/append-ts-data#overload-2)  
      * [Usage Flow](../../../../document-extensions/timeseries/client-api/session-methods/append-ts-data#usage-flow)  
      * [Usage Samples](../../../../document-extensions/timeseries/client-api/session-methods/append-ts-data#usage-samples)  
{NOTE/}



---

{PANEL: `TimeSeriesFor.Append`}

* `TimeSeriesFor.Append` is used for the creation of time-series and 
  time-series entries, and for the modification of entries values.  
   * **Creating a Time-Series**  
     Append an entry to a time-series that doesn't exist yet,  
     to create the time-series and add it the new entry.  
   * **Creating a Time-Series Entry**  
     Append an existing time-series a new entry,  
     to add the entry to this series at the specified timestamp.  
   * **Modifying Entry Values**  
     Append a time-series an entry it already has,  
     to update the existing entry with the new data. 

{PANEL/}

{PANEL: Syntax}

There are two `TimeSeriesFor.Append` methods:  
[Overload 1](../../../../document-extensions/timeseries/client-api/session-methods/append-ts-data#overload-1) 
- Append an entry with a single value.  
[Overload 2](../../../../document-extensions/timeseries/client-api/session-methods/append-ts-data#overload-2) 
- Append an entry with multiple values.  

---

#### Overload 1:  

* **Definition**
  {CODE TimeSeriesFor-Append-definition-double@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `timestamp` | DateTime | Time-series entry's timestamp |
    | `value` | double | Entry's value |
    | `tag` | string | Entry's tag <br> The tag is optional. |

* **Return Value**  
  No return value.  

* **Exceptions**  
  If the document doesn't exist, a `DocumentDoesNotExistException` exception is thrown.  

---

#### Overload 2:  
* **Definition**
  {CODE TimeSeriesFor-Append-definition-inum@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `timestamp` | DateTime | Time-series entry's timestamp |
    | `values` | IEnumerable<double> | Entry's values |
    | `tag` | string | Entry's tag <br> The tag is optional. |

* **Return Value**  
  No return value.  

* **Exceptions**  
  If the document doesn't exist, a `DocumentDoesNotExistException` exception is thrown.  


{PANEL/}

{PANEL: Usage Flow}

* Open a session.  
* Create an instance of `TimeSeriesFor`.  
    * Either pass `TimeSeriesFor` an explicit document ID, -or-  
      Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.Query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
    * Pass `TimeSeriesFor` the time-series name.  
* Call `TimeSeriesFor.Append`.  
* Call `session.SaveChanges` for the action to take effect on the server.  

{PANEL/}

{PANEL: Usage Samples}

* In this sample we use the [first overload](../../../../document-extensions/timeseries/client-api/session-methods/append-ts-data#overload-1) 
  to append an entry with a single value.  
  Though We run a loop to append multiple entries, all entries are appended in a single 
  transaction when `SaveChanges` is called.  
   {CODE timeseries_region_TimeSeriesFor-Append-TimeSeries-Range@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample we use the [second overload](../../../../document-extensions/timeseries/client-api/session-methods/append-ts-data#overload-2) 
  to append a time-series entry with three values.  
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
