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
{CODE-TAB:csharp:DeleteOperation_Sync delete_by_query_0@ClientApi\Operations\Common\DeleteByQuery.cs /}
{CODE-TAB:csharp:DeleteOperation_Async delete_by_query_0_async@ClientApi\Operations\Common\DeleteByQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Delete with filtering__:  

{CODE-TABS}
{CODE-TAB:csharp:DeleteOperation_Sync delete_by_query_1@ClientApi\Operations\Common\DeleteByQuery.cs /}
{CODE-TAB:csharp:DeleteOperation_Async delete_by_query_1_async@ClientApi\Operations\Common\DeleteByQuery.cs /}
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

{CODE the_index@ClientApi\Operations\Common\DeleteByQuery.cs /}

{NOTE/}

{NOTE: }

__Delete documents via an index query__:

{CODE-TABS}
{CODE-TAB:csharp:DeleteOperation delete_by_query_2@ClientApi\Operations\Common\DeleteByQuery.cs /}
{CODE-TAB:csharp:Overload_1 delete_by_query_3@ClientApi\Operations\Common\DeleteByQuery.cs /}
{CODE-TAB:csharp:Overload_2 delete_by_query_4@ClientApi\Operations\Common\DeleteByQuery.cs /}
{CODE-TAB:csharp:Overload_3 delete_by_query_5@ClientApi\Operations\Common\DeleteByQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByPrice" where Price > 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Delete with options__:

{CODE-TABS}
{CODE-TAB:csharp:DeleteOperation delete_by_query_6@ClientApi\Operations\Common\DeleteByQuery.cs /}
{CODE-TAB:csharp:DeleteOperation_async delete_by_query_6_async@ClientApi\Operations\Common\DeleteByQuery.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByPrice" where Price > 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* Specifying `QueryOperationOptions` is also supported by the other overload methods, see the Syntax section below.

{NOTE/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@ClientApi\Operations\Common\DeleteByQuery.cs /}
<br />

| Parameter         | Type                        | Description                                                |
|-------------------|-----------------------------|------------------------------------------------------------|
| __queryToDelete__ | string                      | The RQL query to perform                                   |
| __queryToDelete__ | `IndexQuery`                | Holds all the information required to query an index       |
| __indexName__     | string                      | The name of the index queried                              |
| __expression__    | `Expression<Func<T, bool>>` | The expression that defines the query criteria             |
| __options__       | `QueryOperationOptions`     | Object holding different setting options for the operation |

{CODE syntax_2@ClientApi\Operations\Common\DeleteByQuery.cs /}

{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)

### Client API

- [How to Query](../../../client-api/session/querying/how-to-query)

### Querying

- [What is RQL](../../../client-api/session/querying/what-is-rql)
- [Querying an index](../../../indexes/querying/query-index)
