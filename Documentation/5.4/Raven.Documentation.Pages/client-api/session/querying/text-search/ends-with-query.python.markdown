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
    * [EndsWith](../../../../client-api/session/querying/text-search/ends-with-query#endswith)
    * [EndsWith (case-sensitive)](../../../../client-api/session/querying/text-search/ends-with-query#endswith-(case-sensitive))
    * [Negate EndsWith](../../../../client-api/session/querying/text-search/ends-with-query#negate-endswith)

{NOTE/}

---

{PANEL: EndsWith}

{CODE-TABS}
{CODE-TAB:python:Query endsWith_1@ClientApi\Session\Querying\TextSearch\EndsWith.py /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where endsWith(Name, "Lager")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: EndsWith (case-sensitive)}

{CODE-TABS}
{CODE-TAB:python:Query endsWith_4@ClientApi\Session\Querying\TextSearch\EndsWith.py /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where exact(endsWith(Name, "Lager"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Negate EndsWith}

{CODE-TABS}
{CODE-TAB:python:Query endsWith_7@ClientApi\Session\Querying\TextSearch\EndsWith.py /}
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
