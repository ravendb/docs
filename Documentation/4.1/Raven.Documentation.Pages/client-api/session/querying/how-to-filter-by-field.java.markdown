# Session: Querying: How to Filter by Field  

---

{NOTE: }

* Filter documents by whether they contain a particular field.  

* In this page:  
  * [Syntax](../../../client-api/session/querying/how-to-filter-by-field#syntax)  
  * [Examples](../../../client-api/session/querying/how-to-filter-by-field#examples)  

{NOTE/}

---

{PANEL: Syntax}

{CODE:java whereexists_1@ClientApi\Session\Querying\HowToFilterByField.java /}

| Parameters | Type | Description |
| - | - | - |
| **fieldName** | `String` | The name or path to the field you want to filter by. If a document doesn't contain this field, it is excluded from the query results. |

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
#### Filter by Path to Field  

{CODE-TABS}
{CODE-TAB:java:Java whereexists_3@ClientApi\Session\Querying\HowToFilterByField.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees 
where exists("Address.Location.Latitude")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}
