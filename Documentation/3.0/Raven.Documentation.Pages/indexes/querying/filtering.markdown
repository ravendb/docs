# Filtering

One of the most basic functionalities when it comes to querying is the ability to filter out data and only return records that match given condition. There are couple of way to to this, and they all depend on type of query you want to use ([Query](), [DocumentQuery](), low-level [Command]()). Following example demonstrates how to add a simple conditions to query using all those methods.

## Where

### Example I

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_0_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_0_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Commands filtering_0_3@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_0_4@Indexes\Querying\Filtering.cs /}
{CODE-TABS/}

### Example II - numeric property

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_1_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_1_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Commands filtering_1_3@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_1_4@Indexes\Querying\Filtering.cs /}
{CODE-TABS/}

### Example III - nested property

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_2_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_2_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Commands filtering_2_3@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_2_4@Indexes\Querying\Filtering.cs /}
{CODE-TABS/}

### Where + Any

`Any` is useful when you have a collection of items (e.g. `Order` contains `OrderLines`) and you want to filter out based on a values from this collection. For example, let's retrieve all orders that contain a `OrderLine` with a given product.

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_3_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_3_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Commands filtering_3_3@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_3_4@Indexes\Querying\Filtering.cs /}
{CODE-TABS/}

### Where + In

When you want to check single value against multiple values `In` operator can become handy. E.g. to retrieve all employees that `FirstName` is either `Robert` or `Nancy` we can issue following query:

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_4_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_4_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Commands filtering_4_3@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_0_4@Indexes\Querying\Filtering.cs /}
{CODE-TABS/}

{WARNING:Important}
Remember to add `Raven.Client.Linq` namespace to usings if you want to use `In` extension method.
{WARNING/}

## Remarks

{INFO Underneath, `Query` and `DocumentQuery` are converting predicates to `IndexQuery` class so they can issue a query from **low-level command method**. /}

{SAFE By default **page size is set to 128** if not specified, so above queries will not return more than 128 results. You can read more about paging [here](). /}

## Related articles

TODO