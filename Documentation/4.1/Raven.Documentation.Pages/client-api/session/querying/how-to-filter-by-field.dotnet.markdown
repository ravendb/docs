# Session: Querying: How to Filter by Existing Fields
---

{NOTE: }

* To filter documents by whether they contain a particular field, use the LINQ extension method `WhereExists()` accessible 
from the [Document Query](../../../client-api/session/querying/document-query/what-is-document-query).  

* If a document doesn't contain the specified field, it is excluded from the query results.

* In this page:  
  * [Syntax](../../../client-api/session/querying/how-to-filter-by-field#syntax)  
  * [Examples](../../../client-api/session/querying/how-to-filter-by-field#examples)  

{NOTE/}

---

{PANEL: Syntax}

{CODE whereexists_1@ClientApi\Session\Querying\HowToFilterByField.cs /}

| Parameters | Type | Description |
| - | - | - |
| **fieldName** | `string` | The name of the field you want to filter by |
| **propertySelector** | `Expression<Func<T,TValue>>` | The path to the field you want to filter by |

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
#### Filter by the Path to a Field  

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

## Related Articles

### Client API

- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)

### Indexes

- [Querying: Filtering](../../../indexes/querying/filtering)
- [Query vs DocumentQuery](../../../indexes/querying/query-vs-document-query)
