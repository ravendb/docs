# What are Indexes
---

{NOTE: }

* Indexes in RavenDB enable efficient querying by processing raw data and providing fast query results without scanning the entire dataset each and every time.
  Learn more in the [Indexes overview](../indexes/what-are-indexes#indexes-overview) below.

* This page provides a [Basic example](../indexes/what-are-indexes#basic-example) of creating, deploying, and querying an index.
  Additional examples can be found in [Creating and deploying indexes](../indexes/creating-and-deploying), [Map Indexes](../indexes/map-indexes),
  and many other articles under the "Indexes" menu.

* In this page:
    * [Indexes overview](../indexes/what-are-indexes#indexes-overview)
    * [Types of indexes](../indexes/what-are-indexes#types-of-indexes)
    * [Basic example](../indexes/what-are-indexes#basic-example)
    * [Understanding index query results](../indexes/what-are-indexes#understanding-index-query-results)
    * [If indexes exhaust system resources](../indexes/what-are-indexes#if-indexes-exhaust-system-resources)

{NOTE/}

{PANEL: Indexes overview}

**Indexes are fundamental**:

* Indexes are fundamental to RavenDB’s query execution, enabling efficient query performance by processing the underlying data and delivering faster results.
* ALL queries in RavenDB use an index to deliver results and ensure optimal performance.  
  Learn more in [Queries always provide results using an index](../client-api/session/querying/how-to-query#queries-always-provide-results-using-an-index).

---

**Main concepts**:

* When discussing indexes in RavenDB, three key components come into play:
    * The index definition
    * The indexing process
    * The indexed data

* Each of these components is described in detail in [Indexes - The moving parts](../studio/database/indexes/indexes-overview#indexes---the-moving-parts).

---

**The indexing process**:

* The indexing process iterates over the raw documents, creating an **index-entry** for each document that is processed.
  (Usually a single index-entry is created per raw document, unless working with a [fanout index](../indexes/indexing-nested-data#fanout-index---multiple-index-entries-per-document)).
* Each index-entry contains **index-fields**, and each index-field contains content (**index-terms**) that was generated from the raw documents,
  as defined by the index definition and depending on the [analyzer](../indexes/using-analyzers) used.
* A map is built between the indexed-terms and the documents they originated from,  
  enabling you to query for documents based on the indexed data.

---

**Automatic data processing**:

* Once defined and deployed, an index will initially process the entire dataset.  
  After that, the index will only process documents that were modified, added or deleted.  
  This happens automatically without requiring direct user intervention.
* For example, if changes are made to documents in the "Orders" collection,  
  all indexes defined for the "Orders" collection will be triggered to update the index with the new data.
* This approach helps avoid costly table scans, allows the server to respond quickly,  
  and reduces the load on queries while optimizing machine resource usage.

---

**Background operation**:

* RavenDB indexes are designed to run asynchronously in the background.
* The indexing process does not block or interrupt database operations, such as writing data or running queries,
  though queries may temporarily return [stale results](../indexes/stale-indexes) until the index is fully updated.

---

**Separate storage**:

* Indexes store their processed data separately, ensuring that the raw data remains unaffected.  
  This separation helps maintain the integrity of the raw data while allowing the index to optimize query performance.

* If system resources become strained due to indexing, it may require adjustments to the index design, hardware, or other factors.
  Learn more in [If indexes exhaust system resources](../indexes/what-are-indexes#if-indexes-exhaust-system-resources).

{PANEL/}

{PANEL: Types of indexes}

* Indexes in RavenDB are categorized along the following axes:
    * **Auto** indexes -vs- **Static** indexes
    * **Map** indexes -vs- **Map-Reduce** indexes
    * **Single-Collection** (Single-Map) indexes -vs- **Multi-Collection** (Multi-Map) indexes

* For a detailed description of each type, refer to section [Index types](../studio/database/indexes/indexes-overview#index-types).

{PANEL/}

{PANEL: Basic example}

In this example we create a static-index that indexes content from documents in the `Employees` [collection](../client-api/faq/what-is-a-collection).  
This allows querying for _Employee_ documents by any of the index-fields (`FullName`, `LastName`, `Country`).

{CONTENT-FRAME: }

#### Define the index

* The first step is to define the index.  
  One way to do this is by extending the `AbstractIndexCreationTask` class.  
  Learn more in [Define a static-index using a custom class](../indexes/creating-and-deploying#using-abstractindexcreationtask).
* Other methods to create a static-index are:
  * [Creating a static-index using an Operation](../client-api/operations/maintenance/indexes/put-indexes)
  * [Creating a static-index from the Studio](../studio/database/indexes/indexes-list-view)

{CODE:java indexes_1@Indexes/WhatAreIndexes.java /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Deploy the index

* The next step is to deploy the index to the RavenDB server.  
  One way to do this is by calling `execute()` on the index instance.  
* Additional methods for deploying static-indexes are described in [Deploy a static index](../indexes/creating-and-deploying#deploy-a-static-index).  
* Once deployed, the indexing process will start indexing documents.  

{CODE:java indexes_2@Indexes/WhatAreIndexes.java /}

{CONTENT-FRAME/}
{CONTENT-FRAME: }

#### Query the index

* Now you can query the _Employees_ collection using the index.  
  In this example we query for _Employee_ documents, **filtering results based on index-fields** `LastName` and `Country`.
  The results will include only the _Employee_ documents that match the query predicate.
* For detailed guidance on querying with an index, refer to the [Querying an index](../indexes/querying/query-index).

{CODE:java indexes_3@Indexes/WhatAreIndexes.java /}

{CONTENT-FRAME/}
{PANEL/}

{PANEL: Understanding index query results}

{WARNING: }

A common mistake is treating indexes like SQL Views, but they are not analogous.  
The results of a query for a given index are the **full raw documents** that match the query predicate,  
and not just the indexed fields.

This behavior can be changed by applying [Projections](../indexes/querying/projections),  
which let you project the query results into selected fields instead of returning the entire document.

{WARNING/}

---

#### Viewing the resulting documents:

For example, the results shown in the following image are the **documents** that match the query predicate.

![Index query results - documents](images/index-query-results-1.png "Query results - Documents")

1. This is the index query.  
   The query predicate filters the resulting documents based on the content of the index-fields.
2. Each row in the results represents a **matching document**.
3. In this example, the `LastName`, `FirstName`, `Title`, etc., are the raw **document-fields**.

---

#### Viewing the index-entries:

If you wish to **view the index-entries** that compose the index itself,  
you can enable the option to show "raw index entries" instead of the matching documents.

![Index query results - index entries](images/index-query-results-2.png "Query results - Index Entries")

1. Query the index (no filtering is applied in this example).
2. Click the "Settings" button and toggle on "Show raw index-entries instead of matching documents".
3. Each row in the results represents an **index-entry**.
4. In this example, the `Country`, `FullName`, and `LastName` columns are the **index-fields**,  
   which were defined in the index definition.
5. This a **term**.  
   In this example, `usa` is a term generated by the analyzer for index-field `Country` from document `employees/4-a`.

{PANEL/}

{PANEL: If indexes exhaust system resources}

* The indexing process utilizes machine resources to keep the data up-to-date for queries.

* If indexing drains system resources, it may indicate one or more of the following:
    * Indexes may have been defined in a way that causes inefficient processing.
    * The [license](https://ravendb.net/buy) may need to be upgraded,
    * Your [cloud instance](../cloud/cloud-instances#a-production-cloud-cluster) (if used) may require optimization.
    * Hardware upgrades may be necessary to better support your workload.

* Refer to the [Indexing Performance View](../studio/database/indexes/indexing-performance) in the Studio to monitor and analyze the indexing process.  
  This view provides graphical representations and detailed statistics of all index activities at each stage.

* Additionally, refer to the [Common indexing issues](../studio/database/indexes/indexing-performance#common-indexing-issues) section
  for troubleshooting and resolving indexing challenges.

{PANEL/}

## Related Articles

### Client API

- [Query Overview](../client-api/session/querying/how-to-query)

### Indexes

- [Creating and Deploying Indexes](../indexes/creating-and-deploying)
- [Query an Index](../indexes/querying/query-index)
- [Stale Indexes](../indexes/stale-indexes)

### Studio

- [Indexes: Overview](../studio/database/indexes/indexes-overview#indexes-overview)
- [Studio Index List View](../studio/database/indexes/indexes-list-view)
