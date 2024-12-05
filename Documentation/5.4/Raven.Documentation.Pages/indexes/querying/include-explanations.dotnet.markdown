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
{CODE-TAB:csharp:Map_index index_1@Indexes\Querying\IncludingExplanations.cs /}
{CODE-TAB:csharp:Query inc_1@Indexes\Querying\IncludingExplanations.cs /}
{CODE-TAB:csharp:Query_async inc_1_async@Indexes\Querying\IncludingExplanations.cs /}
{CODE-TAB:csharp:DocumentQuery inc_2@Indexes\Querying\IncludingExplanations.cs /}
{CODE-TAB:csharp:DocumentQuery_async inc_2_async@Indexes\Querying\IncludingExplanations.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/BySearchName"
where search(Name, "Syrup Lager")
include explanations()
// Or:
from index "Products/BySearchName"
where search(Name, "Syrup") or search(Name, "Lager")
include explanations()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Include explanations when querying Map-Reduce index}

{CODE-TABS}
{CODE-TAB:csharp:Map_Reduce_index index_2@Indexes\Querying\IncludingExplanations.cs /}
{CODE-TAB:csharp:Query inc_3@Indexes\Querying\IncludingExplanations.cs /}
{CODE-TAB:csharp:Query_async inc_3_async@Indexes\Querying\IncludingExplanations.cs /}
{CODE-TAB:csharp:DocumentQuery inc_4@Indexes\Querying\IncludingExplanations.cs /}
{CODE-TAB:csharp:DocumentQuery_async inc_4_async@Indexes\Querying\IncludingExplanations.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "NumberOfUnitsOrdered/PerCategory"
where NumberOfUnitsOrdered > 400
include explanations($p0)
{"p0" : { "GroupKey" : "Category" }}
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax_1@Indexes\Querying\IncludingExplanations.cs /}

| Parameter         | Type                 | Description                                                             |
|-------------------|----------------------|-------------------------------------------------------------------------|
| **explanations**  | `Explanations`       | An _out_ param that will be filled with the explanations results.       |
| **options**       | `ExplanationOptions` | An object that specifies the GroupKey when querying a Map-Reduce index. |

{CODE syntax_2@Indexes\Querying\IncludingExplanations.cs /}
{CODE syntax_3@Indexes\Querying\IncludingExplanations.cs /}

{PANEL/}

## Related Articles

#### Client API

- [Query Overview](../../client-api/session/querying/how-to-query)

### Indexes

- [Indexing Basics](../../indexes/indexing-basics)

### Querying

- [Query an Index](../../indexes/querying/query-index)
- [Filtering](../../indexes/querying/filtering)
