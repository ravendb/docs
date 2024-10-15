# Fuzzy Search

---

{NOTE: }

* A **fuzzy search** retrieves documents containing terms that closely match a given term 
  rather than exact matches, assisting in finding relevant results when the search term is 
  misspelled or has minor variations.

* In this page:
    * [Fuzzy search example](../../../../client-api/session/querying/text-search/fuzzy-search#fuzzy-search-example)
    * [Syntax](../../../../client-api/session/querying/text-search/fuzzy-search#syntax)

{NOTE/}

---

{PANEL: Fuzzy search example}

{CODE-TABS}
{CODE-TAB:php:query fuzzy_1@ClientApi\Session\Querying\TextSearch\FuzzySearch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
where fuzzy(Name = "Ernts Hnadel", 0.5)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax@ClientApi\Session\Querying\TextSearch\FuzzySearch.php /}

| Parameter   | Type      | Description |
|-------------|-----------|-------------|
| **$fuzzy** | `float` | A value between `0.0` and `1.0`.<br>With a value closer to `1.0`, terms with a higher similarity are matched. |

| Return Type | Description |
| ----------- | ----------- |
| `DocumentQueryInterface` | Query results |

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)
