# RQL - Raven Query Language
---

{NOTE: }

* Queries in RavenDB use a SQL-like language called **RQL** (Raven Query Language).

* RQL exposes the RavenDB query pipeline in a straightforward and accessible manner  
  that is easy to use and interact with.

* Any query written using high-level Session methods (`Query`, `DocumentQuery`)  
  is translated by the client to RQL before being sent to the server for execution.

* A query can be written with RQL directly by either:  
  * Using the session's `RawQuery` method  
  * Making a query from the [Query view](../../../studio/database/queries/query-view) in Studio  

* Learn more about querying from the session in this [Query Overview](../../../client-api/session/querying/how-to-query). 

* In this page:  

  * [The query pipeline](../../../client-api/session/querying/what-is-rql#the-query-pipeline)
  
  * [RQL keywords and methods](../../../client-api/session/querying/what-is-rql#rql-keywords-and-methods)
  
     * [`declare`](../../../client-api/session/querying/what-is-rql#declare)  
     * [`from`](../../../client-api/session/querying/what-is-rql#from)
     * [`where`](../../../client-api/session/querying/what-is-rql#where)
     * [`group by`](../../../client-api/session/querying/what-is-rql#group-by)
     * [`include`](../../../client-api/session/querying/what-is-rql#include)
     * [`order by`](../../../client-api/session/querying/what-is-rql#order-by)
     * [`select`](../../../client-api/session/querying/what-is-rql#select)
     * [`load`](../../../client-api/session/querying/what-is-rql#load)
     * [`limit`](../../../client-api/session/querying/what-is-rql#limit)
     * [`update`](../../../client-api/session/querying/what-is-rql#update)
    
  * [RQL comments](../../../client-api/session/querying/what-is-rql#rql-comments)

{NOTE/}

---

{PANEL: The query pipeline}  

The query pipeline in RavenDB includes the following main stages:  

1. __Detect query source__  ([`from`](../../../client-api/session/querying/what-is-rql#from))

   * Based on your query, RavenDB will determine the appropriate data source from which to retrieve results.  
   
   * Note: all queries in RavenDB use an index to provide results, even when you don't specify one.   
    
   * The following options are available:
     
     * `from index` - Explicitly specify which index to use.
   
     * `from collection` - Specify the collection to query.  
       RavenDB will decide which index will be used depending on the query criteria.

   * Learn more about these __query scenarios__ in this [Query Overview](../../../client-api/session/querying/how-to-query#queries-always-provide-results-using-an-index).

2. __Filter the data__ ([`where`](../../../client-api/session/querying/what-is-rql#where))  
   * The index is scanned for records that match the query predicate. 

3. __Include related documents__  ([`include`](../../../client-api/session/querying/what-is-rql#include))
    * [Related documents](../../../client-api/how-to/handle-document-relationships#includes) that are included in the query will be retrieved and returned to the client  
      along with the resulting matching documents, reducing the need to do another network round trip  
      to the database when accessing the included documents.

4. __Sort results__ ([`order by`](../../../client-api/session/querying/what-is-rql#order-by)) 
   * Query results can be sorted.  
     For example, you can order by a field value, by the resulting documents' score, by random ordering, etc.

5. __Limit results__ ([`limit`](../../../client-api/session/querying/what-is-rql#limit))
   * You can specify the number of results you want to get back from the query  
     and the number of results you want to skip.

6. __Project results__ ([`select`](../../../client-api/session/querying/what-is-rql#select)) 
   * [Projections](../../../indexes/querying/projections) are specified when you need to retrieve only specific document fields, instead of the whole full document.
     This reduces the amount of data sent over the network and is useful when only partial data is needed.
     When projections are Not defined on the query - then the full document content is retrieved from the document storage.

   * Projections are applied as the last stage after the query has been processed, filtered, sorted, and paged.  
     This means that the projection doesn't apply to all the documents in the database,  
     only to the results that are actually returned.

   * Data can be loaded ([`load`](../../../client-api/session/querying/what-is-rql#load)) from related documents to be used in the projected fields.  
   
   * For each record, the server extracts the requested field:  
     If a field is stored in the index - the server will fetch it from the index.  
     If a field is Not stored in the index - the server will fetch it from the document storage.  

6. __Return results__ to the client.

{PANEL/}

{PANEL: RQL keywords and methods}

The following keywords and methods are available in RQL:

- [DECLARE](../../../client-api/session/querying/what-is-rql#declare)
- [FROM](../../../client-api/session/querying/what-is-rql#from)
    - index
- [GROUP BY](../../../client-api/session/querying/what-is-rql#group-by)
    - [array()](../../../client-api/session/querying/how-to-perform-group-by-query#by-array-content)
- [WHERE](../../../client-api/session/querying/what-is-rql#where)
    - id()
    - [search()](../../../client-api/session/querying/text-search/full-text-search)
    - cmpxchg()
    - [boost()](../../../client-api/session/querying/text-search/boost-search-results)
    - [regex()](../../../client-api/session/querying/text-search/using-regex)
    - [startsWith()](../../../client-api/session/querying/text-search/starts-with-query)
    - [endsWith()](../../../client-api/session/querying/text-search/ends-with-query)
    - [lucene()](../../../client-api/session/querying/document-query/how-to-use-lucene)
    - [exists()](../../../client-api/session/querying/how-to-filter-by-field)
    - [exact()](../../../client-api/session/querying/text-search/exact-match-query)
    - [intersect()](../../../indexes/querying/intersection)
    - [spatial.within()](../../../indexes/querying/spatial)
    - [spatial.contains()](../../../indexes/querying/spatial)
    - [spatial.disjoint()](../../../indexes/querying/spatial)
    - [spatial.intersects()](../../../indexes/querying/spatial)
    - [moreLikeThis()](../../../client-api/session/querying/how-to-use-morelikethis)
    - [vector.search()](../../../ai-integration/vector-search-using-dynamic-query)
- [ORDER BY](../../../client-api/session/querying/what-is-rql#order-by)
    - [ASC | ASCENDING](../../../indexes/querying/sorting#basics)
    - [DESC | DESCENDING](../../../indexes/querying/sorting#basics)
    - [AS](../../../indexes/querying/sorting#basics)
        - [string](../../../indexes/querying/sorting#basics)
        - [long](../../../indexes/querying/sorting#basics)
        - [double](../../../indexes/querying/sorting#basics)
        - [alphaNumeric](../../../indexes/querying/sorting#alphanumeric-ordering)
    - [random()](../../../indexes/querying/sorting#random-ordering)
    - [score()](../../../indexes/querying/sorting#ordering-by-score)
    - [spatial.distance()](../../../client-api/session/querying/how-to-make-a-spatial-query#spatial-sorting)
- [LOAD](../../../client-api/session/querying/what-is-rql#load)
- [SELECT](../../../client-api/session/querying/what-is-rql#select)
    - DISTINCT
    - key()
    - sum()
    - count()
    - [facet()](../../../indexes/querying/faceted-search)
    - [timeseries()](../../../document-extensions/timeseries/querying/overview-and-syntax#syntax)
    - [counter()](../../../document-extensions/counters/counters-and-other-features#counters-and-queries)
- [LIMIT](../../../client-api/session/querying/what-is-rql#limit)
- [UPDATE](../../../client-api/session/querying/what-is-rql#update)
- [INCLUDE](../../../client-api/session/querying/what-is-rql#include)

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

{PANEL: `declare`}

You can use the `declare` keyword to create a JavaScript function that can then be called from a `select` clause when using a projection. 
JavaScript functions add flexibility to your queries as they can be used to manipulate and format retrieved results.  

{CODE-BLOCK: javascript}
// Declare a JavaScript function 
declare function output(employee) {
    // Format the value that will be returned in the projected field 'FullName'
    var formatName = function(x){ return x.FirstName + " " + x.LastName; };
    return { FullName : formatName(employee) };
}

// Query with projection calling the 'output' JavaScript function
from Employees as employee select output(employee)
{CODE-BLOCK/}

Values are returned from a declared Javascript function as a set of values rather than in a nested array to ease the projection of retrieved values.
See an example for this usage [here](../../../document-extensions/timeseries/querying/overview-and-syntax#combine-time-series-and-javascript-functions).  

{PANEL/}

{PANEL: `from`}

The keyword `from` is used to determine the source data that will be used when the query is executed.  
The following options are available:

---

{NOTE: }

* __Query a specific collection__: &nbsp;&nbsp; `from <collection-name>`

{CODE-BLOCK: csharp}
// Full collection query 
// Data source: The raw collection documents (Auto-index is Not created)
from "Employees"
{CODE-BLOCK/}

{CODE-BLOCK: csharp}
// Collection query - by ID 
// Data source: The raw collection documents (Auto-index is Not created)
from "Employees" where id() = "employees/1-A"
{CODE-BLOCK/}

{CODE-BLOCK: csharp}
// Dynamic query - with filtering
// Data source: Auto-index (server uses an existing auto-index or creates a new one)
from "Employees" where FirstName = "Laura"
{CODE-BLOCK/}

{NOTE/}

{NOTE: }

* __Query all documents__: &nbsp;&nbsp; `from @all_docs`

{CODE-BLOCK: csharp}
// All collections query
// Data source: All raw collections (Auto-index is Not created)
from @all_docs
{CODE-BLOCK/}

{CODE-BLOCK: csharp}
// Dynamic query - with filtering 
// Data source: Auto-index (server uses an existing auto-index or creates a new one)
from @all_docs where FirstName = "Laura"
{CODE-BLOCK/}

{NOTE/}

{NOTE: }

* __Query an index__: &nbsp;&nbsp; `from index <index-name>`

{CODE-BLOCK: csharp}
// Index query
// Data source: The specified index
from index "Employees/ByFirstName"
{CODE-BLOCK/}

{CODE-BLOCK: csharp}
// Index query - with filtering
// Data source: The specified index
from index "Employees/ByFirstName" where FirstName = "Laura"
{CODE-BLOCK/}

{NOTE/}

{PANEL/}

{PANEL: `where`}

Use the `where` keyword with various operators to filter chosen documents from the final result-set.  

---

{NOTE: }

#### Operator: &nbsp;&nbsp; `>=`, `<=`, `<>`, `!=`, `<`, `>`, `=`, `==`

These basic operators can be used with all value types, including 'numbers' and 'strings'.  
For example, you can return every document from the [Companies collection](../../../client-api/faq/what-is-a-collection) 
whose _field value_ **=** _a given input_.  

{CODE-BLOCK:csharp}
from "Companies"
where Name == "The Big Cheese" // Can use either '=' or'==' 
{CODE-BLOCK/}

Filtering on **nested properties** is also supported.  
So in order to return all companies from 'Albuquerque' we need to execute following query:  

{CODE-BLOCK:csharp}
from "Companies"
where Address.City = "Albuquerque"
{CODE-BLOCK/}

{NOTE/}
{NOTE: }

#### Operator: &nbsp;&nbsp; `between`

The operator `between` returns results inclusively, and the type of border values used must match.  
It works on both 'numbers' and 'strings' and can be substituted with the `>=` and `<=` operators.

{CODE-BLOCK:csharp}
from "Products" 
where PricePerUnit between 10.5 and 13.0 // Using between
{CODE-BLOCK/}

{CODE-BLOCK:csharp}
from "Products" 
where PricePerUnit >= 10.5 and PricePerUnit <= 13.0 // Using >= and <=
{CODE-BLOCK/}

{NOTE/}
{NOTE: }

#### Operator: &nbsp;&nbsp; `in`

The operator `in` is validating if a given field contains passed values.  
It will return results if a given field matches **any** of the passed values.

{CODE-BLOCK:csharp}
from "Companies" 
where Name in ("The Big Cheese", "Unknown company name")
{CODE-BLOCK/}

{CODE-BLOCK:csharp}
from "Orders" 
where Lines[].ProductName in ("Chang", "Spegesild", "Unknown product name") 
{CODE-BLOCK/}

{NOTE/}
{NOTE: }

#### Operator: &nbsp;&nbsp; `all in`

This operator checks if **all** passes values are matching a given field.  
Due to its mechanics, it is only useful when used on array fields.

The following query will yield no results in contrast to the `in` operator.

{CODE-BLOCK:csharp}
from "Orders" 
where Lines[].ProductName all in ("Chang", "Spegesild", "Unknown product name")
{CODE-BLOCK/}

Removing 'Unknown product name' will return only orders that contain products with both  
'Chang' and 'Spegesild' names.

{CODE-BLOCK:csharp}
from "Orders" 
where Lines[].ProductName all in ("Chang", "Spegesild") 
{CODE-BLOCK/}

{NOTE/}
{NOTE: }

#### Binary Operators: &nbsp;&nbsp; `AND`, `OR`, `NOT`

Binary operators can be used to build more complex statements.  
The `NOT` operator can only be used with one of the other binary operators creating `OR NOT` or `AND NOT` ones.

{CODE-BLOCK:csharp}
from "Companies"
where Name = "The Big Cheese" OR Name = "Richter Supermarkt"
{CODE-BLOCK/}

{CODE-BLOCK:csharp}
from "Orders"
where Freight > 500 AND ShippedAt > '1998-01-01'
{CODE-BLOCK/}

{CODE-BLOCK:csharp}
from "Orders"
where Freight > 500 AND ShippedAt > '1998-01-01' AND NOT Freight = 830.75
{CODE-BLOCK/}

{NOTE/}
{NOTE: }

#### Subclauses: &nbsp;&nbsp; `(`, `)`

Subclauses can be used along with binary operators to build even more complex logical statements.  

{NOTE/}

{PANEL/}

{PANEL: `group by`}

The keyword `group by` is used to create an aggregation query.  
Learn more in [dynamic group by queries](../../../client-api/session/querying/how-to-perform-group-by-query).

{PANEL/}

{PANEL: `include`}

The keyword `include` has been introduced to support:

- [including related documents](../../../client-api/how-to/handle-document-relationships#includes) in the query response
- [including counters](../../../document-extensions/counters/counters-and-other-features#including-counters),
  [time series](../../../document-extensions/timeseries/client-api/session/include/with-raw-queries),
  or [revisions](../../../document-extensions/revisions/client-api/session/including#include-revisions-when-making-a-raw-query) in the query response
- [including compare-exchange items](../../../client-api/operations/compare-exchange/include-compare-exchange#include-cmpxchg-items-when-querying) in the query response
- [highlighting](../../../client-api/session/querying/text-search/highlight-query-results) results
- [get query timings](../../../client-api/session/querying/debugging/query-timings)
- [get explanations](../../../client-api/session/querying/debugging/include-explanations)

{PANEL/}

{PANEL: `order by`}

Use `order by` to perform sorting.  
Learn more in this [sorting](../../../indexes/querying/sorting) article.  

{PANEL/}

{PANEL: `select`}

Use `select` to have the query return a projection instead of the full document.  
Learn more in this [projection](../../../indexes/querying/projections) article.

{PANEL/}

{PANEL: `load`}

Use `load`when you need to use data from a related document in projection.  
See an example in this [projection](../../../indexes/querying/projections#example-viii---projection-using-a-loaded-document) article.

{PANEL/}

{PANEL: `limit`}

Use `limit` to limit the number of results returned by the query.  
Specify the number of items to __skip__ from the beginning of the result set and the number of items to __take__ (return).  
This is useful when [paging](../../../indexes/querying/paging) results.

{CODE-BLOCK:csharp}
// Available syntax options:
// =========================

from "Products" limit 5, 10       // skip 5, take 10

from "Products" limit 10 offset 5 // skip 5, take 10

from "Products" offset 5          // skip 5, take all the rest
{CODE-BLOCK/}

{PANEL/}

{PANEL: `update`}

To patch documents on the server-side, use `update` with the desired JavaScript that will be applied to any document matching the query criteria.  
For more information, please refer to this [patching](../../../client-api/operations/patching/set-based) article.  

{PANEL/}

{PANEL: RQL comments}

{NOTE: }

__Single-line comments__:  

* Single-line comments start with `//` and end at the end of that line.

{CODE-BLOCK:csharp}
// This is a single-line comment.
from "Companies" 
where Name = "The Big Cheese" OR Name = "Richter Supermarkt"
{CODE-BLOCK/}

{CODE-BLOCK:csharp}
from "Companies"
where Name = "The Big Cheese" // OR Name = "Richter Supermarkt"
{CODE-BLOCK/}

{NOTE/}

{NOTE: }

__Multiline comments__:

* Multiline comments start with `/*` and end with `*/`.

{CODE-BLOCK:csharp}
/*
This is a multiline comment.
Any text here will be ignored.
*/
from "Companies"
where Name = "The Big Cheese" OR Name = "Richter Supermarkt"
{CODE-BLOCK/}

{CODE-BLOCK:csharp}
from "Companies"
where Name = "The Big Cheese" /* this part is a comment */ OR Name = "Richter Supermarkt"
{CODE-BLOCK/}

{NOTE/}

{PANEL/}

## Related Articles

### Client API

- [Query Overview](../../../client-api/session/querying/how-to-query)  
- [How to Use RQL Directly When Querying](../../../client-api/session/querying/how-to-query#session.advanced.rawquery)  

### Querying

- [Querying an index](../../../indexes/querying/query-index)
