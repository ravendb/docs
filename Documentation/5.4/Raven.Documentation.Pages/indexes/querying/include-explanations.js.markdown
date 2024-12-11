# Include Explanations in Index Query

---

{NOTE: }

* This article provides examples of including explanations when querying a static-index.  

* **Prior to this article**, please refer to [Include Query Explanations](../../client-api/session/querying/debugging/include-explanations) for dynamic-query examples  
  and general knowledge about including explanations to retrieve the score of resulting documents.
  
    {INFO: }
    * Including explanations is available only when using **Lucene** as the underlying search engine for static-indexes.
    * You can configure which search engine will be used. Learn how in [Selecting the search engine](../../indexes/search-engine/corax#selecting-the-search-engine).
    {INFO/}

---

* In this page:
  * [Include explanations when querying Map index](../../indexes/querying/include-explanations#include-explanations-when-querying-map-index)  
  * [Include explanations when querying Map-Reduce index](../../indexes/querying/include-explanations#include-explanations-when-querying-map-reduce-index)  
  * [Syntax](../../indexes/querying/include-explanations#syntax)

{NOTE/}

---

{PANEL: Include explanations when querying Map index}

{CODE-TABS}
{CODE-TAB:nodejs:Map_index index_1@indexes\querying\includingExplanations.js /}
{CODE-TAB:nodejs:Query inc_1@indexes\querying\includingExplanations.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/BySearchName"
where search(name, "Syrup Lager")
include explanations()
// Or:
from index "Products/BySearchName"
where search(name, "Syrup") or search(name, "Lager")
include explanations()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Include explanations when querying Map-Reduce index}

{CODE-TABS}
{CODE-TAB:nodejs:Map_Reduce_index index_2@indexes\querying\includingExplanations.js /}
{CODE-TAB:nodejs:Query inc_2@indexes\querying\includingExplanations.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "NumberOfUnitsOrdered/PerCategory"
where numberOfUnitsOrdered > 400
include explanations($p0)
{"p0" : { "GroupKey": "category" }}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax_1@indexes\querying\includingExplanations.js /}

| Parameter                | Type                            | Description                                                                                                                                                                    |
|--------------------------|---------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **explanationsCallback** | `(explanationsResults) => void` | <ul><li>A callback function with an output parameter.</li><li>The parameter passed to the callback will be filled with the `Explanations` object when query returns.</li></ul> |
| **options**              | `object`                        | An object that specifies the group key when querying a Map-Reduce index.                                                                                                       |

{CODE:nodejs syntax_2@indexes\querying\includingExplanations.js /}
{CODE:nodejs syntax_3@indexes\querying\includingExplanations.js /}

{PANEL/}

## Related Articles

#### Client API

- [Query Overview](../../client-api/session/querying/how-to-query)

### Indexes

- [Indexing Basics](../../indexes/indexing-basics)

### Querying

- [Query an Index](../../indexes/querying/query-index)
- [Filtering](../../indexes/querying/filtering)
