# How to Filter by Non-Existing Field  

---

{NOTE: }

* There are situations where new fields are added to some documents in a collection over time.  

* To find the documents that are missing the newly added fields you can either:  
    * [Query the collection (dynamic query)](../../../client-api/session/querying/how-to-filter-by-non-existing-field#query-the-collection-(dynamic-query))  
    * [Query a static index](../../../client-api/session/querying/how-to-filter-by-non-existing-field#query-a-static-index)  
    * [Query by RQL in Studio](../../../client-api/session/querying/how-to-filter-by-non-existing-field#query-by-rql-in-studio)  

----

{NOTE/}

---

{PANEL: Query the collection (dynamic query)}

* You can make a dynamic query on a collection to find which documents are missing the specified field.

* Use extension methods `Not` & `WhereExists` that are accessible from [DocumentQuery](../../../client-api/session/querying/document-query/what-is-document-query).

* This will either create a new auto-index or add the queried field to an existing auto-index.  
  Learn more about the dynamic query flow [here](../../../client-api/session/querying/how-to-query#dynamicQuery).

{NOTE: }

__Example__

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery whereNotExists_1@ClientApi\Session\Querying\FilterByNonExistingField.cs /}
{CODE-TAB:csharp:DocumentQuery_async whereNotExists_1_async@ClientApi\Session\Querying\FilterByNonExistingField.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
where true and not exists("Freight")
// `not` cannot be used immediately after `where`, thus we use `where true`.
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Query a static index}

* You can search for documents with missing fields by querying a static index.  

* The index definition must have the following document-fields indexed:

    1. The field that is suspected to be __missing in some documents__.  
  
    2. A document-field that __exists in all documents__ in the collection,  
       (i.e. the _Id_ field, or any other field that is common to all).  
       Indexing such a field is mandatory so that all documents in the collection will be indexed.

{NOTE: }

__Example__

{CODE the_index@ClientApi\Session\Querying\FilterByNonExistingField.cs /}

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery whereNotexists_2@ClientApi\Session\Querying\FilterByNonExistingField.cs /}
{CODE-TAB:csharp:DocumentQuery_async whereNotexists_2_async@ClientApi\Session\Querying\FilterByNonExistingField.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Orders/ByFreight"
where true and not exists("Freight")
// `not` cannot come immediately after `where`, thus we use `where true`.
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Query by RQL in Studio}

* You can query for documents with missing fields in the Studio's [Query view](../../../studio/database/queries/query-view).

* Use an [RQL](../../../client-api/session/querying/what-is-rql) expression such as:

    {CODE-BLOCK:sql}
from "Orders"    
where exists("Company") and not exists("Freight")
    {CODE-BLOCK/}

* In the `where` clause,  
  first search for a field that __exists__ in every document in the collection,  
  and then search for the field that __may not exist__ in some of document.

![List Documents Without a Specified Field](images/non-existing-field-studio-rql.png "Query for documents that are missing the specified field")

1. **Indexes**  
   Click to see the Indexes menu.
2. **Query**  
   Select to open the Query view.
3. **Query editor**  
   Write the RQL query.
4. **Run Query**  
   Click or press ctrl+enter to run the query.
5. **Index used**  
   This is the name of the auto-index created to serve this query.  
   You can click it to see the available Studio options for this index.  
6. **Results**  
   This is the list of documents that do not contain the specified 'Freight' field.  
   (Field "Freight" was explicitly removed from these Northwind documents for this example.)

{PANEL/}

## Related Articles

### Client API

- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)
- [How to Filter by Field Presence](../../../client-api/session/querying/how-to-filter-by-field)

### Querying

- [Querying: Filtering](../../../indexes/querying/filtering)
- [Query vs DocumentQuery](../../../client-api/session/querying/document-query/query-vs-document-query)
- [RQL - Raven Query Language](../../../client-api/session/querying/what-is-rql)

---

### Code Walkthrough

- [Queries - Filtering Results - Basics](https://demo.ravendb.net/demos/csharp/queries/filtering-results-basics)
- [Queries - Filtering with Multiple Conditions](https://demo.ravendb.net/demos/csharp/queries/filtering-results-multiple-conditions)
