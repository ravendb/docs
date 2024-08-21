# Append & Delete Time Series Operations
---

{NOTE: }

* Use `TimeSeriesBatchOperation` to Append and Delete multiple time series entries on a single document.  
  A list of predefined Append and Delete actions will be executed in this single batch operation.

* To Append and Delete multiple time series entries on multiple documents, see [PatchByQueryOperation](../../../../document-extensions/timeseries/client-api/operations/patch#patchbyqueryoperation).

* For a general _Operations_ overview, see [What are Operations](../../../../client-api/operations/what-are-operations).

* In this page:  
   * [Usage](../../../../document-extensions/timeseries/client-api/operations/append-and-delete#usage)
   * [Examples](../../../../document-extensions/timeseries/client-api/operations/append-and-delete#examples)
       * [Append multiple entries](../../../../document-extensions/timeseries/client-api/operations/append-and-delete#append-multiple-entries)
       * [Delete multiple entries](../../../../document-extensions/timeseries/client-api/operations/append-and-delete#delete-multiple-entries)
       * [Append & Delete entries in the same batch](../../../../document-extensions/timeseries/client-api/operations/append-and-delete#append--delete-entries-in-the-same-batch)
   * [Syntax](../../../../document-extensions/timeseries/client-api/operations/append-and-delete#syntax)

{NOTE/}

---

{PANEL: Usage}

**Flow**:

* Prepare the Append and Delete operations:
    * Create an instance of `TimeSeriesOperation.AppendOperation` to define an Append action.
    * Create an instance of ` TimeSeriesOperation.DeleteOperation` fo define a Delete action.
* Create an instance of `TimeSeriesOperation` and pass it the the time series name.
    * Call `TimeSeriesOperation.Append` to add the Append operation.
    * Call `TimeSeriesOperation.Delete` to add the Delete operation.
* Create a `TimeSeriesBatchOperation` instance and pass it:  
   * The document ID
   * The `TimeSeriesOperation` object
* Execute the `TimeSeriesBatchOperation` operation by calling `store.Operations.Send`

**Note**:

* All the added Append and Delete operations will be executed in a single-node transaction.
* Delete actions are executed **before** Append actions. As seen in [this example](../../../../document-extensions/timeseries/client-api/operations/append-and-delete#append--delete-entries-in-the-same-batch).
* Appending entries to a time series that doesn't yet exist yet will create the time series.
* An exception will be thrown if the specified document does Not exist.

{PANEL/}

{PANEL: Examples}

#### Append multiple entries:

In this example, we append four entries to a time series.

{CODE timeseries_region_Append-Using-TimeSeriesBatchOperation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

---

#### Delete multiple entries:

In this example, we delete a range of two entries from a time series.  

{CODE timeseries_region_Delete-Range-Using-TimeSeriesBatchOperation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

---

#### Append & Delete entries in the same batch:

* In this example, we append and delete entries in the same batch operation.

* Note: the Delete actions are executed **before** all Append actions.

{CODE timeseries_region-Append-and-Delete-TimeSeriesBatchOperation@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

{PANEL/}

{PANEL: Syntax}

#### `TimeSeriesBatchOperation`

{CODE TimeSeriesBatchOperation-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

| Parameter      | Type                  | Description                                                                           |
|----------------|-----------------------|---------------------------------------------------------------------------------------|
| **documentId** | `string`              | The ID of the document to which you want to Append/Delete time series data.           |
| **operation**  | `TimeSeriesOperation` | This class defines which Append/Delete actions to perform within the batch operation. |

#### `TimeSeriesOperation`  

{CODE-BLOCK:csharp}
public class TimeSeriesOperation
{
      public string Name;
      public void Append(AppendOperation appendOperation)
      public void Delete(DeleteOperation deleteOperation)
}
{CODE-BLOCK/}

| Property   | Type     | Description                          |
|------------|----------|--------------------------------------|
| **Name**   | `string` | The time series name                 |
| **Append** | `method` | Add an `AppendOperation` to the list |
| **Delete** | `method` | Add a `DeleteOperation` to the list  |

#### `AppendOperation`
    
{CODE-BLOCK:csharp}
public class AppendOperation
{
      public DateTime Timestamp;
      public double[] Values;
      public string Tag;
}
{CODE-BLOCK/}

| Property      | Type       | Description                                              |
|---------------|------------|----------------------------------------------------------|
| **Timestamp** | `DateTime` | The time series entry will be appended at this timestamp |
| **Values**    | `double[]` | Entry values                                             |
| **Tag**       | `string`   | Entry tag (optional)                                     |

#### `DeleteOperation`
 
{CODE-BLOCK:csharp}
public class DeleteOperation
{
     public DateTime? From, To;
}
{CODE-BLOCK/}

| Property   | Type        | Description                                                                                    |
|------------|-------------|------------------------------------------------------------------------------------------------|
| **From**   | `DateTime?` | Entries will be deleted starting at this timestamp (inclusive)<br>Default: `DateTime.MinValue` |
| **To**     | `DateTime?` | Entries will be deleted up to this timestamp (inclusive)<br>Default: `DateTime.MaxValue`       |

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
