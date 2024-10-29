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

* In this page:  
   * [Delete by dynamic query](../../../client-api/operations/common/delete-by-query#delete-by-dynamic-query)
   * [Delete by index query](../../../client-api/operations/common/delete-by-query#delete-by-index-query)
   * [Syntax](../../../client-api/operations/common/delete-by-query#syntax)

{NOTE/}

{PANEL: Delete by dynamic query}

#### Delete all documents in a collection:

{CODE-TABS}
{CODE-TAB:php:DeleteByQueryOperation delete_by_query_0@ClientApi\Operations\Common\DeleteByQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Delete with filtering:  

{CODE-TABS}
{CODE-TAB:php:DeleteByQueryOperation delete_by_query_1@ClientApi\Operations\Common\DeleteByQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders" where Freight > 30
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Delete by index query}

* `DeleteByQueryOperation` can only be performed on a **Map-index**.  
  An exception is thrown when executing the operation on a Map-Reduce index.  

* A few overloads are available, see the following examples:

---

#### A sample Map-index:

{CODE:php the_index@ClientApi\Operations\Common\DeleteByQuery.php /}

---

#### Delete documents via an index query:

{CODE-TABS}
{CODE-TAB:php:RQL delete_by_query_2@ClientApi\Operations\Common\DeleteByQuery.php /}
{CODE-TAB:php:IndexQuery delete_by_query_3@ClientApi\Operations\Common\DeleteByQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByPrice" where Price > 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### Delete with options:

{CODE-TABS}
{CODE-TAB:php:QueryOperationOptions delete_by_query_6@ClientApi\Operations\Common\DeleteByQuery.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByPrice" where Price > 10
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax_1@ClientApi\Operations\Common\DeleteByQuery.php /}
<br />

| Parameter         | Type                      | Description              |
|-------------------|---------------------------|--------------------------|
| **$queryToDelete** | `string` | The RQL query to perform |
| **$queryToDelete** | `IndexQuery` | Holds all the information required to query an index |
| **$options** | `?QueryOperationOptions` | Object holding different setting options for the operation |

{CODE:php syntax_2@ClientApi\Operations\Common\DeleteByQuery.php /}

{PANEL/}


## Related Articles

### Operations

- [What are Operations](../../../client-api/operations/what-are-operations)

### Client API

- [How to Query](../../../client-api/session/querying/how-to-query)

### Querying

- [What is RQL](../../../client-api/session/querying/what-is-rql)
- [Querying an index](../../../indexes/querying/query-index)
