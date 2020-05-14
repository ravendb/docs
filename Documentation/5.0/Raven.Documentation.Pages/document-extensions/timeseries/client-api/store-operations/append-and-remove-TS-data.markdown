## `TimeSeriesBatchOperation`
# Append and Remove Time-Series Data

---

{NOTE: }

To append or remove single or multiple time-series entries, 
use `TimeSeriesBatchOperation`.  
You can create a list of Append and Remove actions, and 
execute them all in a single call.  

* In this page:  
  * [`TimeSeriesBatchOperation`](../../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-ts-data#timeseriesbatchoperation)  
     * [Syntax](../../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-ts-data#syntax)  
     * [Usage Flow](../../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-ts-data#usage-flow)  
     * [Usage Samples](../../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-ts-data#usage-samples)  
{NOTE/}

---

{PANEL: `TimeSeriesBatchOperation`}

To instruct `TimeSeriesBatchOperation` which actions to perform, pass it 
an `TimeSeriesOperation` instance for each Append or Remove action.  

{PANEL/}

{PANEL: Syntax}

* `TimeSeriesBatchOperation`  
  **This is the operation you need to execute to appends and removes 
  time-series entries.**  

   * **Definition**  
     {CODE TimeSeriesBatchOperation-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  
   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `documentId` | `string` | ID of the document to append TS data to |
        | `operation` | `TimeSeriesOperation` | Operation configuration class: <br> Which Append/Remove actions to perform |

* `TimeSeriesOperation`  
  **This is the configuration class provided to `TimeSeriesBatchOperation` 
  as an argument, with a list of TS-entries Append and Remove actions.**  

   * **Definition**  
     {CODE TimeSeriesOperation-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  
   * **Properties**  

        | Property | Type | Description |
        |:-------------|:-------------|:-------------|
        | `Appends` | `List<AppendOperation>` | A list of TS-entry Append actions |
        | `Removals` | `List<RemoveOperation>` | A list of TS-entry Remove actions |

* `AppendOperation`  
  **This class defines a single TS-entry Append action.**

   * **Definition**  
     {CODE AppendOperation-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  
   * **Properties**  

        | Property | Type | Description |
        |:-------------|:-------------|:-------------|
        | `Timestamp` | `DateTime` | The TS entry will be appended at this timestamp |
        | `Values` | `double[]` | New values for the TS entry |
        | `Tag` | `string` | An optional tag for the TS entry |

* `RemoveOperation`  
  **This class defines a single Remove action, of a range of TS entries.**

   * **Definition**  
     {CODE RemoveOperation-class@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  
   * **Properties**  

        | Property | Type | Description |
        |:-------------|:-------------|:-------------|
        | `From` | `DateTime` | Range start: TS entries will be removed from this timestamp on |
        | `To` | `DateTime[]` | Range end: entries will be removed to this timestamp |

{PANEL/}


{PANEL: Usage Flow}

* Create an instance of `TimeSeriesOperation`  

* Add the `TimeSeriesOperation` instance an `AppendOperation` list.  
  Add the list an `AppendOperation` instance for each Append action you want 
  to perform.  

* Add the `TimeSeriesOperation` instance a `RemoveOperation` list.  
  Add the list a `RemoveOperation` instance for each Remove action you want 
  to perform.  

* Create a `TimeSeriesBatchOperation` instance.  
  Pass it the **document ID** and your **`TimeSeriesOperation` instance**  

* Call `store.Operations.Send` with your `TimeSeriesBatchOperation` 
  instance to execute the operation.  

{PANEL/}


{PANEL: Usage Samples}

* In this sample, we use `TimeSeriesBatchOperation` to append 
  a time-series two entries.  
   {CODE timeseries_region_Append-Using-TimeSeriesBatchOperation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we remove two ranges of entries from a time-series.  
   {CODE timeseries_region_Remove-Range-Using-TimeSeriesBatchOperation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
