# Querying: RQL - Raven Query Language
---

{NOTE: }

Queries in RavenDB use an SQL-like language called **RQL** (Raven Query Language).  

* In this page:  
  * [Overview](../../indexes/querying/what-is-rql#overview)  
  * [Query Optimizer](../../indexes/querying/what-is-rql#query-optimizer)  
     * [Dynamic and Indexed Queries](../../indexes/querying/what-is-rql#dynamic-and-indexed-queries)  
     * [Queries Usage of Indexes](../../indexes/querying/what-is-rql#queries-usage-of-indexes)  
  * [RQL Keywords and Methods](../../indexes/querying/what-is-rql#rql-keywords-and-methods)  
     * [`declare`](../../indexes/querying/what-is-rql#declare)  
     * [`from`](../../indexes/querying/what-is-rql#from)  
     * [`group by`](../../indexes/querying/what-is-rql#group-by)  
     * [`where`](../../indexes/querying/what-is-rql#where)  
     * [`order by`](../../indexes/querying/what-is-rql#order-by)  
     * [`load`](../../indexes/querying/what-is-rql#load)  
     * [`select`](../../indexes/querying/what-is-rql#select)  
     * [`update`](../../indexes/querying/what-is-rql#update)  
     * [`include`](../../indexes/querying/what-is-rql#include)  
     * [`with`](../../indexes/querying/what-is-rql#with)  
     * [`match`](../../indexes/querying/what-is-rql#match)  
     * [`Keywords and Methods Summary`](../../indexes/querying/what-is-rql#keywords-and-methods)  

{NOTE/}

---

{PANEL: Overview}

RQL is designed to expose externally the RavenDB query pipeline in a way that is easy 
to understand, easy to use, and not overwhelming to the user.

{PANEL/}

{PANEL: Query Optimizer}

As soon as a query reaches a RavenDB instance, the instance calls its query optimizer to 
analyze the query and determine which indexes should be used to retrieve the requested data.   

---

#### Dynamic and Indexed Queries

RavenDB has two types of queries:  

* A **dynamic query**, e.g. ```from Orders where ...```, which gives the query optimizer 
  full freedom to choose which index the query will use.  
* An **indexed query**, that specifies the index it would use.  
  E.g. ```from index 'Orders/ByCompany' where ...```, 
  which instructs RavenDB to use the ```Orders/ByCompany``` index.

---

#### Queries Usage of Indexes

In other databases, the query optimizer may fail to find a suitable index and fall back 
into querying using a full scan.  

RavenDB doesn't include support for full scans. If an index cannot be found for a query, 
the query optimizer will **create a new index for the query**.  

RavenDB queries will use the index they have created or found, to minimize response 
time and return results at the same speed regardless of the size of the data.  

You can read more about indexes [here](../indexing-basics).   


{NOTE: Exception: Counters}
Queries in RavenDB will always use an index, with the exception of [distributed counters]().  
Counters are **not** automatically indexed. They **can** be indexed manually.  
When an index has not been created manually for counters, they are queried without an index.  
{NOTE/}

{NOTE: Indexing and queries in RavenDB }
Indexing in RavenDB is a **background operation**.  
An yet-unindexed query will wait for the indexing process to complete (or timeout).  
A query that can be answered using an existing index will proceed normally 
using this index.  
When the creation of a new index has caught up, RavenDB will remove all the 
old indexes that are now superseded by the new index.  
{NOTE/}

{PANEL/}

---

#RQL Keywords and Methods

{PANEL: `declare`}

The keyword `declare` gives you the ability to create a JavaScript function that can be reused in `select` (when projection is done). You can read more about it [here](../../client-api/session/querying/how-to-project-query-results#example-iv---projection-with-).

{PANEL/}

{PANEL: `from`}

The keyword `from` is used to determine the source data that will be used when a query is executed.  

You have two options:

* `from <collection>`  
  This option is used to perform:
   - Collection queries that perform just basic ID filtering.  
     When this is the case, there is no need to query an index 
     and the required document is returned directly from the storage.  
     E.g.  
     `from Companies where id() == 'companies/1-A'`  
   - Dynamic queries that are executed using [auto indexes](../../indexes/creating-and-deploying#auto-indexes).  

      {INFO:All Documents}
       In order to query all documents, the `@all_docs` keyword can be used:

         - `from @all_docs where FirstName = 'Laura'`
         - `from @all_docs where id() = 'companies/1-A'`.
       {INFO/}

* `from INDEX <index-name>`  
  This option is used to perform RQL operations with a specific index.  

{PANEL/}

{PANEL: `group by`}

The keyword `group by` is used to create an aggregation query. Please refer to the article about [dynamic group by queries](../../client-api/session/querying/how-to-perform-group-by-query) to find out more.

{PANEL/}

{PANEL: `where`}

You can use the `where` keyword with various operators 
to filter chosen documents from the final result-set.  

---

### Operator: `>=`, `<=`, `<>`, `!=`, `<`, `>`, `=`, `==`

These basic operators can be used with all value types, including 'numbers' and 'strings'.

You can, for example, return every document from the 
[companies collection](../../client-api/faq/what-is-a-collection) 
whose _field value_ **=** _a given input_.  

{CODE-BLOCK:csharp}
from Companies
where Name = 'The Big Cheese'
{CODE-BLOCK/}

Filtering on **nested properties** is also supported, so in order to return all companies from 'Albuquerque' we need to execute following query:

{CODE-BLOCK:csharp}
from Companies
where Address.City = 'Albuquerque'
{CODE-BLOCK/}

---

### Operator: `between`

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

---

### Operator: `in`

The operator `in` is validating if a given field contains passed values. It will return results if a given field matches **any** of the passed values.

{CODE-BLOCK:csharp}
from Companies 
where Name IN ('The Big Cheese', 'Unknown company name')
{CODE-BLOCK/}

{CODE-BLOCK:csharp}
from Orders 
where Lines[].ProductName in ('Chang', 'Spegesild', 'Unknown product name') 
{CODE-BLOCK/}

---

### Operator: `all in`

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

---

### Binary Operators: `AND`, `OR`, `NOT`

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

---

### Subclauses: `(`, `)`

Subclauses can be used along with binary operators to build even more complex logical statements. They are self-explanatory so no example will be given.

---

{PANEL/}

{PANEL: `order by`}

To perform sorting, `order by` must be used.  
Read more about this subject in the [article dedicated to sorting](../../indexes/querying/sorting).  

{PANEL/}

{PANEL: `load`}

When there is a need to use data from an external document in projection, `load` can be used. Please refer to the following [projection article](../../indexes/querying/projections#example-vii---projection-using-a-loaded-document) to find out more about it.

{PANEL/}

{PANEL: `select`}

Projections can be performed by using `select`. Please read our dedicated projection article [here](../../indexes/querying/projections).

{PANEL/}

{PANEL: `update`}

To patch documents on the server-side, use `update` with the desired JavaScript script that will be applied to any documents matching the query criteria. For more information, please refer to our [patching article](../../client-api/operations/patching/set-based).

{PANEL/}

{PANEL: `include`}

The keyword `include` has been introduced to support:

- [including additional documents](../../client-api/how-to/handle-document-relationships#includes) or counters to the query response
- [highlighting](../../client-api/session/querying/how-to-use-highlighting) results
- query timings
- explanations

{PANEL/}

{PANEL: `with`}

The keyword `with` is used to determine the data source of a [graph query](../../indexes/querying/graph/graph-queries).  
There are two types of `with` clauses, regular `with` and `with edges`.

- with: `with {from Orders} as o`  
  The above statement means that the data source referred to by the alias `o` is the result of the `from Orders` query  
    
- with edges: `with edges (Lines) { where Discount >= 0.25 select Product } as cheap`  
  The above statement means that our data source is the property `Lines` of the source documents and we filter all lines that match `Discount >= 0.25` query
  the destination referred to by the `cheap` alias is the product pointed by the `Product` property of the order line  
    
For more details regarding graph queries please read the following article about [graph query](../../indexes/querying/graph/graph-queries) 

{PANEL/}

{PANEL: `match`}

The keyword `match` is used to determine the pattern of a [graph query](../../indexes/querying/graph/graph-queries).  
`match (Orders as o)-[Lines as cheap where Discount >= 0.25 select Product]->(Products as p)`  
The above statement means that we are searching for a pattern that starts with an order and traverse using the
order lines referred to by the `Lines` property where their `Discount` property is larger than 25%  and the destination is the product referred to by the `Product` property.  

A match may contain an edge in both direction, a right edge would look like so `(node1)-[right]->(node2)` and a left one would look like so `(node1)<-[left]-(node2)`.  
Any combination of edges is allowed in a match clause e.g.  
`(node1)-[right]->(node2)<-[left]-(node3)`  
The above match will actually be translated to:  
`(node1)-[right]->(node2)`  
and  
`(node3)-[left]->(node2)`  
where the `and` is a set intersection between the two patterns.  

For more details regarding graph queries please read the following article about [graph query](../../indexes/querying/graph/graph-queries)  

{PANEL/}

{PANEL: Keywords and Methods}

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
  - [DESC | DESCENDING](../../indexes/querying/sorting#basics)
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
- [WITH](../../indexes/querying/what-is-rql#with)
- [MATCH](../../indexes/querying/what-is-rql#match)

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

## Related Articles

### Client API

- [How to Query](../../client-api/session/querying/how-to-query)
- [How to Use RQL Directly When Querying](../../client-api/session/querying/how-to-query#session.advanced.rawquery)

### Querying

- [Basics](../../indexes/querying/basics)
