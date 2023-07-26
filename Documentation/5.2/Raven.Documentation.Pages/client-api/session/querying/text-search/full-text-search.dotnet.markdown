# Full-Text Search

---

{NOTE: }

* This article is about making a full-text search with a __dynamic query__.  
  For making a full-text search using a static-index see [full-text search in index](../../../../indexes/querying/searching).

---

* Use method `Search()` to query for documents that contain the specified term(s)  
  within the text of the specified document field(s).

* You can provide a __boost__ value to each search in order to prioritize results.  
  Learn more in [boost search results](../../../../client-api/session/querying/text-search/boost-search-results).
  
* In addition to retrieving matching documents, in order to enhance user experience,  
  you can request to get text fragments that __highlight__ the searched terms in the results.  
  Learn more in [highlight search results](../../../../client-api/session/querying/text-search/highlight-query-results).

---

* When making a full-text search with a dynamic query, the __auto-index__ created by the server  
  breaks down the text of the document field in which you search using the [default search analyzer](../../../../indexes/using-analyzers#ravendb).  
  All terms generated will be lower-cased so search will be __case-insensitive__.  

* To have more control over how terms are tokenized, perform a full-text search using a [static-index](../../../../indexes/querying/searching),  
  where you can configure which analyzer to use.  

---

* In this page:
  * [Search for single term](../../../../client-api/session/querying/text-search/full-text-search#search-for-single-term)
  * [Search for multiple terms](../../../../client-api/session/querying/text-search/full-text-search#search-for-multiple-terms)
  * [Search in multiple fields](../../../../client-api/session/querying/text-search/full-text-search#search-in-multiple-fields)
  * [Search in complex object](../../../../client-api/session/querying/text-search/full-text-search#search-in-complex-object)
  * [Search operators](../../../..//client-api/session/querying/text-search/full-text-search#search-operators)
  * [Search options](../../../../client-api/session/querying/text-search/full-text-search#search-options)
  * [Using wildcards](../../../../client-api/session/querying/text-search/full-text-search#using-wildcards)
  * [Syntax](../../../../client-api/session/querying/text-search/full-text-search#syntax)

{NOTE/}

---

{PANEL: Search for single term}

{NOTE: }

{CODE-TABS}
{CODE-TAB:csharp:Query fts_1@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:Query_async fts_2@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:DocumentQuery fts_3@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
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
    Contains __lower-cased terms__ that were tokenized from the 'Notes' field by the [default search analyzer](indexes/using-analyzers#ravendb) (RavenStandardAnalyzer). 
    Calling the `Search()` method targets these terms to find matching documents.

{PANEL/}

{PANEL: Search for multiple terms}

* You can search for multiple terms in the __same field__ in a single search method.

* By default, the logical operator between these terms is 'OR'.

* This behavior can be modified. See section [Search operators](../../../../client-api/session/querying/text-search/full-text-search#search-operators). 

{NOTE: }

__Pass terms in a string__:

{CODE-TABS}
{CODE-TAB:csharp:Query fts_4@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:Query_async fts_5@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:DocumentQuery fts_6@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "University Sales Japanese")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Pass terms in a list__:
 
{CODE-TABS}
{CODE-TAB:csharp:Query fts_7@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:Query_async fts_8@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "University Sales Japanese")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Search in multiple fields}

* You can search for terms in __different fields__ by making multiple search calls.

* By default, the logical operator between consecutive search methods is 'OR'.

* This behavior can be modified. See section [Search options](../../../../client-api/session/querying/text-search/full-text-search#search-operators).

{NOTE: }

{CODE-TABS}
{CODE-TAB:csharp:Query fts_9@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:Query_async fts_10@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:DocumentQuery fts_11@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
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
{CODE-TAB:csharp:Query fts_12@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:Query_async fts_13@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:DocumentQuery fts_14@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
where search(Address, "USA London")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Search operators}

* By default, the logical operator between multiple terms within the __same field__ in a search call is __OR__.

* This can be modified using the `@operator` parameter as follows: 

{NOTE: }

__AND__:

{CODE-TABS}
{CODE-TAB:csharp:Query fts_15@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:Query_async fts_16@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:DocumentQuery fts_17@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "College German", and)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__OR__:

{CODE-TABS}
{CODE-TAB:csharp:Query fts_18@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:Query_async fts_19@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:DocumentQuery fts_20@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where search(Notes, "College German")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Search options}

* Search options allow to:  
    * Negate a search criteria.  
    * Specify the logical operator used between __consecutive search calls__.  

* When using `Query`: use the `options` parameter.  
  When using `DocumentQuery`: follow the specific syntax in each example below.  

{NOTE: }

__Negate search__:

{CODE-TABS}
{CODE-TAB:csharp:Query fts_21@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:Query_async fts_22@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:DocumentQuery fts_23@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
where (exists(Address) and not search(Address, "USA"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Default behavior between search calls__:

* By default, the logical operator between consecutive search methods is __OR__.

{CODE-TABS}
{CODE-TAB:csharp:Query fts_24@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:Query_async fts_25@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:DocumentQuery fts_26@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies" 
where Contact.Title == "Owner" and
(search(Address.Country, "France") or search(Name, "Markets"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__AND search calls__:

{CODE-TABS}
{CODE-TAB:csharp:Query fts_27@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:Query_async fts_28@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:DocumentQuery fts_29@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees" 
where search(Notes, "French") and search(Title, "Manager")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{NOTE: }

__Use options as bit flags__:

{CODE-TABS}
{CODE-TAB:csharp:Query fts_30@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:Query_async fts_31@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:DocumentQuery fts_32@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
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

  * Searching with a wildcard as the prefix of the term (e.g. `*text`) is less recommended,  
    as it will cause the server to perform a full index scan.
  
  * Instead, consider using a static-index that indexes the field in reverse order  
    and then query with a wildcard as the postfix, which is much faster.  

{NOTE: }

{CODE-TABS}
{CODE-TAB:csharp:Query fts_33@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:Query_async fts_34@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
{CODE-TAB:csharp:DocumentQuery fts_35@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}
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

{CODE syntax@ClientApi\Session\Querying\TextSearch\FullTextSearch.cs /}

| Parameter         | Type                                | Description                                                                                                                                                              |
|-------------------|-------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| __fieldSelector__ | `Expression<Func<TResult>>`         | Points to the field in which you search.                                                                                                                                 |
| __fieldName__     | string                              | Name of the field in which you search.                                                                                                                                   |
| __searchTerms__   | string / <br/>`IEnumerable<string>` | A string containing the term or terms (separated by spaces) to search for.<br/>Or, can pass an array (or other `IEnumerable`) with terms to search for.                  |
| __boost__         | decimal                             | The boost value.<br>Learn more in [boost search results](../../../../client-api/session/querying/text-search/boost-search-results).<br><strong>Default</strong> is `1.0` |
| __options__       | `SearchOptions` enum                | Logical operator to use between consecutive Search methods.<br> Can be `Or`, `And`, `Not`, or `Guess`.<br><strong>Default</strong> is `SearchOptions.Guess`              |
| __@operator__     | `SearchOperator` enum               | Logical operator to use between multiple terms in the same Search method.<br>Can be `Or` or `And`.<br><strong>Default</strong> is `SearchOperation.Or`                   |

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [How to Use Regex](../../../../client-api/session/querying/text-search/using-regex)
- [How to Query With Exact Match](../../../../client-api/session/querying/text-search/exact-match-search)

### Indexes

- [Analyzers](../../../../indexes/using-analyzers)
- [Full-text search in index](../../../../indexes/querying/searching)
