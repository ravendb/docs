# Querying: Basics

Indexes are used by RavenDB to satisfy queries.

## Query-Flow

Each query in RavenDB must be expressed by [RQL](../../indexes/querying/what-is-rql), our query language. Each query must match an index in order to return the results. The full query flow is as follows:

1. `from index | collection` 
  - First step. When a query is issued, it locates the appropriate index. If our query specifies that index, the task is simple - use this index. Otherwise, a query analysis takes place and an auto-index is created.

2. `where` 
  - When we have our index, we scan it for records that match the query predicate.

3. `load`
  - If a query contains a projection that requires any document loads to be processed, they are done just before projection is executed.

3. `select`
  - From each record, the server extracts the appropriate fields. It always extracts the `id()` field ([stored](../../indexes/storing-data-in-index) by default).   

  - If a query is not a projection query, then we load a document from storage. Otherwise, if we stored all requested fields in the index, we use them and continue. If not, the document is loaded from storage and the missing fields are fetched from it.

  - If a query indicates that [projection](../../indexes/querying/projections) should be used, then all results that were not filtered out are processed by that projection. Fields defined in the projection are extracted from the index (if stored).

4. `include` 
  - If any [includes](../../client-api/how-to/handle-document-relationships#includes) are defined, then the results are being traversed to extract the IDs of potential documents to include with the results.

5. Return results.

## Querying Using LINQ

RavenDB Client supports querying using LINQ. This functionality can be accessed using the session `Query` method, and is the most common and basic method for querying the database.

### Example I

Let's execute our first query and return all the employees from the Northwind database. To do that, we need to have a [document store](../../client-api/what-is-a-document-store) and [opened session](../../client-api/session/opening-a-session) and specify a [collection](../../client-api/faq/what-is-a-collection) type that we want to query (in our case `Employees`) by passing `Employee` as a generic parameter to the `Query` method:

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

In the above examples, we **did not** specify an index that we want to query. RavenDB will try to locate an appropriate index or create a new one. You can read more about creating indexes [here](../../indexes/creating-and-deploying).

In order to specify an index, we need to pass it as a second generic parameter to the `Query` method or pass the index name as a parameter.

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

## Related Articles

### Indexes

- [Indexing Basics](../../indexes/indexing-basics)

### Querying

- [Filtering](../../indexes/querying/filtering)
- [Paging](../../indexes/querying/paging)

### Client API

- [What is a Document Store](../../client-api/what-is-a-document-store)
- [Opening a Session](../../client-api/session/opening-a-session)
- [How to Handle Document Relationships](../../client-api/how-to/handle-document-relationships)
