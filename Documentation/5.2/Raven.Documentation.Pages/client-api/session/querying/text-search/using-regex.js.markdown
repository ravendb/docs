# Using Regex

To return only documents that match a given regular expression,  
use the `regex()` method which enables RavenDB to perform server-side pattern matching queries. 

The supplied regular expression must be [.NET compatible](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=netframework-4.7.1).

## Example

{CODE-TABS}
{CODE-TAB:nodejs:Node.js regex_1@ClientApi\Session\Querying\TextSearch\usingRegex.js /}
{CODE-TAB-BLOCK:sql:RQL}
from Products 
where regex(name, '^[NA]')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)
- [Query with exact match](../../../../client-api/session/querying/text-search/exact-match-query)
