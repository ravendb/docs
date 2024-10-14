# Exact Match Query

---

{NOTE: }

* By default, when querying strings the string comparisons are **case-insensitive**.

* To perform a **case-sensitive** search, use the `whereEquals` or `whereNotEquals` 
  method with its `exact` parameter set to `true`.  

* When making a dynamic query with an exact match, the auto-index created by the server indexes 
  the text of the document field using the [default exact analyzer](../../../../indexes/using-analyzers#ravendb) 
  where the casing of the original text is unchanged.

* In this page:
    * [Query with exact match](../../../../client-api/session/querying/text-search/exact-match-query#query-with-exact-match)
    * [Query with exact match - nested object](../../../../client-api/session/querying/text-search/exact-match-query#query-with-exact-match---nested-object)
    * [Syntax](../../../../client-api/session/querying/text-search/exact-match-query#syntax)

{NOTE/}

---

{PANEL: Query with exact match}

{NOTE: }

{CODE-TABS}
{CODE-TAB:php:query exact_1@ClientApi\Session\Querying\TextSearch\ExactMatch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where exact(FirstName == "Robert")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

* Executing the above query will generate the auto-index `Auto/Employees/ByExact(FirstName)`.

* This auto-index will contain the following two index-fields:
  
  * `FirstName`  
    Contains terms with the text from the indexed document field 'FirstName'.  
    Text is lower-cased and not tokenized.  

  * `exact(FirstName)`  
    Contain terms with the original text from the indexed document field 'FirstName'.  
    Casing is exactly the same as in the original text, and the text is not tokenized.  
    Making an exact query targets these terms to find matching documents.

{PANEL/}

{PANEL: Query with exact match - nested object}

{NOTE: }

{CODE-TABS}
{CODE-TAB:php:query exact_4@ClientApi\Session\Querying\TextSearch\ExactMatch.php /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders" 
where exact(Lines.ProductName == "Teatime Chocolate Biscuits")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Syntax}

{CODE:php syntax@ClientApi\Session\Querying\TextSearch\ExactMatch.php /}

| Parameter     | Type                       | Description |
|---------------|----------------------------|-------------|
| **$fieldName** | `string` | Search-field name |
| **$value** | `string` | string to match with match condition |
| **$exact** | `bool` | `false` - search is case-insensitive<br>`true` - search is case-sensitive |

| Return Type | Description |
| ------------- | ----- |
| `DocumentQueryInterface` | Query results |

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)
- [How to Use Regex](../../../../client-api/session/querying/text-search/using-regex)
