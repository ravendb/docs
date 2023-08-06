# Fuzzy Search

---

{NOTE: }

* A __fuzzy search__ retrieves documents containing terms that closely match a given term instead of exact matches, 
  assisting in finding relevant results when the search term is misspelled or has minor variations.

* Fuzzy search is available only via [DocumentQuery](../../../../client-api/session/querying/document-query/what-is-document-query) or RQL.

* Use the `Fuzzy` method when querying with `WhereEquals`.

* In this page:
    * [Fuzzy search example](../../../../client-api/session/querying/text-search/fuzzy-search#fuzzy-search-example)
    * [Syntax](../../../../client-api/session/querying/text-search/fuzzy-search#syntax)

{NOTE/}

---

{PANEL: Fuzzy search example}

{CODE-TABS}
{CODE-TAB:csharp:DocumentQuery fuzzy_1@ClientApi\Session\Querying\TextSearch\FuzzySearch.cs /}
{CODE-TAB:csharp:DocumentQuery_async fuzzy_2@ClientApi\Session\Querying\TextSearch\FuzzySearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
where fuzzy(Name = "Ernts Hnadel", 0.5)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax@ClientApi\Session\Querying\TextSearch\FuzzySearch.cs /}

| Parameter   | Type      | Description                                                                                                       |
|-------------|-----------|-------------------------------------------------------------------------------------------------------------------|
| **fuzzy**   | `decimal` | A value between `0.0` and `1.0`.<br>With a value closer to `1.0`, terms with a higher similarity will be matched. |

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)
