# Ends-With Query

---

{NOTE: }

* Use `whereEndsWith` to query for documents having a field that ends with some specified string.  

* Unless explicitly specified, the string comparisons are case-insensitive by default.

* __Note__:  
  This postfix search causes the server to perform a full index scan.  
  Instead, consider using a static index that indexes the field in reverse order  
  and then query with a [prefix search](../../../../client-api/session/querying/text-search/starts-with-search), which is much faster.

* In this page:
    * [whereEndsWith](../../../../client-api/session/querying/text-search/starts-with-search#wherestartswith)
    * [whereEndsWith (case-sensitive)](../../../../client-api/session/querying/text-search/starts-with-search#wherestartswith-(case-sensitive))
    * [Negate whereEndsWith](../../../../client-api/session/querying/text-search/starts-with-search#negate-wherestartswith)
    * [Syntax](../../../../client-api/session/querying/text-search/starts-with-search#syntax)

{NOTE/}

---

{PANEL: EndsWith}

{CODE-TABS}
{CODE-TAB:nodejs:Query endsWith_1@ClientApi\Session\Querying\TextSearch\endsWith.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where endsWith(Name, "Lager")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: EndsWith (case-sensitive)}

{CODE-TABS}
{CODE-TAB:nodejs:Query endsWith_2@ClientApi\Session\Querying\TextSearch\endsWith.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where exact(endsWith(Name, "Lager"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Negate EndsWith}

{CODE-TABS}
{CODE-TAB:nodejs:Query endsWith_3@ClientApi\Session\Querying\TextSearch\endsWith.js /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where exists(Name) and not endsWith(Name, "Lager")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Syntax}

{CODE:nodejs syntax@ClientApi\Session\Querying\TextSearch\StartsWith.js /}

| Parameter     | Type    | Description                                                               |
|---------------|---------|---------------------------------------------------------------------------|
| __fieldName__ | string  | The field name in which to search                                         |
| __value__     | string  | The __postfix__ string to search by                                       |
| __exact__     | boolean | `false` - search is case-insensitive<br>`true` - search is case-sensitive |


{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Starts-With query](../../../../client-api/session/querying/text-search/starts-with-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)

### Indexes

- [map indexes](../../../../indexes/map-indexes)
