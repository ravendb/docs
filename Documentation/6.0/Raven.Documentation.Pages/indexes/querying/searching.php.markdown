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

{NOTE/}

---

{PANEL: Indexing single field for FTS}

#### The index:

{CODE:php index_1@Indexes\Querying\Searching.php/}

---

#### Query with Search:

* Use `search` to make a full-text search when querying the index.  

* Refer to [Full-Text search with dynamic queries](../../client-api/session/querying/text-search/full-text-search) for all available **Search options**,  
  such as using wildcards, searching for multiple terms, etc.

{CODE-TABS}
{CODE-TAB:php:Query search_1@Indexes\Querying\Searching.php /}
{CODE-TAB:php:documentQuery search_3@Indexes\Querying\Searching.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNotes"
where search(EmployeeNotes, "French")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Indexing multiple fields for FTS}

#### The index:

{CODE:php index_2@Indexes\Querying\Searching.php/}

---

#### Sample query:

{CODE-TABS}
{CODE-TAB:php:Query search_4@Indexes\Querying\Searching.php /}
{CODE-TAB:php:documentQuery search_6@Indexes\Querying\Searching.php /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByEmployeeData"
where (search(EmployeeData, "Manager") or search(EmployeeData, "French Spanish", and))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Indexing all fields for FTS (using AsJson)}

* To search across ALL fields in a document without defining each one explicitly, use the `AsJson` method,
  which is available in the **C# LINQ string** that is assigned to the `$this->map` property in the PHP index class,
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

{CODE:php index_3@Indexes\Querying\Searching.php/}

---

#### Sample query:

{CODE-TABS}
{CODE-TAB:php:Query search_7@Indexes\Querying\Searching.php/}
{CODE-TAB-BLOCK:sql:RQL}
from index "Products/ByAllValues"
where search(allValues, "tofu")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Boosting search results}

* To prioritize results, you can provide a boost value to the searched terms.  
  This can be applied by either of the following:

  * Add a boost value to the relevant index-field **inside the index definition**.  
    Refer to the indexes [Boosting](../../indexes/boosting) article.  

  * Add a boost value to the queried terms **at query time**.  
    Refer to the [Boost search results](../../client-api/session/querying/text-search/boost-search-results) article.  

{PANEL/}

## Related Articles

### Client API

- [Full-Text search](../../client-api/session/querying/text-search/full-text-search)

### Indexes

- [Analyzers](../../indexes/using-analyzers)
- [Boosting](../../indexes/boosting)
