#Dynamic aggregation

This feature is an another way to do aggregations and in contrast to map/reduce indexes, it allows to create much more complex queries. It gives you more options for reporting applications,
dynamic selection and complex aggregation with additional filtering. 

When working with a map/reduce index we are able to do only limited amount of queries. Let's imagine a sample SQL query:

{CODE-START:csharp /}
select sum(Total) from Orders where Total > 500 group by Product
{CODE-END /}

In order give you the ability to query like this, we introduced the dynamic aggregation feature. Thanks this you can build the following query:

{CODE dynamic_aggregation_1@ClientApi\Querying\DynamicAggregation.cs /}

The _Orders/All_ index used in the query is a simple map-only index. The only difference is that you have to specify sort options for numeric fields used in the query. This is needed
for recognizing by RavenDB types of numeric fields when such a query will come in.

{CODE dynamic_aggregation_index_def@ClientApi\Querying\DynamicAggregation.cs /}

{NOTE Results of a dynamic aggregation query are calculated on the fly, while results of map/reduce index are precomputed. Most cases is fast enough but note that you can hit a lot of data. This feature should be used only for complex aggregate queries that cannot be done by using standard map/reduce. /}

##Extended feceted search

Under the covers this is [a faceted search](../faceted-search) with an extended support for doing aggregations. For example you can aggregate ranges:

{CODE dynamic_aggregation_range@ClientApi\Querying\DynamicAggregation.cs /}

It also supports an aggregation on multiple levels:

{CODE dynamic_aggregation_multiple_items@ClientApi\Querying\DynamicAggregation.cs /}

Another example is an aggregation on different fields based on same facet:

{CODE dynamic_aggregation_different_fieldss@ClientApi\Querying\DynamicAggregation.cs /}