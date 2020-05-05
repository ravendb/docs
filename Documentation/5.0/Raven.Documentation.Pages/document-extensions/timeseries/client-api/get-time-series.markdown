# Get Time-Series  

---

{NOTE: }

* You can use the session `TimeSeriesFor.Get` method to retrieve 
  a time-series' data.  
  Data retrieved using `TimeSeriesFor.Get` is **cached in clients' local storage**.  

* You can also use the store `GetTimeSeriesOperaion` operation to retrieve 
  time-series data.  
  With `GetTimeSeriesOperaion` you can retrieve the data of **multiple time-series**.  

* In this page:  
   * [Get Time-Series Entry Using `session.TimeSeriesFor.Get`](../../../document-extensions/timeseries/client-api/get-time-series#get-time-series-entry-using-session.timeseriesfor.get)  
   * [Get Time-Series Entry Using `GetTimeSeriesOperaion`](../../../document-extensions/timeseries/client-api/get-time-series#get-time-series-entries-using-gettimeseriesoperaion)  


{NOTE/}

---

{PANEL: Get Time-Series Entry Using `session.TimeSeriesFor.Get`}

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

#### Usage Flow  

* Open a session.  
* Create an instance of `TimeSeriesFor`.  
    * Either pass `TimeSeriesFor` an explicit document ID, -or-  
    * Pass it an [entity tracked by the session](../../../client-api/session/loading-entities), e.g. a document object returned from [session.query](../../../client-api/session/querying/how-to-query) or from [session.Load](../../../client-api/session/loading-entities#load).  
* Call `TimeSeriesFor.Get`.  

---

#### Usage Samples  

Here, we retrieve all time-series entries of a time-series.  
{CODE timeseries_region_TimeSeriesFor-Get-Single-Value-Using-Document-ID@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

And here we query our collection for a document with "Name: John" as a property 
and get its "Heartrate" time-series data.  
{CODE timeseries_region_Pass-TimeSeriesFor-Get-Query-Results@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}

[add a sample that uses paging here]

{PANEL/}

{PANEL: Get Time-Series Entries Using `GetTimeSeriesOperaion`}

Get time-series entries using the `GetTimeSeriesOperaion` operation.  
It has an advantage over `session.Get`, in that it allows you to retrieve 
data from multiple time-series of a selected document in a single call.  

Learn how to use `GetTimeSeriesOperaion` [in the article dedicated to 
time-series operations](../../../document-extensions/timeseries/client-api/time-series-operations#gettimeseriesoperation:-get-time-series-data).  


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
