# where_ends_with Query

---

{NOTE: }

* You can query for documents having a field that ends with some specified string.

* Unless explicitly specified, the string comparisons are case-insensitive by default.

* **Note**:  
  This postfix search causes the server to perform a full index scan.  
  Instead, consider using a static index that indexes the field in reverse order  
  and then query with a [prefix search](../../../../client-api/session/querying/text-search/starts-with-query), which is much faster.

* In this page:
    * [where_ends_with](../../../../client-api/session/querying/text-search/ends-with-query#where_ends_with)
    * [where_ends_with (case-sensitive)](../../../../client-api/session/querying/text-search/ends-with-query#where_ends_with-(case-sensitive))
    * [Negate where_ends_with](../../../../client-api/session/querying/text-search/ends-with-query#negate-where_ends_with)

{NOTE/}

---

{PANEL: where_ends_with}

{CODE-TABS}
{CODE-TAB:php:query endsWith_1@ClientApi\Session\Querying\TextSearch\EndsWith.php /}
{CODE-TAB:csharp:documentQuery endsWith_3@ClientApi\Session\Querying\TextSearch\EndsWith.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where endsWith(Name, "Lager")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: where_ends_with (case-sensitive)}

{CODE-TABS}
{CODE-TAB:php:query endsWith_4@ClientApi\Session\Querying\TextSearch\EndsWith.php /}
{CODE-TAB:csharp:documentQuery endsWith_6@ClientApi\Session\Querying\TextSearch\EndsWith.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where exact(endsWith(Name, "Lager"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Negate where_ends_with}

{CODE-TABS}
{CODE-TAB:php:query endsWith_7@ClientApi\Session\Querying\TextSearch\EndsWith.php /}
{CODE-TAB:csharp:documentQuery endsWith_9@ClientApi\Session\Querying\TextSearch\EndsWith.cs /}
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
