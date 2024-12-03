# Include Query Explanations

---

{NOTE: }

* When making a query, each document in the query results is assigned a **score**.  
  This score determines the order by which the documents come back in the results when requesting   
  to [order by score](../../../../client-api/session/querying/sort-query-results#order-by-score).

* Each document in the results includes this score under the `@index-score` property in its metadata.

* Use `include_explanations` to get the score details** and see how it was calculated.  

    {INFO: }
    * Including explanations is available only when using **Lucene** as the underlying search engine for auto-indexes.
    * You can set the search engine for auto-indexes using the [Indexing.Auto.SearchEngineType](../../../../server/configuration/indexing-configuration#indexing.auto.searchenginetype) configuration key.
    {INFO/}

---

* In this page:
    * [Include explanations in a query](../../../../client-api/session/querying/debugging/include-explanations#include-explanations-in-a-query)  
    * [View explanations](../../../../client-api/session/querying/debugging/include-explanations#view-explanations)  
    * [Syntax](../../../../client-api/session/querying/debugging/include-explanations#syntax)  

{NOTE/}

---

{PANEL: Include explanations in a query}

{CODE-TABS}
{CODE-TAB:python:Query explain@ClientApi\Session\Querying\Debugging\IncludeExplanations.py /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where search(Name, "Syrup") or search(Name, "Lager")
include explanations()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: View explanations}

* The detailed explanations can be viewed from the **Query view** in Studio.  

* Running a query with `include_explanations` will show an additional **Explanations Tab**.

![Figure 1. Explanations in the Studio](images/include-explanations-1.png "Include explanations")

* Sample score details:

![Figure 2. View explanations](images/include-explanations-2.png "View explanation")

{PANEL/}

{PANEL: Syntax}

{CODE:python syntax@ClientApi\Session\Querying\Debugging\IncludeExplanations.py /}

| Parameter                 | Type                             | Description                                                                                                                                                                                                                             |
|---------------------------|----------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **explanations_callback** | `Callable[[Explanations], None]` | <ul><li>A callback function (action) that takes `Explanations` as an argument. It will be called by the client with the resulting `Explanations`.</li> <li>You can interact with resulting Explanations inside your callback.</li></ul> |
| **options** (Optional)    | `ExplanationOptions`             | Can be a `group_key` string                                                                                                                                                                                                             |

| `Explanations`         |                                                                                                                                |
|------------------------|--------------------------------------------------------------------------------------------------------------------------------|
| `Dict[str, List[str]]` | <ul><li>Pass the resulting document ID for which to get score details.</li><li>Returns a list with all explanations.</li></ul> |

{PANEL/}
