# Fuzzy Search

Fuzzy search is supported via `Fuzzy` method.   
This method is available only from [DocumentQuery](../../../../client-api/session/querying/document-query/what-is-document-query) level and can only be performed on single term values.  
Because of that it can be used only right after `WhereEquals` method.

## Syntax

{CODE fuzzy_1@ClientApi\Session\Querying\TextSearch\FuzzySearch.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fuzzy** | `decimal` | Value between 0.0 and 1.0 where 1.0 means closer match. |

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync fuzzy_2@ClientApi\Session\Querying\TextSearch\FuzzySearch.cs /}
{CODE-TAB:csharp:Async fuzzy_3@ClientApi\Session\Querying\TextSearch\FuzzySearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Companies
where fuzzy(Name = 'Ernts Hnadel', 0.5)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Session

- [Query overview](../../../../client-api/session/querying/how-to-query)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)
