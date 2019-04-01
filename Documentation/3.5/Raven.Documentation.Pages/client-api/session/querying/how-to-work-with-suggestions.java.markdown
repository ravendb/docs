# Session: Querying: How to work with suggestions?

Session `query` allows you to use suggestion feature.

## Syntax

{CODE:java suggest_1@ClientApi\Session\Querying\HowToWorkWithSuggestions.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **query** | [SuggestionQuery](../../../glossary/suggestion-query) | A suggestion query definition containing all information required to query a specified index. |

| Return Value | |
| ------------- | ----- |
| [SuggestionQueryResult](../../../glossary/suggestion-query-result) | Result containing array of all suggestions for executed query. |

## Example

{CODE:java suggest_2@ClientApi\Session\Querying\HowToWorkWithSuggestions.java /}

## Related articles

- [Indexes : Suggestions](../../../indexes/querying/suggestions)
