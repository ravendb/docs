# Full-Text Search with Index
---

{NOTE: }

* __Prior to this article__, please refer to [Full-Text search with dynamic queries](../../client-api/session/querying/text-search/full-text-search)  
  to learn about the `Search` method.  

* __All capabilities__ provided by `Search` with a dynamic query can also be used when querying a static-index.

* However, as opposed to making a dynamic search query where an auto-index is created for you,  
  when using a __static-index__:  

    * You must configure the index-field in which you want to search.  
      See examples below.  
      
    * You can configure which analyzer will be used to tokenize this field.  
      See [selecting an analyzer](../../indexes/using-analyzers#selecting-an-analyzer-for-a-field).    

---

* In this page:
  * [Indexing single field for FTS](../../indexes/querying/searching#indexing-single-field-for-fts)
  * [Indexing multiple fields for FTS](../../indexes/querying/searching#indexing-multiple-fields-for-fts)
  * [Boosting search results](../../indexes/querying/searching#boosting-search-results)

{NOTE/}

---

{PANEL: Indexing single field for FTS}

{NOTE: }

__The index__:

---

{CODE:csharp index_1@Indexes\Querying\Searching.cs/}

{NOTE/}

{NOTE: }

__Query with Search__:

---

* Use `Search` to make a full-text search when querying the index.  

* Refer to [Full-Text search with dynamic queries](../../client-api/session/querying/text-search/full-text-search) for all available __Search options__,  
  such as using wildcards, searching for multiple terms, etc.

{CODE-TABS}
{CODE-TAB:csharp:Query search_1@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Query_async search_2@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_3@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNotes"
where search(EmployeeNotes, "French")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Indexing multiple fields for FTS}

{NOTE: }

__The index__:

---

{CODE:csharp index_2@Indexes\Querying\Searching.cs/}

{NOTE/}

{NOTE: }

__Sample query__:

---

{CODE-TABS}
{CODE-TAB:csharp:Query search_4@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:Query_async search_5@Indexes\Querying\Searching.cs /}
{CODE-TAB:csharp:DocumentQuery search_6@Indexes\Querying\Searching.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByEmployeeData"
where (search(EmployeeData, "Manager") or search(EmployeeData, "French Spanish", and))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Boosting search results}

* In order to prioritize results, you can provide a boost value to the searched terms.  
  This can be applied by either of the following:

  * Add a boost value to the relevant index-field __inside the index definition__.  
    Refer to article [indexes - boosting](../../indexes/boosting).

  * Add a boost value to the queried terms __at query time__.  
    Refer to article [Boost search results](../../client-api/session/querying/text-search/boost-search-results).

{PANEL/}

## Related Articles

### Client API

- [Full-Text search](../../client-api/session/querying/text-search/full-text-search)

### Indexes

- [Analyzers](../../indexes/using-analyzers)
- [Boosting](../../indexes/boosting)
