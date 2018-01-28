# Map-Reduce Indexes

Map-Reduce indexes allow you to perform complex aggregations of data. The first stage, called the map, runs over documents and extracts portions of data according to the defined mapping function(s).
Upon completion of the first phase, reduction is applied to the map results and the final outcome is produced.

The idea behind map-reduce indexing is that aggregation queries using such indexes are very cheap. The aggregation is performed only once and the results are stored inside the index.
Once new data come into the database or existing documents are modified, the map-reduce index will keep the aggregation results up-to-date. The aggregations are never done during
querying to avoid expensive calculations that could result in severe performance degradation. When you make the query, RavenDB immediately returns the matching results directly from the index.

For a more in-depth look at how map reduce works, you can read this post: [RavenDB 4.0 Unsung Heroes: Map/reduce](https://ayende.com/blog/179938/ravendb-4-0-unsung-heroes-map-reduce).

## Creating

When it comes to index creation, the only difference between simple indexes and the map-reduce ones is an additional reduce function defined in index definition. 
To deploy an index we need to create a definition and deploy it using one of the ways described in the [creating and deploying](../indexes/creating-and-deploying) article.

### Example I - Count

Let's assume that we want to count the number of products for each category. To do it, we can create the following index using `LoadDocument` inside:

{CODE map_reduce_0_0@Indexes\MapReduceIndexes.cs /}

and issue the query:

{CODE-TABS}
{CODE-TAB:csharp:Query map_reduce_0_1@Indexes\MapReduceIndexes.cs /}
{CODE-TAB:csharp:DocumentQuery map_reduce_0_2@Indexes\MapReduceIndexes.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from 'Products/ByCategory'
where Category == 'Seafood'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

The above query will return one result for _Seafood_, with the appropriate number of products from that category.

### Example II - Average

In this example, we will count average product price for each category. The index definition:

{CODE map_reduce_1_0@Indexes\MapReduceIndexes.cs /}

and the query:

{CODE-TABS}
{CODE-TAB:csharp:Query map_reduce_1_1@Indexes\MapReduceIndexes.cs /}
{CODE-TAB:csharp:DocumentQuery map_reduce_1_2@Indexes\MapReduceIndexes.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from 'Products/Average/ByCategory'
where Category == 'Seafood'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example III - Calculations

This example illustrates how we can put some calculations inside an index using on one of the indexes available in sample database (`Product/Sales`).

We want to know how many times each product was ordered and how much we earned for it. In order to extract that information, we need to define the following index:

{CODE map_reduce_2_0@Indexes\MapReduceIndexes.cs /}

and send the query:

{CODE-TABS}
{CODE-TAB:csharp:Query map_reduce_2_1@Indexes\MapReduceIndexes.cs /}
{CODE-TAB:csharp:DocumentQuery map_reduce_2_2@Indexes\MapReduceIndexes.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from 'Product/Sales'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

## Related Articles

- [Indexing related documents](../indexes/indexing-related-documents)
- [Creating and deploying indexes](../indexes/creating-and-deploying)
- [Querying : Basics](../indexes/querying/basics)

