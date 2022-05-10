# Indexes: Map-Reduce Indexes
---

{NOTE: }

* **Map-Reduce indexes** allow you to perform complex ***data aggregation*** that can be queried on with very little cost, 
  regardless of the data size.  

* To expedite queries and prevent performance degradation during queries, the aggregation is done during the indexing phase, _not_ at query time.  

* Once new data comes into the database, or existing documents are modified,  
  the Map-Reduce index will re-calculate the aggregated data  
  so that the aggregation results are always available and up-to-date!  

* The aggregation computation is done in two separate consecutive actions: the `Map` and the `Reduce`.  
  * **The Map stage:**  
    This first stage runs the defined Map function(s) on each document, indexing the specified fields.  
  * **The Reduce stage:**  
    This second stage groups the specified requested fields that were indexed in the Map stage,  
    and then runs the Reduce function to get a final aggregation result per field value.  

For a more in-depth look at how map-reduce works, you can read this post: [RavenDB 4.0 Unsung Heroes: Map/reduce](https://ayende.com/blog/179938/ravendb-4-0-unsung-heroes-map-reduce).

* In this page: 
  * [Creating Map Reduce Indexes](../indexes/map-reduce-indexes#creating-map-reduce-indexes)
  * [Creating Multi-Map-Reduce Indexes](../indexes/map-reduce-indexes#creating-multi-map-reduce-indexes)
  * [Reduce Results as Artificial Documents](../indexes/map-reduce-indexes#reduce-results-as-artificial-documents)

{NOTE/}

{PANEL:Creating Map Reduce Indexes}

When it comes to index creation, the only difference between simple indexes and the map-reduce ones is an additional 
reduce function defined in the index definition. 
To deploy an index we need to create a definition and deploy it using one of the ways described in the 
[creating and deploying](../indexes/creating-and-deploying) article.

---

### Example I - Count

Let's assume that we want to count the number of products for each category. To do it, we can create the following index using `LoadDocument` inside:

{CODE-TABS}
{CODE-TAB:csharp:LINQ map_reduce_0_0@Indexes\MapReduceIndexes.cs /}
{CODE-TAB:csharp:JavaScript map_reduce_0_0@Indexes\JavaScript.cs /}}
{CODE-TABS/}

and issue the query:

{CODE-TABS}
{CODE-TAB:csharp:Query map_reduce_0_1@Indexes\MapReduceIndexes.cs /}
{CODE-TAB:csharp:DocumentQuery map_reduce_0_2@Indexes\MapReduceIndexes.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from 'Products/ByCategory'
where Category == 'Seafood'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

The above query will return one result for _Seafood_ with the appropriate number of products from that category.

---

### Example II - Average

In this example, we will count an average product price for each category. The index definition:

{CODE-TABS}
{CODE-TAB:csharp:LINQ map_reduce_1_0@Indexes\MapReduceIndexes.cs /}
{CODE-TAB:csharp:JavaScript map_reduce_1_0@Indexes\JavaScript.cs /}}
{CODE-TABS/}

and the query:

{CODE-TABS}
{CODE-TAB:csharp:Query map_reduce_1_1@Indexes\MapReduceIndexes.cs /}
{CODE-TAB:csharp:DocumentQuery map_reduce_1_2@Indexes\MapReduceIndexes.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from 'Products/Average/ByCategory'
where Category == 'Seafood'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

---

### Example III - Calculations

This example illustrates how we can put some calculations inside an index using on one of the indexes available in the sample database (`Product/Sales`).

We want to know how many times each product was ordered and how much we earned for it. In order to extract that information, we need to define the following index:

{CODE-TABS}
{CODE-TAB:csharp:LINQ map_reduce_2_0@Indexes\MapReduceIndexes.cs /}
{CODE-TAB:csharp:JavaScript map_reduce_2_0@Indexes\JavaScript.cs /}}
{CODE-TABS/}

and send the query:

{CODE-TABS}
{CODE-TAB:csharp:Query map_reduce_2_1@Indexes\MapReduceIndexes.cs /}
{CODE-TAB:csharp:DocumentQuery map_reduce_2_2@Indexes\MapReduceIndexes.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from 'Product/Sales'
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL:Creating Multi-Map-Reduce Indexes}

A **Multi-Map-Reduce** index allows aggregating (or 'reducing') data from several collections.  

They can be created and edited via [Studio](../studio/database/indexes/create-map-reduce-index#multi-map-reduce), or with API as shown below.  

See samples about [counting](../indexes/map-reduce-indexes#example-i---count), 
[calculating average](../indexes/map-reduce-indexes#example-ii---average), and a more advanced [calculation](../indexes/map-reduce-indexes#example-iii---calculations).  

In the following code sample, we want the number of companies, suppliers, and employees per city.  
We define the map phase on collections 'Employees', 'Companies', and 'Suppliers'.  
We then define the reduce phase.  

{CODE:csharp multi_map_reduce_LINQ@Indexes\MapReduceIndexes.cs /}

A query on the index:

{CODE:csharp multi-map-reduce-index-query@Indexes\MapReduceIndexes.cs /}

{NOTE: }
You can see this sample described in detail in [Inside RavenDB - Multi-Map-Reduce Indexes](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/11-mapreduce-and-aggregations-in-ravendb#multimapreduce-indexes).
{NOTE/}

{PANEL/}

{PANEL:Reduce Results as Artificial Documents}

In addition to storing the aggregation results in the index, the map-reduce indexes can also output reduce results as documents to a specified collection.
In order to create such documents, called _artificial_, you need to define the target collection using the `OutputReduceToCollection` property in the index definition.

{CODE-TABS}
{CODE-TAB:csharp:LINQ map_reduce_3_0@Indexes\MapReduceIndexes.cs /}
{CODE-TAB:csharp:JavaScript map_reduce_3_0@Indexes\JavaScript.cs /}}
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

---

### Artificial Document IDs

The identifiers of artificial documents are generated as:

- `<OutputCollectionName>/<hash-of-reduce-key>`

For the above sample index, the document ID can be:

- `MonthlyProductSales/13770576973199715021`

The numeric part is the hash of the reduce key values, in this case: `hash(Product, Month)`.

If the aggregation value for a given reduce key changes then we overwrite the artificial document. It will get removed once there is no result for a given reduce key.
    
---

### Artificial Document Flags

Documents generated by map-reduce indexes get the following `@flags` metadata:

{CODE-BLOCK:json}
"@flags": "Artificial, FromIndex"
{CODE-BLOCK/}

Those flags are used internally by the database to filter out artificial documents during replication.

{PANEL/}

## Related Articles

### Indexes

- [Map Indexes](../indexes/map-indexes)
- [Multi-Map Indexes](../indexes/multi-map-indexes)

### Querying

- [Basics](../indexes/querying/basics)

### Studio

- [Indexes: Overview](../studio/database/indexes/indexes-overview)
- [Index List View](../studio/database/indexes/indexes-list-view)
- [Create Map Index](../studio/database/indexes/create-map-index)
- [Create Multi-Map Index](../studio/database/indexes/create-multi-map-index)
- [Map-Reduce Visualizer](../studio/database/indexes/map-reduce-visualizer)

<br/>

## Code Walkthrough

- [Map Index](https://demo.ravendb.net/demos/csharp/static-indexes/map-index)
- [Map-Reduce Index](https://demo.ravendb.net/demos/csharp/static-indexes/map-reduce-index)
- [Multi-Map-Reduce-Index](https://demo.ravendb.net/demos/csharp/multi-map-indexes/multi-map-reduce-index#)

