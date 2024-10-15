# Query vs DocumentQuery

---

{NOTE: }

* RavenDB Queries can be executed using `query` or `document_query`, or by passing 
  [RQL](../../../../client-api/session/querying/what-is-rql) directly to the server 
  via `raw_query`.  
  Learn more in [Query Overview](../../../../client-api/session/querying/how-to-query).

* In the PHP client API, `query` methods and their equivalent `documentQuery` methods 
  provide the same functionality. (This is different from the C# client implementation, 
  which often provides different functionality for `Query` methods and their `DocumentQuery` 
  counterparts.)  
  The PHP documentation therefore often provides `query` usage samples without adding 
  `documentQuery` examples as well.  

* In this page:
   * [API support](../../../../client-api/session/querying/document-query/query-vs-document-query#api-support)
   * [`query `and `documentQuery` equivalents](../../../../client-api/session/querying/document-query/query-vs-document-query#queryand-documentquery-equivalents)

{NOTE/}

---

{PANEL: API support}

* `query` and `documentQquery` queries are translated to RQL and sent to the server.  
* Available _query_ methods are listed [here](../../../../client-api/session/querying/how-to-query#custom-methods).  
* Available _documentQuery_ methods and extensions are listed [here](../../../../client-api/session/querying/document-query/what-is-document-query#custom-methods-and-extensions).  

{PANEL/}

{PANEL: `query `and `documentQuery` equivalents}

#### 1. 

{CODE:php query_1a@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.php /}

is equivalent to:

{CODE:php query_1b@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.php /}

---

#### 2. 

{CODE:php query_2a@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.php /}

is equivalent to:

{CODE:php query_2b@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.php /}

---

#### 3. 

{CODE:php query_3a@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.php /}

is equivalent to:

{CODE:php query_3b@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.php /}

---

#### 4. 

{CODE:php query_4a@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.php /}

is equivalent to:

{CODE:php query_4b@ClientApi\Session\Querying\DocumentQuery\QueryVsDocumentQuery.php /}

{PANEL/}

## Related Articles

### Session 

- [Query Overview](../../../../client-api/session/querying/how-to-query)
- [What is a Document Query](../../../../client-api/session/querying/document-query/what-is-document-query)
- [How to use Lucene](../../../../client-api/session/querying/document-query/how-to-use-lucene)

### Indexes

- [Querying an Index](../../../../indexes/querying/query-index)
