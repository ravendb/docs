# Querying : How to perform queries lazily?

In some situations query execution must be delayed. To cover such scenario `Lazily` and many others query extensions has been introduced.

## Lazily and LazilyAsync

{CODE lazy_1@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

**Parameters**   

onEval
:   Type: Action<IEnumerable&lt;TResult&gt;>   
Action that will be performed on query results.

**Return Value**

Type: Lazy<IEnumerable&lt;TResult&gt;>  
Lazy query initializer returning query results.

### Example

{CODE lazy_2@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

or

{CODE lazy_3@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

## CountLazily

{CODE lazy_4@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

**Return Value**

Type: Lazy<IEnumerable&lt;TResult&gt;>  
Lazy query initializer returning count of matched documents.

### Example

{CODE lazy_5@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

## SuggestLazy

{CODE lazy_6@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

**Parameters**   

query
:   Type: [SuggestionQuery](../../../glossary/client-api/querying/suggestion-query)     
A suggestion query definition containing all information required to query a specified index.

**Return Value**

Type: Lazy<[SuggestionQueryResult]()>  
Lazy query initializer containing array of all suggestions for executed query.

### Example

{CODE lazy_7@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

## ToFacetsLazy

{CODE lazy_8@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

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

Type: Lazy<[FacetResults]()>   
Lazy query initializer containing Facet query results with query `Duration` and list of `Results` - one entry for each term/range as specified in [FacetSetup] document or passed in parameters.

### Example

{CODE lazy_9@ClientApi\Session\Querying\HowToPerformQueriesLazily.cs /}

#### Related articles

TODO