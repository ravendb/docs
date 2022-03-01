# Querying: Exploration Queries
---

{NOTE: }

* **Exploration Queries** form an additional layer of filtering that can be applied 
  to a dataset after its retrieval by [Query](../../client-api/session/querying/how-to-query#session.query), 
  [DocumentQuery](../../client-api/session/querying/how-to-query#session.advanced.documentquery), 
  or [Raw RQL](../../client-api/session/querying/how-to-query#session.advanced.rawquery), 
  while the dataset is still held by the server.  

* An exploration query uses no index, and will never create one.  
  You can use it to explore datasets retrieved by both Index queries and Collection queries.  

* Apply an exploration query using -  
   * `Query.Filter`  
   * `DocumentQuery.Filter`  
   * RQL's `filter` keyword  

* Run an exploration query as a one-time operation when you don't want your filtering to 
  create an index that will then have to be maintained by the cluster.  

* Exploration queries may be a taxing operation, since they scan and filter the entire retrieved dataset.
  {WARNING: }
  We recommend that you **Limit** the number of records that an exploration query filters,  
  and filter data using [where](../../../../indexes/querying/filtering) in reoccuring queries.
  {WARNING/}
  Read more [below](../../indexes/querying/exploration-queries#when-should-exploration-queries-be-used) 
  about using, limiting, and avoiding exploration queries.  

* In this page:  
   * [When Should Exploration Queries Be Used](../../indexes/querying/exploration-queries#when-should-exploration-queries-be-used)
   * [Syntax](../../indexes/querying/exploration-queries#syntax)  
   * [`filter` properties]()  
   * [Exploration Queries and Other RavenDB Features]()  

{NOTE/}

{PANEL: When Should Exploration Queries Be Used}

Exploration queries require no index and will never create one, and can therefore be applied 
to explore the retrieved dataset without fearing that an index would be created and have to be 
maintained from now on by the cluster.  

{WARNING: }
This kind of filtering may, however, be a **taxing operation**, since the entire dataset is 
scanned and filtered. Applying `filter` to the a large dataset, e.g. to the results of 
a collection query like in `from Employees filter Name = 'Jane'`, would burden the server 
with the scanning and filtering of the entire collection and cost the user a substantial 
waiting time.  
It is therefore recommended to -  
    
* Use exploration queries **for one-time explorations** that require no index, 
  and prefer [where](../../indexes/querying/filtering) for repeating queries.  
* **Limit** the number of records that the explorfation query filters (we provide 
  the different `filter` methods with a `limit` option for this purpose).  
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
