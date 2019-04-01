# Session: Querying: How to Query With Exact Match

By default, the `whereXXX()` methods in `query` use a case-insensitive match.

To perform a case-sensitive match you should use the `exact` parameter.

### Syntax

{CODE:nodejs query_1_0@client-api\session\querying\howToQueryWithExactMatch.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | string | Object field to use |
| **value** | any | Predicate value |
| **exact** | boolean | Indicates if `predicate` should be matched in case-sensitive manner |

###Example I - Query With Exact Match

{CODE-TABS}
{CODE-TAB:nodejs:Node.js query_1_1@client-api\session\querying\howToQueryWithExactMatch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees where exact(FirstName == 'Robert')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example II - Query With Inner Exact Match

{CODE-TABS}
{CODE-TAB:nodejs:Node.js query_2_1@client-api\session\querying\howToQueryWithExactMatch.js /}
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
