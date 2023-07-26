# Ends-With Query

---

{NOTE: }

* You can query for documents having a field that ends with some specified string.

* Unless explicitly specified, the string comparisons are case-insensitive by default.

* __Note__:  
  This postfix search causes the server to perform a full index scan.  
  Instead, consider using a static index that indexes the field in reverse order  
  and then query with a [prefix search](../../../../client-api/session/querying/text-search/starts-with-query), which is much faster.

* In this page:
    * [EndsWith](../../../../client-api/session/querying/text-search/starts-with-search#startswith)
    * [EndsWith (case-sensitive)](../../../../client-api/session/querying/text-search/starts-with-search#startswith-(case-sensitive))
    * [Negate EndsWith](../../../../client-api/session/querying/text-search/starts-with-search#negate-startswith)

{NOTE/}

---

{PANEL: EndsWith}

{CODE-TABS}
{CODE-TAB:csharp:Query endsWith_1@ClientApi\Session\Querying\TextSearch\EndsWith.cs /}
{CODE-TAB:csharp:Query_async endsWith_2@ClientApi\Session\Querying\TextSearch\EndsWith.cs /}
{CODE-TAB:csharp:DocumentQuery endsWith_3@ClientApi\Session\Querying\TextSearch\EndsWith.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where endsWith(Name, "Lager")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: EndsWith (case-sensitive)}

{CODE-TABS}
{CODE-TAB:csharp:Query endsWith_4@ClientApi\Session\Querying\TextSearch\EndsWith.cs /}
{CODE-TAB:csharp:Query_async endsWith_5@ClientApi\Session\Querying\TextSearch\EndsWith.cs /}
{CODE-TAB:csharp:DocumentQuery endsWith_6@ClientApi\Session\Querying\TextSearch\EndsWith.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where exact(endsWith(Name, "Lager"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Negate EndsWith}

{CODE-TABS}
{CODE-TAB:csharp:Query endsWith_7@ClientApi\Session\Querying\TextSearch\EndsWith.cs /}
{CODE-TAB:csharp:Query_async endsWith_8@ClientApi\Session\Querying\TextSearch\EndsWith.cs /}
{CODE-TAB:csharp:DocumentQuery endsWith_9@ClientApi\Session\Querying\TextSearch\EndsWith.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where (true and not endsWith(Name, "Lager"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Starts-With query](../../../../client-api/session/querying/text-search/starts-with-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)

### Indexes

- [map indexes](../../../../indexes/map-indexes)
