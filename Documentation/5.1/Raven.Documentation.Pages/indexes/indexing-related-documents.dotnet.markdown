# Indexes: Indexing Related Documents
---

{NOTE: }

* To extend indexing capabilities and simplify many scenarios, we have introduced the possibility to 
  index related documents with `LoadDocument`.

* [Include( )](../client-api/session/loading-entities#load-with-includes) 
  is an alternative session CRUD method that can pull data from related documents while reducing expensive trips to the disk.  

* People who are accustomed to relational models but want the agility and efficiency of document-based models 
  should understand that [documents are most effective when they generally stand on their own](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/3-document-modeling) 
  and [relations are harmless exceptions](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/10-static-indexes-and-other-advanced-options#indexing-referenced-data) to the rule.

{INFO: Important} 
Indexes are updated automatically whenever related documents change. 
{INFO/}

{WARNING: Linking Many Documents to a Constantly Changing Document}
`LoadDocument` can be a useful way to enable rapid querying of related documents by having 
an index do the work behind the scenes.  

However, it has a performance cost if you frequently modify documents that are referenced by many other documents. 
Referencing frequently changed documents will repeatedly trigger background indexing on every related document. 
This can tax system resources and cause slow, stale indexing.  
Consider using [Include( )](../client-api/session/loading-entities#load-with-includes) instead.  
{WARNING/}

In this page:

* [Example I](../indexes/indexing-related-documents#example-i)
* [Example II](../indexes/indexing-related-documents#example-ii)

{NOTE/}

{PANEL: Example I}

Let's consider a simple `Product - Category` scenario where you want to look for a `Product` by `Category Name`.

![Product-Category Link in JSON Documents](images/products-categories-link.png "Product-Category Link in JSON Documents")

Without `LoadDocument`, you would have to create a fairly complex Multi-Map-Reduce index.  
This is why the `LoadDocument` function was introduced.

{CODE-TABS}
{CODE-TAB:csharp:Linq-syntax indexing_related_documents_2@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB:csharp:Operation indexing_related_documents_3@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB:csharp:JavaScript indexing_related_documents_2@Indexes\JavaScript.cs /}
{CODE-TAB-BLOCK:csharp:Studio-syntax}
docs.Products.Select(product =>
new{CategoryName = (this.LoadDocument(
    product.Category, "Categories")).Name
    })
{CODE-TAB-BLOCK/}
{CODE-TABS/}

To see how this code works with Northwind sample data, you can [create sample data for a playground server](../studio/database/tasks/create-sample-data) 
or see the [Code Walkthrough](https://demo.ravendb.net/demos/csharp/related-documents/index-related-documents).  
Now we can query the index to search for products using the `CategoryName` as a parameter:

{CODE-TABS}
{CODE-TAB:csharp:Sync-Linq-syntax indexing_related_documents_7@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB:csharp:Async-Linq-syntax indexing_related_documents_AsyncQuery_Products-Beverages@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB-BLOCK:sql:RQL}
from index "ProductCategory"
where CategoryName == "Beverages"
{CODE-TAB-BLOCK/}
{CODE-TABS/}

{PANEL/}

{PANEL: Example II}

Our next scenario will show us how indexing more complex relationships is also straightforward.  
Let's consider the following case where we'll want to query two related documents:

{CODE indexing_related_documents_4@Indexes\IndexingRelatedDocuments.cs /}

To create an index with `Author Name` and list of `Book Names`, we need do the following:

{CODE-TABS}
{CODE-TAB:csharp:Linq-syntax indexing_related_documents_5@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB:csharp:Operation indexing_related_documents_6@Indexes\IndexingRelatedDocuments.cs /}
{CODE-TAB:csharp:JavaScript indexing_related_documents_5@Indexes\JavaScript.cs /}
{CODE-TABS/}

We can now query the index by specifying fields from related documents. 

{CODE indexing_related_documents_8@Indexes\IndexingRelatedDocuments.cs /}

{PANEL/}

## Related Articles

### Indexes

- [Indexing Basics](../indexes/indexing-basics)
- [Indexing Hierarchical Data](../indexes/indexing-hierarchical-data)
- [Indexing Spatial Data](../indexes/indexing-spatial-data)
- [Indexing Polymorphic Data](../indexes/indexing-polymorphic-data)

### Querying

- [Basics](../indexes/querying/basics)

### Session

- [Loading Entities with Include](../client-api/session/loading-entities#load-with-includes) 

---

## Code Walkthrough

- [Index Related Documents](https://demo.ravendb.net/demos/csharp/related-documents/index-related-documents)

---

## Inside RavenDB

- [Document Modeling](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/3-document-modeling)
- [Session CRUD Operations - Include](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/2-zero-to-ravendb#includes)
- [Indexing Referenced Data](https://ravendb.net/learn/inside-ravendb-book/reader/4.0/10-static-indexes-and-other-advanced-options#indexing-referenced-data)

