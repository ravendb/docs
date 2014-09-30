# Map-Reduce indexes

This section of the documentation focuses on map reduce indexes. Map reduce indexes allow for complex aggregation of data by first selecting records (map) and then applying the specified reduce function to these records to produce a smaller set of results.

For a more in-depth look at how map reduce works, read this post: [Map-Reduce a Visual Explanation](http://ayende.com/blog/4435/map-reduce-a-visual-explanation).

## Creating

When it comes to index creation, the only difference between simple indexes and map-reduce ones is an additional reduce function defined in index definition. So to deploy an index we need to create a definition and deploy it using one of the ways described in [creating and deploying]() article. Please check any of the examples below for a sample map-reduce index.

## Applications

The applications of such indexes are many, but most common is a calculation.

### Example I - counting

Let's assume that we want to count number of products for each category. To do it, we need to specify following index:

{CODE map_reduce_0_0@Indexes\MapReduceIndexes.cs /}

and issue a query:

{CODE-TABS}
{CODE-TAB:csharp:Query map_reduce_0_1@Indexes\MapReduceIndexes.cs /}
{CODE-TAB:csharp:DocumentQuery map_reduce_0_2@Indexes\MapReduceIndexes.cs /}
{CODE-TABS/}

Above query will return one result for _Seafood_ with the appropriate number of products from that category.