## `TimeSeriesBatchOperation`
# Append and Remove Time-Series Data

---

{NOTE: }

To **Append** and **Remove** multiple time-series entries, use `TimeSeriesBatchOperation`.  

* In this page:  
  * [`TimeSeriesBatchOperation`](../../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-ts-data#timeseriesbatchoperation)  
     * [Syntax](../../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-ts-data#syntax)  
     * [Usage Flow](../../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-ts-data#usage-flow)  
     * [Usage Samples](../../../../document-extensions/timeseries/client-api/store-operations/append-and-remove-ts-data#usage-samples)  
{NOTE/}

---

{PANEL: `TimeSeriesBatchOperation`}

`TimeSeriesBatchOperation` executes a list of time-series entries **Append** 
and **Remove** actions.  
The list is prepared beforehand in a `TimeSeriesOperation` instance using 
**TimeSeriesOperation.Append** and **TimeSeriesOperation.Remove**, and is 
passed to `TimeSeriesBatchOperation` as an argumenjt.  

{PANEL/}

{PANEL: Syntax}

* `TimeSeriesBatchOperation`  
  **This is the operation you need to execute to append and remove 
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
  as an argument, with a list of time-series entries Append and Remove actions.**  

      {CODE-BLOCK:JSON}
      public class TimeSeriesOperation
      {
          public string Name;
          public void Append(AppendOperation appendOperation)
          public void Remove(RemoveOperation removeOperation)

          //..
      }
      {CODE-BLOCK/}

         | Property | Type | Description |
         |:-------------|:-------------|:-------------|
         | `Name` | `string` | Time-Series name |
         | `Append` | `method` | Add an Append action to the list |
         | `Remove` | `method` | Add a Remove action to the list |


---

   * To add a time-series entry Append action, call `TimeSeriesOperation.Append`.
      {CODE-BLOCK:JSON}
      public void Append(AppendOperation appendOperation)
      {CODE-BLOCK/}
      {CODE-BLOCK:JSON}
      public class AppendOperation
      {
          public DateTime Timestamp;
          public double[] Values;
          public string Tag;

          //..
      }
      {CODE-BLOCK/}

         | Property | Type | Description |
         |:-------------|:-------------|:-------------|
         | `Timestamp` | `DateTime` | The TS entry will be appended at this timestamp |
         | `Values` | `double[]` | Entry values |
         | `Tag` | `string` | Entry tag (optional) |

---

   * To add a time-series entry Remove action, call `TimeSeriesOperation.Remove`.
      {CODE-BLOCK:JSON}
      public void Remove(RemoveOperation removeOperation)
      {CODE-BLOCK/}
      {CODE-BLOCK:JSON}
      public class RemoveOperation
      {
          public DateTime? From, To;

          //..
      }
      {CODE-BLOCK/}

         | Property | Type | Description |
         |:-------------|:-------------|:-------------|
         | `From` | `DateTime` | Range start <br> TS entries will be removed starting at this timestamp |
         | `To` | `DateTime` | Range end <br> entries will be removed up to this timestamp |

{PANEL/}


{PANEL: Usage Flow}

* Create an instance of `TimeSeriesOperation`  
   * Add it the **time-series name**.  

* Prepare the Append and Remove sequence.  
   * Call `TimeSeriesOperation.Append` to add an Append action  
   * Call `TimeSeriesOperation.Remove` to add a Remove action  

        {INFO: }
         NOTE: Remove actions will be executed **before** Append actions.  
        {INFO/}

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
