# Querying : How to perform a faceted search?

To execute facet query using session `Query` method use `ToFacets` extension. There is also a possibility to convert query straight into `FacetQuery` instance using `ToFacetQuery` extension.

## Syntax

{CODE facet_1@ClientApi\Session\Querying\HowToPerformFacetedSearch.cs /}

**Parameters**   

facets
:   Type: List<[Facet]()>   
List of facets required to perform a facet query (mutually exclusive with `facetSetupDoc`).

facetSetupDoc
:   Type: string   
Document key that contains predefined [FacetSetup]() (mutually exclusive with `facets`).

start
:   Type: int   
number of results that should be skipped. Default: `0`. 

pageSize
:   Type: int   
maximum number of results that will be retrieved. Default: `null`.

**Return Value**

Type: [FacetResults]()   
Facet query results with query `Duration` and list of `Results` - one entry for each term/range as specified in [FacetSetup] document or passed in parameters.

## Example I

{CODE facet_2@ClientApi\Session\Querying\HowToPerformFacetedSearch.cs /}

## Example II

{CODE facet_3@ClientApi\Session\Querying\HowToPerformFacetedSearch.cs /}

## Converting Query into FacetQuery

{CODE facet_4@ClientApi\Session\Querying\HowToPerformFacetedSearch.cs /}

**Parameters**   

facets
:   Type: List<[Facet]()>   
List of facets required to perform a facet query (mutually exclusive with `facetSetupDoc`).

facetSetupDoc
:   Type: string   
Document key that contains predefined [FacetSetup]() (mutually exclusive with `facets`).

start
:   Type: int   
number of results that should be skipped. Default: `0`. 

pageSize
:   Type: int   
maximum number of results that will be retrieved. Default: `null`.

**Return Value**

Type: [FacetQuery]()   
Instance of FacetQuery containing all options set in `Query`. Can be used with `MultiFacetedSearch` from `Advanced` session operations or with `Commands` directly.

### Example

{CODE facet_5@ClientApi\Session\Querying\HowToPerformFacetedSearch.cs /}

#### Related articles

TODO