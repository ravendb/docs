# Starts-With Search

---

{NOTE: }

* Use `whereStartsWith` to query for documents having a field that starts with some specified string.

* Unless explicitly specified, the string comparisons are case-insensitive by default,  
  see examples below.


* In this page:
  * [whereStartsWith](../../../../client-api/session/querying/text-search/starts-with-search#startswith)
  * [Negate whereStartsWith](../../../../client-api/session/querying/text-search/starts-with-search#negate-startswith)  
  * [whereStartsWith (case-sensitive)](../../../../client-api/session/querying/text-search/starts-with-search#startswith-(case-sensitive))
  * [Syntax](../../../../client-api/session/querying/text-search/starts-with-search#syntax)

{NOTE/}

---

{PANEL: whereStartsWith}

{CODE-TABS}
{CODE-TAB:nodejs:Query startsWith_1@ClientApi\Session\Querying\TextSearch\startsWith.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where startsWith(Name, "Ch")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Negate whereStartsWith}

{CODE-TABS}
{CODE-TAB:nodejs:Query startsWith_2@ClientApi\Session\Querying\TextSearch\StartsWith.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where exists(Name) and not startsWith(Name, "Ch")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: whereStartsWith (case-sensitive)}

{CODE-TABS}
{CODE-TAB:nodejs:Query startsWith_3@ClientApi\Session\Querying\TextSearch\StartsWith.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where exact(startsWith(Name, "Ch"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Session\Querying\TextSearch\StartsWith.js /}

| Parameter     | Type    | Description                                                               |
|---------------|---------|---------------------------------------------------------------------------|
| __fieldName__ | string  | The field name in which to search                                         |
| __value__     | string  | The prefix string to search by                                            |
| __exact__     | boolean | `false` - search is case-insensitive<br>`true` - search is case-sensitive |


{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [EndsWith search](../../../../client-api/session/querying/text-search/ends-with-search)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)


