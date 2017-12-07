#Fanout Indexes

The fanout index is the index that outputs multiple index entries per each document. Here is the example of such one:

{CODE fanout_index_def_1@Indexes/FanoutIndexes.cs /}

Note that a large order, having a lot of line items, we will create an index entry per each `OrderLine` item from `Lines` collection. It means that a single document can generate hundreds of index entries.

The fanout index concept is not specific for map-only indexes. It also applies to map-reduce ones:

{CODE fanout_index_def_2@Indexes/FanoutIndexes.cs /}

The above index definitions are correct and in both cases this is actually what we want. However, you need to be aware that fanout indexes are typically more expensive than regular ones.
RavenDB has to index many more entries than usual what can result in higher utilization of CPU and memory and overall declining performance of the index.

{NOTE:Note}
Starting from version 4.0 the fanout indexes won't error when the number of index entries created from a single document exceeds the configured limit. The configuration options from 3.x:

- `Raven/MaxSimpleIndexOutputsPerDocument` 
- `Raven/MaxMapReduceIndexOutputsPerDocument` 

are no longer valid.

Instead, RavenDB will give you a performance hint regarding high fanout ratio using the Studio's notification center.
{NOTE/}


##Performance Hints

Once RavenDB notices that the number of indexing outputs created from a document is high the notification that will appear in the Studio:

![Figure 1. High indexing fanout ratio notification](images/fanout-index-performance-hint-1.png "High indexing fanout ratio notification")

The details will give you the following info:

![Figure 2. Fanout index, performance hint details](images/fanout-index-performance-hint-2.png "Fanout index, performance hint details")

You can control when a performance hint should be created using `PerformanceHints.Indexing.MaxIndexOutputsPerDocument` setting (default: 1024).

##Paging 

Since the fanout index creates multiple entries for a single document and queries return documents by default (it be changed if the query defines the projection) the paging of query results
is a bit more complex. Please read the dedicated article about [paging through tampered results](../indexes/querying/paging#paging-through-tampered-results).
