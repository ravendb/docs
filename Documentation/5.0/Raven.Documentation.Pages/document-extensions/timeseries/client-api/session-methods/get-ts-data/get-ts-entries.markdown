# Get Time-Series Entries 

---

{NOTE: }

Get a range of time-series entries using `TimeSeriesFor.Get`.  
Results can be sliced to pages and retrieved gradually.  

* In this page:  
   * [`TimeSeriesFor.Get`](../../../../../document-extensions/timeseries/client-api/session-methods/get-ts-data/get-ts-entries#timeseriesfor.get)  
      * [Syntax](../../../../../document-extensions/timeseries/client-api/session-methods/get-ts-data/get-ts-entries#syntax)  
      * [Usage Flow](../../../../../document-extensions/timeseries/client-api/session-methods/get-ts-data/get-ts-entries#usage-flow)  
      * [Usage Samples](../../../../../document-extensions/timeseries/client-api/session-methods/get-ts-data/get-ts-entries#usage-samples)  

{NOTE/}

---

{PANEL: `TimeSeriesFor.Get`}

Use `TimeSeriesFor.Get` to get a range of a document's time-series entries.  

* You can retrieve time-series data **for a single document**.  
   * You can, however, call `TimeSeriesFor.Get` as many times as you 
     need to before executing saveChanges(), to retrieve multiple 
     documents' time-series data in a single transaction.  
   * To retrieve multiple documents' time-series' data in a single 
     call, use the [GetTimeSeriesOperaion](../../../../document-extensions/timeseries/client-api/store-operations/get-TS-data) 
     document-store operation instead.  
* You can slice retrieved data to **pages** to get time-series entries 
  gradually, one custom-size page at a time.  

{PANEL/}

{PANEL: Syntax}

* **Definition**  
  {CODE TimeSeriesFor-Get-definition@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* **Parameters**  

    | Parameters | Type | Description |
    |:-------------|:-------------|:-------------|
    | `from` | DateTime | Range Start |
    | `to` | DateTime | Range End |
    | `start` | int | **Paging** first entry <br> e.g. set `start` to 50 for the first page to start at the 50th time-series entry. <br> **Default Value**: 0, for the first time-series entry. |
    | `pagesize` | int | **Paging** page-size <br> e.g. set `pagesize` to 10 to retrieve 10-entries pages. <br> **Default Value**: int.MaxValue, for all time-series entries. |

* **Return Value**  

     **`IEnumerable<TimeSeriesEntry>`**  
     Time-series entries are returned in an array of TimeSeriesEntry IEnumerables.  
     {CODE-BLOCK: JSON}
{
  public DateTime Timestamp { get; set; }
  public string Tag { get; set; }
  public double[] Values { get; set; }
  public double Value => Values[0];
}
    {CODE-BLOCK/}

* **Exceptions**  
  Exceptions are not generated.  

{PANEL/}

{PANEL: Usage Flow}

* Open a session.  
* Create an instance of `TimeSeriesFor`.  
    * Either pass `TimeSeriesFor` an explicit document ID, -or-  
    * Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
* Call `TimeSeriesFor.Get`.  

{PANEL/}

{PANEL: Usage Samples}

* In this sample we retrieve all entries of a time-series.  
   {CODE timeseries_region_TimeSeriesFor-Get-Single-Value-Using-Document-ID@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

* In this sample we query for a document and get its "Heartrate" time-series data.  
{CODE timeseries_region_Pass-TimeSeriesFor-Get-Query-Results@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

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
