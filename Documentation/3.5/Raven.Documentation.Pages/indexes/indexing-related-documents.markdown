# Indexing Related Documents

To extend indexing capabilities and simplify many scenarios, we have introduced the possibility for indexing related documents.

## Example I

Firstly, let's consider a simple `Product - Category` scenario where you want to look for a `Product` by `Category Name`.

Without this feature, the index that had to be created would be a fairly complex multiple map-reduce index. This is why the `LoadDocument` function was introduced.

{CODE-TABS}
{CODE-TAB:csharp:AbstractIndexCreationTask indexing_related_documents_2@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB:csharp:Commands indexing_related_documents_3@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TABS/}

Now we will be able to search for products using the `Category Name` as a parameter:

{CODE indexing_related_documents_7@Indexes\IndexingRelatedDocuments.cs /}

## Example II

Our next scenario will show us that indexing of more complex relationships is also trivial. Let's consider the following case:

{CODE indexing_related_documents_4@Indexes\IndexingRelatedDocuments.cs /}

Now, to create an index with `Author Name` and list of `Book Names`, we need do the following:

{CODE-TABS}
{CODE-TAB:csharp:AbstractIndexCreationTask indexing_related_documents_5@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB:csharp:Commands indexing_related_documents_6@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TABS/}

{CODE indexing_related_documents_8@Indexes\IndexingRelatedDocuments.cs /}

## Remarks

{INFO Indexes are updated automatically when related documents change. /}

{WARNING Using the `LoadDocument` adds a loaded document to the tracking list. This may cause very expensive calculations to occur, especially when multiple documents are tracking the same document. /}

## Related articles

- [Indexing : Basics](../indexes/indexing-basics)
- [Indexing hierarchical data](../indexes/indexing-hierarchical-data)
- [Indexing spatial data](../indexes/indexing-spatial-data)
- [Indexing polymorphic data](../indexes/indexing-polymorphic-data)
- [Querying : Basics](../indexes/querying/basics)
