# Term Vectors

[Term Vector](http://en.wikipedia.org/wiki/Vector_space_model) is a representation of a text document as a vector of identifiers that can be used for similarity searches, information filtering, information retrieval and indexing. In RavenDB the features like [MoreLikeThis](../../../server/extending/bundles/morelikethis) or [text highlighting](searching#highlights) are leveraging the term vectors to acomplish their purposes.

To create an index and enable Term Vectors on a specific field we can create index using `AbstractIndexCreationTask` and specify term vectors there or define our term vectors in `IndexDefinition` (directly or using `IndexDefinitionBuilder`).

{CODE configuring_index_options_1@ClientApi\Querying\StaticIndexes\ConfiguringIndexOptions.cs /}

{CODE configuring_index_options_2@ClientApi\Querying\StaticIndexes\ConfiguringIndexOptions.cs /}

The available Term Vector options are:

{CODE configuring_index_options_3@ClientApi\Querying\StaticIndexes\ConfiguringIndexOptions.cs /}