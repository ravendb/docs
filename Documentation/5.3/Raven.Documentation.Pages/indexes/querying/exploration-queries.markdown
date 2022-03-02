# Querying: Exploration Queries
---

{NOTE: }

* **Exploration Queries** form an additional layer of filtering that can be applied 
  to a dataset after its retrieval by [Query](../../client-api/session/querying/how-to-query#session.query), 
  [DocumentQuery](../../client-api/session/querying/how-to-query#session.advanced.documentquery), 
  or [Raw RQL](../../client-api/session/querying/how-to-query#session.advanced.rawquery), 
  while the dataset is still held by the server.  

* The **entire retrieved dataset** is scanned and filtered, **without requiring or creating an index**.  
  This makes this type of filtering ideal for one-time explorations, when you want to avoid the creation 
  of an index that would then have to be maintained by the cluster.  

* Apply an exploration query using -  
   * `Query.Filter`  
   * `DocumentQuery.Filter`  
   * RQL's `filter` keyword  

* You can filter the datasets retrieved by both **Index queries** and **Collection queries**.  

* Exploration queries need to be used 
  [catiously](../../indexes/querying/exploration-queries#when-should-exploration-queries-be-used), 
  since the scanning and filtering of all the retrieved data without index makes them 
  a **taxing operation** when large datasets are handled.  
  We recommend that you -  
    * **Limit** the number of records that an exploration query filters.  
    * Use [where](../../indexes/querying/filtering) rather than `filter` in reoccuring queries.  

* In this page:  
   * [When Should Exploration Queries Be Used](../../indexes/querying/exploration-queries#when-should-exploration-queries-be-used)
   * [Syntax](../../indexes/querying/exploration-queries#syntax)  
   * [`filter` properties]()  
   * [Exploration Queries and Other RavenDB Features]()  

{NOTE/}

{PANEL: When Should Exploration Queries Be Used}

`filter` can be applied to a **collection query**, like in:  
`from Employees filter Name = 'Jane'`  

it can also be applied to an **index query**, like in:  
`from Employees as e where e.ReportsTo = 'Central Office' filter e.Address.Country = 'USA'`

In both cases, the **entire retrieved dataset** is scanned and filtered, 
**without using or creating any index**.  
This helps understand when exploration queries should and shouldn't be used.  

{INFO: }
**Use** `filter` for an ad-hoc exploration of the retrieved dataset, that matches 
no existing index and is not expected to be repeated much.  
The dataset will be filtered without creating an unrequired index that the cluster 
would continue updating from now on.  
{INFO/}

Be aware, though, that when a large dataset is retrieved (the whole collection in 
the case of a collection query) exploring it using `filter` would be a **taxing operation**, 
occupy the server and cost the user a substantial waiting time.  

{WARNING: }

* **Limit** the number of records that an explorfation query filters.  
  We provide the different `filter` methods with a `limit` option for this purpose.  
* Use [where](../../indexes/querying/filtering) for reoccuring queries.  
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
     The `filter` keyword, followed by the filter conditions.  
     The `filter_limit` option, followed by the number of records to be filtered.  
   * E.g. -  
     `from Employees as e where e.ReportsTo = 'Central Office filter e.Address.Country = 'USA' filter_limit 500``  

## Usage

`filter` can be placed after a collection query, to scan and filter the entire collection.  
{CODE-TABS}
{CODE-TAB:csharp:Query exploration-queries_1.1@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TAB:csharp:DocumentQuery exploration-queries_1.2@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TAB:csharp:RawQuery exploration-queries_1.3@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TABS/}

{WARNING: }
**Be aware** that in case that a sizable collection is retrieved, the resources that the server 
would invest in its filtering and the delay that users would experience may be considerable.  
It is therefore recommended to [limit]() the number of filtered records.  
{WARNING/}

`filter` can also be placed after a `where` clause, to filter the reults retrieved by an index query.  
{CODE-TABS}
{CODE-TAB:csharp:Query exploration-queries_2.1@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TAB:csharp:DocumentQuery exploration-queries_2.2@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TAB:csharp:RawQuery exploration-queries_2.3@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TABS/}

Returned results can be projected using `select`.  
{CODE-BLOCK:csharp}
from Employees as e
where e.ReportsTo != null
filter e.FirstName == 'Anne' or e.Extension == 5467
select { FullName: e.FirstName +  " " + e.LastName }
{CODE-BLOCK/}

{PANEL/}

{PANEL: Exploration Queries and Other RavenDB Features}

exploration queries and subscriptions


{PANEL/}

## filter - Numeric Property

## filter - Nested Property

## filter + Any

## filter + In

## filter + ContainsAny

## filter + ContainsAll

## filter - StartsWith

## filter - EndsWith

## filter - Identifier Property

## Related Articles

### Indexes

- [Indexing Basics](../../indexes/indexing-basics)

### Querying

- [Basics](../../indexes/querying/basics)
- [Paging](../../indexes/querying/paging)
- [Sorting](../../indexes/querying/sorting)
