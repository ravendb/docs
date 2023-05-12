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
  * [session.query](../../indexes/querying/query-index#session.query) (using API)
  * [session.advanced.rawQuery](../../indexes/querying/query-index#session.advanced.rawquery) (using RQL)
  * [Query from Studio](../../studio/database/queries/query-view) (using [RQL](../../client-api/session/querying/what-is-rql))

{NOTE/}

---

{PANEL: session.query}

* The following examples __query an index__ using the session's `query` method.  

* Customize your query with these [API methods](../../client-api/session/querying/how-to-query#query-api).

{NOTE: }

__Query index - no filtering__

{CODE-TABS}
{CODE-TAB:nodejs:Query index_query_1_1@indexes\querying\queryIndex.js /}
{CODE-TAB:nodejs:Query_overload index_query_1_2@indexes\querying\queryIndex.js /}
{CODE-TAB:nodejs:Index the_index@indexes\querying\queryIndex.js /}
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
{CODE-TAB:nodejs:Query index_query_2_1@indexes\querying\queryIndex.js /}
{CODE-TAB:nodejs:Query_overload index_query_2_2@indexes\querying\queryIndex.js /}
{CODE-TAB:nodejs:Index the_index@indexes\querying\queryIndex.js /}
{CODE-TAB-BLOCK:sql:RQL}
// Note:
// Use slash `/` in the index name, replacing the underscore `_` from the index class definition

from index "Employees/ByName"
where lastName == "King"

// Results will include all documents from 'Employees' collection whose 'lastName' equals to 'King'.
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* An exception will be thrown when filtering by fields that are Not defined in the index.

{NOTE/}

{NOTE: }

__Query index - with paging__

{CODE-TABS}
{CODE-TAB:nodejs:Query index_query_3_1@indexes\querying\queryIndex.js /}
{CODE-TAB:nodejs:Query_overload index_query_3_2@indexes\querying\queryIndex.js /}
{CODE-TAB:nodejs:Index the_index@indexes\querying\queryIndex.js /}
{CODE-TAB-BLOCK:sql:RQL}
// Note:
// Use slash `/` in the index name, replacing the underscore `_` from the index class definition

from index "Employees/ByName"
where lastName == "King"
limit 5, 10 // skip 5, take 10

{CODE-TAB-BLOCK/}
{CODE-TABS/}

* Read more about paging [here](../../indexes/querying/paging).

{NOTE/}

{PANEL/}

{PANEL: session.advanced.rawQuery}

* Queries defined with [query](../../indexes/querying/query-index#session.query) are translated by the RavenDB client to [RQL](../../client-api/session/querying/what-is-rql) when sent to the server.

* The session also gives you a way to express the query directly in RQL using the `rawQuery` method.

__Example__:

{CODE-TABS}
{CODE-TAB:nodejs:Query index_query_4_1@indexes\querying\queryIndex.js /}
{CODE-TAB:nodejs:Index the_index@indexes\querying\queryIndex.js /}
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
