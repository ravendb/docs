# Session: Querying: How to Perform Proximity Search

To find words within a specific distance away use `Proximity` method. This method is available only from [DocumentQuery](../../../client-api/session/querying/document-query/what-is-document-query) level and can only be used right after `Search` method.

## Syntax

{CODE proximity_1@ClientApi\Session\Querying\HowToPerformProximitySearch.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **proximity** | `int` | Number of words within. |

## Example

{CODE-TABS}
{CODE-TAB:csharp:Sync proximity_2@ClientApi\Session\Querying\HowToPerformProximitySearch.cs /}
{CODE-TAB:csharp:Async proximity_3@ClientApi\Session\Querying\HowToPerformProximitySearch.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Foxes
where proximity(search(Name, 'quick fox'), 2)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [How to Use Search](../../../client-api/session/querying/how-to-use-search)
