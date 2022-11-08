# How to Filter by Field Presence  

---

{NOTE: }

* To query for documents that contain a particular field,  
  use extension method `WhereExists()` which is accessible from [DocumentQuery](../../../client-api/session/querying/document-query/what-is-document-query).  

* A document that doesn't contain the specified field will be excluded from the query results.

* In this page:  
  * [Filter by field name](../../../client-api/session/querying/how-to-filter-by-field#filter-by-field-name)  
  * [Filter by field path](../../../client-api/session/querying/how-to-filter-by-field#filter-by-field-path)  
  * [Syntax](../../../client-api/session/querying/how-to-filter-by-field#syntax)

{NOTE/}

---

{PANEL: Filter by field name }

{CODE-TABS}
{CODE-TAB:csharp:Sync whereExists_1@ClientApi\Session\Querying\HowToFilterByField.cs /}
{CODE-TAB:csharp:Async whereExists_1_async@ClientApi\Session\Querying\HowToFilterByField.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// Only documents that contain field 'FirstName' will be returned

from Employees
where exists("FirstName")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Filter by field path }

{CODE-TABS}
{CODE-TAB:csharp:Sync whereExists_2@ClientApi\Session\Querying\HowToFilterByField.cs /}
{CODE-TAB:csharp:Async whereExists_2_async@ClientApi\Session\Querying\HowToFilterByField.cs /}
{CODE-TAB-BLOCK:sql:RQL}
// Only documents that contain the 'Latitude' property in the specified path will be returned

from Employees
where exists("Address.Location.Latitude")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE whereExists_syntax@ClientApi\Session\Querying\HowToFilterByField.cs /}

| Parameters           | Type                         | Description                                                           |
|----------------------|------------------------------|-----------------------------------------------------------------------|
| **fieldName**        | `string`                     | The name / path of the document field to filter by                    |
| **propertySelector** | `Expression<Func<T,TValue>>` | Lambda expression with name / path of the document field to filter by |

{PANEL/}

## Related Articles

### Client API

- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)

### Indexes

- [Querying: Filtering](../../../indexes/querying/filtering)
- [Query vs DocumentQuery](../../../indexes/querying/query-vs-document-query)
