# Session: Querying: How to perform a faceted search?

To execute facet query using session `Query` method use `ToFacets` extension. There is also a possibility to convert query straight into `FacetQuery` instance using `ToFacetQuery` extension.

## Syntax

{CODE facet_1@ClientApi\Session\Querying\HowToPerformFacetedSearch.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **facets** | List<[Facet](../../../glossary/facet)> | List of facets required to perform a facet query (mutually exclusive with `facetSetupDoc`). |
| **facetSetupDoc** | string | Document key that contains predefined [FacetSetup](../../../glossary/facet-setup) (mutually exclusive with `facets`). |
| **start** | int | number of results that should be skipped. Default: `0`. |
| **pageSize** | int | maximum number of results that will be retrieved. Default: `null`. | 

| Return Value | |
| ------------- | ----- |
| [FacetResults](../../../glossary/facet-results) | Facet query results with query `Duration` and list of `Results` - one entry for each term/range as specified in [FacetSetup] document or passed in parameters. |

## Example I

{CODE facet_2@ClientApi\Session\Querying\HowToPerformFacetedSearch.cs /}

## Example II

{CODE facet_3@ClientApi\Session\Querying\HowToPerformFacetedSearch.cs /}

## Converting Query into FacetQuery

{CODE facet_4@ClientApi\Session\Querying\HowToPerformFacetedSearch.cs /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **facets** | List<[Facet](../../../glossary/facet)> | List of facets required to perform a facet query (mutually exclusive with `facetSetupDoc`). |
| **facetSetupDoc** | string | Document key that contains predefined [FacetSetup](../../../glossary/facet-setup) (mutually exclusive with `facets`). |
| **start** | int | number of results that should be skipped. Default: `0`. |
| **pageSize** | int | maximum number of results that will be retrieved. Default: `null`. | 

| Return Value | |
| ------------- | ----- |
| [FacetQuery](../../../glossary/facet-query) | Instance of FacetQuery containing all options set in `Query`. Can be used with `MultiFacetedSearch` from `Advanced` session operations or with `Commands` directly. |

### Example

{CODE facet_5@ClientApi\Session\Querying\HowToPerformFacetedSearch.cs /}

## Related articles

- [Indexes : Querying : Faceted Search](../../../indexes/querying/faceted-search)   
