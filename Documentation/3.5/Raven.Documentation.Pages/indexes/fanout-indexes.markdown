#Fanout indexes

A fanout index is an index that outputs multiple index entries per each document. Here is an example of such an index:

{CODE fanout_index_def_1@Indexes/FanoutIndexes.cs /}

Note that for a large `order`, with a lot of `order.Lines`, we will get an entry per each `OrderLine` item. It means that a single document can generate hundreds of index entries.

The fanout index concept isn't specific for map-only indexes. It also applies to map-reduce indexes:

{CODE fanout_index_def_2@Indexes/FanoutIndexes.cs /}

Note that these index definitions are correct and it both cases this is actually what we want. But there is a problem here. RavenDB has no way of knowing upfront how many index 
entries a document will generate, that means that it is very hard to allocate the appropriate amount of memory reserved for this, and it is possible to get into situations where
we simply run out of memory. 

That's why we introduced the concept of explicit control over the max number of index entries per document. In RavenDB 3.5 by default each document is allowed to output up to 15 entries
for a simple index and 50 entries for map-reduce index. If it tries to output more entries, the indexing of this document is aborted, and  it will be skipped by the indexer. An appropriate 
error message will appear in logs and [indexing errors](../studio/overview/status/index-errors).

These limits applies globally to all indexes and can be changed in the configuration:

* `Raven/MaxSimpleIndexOutputsPerDocument` - affects map-only indexes. Default: 15 (you can specify -1 in order to disable the output limit check),
* `Raven/MaxMapReduceIndexOutputsPerDocument`- limit for map-reduce indexes. Default: 50 (you can specify -1 in order to disable the output limit check).

You can also set this option for a specific index by setting `MaxIndexOutputsPerDocument` in the index definition.

{CODE fanout_index_def_with_max_index_outputs@Indexes/FanoutIndexes.cs /}

The limit configured in the index definition has the priority over the limit from the configuration settings.

{INFO: Upgrade to 3.5}
If you upgrade to RavenDB 3.5, old indexes will have a limit of 16,384 items, to avoid breaking of existing indexes.
{INFO/}