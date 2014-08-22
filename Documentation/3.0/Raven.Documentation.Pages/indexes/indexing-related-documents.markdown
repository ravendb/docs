# Indexing Related Documents

To extend indexing capabilities and simplify many scenarios, we have introduced the possibility to index related documents.

## Example I

For start, lets consider a simple `Product - Category` scenario where you want to lookup for a `Product` by `Category Name`.

Without this feature, the index that had to be created would be a fairly complex multiple map-reduce index and this is why the `LoadDocument` function was introduced.

{CODE-TABS}
{CODE-TAB:csharp:AbstractIndexCreationTask indexing_related_documents_2@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB:csharp:Commands indexing_related_documents_3@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TABS/}

Now we will be able to search for invoices using `Category Name` as a parameter:

{CODE indexing_related_documents_7@Indexes\IndexingRelatedDocuments.cs /}

## Example II

Our next scenario will show us that indexing more complex relationships is also trivial. Lets consider a case:

{CODE indexing_related_documents_4@Indexes\IndexingRelatedDocuments.cs /}

Now to create an index with `Author Name` and list of `Book Names` we need to create it as follows:

{CODE-TABS}
{CODE-TAB:csharp:AbstractIndexCreationTask indexing_related_documents_5@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB:csharp:Commands indexing_related_documents_6@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TABS/}

{CODE indexing_related_documents_8@Indexes\IndexingRelatedDocuments.cs /}

## Remarks

{INFO Indexes will be updated automatically when related documents will change. /}

{WARNING Using `LoadDocument` adds a loaded document to tracking list. This may cause very expensive calculations to occur especially when multiple documents are tracking the same document. /}

#### Related articles

TODO