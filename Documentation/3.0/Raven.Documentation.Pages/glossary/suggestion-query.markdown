# Glossary: SuggestionQuery

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Term** | string | The term is what the user likely entered, and will used as the basis of the suggestions |
| **Field** | string | The field to be used in conjunction with the index |
| **MaxSuggestions** | int | The number of suggestions to return |
| **Distance** | [StringDistanceTypes](../glossary/suggestion-query#stringdistancetypes-enum)? | The string distance algorithm |
| **Accuracy** | float? | The accuracy |
| **Popularity** | bool | Whatever to return the terms in order of popularity |

<hr />

# StringDistanceTypes (enum)

### Members

| Name | Description |
| ---- | ----- |
| **None** |  Default, suggestion is not active |
| **Default** | Default, equivalent to Levenshtein |
| **Levenshtein** | Levenshtein distance algorithm (default) |
| **JaroWinkler** | JaroWinkler distance algorithm |
| **NGram** | NGram distance algorithm |
