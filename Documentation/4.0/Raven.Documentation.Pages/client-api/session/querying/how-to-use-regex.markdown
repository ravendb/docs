# Session : Querying : How to Use Regexp

To return only documents that match regular expression, use the `Regex` method which enables RavenDB to perform server-side pattern matching queries. 

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync regex_1@ClientApi\Session\Querying\HowToUseRegex.cs /}
{CODE-TAB:csharp:Async regex_1_async@ClientApi\Session\Querying\HowToUseRegex.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from Users 
where regex(FirstName, '^[NA]')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

