# Session: Querying: How to Perform Proximity Search

To find words within a specific distance away use `proximity` method. This method is available only from [DocumentQuery](../../../client-api/session/querying/document-query/what-is-document-query) level and can only be used right after `search` method.

## Syntax

{CODE:java proximity_1@ClientApi\Session\Querying\HowToPerformProximitySearch.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **proximity** | `int` | Number of words within. |

## Example

{CODE-TABS}
{CODE-TAB:java:Java proximity_2@ClientApi\Session\Querying\HowToPerformProximitySearch.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Foxes
where proximity(search(Name, 'quick fox'), 2)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)
- [How to Use Search](../../../client-api/session/querying/how-to-use-search)
