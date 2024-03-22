# Filter by Field Presence  

---

{NOTE: }

* To search for documents that contain a specific field by the field **name** or **path**, 
  use the [document_query](../../../client-api/session/querying/document-query/what-is-document-query) 
  `where_exists` or `where_equals` extension method.  

* Documents that do not comprise the specified field will be excluded from the query results.  

* In this page:  
  * [Filter by field name](../../../client-api/session/querying/how-to-filter-by-field#filter-by-field-name)  
  * [Filter by field path](../../../client-api/session/querying/how-to-filter-by-field#filter-by-field-path)  
  * [Syntax](../../../client-api/session/querying/how-to-filter-by-field#syntax)

{NOTE/}

---

{PANEL: Filter by field name }

To search for documents that contain a specific field by the field **name**, pass the name to `where_exists`.

{CODE-TABS}
{CODE-TAB:python:Query whereExists_1@ClientApi\Session\Querying\HowToFilterByField.py /}
{CODE-TAB-BLOCK:sql:RQL}
// Only documents that contain the 'FirstName' field will be returned

from Employees
where exists("FirstName")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Filter by field path }

To search for documents that contain a specific field by the field **path**, pass the path to `where_equals`.

{CODE-TABS}
{CODE-TAB:python:Query whereExists_2@ClientApi\Session\Querying\HowToFilterByField.py /}
{CODE-TAB-BLOCK:sql:RQL}
// Only documents that contain the 'Latitude' property in the specified path will be returned

from Employees
where exists("Address.Location.Latitude")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:python whereExists_syntax@ClientApi\Session\Querying\HowToFilterByField.py /}

| Parameters           | Type        | Description                                                           |
|----------------------|-------------|-----------------------------------------|
| **field_name**       | `str`       | Name of the document field to filter by |
| **field_path**       | `str`       | Path of the document field to filter by |

{PANEL/}

## Related Articles

### Client API

- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)

### Indexes

- [Querying: Filtering](../../../indexes/querying/filtering)
- [Query vs DocumentQuery](../../../client-api/session/querying/document-query/query-vs-document-query)
