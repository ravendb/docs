# Commands : Querying : How to query a database?

Use **Query** method to fetch results of a selected index according to a specified query.

## Syntax

{CODE:java query_database_1@ClientApi\Commands\Querying\HowToQueryDatabase.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **index** | String | A name of an index to query |
| **query** | [IndexQuery]() | A query definition containing all information required to query a specified index. |
| **includes** | String[] | An array of relative paths that specify related documents ids which should be included in a query result. (default: `null`) |
| **metadataOnly** | boolean | True if returned documents should include only metadata without a document body. (default: `false`) |
| **indexEntriesOnly** | boolean | True if query results should contain only [index entries](../../../glossary/indexing#index-entry). (default: `false`) |

| Return Value | |
| ------------- | ----- |
| [QueryResult]() | Object which represents results of a specified query. |

## Example I

A sample **Query** method call that returns orders for a company specified:

{CODE:java query_database_2@ClientApi\Commands\Querying\HowToQueryDatabase.java /}

## Example II

If a model of your documents is such that they reference others and you want to retrieve them together in a single query request, then you need to specify paths to properties that contain IDs of referenced documents:

{CODE:java query_database_3@ClientApi\Commands\Querying\HowToQueryDatabase.java /}

## Related articles

- [Full RavenDB query syntax](../../../Indexes/full-query-syntax) 
- [How to **stream query** results?](../../../client-api/commands/querying/how-to-stream-query-results)