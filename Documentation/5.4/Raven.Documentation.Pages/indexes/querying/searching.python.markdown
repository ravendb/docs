# Full-Text Search with Index
---

{NOTE: }

* Prior to reading this article, it is recommended to take a look at the 
  [Full-Text search with dynamic queries](../../client-api/session/querying/text-search/full-text-search) 
  article to learn about the `search` method.  

* **All capabilities** provided by `search` with a dynamic query can also be used when querying a static-index.

* However, as opposed to making a dynamic search query where an auto-index is created for you,  
  when using a **static-index**:  

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

#### The index:

{CODE:python index_1@Indexes\Querying\Searching.py/}

---

#### Query with Search:

* Use `Search` to make a full-text search when querying the index.  

* Refer to [Full-Text search with dynamic queries](../../client-api/session/querying/text-search/full-text-search) for all available **Search options**,  
  such as using wildcards, searching for multiple terms, etc.

{CODE-TABS}
{CODE-TAB:python:Query search_1@Indexes\Querying\Searching.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByNotes"
where search(EmployeeNotes, "French")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Indexing multiple fields for FTS}

#### The index:

{CODE:python index_2@Indexes\Querying\Searching.py/}

---

#### Sample query:

{CODE-TABS}
{CODE-TAB:python:Query search_4@Indexes\Querying\Searching.py /}
{CODE-TAB-BLOCK:sql:RQL}
from index "Employees/ByEmployeeData"
where (search(EmployeeData, "Manager") or search(EmployeeData, "French Spanish", and))
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

## Related Articles

### Client API

- [Full-Text search](../../client-api/session/querying/text-search/full-text-search)

### Indexes

- [Analyzers](../../indexes/using-analyzers)
- [Boosting](../../indexes/boosting)
