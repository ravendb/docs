# Filtering

One of the most basic functionalities when it comes to querying is the ability to filter out data and only return records that match given condition. There are couple of ways to do this, and they all depend on querying approach you want to use ([Query](../../client-api/session/querying/how-to-query) from basic session operations, [DocumentQuery](../../client-api/session/querying/lucene/how-to-use-lucene-in-queries) from advanced session operations or directly using [RQL]()).  

Following examples demonstrates how to add a simple conditions to query using all of those methods.

## Where

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_0_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_0_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_0_4@Indexes\Querying\Filtering.cs 
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Employees/ByFirstAndLastName'
where FirstName = 'Robert' and LastName = 'King'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where - numeric property

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_1_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_1_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_1_4@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Products/ByUnitsInStock'
where UnitsInStock > 50
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where - nested property

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_2_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_2_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_2_4@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Order/ByOrderLinesCount'
where Lines.Count > 50
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where + Any

`Any` is useful when you have a collection of items (e.g. `Order` contains `OrderLines`) and you want to filter out based on a values from this collection. For example, let's retrieve all orders that contain a `OrderLine` with a given product.

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_3_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_3_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_3_4@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Order/ByOrderLinesCount'
where Lines_ProductName = 'Teatime Chocolate Biscuits'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where + In

When you want to check single value against multiple values `In` operator can become handy. E.g. to retrieve all employees that `FirstName` is either `Robert` or `Nancy` we can issue following query:

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_4_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_4_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_0_4@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Employees/ByFirstAndLastName'
where FirstName IN ('Robert', 'Nancy')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{WARNING:Important}
Remember to add `Raven.Client.Documents.Linq` namespace to usings if you want to use `In` extension method.
{WARNING/}

## Where + ContainsAny

To check if enumeration contains **any** of the values from a specified collection you can use `ContainsAny` method.

Let's assume that we want to return all `BlogPosts` that contain any of the specified `Tags`.

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_5_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_5_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_5_4@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'BlogPosts/ByTags'
where Tags IN ('Development', 'Research')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{WARNING:Important}
Remember to add `Raven.Client.Documents.Linq` namespace to usings if you want to use `ContainsAny` extension method.
{WARNING/}

## Where + ContainsAll

To check if enumeration contains **all** of the values from a specified collection you can use `ContainsAll` method.

Let's assume that we want to return all `BlogPosts` that contain all of the specified `Tags`.

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_6_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_6_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_5_4@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'BlogPosts/ByTags'
where Tags ALL IN ('Development', 'Research')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{WARNING:Important}
Remember to add `Raven.Client.Documents.Linq` namespace to usings if you want to use `ContainsAll` extension method.
{WARNING/}

## Remarks

{INFO Underneath, `Query` and `DocumentQuery` are converting predicates to `IndexQuery` class so they can issue a query from **low-level operation method**. /}

## Related articles

- [Indexing : Basics](../../indexes/indexing-basics)
- [Querying : Basics](../../indexes/querying/basics)
- [Querying : Paging](../../indexes/querying/paging)
- [Querying : Sorting](../../indexes/querying/sorting)
