# Indexes: Indexing Related Documents

To extend indexing capabilities and simplify many scenarios, we have introduced the possibility for indexing related documents.

## Example I

Let's consider a simple `Product - Category` scenario where you want to look for a `Product` by `Category Name`.

Without this feature, you would have to create a fairly complex multiple map-reduce index. This is why the `LoadDocument` function was introduced.

{CODE-TABS}
{CODE-TAB:java:AbstractIndexCreationTask indexing_related_documents_2@Indexes\IndexingRelatedDocuments.java /}
{CODE-TAB:java:Operation indexing_related_documents_3@Indexes\IndexingRelatedDocuments.java /}
{CODE-TAB:java:JavaScript indexing_related_documents_2@Indexes\JavaScript.java /}
{CODE-TABS/}

Now we will be able to search for products using the `CategoryName` as a parameter:

{CODE:java indexing_related_documents_7@Indexes\IndexingRelatedDocuments.java /}

## Example II

Our next scenario will show us how indexing of more complex relationships is also trivial. Let's consider the following case:

{CODE:java indexing_related_documents_4@Indexes\IndexingRelatedDocuments.java /}

To create an index with `Author Name` and list of `Book Names`, we need do the following:

{CODE-TABS}
{CODE-TAB:java:AbstractIndexCreationTask indexing_related_documents_5@Indexes\IndexingRelatedDocuments.java /}
{CODE-TAB:java:Operation indexing_related_documents_6@Indexes\IndexingRelatedDocuments.java /}
{CODE-TAB:java:JavaScript indexing_related_documents_5@Indexes\JavaScript.java /}
{CODE-TABS/}

{CODE:java indexing_related_documents_8@Indexes\IndexingRelatedDocuments.java /}

## Remarks

{INFO Indexes are updated automatically when related documents change. /}

{WARNING Using the `LoadDocument` adds a loaded document to the tracking list. This may cause very expensive calculations to occur, especially when multiple documents are tracking the same document. /}

## Related Articles

### Indexes

- [Indexing Basics](../indexes/indexing-basics)
- [Indexing Hierarchical Data](../indexes/indexing-hierarchical-data)
- [Indexing Spatial Data](../indexes/indexing-spatial-data)
- [Indexing Polymorphic Data](../indexes/indexing-polymorphic-data)

### Querying

- [Basics](../indexes/querying/basics)
