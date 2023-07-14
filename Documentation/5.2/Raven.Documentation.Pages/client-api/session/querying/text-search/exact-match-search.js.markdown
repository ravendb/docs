# Exact Match Search

---

{NOTE: }

* By default, when making a query that filters by strings, the string comparisons are __case-insensitive__.

* Use the `exact` parameter to perform a search that is __case-sensitive__.

* In this page:
    * [Query with exact match](../../../../client-api/session/querying/text-search/exact-match-search#query-with-exact-match)
    * [Query with inner exact match](../../../../client-api/session/querying/text-search/exact-match-search#query-with-inner-exact-match)
    * [Syntax](../../../../client-api/session/querying/text-search/exact-match-search#syntax)

{NOTE/}

---

{PANEL: Query with exact match}

{CODE-TABS}
{CODE-TAB:nodejs:Query exact_1@ClientApi\Session\Querying\TextSearch\exactMatch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where exact(FirstName == "Robert")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Query with inner exact match}

{CODE-TABS}
{CODE-TAB:nodejs:Query exact_2@ClientApi\Session\Querying\TextSearch\exactMatch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
where exact(Lines.ProductName == "Teatime Chocolate Biscuits")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Session\Querying\TextSearch\exactMatch.js /}

| Parameter     | Type    | Description                                                               |
|---------------|---------|---------------------------------------------------------------------------|
| __fieldName__ | string  | Name of field in which to search                                          |
| __value__     | any     | The value searched for                                                    |
| __exact__     | boolean | `false` - search is case-insensitive<br>`true` - search is case-sensitive |

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)
- [How to Use Regex](../../../../client-api/session/querying/how-to-use-regex)
