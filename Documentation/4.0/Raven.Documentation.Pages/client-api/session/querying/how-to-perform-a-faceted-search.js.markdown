# Session: Querying: How to Perform a Faceted (Aggregated) Search

To execute facet (aggregation) query using the session `query()` method, use the `aggregateBy()` or `aggregateUsing()` methods. This will scope you to the aggregation query builder where you will be allowed to define single or multiple facets for the query using a straightforward and fluent API.

## Syntax

{CODE:nodejs facet_1@client-api\session\querying\howToPerformFacetedSearch.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **facet** | `FacetBase` | `FacetBase` implementation defining the scope of the facet and its options (either `Facet` or `RangeFacet`) |
| **facets** | `...FacetBase` | Items containing `FacetBase` implementations |
| **facetBuilder** | `(builder) => void` | Builder with a fluent API that constructs a `FacetBase` instance |
| **facetSetupDocumentId** | string | ID of a document containing `FacetSetup` | 

### Facet & RangeFacet

{INFO:Facet vs RangeFacet}
`RangeFacet` allows you to split the results of the calculations into several ranges, in contrast to `Facet` where whole spectrum of results will be used to generate a single outcome.
{INFO/}

`FacetAggregation` is one of the following:

* `"None"` 

* `"Max"` 

* `"Min"` 

* `"Average"`

* `"Sum"`

#### Facet

| Fields | |
| ------------- | ------------- |
| **fieldName** | string |
| **options** | `FacetOptions` |
| **aggregrations** | `Map<FieldAggregration, string>` |
| **displayFieldName** | string |

#### RangeFacet

| Fields | |
| ------------- | ------------- |
| **ranges** | string[] |
| **options** | `FacetOptions` |
| **aggregrations** | `Map<FieldAggregration, string>` |
| **displayFieldName** | string |

### Builder

{CODE:nodejs facet_7_1@client-api\session\querying\howToPerformFacetedSearch.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | string | Points to the index field that should be used for operation (`byRanges()`, `byField()`) or to document field that should be used for aggregation (`sumOn()`, `minOn()`, `maxOn()`, `averageOn()`) |
| **fieldName** | string | Points to the index field that should be used for operation (`byRanges()`, `byField()`) or to document field that should be used for aggregation (`sumOn()`, `minOn()`, `maxOn()`, `averageOn()`) |
| **displayName** | string | If set, results of a facet will be returned under this name |
| **options** | object | Non-default options that should be used for operation |
| &nbsp;&nbsp;**termSortMode** | string | Indicates how terms should be sorted (`ValueAsc`, `ValueDesc`, `CountAsc`, `CountDesc`) |
| &nbsp;&nbsp;**includeRemainingTerms** | boolean | Indicates if remaining terms should be included in results |
| &nbsp;&nbsp;**start** | number | Used to skip given number of facet results in the outcome |
| &nbsp;&nbsp;**pageSize** | number | Used to limit facet results to the given value |

## Example I

{CODE-TABS}
{CODE-TAB:nodejs:Node.js facet_2_1@client-api\session\querying\howToPerformFacetedSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Camera/Costs' 
select 
facet(manufacturer), 
facet(cost < 200, cost >= 200 AND cost < 400, cost >= 400 AND cost < 600, cost >= 600 AND cost < 800, cost >= 800),
facet(megapixels < 3, megapixels >= 3 AND megapixels < 7, megapixels >= 7 AND megapixels < 10, megapixels >= 10)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example II

{CODE-TABS}
{CODE-TAB:nodejs:Node.js facet_3_1@client-api\session\querying\howToPerformFacetedSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Camera/Costs' 
select 
facet(manufacturer), 
facet(cost < 200, cost >= 200 and cost < 400, cost >= 400 and cost < 600, cost >= 600 and cost < 800, cost >= 800),
facet(megapixels < 3, megapixels >= 3 AND megapixels < 7, megapixels >= 7 AND megapixels < 10, megapixels >= 10)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example III

{CODE-TABS}
{CODE-TAB:nodejs:Node.js facet_4_1@client-api\session\querying\howToPerformFacetedSearch.js /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Camera/Costs' 
select facet(id('facets/CameraFacets'))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Remarks

{WARNING `aggregateBy()` only supports aggregation by a single field. If you want to aggregate by multiple fields, you need to emit a single field that contains all values. /}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Indexes

- [Faceted Search](../../../indexes/querying/faceted-search) 
