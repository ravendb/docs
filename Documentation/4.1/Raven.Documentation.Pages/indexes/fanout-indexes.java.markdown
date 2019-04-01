# Indexes: Fanout Indexes

The fanout index is the index that outputs multiple index entries per each document. Here is an example of such one:

{CODE-TABS}
{CODE-TAB:java:LINQ fanout_index_def_1@Indexes\FanoutIndexes.java /}
{CODE-TAB:java:JavaScript fanout_index_def_1@Indexes\JavaScript.java /}}
{CODE-TABS/}

A large order, having a lot of line items, will create an index entry per each `OrderLine` item from the `Lines` collection. A single document can generate hundreds of index entries.

The fanout index concept is not specific for map-only indexes. It also applies to map-reduce indexes:

{CODE-TABS}
{CODE-TAB:java:LINQ fanout_index_def_2@Indexes\FanoutIndexes.java /}
{CODE-TAB:java:JavaScript map_reduce_2_0@Indexes\JavaScript.java /}}
{CODE-TABS/}

The above index definitions are correct. In both cases this is actually what we want. However, you need to be aware that fanout indexes are typically more expensive than regular ones.
RavenDB has to index many more entries than usual. What can result is higher utilization of CPU and memory, and overall declining performance of the index.

{NOTE:Note}
Starting from version 4.0, the fanout indexes won't error when the number of index entries created from a single document exceeds the configured limit. The configuration options from 3.x:

- `Raven/MaxSimpleIndexOutputsPerDocument` 
- `Raven/MaxMapReduceIndexOutputsPerDocument` 

are no longer valid.

RavenDB will give you a performance hint regarding high fanout ratio using the Studio's notification center.
{NOTE/}


##Performance Hints

Once RavenDB notices that the number of indexing outputs created from a document is high, the notification that will appear in the Studio:

![Figure 1. High indexing fanout ratio notification](images/fanout-index-performance-hint-1.png "High indexing fanout ratio notification")

The details will give you the following info:

![Figure 2. Fanout index, performance hint details](images/fanout-index-performance-hint-2.png "Fanout index, performance hint details")

You can control when a performance hint should be created using the `PerformanceHints.Indexing.MaxIndexOutputsPerDocument` setting (default: 1024).

##Paging 

Since the fanout index creates multiple entries for a single document and queries return documents by default (it can change if the query defines the projection) the paging of query results
is a bit more complex. Please read the dedicated article about [paging through tampered results](../indexes/querying/paging#paging-through-tampered-results).

## Related articles

### Indexes

- [What are Indexes](../indexes/what-are-indexes)
