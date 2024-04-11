# Exact Match Query

---

{NOTE: }

* By default, when making a query that filters by strings, the string comparisons are __case-insensitive__.

* Use the `exact` parameter to perform a search that is __case-sensitive__.

* When making a dynamic query with an exact match,  
  the auto-index created by the server indexes the text of the document field  
  using the [default exact analyzer](../../../../indexes/using-analyzers#ravendb) where the casing of the original text is not changed.

* In this page:
    * [Query with exact match](../../../../client-api/session/querying/text-search/exact-match-query#query-with-exact-match)
    * [Query with exact match - nested object](../../../../client-api/session/querying/text-search/exact-match-query#query-with-exact-match---nested-object)
    * [Syntax](../../../../client-api/session/querying/text-search/exact-match-query#syntax)

{NOTE/}

---

{PANEL: Query with exact match}

{NOTE: }

{CODE-TABS}
{CODE-TAB:nodejs:Query exact_1@client-api\session\querying\textSearch\exactMatch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Employees"
where exact(FirstName == "Robert")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

* Executing the above query will generate the auto-index `Auto/Employees/ByExact(FirstName)`.

* This auto-index will contain the following two index-fields:

    * `FirstName`  
      Contains terms with text from the indexed document field 'FirstName'.  
      Text is lower-cased and not tokenized.

    * `exact(FirstName)`  
      Contain terms with the original text from the indexed document field 'FirstName'.  
      Casing is exactly the same as in the original text, and the text is not tokenized.  
      Making an exact query targets these terms to find matching documents.

{PANEL/}

{PANEL: Query with exact match - nested object}

{NOTE: }

{CODE-TABS}
{CODE-TAB:nodejs:Query exact_2@client-api\session\querying\textSearch\exactMatch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Orders"
where exact(Lines.ProductName == "Teatime Chocolate Biscuits")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{NOTE/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@client-api\session\querying\textSearch\exactMatch.js /}

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
- [How to Use Regex](../../../../client-api/session/querying/text-search/using-regex)
