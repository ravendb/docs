## Time-Series Queries:
# Filtering

---

{NOTE: }

* Time-series entries can be filtered by their **value** (e.g. to 
  retrieve a "Thermometer" time-series entries whose measurement exceed 
  32 Celsius degrees) or **tag** (e.g. to retrieve all the entries whose 
  tag is "Thermometer No. 3").  

* Entries can also be filtered by the **contents of a document they refer to**.  
  A time-series entry's tag can contain the **ID of a document**. 
  A query can **load the document** that the entry refers to, check 
  its properties and filter time-series entries by them.  

* In this page:  
  * [Filtering Results](../../../document-extensions/timeseries/querying/filtering#filtering)  
     * [Using Tags as References](../../../document-extensions/timeseries/querying/filtering#using-tags-as-references---)  
  * [Client Usage Samples](../../../document-extensions/timeseries/querying/filtering#client-usage-samples)  

{NOTE/}

---

{PANEL: Filtering}

In an RQL query, use the `where` keyword to filter time-series entries 
by their **tags** or **values**.  

{CODE-BLOCK: JSON}
from Users as u where Age < 30
select timeseries(
    from HeartRate 
       between '2020-05-27T00:00:00.0000000Z' 
            and '2020-06-23T00:00:00.0000000Z'
       where Tag='watches/fitbit'
)
{CODE-BLOCK/}
  
* `where Tag='watches/fitbit'`  
  In this example we retrieve time-series entries whose tag is 'watches/fitbit'.  
  To filter entries by their by value, use **Value**. E.g.:  
  `where Value < 80`  

---

#### Using Tags as References - `load tag`

Use the `load Tag ` expression to **load a document** whose ID is stored in 
a time-series entry's tag.  
Use `load Tag ` with `where` to **filter your results by properties of the 
loaded document**, as we do in the following example.  

{CODE-BLOCK: JSON}
from Companies as c where c.Address.Country = "USA"
select timeseries(
    from StockPrice
       load Tag as emp
       where emp.Title == "Sales Representative"
)   
{CODE-BLOCK/}

* `load Tag as emp`  
   We know in advance that the tag of each StockPrice entry contains 
   the ID of an Employee document.  
   Here, we use `load tag` to load the employee profile referred to 
   by each tag.  
* `where emp.Title == "Sales Representative"`  
   Here, we filter time-series entries by the **Title** property 
   of the employee documents the entries refer to.  
   If the sales representatives the entries refer to are stock brokers 
   for example, we can now [group](), for example, the successful ones 
   whose services we'd like to hire.  

{PANEL/}

{PANEL: Client Usage Samples}

{INFO: }
You can run queries from your client using raw RQL and LINQ.  

* Learn how to run a raw RQL time-series query [here](../../../document-extensions/timeseries/client-api/session-methods/query-time-series/raw-rql-queries).  
* Learn how to run a LINQ time-series query [here](../../../document-extensions/timeseries/client-api/session-methods/query-time-series/linq-queries).  

{INFO/}

To filter results, use `where` in a raw RQL query or `Where()` in a LINQ query.  
To filter results by a tag reference to a document, 
use `load tag` in a raw RQL query or `LoadTag()` in a LINQ query.  

* In this sample, we send the query we 
  [presented above](../../../document-extensions/timeseries/querying/filtering#using-tags-as-references---) 
  to the server in raw RQL and in LINQ format.

    {CODE-TABS}
    {CODE-TAB:csharp:Raw-RQL ts_region_Filter-By-load-Tag-Raw-RQL@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
    {CODE-TAB:csharp:LINQ ts_region_Filter-By-LoadTag-LINQ@DocumentExtensions\TimeSeries\TimeSeriesTests.cs /}
    {CODE-TABS/}

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
