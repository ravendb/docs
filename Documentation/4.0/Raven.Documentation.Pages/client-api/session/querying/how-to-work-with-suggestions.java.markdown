# Session: Querying: How to Work with Suggestions

The `suggestion` feature is available through query extension methods. It gives you the ability to find word similarities using string distance algorithms.

## Syntax

{CODE:java suggest_1@ClientApi\Session\Querying\HowToWorkWithSuggestions.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **suggestion** | `SuggestionBase`  | Defines the type of suggestion that should be executed |
| **builder** | `Consumer<ISuggestionBuilder<T>>` | Builder with a fluent API that constructs a `SuggestionBase` instance |

### Builder

{CODE:java suggest_2@ClientApi\Session\Querying\HowToWorkWithSuggestions.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | String | Points to the index field that should be used for operation |
| **term** | String | Term that will be used as a basis of the suggestions |
| **terms** | String[] | Terms that will be used as a basis of the suggestions |
| **options** | `SuggestionOptions` | Non-default options that should be used for operation |

### Options

{CODE:java suggest_7@ClientApi\Session\Querying\HowToWorkWithSuggestions.java /}

| Options | | |
| ------------- | ------------- | ----- |
| **pageSize** | int | Maximum number of suggestions that will be returned |
| **distance** | `StringDistanceTypes` | String distance algorithm to use (`NONE`, `LEVENSTEIN`, `JARO_WINKLER`, `N_GRAM`) |
| **accuracy** | Float | Suggestion accuracy |
| **sortMode** | `SuggestionSortMode` | Indicates in what order the results should be returned (`None`, `Popularity`) |

## Example I

{CODE-TABS}
{CODE-TAB:java:Java suggest_5@ClientApi\Session\Querying\HowToWorkWithSuggestions.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFullName' 
select suggest('FullName', 'johne', '{ "Accuracy" : 0.4, "PageSize" : 5, "Distance" : "JaroWinkler", "SortMode" : "Popularity" }')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example II

{CODE-TABS}
{CODE-TAB:java:Java suggest_8@ClientApi\Session\Querying\HowToWorkWithSuggestions.java /}
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
