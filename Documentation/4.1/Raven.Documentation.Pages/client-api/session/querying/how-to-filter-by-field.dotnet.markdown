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

{CODE whereexists_1@ClientApi\Session\Querying\HowToFilterByField.cs /}

| Parameters | Type | Description |
| - | - | - |
| **fieldName** | `string` | The name of the field you want to filter by. If a document doesn't contain this field, it is excluded from the query results. |
| **propertySelector** | `Expression<Func<T,TValue>>` | Path to the field you want to filter by |

{PANEL/}

{PANEL: Examples}

#### Filter by Field Name  

{CODE-TABS}
{CODE-TAB:csharp:Sync whereexists_2@ClientApi\Session\Querying\HowToFilterByField.cs /}
{CODE-TAB:csharp:Async whereexists_2_async@ClientApi\Session\Querying\HowToFilterByField.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees 
where exists("FirstName")
{CODE-TAB-BLOCK/}
{CODE-TABS/}
<br/>
<br/>
#### Filter by Path to Field  

This is done by passing the path as a lambda expression:  

{CODE-TABS}
{CODE-TAB:csharp:Sync whereexists_4@ClientApi\Session\Querying\HowToFilterByField.cs /}
{CODE-TAB:csharp:Async whereexists_4_async@ClientApi\Session\Querying\HowToFilterByField.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees 
where exists(Address.Location.Latitude)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

The path can also be passed as a `string`:

{CODE-TABS}
{CODE-TAB:csharp:Sync whereexists_3@ClientApi\Session\Querying\HowToFilterByField.cs /}
{CODE-TAB:csharp:Async whereexists_3_async@ClientApi\Session\Querying\HowToFilterByField.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees 
where exists("Address.Location.Latitude")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}
