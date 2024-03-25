# Filter by Field Presence  

---

{NOTE: }

* To search for documents that contain a specific field by the field **name** or **path**, 
  use the [document_query](../../../client-api/session/querying/document-query/what-is-document-query) 
  `where_exists` method.  
  Documents that do not contain the specified field will be excluded from the query results.  

* In this page:  
   * [Filter by Field Name or Path](../../../client-api/session/querying/how-to-filter-by-field#filter-by-field-name-or-path)  
   * [Syntax](../../../client-api/session/querying/how-to-filter-by-field#syntax)  
   * [Examples](../../../client-api/session/querying/how-to-filter-by-field#examples)  

{NOTE/}

---

{PANEL: Filter by Field Name or Path}

To search for documents that contain a specific field by the field's **name** or **path**, 
pass the name or the path to `where_exists` as demonstrated below.  

---

### Syntax 

{CODE:python whereExists_syntax@ClientApi\Session\Querying\HowToFilterByField.py /}

| Parameters           | Type        | Description                               |
|----------------------|-------------|-------------------------------------------|
| **field_name**       | `str`       | Field Name or Path to filter documents by |

---

### Examples

Pass `where_exists` a string containing the field name or path.  

* **Pass a Field Name**:
   {CODE-TABS}
{CODE-TAB:python:Query whereExists_1@ClientApi\Session\Querying\HowToFilterByField.py /}
{CODE-TAB-BLOCK:sql:RQL}
// Only documents that contain the 'FirstName' field will be returned

from Employees
where exists("FirstName")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

* **Pass a Field Path**:
   {CODE-TABS}
{CODE-TAB:python:Query whereExists_2@ClientApi\Session\Querying\HowToFilterByField.py /}
{CODE-TAB-BLOCK:sql:RQL}
// Only documents that contain the 'Latitude' property in the specified path will be returned

from Employees
where exists("Address.Location.Latitude")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Client API

- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)

### Indexes

- [Querying: Filtering](../../../indexes/querying/filtering)
- [Query vs DocumentQuery](../../../client-api/session/querying/document-query/query-vs-document-query)
