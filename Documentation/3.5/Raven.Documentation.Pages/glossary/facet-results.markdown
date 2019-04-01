# Glossary: FacetResults

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Results** | Dictionary&lt;string, [FacetResult](../glossary/facet-results#facetresult)&gt; | A list of results for the facet.  One entry for each term/range as specified in the facet setup document. |
| **Duration** | TimeSpan | Operation duration |

<hr />

# FacetResult

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Values** | List&lt;[FacetValue](../glossary/facet-results#facetvalue)&gt; | The facet terms and hits up to a limit of MaxResults items (as specified in the facet setup document), sorted in TermSortMode order (as indicated in the facet setup document). |
| **RemainingTermsCount** | int | The number of remaining terms outside of those covered by the Values terms. |
| **RemainingHits** | int | The number of remaining hits outside of those covered by the Values terms. |

<hr />

# FacetValue

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Range** | string | Facet range |
| **Hits** | int | Number of terms that are covered by this facet. |
| **Count** | int? | Used for storing count |
| **Sum** | double? | Used for storing sum |
| **Max** | double? | Used for storing max |
| **Min** | double? | Used for storing min |
| **Average** | double? | Used for storing average |

### Methods

| Signature | Description |
| ----------| ----- |
| **double? GetAggregation(FacetAggregation aggregation)** | Returns aggregation by given type |
