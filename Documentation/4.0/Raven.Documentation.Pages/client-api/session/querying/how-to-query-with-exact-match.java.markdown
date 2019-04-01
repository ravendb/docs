# Session: Querying: How to Query With Exact Match

By default, the `whereXXX` methods in `query` uses a case-insensitive match.

To perform a case-sensitive match you should use the `exact` parameter.

### Syntax

{CODE:java query_1_0@ClientApi\Session\Querying\HowToQueryWithExactMatch.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | String | Object field to use |
| **value** | Object | Predicate value |
| **exact** | boolean | Indicates if `predicate` should be matched in case-sensitive manner |

###Example I - Query With Exact Match

{CODE-TABS}
{CODE-TAB:java:Java query_1_1@ClientApi\Session\Querying\HowToQueryWithExactMatch.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees where exact(FirstName == 'Robert')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II - Query With Inner Exact Match

{CODE-TABS}
{CODE-TAB:java:Java query_2_1@ClientApi\Session\Querying\HowToQueryWithExactMatch.java /}
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
