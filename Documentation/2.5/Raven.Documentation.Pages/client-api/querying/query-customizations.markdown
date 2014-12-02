#Query customizations

Raven's `Query` has a bunch of customizations which you can set by using `Customize()` method. Example:

{CODE customize_usage@ClientApi\Querying\QueryCustomizations.cs /}

Below there are presented available query customization options. All of them have equivalents for `LuceneQuery`. In example the Lucene version of the query above would be:

{CODE customize_usage_lucene@ClientApi\Querying\QueryCustomizations.cs /}

##NoCaching

It disables the caching for query results. It means that a response of a request send to get results of a specified query will not be keep in the cache.

{CODE no_cache@ClientApi\Querying\QueryCustomizations.cs /}

##NoTracking

It disables the tracking mechanism for queried entities by Raven's [Unit of Work](../basic-operations/understanding-session-object#unit-of-work). The usage of this option will prevent holding query results in memory.

{CODE no_tracking@ClientApi\Querying\QueryCustomizations.cs /}
