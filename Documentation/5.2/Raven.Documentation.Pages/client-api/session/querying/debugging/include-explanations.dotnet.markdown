# How to Include Query Explanations


---

{NOTE: }

* When making a query, each document in the query results is assigned a __score__.  
  This score determines the order by which the documents come back in the results.

* Each document in the results includes this score under the `@index-score` property in its metadata.

* __To get the score details__ and see how it was calculated,  
  you can use `IncludeExplanations` when querying with a [DocumentQuery](../../../../client-api/session/querying/document-query/what-is-document-query). 

* In this page:
    * [Include explanations in query](../../../../client-api/session/querying/debugging/include-explanations#include-explanations-in-query)  
    * [View explanations](../../../../client-api/session/querying/debugging/include-explanations#view-explanations)  
    * [Syntax](../../../../client-api/session/querying/debugging/include-explanations#syntax)  
{NOTE/}

---

{PANEL: Include explanations in query}

{CODE-TABS}
{CODE-TAB:csharp:Sync explain@ClientApi\Session\Querying\Debugging\IncludeExplanations.cs /}
{CODE-TAB:csharp:Async explain_async@ClientApi\Session\Querying\Debugging\IncludeExplanations.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Products
where search(Name, 'Syrup') or search(Name, 'Lager')
include explanations()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: View explanations}

* The detailed explanations can be viewed from the __Query view__ in the Studio:  

![Figure 1. Explanations in the Studio](images/include-explanations-1.png "Include explanations")

* Sample score details:

![Figure 2. View explanations](images/include-explanations-2.png "View explanation")

{PANEL/}

{PANEL: Syntax}

{CODE syntax@ClientApi\Session\Querying\Debugging\IncludeExplanations.cs /}

| Parameters | Data type | Description |
| - | - | - |
| __explanations__ | `Explanations` | An _out_ param that will be filled with the explanations results |

{CODE syntax_2@ClientApi\Session\Querying\Debugging\IncludeExplanations.cs /}

| Parameters | Data type | Description |
| - | - |
| __docId__ | `string` | Resulting document ID for which to get score details |

| Return Value | |
|- | - |
| `string[]` | A list with all explanations |

{PANEL/}
