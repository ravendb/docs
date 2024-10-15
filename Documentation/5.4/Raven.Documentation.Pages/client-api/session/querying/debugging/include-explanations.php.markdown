# Include Query Explanations

---

{NOTE: }

* When making a query, each document in the query results is assigned a **score**.  
  This score determines the order by which the documents come back in the results when requesting   
  to [order by score](../../../../client-api/session/querying/sort-query-results#order-by-score).

* Each document in the results includes this score under the `@index-score` property in its metadata.

* Use `includeExplanations` to get the score details and see how it was calculated.  

* In this page:
    * [Include explanations in a query](../../../../client-api/session/querying/debugging/include-explanations#include-explanations-in-a-query)  
    * [View explanations](../../../../client-api/session/querying/debugging/include-explanations#view-explanations)  
    * [Syntax](../../../../client-api/session/querying/debugging/include-explanations#syntax)  
{NOTE/}

---

{PANEL: Include explanations in a query}

{CODE-TABS}
{CODE-TAB:php:query explain@ClientApi\Session\Querying\Debugging\IncludeExplanations.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where search(Name, "Syrup") or search(Name, "Lager")
include explanations()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE: }
Please note that the First parameter is optional.  
If you intend to use the default options, just paste `null` instead of the options object.  
{NOTE/}

{PANEL/}

{PANEL: View explanations}

* The detailed explanations can be viewed from the **Query view** in Studio.  

* Running a query with `includeExplanations` will show an additional **Explanations Tab**.

![Figure 1. Explanations in the Studio](images/include-explanations-1.png "Include explanations")

* Sample score details:

![Figure 2. View explanations](images/include-explanations-2.png "View explanation")

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax@ClientApi\Session\Querying\Debugging\IncludeExplanations.php /}

| Parameters | Data type | Description |
| - | - | - |
| **$options** | `?ExplanationOptions` | This object is optional.<br>If you intend to use the default options, place `null` here. |
| **&$explanations** | `Explanations` | <ul><li>A callback function (action) that takes `Explanations` as an argument. It will be called by the client with the resulting `Explanations`.</li> <li>You can interact with resulting Explanations inside your callback.</li></ul> |

{PANEL/}
