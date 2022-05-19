# Client API: How to Use TimeOnly and DateOnly Types
---

{NOTE: }

* To save storage space and streamline your process when you only need to know the date or the time, you can store and query 
  [DateOnly](https://devblogs.microsoft.com/dotnet/date-time-and-time-zone-enhancements-in-net-6/#the-dateonly-type) 
  and [TimeOnly](https://devblogs.microsoft.com/dotnet/date-time-and-time-zone-enhancements-in-net-6/#the-timeonly-type) types 
  instead of `DateTime`. (As of .NET version 6.0+ and RavenDB 5.3+)

* You can now convert `DateTime` or strings written in date/time formats to .NET's 
  `DateOnly` or `TimeOnly` types without slowing down queries and while leaving your existing data as is.  
   * Use `AsDateOnly` or `AsTimeOnly` in a static index ([see examples below](../../client-api/how-to/using-timeonly-and-dateonly#use--or--in-a-static-index-to-convert-strings-or-datetime))
   * `AsDateOnly` and `AsTimeOnly` automatically convert strings to ticks for faster querying.  

* We convert the types in [static indexes](../../indexes/map-indexes) so that the conversions and calculations are done behind the scenes
  and the data is ready for fast queries. ([See sample index below.](../../client-api/how-to/using-timeonly-and-dateonly#convert-and-use-date/timeonly-without-affecting-your-existing-data))  

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

In RavenDB, we offer conversion of types in static indexes with the methods [AsDateOnly or AsTimeOnly](../../client-api/how-to/using-timeonly-and-dateonly#use--or--in-a-static-index-to-convert-strings-or-datetime).

* [Static indexes](../../indexes/indexing-basics) process new data in the background, 
  including calculations and converstions to DateOnly/TimeOnly values, which can be used as ticks,  
  so that the data is ready at query time when you [query the index](../../indexes/querying/basics#example-iv---querying-a-specified-index).  
    * These indexes do all of the calculations on the entire dataset that you define the first time they run, and then they only need to 
      process changes in data. 

{INFO: Ticks}
If your data is in strings, to use ticks you must create a **static index** 
that computes the conversion from strings to [DateOnly or TimeOnly](../../client-api/how-to/using-timeonly-and-dateonly#convert-and-use-date/timeonly-without-affecting-your-existing-data).  

An auto-index will not convert DateOnly or TimeOnly into ticks, but will index data as strings.  
By defining a query that creates an auto-index which orders the strings you can also compare strings, 
though comparing ticks is much faster.  

RavenDB automatically converts strings into ticks via `AsDateOnly` or `AsTimeOnly`.  
{INFO/}

### Use `AsDateOnly` or `AsTimeOnly` in a static index to convert strings or DateTime

* [Converting Strings to DateOnly or TimeOnly](../../client-api/how-to/using-timeonly-and-dateonly#converting-strings-with-minimal-cost)
* [Converting DateTime to DateOnly or TimeOnly](../../client-api/how-to/using-timeonly-and-dateonly#converting--with-minimal-cost)

#### Converting Strings with minimal cost

The following generic sample is a map index where `AsDateOnly` converts the string `item.StringDateOnlyField` into `DateOnly`.  

When the converted data is available in the index, you can inexpensively [query the index](../../indexes/querying/basics#example-iv---querying-a-specified-index).

Strings are automatically converted to ticks for faster querying.  

{CODE IndexConvertsStringsWithAsDateOnlySample@ClientApi/HowTo/UseTimeOnlyAndDateOnly.cs /}

Using the static index above, here a string in date format "2022-05-12" is saved, the index above converts it to `DateOnly`, then 
the index is queried.  

{CODE AsDateOnlyStringToDateOnlyQuerySample@ClientApi/HowTo/UseTimeOnlyAndDateOnly.cs /}

---

#### Converting `DateTime` with minimal cost

The following generic sample is a map index that converts `DateTime` into `DateOnly` and saves the values in the index.

Once the converted data is available in the static index, you can inexpensively [query the index](../../indexes/querying/basics#example-iv---querying-a-specified-index).

{CODE IndexConvertsDateTimeWithAsDateOnlySample@ClientApi/HowTo/UseTimeOnlyAndDateOnly.cs /}


Using the index above, the following example saves `DateTime.Now`, the type is converted in the index, then 
the index is queried. 

{CODE AsDateOnlyStringToDateOnlyQuerySample@ClientApi/HowTo/UseTimeOnlyAndDateOnly.cs /}


{PANEL/}

{PANEL: Using already existing `DateOnly` or `TimeOnly` fields }

The index must have a field that declares the type as DateOnly or TimeOnly. 

{CODE DateAndTimeOnlyIndexSample@ClientApi/HowTo/UseTimeOnlyAndDateOnly.cs /}

For example, the following query will find all of the entries that occured between 15:00 and 17:00 
without considering the date.

{CODE TimeOnly_between15-17@ClientApi/HowTo/UseTimeOnlyAndDateOnly.cs /}

**Querying on Ticks**  
Strings are automatically converted to ticks with [`AsDateOnly` and `AsTimeOnly`](../../client-api/how-to/using-timeonly-and-dateonly#use--or--in-a-static-index-to-convert-strings-or-datetime).  

{PANEL/}


{PANEL: }

{PANEL/}


## Related Articles 

- [Creating and Deploying Indexes](../../indexes/creating-and-deploying)  
- [Indexing Basics](../../indexes/indexing-basics)
- [Map Indexes](../../indexes/map-indexes)


