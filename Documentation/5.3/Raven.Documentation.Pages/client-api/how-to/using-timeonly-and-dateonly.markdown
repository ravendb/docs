# Client API: How to Use TimeOnly and DateOnly Types
---

{NOTE: }

* RavenDB has introduced the ability to convert `DateTime` or strings written in date/time formats to .NET's 
  `DateOnly` or `TimeOnly` types without slowing down queries and while leaving your existing data as is.  

* We do this in the [indexes](../../indexes/map-indexes) so that the conversions and calculations are done behind the scenes
  and the data is ready for fast queries. [See sample index below.](../../client-api/how-to/using-timeonly-and-dateonly#convert-and-use-date/timeonly-without-affecting-your-existing-data)).  

* It is also possible to covert via patch or query, but we recommend doing so in the index because:
  * Patch changes your existing data, which may cause problems with existing logic that uses that data.
  * Queries become slow if they are defined with demanding processes. 
  * [Static indexes](../../indexes/indexing-basics) process new data in the background, 
    including calculations and converstions to Ticks or to Date/TimeOnly values, 
    so that the data is ready at query time.

In this page: 

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

[Static indexes](../../indexes/map-indexes) can process calculations on existing data and enable you to rapidly query it without changing your data.  

{INFO: Ticks}
To be able to use ticks with DateOnly or TimeOnly, you must create a **static index** that computes the conversion.  

An auto-index will not convert DateOnly or TimeOnly into ticks, but will be able to convert data to strings.  
You can also compare strings, though comparing ticks is usually much faster.  
The index orders the strings according to the first digits, then the second, and so on.  

{INFO/}

The index must be declared as DateOnly or TimeOnly. 

{CODE DateAndTimeOnlyIndexSample@ClientApi/HowTo/UseTimeOnlyAndDateOnly.cs /}

Once the data has been converted inside the index, you can query on the converted data to find DateOnly or TimeOnly information.  

For example, the following query will find all of the entries that occured between 15:00 and 17:00.

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


