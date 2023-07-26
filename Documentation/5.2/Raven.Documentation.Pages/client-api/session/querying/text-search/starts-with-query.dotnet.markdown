# Starts-With Query

---

{NOTE: }

* You can query for documents having a field that starts with some specified string.  

* Unless explicitly specified, the string comparisons are case-insensitive by default.

* In this page:
  * [StartsWith](../../../../client-api/session/querying/text-search/starts-with-search#startswith)
  * [StartsWith (case-sensitive)](../../../../client-api/session/querying/text-search/starts-with-search#startswith-(case-sensitive))
  * [Negate StartsWith](../../../../client-api/session/querying/text-search/starts-with-search#negate-startswith)

{NOTE/}

---

{PANEL: StartsWith}

{CODE-TABS}
{CODE-TAB:csharp:Query startsWith_1@ClientApi\Session\Querying\TextSearch\StartsWith.cs /}
{CODE-TAB:csharp:Query_async startsWith_2@ClientApi\Session\Querying\TextSearch\StartsWith.cs /}
{CODE-TAB:csharp:DocumentQuery startsWith_3@ClientApi\Session\Querying\TextSearch\StartsWith.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where startsWith(Name, "Ch")
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: StartsWith (case-sensitive)}

{CODE-TABS}
{CODE-TAB:csharp:Query startsWith_4@ClientApi\Session\Querying\TextSearch\StartsWith.cs /}
{CODE-TAB:csharp:Query_async startsWith_5@ClientApi\Session\Querying\TextSearch\StartsWith.cs /}
{CODE-TAB:csharp:DocumentQuery startsWith_6@ClientApi\Session\Querying\TextSearch\StartsWith.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where exact(startsWith(Name, "Ch"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Negate StartsWith}

{CODE-TABS}
{CODE-TAB:csharp:Query startsWith_7@ClientApi\Session\Querying\TextSearch\StartsWith.cs /}
{CODE-TAB:csharp:Query_async startsWith_8@ClientApi\Session\Querying\TextSearch\StartsWith.cs /}
{CODE-TAB:csharp:DocumentQuery startsWith_9@ClientApi\Session\Querying\TextSearch\StartsWith.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from "Products"
where (true and not startsWith(Name, "Ch"))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Ends-With query](../../../../client-api/session/querying/text-search/ends-with-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)


