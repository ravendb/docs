# Indexes: Map-Reduce Indexes

Map-Reduce indexes allow you to perform complex aggregations of data. The first stage, called the map, runs over documents and extracts portions of data according to the defined mapping function(s).
Upon completion of the first phase, reduction is applied to the map results and the final outcome is produced.

The idea behind map-reduce indexing is that aggregation queries using such indexes are very cheap. The aggregation is performed only once and the results are stored inside the index.
Once new data comes into the database or existing documents are modified, the map-reduce index will keep the aggregation results up-to-date. The aggregations are never done during
querying to avoid expensive calculations that could result in severe performance degradation. When you make the query, RavenDB immediately returns the matching results directly from the index.

For a more in-depth look at how map reduce works, you can read this post: [RavenDB 4.0 Unsung Heroes: Map/reduce](https://ayende.com/blog/179938/ravendb-4-0-unsung-heroes-map-reduce).

{PANEL:Creating}

When it comes to index creation, the only difference between simple indexes and the map-reduce ones is an additional reduce function defined in index definition. 
To deploy an index we need to create a definition and deploy it using one of the ways described in the [creating and deploying](../indexes/creating-and-deploying) article.

### Example I - Count

Let's assume that we want to count the number of products for each category. To do it, we can create the following index using `LoadDocument` inside:

{CODE-TABS}
{CODE-TAB:java:LINQ map_reduce_0_0@Indexes\MapReduceIndexes.java /}
{CODE-TAB:java:JavaScript map_reduce_0_0@Indexes\JavaScript.java /}
{CODE-TABS/}

and issue the query:

{CODE-TABS}
{CODE-TAB:java:Query map_reduce_0_1@Indexes\MapReduceIndexes.java /}
{CODE-TAB-BLOCK:sql:RQL}
from 'Products/ByCategory'
where Category == 'Seafood'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

The above query will return one result for _Seafood_ with the appropriate number of products from that category.

### Example II - Average

In this example, we will count an average product price for each category. The index definition:

{CODE-TABS}
{CODE-TAB:java:LINQ map_reduce_1_0@Indexes\MapReduceIndexes.java /}
{CODE-TAB:java:JavaScript map_reduce_1_0@Indexes\JavaScript.java /}
{CODE-TABS/}

and the query:

{CODE-TABS}
{CODE-TAB:java:Query map_reduce_1_1@Indexes\MapReduceIndexes.java /}
{CODE-TAB-BLOCK:sql:RQL}
from 'Products/Average/ByCategory'
where Category == 'Seafood'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

### Example III - Calculations

This example illustrates how we can put some calculations inside an index using on one of the indexes available in the sample database (`Product/Sales`).

We want to know how many times each product was ordered and how much we earned for it. In order to extract that information, we need to define the following index:

{CODE-TABS}
{CODE-TAB:java:LINQ map_reduce_2_0@Indexes\MapReduceIndexes.java /}
{CODE-TAB:java:JavaScript map_reduce_2_0@Indexes\JavaScript.java /}}
{CODE-TABS/}

and send the query:

{CODE-TABS}
{CODE-TAB:java:Query map_reduce_2_1@Indexes\MapReduceIndexes.java /}
{CODE-TAB-BLOCK:sql:RQL}
from 'Product/Sales'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL:Reduce Results as Artificial Documents}

In addition to storing the aggregation results in the index, the map-reduce indexes can also output reduce results as documents to a specified collection.
In order to create such documents, called _artificial_, you need to define the target collection using the `OutputReduceToCollection` property in the index definition.

{CODE-TABS}
{CODE-TAB:java:LINQ map_reduce_3_0@Indexes\MapReduceIndexes.java /}
{CODE-TAB:java:JavaScript map_reduce_3_0@Indexes\JavaScript.java /}
{CODE-TABS/}

Writing map-reduce outputs into documents allows you to define additional indexes on top of them that give you the option to create recursive map-reduce operations.
This way, you can do daily/monthly/yearly summaries very cheaply and easy. 

In addition, you can also apply the usual operations on documents (e.g. data subscriptions or ETL).

{INFO: Saving documents}

Artificial documents are stored immediately after the indexing transaction completes.

{INFO/}

{WARNING:Recursive indexing loop}

It's forbidden to output reduce results to the collection that:

- the current index is already working on (e.g. index on `DailyInvoices` collections outputs to `DailyInvoices`),
- the current index is loading a document from it (e.g. index has `LoadDocument(id, "Invoices")` outputs to `Invoices`), 
- it is processed by another map-reduce index that outputs results to a collection that the current index is working on (e.g. one index on `Invoices` collection outputs to `DailyInvoices`, another index on `DailyInvoices` outputs to `Invoices`)

Since that would result in the infinite indexing loop (the index puts an artificial document what triggers the indexing and so on), you will get the detailed error on attempt to create such invalid construction.

{WARNING/}

{WARNING:Existing collection}

Creating a map-reduce index which defines the output collection that already exists and it contains documents will result in an error. You need to delete all documents
from the relevant collection before creating the index or output the results to a different one.

{WARNING/}

### Artificial Document IDs

The identifiers of artificial documents are generated as:

- `<OutputCollectionName>/<hash-of-reduce-key>`

For the above sample index, the document ID can be:

- `MonthlyProductSales/13770576973199715021`

The numeric part is the hash of the reduce key values, in this case: `hash(Product, Month)`.

If the aggregation value for a given reduce key changes then we overwrite the artificial document. It will get removed once there is no result for a given reduce key.
    
### Artificial Document Flags

Documents generated by map-reduce indexes get the following `@flags` metadata:

{CODE-BLOCK:json}
"@flags": "Artificial, FromIndex"
{CODE-BLOCK/}

Those flags are used internally by the database to filter out artificial documents during replication.

{PANEL/}

## Related Articles

### Indexes

- [Indexing Related Documents](../indexes/indexing-related-documents)
- [Creating and Deploying Indexes](../indexes/creating-and-deploying)

### Querying

- [Basics](../indexes/querying/basics)

### Studio

- [Create Map-Reduce Index](../studio/database/indexes/create-map-reduce-index)

