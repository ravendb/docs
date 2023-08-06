# Fuzzy Search

---

{NOTE: }

* A __fuzzy search__ retrieves documents containing terms that closely match a given term instead of exact matches, 
  assisting in finding relevant results when the search term is misspelled or has minor variations.

* Use the `fuzzy` method when querying with `whereEquals`.

* In this page:
    * [Fuzzy search](../../../../client-api/session/querying/text-search/proximity-search#fuzzy-search)
    * [Syntax](../../../../client-api/session/querying/text-search/proximity-search#syntax)

{NOTE/}

---

{PANEL: Fuzzy search}

{CODE-TABS}
{CODE-TAB:nodejs:Query fuzzy@ClientApi\Session\Querying\TextSearch\fuzzySearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Companies"
where fuzzy(Name = "Ernts Hnadel", 0.5)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Session\Querying\TextSearch\fuzzySearch.js /}

| Parameter   | Type     | Description                                                                                                       |
|-------------|----------|-------------------------------------------------------------------------------------------------------------------|
| **fuzzy**   | `number` | A value between `0.0` and `1.0`.<br>With a value closer to `1.0`, terms with a higher similarity will be matched. |

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)
