#Lazy Operations

Some of the operations in RavenDB can be evaluated lazily (performed only when needed).

This section will describe each of the available lazy operations:   
* **Querying**  
* **Loading**  
* **Faceted searching**   
* **Suggesting**    

## Querying

To perform query below

{CODE lazy_operations_querying_1@ClientApi\Querying\LazyOperations.cs /}

as a lazy operation we have introduced `Lazily()` extension method that will mark any type of queries as a lazy operation, so to perform above query as such an operation just mark it with this extension method like in example below.

{CODE lazy_operations_querying_2@ClientApi\Querying\LazyOperations.cs /}

To evaluate our `lazyUsers` just access `Value` property.

{CODE lazy_operations_querying_3@ClientApi\Querying\LazyOperations.cs /}

An action that will be executed when value gets evaluated can be passed to `Lazily`. It is very handy in scenarios when you want to perform additional actions when value gets evaluated or when you want to execute all pending lazy operations at once.

{CODE lazy_operations_querying_4@ClientApi\Querying\LazyOperations.cs /}

`Lazily` is a part of `LinqExtensions` available in `Raven.Client` namespace, so

{CODE using@ClientApi\Querying\LazyOperations.cs /}

is mandatory.

Lazy Lucene queries are also possible.

{CODE lazy_operations_querying_5@ClientApi\Querying\LazyOperations.cs /}

##Loading

Loading lazily is done in a bit different manner, it can be achieved by using one of the methods available in `session.Advanced.Lazily` property, so to perform below query

{CODE lazy_operations_loading_1@ClientApi\Querying\LazyOperations.cs /}

as a lazy operation just use one of the methods from `session.Advanced.Lazily` like in example below

{CODE lazy_operations_loading_2@ClientApi\Querying\LazyOperations.cs /}

Value can be evaluated in same way as when Querying and Action that will be performed when value gets evaluated can also be passed.

{CODE lazy_operations_loading_3@ClientApi\Querying\LazyOperations.cs /}

{CODE lazy_operations_loading_4@ClientApi\Querying\LazyOperations.cs /}

Other available lazy loading methods are:   
1\. **LoadStartingWith** where you can load all users with given key prefix.   

{CODE lazy_operations_loading_5@ClientApi\Querying\LazyOperations.cs /}

2\. **Includes** where additional documents will be loaded by given path.   

If we consider having `User` and `City` classes as defined below

{CODE lazy_operations_loading_6@ClientApi\Querying\LazyOperations.cs /}

and store one `User` and `City`

{CODE lazy_operations_loading_7@ClientApi\Querying\LazyOperations.cs /}

then we will be able to perform code such as

{CODE lazy_operations_loading_8@ClientApi\Querying\LazyOperations.cs /}

##Faceted search

To take advantage of lazy Faceted search use `ToFacetsLazy()` extension method from `LinqExtensions` found in `Raven.Client` namespace.

To change Faceted search from last step described [here](../faceted-search#step-3) to lazy operation just substitute `ToFacets` with `ToFacetsLazy`.

{CODE lazy_operations_facets_1@ClientApi\Querying\LazyOperations.cs /}

##Suggesting

Similar practice as in Faceted search has been used in lazy suggestions. The `SuggestLazy()` extension method is avaialbe in `LinqExtensions` and can be used as a substitution to `Suggest()` to mark operation as a lazy one.

{CODE lazy_operations_suggest_1@ClientApi\Querying\LazyOperations.cs /}