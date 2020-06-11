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
  In your query, you can **load the document** that the entry refers to.  
  and **filter your results by its contents**.  

* In this page:  
  * [Filtering Results](../../../)  
     * [Using Tags as References](../../../)  
  * [Client Usage Samples](../../../)  

{NOTE/}

---

{PANEL: Filtering}

Use `where` to filter time-series entries by their **tags** or **values**.  

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
    where emp.Title == "Vice President, Sales"
)   
{CODE-BLOCK/}

* `from Companies as c where c.Address.Country = "USA"`  
   Here, we choose companies that reside in the USA.  
* `from StockPrice`  
   Here we choose these companies' **StockPrice** time-series.  
* `load Tag as emp`  
   We know in advance, that the tag of each time-series entry contains 
   the ID of an Employee document.  
   Here, we use the `load tag` expression to load the employee profile 
   referred to by each tag.  
* `where emp.Title == "Vice President, Sales"`  
   And finally, we filter time-series entries by the **Title** property 
   of employee documents they refer to.  

The result-set of this sample includes time-series entries with 
references (IDs stored in their tags) to a Sales Vice-President's profile.  

{PANEL/}

{PANEL: Client Usage Samples}

{INFO: }
You can run queries from your client using raw RQL and LINQ.  
* Learn how to run a raw RQL time-series query [here](../../../document-extensions/timeseries/client-api/session-methods/query-time-series/raw-rql-queries).  
* Learn how to run a LINQ time-series query [here](../../../document-extensions/timeseries/client-api/session-methods/query-time-series/linq-queries).  
{INFO/}

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
