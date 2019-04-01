# Session: Querying: How to perform a faceted search?

To execute facet query using session `query` method use `toFacets` method.

## Syntax

{CODE:java facet_1@ClientApi\Session\Querying\HowToPerformFacetedSearch.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **facets** | List<[Facet](../../../glossary/facet)> | List of facets required to perform a facet query (mutually exclusive with `facetSetupDoc`). |
| **facetSetupDoc** | String | Document key that contains predefined [FacetSetup](../../../glossary/facet-setup) (mutually exclusive with `facets`). |
| **start** | int | number of results that should be skipped. Default: `0`. |
| **pageSize** | int | maximum number of results that will be retrieved. Default: `null`. | 

| Return Value | |
| ------------- | ----- |
| [FacetResults](../../../glossary/facet-results) | Facet query results with query `duration` and list of `results` - one entry for each term/range as specified in [FacetSetup] document or passed in parameters. |

## Example I

{CODE:java facet_2@ClientApi\Session\Querying\HowToPerformFacetedSearch.java /}

## Example II

{CODE:java facet_3@ClientApi\Session\Querying\HowToPerformFacetedSearch.java /}


## Converting Query into FacetQuery

{CODE:java facet_4@ClientApi\Session\Querying\HowToPerformFacetedSearch.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **facets** | List<[Facet](../../../glossary/facet)> | List of facets required to perform a facet query (mutually exclusive with `facetSetupDoc`). |
| **facetSetupDoc** | String | Document key that contains predefined [FacetSetup](../../../glossary/facet-setup) (mutually exclusive with `facets`). |
| **start** | int | number of results that should be skipped. Default: `0`. |
| **pageSize** | int | maximum number of results that will be retrieved. Default: `null`. | 

| Return Value | |
| ------------- | ----- |
| [FacetQuery](../../../glossary/facet-query) | Instance of FacetQuery containing all options set in `query`. Can be used with `multiFacetedSearch` from `advanced` session operations or with `commands` directly. |

### Example

{CODE:java facet_5@ClientApi\Session\Querying\HowToPerformFacetedSearch.java /}


## Related articles

- [Indexes : Querying : Faceted Search](../../../indexes/querying/faceted-search)   
