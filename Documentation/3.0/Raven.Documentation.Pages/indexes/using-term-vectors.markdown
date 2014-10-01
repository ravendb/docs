# Term Vectors

[Term Vector](http://en.wikipedia.org/wiki/Vector_space_model) is a representation of a text document as a vector of identifiers that can be used for similarity searches, information filtering, information retrieval and indexing. In RavenDB the features like [MoreLikeThis](../../../server/extending/bundles/morelikethis) or [text highlighting](../indexes/querying/highlights) are leveraging the term vectors to acomplish their purposes.

To create an index and enable Term Vectors on a specific field we can create index using `AbstractIndexCreationTask` and specify term vectors there or define our term vectors in `IndexDefinition` (directly or using `IndexDefinitionBuilder`).

{CODE-TABS}
{CODE-TAB:csharp:AbstractIndexCreationTask term_vectors_1@Indexes\TermVectors.cs /}
{CODE-TAB:csharp:Commands term_vectors_2@Indexes\TermVectors.cs /}
{CODE-TABS/}

The available Term Vector options are:

{CODE term_vectors_3@Indexes\TermVectors.cs /}

#### Related articles

TODO