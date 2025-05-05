# Indexes: Term Vectors

[Term Vector](https://en.wikipedia.org/wiki/Vector_space_model) is a representation of a text document as a vector of identifiers that can be used for similarity searches, information filtering, information retrieval, and indexing. In RavenDB the feature like [MoreLikeThis](../client-api/session/querying/how-to-use-morelikethis) is leveraging the term vectors to accomplish its purposes.

To create an index and enable Term Vectors on a specific field we can create an index using  the `AbstractIndexCreationTask`, then specify the term vectors there, or define our term vectors in the `IndexDefinition` (directly or using the `IndexDefinitionBuilder`).

{CODE-TABS}
{CODE-TAB:java:AbstractIndexCreationTask term_vectors_1@Indexes\TermVectors.java /}
{CODE-TAB:java:Operation term_vectors_2@Indexes\TermVectors.java /}
{CODE-TABS/}

The available Term Vector options are:

{CODE:java term_vectors_3@Indexes\TermVectors.java /}

## Related articles

### Indexes

- [Boosting](../indexes/boosting)
- [Analyzers](../indexes/using-analyzers)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Dynamic Fields](../indexes/using-dynamic-fields)
