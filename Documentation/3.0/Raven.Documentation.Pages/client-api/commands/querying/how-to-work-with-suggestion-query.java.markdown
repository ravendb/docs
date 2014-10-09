# Commands : Querying : How to work with Suggestion query?

To take advantage of a suggestion feature use **Suggest** method from commands.

## Syntax

{CODE:java suggest_1@ClientApi\Commands\Querying\HowToWorkWithSuggestionQuery.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **index** | String | A name of an index to query. |
| **suggestionQuery** | [SuggestionQuery](../../../glossary/client-api/querying/suggestion-query) | A suggestion query definition containing all information required to query a specified index. |

| Return Value | |
| ------------- | ----- |
| [SuggestionQueryResult]() | Result containing array of all suggestions for executed query |

## Example

{CODE:java suggest_2@ClientApi\Commands\Querying\HowToWorkWithSuggestionQuery.java /}

## Related articles

- [Full RavenDB query syntax](../../../indexes/full-query-syntax)   
- [How to **query** a **database**?](../../../client-api/commands/querying/how-to-query-a-database)   
- [How to **stream query** results?](../../../client-api/commands/querying/how-to-stream-query-results)   