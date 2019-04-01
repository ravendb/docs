# Session: Querying: How to Work with Suggestions

The `Suggestion` feature is available through query extension methods. It gives you the ability to find word similarities using string distance algorithms.

## Syntax

{CODE suggest_1@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **suggestion** | `SuggestionBase`  | Defines the type of suggestion that should be executed |
| **builder** | `Action<ISuggestionBuilder<T>>` | Builder with a fluent API that constructs a `SuggestionBase` instance |

### Builder

{CODE suggest_2@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | string | Points to the index field that should be used for operation |
| **path** | `Expression<Func<T, object>>` | Points to the index field that should be used for operation |
| **term** | string | Term that will be used as a basis of the suggestions |
| **terms** | string[] | Terms that will be used as a basis of the suggestions |
| **displayName** | string | User defined friendly name for suggestion result. If `null`, field name will be used.  |
| **options** | `SuggestionOptions` | Non-default options that should be used for operation |

### Options

{CODE suggest_7@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}

| Options | | |
| ------------- | ------------- | ----- |
| **PageSize** | int | Maximum number of suggestions that will be returned |
| **Distance** | `StringDistanceTypes` | String distance algorithm to use (`None`, `Levenshtein`, `JaroWinkler`, `NGram`) |
| **Accuracy** | float? | Suggestion accuracy |
| **SortMode** | `SuggestionSortMode` | Indicates in what order the results should be returned (`None`, `Popularity`) |

### Multiple suggestions

You are able to ask for multiple suggestions using a single query.

{CODE suggest_3@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}

## Example I

{CODE-TABS}
{CODE-TAB:csharp:Sync suggest_5@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}
{CODE-TAB:csharp:Async suggest_6@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFullName' 
select suggest('FullName', 'johne', '{ "Accuracy" : 0.4, "PageSize" : 5, "Distance" : "JaroWinkler", "SortMode" : "Popularity" }')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example II

{CODE-TABS}
{CODE-TAB:csharp:Sync suggest_8@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}
{CODE-TAB:csharp:Async suggest_9@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Employees/ByFullName' 
select suggest('FullName', 'johne')
{CODE-TAB-BLOCK/}
{CODE-TABS/}


## Example III

Looking for suggestions with dynamic query usage for multiple fields:

{CODE-TABS}
{CODE-TAB:csharp:Sync suggest_10@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}
{CODE-TAB:csharp:Async suggest_11@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from Employees 
select suggest('FirstName', 'johne') as CustomFirstName, suggest('LastName', 'owen')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Querying

- [Suggestions](../../../indexes/querying/suggestions)
