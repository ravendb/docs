# Full-Text Search with Index
---

{NOTE: }

* Prior to reading this article, please refer to [full-Text search with dynamic queries](../../client-api/session/querying/text-search/full-text-search) 
  to learn about the `search` method.  

* **All capabilities** provided by `search` with a dynamic query can also be used when querying a static-index.

* However, as opposed to making a dynamic search query where an auto-index is created for you,  
  when using a **static-index**:  

    * You must configure the index-field in which you want to search.  
      See examples below.  
      
    * You can configure which analyzer will be used to tokenize this field.  
      See [selecting an analyzer](../../indexes/using-analyzers#selecting-an-analyzer-for-a-field).

---

* In this article:
  * [Indexing single field for FTS](../../indexes/querying/searching#indexing-single-field-for-fts)
  * [Indexing multiple fields for FTS](../../indexes/querying/searching#indexing-multiple-fields-for-fts)
  * [Indexing all fields for FTS (using AsJson)](../../indexes/querying/searching#indexing-all-fields-for-fts-(using-asjson))
  * [Boosting search results](../../indexes/querying/searching#boosting-search-results)
  * [Searching with wildcards](../../indexes/querying/searching#searching-with-wildcards)
      * [When using RavenStandardAnalyzer or StandardAnalyzer or NGramAnalyzer](../../indexes/querying/searching#when-usingoror)
      * [When using a custom analyzer](../../indexes/querying/searching#when-using-a-custom-analyzer)
      * [When using the Exact analyzer](../../indexes/querying/searching#when-using-the-exact-analyzer)

{NOTE/}

---

{PANEL: Indexing single field for FTS}

#### The index:

{CODE:nodejs index_1@Indexes\Querying\searching.js/}

---

#### Query with Search:

* Using the `search` method has the advantage of using any of its functionalities,  
  such as using wildcards, searching for multiple terms, etc.  

* Refer to [Full-Text search with dynamic queries](../../client-api/session/querying/text-search/full-text-search) for all available **Search options**.

{CODE-TABS}
{CODE-TAB:nodejs:Query search_1@Indexes\Querying\searching.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNotes"
where search(employeeNotes, "French")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Indexing multiple fields for FTS}

#### The index:

{CODE:nodejs index_2@Indexes\Querying\searching.js/}

---

#### Sample query:

{CODE-TABS}
{CODE-TAB:nodejs:Query search_2@Indexes\Querying\searching.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByEmployeeData"
where (search(employeeData, "Manager") or search(employeeData, "French Spanish", and))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Indexing all fields for FTS (using AsJson)}

* To search across ALL fields in a document without defining each one explicitly, use the `AsJson` method, 
  which is available when using **a C# LINQ string** that is assigned to the `map` property in the Node.js index class,
  as shown in the example below.

* This approach makes the index robust to changes in the document schema.  
  By calling `.Select(x => x.Value)` on the result of `AsJson(...)`,
  the index automatically includes values from ALL existing and newly added properties
  and there is no need to update the index when the document structure changes.

* {INFO: }
  This indexing method is supported only when using **Lucene** as the indexing engine.
  {INFO/}

---

#### The index:

{CODE:nodejs index_6@Indexes\Querying\searching.js/}

---

#### Sample query:

{CODE-TABS}
{CODE-TAB:nodejs:Query search_6@Indexes\Querying\searching.js/}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByAllValues"
where search(AllValues, "tofu")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Boosting search results}

* In order to prioritize results, you can provide a boost value to the searched terms.  
  This can be applied by either of the following:

  * Add a boost value to the relevant index-field **inside the index definition**.  
    Refer to article [indexes - boosting](../../indexes/boosting).

  * Add a boost value to the queried terms **at query time**.  
    Refer to article [Boost search results](../../client-api/session/querying/text-search/boost-search-results).

{PANEL/}

{PANEL: Searching with wildcards}

* When making a full-text search with wildcards in the search terms,
  the presence of wildcards (`*`) in the terms sent to the search engine is determined by the transformations applied by the
  [analyzer](../../indexes/using-analyzers) used in the index.

* Note the different behavior in the following cases, as described below:
  * [When using RavenStandardAnalyzer or StandardAnalyzer or NGramAnalyzer](../../indexes/querying/searching#when-usingoror)
  * [When using a custom analyzer](../../indexes/querying/searching#when-using-a-custom-analyzer)
  * [When using the Exact analyzer](../../indexes/querying/searching#when-using-the-exact-analyzer)

* When using [Corax](../../indexes/search-engine/corax) as the search engine,  
  this behavior will only apply to indexes that are newly created or have been reset.

---

{NOTE: }

##### When using&nbsp;`RavenStandardAnalyzer`&nbsp;or`StandardAnalyzer`&nbsp;or&nbsp;`NGramAnalyzer`:
---

Usually, the same analyzer used to tokenize field content at **indexing time** is also used to process the terms provided in the **full-text search query**
before they are sent to the search engine to retrieve matching documents.

**However, in the following cases**:

* When making a [dynamic search query](../../client-api/session/querying/text-search/full-text-search)
* or when querying a static index that uses the default [RavenStandardAnalyzer](../../indexes/using-analyzers#using-the-default-search-analyzer)
* or when querying a static index that uses the [StandardAnalyzer](../../indexes/using-analyzers#analyzers-that-remove-common-stop-words)
* or when querying a static index that uses the [NGramAnalyzer](../../indexes/using-analyzers#analyzers-that-tokenize-according-to-the-defined-number-of-characters)

the queried terms in the _search_ method are processed with the [LowerCaseKeywordAnalyzer](../../indexes/using-analyzers#using-the-default-analyzer)  
before being sent to the search engine.

This analyzer does Not remove the `*`, so the terms are sent with `*`, as provided in the search terms.  
For example:

{CODE-TABS}
{CODE-TAB:nodejs:Index index_3@Indexes\Querying\searching.js /}
{CODE-TAB:nodejs:Query search_3@Indexes\Querying\searching.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNotes/usingDefaultAnalyzer"
where search(EmployeeNotes, "*rench")
include explanations()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}
{NOTE: }

##### When using a custom analyzer:
---

* When setting a [custom analyzer](../../indexes/using-analyzers#creating-custom-analyzers) in your index to tokenize field content,
  then when querying the index, the search terms in the query will be processed according to the custom analyzer's logic.

* The `*` will remain in the terms if the custom analyzer allows it.
  It is the user’s responsibility to ensure that wildcards are not removed by the custom analyzer if they should be included in the query.

* Note:  
  An exception to the above is when the wildcard is used as a suffix in the search term (e.g. `Fren*`).  
  In this case the wildcard will be included in the query regardless of the analyzer's logic.

For example:

{CODE-TABS}
{CODE-TAB:nodejs:Index index_4@Indexes\Querying\searching.js /}
{CODE-TAB:nodejs:Custom_analyzer analyzer_1@Indexes\Querying\searching.js /}
{CODE-TAB:nodejs:Query search_4@Indexes\Querying\searching.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNotes/UsingCustomAnalyzer"
where search(EmployeeNotes, "*French*")
include explanations()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}
{NOTE: }

##### When using the Exact analyzer:
---

When using the default Exact analyzer in your index (which is [KeywordAnalyzer](../../indexes/using-analyzers#using-the-default-exact-analyzer)),  
then when querying the index, the wildcards in your search terms remain untouched.  
The terms are sent to the search engine exactly as produced by the analyzer.

For example:

{CODE-TABS}
{CODE-TAB:nodejs:Index index_5@Indexes\Querying\searching.js /}
{CODE-TAB:nodejs:Query search_5@Indexes\Querying\searching.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByFirstName/usingExactAnalyzer"
where search(FirstName, "Mich*")
include explanations()
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}
{PANEL/}

## Related Articles

### Client API

- [Full-Text search](../../client-api/session/querying/text-search/full-text-search)

### Indexes

- [Analyzers](../../indexes/using-analyzers)
- [Boosting](../../indexes/boosting)
