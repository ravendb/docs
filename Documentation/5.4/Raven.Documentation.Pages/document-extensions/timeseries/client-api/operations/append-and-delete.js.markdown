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
       * [Append & delete entries in the same batch](../../../../document-extensions/timeseries/client-api/operations/append-and-delete#append--delete-entries-in-the-same-batch)
   * [Syntax](../../../../document-extensions/timeseries/client-api/operations/append-and-delete#syntax)

{NOTE/}

---

{PANEL: Usage}

**Flow**:

* Prepare the Append and Delete operations:
    * Create an instance of `TimeSeriesOperation.AppendOperation` to define an Append action.
    * Create an instance of ` TimeSeriesOperation.DeleteOperation` fo define a Delete action.
* Create an instance of `TimeSeriesOperation` and pass it the the time series name.
    * Call `TimeSeriesOperation.append` to add the Append operation.
    * Call `TimeSeriesOperation.delete` to add the Delete operation.
* Create a `TimeSeriesBatchOperation` instance and pass it:  
   * The document ID
   * The `TimeSeriesOperation` object
* Execute the `TimeSeriesBatchOperation` operation by calling `store.operations.send`

**Note**:

   * All the added Append and Delete operations will be executed in a single-node transaction.
   * Delete actions are executed **before** Append actions. As seen in [this example](../../../../document-extensions/timeseries/client-api/operations/append-and-delete#append--delete-entries-in-the-same-batch).
   * Appending entries to a time series that doesn't yet exist yet will create the time series.
   * An exception will be thrown if the specified document does Not exist.

{PANEL/}

{PANEL: Examples}

#### Append multiple entries:

In this example, we append four entries to a time series.

{CODE:nodejs operation_1@documentExtensions\timeSeries\client-api\appendAndDeleteOperations.js /}

---

#### Delete multiple entries:

In this example, we delete a range of two entries from a time series.  

{CODE:nodejs operation_2@documentExtensions\timeSeries\client-api\appendAndDeleteOperations.js /}

---

#### Append & delete entries in the same batch:

* In this example, we append and delete entries in the same batch operation.

* Note: the Delete actions are executed **before** all Append actions.

{CODE:nodejs operation_3@documentExtensions\timeSeries\client-api\appendAndDeleteOperations.js /}

{PANEL/}

{PANEL: Syntax}

#### `TimeSeriesBatchOperation`

{CODE:nodejs syntax_1@documentExtensions\timeSeries\client-api\appendAndDeleteOperations.js /}

| Parameter      | Type                  | Description                                                                           |
|----------------|-----------------------|---------------------------------------------------------------------------------------|
| **documentId** | `string`              | The ID of the document to which you want to Append/Delete time series data.           |
| **operation**  | `TimeSeriesOperation` | This class defines which Append/Delete actions to perform within the batch operation. |

#### `TimeSeriesOperation`  

{CODE:nodejs syntax_2@documentExtensions\timeSeries\client-api\appendAndDeleteOperations.js /}

| Property   | Type     | Description                                                          |
|------------|----------|----------------------------------------------------------------------|
| **name**   | `string` | The time series name                                                 |
| **append** | `method` | Pass a `AppendOperation` object to this method to add it to the list |
| **delete** | `method` | Pass a `DeleteOperation` object to this method to add it to the list |

#### `AppendOperation`

{CODE:nodejs syntax_3@documentExtensions\timeSeries\client-api\appendAndDeleteOperations.js /}

| Property      | Type       | Description                                              |
|---------------|------------|----------------------------------------------------------|
| **timestamp** | `Date`     | The time series entry will be appended at this timestamp |
| **values**    | `number[]` | Entry values                                             |
| **tag**       | `string`   | Entry tag (optional)                                     |

#### `DeleteOperation`

{CODE:nodejs syntax_4@documentExtensions\timeSeries\client-api\appendAndDeleteOperations.js /}

| Property  | Type   | Description                                                                                       |
|-----------|--------|---------------------------------------------------------------------------------------------------|
| **from**  | `Date` | Entries will be deleted starting at this timestamp (inclusive)<br>Default: the minimum date value |
| **to**    | `Date` | Entries will be deleted up to this timestamp (inclusive)<br>Default: the maximum date value       |

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
