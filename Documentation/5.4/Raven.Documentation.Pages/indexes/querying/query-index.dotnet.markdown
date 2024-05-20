# Querying an Index

---

{NOTE: }

* Prior to reading this article, it is recommended to take a look at the 
  [Query Overview](../../client-api/session/querying/how-to-query).  
  
* For a basic indexes overview, see the [Indexes Overview](../../studio/database/indexes/indexes-overview).

---

* Indexing the content of your documents allows for **fast document retrieval** when querying the index.  

* This article is a basic overview of how to query a **static index** using **code**.  
   * For dynamic query examples see [Query Overview](../../client-api/session/querying/how-to-query).  
   * An index can also be queried from [Studio](../../studio/database/queries/query-view) 
     using [RQL](../../client-api/session/querying/what-is-rql).

* In this page:  
   * [Query an index by `Query`](../../indexes/querying/query-index#query-an-index-by-query) (using LINQ)
   * [Query an index by `DocumentQuery`](../../indexes/querying/query-index#query-an-index-by-documentquery) (low-level API)
   * [Query an index by `RawQuery`](../../indexes/querying/query-index#query-an-index-by-rawquery) (using RQL)

{NOTE/}

---

{PANEL: Query an index by `Query`}

* In the following examples we **query an index** using the session's `Session.Query` method, which supports LINQ.  

* Querying can be enhanced using these [extension methods](../../client-api/session/querying/how-to-query#custom-methods-and-extensions-for-linq).

---

#### Query index - no filtering:

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

---

#### Query index - with filtering:

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

---

#### Query index - with paging:

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

{PANEL/}

{PANEL: Query an index by `DocumentQuery`}

* `Session.Advanced.DocumentQuery` provides low-level access to RavenDB's querying mechanism,  
  giving you more flexibility and control when making complex queries.

* For more information about _DocumentQuery_ see:
    * [What is a document query](../../client-api/session/querying/document-query/what-is-document-query)
    * [Query -vs- DocumentQuery](../../client-api/session/querying/document-query/query-vs-document-query)

**Example**:

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery index_query_4_1@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:DocumentQuery_async index_query_4_2@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:Overload index_query_4_3@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:Overload_async index_query_4_4@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:Index the_index@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// Note:
// Use slash `/` in the index name, replacing the underscore `_` from the index class definition

from index "Employees/ByName"
where LastName == "King"

{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Query an index by `RawQuery`}

* Queries defined with [Query](../../indexes/querying/query-index#session.query) 
  or [DocumentQuery](../../indexes/querying/query-index#session.advanced.documentquery) 
  are translated by the RavenDB client to [RQL](../../client-api/session/querying/what-is-rql)  
  when sent to the server.

* The session also gives you a way to express the query directly in RQL using the 
  `Session.Advanced.RawQuery` method.

**Example**:

{CODE-TABS}
{CODE-TAB:csharp:RawQuery index_query_5_1@Indexes\Querying\QueryIndex.cs /}
{CODE-TAB:csharp:RawQuery_async index_query_5_2@Indexes\Querying\QueryIndex.cs /}
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
