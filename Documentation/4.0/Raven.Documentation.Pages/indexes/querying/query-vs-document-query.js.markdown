# Querying: Query vs DocumentQuery

Unlike .NET, Node.js client offers single API for querying documents. 

Below you can find examples of equivalent calls. 

`query()` method in `DocumentSession` is shorthand for calling `session.advanced.documentQuery()`.

{CODE:nodejs query_1a@indexes\querying\queryAndLuceneQuery.js /}

is equivalent to:

{CODE:nodejs query_1b@indexes\querying\queryAndLuceneQuery.js /}

<hr />

{CODE:nodejs query_2a@indexes\querying\queryAndLuceneQuery.js /}

is equivalent to:

{CODE:nodejs query_2b@indexes\querying\queryAndLuceneQuery.js /}

<hr />

{CODE:nodejs query_3a@indexes\querying\queryAndLuceneQuery.js /}

is equivalent to:

{CODE:nodejs query_3b@indexes\querying\queryAndLuceneQuery.js /}

<hr />

{CODE:nodejs query_4a@indexes\querying\queryAndLuceneQuery.js /}

is equivalent to:

{CODE:nodejs query_4b@indexes\querying\queryAndLuceneQuery.js /}


## Related Articles

### Querying

- [Basics](../../indexes/querying/basics)

### Session 

- [How to Query](../../client-api/session/querying/how-to-query)
- [What is a Document Query](../../client-api/session/querying/document-query/what-is-document-query)
- [How to use Lucene](../../client-api/session/querying/document-query/how-to-use-lucene)
