# What Indexes Are

Indexes are server-side functions that define which fields and what values documents can be searched on. They are _the only way_ to satisfy queries in RavenDB. The whole indexing process is done in the background and is triggered whenever data is added or changed. This approach allows the server to respond quickly even when large amounts of data have changed and avoid costly table scan operations. You can read more about this (including mitigation techniques) on the [Stale Indexes](../indexes/stale-indexes) page.

The core of every index is its mapping function that utilizes LINQ-like syntax, and the result of such a mapping is converted to a [Lucene](http://lucene.apache.org/) index entry which is persisted for future use to avoid re-indexing each time the query is issued and keep response times minimal.

## Basic Example

In our example, we will create an index that will map documents from the `Employees` [collection](../client-api/faq/what-is-a-collection) and enable querying by `FirstName`, `LastName`, or both.

- First we need to create an index. One way to create it is to use the `AbstractIndexCreationTask`, but there are other ways available as well (you can read about them [here](../indexes/creating-and-deploying)).

{CODE:java indexes_1@Indexes/WhatAreIndexes.java /}

- The next step is to send an index to a server (more about index deployment options can be found [here](../indexes/creating-and-deploying)) so the indexing process can start indexing documents.

{CODE:java indexes_2@Indexes/WhatAreIndexes.java /}

- Now our index can be queried and indexed results can be returned.

{CODE:java indexes_3@Indexes/WhatAreIndexes.java /}

More examples with detailed descriptions can be found [here](../indexes/indexing-basics).

## Remarks

{WARNING:Remember}
A frequent mistake is to treat indexes as SQL Views, but they are not analogous. The **result of a query for the given index is a full document**, not only the fields that were indexed. 

This behavior can be altered by [storing](../indexes/storing-data-in-index) fields and doing [projections](../indexes/querying/projections).
{WARNING/}

## Related Articles

### Indexes

- [Creating and Deploying Indexes](../indexes/creating-and-deploying)
- [Indexing Basics](../indexes/indexing-basics)

### Querying

- [Basics](../indexes/querying/basics)

### Client API

- [How to Query](../client-api/session/querying/how-to-query)

### Studio

- [Indexes: Overview](../studio/database/indexes/indexes-overview#indexes-overview)
- [Studio Index List View](../studio/database/indexes/indexes-list-view)
