# Session: Querying: How to Use Regex

To return only documents that match regular expression, use the `Regex` method which enables RavenDB to perform server-side pattern matching queries. 

The supplied regular expression must be [.NET compatible](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=netframework-4.7.1).

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync regex_1@ClientApi\Session\Querying\HowToUseRegex.cs /}
{CODE-TAB:csharp:Async regex_1_async@ClientApi\Session\Querying\HowToUseRegex.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Products 
where regex(Name, '^[NA]')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [How to Use Search](../../../client-api/session/querying/how-to-use-search)
- [How to Query With Exact Match](../../../client-api/session/querying/how-to-query-with-exact-match)
