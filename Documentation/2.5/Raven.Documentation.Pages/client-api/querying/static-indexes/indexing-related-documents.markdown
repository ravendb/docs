# Indexing Related Documents

To extend indexing capabilities and simplify many scenarios, we have introduced the possibility to index related documents. 
For start, let's consider a simple `Customer - Invoice` scenario where you want to lookup for an `Invoice` by `Customer Name`.

{CODE indexing_related_documents_1@ClientApi\Querying\StaticIndexes\IndexingRelatedDocuments.cs /}

Without this feature, the index that had to be created would be a fairly complex multiple map-reduce index and this is why the `LoadDocument` function was introduced.

{CODE indexing_related_documents_2@ClientApi\Querying\StaticIndexes\IndexingRelatedDocuments.cs /}

Alternative way is to use `PutIndex` command:

{CODE indexing_related_documents_3@ClientApi\Querying\StaticIndexes\IndexingRelatedDocuments.cs /}

Now we will be able to search for invoices using `Customer Name` as a parameter.

Our next scenario will show us that indexing more complex relationships is also trivial. Lets consider a case:

{CODE indexing_related_documents_4@ClientApi\Querying\StaticIndexes\IndexingRelatedDocuments.cs /}

Now to create an index with `Author Name` and list of `Book Names` we need to create it as follows:

{CODE indexing_related_documents_5@ClientApi\Querying\StaticIndexes\IndexingRelatedDocuments.cs /}

or

{CODE indexing_related_documents_6@ClientApi\Querying\StaticIndexes\IndexingRelatedDocuments.cs /}

{NOTE Indexes will be updated automatically when related documents will change. /}

{NOTE Using `LoadDocument` adds a loaded document to tracking list. This may cause very expensive calculations to occur especially when multiple documents are tracking the same document. /}