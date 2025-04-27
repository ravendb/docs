# Indexes: Term Vectors

[Term Vector](https://en.wikipedia.org/wiki/Vector_space_model) is a representation of a text document as a vector of identifiers that can be used for similarity searches, information filtering, information retrieval, and indexing. In RavenDB the feature like [MoreLikeThis](../client-api/session/querying/how-to-use-morelikethis) is leveraging the term vectors to accomplish its purposes.

To create an index and enable Term Vectors on a specific field we can create an index using  the `AbstractIndexCreationTask`, then specify the term vectors there, or define our term vectors in the `IndexDefinition` (directly or using the `IndexDefinitionBuilder`).

{CODE-TABS}
{CODE-TAB:nodejs:AbstractIndexCreationTask term_vectors_1@indexes\termVectors.js /}
{CODE-TAB:nodejs:Operation term_vectors_2@indexes\termVectors.js /}
{CODE-TABS/}

The available Term Vector options are:

| Term Vector | |
| ----------- | - |
| `"No"` | Do not store term vectors |
| `"Yes"` | Store the term vectors of each document. A term vector is a list of the document's terms and their number of occurrences in that document. |
| `"WithPositions"` | Store the term vector + token position information |
| `"WithOffsets"` | Store the term vector + token offset information |
| `"WithPositionsAndOffsets"` | Store the term vector + token position and offset information |

## Related articles

### Indexes

- [Boosting](../indexes/boosting)
- [Analyzers](../indexes/using-analyzers)
- [Storing Data in Index](../indexes/storing-data-in-index)
- [Dynamic Fields](../indexes/using-dynamic-fields)
