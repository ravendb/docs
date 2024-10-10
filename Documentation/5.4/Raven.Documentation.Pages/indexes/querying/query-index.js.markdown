# Querying an Index

---

{NOTE: }

* Prior to this article, it is recommended that you first read this [Query Overview](../../client-api/session/querying/how-to-query).

* For a basic indexes overview, see the [Indexes Overview](../../studio/database/indexes/indexes-overview).

---

* Indexing the content of your documents allows for **fast document retrieval** when querying the index.  
 
* This article is a basic overview of how to query a **static index** using **code**.  
   * For dynamic query examples see [Query Overview](../../client-api/session/querying/how-to-query).
   * An index can also be queried from [Studio](../../studio/database/queries/query-view) 
     using [RQL](../../client-api/session/querying/what-is-rql).

* In this page:  
   * [Query an index by `Query`](../../indexes/querying/query-index#query-an-index-by-query) (using API)
   * [Query an index by `RawQuery`](../../indexes/querying/query-index#query-an-index-by-rawquery) (using RQL)

{NOTE/}

---

{PANEL: Query an index by `Query`}

* The following examples **query an index** using the session's `query` method.  

* Customize your query with these [API methods](../../client-api/session/querying/how-to-query#query-api).

**Query index - no filtering**

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

---

#### Query index - with filtering:

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

* Read more about filtering [here](../../indexes/querying/filtering).

---

#### Query index - with paging:

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

{PANEL/}

{PANEL: Query an index by `RawQuery`}

* Queries defined with [query](../../indexes/querying/query-index#session.query) are translated by the RavenDB client to [RQL](../../client-api/session/querying/what-is-rql) when sent to the server.

* The session also gives you a way to express the query directly in RQL using the `rawQuery` method.

**Example**:

{CODE-TABS}
{CODE-TAB:nodejs:RawQuery index_query_4_1@indexes\querying\queryIndex.js /}
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
