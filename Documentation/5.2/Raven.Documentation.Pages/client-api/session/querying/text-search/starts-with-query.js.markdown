# Starts-With Query

---

{NOTE: }

* Use `whereStartsWith` to query for documents having a field that starts with some specified string.

* Unless explicitly specified, the string comparisons are case-insensitive by default.

* In this page:
  * [whereStartsWith](../../../../client-api/session/querying/text-search/starts-with-query#wherestartswith)  
  * [whereStartsWith (case-sensitive)](../../../../client-api/session/querying/text-search/starts-with-query#wherestartswith-(case-sensitive))  
  * [Negate whereStartsWith](../../../../client-api/session/querying/text-search/starts-with-query#negate-wherestartswith)  
  * [Syntax](../../../../client-api/session/querying/text-search/starts-with-query#syntax)  

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

{PANEL: whereStartsWith (case-sensitive)}

{CODE-TABS}
{CODE-TAB:nodejs:Query startsWith_2@ClientApi\Session\Querying\TextSearch\StartsWith.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where exact(startsWith(Name, "Ch"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Negate whereStartsWith}

{CODE-TABS}
{CODE-TAB:nodejs:Query startsWith_3@ClientApi\Session\Querying\TextSearch\StartsWith.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where exists(Name) and not startsWith(Name, "Ch")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Session\Querying\TextSearch\StartsWith.js /}

| Parameter     | Type    | Description                                                               |
|---------------|---------|---------------------------------------------------------------------------|
| __fieldName__ | string  | The field name in which to search                                         |
| __value__     | string  | The __prefix__ string to search by                                        |
| __exact__     | boolean | `false` - search is case-insensitive<br>`true` - search is case-sensitive |


{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Ends-With query](../../../../client-api/session/querying/text-search/ends-with-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)


