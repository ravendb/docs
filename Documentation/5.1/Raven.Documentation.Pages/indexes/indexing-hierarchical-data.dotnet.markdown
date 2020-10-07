# Indexes: Indexing Hierarchical Data

One of the greatest advantages of a document database is that we have very few limits on how we structure our data. One very common scenario is the usage of hierarchical data structures. The most trivial of them is the comment thread:

{CODE indexes_1@Indexes\IndexingHierarchicalData.cs /}

While it is very easy to work with such a structure in all respects, it does bring up an interesting question, namely how can we search for all blog posts that were commented by specified author?

The answer to that is that RavenDB contains built-in support for indexing hierarchies, and you can take advantage of the `Recurse` method to define an index using the following syntax:

{CODE-TABS}
{CODE-TAB:csharp:AbstractIndexCreationTask indexes_2@Indexes\IndexingHierarchicalData.cs /}
{CODE-TAB:csharp:Operation indexes_3@Indexes\IndexingHierarchicalData.cs /}
{CODE-TAB:csharp:JavaScript indexes_2@Indexes\JavaScript.cs /}
{CODE-TABS/}

This will index all the comments in the thread, regardless of their location in the hierarchy.

{CODE-TABS}
{CODE-TAB:csharp:Query indexes_4@Indexes\IndexingHierarchicalData.cs /}
{CODE-TAB:csharp:DocumentQuery indexes_5@Indexes\IndexingHierarchicalData.cs /}
{CODE-TABS/}

## Related articles

### Indexes

- [Indexing Basics](../indexes/indexing-basics)
- [Indexing Related Documents](../indexes/indexing-related-documents)
- [Indexing Spatial Data](../indexes/indexing-spatial-data)
- [Indexing Polymorphic Data](../indexes/indexing-polymorphic-data)

### Querying 

- [Basics](../indexes/querying/basics)
