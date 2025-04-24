# Delete by Query Operation
---

{NOTE: }

* Use `DeleteByQueryOperation` to delete a large number of documents that match the provided query in a single server call.

* **Dynamic behavior**:   
  The deletion of documents matching the specified query is performed in batches of size 1024.  
  During the deletion process, documents that are added/modified **after** the delete operation has started  
  may also be deleted if they match the query criteria.

* **Background operation**:  
  This operation is performed in the background on the server.  

* **Operation scope**:  
  `DeleteByQueryOperation` runs as a single-node transaction, not a cluster-wide transaction. As a result,  
  if you use this operation to delete documents that were originally created using a cluster-wide transaction,  
  their associated [Atomic guards](../../../client-api/session/cluster-transaction/atomic-guards) will Not be deleted.

    * To avoid issues when recreating such documents using a cluster-wide session,
      see [Best practice when storing a document](../../../client-api/session/cluster-transaction/atomic-guards#best-practice-when-storing-a-document-in-a-cluster-wide-transaction).
    * To learn more about the differences between transaction types,
      see [Cluster-wide transaction vs. Single-node transaction](../../../client-api/session/cluster-transaction/overview#cluster-wide-transaction-vs.-single-node-transaction).

---

* In this article:  
   * [Delete by dynamic query](../../../client-api/operations/common/delete-by-query#delete-by-dynamic-query)
   * [Delete by index query](../../../client-api/operations/common/delete-by-query#delete-by-index-query)

{NOTE/}

{PANEL: Delete by dynamic query}

{CONTENT-FRAME: }

##### Delete all documents in a collection

{CODE-TABS}
{CODE-TAB:python:DeleteByQueryOperation delete_by_query_0@ClientApi\Operations\Common\DeleteByQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

##### Delete with filtering  

{CODE-TABS}
{CODE-TAB:python:DeleteByQueryOperation delete_by_query_1@ClientApi\Operations\Common\DeleteByQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders" where Freight > 30
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Delete by index query}

* `DeleteByQueryOperation` can only be performed on a **Map-index**.  
  An exception is thrown when executing the operation on a Map-Reduce index.  

* A few overloads are available, see the following examples:

---

{CONTENT-FRAME: }

##### A sample Map-index

{CODE:python the_index@ClientApi\Operations\Common\DeleteByQuery.py /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

##### Delete documents via an index query

{CODE-TABS}
{CODE-TAB:python:Query delete_by_query_2@ClientApi\Operations\Common\DeleteByQuery.py /}
{CODE-TAB:python:IndexQuery delete_by_query_3@ClientApi\Operations\Common\DeleteByQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByPrice" where Price > 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

##### Delete with options

{CODE-TABS}
{CODE-TAB:python:QueryOperationOptions delete_by_query_6@ClientApi\Operations\Common\DeleteByQuery.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByPrice" where Price > 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* Specifying `QueryOperationOptions` is also supported by the other overload methods, see the Syntax section below.

{CONTENT-FRAME/}
{PANEL/}

## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)

### Client API

- [How to Query](../../../client-api/session/querying/how-to-query)

### Querying

- [What is RQL](../../../client-api/session/querying/what-is-rql)
- [Querying an index](../../../indexes/querying/query-index)
