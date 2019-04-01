# Glossary: Facet

### Properties

| Name | Type | Description |
| ------------- | ------------- | ----- |
| **Mode** | [FacetMode](../glossary/facet#facetmode-enum) | The facet mode |
| **Aggregation** | [FacetAggregation](../glossary/facet#facetaggregation-enum-flags) | The aggregation mode |
| **AggregationField** | string | Aggregation field |
| **AggregationType** | string | Aggregation type (i.e type of decimal, int, float, etc) |
| **Name** | string | The aggregation name |
| **DisplayName** | string | The display name |
| **Ranges** | List&lt;string&gt; | Facet ranges |
| **MaxResults** | int? | Maximum results |
| **TermSortMode** | [FacetTermSortMode](../glossary/facet#facettermsortmode-enum) | The facet term sorting mode |
| **IncludeRemainingTerms** | bool | whether to include remaining terms |

<hr />

# FacetMode (enum)

### Members

| Name | Description |
| ---- | ----- |
| **Default** |  Default facet mode |
| **Ranges** | Create facets using supplied ranges |

<hr />

# FacetAggregation (enum flags)

### Members

| Name | Value |
| ---- | ----- |
| **None** | `0` |
| **Count** | `1` |
| **Max** | `2` |
| **Min** | `4` |
| **Average** | `8` |
| **Sum** | `16` |

<hr />

# FacetTermSortMode (enum)

### Members

| Name | Description |
| ---- | ----- |
| **ValueAsc** | Sort by value ascending |
| **ValueDesc** | Sort by value descending |
| **HitsAsc** | Sort by hits ascending |
| **HitsDesc** | Sort by hits descending |
