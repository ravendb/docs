# Query vs DocumentQuery

---

{NOTE: }

* Queries in RavenDB can be written using `Query`, `DocumentQuery`, `RawQuery`, or directly with [RQL](../../../../client-api/session/querying/what-is-rql).  
  Learn more in [Query Overview](../../../../client-api/session/querying/how-to-query).

* The main differences between `Query` and `DocumentQuery` are outlined in this article.

* In this page:
  * [API support](../../../../client-api/session/querying/document-query/query-vs-document-query#api-support)
  * [Immutability](../../../../client-api/session/querying/document-query/query-vs-document-query#immutability)
  * [Default query operator](../../../../client-api/session/querying/document-query/query-vs-document-query#default-query-operator)

{NOTE/}

---

{PANEL: API support}

__Query__:

* The `Query` API supports LINQ, the essential data access solution in .NET.

* The API exposed by the _Query_ method is a wrapper of _DocumentQuery_ and is built on top of it.

* When using _Query_, the query is translated into a _DocumentQuery_ object,  
  which then builds into an RQL that is sent to the server.

* The available _Query_ methods and extensions are listed [here](../../../../client-api/session/querying/how-to-query#custom-methods-and-extensions-for-linq).

---

__DocumentQuery__:

* `DocumentQuery` does Not support LINQ.
 
* It exposes a lower-level API that provides more flexibility and control when building a query.

* When using _DocumentQuery_, the query is translated into an RQL that is sent to the server.

* The available _DocumentQuery_ methods and extensions are listed [here](../../../../client-api/session/querying/document-query/what-is-document-query#custom-methods-and-extensions).

---

{NOTE: }

__Note__:

`Query` and `DocumentQuery` can be converted to one another.  
This enables you to take advantage of all available API methods & extensions.  
See [Convert between DocumentQuery and Query](../../../../client-api/session/querying/document-query/what-is-document-query#convert-between-documentquery-and-query).

{NOTE/}

{PANEL/}

{PANEL: Immutability}

* `Query` is __immutable__ while `DocumentQuery` is __mutable__.  
  You might get different results if you try to *reuse* a query.

---

* The usage of the `Query` method in the following example:

    {CODE:python immutable_query@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.py /}

    will result with the following Lucene-syntax queries:

    `query: from Users where startsWith(Name, 'A')`

    `ageQuery: from Users where startsWith(Name, 'A') and Age > 21`

    `eyeQuery: from Users where startsWith(Name, 'A') and EyeColor = 'blue'`

---

* A similar usage with `DocumentQuery`:

    {CODE:python mutable_query@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.py /}

    will result with the following Lucene queries:

    `documentQuery: from Users where startsWith(Name, 'A')`  
    (before creating `ageDocumentQuery`)

    `ageDocumentQuery: from Users where startsWith(Name, 'A') and Age > 21`  
    (before creating `eyeDocumentQuery`)

    `eyeDocumentuery: from Users where startsWith(Name, 'A') and Age > 21 and EyeColor = 'blue'`

    All created Lucene queries are the same query (actually the same instance).  
    This is an important hint to be aware of if you are going to reuse `DocumentQuery`.

{PANEL/}

{PANEL: Default Query Operator}

* Starting from version 4.0, both `Query` and `DocumentQuery` use `AND` as the default operator.  
  (Previously, `Query` used `AND` and `DocumentQuery` used `OR`).

* This behavior can be modified by calling `UsingDefaultOperator`:
        
{CODE:python default_operator@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.py /}

{PANEL/}

## Related Articles

### Session 

- [Query Overview](../../../../client-api/session/querying/how-to-query)
- [What is a Document Query](../../../../client-api/session/querying/document-query/what-is-document-query)
- [How to use Lucene](../../../../client-api/session/querying/document-query/how-to-use-lucene)

### Indexes

- [Querying an Index](../../../../indexes/querying/query-index)
