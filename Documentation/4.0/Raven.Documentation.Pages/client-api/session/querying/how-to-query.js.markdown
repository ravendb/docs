# Session: Querying: How to Query

This session explains the following methods to query a database:

* `session.query()`
* `session.advanced.documentQuery()`
* `session.advanced.rawQuery()`

## Session.Query

The most straightforward way to issue a query is by using the `query()` method.

### Syntax

{CODE:nodejs query_1_0@client-api\session\querying\howToQuery.js /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **documentType** | class | A class constructor used for reviving the results' entities from which the collection name is determined |
| **options** | object | |
| &nbsp;&nbsp;*indexName* | string | Name of an index to perform a query on (exclusive with *collectionName*)  |
| &nbsp;&nbsp;*collection* | string | Name of a collection to perform a query on (exclusive with *indexName*) |
| &nbsp;&nbsp;*documentType* | function | A class constructor used for reviving the results' entities |

| Return Value | | 
| ------------- | ----- |
| `Promise<IDocumentQuery>` | Promise resolving to instance implementing `IDocumentQuery` interface containing additional query methods |


###Example I - Basic Dynamic Query

{CODE:nodejs query_1_1@client-api\session\querying\howToQuery.js /}

The above is an example of a dynamic query which doesn't require you to specify an index name. RavenDB will create an auto index automatically if necessary.

The provided `Employee` type as the generic type parameter does not only define the type of returned
results, but it also indicates that the queried collection will be `Employees`.

### Example II - Query Syntax

{CODE:nodejs query_1_2@client-api\session\querying\howToQuery.js /}

### Example III - Using Specific Index

{CODE:nodejs query_1_4@client-api\session\querying\howToQuery.js /}


## session.advanced.documentQuery()

### Example IV

{CODE:nodejs query_1_6@client-api\session\querying\howToQuery.js /}

## session.advanced.rawQuery()

Queries in RavenDB use a SQL-like language called RavenDB Query Language ([RQL](../../../indexes/querying/what-is-rql)). All of the above queries generate RQL sent to the server. The session also gives you the way to express the query directly in RQL using `rawQuery()` method.

### Example IV

{CODE:nodejs query_1_7@client-api\session\querying\howToQuery.js /}

### On entities loading, JS classes and the&nbsp;*documentType*&nbsp;parameter

Type information about the entity and its contents is by default stored in the document metadata. Based on that its types are revived when loaded from the server.

{INFO: Entity type registration }
In order to avoid passing **documentType** argument every time, you can register the type in the document conventions using the `registerEntityType()` method before calling DocumentStore's `initialize()` like so:

{CODE:nodejs query_1_8@client-api\session\querying\howToQuery.js /}

{INFO/}

If you fail to do so, entities (and all subobjects) loaded from the server are going to be plain object literals and not instances of the original type they were stored with.

## Related Articles

### Session

- [How to Project Query Results](../../../client-api/session/querying/how-to-project-query-results)
- [How to Stream Query Results](../../../client-api/session/querying/how-to-stream-query-results)
- [What is a Document Query](../../../client-api/session/querying/document-query/what-is-document-query)

### Querying

- [Basics](../../../indexes/querying/basics)
- [Filtering](../../../indexes/querying/filtering)
- [Paging](../../../indexes/querying/paging)
- [Sorting](../../../indexes/querying/sorting)
- [Projections](../../../indexes/querying/projections)

### Indexes

- [What are Indexes](../../../indexes/what-are-indexes)  
- [Indexing Basics](../../../indexes/indexing-basics)
- [Map Indexes](../../../indexes/map-indexes)
