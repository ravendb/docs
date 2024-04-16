# Full-Text Search

---

{NOTE: }

* This article is about running a full-text search with a **dynamic query**.  
  To learn how to run a full-text search using a static-index, see [full-text search with index](../../../../indexes/querying/searching).  

* Use the `search()` method to query for documents that contain specified term/s
  within the text of the specified document field/s.

* When running a full-text search with a dynamic query, the **auto-index** created by the server 
  breaks down the text of the searched document field using the [default search analyzer](../../../../indexes/using-analyzers#ravendb).  
  All generated terms are lower-cased, so the search is **case-insensitive**.  

* Gain additional control over term tokenization by running a full-text search 
  using a [static-index](../../../../indexes/querying/searching), where the used 
  analyzer is configurable.  

{INFO: }

* A **boost** value can be set for each search to prioritize results. 
  Learn more in [boost search results](../../../../client-api/session/querying/text-search/boost-search-results).
  
* User experience can be enhanced by requesting text fragments that **highlight** 
  the searched terms in the results. Learn more in [highlight search results](../../../../client-api/session/querying/text-search/highlight-query-results).
{INFO/}

---

* In this page:
  * [Search for single term](../../../../client-api/session/querying/text-search/full-text-search#search-for-single-term)
  * [Search for multiple terms](../../../../client-api/session/querying/text-search/full-text-search#search-for-multiple-terms)
  * [Search in multiple fields](../../../../client-api/session/querying/text-search/full-text-search#search-in-multiple-fields)
  * [Search in complex object](../../../../client-api/session/querying/text-search/full-text-search#search-in-complex-object)
  * [Using wildcards](../../../../client-api/session/querying/text-search/full-text-search#using-wildcards)
  * [Syntax](../../../../client-api/session/querying/text-search/full-text-search#syntax)

{NOTE/}

---

{PANEL: Search for single term}

{NOTE: }

{CODE-TABS}
{CODE-TAB:nodejs:Query fts_1@client-api\session\Querying\TextSearch\fullTextSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "University")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

* Executing the above query will generate the auto-index `Auto/Employees/BySearch(Notes)`.  

* This auto-index will contain the following two index-fields:

  * `Notes`  
    Contains terms with the original text from the indexed document field 'Notes'.  
    Text is lower-cased and Not tokenized.
  
  * `search(Notes)`  
    Contains __lower-cased terms__ that were tokenized from the 'Notes' field by the [default search analyzer](../../../../indexes/using-analyzers#ravendb) (RavenStandardAnalyzer). 
    Calling the `search()` method targets these terms to find matching documents.

{PANEL/}

{PANEL: Search for multiple terms}

* You can search for multiple terms in the __same field__ in a single search method.

* By default, the logical operator between these terms is __OR__.  
  Specify __AND__ explicitly To perform an 'and' operation between these terms.  

{NOTE: }

__AND__:

{CODE-TABS}
{CODE-TAB:nodejs:Query fts_2@client-api\session\Querying\TextSearch\fullTextSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "College German", and)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__OR__:

{CODE-TABS}
{CODE-TAB:nodejs:Query fts_3@client-api\session\Querying\TextSearch\fullTextSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "University Sales Japanese")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Search in multiple fields}

* You can search for terms in __different fields__ by making multiple search calls.

* By default, the logical operator between __consecutive search methods__ is 'OR'.  
  This behavior can be modified. See examples below.

{NOTE: }

__Default behavior between search calls__:

{CODE-TABS}
{CODE-TAB:nodejs:Query fts_4@client-api\session\Querying\TextSearch\fullTextSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "French") or search(Title, "President")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{CODE-TABS}
{CODE-TAB:nodejs:Query fts_5@client-api\session\Querying\TextSearch\fullTextSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
where Contact.Title = "Owner" and 
(search(Address.Country, "France") or search(Name, "Markets"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__AND search calls:__:

{CODE-TABS}
{CODE-TAB:nodejs:Query fts_6@client-api\session\Querying\TextSearch\fullTextSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "French") and search(Title, "Manger")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Negate search__:

{CODE-TABS}
{CODE-TAB:nodejs:Query fts_7@client-api\session\Querying\TextSearch\fullTextSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "French") and
(exists(Title) and not search(Title, "Manager"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Search in complex object}

* You can search for terms within a complex object.

* Any nested text field within the object is searchable.

{NOTE: }

{CODE-TABS}
{CODE-TAB:nodejs:Query fts_8@client-api\session\Querying\TextSearch\fullTextSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
where search(Address, "USA London")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Using wildcards}

* Wildcards can be used to replace:
  * Prefix of a searched term
  * Postfix of a searched term
  * Both prefix & postfix

* Note:  

  * Searching with a wildcard as the prefix of the term (e.g. `*text`) is less recommended,  
    as it will cause the server to perform a full index scan.
  
  * Instead, consider using a static-index that indexes the field in reverse order  
    and then query with a wildcard as the postfix, which is much faster.  

{NOTE: }

{CODE-TABS}
{CODE-TAB:nodejs:Query fts_9@client-api\session\Querying\TextSearch\fullTextSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" where
search(Notes, "art*") or
search(Notes, "*logy") or
search(Notes, "*mark*")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@client-api\session\Querying\TextSearch\fullTextSearch.js /}

| Parameter           | Type     | Description                                                                                                   |
|---------------------|----------|---------------------------------------------------------------------------------------------------------------|
| __fieldName__       | string   | Name of the field in which you search.                                                                        |
| __searchTerms__     | string   | A string containing the term or terms (separated by spaces) to search for.                                    |
| __operator__        | string   | Logical operator to use between multiple terms in the same Search method.<br>Can be `AND` or `OR` (default). |

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [How to Use Regex](../../../../client-api/session/querying/text-search/using-regex)
- [How to Query With Exact Match](../../../../client-api/session/querying/text-search/exact-match-query)

### Indexes

- [Analyzers](../../../../indexes/using-analyzers)
- [Full-text search with index](../../../../indexes/querying/searching)
