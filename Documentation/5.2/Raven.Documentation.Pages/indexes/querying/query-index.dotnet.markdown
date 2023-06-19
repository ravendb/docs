# Querying an Index

---

{NOTE: }

* Prior to this article, it is recommended that you first read the [Query Overview](../../client-api/session/querying/how-to-query).  
  For a basic indexes overview see [Indexes Overview](../../studio/database/indexes/indexes-overview).

---

* This article is a basic overview of how to __query a static index__.  
  Indexing the content of your documents allows for __fast document retrieval__ when querying the index.

* Examples in this article show querying an index.  
  For dynamic query examples see [Query Overview](../../client-api/session/querying/how-to-query).

* You can query an index with either of the following:
      * [Session.Query](../../indexes/querying/query-index#session.query) (using LINQ)
      * [Session.Advanced.DocumentQuery](../../indexes/querying/query-index#session.advanced.documentquery) (low-level API)
      * [Session.Advanced.RawQuery](../../indexes/querying/query-index#session.advanced.rawquery) (using RQL)
      * [Query from Studio](../../studio/database/queries/query-view) (using [RQL](../../client-api/session/querying/what-is-rql))

{NOTE/}

---

{PANEL: Session.Query}

* The following examples __query an index__ using the session's `Query` method which supports LINQ.  

* Querying can be enhanced using these [extension methods](../../client-api/session/querying/how-to-query#custom-methods-and-extensions-for-linq).

{NOTE: }

__Query index - no filtering__


{CODE-TABS}
{CODE-TAB:csharp:Query index_query_1_1@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:Query_async index_query_1_2@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:Query_overload index_query_1_3@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:Index the_index@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// Note:
// Use slash `/` in the index name, replacing the underscore `_` from the index class definition

from index "Employees/ByName"

// All 'Employee' documents that contain DOCUMENT-fields 'FirstName' and\or 'LastName' will be returned
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Query index - with filtering__

{CODE-TABS}
{CODE-TAB:csharp:Query index_query_2_1@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:Query_async index_query_2_2@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:Index the_index@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// Note:
// Use slash `/` in the index name, replacing the underscore `_` from the index class definition

from index "Employees/ByName"
where LastName == "King"

// Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'.
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* `OfType` is used to convert the type being used in the where clause (`IndexEntry`)   
  to the collection type (`Employee`).  
  The reason for this is that while the `IndexEntry` type allows for a strongly typed query,  
  the server returns the actual documents entities objects.

* An exception will be thrown when filtering by fields that are Not defined in the index.

* Read more about filtering [here](../../indexes/querying/filtering).

{NOTE/}

{NOTE: }

__Query index - with paging__

{CODE-TABS}
{CODE-TAB:csharp:Query index_query_3_1@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:Query_async index_query_3_2@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:Index the_index@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// Note:
// Use slash `/` in the index name, replacing the underscore `_` from the index class definition

from index "Employees/ByName"
where LastName == "King"
limit 5, 10 // skip 5, take 10

{CODE-TAB-BLOCK/}
{CODE-TABS/}

* Read more about paging [here](../../indexes/querying/paging).

{NOTE/}

{PANEL/}

{PANEL: Session.Advanced.DocumentQuery}

* `DocumentQuery` provides low-level access to RavenDB's querying mechanism,  
  giving you more flexibility and control when making complex queries.

* Below is a simple _DocumentQuery_ usage.  
  For a full description and more examples see:
    * [What is a document query](../../client-api/session/querying/document-query/what-is-document-query)
    * [Query -vs- DocumentQuery](../../client-api/session/querying/document-query/query-vs-document-query)

__Example__:

{CODE-TABS}
{CODE-TAB:csharp:Query index_query_4_1@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:Query_async index_query_4_2@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:Index the_index@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// Note:
// Use slash `/` in the index name, replacing the underscore `_` from the index class definition

from index "Employees/ByName"
where LastName == "King"

{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Session.Advanced.RawQuery}

* Queries defined with [Query](../../indexes/querying/query-index#session.query) or [DocumentQuery](../../indexes/querying/query-index#session.advanced.documentquery) are translated by the RavenDB client to [RQL](../../client-api/session/querying/what-is-rql)  
  when sent to the server.

* The session also gives you a way to express the query directly in RQL using the `RawQuery` method.

__Example__:

{CODE-TABS}
{CODE-TAB:csharp:Query index_query_5_1@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:Query_async index_query_5_2@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:Index the_index@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// Note:
// Use slash `/` in the index name, replacing the underscore `_` from the index class definition

from index "Employees/ByName"
where LastName == "King"

{CODE-TAB-BLOCK/}
{CODE-TABS/}

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

- [Query View](../../studio/database/queries/query-view)

---

### Code Walkthrough

- [Scroll for Queries Section](https://demo.ravendb.net/)
