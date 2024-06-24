# Querying: Basics

Indexes are used by RavenDB to satisfy queries.

## Query-Flow

Each query in RavenDB must be expressed by [RQL](../../client-api/session/querying/what-is-rql), our query language. Each query must match an index in order to return the results. The full query flow is as follows:

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

## Querying

RavenDB Client supports querying for data. This functionality can be accessed using the session `query` method, and is the most common and basic method for querying the database.

### Example I

Let's execute our first query and return all the employees from the Northwind database. To do that, we need to have a [document store](../../client-api/what-is-a-document-store) and [opened session](../../client-api/session/opening-a-session) and specify a [collection](../../client-api/faq/what-is-a-collection) type that we want to query (in our case `Employees`) by passing `Employee` as a first parameter to the `query` method:

{CODE-TABS}
{CODE-TAB:java:Java basics_0_0@Indexes\Querying\QueryIndex.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees
{CODE-TAB-BLOCK/}
{CODE-TABS/}

By specifying `Employee` class as a parameter, we are also defining a result type.

### Example II - Filtering

To filter the results, use the suitable method, like `whereEquals`:

{CODE-TABS}
{CODE-TAB:java:Java basics_0_1@Indexes\Querying\QueryIndex.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees
where FirstName = 'Robert'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:java:Java basics_3_0@Indexes\Querying\QueryIndex.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees
where id() = 'employees/1-A'
{CODE-TAB-BLOCK/}
{CODE-TABS/}


You can read more about filtering [here](../../indexes/querying/filtering).

### Example III - Paging

Paging is very simple. The methods `take` and `skip` can be used:

{CODE:java basics_0_2@Indexes\Querying\QueryIndex.java /}

You can read more about paging [here](../../indexes/querying/paging).

### Example IV - Querying a Specified Index

In the above examples, we **did not** specify an index that we want to query. RavenDB will try to locate an appropriate index or create a new one. You can read more about creating indexes [here](../../indexes/creating-and-deploying).

In order to specify an index, we need to pass it as a second parameter to the `query` method or pass the index name as a parameter.

{CODE-TABS}
{CODE-TAB:java:Java basics_0_3@Indexes\Querying\QueryIndex.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstName' 
where FirstName = 'Robert'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:java:Java basics_0_4@Indexes\Querying\QueryIndex.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFirstName' 
where FirstName = 'Robert'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{INFO:Remember}
If you are filtering by fields that are not present in an index, an exception will be thrown.
{INFO/}

### Remarks

{INFO You can check the API reference for the `DocumentQuery` [here](../../client-api/session/querying/document-query/what-is-document-query). /}

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
