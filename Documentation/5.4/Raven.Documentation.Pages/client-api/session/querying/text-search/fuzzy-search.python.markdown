# Fuzzy Search

---

{NOTE: }

* A **fuzzy search** retrieves documents containing terms that closely match a given term instead of exact matches, 
  assisting in finding relevant results when the search term is misspelled or has minor variations.

* Use the `fuzzy` method when querying with `where_equals`.

* In this page:
    * [Fuzzy search example](../../../../client-api/session/querying/text-search/fuzzy-search#fuzzy-search-example)
    * [Syntax](../../../../client-api/session/querying/text-search/fuzzy-search#syntax)

{NOTE/}

---

{PANEL: Fuzzy search example}

{CODE-TABS}
{CODE-TAB:python:Query fuzzy_1@ClientApi\Session\Querying\TextSearch\FuzzySearch.py /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
where fuzzy(Name = "Ernts Hnadel", 0.5)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:python syntax@ClientApi\Session\Querying\TextSearch\FuzzySearch.py /}

| Parameter   | Type      | Description                                                                                                       |
|-------------|-----------|------------------|
| **fuzzy** | `float` | A value between `0.0` and `1.0`.<br>With a value closer to `1.0`, terms with a higher similarity are matched. |

| Return Type | Description |
| ----------- | ----------- |
| `DocumentQuery[_T]` | The same object used for the query |

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)
