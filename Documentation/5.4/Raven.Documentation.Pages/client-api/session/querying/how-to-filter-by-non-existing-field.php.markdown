# Filter by Non-Existing Field  

---

{NOTE: }

* There are situations where new fields are added to some documents in a collection over time.  
* To find documents that are missing the newly added fields you can either:  
    * [Query the collection (dynamic query)](../../../client-api/session/querying/how-to-filter-by-non-existing-field#query-the-collection-(dynamic-query))  
    * [Query a static index](../../../client-api/session/querying/how-to-filter-by-non-existing-field#query-a-static-index)  
    * [Use Studio to run an RQL Query](../../../client-api/session/querying/how-to-filter-by-non-existing-field#use-studio-to-run-an-rql-query)  

----

{NOTE/}

---

{PANEL: Query the collection (dynamic query)}

To run a dynamic query over a collection and find which documents are missing a specified field,  
use the `not` and `whereExists` extension methods, accessible from the [documentQuery](../../../client-api/session/querying/document-query/what-is-document-query) API, 
as shown below.  

This will either create a new auto-index or add the queried field to an existing auto-index.  
Learn more about the dynamic query flow [here](../../../client-api/session/querying/how-to-query#dynamicQuery).  

**Example**

{CODE-TABS}
{CODE-TAB:php:documentQuery whereNotExists_1@ClientApi\Session\Querying\FilterByNonExistingField.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
where true and not exists("Freight")
// `not` cannot be used immediately after `where`, thus we use `where true`.
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Query a static index}

Documents with missing fields can be searched by querying a static index.  

The index definition must contain the following document fields indexed:

* A document field that **exists** in **all** documents of the queried collection, e.g. the _Id_ field.  
  Indexing this field will ensure that all the documents of this collection are indexed.
* A document field that is suspected to be **missing** from some documents of the queried collection.  

**Example**

{CODE:php the_index@ClientApi\Session\Querying\FilterByNonExistingField.php /}

{CODE-TABS}
{CODE-TAB:php:documentQuery whereNotexists_2@ClientApi\Session\Querying\FilterByNonExistingField.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Orders/ByFreight"
where true and not exists("Freight")
// `not` cannot come immediately after `where`, thus we use `where true`.
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Use Studio to Run an RQL Query}

* Documents can be searched by missing fields using Studio's [Query view](../../../studio/database/queries/query-view).  

* Use an [RQL](../../../client-api/session/querying/what-is-rql) expression such as:
  {CODE-BLOCK:sql}
from "Orders"    
where exists("Company") and not exists("Freight")
{CODE-BLOCK/}

* In the `where` clause:  
  First search for a field that **exists** in **all** documents of the queried collection, e.g. the _Id_ field.  
  Then search for a field that **may be missing** from some documents of the queried collection.  

    ![List Documents Without a Specified Field](images/non-existing-field-studio-rql.png "Query for documents that are missing the specified field")

    1. **Indexes**  
       Click to see the Indexes menu.
    2. **Query**  
       Select to open the Query view.
    3. **Query editor**  
       Write the RQL query.
    4. **Run Query**  
       Click to run the query.
    5. **Index used**  
       The name of the auto-index created to serve this query.  
       You can click it to see the available Studio options for this index.  
    6. **Results**  
       This is the list of documents that do not contain the specified 'Freight' field.  
       (the "Freight" Field was removed from these Northwind documents for this example.)

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
