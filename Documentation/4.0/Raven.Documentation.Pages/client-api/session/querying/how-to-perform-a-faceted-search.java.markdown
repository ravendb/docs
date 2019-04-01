# Session: Querying: How to Perform a Faceted (Aggregated) Search

To execute facet (aggregation) query using the session `query` method, use the `aggregateBy` or `aggregateUsing` methods. This will scope you to the aggregation query builder where you will be allowed to define single or multiple facets for the query using a straightforward and fluent API.

## Syntax

{CODE:java facet_1@ClientApi\Session\Querying\HowToPerformFacetedSearch.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **facet** | FacetBase | `FacetBase` implementation defining the scope of the facet and its options (either `Facet` or `RangeFacet`) |
| **facets** | `FacetBase...` | Items containing `FacetBase` implementations |
| **builder** | `Consumer<IFacetBuilder<T>>` | Builder with a fluent API that constructs a `FacetBase` instance |
| **facetSetupDocumentId** | String | ID of a document containing `FacetSetup` | 

### Facet & RangeFacet

{INFO:Facet vs RangeFacet}
`RangeFacet` allows you to split the results of the calculations into several ranges, in contrast to `Facet` where whole spectrum of results will be used to generate a single outcome.
{INFO/}

{CODE-TABS}
{CODE-TAB:java:Facet facet_7_3@ClientApi\Session\Querying\HowToPerformFacetedSearch.java /}
{CODE-TAB:java:RangeFacet facet_7_4@ClientApi\Session\Querying\HowToPerformFacetedSearch.java /}
{CODE-TAB:java:FacetAggregation facet_7_5@ClientApi\Session\Querying\HowToPerformFacetedSearch.java /}
{CODE-TABS/}

### Builder

{CODE:java facet_7_1@ClientApi\Session\Querying\HowToPerformFacetedSearch.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **path** | String | Points to the index field that should be used for operation (`byRanges`, `byField`) or to document field that should be used for aggregation (`sumOn`, `minOn`, `maxOn`, `averageOn`) |
| **fieldName** | String | Points to the index field that should be used for operation (`byRanges`, `byField`) or to document field that should be used for aggregation (`sumOn`, `minOn`, `maxOn`, `averageOn`) |
| **displayName** | String | If set, results of a facet will be returned under this name |
| **options** | `FacetOptions` | Non-default options that should be used for operation |

### Options

{CODE:java facet_7_2@ClientApi\Session\Querying\HowToPerformFacetedSearch.java /}

| Options | | |
| ------------- | ------------- | ----- |
| **termSortMode** | `FacetTermSortMode` | Indicates how terms should be sorted (`VALUE_ASC`, `VALUE_DESC`, `COUNT_ASC`, `COUNT_DESC`) |
| **includeRemainingTerms** | booelean | Indicates if remaining terms should be included in results |
| **start** | int | Used to skip given number of facet results in the outcome |
| **pageSize** | int | Used to limit facet results to the given value |

## Example I

{CODE-TABS}
{CODE-TAB:java:Java facet_2_1@ClientApi\Session\Querying\HowToPerformFacetedSearch.java /}
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
{CODE-TAB:java:Java facet_3_1@ClientApi\Session\Querying\HowToPerformFacetedSearch.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Camera/Costs' 
select 
facet(manufacturer), 
facet(cost < 200, cost >= 200 AND cost < 400, cost >= 400 AND cost < 600, cost >= 600 AND cost < 800, cost >= 800),
facet(megapixels < 3, megapixels >= 3 AND megapixels < 7, megapixels >= 7 AND megapixels < 10, megapixels >= 10)
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Example III

{CODE-TABS}
{CODE-TAB:java:Java facet_4_1@ClientApi\Session\Querying\HowToPerformFacetedSearch.java /}
{CODE-TAB-BLOCK:sql:RQL}
from index 'Camera/Costs' 
select facet(id('facets/CameraFacets'))
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Remarks

{WARNING `aggregateBy` only supports aggregation by a single field. If you want to aggregate by multiple fields, you need to emit a single field that contains all values. /}

## Related Articles

### Session

- [How to Query](../../../client-api/session/querying/how-to-query)

### Indexes

- [Faceted Search](../../../indexes/querying/faceted-search) 
