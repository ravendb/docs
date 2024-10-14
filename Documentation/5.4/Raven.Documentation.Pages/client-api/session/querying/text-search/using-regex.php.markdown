# Using Regex

To return only documents that match a regular expression ("regex"),  
use the `whereRegex` method which enables RavenDB to perform server-side pattern matching queries. 

The supplied regular expression must be [.NET compatible](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex?view=netframework-4.7.1).

## Example

Load all products whose name starts with 'N' or 'A'.

{CODE-TABS}
{CODE-TAB:php regex_1@ClientApi\Session\Querying\TextSearch\UsingRegex.php /}
{CODE-TAB-BLOCK:sql:RQL}
from Products 
where regex(Name, '^[NA]')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Syntax

{CODE:php syntax@ClientApi\Session\Querying\TextSearch\UsingRegex.php /}

| Parameter     | Type  | Description                                                                   |
|---------------|-------|-------------------------------------------------------------------------------|
| **$fieldName** | `?string` | Name of the field to query |
| **$pattern** | `?string` | Pattern to query for |

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)
- [Query with exact match](../../../../client-api/session/querying/text-search/exact-match-query)
