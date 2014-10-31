# Session : Querying : How to perform a multifaceted search?

Multiple [Facet queries](../../../client-api/session/querying/how-to-perform-a-faceted-search) can be executed at once using `multiFacetedSearch` method from `advanced` session operations.

## Syntax

{CODE:java multi_facet_1@ClientApi\Session\Querying\HowToPerformMultiFacetedSearch.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **queries** | params [FacetQuery]() | Array of FacetQueries that will be executed on server. |

| Return Value | |
| ------------- | ----- |
| [FacetResult]()[] | Array of FacetResults. Each matching its FacetQuery position from `queries` parameter. |

## Example

{CODE:java multi_facet_2@ClientApi\Session\Querying\HowToPerformMultiFacetedSearch.java /}

## Related articles

TODO