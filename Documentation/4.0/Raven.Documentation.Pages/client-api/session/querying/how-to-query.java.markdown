# Session: Querying: How to Query

This session explains the following methods to query a database:

* `session.query`
* `session.advanced().documentQuery`
* `session.advanced().rawQuery`

## Session.Query

The most straightforward way to issue a query is by using the `query` method.

### Syntax

{CODE:java query_1_0@ClientApi\Session\Querying\HowToQuery.java /}

| Parameters | | |
| ------------- | ------------- | ----- |
| **clazz** | Class | Target type |
| **collectionOrIndexName** | Query | Collection name of index name to use |
| **indexClazz** | Class | Indicates class of index to use |

| Return Value | | 
| ------------- | ----- |
| IDocumentQuery | Instance implementing `IDocumentQuery` interface containing additional query methods|


###Example I - Basic Dynamic Query

{CODE:java query_1_1@ClientApi\Session\Querying\HowToQuery.java /}

The above is an example of a dynamic query which doesn't require you to specify an index name. RavenDB will create an auto index automatically if necessary.

The provided `Employee` type as the generic type parameter does not only define the type of returned
results, but it also indicates that the queried collection will be `Employees`.

### Example II - Query Syntax

{CODE:java query_1_2@ClientApi\Session\Querying\HowToQuery.java /}

### Example III - Using Specific Index

{CODE:java query_1_4@ClientApi\Session\Querying\HowToQuery.java /}

or 

{CODE:java query_1_5@ClientApi\Session\Querying\HowToQuery.java /}

## session.advanced().documentQuery

### Example IV

{CODE:java query_1_6@ClientApi\Session\Querying\HowToQuery.java /}

## session.advanced().rawQuery

Queries in RavenDB use a SQL-like language called RavenDB Query Language ([RQL](../../../indexes/querying/what-is-rql)). All of the above queries generate RQL sent to the server. The session also gives you the way to express the query directly in RQL using `rawQuery` method.

### Example IV

{CODE:java query_1_7@ClientApi\Session\Querying\HowToQuery.java /}

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
