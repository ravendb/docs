# Querying: Filtering

One of the most basic functionalities of querying is the ability to filter out data and return records that match a given condition. There are couple of ways to do this. 

The following examples demonstrate how to add simple conditions to a query using all of those methods.

## Where

{CODE-TABS}
{CODE-TAB:java:Query filtering_0_1@Indexes\Querying\Filtering.java /}
{CODE-TAB:java:Index filtering_0_4@Indexes\Querying\Filtering.java  /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName'
where FirstName = 'Robert' and LastName = 'King'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where - Numeric Property

{CODE-TABS}
{CODE-TAB:java:Query filtering_1_1@Indexes\Querying\Filtering.java /}
{CODE-TAB:java:Index filtering_1_4@Indexes\Querying\Filtering.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Products/ByUnitsInStock'
where UnitsInStock > 50
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where - Nested Property

{CODE-TABS}
{CODE-TAB:java:Query filtering_10_1@Indexes\Querying\Filtering.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders
where ShipTo.City = 'Albuquerque'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:java:Query filtering_2_1@Indexes\Querying\Filtering.java /}
{CODE-TAB:java:Index filtering_2_4@Indexes\Querying\Filtering.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Order/ByOrderLinesCount'
where Lines.Count > 50
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where + Any

`Any` is useful when you have a collection of items (e.g. `Order` contains `OrderLines`) and you want to filter out based on values from this collection. For example, let's retrieve all orders that contain an `OrderLine` with a given product.

{CODE-TABS}
{CODE-TAB:java:Query filtering_3_1@Indexes\Querying\Filtering.java /}
{CODE-TAB:java:Index filtering_3_4@Indexes\Querying\Filtering.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Order/ByOrderLinesCount'
where Lines_ProductName = 'Teatime Chocolate Biscuits'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where + In

When you want to check a single value against multiple values, the `In` operator can become handy. To retrieve all employees where `FirstName` is either `Robert` or `Nancy`, we can issue the following query:

{CODE-TABS}
{CODE-TAB:java:Query filtering_4_1@Indexes\Querying\Filtering.java /}
{CODE-TAB:java:Index filtering_0_4@Indexes\Querying\Filtering.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstAndLastName'
where FirstName IN ('Robert', 'Nancy')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where + ContainsAny

To check if enumeration contains **any** of the values from a specified collection, you can use the `containsAny` method.

Let's assume that we want to return all `BlogPosts` that contain any of the specified `tags`.

{CODE-TABS}
{CODE-TAB:java:Query filtering_5_1@Indexes\Querying\Filtering.java /}
{CODE-TAB:java:Index filtering_5_4@Indexes\Querying\Filtering.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'BlogPosts/ByTags'
where Tags IN ('Development', 'Research')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where + ContainsAll

To check if an enumeration contains **all** of the values from a specified collection, you can use the `containsAll` method.

Let's assume that we want to return all the `BlogPosts` that contain all of the specified `tags`.

{CODE-TABS}
{CODE-TAB:java:Query filtering_6_1@Indexes\Querying\Filtering.java /}
{CODE-TAB:java:Index filtering_5_4@Indexes\Querying\Filtering.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'BlogPosts/ByTags'
where Tags ALL IN ('Development', 'Research')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where - StartsWith

{CODE-TABS}
{CODE-TAB:java:Query filtering_8_1@Indexes\Querying\Filtering.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Products 
where startsWith(Name, 'ch')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Where - EndsWith

{CODE-TABS}
{CODE-TAB:java:Query filtering_9_1@Indexes\Querying\Filtering.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Products 
where endsWith(Name, 'ra')
{CODE-TAB-BLOCK/}
{CODE-TABS/}


## Remarks

{INFO Underneath, `Query` is converting predicates to the `IndexQuery` class so they can issue a query from a **low-level operation method**. /}

## Related Articles

### Indexes

- [Indexing Basics](../../indexes/indexing-basics)

### Querying

- [Basics](../../indexes/querying/basics)
- [Paging](../../indexes/querying/paging)
- [Sorting](../../indexes/querying/sorting)
