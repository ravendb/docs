# Querying : How to perform a multifaceted search?

Multiple [Facet queries] can be executed at once using `MultiFacetedSearch` method from `Advanced` session operations.

## Syntax

{CODE multi_facet_1@ClientApi\Session\Querying\HowToPerformMultiFacetedSearch.cs /}

**Parameters**   

queries
:   Type: params [FacetQuery]()   
Array of FacetQueries that will be executed on server.

**Return value**

Type: [FacetResult]()[]   
Array of FacetResults. Each matching its FacetQuery position from `queries` parameter.   

## Example

{CODE multi_facet_2@ClientApi\Session\Querying\HowToPerformMultiFacetedSearch.cs /}

#### Related articles

TODO