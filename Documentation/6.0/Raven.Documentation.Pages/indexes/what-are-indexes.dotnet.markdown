# What Indexes Are
---

{NOTE: }

* Indexes are server-side functions that define which fields and what values documents can be searched on.  
  They are _the only way_ to satisfy queries in RavenDB.  

* The indexing process is performed in the background and is triggered whenever data is added or changed.  
  This approach allows the server to respond quickly even when large amounts of data have changed and avoid 
  costly table scan operations.  

* The core of every index is its mapping function, that utilizes LINQ-like syntax.  
  The result of this mapping is converted to a [Lucene](http://lucene.apache.org/) index entry 
  which is persisted for future use to avoid re-indexing each time the query is issued and keep 
  response times minimal.

* This page shows a basic example for the creation, deployment, and usage of an index.  
  Additional examples are available, among other places, on the [Indexing Basics](../indexes/indexing-basics) 
  and [Map Indexes](../indexes/map-indexes) pages.  

* In this page:
   * [Basic Example](../indexes/what-are-indexes#basic-example)
   * [Remarks](../indexes/what-are-indexes#remarks)

{NOTE/}

{PANEL: Basic Example}

In the below example we create an index that maps documents from the `Employees` 
[collection](../client-api/faq/what-is-a-collection) and enables querying by `FirstName`, 
`LastName`, or both.

- First, we need to create an index.  
  One way to create it is to use the `AbstractIndexCreationTask`, but there are other ways 
  available as well (you can read about them [here](../indexes/creating-and-deploying)).  
  {CODE indexes_1@Indexes/WhatAreIndexes.cs /}

- The next step is to send an index to a server (more about index deployment options can 
  be found [here](../indexes/creating-and-deploying)) so the indexing process can start indexing documents.  
  {CODE indexes_2@Indexes/WhatAreIndexes.cs /}

- Now, our index can be queried and indexed results can be returned.  
  {CODE indexes_3@Indexes/WhatAreIndexes.cs /}

{PANEL/}

{PANEL: Remarks}

{WARNING:Remember}
A frequent mistake is to handle indexes as SQL Views, but they are not analogous.  
The **result of a query for the given index is a full document**, not only the fields 
that were indexed. 

This behavior can be altered by [storing](../indexes/storing-data-in-index) fields 
and applying [projections](../indexes/querying/projections).  
{WARNING/}

{PANEL/}

## Related Articles

### Client API

- [Query Overview](../client-api/session/querying/how-to-query)

### Indexes

- [Creating and Deploying Indexes](../indexes/creating-and-deploying)
- [Indexing Basics](../indexes/indexing-basics)
- [Query an Index](../indexes/querying/query-index)
- [Stale Indexes](../indexes/stale-indexes)

### Studio

- [Indexes: Overview](../studio/database/indexes/indexes-overview#indexes-overview)
- [Studio Index List View](../studio/database/indexes/indexes-list-view)

### Sharding
- [Indexing in a Sharded DB](../sharding/indexing)  
- [Querying in a Sharded DB](../sharding/querying)  
