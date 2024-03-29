# Query With Exact Match

By default, the `Where` method in `Query` uses a case-insensitive match.

To perform a case-sensitive match you should use the `exact` parameter.

### Syntax

{CODE query_1_0@ClientApi\Session\Querying\HowToQueryWithExactMatch.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **predicate** | Expression<Func<T, int, bool>> | Predicate with match condition |
| **exact** | bool | Indicates if `predicate` should be matched in case-sensitive manner |

###Example I - Query With Exact Match

{CODE-TABS}
{CODE-TAB:csharp:Sync query_1_1@ClientApi\Session\Querying\HowToQueryWithExactMatch.cs /}
{CODE-TAB:csharp:Async query_1_1_async@ClientApi\Session\Querying\HowToQueryWithExactMatch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees where exact(FirstName == 'Robert')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II - Query With Inner Exact Match

{CODE-TABS}
{CODE-TAB:csharp:Sync query_2_1@ClientApi\Session\Querying\HowToQueryWithExactMatch.cs /}
{CODE-TAB:csharp:Async query_2_1_async@ClientApi\Session\Querying\HowToQueryWithExactMatch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Orders 
where exact(Lines[].ProductName == 'Singaporean Hokkien Fried Mee')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [How to Use Search](../../../client-api/session/querying/how-to-use-search)
- [How to Use Regex](../../../client-api/session/querying/how-to-use-regex)
