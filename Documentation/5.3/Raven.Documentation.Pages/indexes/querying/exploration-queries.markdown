# Querying: Exploration Queries
---

{NOTE: }

* An **Exploration Query** is an additional layer of filtering that can be applied 
  to the results returned by a query, using the `filter` keyword.  

* `filter` can be used with -  
   * [Query](../../client-api/session/querying/how-to-query#session.query)  
   * [DocumentQuery](../../client-api/session/querying/how-to-query#session.advanced.documentquery)  
   * [Raw RQL](../../client-api/session/querying/how-to-query#session.advanced.rawquery)  

* Exploration queries can filter results from -  
   * an **index query** (retrieved using [where](../../indexes/querying/filtering)), e.g. -  
     `from Employees as e where e.ReportsTo = 'Central Office`  
     `filter e.Address.Country = 'USA'`  
   * a **collection query**, e.g. -  
     `from Employees`  
     `filter Name = 'Jane'`  

* An exploration query is performed by the server **over the dataset** retrieved 
  by a query. It requires no index, and will never create one.  
  You can therefore apply such filters without fearing that the cluster 
  would have to continue maintaining indexes created as part of their execution.  
  **However**, an exploration query is a **taxing operation**. It filters the 
  entire dataset retrieved by a query, and in the case of a large dataset the 
  client might experience a substantial delay before the results arrive.  
  Such is the case, for example, when a filter is applied to a collection query:  
  The entire collection will be scanned before the results are returned.  

{WARNING: }
* Apply an exploration query with caution, as a one-time operation and 
  **not** as an ongoing substitute for the much cheaper `where` filtering.  
* We recommend that you [limit]() the number of records filtered by 
  an exploration query.  
{WARNING/}  

* In this page:  
   * [`filter`](../../indexes/querying/exploration-queries#filter)  
   * [`filter` properties]()  

{NOTE/}

---

{PANEL: `filter`}

`filter` can be placed after a collection query, forcing a full scan of the retrieved collection.  
{CODE-TABS}
{CODE-TAB:csharp:Query exploration-queries_1.1@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TAB:csharp:DocumentQuery exploration-queries_1.2@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TAB:csharp:RawQuery exploration-queries_1.3@Indexes\Querying\ExplorationQueries.cs /}
{CODE-TABS/}
  
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
