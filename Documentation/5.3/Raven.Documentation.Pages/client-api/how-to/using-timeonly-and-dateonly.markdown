# Client API: How to Use TimeOnly and DateOnly Types
---

{NOTE: }

* As long as you are running .NET version 6.0+ and RavenDB 5.3+, to save storage space and streamline you can store and query [DateOnly](https://devblogs.microsoft.com/dotnet/date-time-and-time-zone-enhancements-in-net-6/#the-dateonly-type) 
  and [TimeOnly](https://devblogs.microsoft.com/dotnet/date-time-and-time-zone-enhancements-in-net-6/#the-timeonly-type) types 
  instead of `DateTime` when you only need to know the date or the time.  

* RavenDB has introduced the ability to convert `DateTime` or strings written in date/time formats to .NET's 
  `DateOnly` or `TimeOnly` types without slowing down queries and while leaving your existing data as is.  
   * Use `AsDateOnly` or `AsTimeOnly` in a static index (SEE EXAMPLES BELOW)
   * `AsDateOnly` and `AsTimeOnly` automatically convert strings to ticks for faster querying.  

* We convert the types in [static indexes](../../indexes/map-indexes) so that the conversions and calculations are done behind the scenes
  and the data is ready for fast queries. [See sample index below.](../../client-api/how-to/using-timeonly-and-dateonly#convert-and-use-date/timeonly-without-affecting-your-existing-data)).  

* It is also possible to covert via patch or query, but it is [far more efficient](../../client-api/how-to/using-timeonly-and-dateonly#convert-and-use-date/timeonly-without-affecting-your-existing-data) 
  to do so in a static index. 

* In this page: 
   * [About DateOnly and TimeOnly](../../client-api/how-to/using-timeonly-and-dateonly#about-dateonly-and-timeonly) 
   * [Convert and Use Date/TimeOnly Without Affecting Your Existing Data](../../client-api/how-to/using-timeonly-and-dateonly#convert-and-use-date/timeonly-without-affecting-your-existing-data) 

{NOTE/}

{PANEL: About DateOnly and TimeOnly}

These two new C# types are available from .NET 6.0+ (RavenDB 5.3+).  

* **DateOnly**  
  According to [Microsoft .NET Blog](https://devblogs.microsoft.com/dotnet/date-time-and-time-zone-enhancements-in-net-6/#the-dateonly-type)
  DateOnly is ideal for scenarios such as birth dates, anniversaries, hire dates, 
  and other business dates that are not typically associated with any particular time. 
  * See [their usage examples here.](https://devblogs.microsoft.com/dotnet/date-time-and-time-zone-enhancements-in-net-6/#the-dateonly-type)

* **TimeOnly**  
  According to [Microsoft .NET Blog](https://devblogs.microsoft.com/dotnet/date-time-and-time-zone-enhancements-in-net-6/#the-timeonly-type)
  TimeOnly is ideal for scenarios such as recurring meeting times, daily alarm clock times, 
  or the times that a business opens and closes each day of the week.
  * See [their usage examples here.](https://devblogs.microsoft.com/dotnet/date-time-and-time-zone-enhancements-in-net-6/#the-timeonly-type)

{PANEL/}

{PANEL: Convert and Use Date/TimeOnly Without Affecting Your Existing Data}

It is possible to covert via patch or query, but it is far more efficient to do so in a [static index](../../indexes/map-indexes). 

   * Patch changes your existing data, which may cause problems with existing logic that uses that data.
   * Queries become slow if they are defined with demanding processes.  
     Also, queries will need to process all of your data every time. 
   * [Static indexes](../../indexes/indexing-basics) process new data in the background, 
    including calculations and converstions to DateOnly/TimeOnly values, which can be used as ticks,  
    so that the data is ready at query time when you [query the index](../../indexes/querying/basics#example-iv---querying-a-specified-index).  
       * These indexes do all of the calculations on the entire dataset defined the first time they run, and then they only need to 
         process changes in data, which is far less expensive than running the calculations and conversions on the entire dataset 
         each time with a query.

{INFO: Ticks}
To be able to use ticks as DateOnly or TimeOnly, you must create a **static index** that computes the conversion from strings.  

An auto-index will not convert DateOnly or TimeOnly into ticks, but will process strings as strings.  
By defining the query that creates an auto-index that orders the strings, you can also compare strings, 
though comparing ticks is usually much faster.  
Strings can be ordered according to the first digits, then the second, and so on (Radix Sorting).  

RavenDB automatically makes data converted via `AsDateOnly` or `AsTimeOnly` available as ticks.  
{INFO/}

### Use `AsDateOnly` or `AsTimeOnly` in a static index to convert strings or DateTime types  

#### Converting Strings with minimal cost

The following generic sample is a map index where `AsDateOnly` converts the string `item.StringDateOnlyField` into `DateTime`.  

The conversion automatically makes the DateTime type available as ticks.

When the converted data is available in the index, you can inexpensively [query the index](../../indexes/querying/basics#example-iv---querying-a-specified-index).

{CODE-BLOCK:csharp}
public class StringAsDateOnlyConversion : AbstractIndexCreationTask<StringItem, DateOnlyItem>
{
    public StringAsDateOnlyConversion()
    {
        Map = items => from item in items
            select new DateOnlyItem {DateOnlyField = AsDateOnly(item.StringDateOnlyField)};
    }
}

public class StringItem
{
    public string StringDateOnlyField { get; set; }
}
{CODE-BLOCK/}

Using the static index above, here a string "2022-05-12" is saved, the index converts it to `DateOnly`, then 
the index is queried.  

{CODE-BLOCK:csharp}
using (var session = store.OpenSession())
{
    session.Store(new StringItem()
    {
        StringDateOnlyField = "2022-05-12"
    });
    session.SaveChanges();
}
new StringAsDateOnlyConversion().Execute(store);
Indexes.WaitForIndexing(store);
        
using (var session = store.OpenSession())
{
    var today = new DateOnly(2022, 5, 12);
    var element = session.Query<DateOnlyItem, StringAsDateOnlyConversion>().Where(item => item.DateOnlyField == today).As<StringItem>().Single();
}
{CODE-BLOCK/}

---

#### Converting `DateTime` with minimal cost

The following generic sample is a map index that converts `DateTime` into `DateOnly` in the index instead of doing expensive 
conversions repetetively in queries.  

When the converted data is available in the index, you can inexpensively [query the index](../../indexes/querying/basics#example-iv---querying-a-specified-index).

{CODE-BLOCK:csharp}
public class DateTimeAsDateOnlyConversion : AbstractIndexCreationTask<DateTimeItem, DateOnlyItem>
{
    public DateTimeAsDateOnlyConversion()
    {
        Map = items => from item in items
            select new DateOnlyItem {DateOnlyField = AsDateOnly(item.DateTimeField)};
    }
}

public class DateTimeItem
{
    public DateTime? DateTimeField { get; set; }
}
{CODE-BLOCK/}

Using the index above, the following example saves `DateTime.Now`, the type is converted in the index, then 
the index is queried. 

{CODE-BLOCK:csharp}
using (var session = store.OpenSession())
{
    session.Store(new DateTimeItem()
    {
        DateTimeField = DateTime.Now
    });
    session.SaveChanges();
}
new DateTimeAsDateOnlyConversion().Execute(store);
Indexes.WaitForIndexing(store);
        
using (var session = store.OpenSession())
{
    var today = DateOnly.FromDateTime(DateTime.Now);
    var element = session.Query<DateOnlyItem, DateTimeAsDateOnlyConversion>().Where(item => item.DateOnlyField == today).As<DateTimeItem>().Single();
}
{CODE-BLOCK/}

{PANEL/}

{ PANEL: Using already existing `DateOnly` or `TimeOnly` fields }

The index must have a field that declares the type as DateOnly or TimeOnly. 

{CODE DateAndTimeOnlyIndexSample@ClientApi/HowTo/UseTimeOnlyAndDateOnly.cs /}

Now you can query on the converted data to find DateOnly or TimeOnly information.  

For example, the following query will find all of the entries that occured between 15:00 and 17:00 
without considering the date.

{CODE TimeOnly_between15-17@ClientApi/HowTo/UseTimeOnlyAndDateOnly.cs /}

**Querying on Ticks**  
Because the query is on a static index, you can rapidly query on ticks if you choose.

{PANEL/}


{PANEL: }

{PANEL/}


## Related Articles 

- [Creating and Deploying Indexes](../../indexes/creating-and-deploying)  
- [Indexing Basics](../../indexes/indexing-basics)
- [Map Indexes](../../indexes/map-indexes)


