# Commands: Querying: How to query a database?

Use **Query** method to fetch results of a selected index according to a specified query.

## Syntax

{CODE query_database_1@ClientApi\Commands\Querying\HowToQueryDatabase.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **index** | string | A name of an index to query |
| **query** | [IndexQuery](../../../glossary/index-query) | A query definition containing all information required to query a specified index. |
| **includes** | string[] | An array of relative paths that specify related documents ids which should be included in a query result. |
| **metadataOnly** | bool | True if returned documents should include only metadata without a document body. |
| **indexEntriesOnly** | bool | True if query results should contain only index entries. |

| Return Value | |
| ------------- | ----- |
| [QueryResult](../../../glossary/query-result) | Object which represents results of a specified query. |

## Example I

A sample **Query** method call that returns orders for a company specified:

{CODE query_database_2@ClientApi\Commands\Querying\HowToQueryDatabase.cs /}

## Example II

If a model of your documents is such that they reference others and you want to retrieve them together in a single query request, then you need to specify paths to properties that contain IDs of referenced documents:

{CODE query_database_3@ClientApi\Commands\Querying\HowToQueryDatabase.cs /}

## Related articles

- [Full RavenDB query syntax](../../../indexes/querying/full-query-syntax) 
- [How to **stream query** results?](../../../client-api/commands/querying/how-to-stream-query-results)
