# Commands : Querying : How to work with Facet query?

There are two methods that allow you to send a Facet query to a database:   
- [GetFacets](../../../client-api/commands/querying/how-to-work-with-facet-query#getfacets)    
- [GetMultiFacets](../../../client-api/commands/querying/how-to-work-with-facet-query#getmultifacets)   

## GetFacets

There are two overloads for a **GetFacets** method and the only difference between them is a source of facets. In one facets are passed as a parameter, in the other user must provide a `key` to a `facet setup` document.

### Syntax

{CODE get_facets_1@ClientApi\Commands\Querying\HowToWorkWithFacetQuery.cs /}

{CODE get_facets_2@ClientApi\Commands\Querying\HowToWorkWithFacetQuery.cs /}

**Parameters**

index
:   Type: string   
A name of an index to query

query
:   Type: [IndexQuery]()   
A query definition containing all information required to query a specified index.

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
Facet query results containing query `Duration` and list of `Results` - one entry for each term/range as specified in [FacetSetup] document or passed in parameters.

### Example I

{CODE get_facets_3@ClientApi\Commands\Querying\HowToWorkWithFacetQuery.cs /}

### Example II

{CODE get_facets_4@ClientApi\Commands\Querying\HowToWorkWithFacetQuery.cs /}

## GetMultiFacets

Sending multiple facet queries is achievable by using `GetMultiFacets` method.

### Syntax

{CODE get_facets_5@ClientApi\Commands\Querying\HowToWorkWithFacetQuery.cs /}

**Parameters**

facetedQueries
:   Type: [FacetQuery]()[]   
List of faceted queries that will be executed on server-side.

**Return Value**

Type: [FacetResult]()[]   
List of results, each maching position of a FacetQuery in `facetedQueries` parameter.

### Example

{CODE get_facets_6@ClientApi\Commands\Querying\HowToWorkWithFacetQuery.cs /}

#### Related articles

- [Full RavenDB query syntax](../../../Indexes/full-query-syntax)   
- [How to **query** a **database**?](../../../client-api/commands/querying/how-to-query-a-database)   
- [How to **stream query** results?](../../../client-api/commands/querying/how-to-stream-query-results)   