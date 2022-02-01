# Indexing Performance

* To help analyze consumption of system resources and optimize indexes, this Studio view tracks all of your indexes.  
  It shows the activities, resource usage, and performance of each index over time.  
* You can see what happens during each indexing batch, including errors if there are any.  
* Each stage of an indexing process is represented graphically by a colorful stripe to show the length of time elapsed.  
* Hovering over various indexing stages shows detailed statistics about the resource usage and performance of each stage.  
* You can export an interactive snapshot of the indexing statistics to a colleague or to RavenDB technical support.  


In this page:  

* [Indexing Performance View](../../../studio/database/indexes/indexing-performance#indexing-performance-view)
* [Types of Index Tracks](../../../studio/database/indexes/indexing-performance#types-of-index-tracks)
* [Index Statistics Overview](../../../studio/database/indexes/indexing-performance#index-statistics-overview)
* [Common Indexing Issues](../../../studio/database/indexes/indexing-performance#common-indexing-issues)
  - [Resource Overload](../../../studio/database/indexes/indexing-performance#resource-overload)
  - [Low Memory](../../../studio/database/indexes/indexing-performance#low-memory)
  - [Concurrent Processing of Too Many Indexes](../../../studio/database/indexes/indexing-performance#concurrent-processing-of-too-many-indexes)
  - [LoadDocument Misuse](../../../studio/database/indexes/indexing-performance#loaddocument-misuse)


  
---

{PANEL: Indexing Performance View }

![Indexing Performance View](images/indexing-performance-overview.png "Indexing Performance View")

1. **Indexes tab**  
   Click to reveal the Studio indexing options.  
2. **Indexing Performance**  
   Select to see the Indexing Performance view.  
3. **Current Database**  
   Make sure that this is the database that you want to analyze.  
4. **Filter**  
   Write keywords from the index names (such as which collection or specifications) to see only indexes with these keywords in the definition.  
5. **Expand All**  
   Click to see the details of all index tracks at once.  
6. **Export**  
   Click to download an interactive snapshot of the indexing statistics so that you can send it to RavenDB technical support.  
7. **Import**  
   Click to upload and analyze an interactive snapshot that was sent to you.  
8. **General Timeline**  
   * Shows when the indexes were active.  
   * Verticle purple lines show time-lapses until a batch process started.  
   * The red area represents the area that is zoomed in below. Enlarge or shrink it to zoom in and out below.  
9. **Auto/Orders/ByOrderedAt**  
   * This is one of the indexes that is tracked in time.  
     Varying index types have different processes and they may index different volumes of data, so the graphs are likely to differ from one another.
   * Clicking the index title expands the track to show index batch process details.  
   * Each colored stripe shows the duration of a stage in the batch process.  
     * Hovering over each stripe shows detailed statistics about each stage.  
10. **Purple line**  
    Each verticle purple line shows that there was a time-lapse in the timeline before a batch process started.  

{PANEL/}

---

{PANEL: Types of Index Tracks}

There are many different types of indexes and RavenDB's way of querying and indexing is refreshingly different from more traditional databases, providing far greater agility.  
To learn about RavenDB's novel approach to indexing and querying, there are four chapters on [querying and indexing](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/9-querying-in-ravendb) in the book "Inside RavenDB".
If you are already familiar and want to skip to the chapter on how to work with RavenDB indexes in a client application, see the chapter [Working with Indexes](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/12-working-with-indexes).

![Index Types](images/index-types.png "Index Types")

 1. [**Auto (Dynamic) Indexes**](../../../indexes/creating-and-deploying#auto-indexes)  
    When RavenDB processes a query, it scans for an existing index that will properly answer the query.  
     * If none exists, it automatically creates an [index based on the query](https://ravendb.net/articles/speeding-up-your-application-with-an-automatic-index).  
     * If there is an auto-index that partially answers the query, RavenDB's Query Optimizer may improve that index with the new query requirement, 
       then removes any newly-obsolete indexes.  
     * The query optimizer analyzes the set of queries you make to your database and generates the optimal set of indexes to answer those queries. 
       Changes in your queries also trigger changes in the indexes on your database as they adjust to the new requirements.  
       Dynamic indexing automatically adapts to changes and optimizations in your application, thus increasing agility.  
     * [Auto (Dynamic) indexes](../../../indexes/creating-and-deploying#auto-indexes) are identified by the prefix `Auto/` in the index names.  
 2. [**Static Indexes (Defined Explicitly by Developers)**](../../../indexes/creating-and-deploying#static-indexes)  
    [Static Indexes](../../../indexes/creating-and-deploying) can be created and edited explicitly by database admins and operators. 
     * Although they do not change dynamically whenever your clients' query needs change, you can program more complex functions into them, such as calculations and much more.  
     * These indexes can be 
       - [map](../../../indexes/map-indexes)  
       - [multi-map](../../../indexes/multi-map-indexes)  
       - [map-reduce](../../../indexes/map-reduce-indexes)  
       - [fanout](../../../indexes/fanout-indexes)  
     * These indexes do not have a tag word prefix, but instead start with the name that was given to them.  
 3. **ReplacementOf**  
    * When an existing index definition is changed, RavenDB uses the old version until the new version is completely built, then saves the old definition in the [index history](../../../studio/database/indexes/index-history).  
      Until the new version completely replaces the old, the two are in a state called [side-by-side indexing](../../../studio/database/indexes/indexes-list-view#index-list-view---side-by-side-indexing).

{INFO: Editing an Index}
Click the following for instructions about using the Studio to define  
- [Map Index](../../../studio/database/indexes/create-map-index)  
- [Multi-Map Index](../../../studio/database/indexes/create-multi-map-index)  
- [Map-Reduce Index](../../../studio/database/indexes/create-map-reduce-index)  
{INFO/}

{PANEL/}

---

{PANEL: Index Statistics Overview}

* When expanding index tracks, you can zoom in on batch processes to visually see the amount of time it took to do each stage.  
* Some stages are much shorter than others.  Zooming in with the mouse scroller reveals stages that happen more quickly than others.  
* When an index track is expanded, we typically see four rows of colored stripes. 
  * The top row is the entire indexing process.
  * The following rows are breakdowns of what happened in each stage in the rows above them.  

* Hover over these colored stripes to see detailed statistics about each stage.  

#### Indexing Stage

The indexing stripe shows the overall indexing process.  

![Indexing Statistics](images/indexing-stats.png "Indexing Statistics")

 1. **Mouse Cursor**  
    Notice that the mouse cursor is hovering over the "Indexing" stripe.  
 2. **Duration**  
    The amount of time this batch process took to complete.  
      * This is an example of RavenDB building a new auto-index, thus it took ~ 295 ms to process 830 documents.  
        The second time this index was run, it took ~1ms because the index was already built.  
        To get a more accurate rate, we would need a larger sample size of documents.  
 3. **Input Count**  
    The number of index entries that were scanned.  
 4. **Output Count**  
    The number of index entries that the index returned from the data store.  
 5. **Failed Count**  
    The number of documents that the index failed to process.  
 6. **Success Count**  
    The number of documents that the index succeeded to process.  
 7. **Documents Size**  
    Total size of the documents returned from the data store.  
 8. **Average Document Size**  
    The average size of each document that was returned.  
 9.  **Managed Allocation Size**  
 10. **Processed Data Speed**  
     The speed at which the data was processed.  
 11. **Document Processing Speed**  
     The number of documents per second.  
       * As the auto-index was built, it processed at a speed of ~2,814 documents per second.  
         The second time this index was run, it processed ~830,000 documents per second.  
         Again, to get a more accurate rate, we would need a larger sample size of documents.  

---

#### Map Stage

The statistics in the map stage detail memory usage and show the batch status.

![Map Statistics](images/map-stats.png "Map Statistics")

 1. **Mouse Cursor**  
    Notice that the mouse cursor is hovering over the "Map" stripe.  
 2. **Duration**  
    The amount of time this batch process took to complete.  
 3. **Batch status**  
    There are a number of possible batch status messages.  They fall into two main categories.  
     - **No more documents to index**  
       The batch managed to cover all of the documents needed.  
     - **(Name of the method used to create a batch stop)**  
       There are a number of configurations that break up large batch processes into smaller batches to prevent overwhelming your system.  
       While these batch stops prevent system overloads, they also point to potential opportunities to optimize your indexes.  
       These situations are discussed in the section on [common indexing issues](../../../studio/database/indexes/indexing-performance#common-indexing-issues)

---

#### Storage Stages

Storage stages show statistics related to reading and writing to disk.

![Storage/Commit Statistics](images/storage-commit-stats.png "Storage/Commit Statistics")

  1. **Duration**  
     The amount of time it took to store the data to disk.  
     If this stage takes a long time after the index is already built, it may reveal a hardware problem.

---

#### Lucene Stages

 Lucene stages show how long it took to store the information in the [Lucene](https://lucene.apache.org/core/) search engine.

![Lucene Stages](images/lucene.stages.png "Lucene Stages")


   

{PANEL/}

---

{PANEL: Common Indexing Issues}

Indexing can be a taxing operation on CPU resources.  

There are a number of configurations that efficiently use batch stops to break up huge batch processes into smaller batches to prevent exhausting resources. 

While they prevent system overloads, batch stops also point to potential **opportunities to optimize** your indexes.  
 
Batch stops break up processes into smaller batches when  

  - A batch is processing a [dataset that is too large](../../../studio/database/indexes/indexing-performance#resource-overload) for your system resources.  
  - The index definition requires a complex Linq process that can overwhelm your resources.  
  - [Low memory](../../../studio/database/indexes/indexing-performance#low-memory) resources for indexing process on local machines or burstable cloud instances.  
  - [Too many concurrent index processes](../../../studio/database/indexes/indexing-performance#concurrent-processing-of-too-many-indexes) are overwhelming your system.  
  - The [LoadDocument](../../../studio/database/indexes/indexing-performance#loaddocument-misuse) method is misused.  

---

#### Resource Overload

Some indexes are responsible for a huge dataset and/or have very complex, demanding definitions.  
To prevent resource overload, RavenDB can break up large batches into smaller ones.  
You can configure [batch stops](../../../studio/database/indexes/indexing-performance#common-indexing-issues) with the following methods:

- [Indexing.MapTimeoutInSec](../../../server/configuration/indexing-configuration#indexing.maptimeoutinsec)  
    Number of seconds after which mapping will end even if there is more to map.  
- [Indexing.MapTimeoutAfterEtagReachedInMin](../../../server/configuration/indexing-configuration#indexing.maptimeoutafteretagreachedinmin)  
    This will only be applied if we pass the last [etag](../../../glossary/etag) in collection that we saw when batch was started.  
- [Indexing.MapBatchSize](../../../server/configuration/indexing-configuration#indexing.mapbatchsize)  
    Maximum number of documents to be processed by the index per indexing batch.  

---

#### Low Memory 

- [Low Memory](../../../server/configuration/memory-configuration#memory.lowmemorylimitinmb) resources can slow down your system and result in [batch stops](../../../studio/database/indexes/indexing-performance#common-indexing-issues).  
  This can happen on a local machine or [basic](../../../cloud/cloud-instances#basic-grade-production-cluster) cloud instances.  
    * Unless you're running a [production cloud tier](../../../) with CPU priority of [standard](../../../cloud/cloud-instances#standard-grade-production-cluster) 
    or [Performance](../../../cloud/cloud-instances#performance-grade-production-cluster), your cluster is [burstable and subject to throttling](../../../cloud/cloud-overview#budget-credits-and-throttling).  
    This means that you have CPU credits with limits.  If you have reached your credit limit, RavenDB will wait until you've 
    accumulated enough credits to run another batch process.  
    This causes indexing to seem slow.  
    * On a local machine, you can upgrade your hardware, divide the work onto more machines in a cluster, and/or optimize your indexes. 
    * On a cloud instance, you can upgrade your instance and/or optimize your indexes.  

---

#### Concurrent Processing of Too Many Indexes

- [Limit concurrent index processes](../../../server/configuration/indexing-configuration#indexing.maxnumberofconcurrentlyrunningindexes) - 
  RavenDB can handle multiple index processes at the same time, but if there are too many, it will overwhelm the system and cause a 
  noticeable slow-down.  The `Indexing.MaxNumberOfConcurrentlyRunningIndexes` method enables you to have many indexes without exhausting resources by allowing you to set
  the number of concurrent index processes.  

---

#### LoadDocument Misuse

- [Indexing referenced/related data](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/10-static-indexes-and-other-advanced-options#indexing-referenced-data)
  can be useful (even in a NoSql database) when developers need to pull information from different documents into an indexing process.  The `LoadDocument` method
  creates a relationship between the two documents and ensures that whenever the referenced document is updated, the referencing documents will be re-indexed to 
  stay current with the new details.  
   - `LoadDocument` is a great feature, but problems arise if a large number of documents reference a single document (or a small set of them) that are frequently changed. 
     If frequent changes are made to these few documents, all the documents referencing it will also need to be reindexed. 
     In other words, the amount of work that an index has to do because 
     of a single document change can be extremely large and may cause delays in indexing.  
      - The high IO demands in this situation can then cause further problems such as longer request duration and cluster instability.
      - Sometimes, `LoadDocument` misuse is caused by trying to apply relational modeling approaches to document-based databases.  
        If you're accustomed to relational data modeling, you can learn about effective [document modeling](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/3-document-modeling) in the "Inside RavenDB" book.  





{PANEL/}

## Related Articles

### Indexes
- [Map Indexes](../../../indexes/map-indexes)
- [Multi-Map Indexes](../../../indexes/multi-map-indexes)
- [Map-Reduce Indexes](../../../indexes/map-reduce-indexes)

### Studio
- [Indexes: Overview](../../../studio/database/indexes/indexes-overview)
- [Index List View](../../../studio/database/indexes/indexes-list-view)
- [Create Map Index](../../../studio/database/indexes/create-map-index)
- [Create Multi-Map Index](../../../studio/database/indexes/create-multi-map-index)
- [Map-Reduce Visualizer](../../../studio/database/indexes/map-reduce-visualizer)
