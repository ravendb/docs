# Querying: Filtering
---

{NOTE: }

One of the most basic functionalities of querying is the ability to filter out data and return records that match a given condition. 
There are a couple of ways to do this and they all depend on the querying approach you want to use: 

* [Query](../../client-api/session/querying/how-to-query) from basic session operations
* [DocumentQuery](../../client-api/session/querying/document-query/what-is-document-query) from `advanced` session operations 
* [RQL](../../client-api/session/querying/what-is-rql) - Raven Query Language 

The following examples demonstrate how to add various filtering conditions to a query.

* In this page:
   * [Where](../../indexes/querying/filtering#where)
   * [Where - Numeric Property](../../indexes/querying/filtering#where---numeric-property)
   * [Where - Nested Property](../../indexes/querying/filtering#where---nested-property)
   * [Where + Any](../../indexes/querying/filtering#where--any)
   * [Where + In](../../indexes/querying/filtering#where--in)
   * [Where + ContainsAny](../../indexes/querying/filtering#where--containsany)
   * [Where + ContainsAll](../../indexes/querying/filtering#where--containsall)
   * [Where - StartsWith](../../indexes/querying/filtering#where---startswith)
   * [Where - EndsWith](../../indexes/querying/filtering#where---endswith)
   * [Where - Identifier Property](../../indexes/querying/filtering#where---identifier-property)
   * [Where - Exists](../../indexes/querying/filtering#where---exists)
   * [Remarks](../../indexes/querying/filtering#remarks)

{NOTE/}

## Where

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_0_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_0_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_0_4@Indexes\Querying\Filtering.cs  /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName'
where FirstName = 'Robert' and LastName = 'King'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where - Numeric Property

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_1_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_1_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_1_4@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Products/ByUnitsInStock'
where UnitsInStock > 50
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where - Nested Property

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_10_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_10_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
where ShipTo.City = 'Albuquerque'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_2_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_2_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_2_4@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Order/ByOrderLinesCount'
where Lines.Count > 50
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where + Any

`Any` is useful when you have a collection of items (e.g. `Order` contains `OrderLines`) and you want to filter out based on values from this collection. For example, let's retrieve all orders that contain an `OrderLine` with a given product.

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_3_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_3_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_3_4@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Order/ByOrderLinesCount'
where Lines_ProductName = 'Teatime Chocolate Biscuits'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where + In

When you want to check a single value against multiple values, the `In` operator can become handy. To retrieve all employees where `FirstName` is either `Robert` or `Nancy`, we can issue the following query:

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_4_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_4_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_0_4@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName'
where FirstName IN ('Robert', 'Nancy')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{WARNING:Important}
Remember to add the `Raven.Client.Documents.Linq` namespace to usings if you want to use `In` extension method.
{WARNING/}

## Where + ContainsAny

To check if enumeration contains **any** of the values from a specified collection, you can use the `ContainsAny` method.

Let's assume that we want to return all `BlogPosts` that contain any of the specified `Tags`.

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_5_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_5_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_5_4@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'BlogPosts/ByTags'
where Tags IN ('Development', 'Research')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{WARNING:Important}
Remember to add the `Raven.Client.Documents.Linq` namespace to usings if you want to use the `ContainsAny` extension method.
{WARNING/}

## Where + ContainsAll

To check if an enumeration contains **all** of the values from a specified collection, you can use the `ContainsAll` method.

Let's assume that we want to return all the `BlogPosts` that contain all of the specified `Tags`.

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_6_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_6_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:Index filtering_5_4@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'BlogPosts/ByTags'
where Tags ALL IN ('Development', 'Research')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{WARNING:Important}
Remember to add the `Raven.Client.Documents.Linq` namespace to usings if you want to use the `ContainsAll` extension method.
{WARNING/}

## Where - StartsWith

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_8_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_8_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Products 
where startsWith(Name, 'ch')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where - EndsWith

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_9_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_9_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Products 
where endsWith(Name, 'ra')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where - Identifier Property

Once a property used in the `Where` clause is recognized as an identity property of a given entity type 
(according to [`FindIdentityProperty` convention](../../client-api/configuration/identifier-generation/global#findidentityproperty))
and there aren't any other fields involved in the query, then it is called a "collection query". 
Simple collection queries that ask about documents with given IDs or where identifiers start with a given prefix
and don't require any additional handling like ordering, full-text searching, etc, are handled directly by the storage engine. 
It means that querying by ID doesn't create an auto-index and has no extra cost. In terms of efficiency, it is the same as
loading documents with [`session.Load`](../../client-api/session/loading-entities) usage.


{CODE-TABS}
{CODE-TAB:csharp:Query filtering_11_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_11_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
where id() = 'orders/1-A'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:csharp:Query filtering_12_1@Indexes\Querying\Filtering.cs /}
{CODE-TAB:csharp:DocumentQuery filtering_12_2@Indexes\Querying\Filtering.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
where startsWith(id(), 'orders/1')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where - Exists

To find all documents in a collection that have a specified field, see [How to Filter by Field Presence](../../client-api/session/querying/how-to-filter-by-field).  

To find all documents in a collection that don't have a specified field, see [How to Filter by Non-Existing Field](../../client-api/session/querying/how-to-filter-by-non-existing-field).

## Remarks

{INFO Underneath, `Query` and `DocumentQuery` are converting predicates to the `IndexQuery` class so they can issue a query from a **low-level operation method**. /}

## Related Articles

### Client API

- [Query Overview](../../client-api/session/querying/how-to-query)

### Indexes

- [Indexing Basics](../../indexes/indexing-basics)

### Querying

- [Query an Index](../../indexes/querying/query-index)
- [Paging](../../indexes/querying/paging)
- [Sorting](../../indexes/querying/sorting)
