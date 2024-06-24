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
   * [Query an index by `query_index_type` and `query_index`](../../indexes/querying/query-index#query-an-index-by-query_index_type-and-query_index)
   * [Query an index by `raw_query`](../../indexes/querying/query-index#query-an-index-by-raw_query) (using RQL)

{NOTE/}

---

{PANEL: Query an index by `query_index_type` and `query_index`}

* In the following examples we **query an index** using the session `query_index_type` and `query_index` methods.  

* Querying can be enhanced using these [extension methods](../../client-api/session/querying/how-to-query#custom-methods).

---

#### Query index - no filtering:

{CODE-TABS}
{CODE-TAB:python:query_index_type index_query_1_1@Indexes\Querying\QueryIndex.py /}
{CODE-TAB:python:query_index index_query_1_3@Indexes\Querying\QueryIndex.py /}
{CODE-TAB:python:Index the_index@Indexes\Querying\QueryIndex.py /}
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
{CODE-TAB:python:query_index_type index_query_2_1@Indexes\Querying\QueryIndex.py /}
{CODE-TAB:python:Index the_index@Indexes\Querying\QueryIndex.py /}
{CODE-TAB-BLOCK:sql:RQL}
// Note:
// Use slash `/` in the index name, replacing the underscore `_` from the index class definition

from index "Employees/ByName"
where LastName == "King"

// Results will include all documents from 'Employees' collection whose 'LastName' equals to 'King'.
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* `of_type` is used to convert the type being used in the where clause (`IndexEntry`)   
  to the collection type (`Employee`).  
  The reason for this is that while the `IndexEntry` type allows for a strongly typed query,  
  the server returns the actual documents entities objects.

* An exception will be thrown when filtering by fields that are Not defined in the index.

* Read more about filtering [here](../../indexes/querying/filtering).

---

#### Query index - with paging:

{CODE-TABS}
{CODE-TAB:python:query_index_type index_query_3_1@Indexes\Querying\QueryIndex.py /}
{CODE-TAB:python:Index the_index@Indexes\Querying\QueryIndex.py /}
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

{PANEL: Query an index by `raw_query`}

* Queries defined with [Query](../../indexes/querying/query-index#session.query) 
  or [DocumentQuery](../../indexes/querying/query-index#session.advanced.documentquery) 
  are translated by the RavenDB client to [RQL](../../client-api/session/querying/what-is-rql)  
  when sent to the server.

* The session also gives you a way to express the query directly in RQL using the 
  `session.advanced.raw_query` method.

**Example**:

{CODE-TABS}
{CODE-TAB:python:raw_query index_query_5_1@Indexes\Querying\QueryIndex.py /}
{CODE-TAB:python:Index the_index@Indexes\Querying\QueryIndex.py /}
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
