# Querying: RQL - Raven Query Language

##Overview

Queries in RavenDB use a SQL-like language called "RavenDB Query Language," henceforth known as RQL.  
It is designed to expose externally the RavenDB query pipeline in a way that is easy to understand, easy to use, and not overwhelming to the user.

### Query Optimizer
When a query hits a RavenDB instance, the very first thing that happens is that it will be analyzed by the query optimizer. The role of the query optimizer is to determine what indexes should be used by this particular query.   
In RavenDB, there are two types of queries.  
We may have a *dynamic query*, such as ```from Orders where ...```, which gives the query optimizer full freedom with regards to which index that query will use.  
Alternatively, a query can specify a specific index to be used, such as ```from index 'Orders/ByCompany' where ...```, which instructs RavenDB to use the ```Orders/ByCompany``` index.

{NOTE: Queries are always going to use an index}
In other databases, the query optimizer may fail to find a suitable index and fall back into querying using a full scan, RavenDB doesn't include support for full scans, and the query optimizer will _create_ a new index for the query if it cannot find an relevant index.  
You can read more about indexes [here](../indexing-basics).   
Queries in RavenDB will always use an index. RavenDB queries always use an index and can return results with the same speed regardless of the size of the data.  
{NOTE/}

{NOTE: Indexing and queries in RavenDB }
Indexing in RavenDB is a background operation, which means the new query will be waiting for the index to complete indexing (or timeout). But at the same time, queries that can be answered using the existing indexes will proceed normally using these indexes. When the new index has caught up, RavenDB will clean up all the old indexes that are now superseded by the new one.
{NOTE/}

{PANEL:Keywords and methods}

The following keywords and methods are available in RQL:

- DECLARE
- [FROM](../../indexes/querying/what-is-rql#from)
  - INDEX
- [GROUP BY](../../indexes/querying/what-is-rql#group-by)
  - [array()](../../client-api/session/querying/how-to-perform-group-by-query#by-array-content)
- [WHERE](../../indexes/querying/what-is-rql#where)
  - id()
  - [search()](../../indexes/querying/searching)
  - cmpxchg()
  - boost()
  - [regex()](../../client-api/session/querying/how-to-use-regex)
  - startsWith()
  - endsWith()
  - [lucene()](../../client-api/session/querying/document-query/how-to-use-lucene)
  - [exists()](../../client-api/session/querying/how-to-filter-by-field)
  - exact()
  - [intersect()](../../indexes/querying/intersection)
  - [spatial.within()](../../indexes/querying/spatial)
  - [spatial.contains()](../../indexes/querying/spatial)
  - [spatial.disjoint()](../../indexes/querying/spatial)
  - [spatial.intersects()](../../indexes/querying/spatial)
  - [moreLikeThis()](../../client-api/session/querying/how-to-use-morelikethis)
- [ORDER BY](../../indexes/querying/what-is-rql#order-by)
  - [ASC | ASCENDING](../../indexes/querying/sorting#basics)
  - [DESC | DESCEDING](../../indexes/querying/sorting#basics)
  - [AS](../../indexes/querying/sorting#basics)
    - [string](../../indexes/querying/sorting#basics)
    - [long](../../indexes/querying/sorting#basics)
    - [double](../../indexes/querying/sorting#basics)
    - [alphaNumeric](../../indexes/querying/sorting#alphanumeric-ordering)
  - [random()](../../indexes/querying/sorting#random-ordering)
  - [score()](../../indexes/querying/sorting#ordering-by-score)
  - [spatial.distance()](../../client-api/session/querying/how-to-query-a-spatial-index#orderbydistance)
- [LOAD](../../indexes/querying/what-is-rql#load)
- [SELECT](../../indexes/querying/what-is-rql#select)
  - DISTINCT
  - key()
  - sum()
  - count()
  - [facet()](../../indexes/querying/faceted-search)
- [UPDATE](../../indexes/querying/what-is-rql#update)
- [INCLUDE](../../indexes/querying/what-is-rql#include)

With the following operators:

- >=
- <=
- <> or !=
- <
- >
- = or ==
- BETWEEN
- IN
- ALL IN
- OR
- AND
- NOT
- (
- )

And the following values:

- true
- false
- null
- string e.g. 'John' or "John"
- number (long and double) e.g. 17
- parameter e.g. $param1

{PANEL/}

<hr />

{PANEL:DECLARE}

The keyword `declare` gives you the ability to create a JS function that can be reused in `select` (when projection is done). You can read more about it [here](../../client-api/session/querying/how-to-project-query-results#example-iv---projection-with-).

{PANEL/}

<hr />

{PANEL:FROM}

The keyword `from` is used to determine the source data that should be used when a query is executed. You have two options:

1. `from <collection>`

This option is used to perform:

- Collection queries that are doing basic ID filtering only, e.g. `from Companies where id() == 'companies/1-A'` where there is no need to query an index, we can return the document from the storage directly
- Dynamic queries that are being executed against [Auto Index](../../indexes/creating-and-deploying#auto-indexes)

{INFO:All Documents}

In order to query all documents, the `@all_docs` keyword can be used:

- `from @all_docs where FirstName = 'Laura'`
- `from @all_docs where id() = 'companies/1-A'`.

{INFO/}

2. `from INDEX <index-name>`

This option is used to perform RQL operations against a given [static index].

{PANEL/}

{PANEL:GROUP BY}

The keyword `group by` is used to create an aggregation query. Please refer to the article about [dynamic group by queries](../../client-api/session/querying/how-to-perform-group-by-query) to find out more.

{PANEL/}

<hr />

{PANEL:WHERE}

The keyword `where` is used to filter-out the documents from final results.

### Operator: >= <= <> != < > = ==

The operators above are considered basic and self-explanatory. They work on all value types including 'numbers' and 'strings'.

The simplest example would be to return results with the field value **equal** to a given input. If you want to return a document from the `@companies` collection (more about collection can be read [here](../../client-api/faq/what-is-a-collection)), you need to execute the following query:

{CODE-BLOCK:csharp}
from Companies
where Name = 'The Big Cheese'
{CODE-BLOCK/}

Filtering on **nested properties** is also supported, so in order to return all companies from 'Albuquerque' we need to execute following query:

{CODE-BLOCK:csharp}
from Companies
where Address.City = 'Albuquerque'
{CODE-BLOCK/}

### Operator: BETWEEN

The operator `between` returns results inclusively, and the type of border values used must match. It works on both 'numbers' and 'strings' and can be substituted with the `>=` and `<=` operators (see the example below).

{CODE-BLOCK:csharp}
from Products 
where PricePerUnit between 10.5 and 13.0
{CODE-BLOCK/}

or

{CODE-BLOCK:csharp}
from Products 
where PricePerUnit >= 10.5 and PricePerUnit <= 13.0
{CODE-BLOCK/}

### Operator: IN

The operator `in` is validating if a given field contains passed values. It will return results if a given field matches **any** of the passed values.

{CODE-BLOCK:csharp}
from Companies 
where Name IN ('The Big Cheese', 'Unknown company name')
{CODE-BLOCK/}

{CODE-BLOCK:csharp}
from Orders 
where Lines[].ProductName in ('Chang', 'Spegesild', 'Unknown product name') 
{CODE-BLOCK/}

### Operator: ALL IN

This operator checks if **all** passes values are matching a given field. Due to its mechanics, it is only useful when used on array fields.

The following query will yield no results in contrast to an 'in' operator.

{CODE-BLOCK:csharp}
from Orders 
where Lines[].ProductName all in ('Chang', 'Spegesild', 'Unknown product name') 
{CODE-BLOCK/}

but removing the 'Unknown product name' will give you orders that only contains products with both 'Chang' and 'Spegesild' names

{CODE-BLOCK:csharp}
from Orders 
where Lines[].ProductName all in ('Chang', 'Spegesild') 
{CODE-BLOCK/}

### Binary operators: AND OR NOT

Binary operators can be used to build more complex statements. The `NOT` operator can only be used with one of the other binary operators creating `OR NOT` or `AND NOT` ones.

{CODE-BLOCK:csharp}
from Companies
where Name = 'The Big Cheese' OR Name = 'Richter Supermarkt'
{CODE-BLOCK/}

{CODE-BLOCK:csharp}
from Orders
where Freight > 500 AND ShippedAt > '1998-01-01'
{CODE-BLOCK/}

{CODE-BLOCK:csharp}
from Orders
where Freight > 500 AND ShippedAt > '1998-01-01' AND NOT Freight = 830.75
{CODE-BLOCK/}

### Subclauses: ( )

Subclauses can be used along with binary operators to build even more complex logical statements. They are self-explanatory so no example will be given.

{PANEL/}

<hr />

{PANEL:ORDER BY}

To perform sorting, the `order by` must be used. If you are interested in this subject, please read our dedicated sorting article [here](../../indexes/querying/sorting).

{PANEL/}

<hr />

{PANEL:LOAD}

When there is a need to use data from an external document in projection, `load` can be used. Please refer to the following [projection article](../../indexes/querying/projections#example-vii---projection-using-a-loaded-document) to find out more about it.

{PANEL/}

<hr />

{PANEL:SELECT}

Projections can be performed by using `select`. Please read our dedicated projection article [here](../../indexes/querying/projections).

{PANEL/}

<hr />

{PANEL:UPDATE}

To patch documents on the server-side, use `update` with the desired JS script that will be applied to any documents matching the query criteria. For more information, please refer to our [patching article](../../client-api/operations/patching/set-based).

{PANEL/}

<hr />

{PANEL:INCLUDE}

The keyword `include` has been introduced to support:

- [including additional documents](../../client-api/how-to/handle-document-relationships#includes) or counters to the query response
- [highlighting](../../client-api/session/querying/how-to-use-highlighting) results
- query timings
- explanations

{PANEL/}

## Related Articles

### Client API

- [How to Query](../../client-api/session/querying/how-to-query)
- [How to Use RQL Directly When Querying](../../client-api/session/querying/how-to-query#session.advanced.rawquery)

### Querying

- [Basics](../../indexes/querying/basics)
