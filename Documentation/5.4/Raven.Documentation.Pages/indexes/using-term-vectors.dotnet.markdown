# Indexes: Term Vectors
---

{NOTE: }

* A [Term Vector](https://en.wikipedia.org/wiki/Vector_space_model) is a representation of a text document 
  as a vector of identifiers.  
  Lucene indexes can contain term vectors for documents they index.  
* Term vectors can be used for various purposes, including similarity searches, information filtering 
  and retrieval, and indexing.  
  A book's index, for example, may have term vector enabled on the book's **subject** field, to be able 
  to use this field to search for books with similar subjects.  
* RavenDB features like [MoreLikeThis](../client-api/session/querying/how-to-use-morelikethis) leverage 
  stored term vectors to accomplish their goals.

* In this page:
   * [Creating an index and enabling Term Vectors on a field](../indexes/using-term-vectors#creating-an-index-and-enabling-term-vectors-on-a-field)
       * [Using the API](../indexes/using-term-vectors#using-the-api)  
       * [Using Studio](../indexes/using-term-vectors#using-studio)

{NOTE/}

---

{PANEL: Creating an index and enabling Term Vectors on a field}

Indexes that include term vectors can be created and configured using the API 
or Studio.  

## Using the API 

To create an index and enable Term Vectors on a specific field,  we can -  

A. Create an index using  the `AbstractIndexCreationTask`, and specify the term vectors there.  
B. Or, we can define our term vectors in the `IndexDefinition` (directly or using the `IndexDefinitionBuilder`).  

{CODE-TABS}
{CODE-TAB:csharp:AbstractIndexCreationTask term_vectors_1@Indexes\TermVectors.cs /}
{CODE-TAB:csharp:Operation term_vectors_2@Indexes\TermVectors.cs /}
{CODE-TABS/}

Available Term Vector options include:

{CODE term_vectors_3@Indexes\TermVectors.cs /}

Learn which Lucene API methods and constants are available [here](https://lucene.apache.org/core/3_6_2/api/all/org/apache/lucene/document/Field.TermVector.html).

## Using Studio

Let's use as an example one of Studio's sample indexes, `Product/Search`, that has term vector 
enabled on its `Name` field so a feature like [MoreLikeThis](../client-api/session/querying/how-to-use-morelikethis) 
can use this fiels to select a product and find products similar to it.  

![Term vector enabled on index field](images/term-vector-enabled.png "Term vector enabled on index field")

We can now use a query like:

{CODE-BLOCK:sql} 
from index 'Product/Search' 
where morelikethis(id() = 'products/7-A')
{CODE-BLOCK/}

{PANEL/}

## Related articles

### Indexes

- [Boosting](../indexes/boosting)
- [Analyzers](../indexes/using-analyzers)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Dynamic Fields](../indexes/using-dynamic-fields)

## External articles

- [Lucene API](https://lucene.apache.org/core/3_6_2/api/all/org/apache/lucene/document/Field.TermVector.html)
