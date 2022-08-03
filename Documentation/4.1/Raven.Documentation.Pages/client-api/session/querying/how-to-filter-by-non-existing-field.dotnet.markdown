# Session: Querying: 
# How to Filter by Non-Existing Field  

---

{NOTE: }

* There are situations where over time new fields are added to documents.  
  You may need to create a list of all of the documents that don't have these fields.  
   * You can then write a [patch](../../../client-api/operations/patching/set-based#update-by-static-index-query-result) 
     to add the missing fields.

* To find documents with a missing field you can either:
   * [Query a Static Index](../../../client-api/session/querying/how-to-filter-by-non-existing-field#query-a-static-index)  
   * [Query the collection to create an Auto Index](../../../client-api/session/querying/how-to-filter-by-non-existing-field#query-the-collection-to-create-an-auto-index) 
   * [Use Studio to filter by non-existing field](../../../client-api/session/querying/how-to-filter-by-non-existing-field#how-to-use-studio-to-filter-by-non-existing-field)  


{NOTE/}

---

{PANEL: Query a Static Index}

You can search for documents with missing fields by using a static index if it indexes the field which is 
suspected to be missing in some of the documents.  

The index definition must also index a field that exists in every document (such as `Id`) so that it will include every document in the collection.  

* For example, if you want to find documents that are missing the field `Freight` in the `Orders` collection,  
  query an index that indexes the fields `Freight` and `Id`. 
* If your static index does not contain the desired field, either
   * Modify your index definition to index the specific field.  (This will trigger re-indexing.)
   * [Create an auto-index](../../../client-api/session/querying/how-to-filter-by-non-existing-field#query-the-collection-to-create-an-auto-index). 
     
### Example: Query a Static Index

In our example, we are looking for documents that are missing the field `Freight` from the collection `Orders`.  

#### First we need an index that includes `Freight` and a field that exists in every document

We index the missing field `Freight` and the field `Id`, which exists in every document. 
This way, the index includes all of the documents in the collection, 
including those that are missing the specified field.

{CODE IndexwhereNotexists_example@ClientApi\Session\Querying\HowToFilterByField.cs /}

#### Then we query the index to find documents where the field does not exist

SYNTAX

{CODE whereNotexists_StaticSignature@ClientApi\Session\Querying\HowToFilterByField.cs /}

| Parameters | Type | Description |
| -- | - | -- |
| **T** | string | The type of object that you want to search. |
| **TIndexCreator** | string | The name of the index that you want to use. |
| **fieldName**| string | The field that is missing in some of the documents. |

QUERY SAMPLE CODE

Query the index `Orders_ByFreight` and filter documents where `freight` does not exist.  

{CODE-TABS}
{CODE-TAB:csharp:LINQ QuerywhereNotexists_example@ClientApi\Session\Querying\HowToFilterByField.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders" 
where true and not exists("Freight")
// `not` cannot come immediately after `where`, thus we use `where true`.
{CODE-TAB-BLOCK/}
{CODE-TABS/}






{PANEL/}

{PANEL: Query the Collection to Create an Auto-Index}

Another option is to query the collection, which creates an auto-index to satisfy the query.  
By including a filter in the query with the keyword `where`,  
we first make sure that our index includes all documents in that collection.  
Then we use the keywords `not exists()` and specify the field that does not exist in some of the documents.

### Example: A query that creates an Auto-Index

The following query will create an auto-index on the field that is missing in some documents of a specified collection.  
It then lists the documents that do not have the specified field.  

{CODE-TABS}
{CODE-TAB:csharp:LINQ whereNotexists_1@ClientApi\Session\Querying\HowToFilterByField.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders" 
where true and not exists("Freight")
// `not` cannot be used immediately after `where`, thus we use `where true`.
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### LINQ Query Syntax

{CODE whereNotexists_signature@ClientApi\Session\Querying\HowToFilterByField.cs /}

| Parameters | Type | Description |
| -- | - | -- |
| **T** | string | The type of object that you want to search. |
| **fieldName**| string | The field that is missing in some of the documents. |


{PANEL/}


{PANEL: How to use Studio to filter by non-existing field}

![List Documents Without a Specified Field](images/non-existing-field-studio-rql.png "List Documents Without a Specified Field")

1. **Indexes**  
   Click to see Studio indexing tools.
2. **Query**  
   Select to open the Studio query interface.
3. **Code text editor**  
   Write the query according to the [RQL example](../../../client-api/session/querying/how-to-filter-by-non-existing-field#example-a-query-that-creates-an-auto-index) described above.  
4. **Run Code**  
   Click or press ctrl+enter to run the query.
5. **Index used**  
   This is the name of the auto-index created for this query.  
   You can click it to see the Studio indexing tools available for this index.  
6. **Results**  
   This is the list of documents that do not have the specified field.
   (The field "Freight" was explicitly removed from these Northwind documents for this example.)

{PANEL/}

## Related Articles

### Client API

- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)
- [How to Filter by Field Presence](../../../client-api/session/querying/how-to-filter-by-field)

### Indexes

- [Querying: Filtering](../../../indexes/querying/filtering)
- [Query vs DocumentQuery](../../../indexes/querying/query-vs-document-query)
