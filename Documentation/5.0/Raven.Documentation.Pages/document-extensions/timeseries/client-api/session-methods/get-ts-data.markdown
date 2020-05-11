# Get Time-Series Data 

---

{NOTE: }

* Get a **single time-series entry** or a **range of entries** 
  using the session method `TimeSeriesFor.Get`.  
* Get the **names of a document's time-series** using the 
  session method `Advanced.GetTimeSeriesFor`.  

{INFO: }
`session.TimeSeriesFor.Get` retrieves a single time-series' data.  
To retrieve multiple time-series' data in a single call, use the 
[GetTimeSeriesOperaion](../../../../document-extensions/timeseries/client-api/store-operations/get-TS-data) 
document-store operation.  
{INFO/}

* In this page:  
   * [`TimeSeriesFor.Get`: Get Time-Series Data](../../../../document-extensions/timeseries/client-api/session-methods/get-ts-data#timeseriesfor.get:-get-time-series-data)  
      * [Definition](../../../../document-extensions/timeseries/client-api/session-methods/get-ts-data#definition)  
      * [Usage Flow](../../../../document-extensions/timeseries/client-api/session-methods/get-ts-data#usage-flow)  
      * [Usage Samples](../../../../document-extensions/timeseries/client-api/session-methods/get-ts-data#usage-samples)  
   * [`Advanced.GetTimeSeriesFor`: Get Time-Series Names](../../../../document-extensions/timeseries/client-api/session-methods/get-ts-data#advanced.gettimeseriesfor:-get-time-series-names)  
      * [Definition](../../../../document-extensions/timeseries/client-api/session-methods/get-ts-data#definition-1)  
      * [Usage Flow](../../../../document-extensions/timeseries/client-api/session-methods/get-ts-data#usage-flow-1)  
      * [Usage Samples](../../../../document-extensions/timeseries/client-api/session-methods/get-ts-data#usage-sample)  

{NOTE/}

---

{PANEL: `TimeSeriesFor.Get`: Get Time-Series Data}

---

#### `TimeSeriesFor.Get` Definition

{CODE TimeSeriesFor-Get-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* Parameters:  

     | Parameters | Type | Description |
     |:-------------|:-------------|:-------------|
     | `from` | DateTime | Range Start |
     | `to` | DateTime | Range End |
     | `start` | int | **Paging** first entry <br> e.g. 50 means the first page starts at the 50th time-series entry. <br> **Default Value**: 0, for the first time-series entry. |
     | `pagesize` | int | **Paging** page-size <br> e.g. 10 means the first page includes entries 50 to 59. <br> **Default Value**: int.MaxValue, for all time-series entries. |

* Return Value: `IEnumerable<TimeSeriesEntry>`  
  `TimeSeriesFor.Get` returns the time-series' data in an array of `IEnumerable`s.  
  Each IEnumerable contains a single time-series entry's data: its **timestamp**, 
  **tag** (if there is one), and **values**.  
    {CODE-BLOCK: JSON}
{
  public DateTime Timestamp { get; set; }
  public string Tag { get; set; }
  public double[] Values { get; set; }
  public double Value => Values[0];
}
    {CODE-BLOCK/}

---

#### `TimeSeriesFor.Get` Usage Flow  

* Open a session.  
* Create an instance of `TimeSeriesFor`.  
    * Either pass `TimeSeriesFor` an explicit document ID, -or-  
    * Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
* Call `TimeSeriesFor.Get`.  

---

#### `TimeSeriesFor.Get` Usage Samples  

Here, we retrieve all time-series entries of a time-series.  
{CODE timeseries_region_TimeSeriesFor-Get-Single-Value-Using-Document-ID@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

And here we query our collection for a document with "Name: John" as a property 
and get its "Heartrate" time-series data.  
{CODE timeseries_region_Pass-TimeSeriesFor-Get-Query-Results@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

[add a sample that uses paging here]

{PANEL/}

{PANEL: `Advanced.GetTimeSeriesFor`: Get Time-Series Names}

Use this method to retrieve the names of a document's time-series.  

#### `Advanced.GetTimeSeriesFor` Definition

#### `Advanced.GetTimeSeriesFor` Usage Flow

#### `Advanced.GetTimeSeriesFor` Usage Sample

{CODE timeseries_region_Retrieve-TimeSeries-Names@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}  

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
