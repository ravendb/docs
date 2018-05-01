# Querying : Query vs DocumentQuery

Unlike .NET client, Java client offers single API for querying documents. 

Below you can find examples of equivalent calls. 


`query` method in `DocumentSession` is shorthand for calling `session.advanced().documentQuery`.

{CODE:java query_1a@Indexes\Querying\QueryAndLuceneQuery.java /}

is equivalent to:

{CODE:java query_1b@Indexes\Querying\QueryAndLuceneQuery.java /}

<hr />

{CODE:java query_2a@Indexes\Querying\QueryAndLuceneQuery.java /}

is equivalent to:

{CODE:java query_2b@Indexes\Querying\QueryAndLuceneQuery.java /}

<hr />

{CODE:java query_3a@Indexes\Querying\QueryAndLuceneQuery.java /}

is equivalent to:

{CODE:java query_3b@Indexes\Querying\QueryAndLuceneQuery.java /}

<hr />

{CODE:java query_4a@Indexes\Querying\QueryAndLuceneQuery.java /}

is equivalent to:

{CODE:java query_4b@Indexes\Querying\QueryAndLuceneQuery.java /}


## Related Articles

- [Querying : Basics](../../indexes/querying/basics)
- [Client API : Session : How to query?](../../client-api/session/querying/how-to-query)
- [Client API : Session : How to use lucene in queries?](../../client-api/session/querying/lucene/how-to-use-lucene-in-queries)
