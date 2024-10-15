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
  * [Search operators](../../../../client-api/session/querying/text-search/full-text-search#search-operators)
  * [Search options](../../../../client-api/session/querying/text-search/full-text-search#search-options)
  * [Using wildcards](../../../../client-api/session/querying/text-search/full-text-search#using-wildcards)
  * [Syntax](../../../../client-api/session/querying/text-search/full-text-search#syntax)

{NOTE/}

---

{PANEL: Search for single term}

{NOTE: }

{CODE-TABS}
{CODE-TAB:php:query fts_1@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB:php:documentQuery fts_3@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
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
    Contains **lower-cased terms** that were tokenized from the 'Notes' field by the [default search analyzer](../../../../indexes/using-analyzers#ravendb) (RavenStandardAnalyzer). 
    Calling the `search()` method targets these terms to find matching documents.

{PANEL/}

{PANEL: Search for multiple terms}

* You can search for multiple terms in the **same field** in a single search method.

* By default, the logical operator between these terms is 'OR'.

* This behavior can be modified. See section [Search operators](../../../../client-api/session/querying/text-search/full-text-search#search-operators). 

{NOTE: }

**Pass terms in a string**:

{CODE-TABS}
{CODE-TAB:php:query fts_4@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB:php:documentQuery fts_6@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "University Sales Japanese")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Pass terms in a list**:
 
{CODE-TABS}
{CODE-TAB:php:query fts_7@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "University Sales Japanese")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Search in multiple fields}

* You can search for terms in **different fields** by making multiple search calls.

* By default, the logical operator between consecutive search methods is 'OR'.

* This behavior can be modified. See section [Search operators](../../../../client-api/session/querying/text-search/full-text-search#search-operators).

{NOTE: }

{CODE-TABS}
{CODE-TAB:php:query fts_9@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB:php:documentQuery fts_11@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where (search(Notes, "French") or search(Title, "President"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Search in complex object}

* You can search for terms within a complex object.

* Any nested text field within the object is searchable.

{NOTE: }

{CODE-TABS}
{CODE-TAB:php:query fts_12@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB:php:documentQuery fts_14@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
where search(Address, "USA London")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Search operators}

* By default, the logical operator between multiple terms within the **same field** in a search call is **OR**.

* This can be modified using the `@operator` parameter as follows: 

{NOTE: }

**AND**:

{CODE-TABS}
{CODE-TAB:php:query fts_15@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB:php:documentQuery fts_17@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "College German", and)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**OR**:

{CODE-TABS}
{CODE-TAB:php:query fts_18@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB:php:documentQuery fts_20@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "College German")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Search options}

Search options allow to:  

* Negate a search criteria.  
* Specify the logical operator used between **consecutive search calls**.  

{NOTE: }

**Negate search**:

{CODE-TABS}
{CODE-TAB:php:query fts_21@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB:php:documentQuery fts_23@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
where (exists(Address) and not search(Address, "USA"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Default behavior between search calls**:

* By default, the logical operator between consecutive search methods is **OR**.

{CODE-TABS}
{CODE-TAB:php:query fts_24@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB:php:documentQuery fts_26@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies" 
where Contact.Title == "Owner" and
(search(Address.Country, "France") or search(Name, "Markets"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }
**AND search calls**:

{CODE-TABS}
{CODE-TAB:php:query fts_27@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB:php:documentQuery fts_29@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" 
where search(Notes, "French") and search(Title, "Manager")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

**Use options as bit flags**:

{CODE-TABS}
{CODE-TAB:php:query fts_30@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB:php:documentQuery fts_32@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "French") and
(exists(Title) and not search(Title, "Manager"))
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

  * Searching with a wildcard as the prefix of the term (e.g. `*text`) is 
    not advised as it will cause the server to perform a full index scan.
  
  * Instead, consider using a static-index that indexes the field in reverse order  
    and then query with a wildcard as the postfix, which is much faster.  

{NOTE: }

{CODE-TABS}
{CODE-TAB:php:query fts_33@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
{CODE-TAB:php:documentQuery fts_35@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}
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

{CODE:php syntax@ClientApi\Session\Querying\TextSearch\FullTextSearch.php /}

| Parameter | Type | Description |
|-----------|------|-------------|
| **$fieldName** | `string` | Name of the searched field. |
| **$searchTerms** | `string` | A string containing the term or terms (separated by spaces) to search for. |
| **$operator** | `?SearchOperator ` | Logical operator to use between multiple terms in the same Search method.<br>**Can be**: `SearchOperator::or` or `SearchOperator::and`<br>**Default**: `SearchOperator::or` |

| Return Type | Description |
| ------------| ----------- |
| `DocumentQueryInterface` | Query results |

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [How to Use Regex](../../../../client-api/session/querying/text-search/using-regex)
- [How to Query With Exact Match](../../../../client-api/session/querying/text-search/exact-match-query)

### Indexes

- [Analyzers](../../../../indexes/using-analyzers)
- [Full-text search with index](../../../../indexes/querying/searching)
