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
   * [Use Studio to filter by non-existing field](../../../client-api/session/querying/how-to-filter-by-non-existing-field#use-studio-to-filter-by-non-existing-field)  


{NOTE/}

---

{PANEL: Query a Static Index}

You can search for documents with missing fields by using a static index if it indexes the field which is 
suspected to be missing in some of the documents.  

The index definition must also index a field that exists in every document (such as `Id`) so that all documents will be indexed.  

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

LINQ SYNTAX:

{CODE whereNotexists_StaticSignature@ClientApi\Session\Querying\HowToFilterByField.cs /}

| Parameters | Type | Description |
| -- | - | -- |
| **T** | string | An object in a collection (singular of the collection name - e.g. Order from the collection Orders). |
| **TIndexCreator** | string | The name of the index that you want to use. |
| **fieldName**| string | The field that is missing in some of the documents. |

SAMPLE QUERY:

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

Another option is to query the collection for the missing field.  
This will either create a new auto-index or add the new field to an existing auto-index if it indexes the same collection.  

See the example and query syntax descriptions below:

### Example: A query that creates an Auto-Index

The following query will create an auto-index on the "Freight" field 
that is missing in some documents in the Orders collection.  
The query result will contain all documents that do not have this field.  

{CODE-TABS}
{CODE-TAB:csharp:LINQ whereNotexists_1@ClientApi\Session\Querying\HowToFilterByField.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders" 
where true and not exists("Freight")
// `not` cannot be used immediately after `where`, thus we use `where true`.
{CODE-TAB-BLOCK/}
{CODE-TABS/}

#### LINQ Query Syntax

{CODE whereNotexists_signature@ClientApi\Session\Querying\HowToFilterByField.cs /}

| Parameters | Type | Description |
| -- | - | -- |
| **T** | string | An object in a collection (singular of the collection name - e.g. Order from the collection Orders). |
| **fieldName** | string | The field that is missing in some of the documents. |


{PANEL/}


{PANEL: Use Studio to filter by non-existing field}

![List Documents Without a Specified Field](images/non-existing-field-studio-rql.png "List Documents Without a Specified Field")

1. **Indexes**  
   Click to see the indexing menu items.
2. **Query**  
   Select to open the Query view.
3. **Query editor**  
   Write the query according to the [RQL example](../../../client-api/session/querying/how-to-filter-by-non-existing-field#example-a-query-that-creates-an-auto-index) described above.  
4. **Run Query**  
   Click or press ctrl+enter to run the query.
5. **Index used**  
   This is the name of the auto-index created for this query.  
   You can click it to see the available Studio options for this index.  
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
