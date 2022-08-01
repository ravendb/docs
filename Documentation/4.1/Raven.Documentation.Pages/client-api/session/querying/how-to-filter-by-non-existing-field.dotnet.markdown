# Session: Querying: How to Filter by Non-Existing Field  

---

{NOTE: }

* To find documents that do not have a specified field, create an [Auto-Index](../../../indexes/creating-and-deploying#auto-indexes) 
  via a query that calls 
    ```
    WhereExists("a field that exists in all documents")  
    .AndAlso()  
    .Not  
    .WhereExists("a field that doesn't exist in some documents")  
    ```

* The query that creates the auto index must specify a field that exists in every document **and** the field that is missing in some documents.  
  See [RQL example for usage in Studio](../../../client-api/session/querying/how-to-filter-by-non-existing-field#example).

* In this page:  
  * [Usage](../../../client-api/session/querying/how-to-filter-by-non-existing-field#usage)  
  * [Syntax](../../../client-api/session/querying/how-to-filter-by-non-existing-field#syntax)  
  * [Example](../../../client-api/session/querying/how-to-filter-by-non-existing-field#example)  

{NOTE/}

---

{PANEL: Usage}

There may be situations where over time new fields were added to documents.  
You may need to create a list of all of the documents that don't have these fields.  

The following query will create an auto-index for this purpose and list the documents that do not have the specified field.  

You can then write a [patch](../../../client-api/operations/patching/set-based) to add the field.

{PANEL/}

{PANEL: Syntax}


{CODE whereNotexists_signature@ClientApi\Session\Querying\HowToFilterByField.cs /}

| Parameters | Type | Description |
| -- | - | -- |
| **T** | string | The name of the collection that you want to scan. |
| **fieldName** | string | A field in this collection that exists in every document (such as ID, or Name). |
| **fieldName2 (after `.Not`)**| string | The field that is missing in some of the documents. |

{PANEL/}

{PANEL: Example}

{CODE-TABS}
{CODE-TAB:csharp:LINQ whereNotexists_1@ClientApi\Session\Querying\HowToFilterByField.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders" 
where exists("Company") and not exists("Employee")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

#### How to use Studio to filter by non-existing field:

![List Documents Without a Specified Field](images/non-existing-field-studio-rql.png "List Documents Without a Specified Field")

1. **Indexes**  
   Click to see Studio indexing tools.
2. **Query**  
   Select to open the Studio query interface.
3. **Code text editor**  
   Write the query according to the [RQL example](../../../client-api/session/querying/how-to-filter-by-non-existing-field#example) described above.  
4. **Run Code**  
   Click or press ctrl+enter to run the query.
5. **Index used**  
   This is the name of the auto-index created for this query.  
   You can click it to see the Studio indexing tools available for this index.  
6. **Results**  
   This is the list of documents that do not have the specified field.
   (The field "Employee" was explicitly removed from these Northwind documents for this example.)

{PANEL/}

## Related Articles

### Client API

- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)

### Indexes

- [Querying: Filtering](../../../indexes/querying/filtering)
- [Query vs DocumentQuery](../../../indexes/querying/query-vs-document-query)
