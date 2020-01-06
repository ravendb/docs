# Session: Querying: How to Filter by Field Presence  

---

{NOTE: }

* To filter documents by whether they contain a particular field, use the LINQ extension method `whereExists()` accessible 
from the [Document Query](../../../client-api/session/querying/document-query/what-is-document-query).  

* If a document doesn't contain the specified field, it is excluded from the query results.

* In this page:  
  * [Syntax](../../../client-api/session/querying/how-to-filter-by-field#syntax)  
  * [Examples](../../../client-api/session/querying/how-to-filter-by-field#examples)  

{NOTE/}

---

{PANEL: Syntax}

{CODE:java whereexists_1@ClientApi\Session\Querying\HowToFilterByField.java /}

| Parameters | Type | Description |
| - | - | - |
| **fieldName** | `String` | The name or path to the field you want to filter by |

{PANEL/}

{PANEL: Examples}

#### Filter by Field Name  

{CODE-TABS}
{CODE-TAB:java:Java whereexists_2@ClientApi\Session\Querying\HowToFilterByField.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees 
where exists("FirstName")
{CODE-TAB-BLOCK/}
{CODE-TABS/}
<br/>
#### Filter by the Path to a Field  

{CODE-TABS}
{CODE-TAB:java:Java whereexists_3@ClientApi\Session\Querying\HowToFilterByField.java /}
{CODE-TAB-BLOCK:sql:RQL}
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
- [Query vs DocumentQuery](../../../indexes/querying/query-vs-document-query)
