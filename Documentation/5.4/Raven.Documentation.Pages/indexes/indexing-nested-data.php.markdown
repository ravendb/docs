# Indexing Nested data

---

{NOTE: }

* JSON documents can have nested structures, where one document contains other objects or arrays of objects.

* Use a static-index to facilitate querying for documents based on the nested data.

* In this page:
 
  * [Sample data](../indexes/indexing-nested-data#sample-data)

  * [Simple index - SINGLE index-entry per document](../indexes/indexing-nested-data#simple-index---single-index-entry-per-document)    
      * [The index](../indexes/indexing-nested-data#theIndex)
      * [The index-entries](../indexes/indexing-nested-data#theIndexEntries)
      * [Querying the index](../indexes/indexing-nested-data#queryingTheIndex)
      * [When to use](../indexes/indexing-nested-data#whenToUse)

  * [Fanout index - MULTIPLE index-entries per document](../indexes/indexing-nested-data#fanout-index---multiple-index-entries-per-document)   
      * [What is a fanout index](../indexes/indexing-nested-data#whatIsFanoutIndex)
      * [Fanout index - Map index example](../indexes/indexing-nested-data#fanoutMapIndex)
      * [Fanout index - Map-Reduce index example](../indexes/indexing-nested-data#fanoutMapReduceIndex)
      * [Performance hints](../indexes/indexing-nested-data#performanceHints)
      * [Paging](../indexes/indexing-nested-data#paging)

{NOTE/}

---

{PANEL: Sample data}

* The examples in this article are based on the following **Classes** and **Sample Data**:

     {CODE-TABS}
     {CODE-TAB:php:Class online_shop_class@Indexes\IndexingNestedData.php /}
     {CODE-TAB:php:Sample_data sample_data@Indexes\IndexingNestedData.php /}
     {CODE-TABS/}

{PANEL/}

{PANEL: Simple index - Single index-entry per document}

* <a id="theIndex" /> **The index**:
  {CODE:php simple_index@Indexes\IndexingNestedData.php /}

* <a id="theIndexEntries" /> **The index-entries**:

     ![Simple - index-entries](images/indexing-nested-data-1.png "A single index-entry per document")

     1. The index-entries content is visible from the Studio [Query view](../studio/database/queries/query-view).

     2. Check option: _Show raw index-entries instead of Matching documents_.

     3. Each row represents an **index-entry**.  
        The index has a single index-entry per document (3 entries in this example).  

     4. The index-field contains a collection of ALL nested values from the document.  
        e.g. The third **index-entry** has the following values in the _Colors_ **index-field**:  
        `{"black", "blue", "red"}`

* <a id="queryingTheIndex" /> **Querying the index**:

     {CODE-TABS}
     {CODE-TAB:php:Query simple_index_query_1@Indexes\IndexingNestedData.php /}
     {CODE-TAB-BLOCK:sql:RQL}
     from index "Shops/ByTShirt/Simple"
where Colors == "red"
     {CODE-TAB-BLOCK/}
     {CODE-TABS/}

     {CODE:php results_1@Indexes\IndexingNestedData.php /}

* <a id="whenToUse" /> **When to use**:

   * This type of index structure is effective for retrieving documents when filtering the query by any of the inner nested values that were indexed.

   * However, due to the way the index-entries are generated, this index **cannot** provide results for a query searching for documents that contain 
     specific sub-objects which satisfy some `and_also` condition.   
     For example:   
     {CODE:php results_2@Indexes\IndexingNestedData.php /}

   * To address this, you must use a **Fanout index** - as described below.

{PANEL/}

{PANEL: Fanout index - Multiple index-entries per document}

* <a id="whatIsFanoutIndex" /> **What is a Fanout index**:

     * A fanout index is an index that outputs multiple index-entries per document.  
       A separate index-entry is created for each nested sub-object from the document.
 
     * The fanout index is useful when you need to retrieve documents matching query criteria  
       that search for specific sub-objects that comply with some logical conditions.

* <a id="fanoutMapIndex" /> **Fanout index - Map index example**:

     {CODE-TABS}
     {CODE-TAB:php:LINQ_index fanout_index_1@Indexes\IndexingNestedData.php /}
     {CODE-TAB:php:JavaScript_index fanout_index_js@Indexes\IndexingNestedData.php /}
     {CODE-TABS/}

     {CODE-TABS}
     {CODE-TAB:php:Query fanout_index_query_1@Indexes\IndexingNestedData.php /}
     {CODE-TAB-BLOCK:sql:RQL}
     from index "Shops/ByTShirt/Fanout" 
where Color == "red" and Size == "M"
     {CODE-TAB-BLOCK/}
     {CODE-TABS/}

     {CODE:php results_3@Indexes\IndexingNestedData.php /}

* <a id="fanoutMapIndexIndexEntries" /> **The index-entries**:
  ![Fanout - index-entries](images/indexing-nested-data-2.png "Multiple index-entries per document")

     1. The index-entries content is visible from the Studio [Query view](../studio/database/queries/query-view).

     2. Check option: _Show raw index-entries instead of Matching documents_.

     3. Each row represents an **index-entry**.  
        Each index-entry corresponds to an inner item in the TShirt list.

     4. In this example, the total number of index-entries is **12**,  
        which is the total number of inner items in the TShirt list in all **3** documents in the collection.

* <a id="fanoutMapReduceIndex" /> **Fanout index - Map-Reduce index example**:

     * The fanout index concept applies to map-reduce indexes as well:

          {CODE-TABS}
          {CODE-TAB:php:Fanout_index fanout_index_2@Indexes\IndexingNestedData.php /}
          {CODE-TABS/}

          {CODE-TABS}
          {CODE-TAB:php:Query fanout_index_query_4@Indexes\IndexingNestedData.php /}
          {CODE-TAB-BLOCK:sql:RQL}
          from index "Sales/ByTShirtColor/Fanout"
where Color == "black"
          {CODE-TAB-BLOCK/}
          {CODE-TABS/}

* <a id="performanceHints" /> **Fanout index - Performance hints**:

     * Fanout indexes are typically more resource-intensive than other indexes as RavenDB has to index a large number of index-entries. 
       This increased workload can lead to higher CPU and memory utilization, potentially causing a decline in the overall performance of the index.

     * When the number of index-entries generated from a single document exceeds a configurable limit,  
       RavenDB will issue a **High indexing fanout ratio** alert in the Studio notification center.

     * You can control when this performance hint is created by setting the 
       [PerformanceHints.Indexing.MaxIndexOutputsPerDocument](../server/configuration/performance-hints-configuration#performancehints.indexing.maxindexoutputsperdocument) configuration key 
       (default is 1024).

     * So, for example, adding another OnlineShop document with a `tShirt` object containing 1025 items  
       will trigger the following alert:  
       
          ![Figure 1. High indexing fanout ratio notification](images/fanout-index-performance-hint-1.png "High indexing fanout ratio notification")

     * Clicking the 'Details' button will show the following info:  

          ![Figure 2. Fanout index, performance hint details](images/fanout-index-performance-hint-2.png "Fanout index, performance hint details")

* <a id="paging" /> **Fanout index - Paging**:

     * A fanout index has more index-entries than the number of documents in the collection indexed.  
       Multiple index-entries "point" to the same document from which they originated,  
       as can be seen in the above [index-entries](../indexes/indexing-nested-data#fanoutMapIndexIndexEntries) example.

     * When making a fanout index query that should return full documents (without projecting results),  
       the `TotalResults` property (available via the `QueryStatistics` object) will contain  
       the total number of index-entries and Not the total number of resulting documents.

     * **To overcome this when paging results**, you must take into account the number of "duplicate"  
       index-entries that are skipped internally by the server when serving the resulting documents.  

     * Please refer to [paging through tampered results](../indexes/querying/paging#paging-through-tampered-results) for further explanation and examples. 

{PANEL/}

## Related articles

### Indexes

- [What are Indexes](../indexes/what-are-indexes)

### Querying

- [Intersect queries](../indexes/querying/intersection)
