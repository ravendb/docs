# Commands: Querying: How to stream query results?

Use **StreamQuery** method to stream results of a selected index according to a specified query.

## Syntax

{CODE:java stream_query_1@ClientApi\Commands\Querying\HowToStreamQueryResults.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **index** | String | A name of an index to query |
| **query** | [IndexQuery](../../../glossary/index-query) | A query definition containing all information required to query a specified index. |
| **queryHeaderInfo** | Reference&lt;[QueryHeaderInformation ](../../../glossary/query-header-information)&gt; | Information about performed query |

| Return Value | |
| ------------- | ----- |
| IEnumerator`<RavenJObject>` | Enumerator with query results |
| Reference&lt;[QueryHeaderInformation ](../../../glossary/query-header-information)&gt; | Information about performed query |

## Example

{CODE:java stream_query_2@ClientApi\Commands\Querying\HowToStreamQueryResults.java /}

## Related articles

- [Full RavenDB query syntax](../../../indexes/querying/full-query-syntax) 
- [How to **query** a **database**?](../../../client-api/commands/querying/how-to-query-a-database)
