# How to Filter by Field Presence  

---

{NOTE: }

* Use method `whereExists()` to query for documents that contain a particular field.  

* A document that doesn't contain the specified field will be excluded from the query results.

* In this page:  
  * [Filter by field name](../../../client-api/session/querying/how-to-filter-by-field#filter-by-field-name)  
  * [Filter by field path](../../../client-api/session/querying/how-to-filter-by-field#filter-by-field-path)  
  * [Syntax](../../../client-api/session/querying/how-to-filter-by-field#syntax)

{NOTE/}

---

{PANEL: Filter by field name }

{CODE-TABS}
{CODE-TAB:nodejs:Query whereExists_1@ClientApi\Session\Querying\howToFilterByField.js /}
{CODE-TAB-BLOCK:sql:RQL}
// Only documents that contain field 'firstName' will be returned

from Employees
where exists("firstName")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Filter by field path }

{CODE-TABS}
{CODE-TAB:nodejs:Query whereExists_2@ClientApi\Session\Querying\howToFilterByField.js /}
{CODE-TAB-BLOCK:sql:RQL}
// Only documents that contain the 'latitude' property in the specified path will be returned

from Employees
where exists("address.location.latitude")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs whereExists_syntax@ClientApi\Session\Querying\howToFilterByField.js /}

| Parameters           | Type                         | Description                                        |
|----------------------|------------------------------|----------------------------------------------------|
| **fieldName**        | `string`                     | The name / path of the document field to filter by |

{PANEL/}

## Related Articles

### Client API

- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)

### Indexes

- [Querying: Filtering](../../../indexes/querying/filtering)
- [Query vs DocumentQuery](../../../client-api/session/querying/document-query/query-vs-document-query)
