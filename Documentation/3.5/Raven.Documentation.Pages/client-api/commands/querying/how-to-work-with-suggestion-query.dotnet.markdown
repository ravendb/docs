# Commands: Querying: How to work with Suggestion query?

To take advantage of a suggestion feature use the **Suggest** method from the commands.

## Syntax

{CODE suggest_1@ClientApi\Commands\Querying\HowToWorkWithSuggestionQuery.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **index** | string | A name of an index to query. |
| **suggestionQuery** | [SuggestionQuery](../../../glossary/suggestion-query) | A suggestion query definition containing all information required to query a specified index. |

| Return Value | |
| ------------- | ----- |
| [SuggestionQueryResult](../../../glossary/suggestion-query-result) | Result containing an array of all suggestions for executed query |

## Example

{CODE suggest_2@ClientApi\Commands\Querying\HowToWorkWithSuggestionQuery.cs /}

## Related articles

- [Full RavenDB query syntax](../../../indexes/querying/full-query-syntax)   
- [How to **query** a **database**?](../../../client-api/commands/querying/how-to-query-a-database)   
- [How to **stream query** results?](../../../client-api/commands/querying/how-to-stream-query-results)   
