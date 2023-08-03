# Proximity Search

To find words within a specific distance away use `proximity` method.  
This method is available only from [DocumentQuery](../../../../client-api/session/querying/document-query/what-is-document-query) level and can only be used right after `search` method.

## Syntax

{CODE:java proximity_1@ClientApi\Session\Querying\TextSearch\ProximitySearch.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **proximity** | `int` | Number of words within. |

## Example

{CODE-TABS}
{CODE-TAB:java:Java proximity_2@ClientApi\Session\Querying\TextSearch\ProximitySearch.java /}
{CODE-TAB-BLOCK:sql:RQL}
from Foxes
where proximity(search(Name, 'quick fox'), 2)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Session

- [Querying: Basics](../../../../indexes/querying/query-index)
- [Full-text search](../../../../client-api/session/querying/text-search/full-text-search)
