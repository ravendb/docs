# What are indexes?

Indexes are server-side functions that define using which fields (and what values) document can be searched on and are _the only way_ to satisfy queries in RavenDB. The whole indexing process is done in the background and is triggered whenever data is added or changed. This approach allows the server to respond quickly even when large amounts of data have changed and avoid costly table scans operations, however implication of this choice is that results might be stale (you can read more about staleness implications and the ways to handle it [here](../indexes/stale-indexes)).

The core of every index is its mapping function with LINQ-like syntax and the result of such a mapping is converted to [Lucene](http://lucene.apache.org/) index entry, which is persisted for future use to avoid reindexation each time when query is issued and to achieve fast response times.

## Basic example

In our example, we will create an index that will map documents from [collection](../client-api/faq/what-is-a-collection) `Employees` and enable querying by `FirstName`, `LastName` or both.

- first we need to create an index. One way to create is is to use `AbstractIndexCreationTask`, but there are other ways and you can read about them [here](../indexes/creating-and-deploying).

{CODE indexes_1@Indexes/WhatAreIndexes.cs /}

- next step is to send index to server (more about index deployment options can be found [here](../indexes/creating-and-deploying)), so indexing process can start indexing documents.

{CODE indexes_2@Indexes/WhatAreIndexes.cs /}

- now, our index can be queried, and indexed results can be returned.

{CODE indexes_3@Indexes/WhatAreIndexes.cs /}

More examples with detailed description can be found [here](../indexes/indexing-basics).

## Remarks

{WARNING:Remember}
Often mistake is to treat indexes as SQL Views, but this is not the case. The **result of a query for given index is a full document**, not only the fields that were indexed. 

This behavior can be altered by [storing](../indexes/storing-data-in-index) fields and doing [projections](../indexes/querying/projections).
{WARNING/}

## Related articles

- [Indexes : Creating and deploying indexes](../indexes/creating-and-deploying)
- [Indexes : Basics](../indexes/indexing-basics)
- [Querying : Basics](../indexes/querying/basics)
- [Client API : How to create a document store?](../client-api/creating-document-store)
