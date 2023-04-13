# Query vs DocumentQuery

Unlike .NET client, Java client offers single API for querying documents. 

Below you can find examples of equivalent calls. 


`query` method in `DocumentSession` is shorthand for calling `session.advanced().documentQuery`.

{CODE:java query_1a@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.java /}

is equivalent to:

{CODE:java query_1b@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.java /}

<hr />

{CODE:java query_2a@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.java /}

is equivalent to:

{CODE:java query_2b@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.java /}

<hr />

{CODE:java query_3a@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.java /}

is equivalent to:

{CODE:java query_3b@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.java /}

<hr />

{CODE:java query_4a@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.java /}

is equivalent to:

{CODE:java query_4b@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.java /}


## Related Articles

### Querying

- [Basics](../../../../indexes/querying/basics)

### Session 

- [How to Query](../../../../client-api/session/querying/how-to-query)
- [What is a Document Query](../../../../client-api/session/querying/document-query/what-is-document-query)
- [How to use Lucene](../../../../client-api/session/querying/document-query/how-to-use-lucene)
