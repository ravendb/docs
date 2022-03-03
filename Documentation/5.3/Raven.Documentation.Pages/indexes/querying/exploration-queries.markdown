# Querying: Exploration Queries
---

{NOTE: }

* **Exploration Queries** form an additional layer of filtering that can be applied 
  to a dataset after its retrieval by [Query](../../client-api/session/querying/how-to-query#session.query), 
  [DocumentQuery](../../client-api/session/querying/how-to-query#session.advanced.documentquery), 
  or [Raw RQL](../../client-api/session/querying/how-to-query#session.advanced.rawquery), 
  while the dataset is still held by the server.  

* The **entire retrieved dataset** is scanned and filtered **without requiring or creating an 
  index**, providing a way to conduct one-time explorations without creating an index that would 
  have to be maintained by the cluster.  

* You can filter the datasets retrieved by both **Index queries** and **Collection queries**.  

* Exploration queries need to be used 
  [catiously](../../indexes/querying/exploration-queries#when-should-exploration-queries-be-used), 
  since the scanning and filtering of all the retrieved data without index makes them 
  a **taxing operation** when large datasets are handled.  
  We recommend that you -  
    * **Limit** the number of records that an exploration query filters.  
    * Use [where](../../indexes/querying/filtering) in recurring queries.  

* In this page:  
   * [`filter`](../../indexes/querying/exploration-queries#filter)
   * [When Should Exploration Queries Be Used](../../indexes/querying/exploration-queries#when-should-exploration-queries-be-used)
   * [Syntax](../../indexes/querying/exploration-queries#syntax)
   * [Usage](../../indexes/querying/exploration-queries#usage)
      * [With Collection Queries](../../indexes/querying/exploration-queries#with-collection-queries)
      * [With Index Queries (`where`)](../../indexes/querying/exploration-queries#with-index-queries-)
      * [With Projections (`select`)](../../indexes/querying/exploration-queries#with-projections-)
      * [With User-Defined Javascript Functions (`declare`)](../../indexes/querying/exploration-queries#with-user-defined-javascript-functions-)

{NOTE/}

{PANEL: `filter`}

Exploration queries can be applied using -  

* `Query.Filter`  
* `DocumentQuery.Filter`  
* RQL's `filter` keyword  

The added filtering is parsed and executed by RavenDB's Javascript engine, 
and can be implemented wherever Javascript is used, including within 
Map/Reduce indexes and by subscription workers.  

The filtering conditions are basically similar to those implemented by 
[where](../../indexes/querying/filtering), and can be further enhanced 
by Javascript functions of your own.  
Read [here](../../indexes/querying/exploration-queries#with-a-javascript-function:-declare) 
about creating and using your own Javascript function in your filters.  

{PANEL/}

{PANEL: When Should Exploration Queries Be Used}

`filter` can be applied to a Collection query, like in:  
{CODE-BLOCK: javascript}
from Employees as e 
filter e.Address.Country = 'USA'
{CODE-BLOCK/}

it can also be applied to an Index query, like in:  
{CODE-BLOCK: javascript}
from Employees as e 
where e.Title = 'Sales Representative'  
filter e.Address.Country = 'USA'
{CODE-BLOCK/}

In both cases, the entire retrieved dataset is scanned and filtered, 
without using or creating an index.  
This helps understand when exploration queries Should and should Not be used:  

{INFO: }
**Use** `filter` for an ad-hoc exploration of the retrieved dataset, that matches 
no existing index and is not expected to be repeated much.  
The dataset will be filtered without creating an unrequired index that the cluster 
would continue updating from now on.  
{INFO/}
{WARNING: }
Be aware, though, that when a large dataset is retrieved, like the whole collection in 
the case of a collection query, exploring it all using `filter` would be taxing for the 
server and cost the user a substantial waiting time.  

* **Limit** the number of records that an exploration query filters, e.g. -  
  {CODE-BLOCK: javascript}
  from Employees as e 
where e.Title = 'Sales Representative'  
filter e.Address.Country = 'USA'
filter_limit 500 // limit the number of filtered records
  {CODE-BLOCK/}
* Use [where](../../indexes/querying/filtering) rather than `filter` for recurring filtering.  
  `where` will use an index, creating it if necessary, to accelerate the filtering 
  in subsequent queries.  
{WARNING/}

{PANEL/}

{PANEL: Syntax}

* `Query.Filter`  
  {CODE-BLOCK:csharp}  
  IRavenQueryable<T> Filter<T>(this IRavenQueryable<T> source, 
                             Expression<Func<T, bool>> predicate, 
                             int limit = int.MaxValue);
  {CODE-BLOCK/}  

    | Parameters | Type | Description |
    | ---------- | ---- | ----------- |
    | **source** | `IRavenQueryable<T>` | Query source |
    | **predicate** | `Expression<Func<T, bool>>` | The condition by which retrieved records are filtered |
    | **limit** | `int ` | Limits the number of filtered records (Recommended) <br> Default: all retrieved records |

* `DocumentQuery.Filter`  
  {CODE-BLOCK:csharp}  
  IDocumentQuery<T> Filter(Action<IFilterFactory<T>> builder, 
                         int limit = int.MaxValue);
  {CODE-BLOCK/}  

    | Parameters | Type | Description |
    | ---------- | ---- | ----------- |
    | **builder** | `Action<IFilterFactory<T>>` | Your filter |
    | **limit** | `int ` | Limits the number of filtered records (Recommended) <br> Default: all retrieved records |

* **RQL**  
   * In an RQL query, use:  
     The `filter` keyword, followed by the filtering condition.  
     The `filter_limit` option, followed by the max number of records to filter.  
   * E.g. -  
     {CODE-BLOCK: javascript}
     from Employees as e 
where e.Title = 'Sales Representative'  
filter e.Address.Country = 'USA' // filter the retrieved dataset
filter_limit 500 // limit the number of filter records
     {CODE-BLOCK/}
{PANEL/}

{PANEL: Usage}

### With Collection Queries

Use `filter` with a collection query to scan and filter the entire collection.  
{CODE-TABS}
{CODE-TAB:csharp:Query exploration-queries_1.1@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TAB:csharp:DocumentQuery exploration-queries_1.2@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TAB:csharp:RawQuery exploration-queries_1.3@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TABS/}

{WARNING: }
Filtering a sizable collection will burden the server and prolong user waiting time.  
Set a `limit` to restrict the number of filtered records.  
{WARNING/}

---

### With Index Queries (`where`)

Use `filter` after a `where` clause to filter the reults retrieved by an index query.  
{CODE-TABS}
{CODE-TAB:csharp:Query exploration-queries_2.1@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TAB:csharp:DocumentQuery exploration-queries_2.2@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TAB:csharp:RawQuery exploration-queries_2.3@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TABS/}

---

### With Projections (`select`)

The filtered results can be projected using `select`.  
{CODE-TABS}
{CODE-TAB:csharp:Query exploration-queries_3.1@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TAB:csharp:DocumentQuery exploration-queries_3.2@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TAB:csharp:RawQuery exploration-queries_3.3@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TABS/}

---

### With User-Defined Javascript Functions (`declare`)

You can define a Javascript function as part of your query using the 
[declare]()../../indexes/querying/what-is-rql#declare) keyword, and 
use it as part of your `filter` condition to freely adapt the filtering 
to your needs.  

Here is a simple example:  
{CODE-BLOCK: javascript}
$prefix = "Sales"

// declare a Javascript function
declare function titlePrefix(r, prefix) 
{ 
    // Add whatever filtering capabilities you like
    return r.Title.startsWith(prefix)
} 

from Employees as e 
where startsWith(e.HiredAt, '1993')

// Filter using the function you've declared
filter titlePrefix(e, $prefix)
filter_limit 100

{CODE-BLOCK/}

{PANEL/}

### Querying

- [What is RQL](../../indexes/querying/what-is-rql)  
- [Query](../../client-api/session/querying/how-to-query#session.query)  
- [DocumentQuery](../../client-api/session/querying/how-to-query#session.advanced.documentquery)  
- [Raw RQL](../../client-api/session/querying/how-to-query#session.advanced.rawquery)  
- [Where](../../indexes/querying/filtering)  
- [Basics](../../indexes/querying/basics)  
- [Sorting](../../indexes/querying/sorting)  
- [declare](../../indexes/querying/what-is-rql#declare)  
