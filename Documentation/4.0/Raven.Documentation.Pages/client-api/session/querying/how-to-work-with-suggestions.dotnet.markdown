# Session : Querying : How to work with suggestions?

Session `Query` method contains extension (`SuggestUsing`) that allows you to use the suggestion feature.

## Syntax

{CODE suggest_1@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **suggestion** | `SuggestionBase`  | Defines the type of suggestion that should be executed |
| **factory** | `Action<ISuggestionFactory<T>>` | Factory with fluent API that builds `SuggestionBase` instance |

### Options

{CODE suggest_7@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}

| Options | | |
| ------------- | ------------- | ----- |
| **PageSize** | int | Maximum number of suggestions that will be returned |
| **Distance** | `StringDistanceTypes` | String distance algorithm to use (`None`, `Levenshtein`, `JaroWinkler`, `NGram`) |
| **Accuracy** | float? | Suggestion accuracy |
| **SortMode** | `SuggestionSortMode` | Indicates in what order results should be returned (`None`, `Popularity`) |

### Factory

{CODE suggest_2@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **fieldName** | string | Points to index field that should be used for operation |
| **path** | `Expression<Func<T, object>>` | Points to index field that should be used for operation |
| **term** | string | Term that will be used as a basis of the suggestions |
| **terms** | string[] | Terms that will be used as a basis of the suggestions |
| **options** | `SuggestionOptions` | Non-default options that should be used for operation |

## Example I

{CODE-TABS}
{CODE-TAB:csharp:Sync suggest_5@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}
{CODE-TAB:csharp:Async suggest_6@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Employees/ByFullName' 
where suggest('FullName', 'johne', '{ Accuracy : 0.4, PageSize : 5, Distance : "JaroWinkler", SortMode : "Popularity" }')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example II

{CODE-TABS}
{CODE-TAB:csharp:Sync suggest_8@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}
{CODE-TAB:csharp:Async suggest_9@ClientApi\Session\Querying\HowToWorkWithSuggestions.cs /}
{CODE-TAB-BLOCK:csharp:RQL}
from index 'Employees/ByFullName' 
where suggest('FullName', 'johne')
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related articles

- [Indexes : Suggestions](../../../indexes/querying/suggestions)
