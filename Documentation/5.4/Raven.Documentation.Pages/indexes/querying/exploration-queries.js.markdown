# Exploration Queries
---

{NOTE: }

* **Exploration Queries** form an additional layer of filtering that can be applied 
  to a dataset after its retrieval by [query](../../client-api/session/querying/how-to-query#session.query), 
  or [rawQuery](../../client-api/session/querying/how-to-query#session.advanced.rawquery), 
  while the dataset is still held by the server.  

* The **retrieved dataset** is scanned and filtered **without requiring or creating an index**,  
  providing a way to conduct one-time explorations without creating an index that would have to be maintained by the cluster.  

* You can filter the datasets retrieved by both **Index queries** and **Collection queries**.  

* Exploration queries need to be used with caution when large datasets are handled
  since scanning and filtering all the data retrieved by a query costs substantial server resources and user waiting time.

    {WARNING: }

    We recommend that you -  

  * **Limit** the number of records that an exploration query filters.  
  * Use [where](../../indexes/querying/filtering) in recurring queries, 
    so the query would [use an index](../../indexes/querying/exploration-queries#limit-the-query-and-prefer--for-recurring-queries).  

    {WARNING/}

* In this page:  

   * [`filter`](../../indexes/querying/exploration-queries#filter)
   * [When should exploration queries be used](../../indexes/querying/exploration-queries#when-should-exploration-queries-be-used)
   * [Usage examples](../../indexes/querying/exploration-queries#usage-examples)
      * [With collection queries](../../indexes/querying/exploration-queries#with-collection-queries)
      * [With queries that use an index](../../indexes/querying/exploration-queries#with-queries-that-use-an-index)
      * [With projections](../../indexes/querying/exploration-queries#with-projections)
      * [With user-defined JavaScript functions (`declare`)](../../indexes/querying/exploration-queries#with-user-defined-javascript-functions-)
   * [Syntax](../../indexes/querying/exploration-queries#syntax)

{NOTE/}

---

{PANEL: `filter`}

Exploration queries can be applied using -  

* `query.filter` 
* RQL's `filter` keyword  

The added filtering is parsed and executed by RavenDB's Javascript engine.  

The provided filtering operations resemble those implemented by [where](../../indexes/querying/filtering) 
and can be further enhanced by Javascript functions of your own. 
Read [here](../../indexes/querying/exploration-queries#with-user-defined-javascript-functions-) about creating and using your own Javascript function in your filters.  

{PANEL/}

{PANEL: When Should Exploration Queries Be Used}

`filter` can be applied to a Full-Collection query, for example:

{CODE-BLOCK: sql}
// A full-collection query:
// ========================

from Employees as e
filter e.Address.Country == "USA"

// Results include only employees from USA
// No auto-index is created
{CODE-BLOCK/}

It can also be applied to queries handled by an index:  

{CODE-BLOCK: sql}
// A dynamic query:
// ================

from Employees as e 
where e.Title == "Sales Representative" // This triggers auto-index creation
filter e.Address.Country == "USA"

// Results include only employees from USA having the specified title
// The auto-index created only indexes the 'Title' field
{CODE-BLOCK/}

{CODE-BLOCK: sql}
// A static-index query:
// =====================

from index "Orders/ByCompany" 
filter Count > 10

// Results include orders with Count > 10 using the specified static-index 
{CODE-BLOCK/}

Both in a collection query and in a query handled by an index, all the results that are retrieved by the query are scanned and filtered by `filter`.  

{INFO: }

#### When to use

Use `filter` for an ad-hoc exploration of the retrieved dataset, that matches no existing index and is not expected to be repeated much.  

* You gain the ability to filter post-query results on the server side, for both collection queries and when an index was used.  
* The dataset will be filtered without creating an unrequired index that the cluster would continue updating from now on.
 
{INFO/}
 
{WARNING: }

#### Limit the query, and prefer `where` for recurring queries

Be aware that when a large dataset is retrieved, like the whole collection in the case of a collection query,  
exploring it using `filter` would tax the server in memory and CPU usage while it checks the filter condition for each query result, 
and cost the user a substantial waiting time. Therefore:  

* **Limit** the number of records that an exploration query filters, e.g.:  
  {CODE-BLOCK: sql}
from Orders
// Apply filter
filter ShipTo.Country == "UK"
// Limit the number of records that will be scanned by the filter operation
filter_limit 100 

// While there are 830 records in the Orders collection, 
// only the first 100 records that are retrieved by the query are scanned by 'filter'
// Running this RQL on the sample data returns 4 matching results out of the 100 scanned.
  {CODE-BLOCK/}

* Use [where](../../indexes/querying/filtering) rather than `filter` for recurring filtering.  
  `where` will use an index, creating it if necessary, to accelerate the filtering in subsequent queries.
 
{WARNING/}

{PANEL/}

{PANEL: Usage examples}

#### With collection queries:

Use `filter` with a full-collection query to scan and filter the entire collection.

{CODE-TABS}
{CODE-TAB:nodejs:Query exploration-queries_1_1@indexes\querying\explorationQueries.js /}
{CODE-TAB:nodejs:RawQuery exploration-queries_1_2@indexes\querying\explorationQueries.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies" 
filter Address.Country == "USA"
filter_limit 50
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{WARNING: }

Filtering a sizable collection will burden the server and prolong user waiting time.  
It is recommended to set a `filter_limit` to restrict the number of filtered records.  

{WARNING/}

---

#### With queries that use an index:

Use `filter` after a `whereEquals` clause to filter the results retrieved by the query.  

{CODE-TABS}
{CODE-TAB:nodejs:Query exploration-queries_2_1@indexes\querying\explorationQueries.js /}
{CODE-TAB:nodejs:RawQuery exploration-queries_2_2@indexes\querying\explorationQueries.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
where Contact.Title == "Sales Representative"
filter Address.Country == "Germany"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### With projections:

The filtered results can be projected using `selectFields`, like those of any other query.  

{CODE-TABS}
{CODE-TAB:nodejs:Query exploration-queries_3_1@indexes\querying\explorationQueries.js /}
{CODE-TAB:nodejs:RawQuery exploration-queries_3_2@indexes\querying\explorationQueries.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
filter Address.Country == "Germany"
select Name, Address.City, Address.Country
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

#### With user-defined JavaScript functions (`declare`):

When using RQL, you can define a JavaScript function using the [declare](../../client-api/session/querying/what-is-rql#declare) keyword.  
This function can then be used as part of your `filter` condition to further customize the results.  
For example:  

{CODE-BLOCK: sql}
// Declare a Javascript function:
// ==============================

declare function filterByTitlePrefix(employee, prefix) 
{ 
    // Include any filtering logic that suits your needs
    return employee.Title.startsWith(prefix)
} 

// Use the function in your RQL:
// =============================

from Employees as employee
// Filter using the declared function
filter filterByTitlePrefix(employee, "Sales")
filter_limit 10
{CODE-BLOCK/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@indexes\querying\explorationQueries.js /}

| Parameter     | Type                | Description                                                                                                               |
|---------------|---------------------|---------------------------------------------------------------------------------------------------------------------------|
| **builder**   | `(factory) => void` | The filtering method                                                                                                      |
| **limit**     | `number`            | The number of records from the query results that `filter` should scan.<br>Default: all retrieved records. |
      
{PANEL/}

## Related articles

### Querying

- [What is RQL](../../client-api/session/querying/what-is-rql)  
- [Query](../../client-api/session/querying/how-to-query#session.query)  
- [DocumentQuery](../../client-api/session/querying/how-to-query#session.advanced.documentquery)  
- [RawQuery](../../client-api/session/querying/how-to-query#session.advanced.rawquery)  
- [Where](../../indexes/querying/filtering)  
- [Querying an Index](../../indexes/querying/query-index)  
- [Sorting](../../indexes/querying/sorting)  
- [declare](../../client-api/session/querying/what-is-rql#declare)  
