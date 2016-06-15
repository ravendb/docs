# Filtering

One of the most basic functionalities when it comes to querying is the ability to filter out data and only return records that match given condition. There are couple of ways to do this, and they all depend on querying approach you want to use ([Query](../../client-api/session/querying/how-to-query) from basic session operations, [DocumentQuery](../../client-api/session/querying/lucene/how-to-use-lucene-in-queries) from advanced session operations or low-level [Command](../../client-api/commands/querying/how-to-query-a-database)). Following example demonstrates how to add a simple conditions to query using all those methods.

## Where

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_0_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_0_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Commands filtering_0_3@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_0_4@Indexes\Querying\Filtering.cs /}
{CODE-TABS/}

## Where - numeric property

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_1_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_1_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Commands filtering_1_3@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_1_4@Indexes\Querying\Filtering.cs /}
{CODE-TABS/}



{INFO: The importance of types in queries}

Let's consider the following index and queries:

{CODE-TABS}
{CODE-TAB:csharp:Index filtering_7_4@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Query filtering_7_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_7_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Commands filtering_7_3@Indexes\Querying\Filtering.cs /}
{CODE-TABS/}

Note that `PricePerUnit` is of type `decimal` in `Order` entity, so indexed `TotalPrice` value  will be `decimal` too. In oder to
properly query such index, we need to preserve types in queries. That is why `Orders_ByTotalPrice.Result.TotalPrice` is `decimal` and 
`IndexQuery.Query` has `Dx` prefix specified before the actual value. Types of properties in predicates have to match types of indexed fields.

* More about the need to use `OfType` here, you can find in [projections article](../../client-api/session/querying/how-to-perform-projection#oftype-(as)---simple-projection).

{INFO/}

## Where - nested property

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_2_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_2_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Commands filtering_2_3@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_2_4@Indexes\Querying\Filtering.cs /}
{CODE-TABS/}

## Where + Any

`Any` is useful when you have a collection of items (e.g. `Order` contains `OrderLines`) and you want to filter out based on a values from this collection. For example, let's retrieve all orders that contain a `OrderLine` with a given product.

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_3_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_3_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Commands filtering_3_3@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_3_4@Indexes\Querying\Filtering.cs /}
{CODE-TABS/}

## Where + In

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

## Where + ContainsAny

To check if enumeration contains **any** of the values from a specified collection you can use `ContainsAny` method.

Let's assume that we want to return all `BlogPosts` that contain any of the specified `Tags`.

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_5_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_5_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Commands filtering_5_3@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_5_4@Indexes\Querying\Filtering.cs /}
{CODE-TABS/}

{WARNING:Important}
Remember to add `Raven.Client.Linq` namespace to usings if you want to use `ContainsAny` extension method.
{WARNING/}

## Where + ContainsAll

To check if enumeration contains **all** of the values from a specified collection you can use `ContainsAll` method.

Let's assume that we want to return all `BlogPosts` that contain all of the specified `Tags`.

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_6_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_6_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Commands filtering_6_3@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_5_4@Indexes\Querying\Filtering.cs /}
{CODE-TABS/}

{WARNING:Important}
Remember to add `Raven.Client.Linq` namespace to usings if you want to use `ContainsAll` extension method.
{WARNING/}

## Remarks

{INFO Underneath, `Query` and `DocumentQuery` are converting predicates to `IndexQuery` class so they can issue a query from **low-level command method**. /}

{SAFE By default **page size is set to 128** if not specified, so above queries will not return more than 128 results. You can read more about paging [here](../../indexes/querying/paging). /}

## Related articles

- [Indexing : Basics](../../indexes/indexing-basics)
- [Querying : Basics](../../indexes/querying/basics)
- [Querying : Paging](../../indexes/querying/paging)
- [Querying : Sorting](../../indexes/querying/sorting)
