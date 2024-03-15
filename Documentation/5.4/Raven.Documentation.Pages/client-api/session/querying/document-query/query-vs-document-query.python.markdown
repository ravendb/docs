# query vs document_query

---

{NOTE: }

* RavenDB Queries can be executed using `query` or `document_query`, or by passing 
  [RQL](../../../../client-api/session/querying/what-is-rql) directly to the server 
  via `raw_query`.  
  Learn more in [Query Overview](../../../../client-api/session/querying/how-to-query).

* In the Python client API, `query` methods and their equivalent `document_query` methods 
  provide the same functionality. (This is different from the C# client implementation, 
  which often provides different functionality for `Query` methods and their `DocumentQuery` 
  counterparts.)  
  Therefore the Python documentation often provides `query` usage samples without adding 
  `document_query` examples as well.  

* In this page:
  * [API support](../../../../client-api/session/querying/document-query/query-vs-document-query#api-support)
  * [Immutability](../../../../client-api/session/querying/document-query/query-vs-document-query#immutability)
  * [Default query operator](../../../../client-api/session/querying/document-query/query-vs-document-query#default-query-operator)

{NOTE/}

---

{PANEL: API support}

**Query**:

* The API exposed by the _query_ method is a wrapper of _document_query_ and is built on top of it.

* When using _query_, the query is translated into a _document_query_ object,  
  which then builds into an RQL that is sent to the server.

* The available _query_ methods are listed [here](../../../../client-api/session/querying/how-to-query#custom-methods).

---

**document_query**:

* When using _document_query_, the query is translated into an RQL that is sent to the server.

* The available _document_query_ methods and extensions are listed [here](../../../../client-api/session/querying/document-query/what-is-document-query#custom-methods-and-extensions).

{PANEL/}

{PANEL: Immutability}

* `query` is **immutable** while `document_query` is **mutable**.  
  You may get different results if you try to *reuse* a query.

---

* The usage of the `Query` method in the following example:

    {CODE:python immutable_query@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.py /}

    will result with the following Lucene-syntax queries:

    `query: from Users where startsWith(Name, 'A')`

    `ageQuery: from Users where startsWith(Name, 'A') and Age > 21`

    `eyeQuery: from Users where startsWith(Name, 'A') and EyeColor = 'blue'`

---

* A similar usage with `document_query`:

    {CODE:python mutable_query@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.py /}

    will result with the following Lucene queries:

    `documentQuery: from Users where startsWith(Name, 'A')`  
    (before creating `ageDocumentQuery`)

    `ageDocumentQuery: from Users where startsWith(Name, 'A') and Age > 21`  
    (before creating `eyeDocumentQuery`)

    `eyeDocumentuery: from Users where startsWith(Name, 'A') and Age > 21 and EyeColor = 'blue'`

    All created Lucene queries are the same query (actually the same instance).  
    This is an important hint to be aware of if you are going to reuse `document_query`.

{PANEL/}

{PANEL: Default Query Operator}

* Both `query` and `document_query` use `AND` as the default operator.  

* The operator can be replaced by calling `using_default_operator`:
        
{CODE:python default_operator@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.py /}

{PANEL/}

## Related Articles

### Session 

- [Query Overview](../../../../client-api/session/querying/how-to-query)
- [What is a Document Query](../../../../client-api/session/querying/document-query/what-is-document-query)
- [How to use Lucene](../../../../client-api/session/querying/document-query/how-to-use-lucene)

### Indexes

- [Querying an Index](../../../../indexes/querying/query-index)
