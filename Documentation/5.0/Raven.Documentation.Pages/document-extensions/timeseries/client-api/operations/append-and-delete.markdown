# Operations: Append & Delete Time Series

---

{NOTE: }

To **Append** and **Delete** multiple time series entries, use `TimeSeriesBatchOperation`.  

* In this page:  
  * [`TimeSeriesBatchOperation`](../../../../document-extensions/timeseries/client-api/operations/append-and-delete#timeseriesbatchoperation)  
     * [Syntax](../../../../document-extensions/timeseries/client-api/operations/append-and-delete#syntax)  
     * [Usage Flow](../../../../document-extensions/timeseries/client-api/operations/append-and-delete#usage-flow)  
     * [Usage Samples](../../../../document-extensions/timeseries/client-api/operations/append-and-delete#usage-samples)  
{NOTE/}

---

{PANEL: `TimeSeriesBatchOperation`}

`TimeSeriesBatchOperation` executes a list of time series entries **Append** 
and **Delete** actions.  
The list is prepared in advance in a `TimeSeriesOperation` instance using 
**TimeSeriesOperation.Append** and **TimeSeriesOperation.Delete**, and is 
passed to `TimeSeriesBatchOperation` as an argument.  

{PANEL/}

{PANEL: Syntax}

* `TimeSeriesBatchOperation`  
  **This is the operation you need to execute to append and delete time series entries.**  

   * **Definition**  
     {CODE TimeSeriesBatchOperation-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

   * **Parameters**  

        | Parameters | Type | Description |
        |:-------------|:-------------|:-------------|
        | `documentId` | `string` | ID of the document to append TS data to |
        | `operation` | `TimeSeriesOperation` | Operation configuration class: <br> Which Append/Delete actions to perform |

* `TimeSeriesOperation`  
  **This is the configuration class provided to `TimeSeriesBatchOperation` 
  as an argument, with a list of time series entries Append and Delete actions.**  

      {CODE-BLOCK:JSON}
      public class TimeSeriesOperation
      {
          public string Name;
          public void Append(AppendOperation appendOperation)
          public void Delete(DeleteOperation deleteOperation)

          //..
      }
      {CODE-BLOCK/}

         | Property | Type | Description |
         |:-------------|:-------------|:-------------|
         | `Name` | `string` | Time Series name |
         | `Append` | `method` | Add an Append action to the list |
         | `Delete` | `method` | Add a Delete action to the list |


---

   * To add a time series entry Append action, call `TimeSeriesOperation.Append`.
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

   * To add a time series entry Delete action, call `TimeSeriesOperation.Delete`.
      {CODE-BLOCK:JSON}
      public void Delete(DeleteOperation deleteOperation)
      {CODE-BLOCK/}
      {CODE-BLOCK:JSON}
      public class DeleteOperation
      {
          public DateTime? From, To;

          //..
      }
      {CODE-BLOCK/}

         | Property | Type | Description |
         |:-------------|:-------------|:-------------|
         | `From` (optional) | `DateTime?` | Range start <br> Entries will be deleted starting at this timestamp |
         | `To` (optional) | `DateTime?` | Range end <br> Entries will be deleted up to this timestamp |

{PANEL/}


{PANEL: Usage Flow}

* Create an instance of `TimeSeriesOperation`  
   * Add it the **time series name**.  

* Prepare the Append and Delete sequence.  
   * Call `TimeSeriesOperation.Append` to add an Append action  
   * Call `TimeSeriesOperation.Delete` to add a Delete action  

        {INFO: }
         NOTE: Delete actions will be executed **before** Append actions.  
        {INFO/}

* Create a `TimeSeriesBatchOperation` instance.  
  Pass it the **document ID** and your **`TimeSeriesOperation` instance**  

* Call `store.Operations.Send` with your `TimeSeriesBatchOperation` 
  instance to execute the operation.  

{PANEL/}


{PANEL: Usage Samples}

* In this sample, we append two entries to a time series using `TimeSeriesBatchOperation`.  
   {CODE timeseries_region_Append-Using-TimeSeriesBatchOperation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

* In this sample, we delete two ranges of entries from a time series.  
   {CODE timeseries_region_Delete-Range-Using-TimeSeriesBatchOperation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

{PANEL/}


## Related articles

**Client API**  
[Time Series API Overview](../../../../document-extensions/timeseries/client-api/overview)  

**Studio Articles**  
[Studio Time Series Management](../../../../studio/database/document-extensions/time-series)  

**Querying and Indexing**  
[Time Series Querying](../../../../document-extensions/timeseries/querying/overview-and-syntax)  
[Time Series Indexing](../../../../document-extensions/timeseries/indexing)  

**Policies**  
[Time Series Rollup and Retention](../../../../document-extensions/timeseries/rollup-and-retention)  
