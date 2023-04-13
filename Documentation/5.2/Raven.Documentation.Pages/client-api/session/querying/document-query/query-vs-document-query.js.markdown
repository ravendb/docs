# Query vs DocumentQuery

Unlike .NET, Node.js client offers single API for querying documents. 

Below you can find examples of equivalent calls. 

`query()` method in `DocumentSession` is shorthand for calling `session.advanced.documentQuery()`.

{CODE:nodejs query_1a@ClientApi\Session\Querying\DocumentQuery\queryVsDocumentQuery.js /}

is equivalent to:

{CODE:nodejs query_1b@ClientApi\Session\Querying\DocumentQuery\queryVsDocumentQuery.js /}

<hr />

{CODE:nodejs query_2a@ClientApi\Session\Querying\DocumentQuery\queryVsDocumentQuery.js /}

is equivalent to:

{CODE:nodejs query_2b@ClientApi\Session\Querying\DocumentQuery\queryVsDocumentQuery.js /}

<hr />

{CODE:nodejs query_3a@ClientApi\Session\Querying\DocumentQuery\queryVsDocumentQuery.js /}

is equivalent to:

{CODE:nodejs query_3b@ClientApi\Session\Querying\DocumentQuery\queryVsDocumentQuery.js /}

<hr />

{CODE:nodejs query_4a@ClientApi\Session\Querying\DocumentQuery\queryVsDocumentQuery.js /}

is equivalent to:

{CODE:nodejs query_4b@ClientApi\Session\Querying\DocumentQuery\queryVsDocumentQuery.js /}


## Related Articles

### Querying

- [Basics](../../../../indexes/querying/basics)

### Session 

- [How to Query](../../../../client-api/session/querying/how-to-query)
- [What is a Document Query](../../../../client-api/session/querying/document-query/what-is-document-query)
- [How to use Lucene](../../../../client-api/session/querying/document-query/how-to-use-lucene)
