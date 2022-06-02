# Querying: Basics
---

{NOTE: }

* Queries can either be written in the [Session with LINQ syntax](../../client-api/session/querying/how-to-query) 
  or in the [Studio with RQL](../../studio/database/queries/query-view).  
  There are examples for both below.

* Indexes are used by RavenDB to satisfy queries. 

* To accelerate queries, RavenDB [indexes](../../indexes/creating-and-deploying) can process various calculations, filters and conversions behind the scenes 
  so that the data is already processed and ready for queries.  
  Indexes keep the processed data in a separate storage so that the raw data isn't affected.  
  Furthermore, indexes only scan and process the entire specified dataset once.  
  After the initial scan, they only need to process specific data as it is modified, added or deleted.
   * For queries to use an index that has already processed the data, the index must be [called in the query](../../indexes/querying/basics#example-iv---querying-a-specified-index).

* In this page:
   * [Query-Flow](../../indexes/querying/basics#query-flow)
   * [Querying Using LINQ](../../indexes/querying/basics#querying-using-linq)
      * [Example I - Querying an Entire Collection](../../indexes/querying/basics#example-i---querying-an-entire-collection)
      * [Example II - Filtering](../../indexes/querying/basics#example-ii---filtering)
      * [Example III - Paging](../../indexes/querying/basics#example-iii---paging)
      * [Example IV - Querying a Specified Index](../../indexes/querying/basics#example-iv---querying-a-specified-index)
      * [Low-Level Query Access](../../indexes/querying/basics#low-level-query-access)

{NOTE/}

{PANEL: Query-Flow}

Queries in RavenDB can be defined in Studio with [RQL](../../indexes/querying/what-is-rql), our query language, or in the [Session with LINQ syntax](../../client-api/session/querying/how-to-query). 
Each query must match an index in order to return the results. If no index exists to satisfy the query and if a specific index isn't specified, 
an Auto-Index will be created automatically.  

The full query flow is as follows:

1. `from index | collection` 
  - First step. When a query is issued, it locates the appropriate index. 
    If our query specifies that index, the task is simple - use this index. 
    Otherwise, a query analysis takes place and an auto-index is created if no auto-index can already satisfy the query.

2. `where` 
  - When we have our index, we scan it for records that match the query predicate.

3. `load`
  - If a query contains a projection that requires any document loads to be processed, they are done just before projection is executed.

3. `select`
  - From each record, the server extracts the appropriate fields. It always extracts the `id()` field ([stored](../../indexes/storing-data-in-index) by default).   

  - If a query is not a projection query, then we load a document from storage. Otherwise, if we stored all requested fields in the index, we use them and continue. If not, the document is loaded from storage and the missing fields are fetched from it.

  - If a query indicates that [projection](../../indexes/querying/projections) should be used, then all results that were not filtered out are processed by that projection. Fields defined in the projection are extracted from the index (if stored).

4. `include` 
  - If any [includes](../../client-api/how-to/handle-document-relationships#includes) are defined, then the query also extracts data from potential related documents to include with the results.

5. (LINQ syntax) `ToList` or `ToListAsync`
  - Return results.

{PANEL/}

{PANEL: Querying Using LINQ}

RavenDB Client supports querying using LINQ. This functionality can be accessed using the session `Query` method, and is the most common and basic method for querying the database.

### Example I - Querying an Entire Collection

Let's execute our first query and return all the employees from the Northwind database. 
To do that, we need to have a [document store](../../client-api/what-is-a-document-store) with an [open session](../../client-api/session/opening-a-session), 
and specify a [collection](../../client-api/faq/what-is-a-collection) 
that we want to query (in our case `Employees`) by passing `Employee` as a generic parameter to the `Query` method:

{CODE-TABS}
{CODE-TAB:csharp:Sync basics_0_0@Indexes\Querying\Basics.cs /}
{CODE-TAB:csharp:Async basics_1_0@Indexes\Querying\Basics.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees
{CODE-TAB-BLOCK/}
{CODE-TABS/}

By specifying `Employee` as a type parameter, we are also defining a result type.

### Example II - Filtering

To filter the results, use the suitable LINQ method, like `Where`:

{CODE-TABS}
{CODE-TAB:csharp:Sync basics_0_1@Indexes\Querying\Basics.cs /}
{CODE-TAB:csharp:Async basics_1_1@Indexes\Querying\Basics.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees
where FirstName = 'Robert'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:csharp:Sync basics_3_0@Indexes\Querying\Basics.cs /}
{CODE-TAB:csharp:Async basics_3_1@Indexes\Querying\Basics.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees
where id() = 'employees/1-A'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

You can read more about filtering [here](../../indexes/querying/filtering).

### Example III - Paging

Paging is very simple. The methods `Take` and `Skip` can be used:

{CODE-TABS}
{CODE-TAB:csharp:Sync basics_0_2@Indexes\Querying\Basics.cs /}
{CODE-TAB:csharp:Async basics_1_2@Indexes\Querying\Basics.cs /}
{CODE-TABS/}

You can read more about paging [here](../../indexes/querying/paging).

### Example IV - Querying a Specified Index

In the above examples, we **did not** specify an index that we want to query. 
If you don't specify an index, RavenDB will look for an appropriate auto-index or create a new one. 
You can read more about creating indexes [here](../../indexes/creating-and-deploying).

**To query a static index, you must specify the index in the query definition.**

In order to specify an index, we need to pass it as a second generic parameter to the `Query` method 
or pass the index name as a parameter.

In this example, the index name is `Employees_ByFirstName` if written for LINQ or `Employees/ByFirstName` if written for RQL.

{CODE-TABS}
{CODE-TAB:csharp:Sync basics_0_3@Indexes\Querying\Basics.cs /}
{CODE-TAB:csharp:Async basics_1_3@Indexes\Querying\Basics.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstName' 
where FirstName = 'Robert'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:csharp:Sync basics_0_4@Indexes\Querying\Basics.cs /}
{CODE-TAB:csharp:Async basics_1_4@Indexes\Querying\Basics.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstName' 
where FirstName = 'Robert'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{INFO:Remember}
If you are filtering by fields that are not present in an index, an exception will be thrown.
{INFO/}

## Low-Level Query Access

To take full control over your queries, we introduced a `DocumentQuery` method that is available in advanced session operations. It is a low-level access to the querying mechanism the user can take advantage of to shape queries according to his needs.

### Example

{CODE-TABS}
{CODE-TAB:csharp:Sync basics_2_0@Indexes\Querying\Basics.cs /}
{CODE-TAB:csharp:Async basics_2_1@Indexes\Querying\Basics.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstName' 
where FirstName = 'Robert'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Remarks

{INFO You can check the API reference for the `DocumentQuery` [here](../../client-api/session/querying/document-query/what-is-document-query). /}

{INFO There are some differences between `Query` and `DocumentQuery`. They are described in [this article](../../indexes/querying/query-vs-document-query). /}

{PANEL/}

## Related Articles

### Indexes

- [Indexing Basics](../../indexes/indexing-basics)
- [Creating and Deploying Indexes](../../indexes/creating-and-deploying)

### Querying

- [Filtering](../../indexes/querying/filtering)
- [Paging](../../indexes/querying/paging)
- [Projections](../../indexes/querying/projections)
- [Sorting](../../indexes/querying/sorting)

### Client API

- [What is a Document Store](../../client-api/what-is-a-document-store)
- [Opening a Session](../../client-api/session/opening-a-session)
- [How to Handle Document Relationships](../../client-api/how-to/handle-document-relationships)

### Studio

- [Studio: Querying](../../studio/database/queries/query-view)

---

### Code Walkthrough

- [Scroll for Queries Section](https://demo.ravendb.net/)
