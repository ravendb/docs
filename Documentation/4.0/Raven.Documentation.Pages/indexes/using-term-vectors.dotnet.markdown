# Indexes: Term Vectors
---

{NOTE: }

* A [Term Vector](https://en.wikipedia.org/wiki/Vector_space_model) is a representation of a text document 
  as a vector of identifiers.  
* A term vector can be used for similarity searches, information filtering, information retrieval, and indexing.  
* In RavenDB, features like [MoreLikeThis](../client-api/session/querying/how-to-use-morelikethis) leverage 
  term vectors to accomplish their goals.

* In this page:
   * Creating an index that enables term vectors  

{NOTE/}

---

{PANEL: }

To create an index and enable Term Vectors on a specific field we can create an index using  
the `AbstractIndexCreationTask`, then specify the term vectors there, or define our term vectors 
in the `IndexDefinition` (directly or using the `IndexDefinitionBuilder`).

{CODE-TABS}
{CODE-TAB:csharp:AbstractIndexCreationTask term_vectors_1@Indexes\TermVectors.cs /}
{CODE-TAB:csharp:Operation term_vectors_2@Indexes\TermVectors.cs /}
{CODE-TABS/}

The available Term Vector options are:

{CODE term_vectors_3@Indexes\TermVectors.cs /}

{PANEL/}

## Related articles

### Indexes

- [Boosting](../indexes/boosting)
- [Analyzers](../indexes/using-analyzers)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Dynamic Fields](../indexes/using-dynamic-fields)
