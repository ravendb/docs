# Delete by Query Operation
---

{NOTE: }

* Use `DeleteByQueryOperation` to delete a large number of documents that match the provided query in a single server call.

* __Dynamic behavior__:   
  The deletion of documents matching the specified query is run in batches of size 1024.  
  During the deletion process, documents that are added/modified __after__ the delete operation has started  
  may also be deleted if they match the query criteria.

* __Background operation__:  
  This operation is performed in the background on the server.  
  If needed, you can __wait__ for the operation to complete. See: [Wait for completion](../../../client-api/operations/what-are-operations#wait-for-completion).

* In this page:  
   * [Delete by dynamic query](../../../client-api/operations/common/delete-by-query#delete-by-dynamic-query)
   * [Delete by index query](../../../client-api/operations/common/delete-by-query#delete-by-index-query)
   * [Syntax](../../../client-api/operations/common/delete-by-query#syntax)

{NOTE/}

{PANEL: Delete by dynamic query}

{NOTE: }

__Delete all documents in collection__:

{CODE-TABS}
{CODE-TAB:nodejs:DeleteOperation delete_by_query_0@client-api\Operations\Common\deleteByQuery.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Delete with filtering__:  

{CODE-TABS}
{CODE-TAB:nodejs:DeleteOperation delete_by_query_1@client-api\Operations\Common\deleteByQuery.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders" where Freight > 30
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Delete by index query}

* `DeleteByQueryOperation` can only be performed on a __Map-index__.  
  An exception is thrown when executing the operation on a Map-Reduce index.  

* A few overloads are available, see the following examples:

---

{NOTE: }

__A sample Map-index__:

{CODE:nodejs the_index@client-api\Operations\Common\deleteByQuery.js /}

{NOTE/}

{NOTE: }

__Delete documents via an index query__:

{CODE-TABS}
{CODE-TAB:nodejs:DeleteOperation delete_by_query_2@client-api\Operations\Common\deleteByQuery.js /}
{CODE-TAB:nodejs:DeleteOperation_overload delete_by_query_3@client-api\Operations\Common\deleteByQuery.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByPrice" where Price > 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Delete with options__:

{CODE-TABS}
{CODE-TAB:nodejs:DeleteOperation delete_by_query_4@client-api\Operations\Common\deleteByQuery.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByPrice" where Price > 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* Specifying `options` is also supported by the other overload methods, see the Syntax section below.

{NOTE/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@client-api\Operations\Common\deleteByQuery.js /}
<br />

| Parameter         | Type                        | Description                                                |
|-------------------|-----------------------------|------------------------------------------------------------|
| __queryToDelete__ | string                      | The RQL query to perform                                   |
| __queryToDelete__ | `IndexQuery`                | Holds all the information required to query an index       |
| __options__       | `object`                    | Object holding different setting options for the operation |

{CODE:nodejs syntax_2@client-api\Operations\Common\DeleteByQuery.js /}

{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)

### Client API

- [How to Query](../../../client-api/session/querying/how-to-query)

### Querying

- [What is RQL](../../../client-api/session/querying/what-is-rql)
- [Querying an index](../../../indexes/querying/query-index)
