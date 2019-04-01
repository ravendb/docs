# Commands: Querying: How to work with Facet query?

There are two methods that allow you to send a Facet query to a database:   
- [GetFacets](../../../client-api/commands/querying/how-to-work-with-facet-query#getfacets)    
- [GetMultiFacets](../../../client-api/commands/querying/how-to-work-with-facet-query#getmultifacets)   

{PANEL:GetFacets}

There are few overloads for the **GetFacets** method and the main difference between them is a source of the facets. In one facets are passed as a parameter, in the other user must provide a `key` to a `facet setup` document.

### Syntax

{CODE:java get_facets_1@ClientApi\Commands\Querying\HowToWorkWithFacetQuery.java /}

{CODE:java get_facets_2@ClientApi\Commands\Querying\HowToWorkWithFacetQuery.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **index** | String | A name of an index to query |
| **query** | [IndexQuery](../../../glossary/index-query) | A query definition containing all information required to query a specified index. |
| **facets** | List<[Facet](../../../glossary/facet)> | List of facets required to perform a facet query (mutually exclusive with `facetSetupDoc`). |
| **facetSetupDoc** | String | Document key that contains predefined [FacetSetup](../../../glossary/facet-setup) (mutually exclusive with `facets`). |
| **start** | int | number of results that should be skipped. Default: `0`. |
| **pageSize** | int | maximum number of results that will be retrieved. Default: `null`. |

| Return Value | |
| ------------- | ----- |
| [FacetResults](../../../glossary/facet-results) | Facet query results containing query `Duration` and a list of `Results` - one entry for each term/range as specified in [FacetSetup] document or passed in parameters. |

### Example I

{CODE:java get_facets_3@ClientApi\Commands\Querying\HowToWorkWithFacetQuery.java /}

### Example II

{CODE:java get_facets_4@ClientApi\Commands\Querying\HowToWorkWithFacetQuery.java /}

{PANEL/}

{PANEL:GetMultiFacets}

Sending multiple facet queries is achievable by using `GetMultiFacets` method.

### Syntax

{CODE:java get_facets_5@ClientApi\Commands\Querying\HowToWorkWithFacetQuery.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **facetedQueries** | [FacetQuery](../../../glossary/facet-query)[] | List of the faceted queries that will be executed on the server-side. |

| Return Value | |
| ------------- | ----- |
| [FacetResult](../../../glossary/facet-results#facetresult)[] | List of the results, each matching position of a FacetQuery in  the `facetedQueries` parameter. |

### Example

{CODE:java get_facets_6@ClientApi\Commands\Querying\HowToWorkWithFacetQuery.java /}

{PANEL/}

## Related articles

- [Full RavenDB query syntax](../../../indexes/querying/full-query-syntax)   
- [How to **query** a **database**?](../../../client-api/commands/querying/how-to-query-a-database)   
- [How to **stream query** results?](../../../client-api/commands/querying/how-to-stream-query-results)   
