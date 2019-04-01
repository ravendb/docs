# Session: Querying: How to Work with Suggestions

The `suggestion` feature is available through query extension methods. It gives you the ability to find word similarities using string distance algorithms.

## Syntax

{CODE:nodejs suggest_1@client-api\session\querying\howToWorkWithSuggestions.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **suggestion** | `SuggestionBase`  | Defines the type of suggestion that should be executed |
| **suggestionBuilder** | `(SuggestionBuilder) => void` | Builder with a fluent API that constructs a `SuggestionBase` instance |

### Builder

{CODE:nodejs suggest_2@client-api\session\querying\howToWorkWithSuggestions.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | string | Points to the index field that should be used for operation |
| **term** | string | Term that will be used as a basis of the suggestions |
| **terms** | string[] | Terms that will be used as a basis of the suggestions |
| **options** | object | Non-default options that should be used for operation |
| &nbsp;&nbsp;&nbsp;*pageSize* | number | Maximum number of suggestions that will be returned |
| &nbsp;&nbsp;&nbsp;*distance* | `StringDistanceTypes` | String distance algorithm to use (`None`, `Levenshtein`, `JaroWinkler`, `NGram`) |
| &nbsp;&nbsp;&nbsp;*accuracy* | number | Suggestion accuracy |
| &nbsp;&nbsp;&nbsp;*sortMode* | `SuggestionSortMode` | Indicates in what order the results should be returned (`None`, `Popularity`) |

## Example I

{CODE-TABS}
{CODE-TAB:nodejs:Node.js suggest_5@client-api\session\querying\howToWorkWithSuggestions.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFullName' 
select suggest('FullName', 'johne', '{ "Accuracy" : 0.4, "PageSize" : 5, "Distance" : "JaroWinkler", "SortMode" : "Popularity" }')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example II

{CODE-TABS}
{CODE-TAB:nodejs:Node.js suggest_8@client-api\session\querying\howToWorkWithSuggestions.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFullName' 
select suggest('FullName', 'johne')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Querying

- [Suggestions](../../../indexes/querying/suggestions)
