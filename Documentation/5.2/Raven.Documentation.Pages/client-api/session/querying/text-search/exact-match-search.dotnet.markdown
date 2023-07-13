# Exact Match Search

---

{NOTE: }

* By default, the string comparisons are __case-insensitive__.  

* Use the `exact` parameter to perform a search that is __case-sensitive__.

* In this page:
    * [Query with exact match](../../../../client-api/session/querying/text-search/exact-match-search#query-with-exact-match)
    * [Query with inner exact match](../../../../client-api/session/querying/text-search/exact-match-search#query-with-inner-exact-match)
    * [Syntax](../../../../client-api/session/querying/text-search/exact-match-search#syntax)

{NOTE/}

---

{PANEL: Query with exact match}

{CODE-TABS}
{CODE-TAB:csharp:Query exact_1@ClientApi\Session\Querying\TextSearch\ExactMatch.cs /}
{CODE-TAB:csharp:Query_async exact_2@ClientApi\Session\Querying\TextSearch\ExactMatch.cs /}
{CODE-TAB:csharp:DocumentQuery exact_3@ClientApi\Session\Querying\TextSearch\ExactMatch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where exact(FirstName == "Robert")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Query with inner exact match}

{CODE-TABS}
{CODE-TAB:csharp:Query exact_4@ClientApi\Session\Querying\TextSearch\ExactMatch.cs /}
{CODE-TAB:csharp:Query_async exact_5@ClientApi\Session\Querying\TextSearch\ExactMatch.cs /}
{CODE-TAB:csharp:DocumentQuery exact_6@ClientApi\Session\Querying\TextSearch\ExactMatch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders" 
where exact(Lines[].ProductName == "Teatime Chocolate Biscuits")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE syntax@ClientApi\Session\Querying\TextSearch\ExactMatch.cs /}

| Parameter     | Type                                                        | Description                                                               |
|---------------|-------------------------------------------------------------|---------------------------------------------------------------------------|
| __predicate__ | Expression<Func<T, int, bool>><br>Expression<Func<T, bool>> | Predicate with match condition                                            |
| __exact__     | bool                                                        | `false` - search is case-insensitive<br>`true` - search is case-sensitive |

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)
- [How to Use Regex](../../../../client-api/session/querying/how-to-use-regex)
