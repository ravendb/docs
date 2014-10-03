# Map-Reduce indexes

This section of the documentation focuses on map reduce indexes. Map reduce indexes allow for complex aggregation of data by first selecting records (map) and then applying the specified reduce function to these records to produce a smaller set of results.

For a more in-depth look at how map reduce works, read this post: [Map-Reduce a Visual Explanation](http://ayende.com/blog/4435/map-reduce-a-visual-explanation).

## Creating

When it comes to index creation, the only difference between simple indexes and map-reduce ones is an additional reduce function defined in index definition. So to deploy an index we need to create a definition and deploy it using one of the ways described in [creating and deploying](../indexes/creating-and-deploying) article. Please check any of the examples below for a sample map-reduce index.

## Applications

The applications of such indexes are many, but most common is aggregation of data from multiple documents.

### Example I - counting

Let's assume that we want to count number of products for each category. To do it, we need to specify following index:

{CODE map_reduce_0_0@Indexes\MapReduceIndexes.cs /}

and issue a query:

{CODE-TABS}
{CODE-TAB:csharp:Query map_reduce_0_1@Indexes\MapReduceIndexes.cs /}
{CODE-TAB:csharp:DocumentQuery map_reduce_0_2@Indexes\MapReduceIndexes.cs /}
{CODE-TABS/}

Above query will return one result for _Seafood_ with the appropriate number of products from that category.

### Example II - average

In this example we will count average product price for each category. First we need to create an index:

{CODE map_reduce_1_0@Indexes\MapReduceIndexes.cs /}

and issue a query:

{CODE-TABS}
{CODE-TAB:csharp:Query map_reduce_1_1@Indexes\MapReduceIndexes.cs /}
{CODE-TAB:csharp:DocumentQuery map_reduce_1_2@Indexes\MapReduceIndexes.cs /}
{CODE-TABS/}

### Example III - complex calculations

This example will illustrate how we can put some calculations inside index and is based on one of the indexes available in sample database (`Product/Sales`).

Let's assume that we want to know how many times product was ordered and how much we earned for it. To do this we need to define following index:

{CODE map_reduce_2_0@Indexes\MapReduceIndexes.cs /}

and issue a query:

{CODE-TABS}
{CODE-TAB:csharp:Query map_reduce_2_1@Indexes\MapReduceIndexes.cs /}
{CODE-TAB:csharp:DocumentQuery map_reduce_2_2@Indexes\MapReduceIndexes.cs /}
{CODE-TABS/}

## Related articles

- [Indexing related documents](../indexes/indexing-related-documents)
- [Map-Reduce indexes](../indexes/map-reduce-indexes)
- [Creating and deploying indexes](../indexes/creating-and-deploying)
- [Querying : Basics](../indexes/querying/basics)

